using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace UEGP3.CustomPlayables
{
	public class LoadScenePlayableMixerBehaviour : PlayableBehaviour
	{
		public override void ProcessFrame(Playable playable, FrameData info, object playerData)
		{
			// How many clips are on the current track?
			int inputCount = playable.GetInputCount();

			for (int i = 0; i < inputCount; i++)
			{
				// Is the current clip active?
				float inputWeight = playable.GetInputWeight(i);
				// roughly said: This "Scriptable Object" of our Template Behaviour
				ScriptPlayable<LoadScenePlayableBehaviour> inputPlayable = (ScriptPlayable<LoadScenePlayableBehaviour>) playable.GetInput(i);
				// Because we dont need the "Scriptable Object" but the actual object that we defined
				LoadScenePlayableBehaviour input = inputPlayable.GetBehaviour();

				// If the current clip is being played, execute logic
				if (inputWeight > 0)
				{
					SceneManager.LoadScene(input.SceneToLoad);
				}
			}
		}
	}
}