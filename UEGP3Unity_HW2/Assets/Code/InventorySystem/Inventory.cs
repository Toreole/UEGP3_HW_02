using System.Collections.Generic;
using UEGP3.InventorySystem.UI;
using UnityEngine;

namespace UEGP3.InventorySystem
{
	[CreateAssetMenu(menuName = "UEGP3/Inventory System/New Inventory", fileName = "New Inventory")]
	public class Inventory : ScriptableObject
	{
		[Tooltip("The bags contained in this inventory")] [SerializeField]
		private List<ItemBag> _bags = default;
		public List<ItemBag> Bags => _bags;
		
		private InventoryUI _inventoryUI = default;
		private bool _isOpen;
		private Item _quickAccesItem;

		/// <summary>
		/// Creates the runtime inventory with all its required UI.
		/// </summary>
		public void Create()
		{
			// search for the inventory UI, disable it at first
			_inventoryUI = FindObjectOfType<InventoryUI>();
			_inventoryUI.SetInventory(this);
			_inventoryUI.Hide();
			_isOpen = false;

			// Add all relevant events
			foreach (ItemBag itemBag in _bags)
			{
				itemBag.SubscribeEvents();
			}
			
			ItemBag.OnItemRemoved += OnItemRemoved;
		}

		/// <summary>
		/// Destroy the runtime inventory
		/// </summary>
		public void Destroy()
		{
			foreach (ItemBag itemBag in _bags)
			{
				itemBag.UnsubscribeEvents();
			}
			
			ItemBag.OnItemRemoved -= OnItemRemoved;
		}

		/// <summary>
		/// Prints the inventory to the console and opens the UI.
		/// </summary>
		public void ToggleInventory()
		{
			_isOpen = !_isOpen;
			if (_isOpen)
			{
				// Open the inventory, stop the time and unlock the cursor.
				Time.timeScale = 0f;
				Cursor.lockState = CursorLockMode.Confined;
				Cursor.visible = true;
				_inventoryUI.Show();
			}
			else
			{
				// Close the inventory, proceed the time and lock the cursor.
				Time.timeScale = 1f;
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
				_inventoryUI.Hide();
			}
			
			Debug.Log(this);
		}
		
		/// <summary>
		/// Tries to add a given item to the inventory
		/// </summary>
		/// <param name="item">The item to be added</param>
		/// <returns>A bool whether the adding process succeeded</returns>
		public bool TryAddItem(Item item)
		{
			bool success = false;
			foreach (ItemBag itemBag in _bags)
			{
				if (itemBag.SupportedItemTypes.Contains(item.ItemType))
				{
					success = itemBag.TryAddItem(item);
				}
			}

			// if item was added successfully and quick access is empty, add it to the quick access.
			if (success && (_quickAccesItem == null))
			{
				AddToQuickAccess(item);
			}

			return success;
		}
		
		public void UseQuickAccessItem()
		{
			// only execute if quick access holds an item
			if (_quickAccesItem == null)
			{
				return;
			}
			
			// Find the item in its bag
			ItemBag bagThatContainsItem = FindBagWithItem(_quickAccesItem);
			if (bagThatContainsItem == null)
			{
				return;
			}
			
			// Use item
			bagThatContainsItem.UseItem(_quickAccesItem);
		}

		private ItemBag FindBagWithItem(Item itemToSearchFor)
		{
			ItemBag bagThatContainsItem = null;
			// Loop over all bags and see if they contain the item we are looking for.
			// Return the first that has it.
			foreach (ItemBag itemBag in _bags)
			{
				if (itemBag.HasItem(itemToSearchFor))
				{
					bagThatContainsItem = itemBag;
					break;
				}
			}

			return bagThatContainsItem;
		}

		/// <summary>
		/// Add the given item to the quick access.
		/// </summary>
		/// <param name="item"></param>
		private void AddToQuickAccess(Item item)
		{
			_quickAccesItem = item;
		}

		/// <summary>
		/// Removes the current item from the quick access.
		/// </summary>
		private void RemoveFromQuickAccess()
		{
			_quickAccesItem = null;
		}

		private void OnItemRemoved(Item item, ItemBag bag)
		{
			// if an item was removed and its not null, the bag is not null and the bag is contained in this inventory, proceed
			if ((item == null) || (bag == null) || !_bags.Contains(bag))
			{
				return;
			}
			
			// if the item in the quick access is the removed one, remove it from the quick access
			if (_quickAccesItem.Equals(item))
			{
				RemoveFromQuickAccess();
			}
		}

		public override string ToString()
		{
			// "String-Interpolation": $ before a string "" allows us to use variables in {} 
			// inventory = "Inventory " + name + " contains:\r\n" is the same as the line below, but nicer! :) 
			string inventory = $"Inventory {name} contains:\r\n";

			foreach (ItemBag itemBag in _bags)
			{
				inventory += $"{itemBag.ToString()}\r\n";
			}
			
			return inventory;
		}
	}
}