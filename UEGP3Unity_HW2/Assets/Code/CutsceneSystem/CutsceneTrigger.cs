using UnityEngine;

namespace UEGP3.CutsceneSystem
{
	/// <summary>
	/// Trigger that can be placed in the world to trigger a Cutscene
	/// </summary>
	public class CutsceneTrigger : MonoBehaviour
	{
		[Tooltip("The Cutscene supposed to trigger.")] [SerializeField] 
		private CutsceneController _cutsceneController = null;
		[Tooltip("When set to true, this trigger only works once.")] [SerializeField] 
		private bool _triggerOnlyOnce = true;

		// Did this trigger trigger already?
		private bool _hasTriggered;
		
		private void OnTriggerEnter(Collider other)
		{
			// Only trigger if it wasn't triggered before or should trigger multiple times
			if (_hasTriggered && _triggerOnlyOnce)
			{
				return;
			}
			
			_hasTriggered = true;
			_cutsceneController.TriggerTimeline();
		}
	}
}