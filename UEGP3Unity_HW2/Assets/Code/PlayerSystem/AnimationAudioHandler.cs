using System;
using UEGP3.Core;
using UnityEngine;

namespace UEGP3.PlayerSystem
{
	[RequireComponent(typeof(AudioSource))]
	public class AnimationAudioHandler : MonoBehaviour
	{
		private AudioSource _audioSource;

		private void Awake()
		{
			_audioSource = GetComponent<AudioSource>();
		}

        //animation event.
        void PlaySound(ScriptableAudioEvent sound)
        {
            sound.Play(_audioSource);
        }
	}
}