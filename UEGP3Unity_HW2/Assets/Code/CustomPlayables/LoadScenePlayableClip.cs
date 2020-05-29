using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace UEGP3.CustomPlayables
{
	[Serializable]
	public class LoadScenePlayableClip : PlayableAsset, ITimelineClipAsset
	{
		public LoadScenePlayableBehaviour template = new LoadScenePlayableBehaviour();
		
		public ClipCaps clipCaps => ClipCaps.None;

		public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
		{
			var playable = ScriptPlayable<LoadScenePlayableBehaviour>.Create(graph, template);
			return playable;
		}
	}
}