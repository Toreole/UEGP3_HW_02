﻿using UnityEngine;

namespace UEGP3.InventorySystem
{
	[CreateAssetMenu(fileName = "New Item Bag", menuName = "UEGP3/Items/Heal Item")]
	public class HealItem : Item
	{
		[SerializeField] private float _restoredHealth = 5.0f;
		
		public override void UseItem(AudioSource audio)
		{
			base.UseItem(audio);
			Debug.Log($"Healing by {_restoredHealth}");
		}
	}
}