using UnityEngine;

namespace UEGP3.InventorySystem
{
	[CreateAssetMenu(fileName = "New Item Bag", menuName = "UEGP3/Items/Item")]
	public class NormalItem : Item
	{
		public override void UseItem()
		{
			base.UseItem();
			Debug.Log($"Using {_itemName} item.");
		}
	}
}