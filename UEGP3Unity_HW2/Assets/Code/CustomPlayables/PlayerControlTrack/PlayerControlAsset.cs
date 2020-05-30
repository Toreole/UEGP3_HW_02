using UnityEngine.Playables;
using UEGP3.PlayerSystem;
using UnityEngine;

namespace UEGP3.CustomPlayables
{
    public class PlayerControlAsset : PlayableAsset
    {
        public bool allowInput;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<PlayerControlBehaviour>.Create(graph);
            playable.GetBehaviour().allowInput = this.allowInput;
            return playable;
        }
    }
}
