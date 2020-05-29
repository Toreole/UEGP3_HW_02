using UnityEngine.Playables;

namespace UEGP3.ExtensionMethods
{
	/// <summary>
	/// Class gathering various extension methods for Timelines
	/// </summary>
	public static class PlayableDirectorExtensions
	{
		/// <summary>
		/// Resumes the TimelineInstance referenced in the playable director.
		/// </summary>
		/// <param name="director">The director to be resumed</param>
		/// <param name="speed">Playback speed for the director</param>
		public static void ResumeDirector(this PlayableDirector director, float speed)
		{
			director.playableGraph.GetRootPlayable(0).SetSpeed(speed);
		}

		/// <summary>
		/// Pauses the TimelineInstance referenced in the playable director.
		/// </summary>
		/// <param name="director">The director to be resumed</param>
		/// <returns>Current playback speed for the director</returns>
		public static float PauseDirector(this PlayableDirector director)
		{
			float currentSpeed = (float) director.playableGraph.GetRootPlayable(0).GetSpeed();
			director.playableGraph.GetRootPlayable(0).SetSpeed(0);
			return currentSpeed;
		}
	}
}