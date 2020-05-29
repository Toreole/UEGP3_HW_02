using UnityEngine;

namespace UEGP3.Core
{
	/// <summary>
	/// Scriptable Object that grouping multiple audio clips into one sfx which is playable at run and edit-time.
	/// </summary>
	[CreateAssetMenu(fileName = "New Audio Event", menuName = "UEGP3/Audio Event", order = -50)]
	public class ScriptableAudioEvent : ScriptableObject
	{
		[Header("Clips")]
		[Tooltip("The audio clips used for the sfx")]
		[SerializeField] 
		private AudioClip[] _clips;
		
		[Header("Volume")]
		[Tooltip("Toggles whether the sfx is played at a fixed or a random range volume")]
		[SerializeField] 
		private bool _randomizeVolume;
		[Tooltip("The fixed volume at which the sfx will be played")] [SerializeField] 
		private float _volume;
		[Tooltip("Minimum volume when using a random value")] [SerializeField] 
		private float _minVolume;
		[Tooltip("Maximum volume when using a random value")] [SerializeField] 
		private float _maxVolume;
		
		[Header("Pitch")]
		[Tooltip("Toggles whether the sfx is played at a fixed or a random range pitch")] 
		[SerializeField] 
		private bool _randomizePitch;
		[Tooltip("The fixed pitch at which the sfx will be played")] [SerializeField] 
		private float _pitch;
		[Tooltip("Minimum pitch when using a random value")] [SerializeField] 
		private float _minPitch;
		[Tooltip("Maximum pitch when using a random value")] [SerializeField] 
		private float _maxPitch;
		
		public bool RandomizeVolume => _randomizeVolume;
		public bool RandomizePitch => _randomizePitch;
		
		/// <summary>
		/// Plays SFX on the given audio source.
		/// </summary>
		/// <param name="source">The audio source to play the event on</param>
		public void Play(AudioSource source)
		{
			if ((_clips.Length == 0) || (source == null))
			{
				return;
			}

			// pick a random audio clip from the array
			source.clip = _clips[Random.Range(0, _clips.Length)];
			// Use either fixed or randomized volume
			source.volume = _randomizeVolume ? Random.Range(_minVolume, _maxVolume) : _volume;
			// Use either fixed or randomized pitch
			source.pitch = _randomizePitch ? Random.Range(_minPitch, _maxPitch) : _pitch;
			
			source.Play();
		}
	}
}