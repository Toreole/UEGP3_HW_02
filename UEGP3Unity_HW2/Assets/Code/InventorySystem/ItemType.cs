using UEGP3.Core;
using UnityEngine;

namespace UEGP3.InventorySystem
{
	[CreateAssetMenu(fileName = "New Item Type", menuName = "UEGP3/Items/ItemType", order = -50)]
	public class ItemType : ScriptableObject
	{
        [SerializeField]
        ScriptableAudioEvent fallbackUseSound;

        public ScriptableAudioEvent FallbackUseSound => fallbackUseSound;
	}
}