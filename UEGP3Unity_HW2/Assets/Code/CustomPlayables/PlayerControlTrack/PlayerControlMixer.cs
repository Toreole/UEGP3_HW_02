using UnityEngine.Playables;
using UEGP3.PlayerSystem;

namespace UEGP3.CustomPlayables
{
    public class PlayerControlMixer : PlayableBehaviour
    {
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            PlayerController target = playerData as PlayerController;
            bool allowInput = false;
            if (!target)
                return;

            int clipCount = playable.GetInputCount();

            for(int i = 0; i < clipCount; i++)
            {
                float weight = playable.GetInputWeight(i);

                if (weight > 0.6f)
                {
                    var inputPlayable = (ScriptPlayable<PlayerControlBehaviour>)playable.GetInput(i);
                    var input = inputPlayable.GetBehaviour();

                    allowInput = input.allowInput;
                }
            }
            target.ReceiveInput = allowInput;
        }
    }
}
