using System;
using UEGP3.Core;
using UnityEngine;

namespace UEGP3.PlayerSystem
{
	[RequireComponent(typeof(AudioSource))]
	public class FootStepAudioHandler : MonoBehaviour
	{
		[Tooltip("The audio event that should be played when the animation event is happening")] [SerializeField]
		private ScriptableAudioEvent _footstepAudioEvent;

		private AudioSource _audioSource;

		private void Awake()
		{
			_audioSource = GetComponent<AudioSource>();
		}

		// Called as an animation event
		private void DoFootStepSound()
		{
			_footstepAudioEvent.Play(_audioSource);
		}
	}
}