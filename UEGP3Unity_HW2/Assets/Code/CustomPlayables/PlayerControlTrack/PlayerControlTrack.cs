using UnityEngine.Playables;
using UnityEngine.Timeline;
using UEGP3.PlayerSystem;
using UnityEngine;

namespace UEGP3.CustomPlayables
{
    [TrackClipType(typeof(PlayerControlAsset))]
    [TrackBindingType(typeof(PlayerController))]
    public class PlayerControlTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<PlayerControlMixer>.Create(graph, inputCount);
        }
    }
}
