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
        [SerializeField]
        ScriptableAudioEvent landSound, jumpSound; //one line for both because theyre connected in a way.

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

        // Called as an animation event
        private void PlayLandSound()
        {
            landSound.Play(_audioSource);
        }

        // Called as an animation event
        void PlayJumpSound()
        {
            jumpSound.Play(_audioSource);
        }
	}
}