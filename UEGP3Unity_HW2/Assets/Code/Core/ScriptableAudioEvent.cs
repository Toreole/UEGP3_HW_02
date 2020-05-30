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
        [SerializeField, MinMax(0, 1.5f)]
        private MinMaxFloat minMaxVolume = default;


        [Header("Pitch")]
		[Tooltip("Toggles whether the sfx is played at a fixed or a random range pitch")] 
		[SerializeField] 
		private bool _randomizePitch;
		[Tooltip("The fixed pitch at which the sfx will be played")] [SerializeField] 
		private float _pitch;
        [SerializeField, MinMax(0, 1.5f)]
        private MinMaxFloat minMaxPitch = default;

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
			source.volume = _randomizeVolume ? Random.Range(minMaxVolume.min, minMaxVolume.max) : _volume;
			// Use either fixed or randomized pitch
			source.pitch = _randomizePitch ? Random.Range(minMaxPitch.min, minMaxPitch.max) : _pitch;
			
			source.Play();
		}
	}
}