using System;
using UEGP3.ExtensionMethods;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

namespace UEGP3.CutsceneSystem
{
	public class CutsceneController : MonoBehaviour
	{
		[Tooltip("The playable director used to play the TimelineAsset")] [SerializeField]
		private PlayableDirector _playableDirector = default;
		[Tooltip("The TimelineAsset that will be played by this CutsceneController")] [SerializeField]
		private TimelineAsset _timelineAsset = default;
		[Tooltip("A Pause UI shown to the user when the Timeline Instance is being paused")] [SerializeField]
		private CanvasGroup _pauseScreenCanvasGroup = default;

		// is this controller currently active?
		private bool _isActive;
		// store paused state
		private bool _isPaused;
		// playback speed is required for pause/resume
		private float _defaultPlaybackSpeed;

		private void Awake()
		{
			// If the director plays on awake, its immediately active
			if (_playableDirector.playOnAwake)
			{
				_isActive = true;
				_playableDirector.stopped += (pd) => _isActive = false;
			}
		}

		private void Update()
		{
			// Don't execute update if we are not active
			if (!_isActive)
			{
				return;
			}
			
			// Check for Pause/Resume Input
			if (Input.GetButtonDown("Pause"))
			{
				if (_isPaused)
				{
					ResumeTimeline();
				}
				else
				{
					PauseTimeline();
				}
			}

			// Check for Skip Input
			if (Input.GetButtonDown("Skip"))
			{
				Skip();
			}
		}

		/// <summary>
		/// Starts the referenced TimelineAsset in the referenced TimelineInstance. 
		/// </summary>
		public void TriggerTimeline()
		{
			_isActive = true;
			_playableDirector.playableAsset = _timelineAsset;
			_playableDirector.Play();
			_playableDirector.stopped += (pd) => _isActive = false;
		}

		/// <summary>
		/// Pauses the currently running TimelineAsset, sets the TimeScale to 0 and shows a pause UI
		/// </summary>
		public void PauseTimeline()
		{
			_isPaused = true;
			Time.timeScale = 0f;
			_defaultPlaybackSpeed = _playableDirector.PauseDirector();
			_pauseScreenCanvasGroup.alpha = 1f;
			_pauseScreenCanvasGroup.interactable = true;
			_pauseScreenCanvasGroup.blocksRaycasts = true;
		}

		/// <summary>
		/// Resumes the paused TimelineAsset, sets the TimeScale to 1 and hides the pause UI
		/// </summary>
		public void ResumeTimeline()
		{
			_isPaused = false;
			_playableDirector.ResumeDirector(_defaultPlaybackSpeed);
			_pauseScreenCanvasGroup.alpha = 0f;
			_pauseScreenCanvasGroup.interactable = false;
			_pauseScreenCanvasGroup.blocksRaycasts = false;
			Time.timeScale = 1f;
		}

		/// <summary>
		/// Skips & Stops the currently running TimelineAsset and executes clean up to correctly set the post playback state.
		/// </summary>
		public void Skip()
		{
			_playableDirector.time = _playableDirector.duration;
			_playableDirector.Stop();
		}
	}
}