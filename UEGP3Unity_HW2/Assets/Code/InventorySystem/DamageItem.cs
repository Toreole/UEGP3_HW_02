﻿using UnityEngine;

namespace UEGP3.InventorySystem
{
	[CreateAssetMenu(fileName = "New Item Bag", menuName = "UEGP3/Items/Damage Item")]
	public class DamageItem : Item
	{
		[SerializeField] private float _damage = 5.0f;
		
		public override void UseItem(AudioSource audio)
		{
			base.UseItem(audio);
			Debug.Log($"Inflict {_damage} damage!");
		}
	}
}