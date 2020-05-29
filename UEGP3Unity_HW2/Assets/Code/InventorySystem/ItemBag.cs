using System;
using System.Collections.Generic;
using UnityEngine;

namespace UEGP3.InventorySystem
{
	/// <summary>
	/// Logical representation of an item bag. Can multiple item types, up to a given capacity.
	/// </summary>
	[CreateAssetMenu(fileName = "New Item Bag", menuName = "UEGP3/Inventory System/Bag")]
	public class ItemBag : ScriptableObject
	{
		public delegate void ItemAddedAction(Item item, ItemBag bag);
		/// <summary>
		/// Event fired when an Item gets added to a bag.
		/// </summary>
		public static event ItemAddedAction OnItemAdded;
		public delegate void ItemRemovedAction(Item item, ItemBag bag);
		/// <summary>
		/// Event fired when an Item gets removed from a bag. 
		/// </summary>
		public static event ItemRemovedAction OnItemRemoved;
		
		[Tooltip("The name of the bag")] [SerializeField]
		private string _bagName = "Potion Bag";
		[Tooltip("The capacity of the bag")] [SerializeField]
		private float _capacity = 5f;

		[Tooltip("The item types this bag can hold")] [SerializeField]
		private List<ItemType> _supportedItemTypes = new List<ItemType>();
		
		public float Capacity => _capacity;
		public string BagName => _bagName;
		public List<ItemType> SupportedItemTypes => _supportedItemTypes;
		
		// Dictionary used to store items in this bag
		private Dictionary<Item, int> _inventoryItems = new Dictionary<Item, int>();

		/// <summary>
		/// Subscribes relevant events for the item bag
		/// </summary>
		public void SubscribeEvents()
		{
			Item.OnItemUsed += OnItemUsed;
		}

		/// <summary>
		/// Unsubscribes all events for the item bag
		/// </summary>
		public void UnsubscribeEvents()
		{
			Item.OnItemUsed -= OnItemUsed;
		}

		/// <summary>
		/// Tries to add the given item to the bag. Succeeds if the bag can hold this item type and is not full yet or can still stack this item.
		/// </summary>
		/// <param name="item">The item to add</param>
		/// <returns>True if it successfully added the item</returns>
		public bool TryAddItem(Item item)
		{
			bool success = false;
			// Item is not yet in inventory, add it
			if (!_inventoryItems.ContainsKey(item))
			{
				// only add items if inventory is not full
				if (_inventoryItems.Count >= _capacity)
				{
					return false;
				}

				_inventoryItems.Add(item, 1);
				success = true;
			}
			// Item is already in inventory, stack it up if possible
			else
			{
				// Only items that are not unique can be stacked
				if (!item.IsUnique)
				{
					_inventoryItems[item]++;
					success = true;
				}
			}

			if (success)
			{
				// we added something so broadcast it to whoever listens.
				OnItemAdded?.Invoke(item, this);
			}
			
			return success;
		}

		/// <summary>
		/// Does this bag contain the given item?
		/// </summary>
		/// <param name="item">The item to check for</param>
		/// <returns>True if it contains the item</returns>
		public bool HasItem(Item item)
		{
			return _inventoryItems.ContainsKey(item) && (_inventoryItems[item] > 0);
		}

		/// <summary>
		/// Uses the item if its available.
		/// </summary>
		/// <param name="item">The item to use</param>
		public void UseItem(Item item)
		{
			// Item can only be used if it is in the inventory
			if (!_inventoryItems.ContainsKey(item))
			{
				return;
			}
			
			// Use the item
			item.UseItem();
		}

		/// <summary>
		/// Removes the given item from the inventory
		/// </summary>
		/// <param name="item">The item to be removed</param>
		private void RemoveItem(Item item)
		{
			OnItemRemoved?.Invoke(item, this);
			_inventoryItems.Remove(item);
		}

		public override string ToString()
		{
			string bagContent = "";
			
			foreach (KeyValuePair<Item, int> inventoryItem in _inventoryItems)
			{
				bagContent += $"[{BagName}] :\r\n\r\n";
				bagContent += $"\t[{inventoryItem.Key.ItemName} - {inventoryItem.Value}]\r\n";
			}

			return bagContent;
		}

		private void OnItemUsed(Item item)
		{
			// Can't do anything if the item is null or not present in the dictionary
			if ((item == null) || !_inventoryItems.ContainsKey(item))
			{
				return;
			}
			
			// if consumed upon use, decrease count
			if (item.ConsumeUponUse)
			{
				_inventoryItems[item]--;
			}

			// if no longer in inventory, because count == 0, remove it
			if (_inventoryItems[item] == 0)
			{
				RemoveItem(item);
			}
		}
	}
}