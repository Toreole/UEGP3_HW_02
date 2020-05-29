using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UEGP3.InventorySystem.UI
{
	/// <summary>
	/// UI class resembling the inventory
	/// </summary>
	public class InventoryUI : MonoBehaviour
	{
		[Tooltip("The description of the currently selected item")] [SerializeField]
		private TextMeshProUGUI _itemDescription = default;
		[Tooltip("Prefab object used for UI bags")] [SerializeField]
		private InventoryBagUI _bagUIPrefab = default;
		[Tooltip("Parent transform that holds all the UI bags")] [SerializeField]
		private RectTransform _bagHolder = default;
		[Tooltip("Prefab object used for bag buttons")] [SerializeField]
		private Button _bagButtonPrefab;
		[Tooltip("Parent transform that holds all the bag buttons")] [SerializeField]
		private RectTransform _bagButtonHolder = default;

		// Reference to all used ui bags
		private List<InventoryBagUI> _inventoryBags = new List<InventoryBagUI>();
		// Store which bag was last opened, so we can re-open it
		private int _lastOpenBagIndex;
		// the displayed inventory
		private Inventory _inventory;

		private void Awake()
		{
			// React to slot hovered event
			InventorySlotUI.OnSlotHovered += OnInventorySlotSelected;
		}

		private void OnDestroy()
		{
			// Remove event again
			InventorySlotUI.OnSlotHovered -= OnInventorySlotSelected;
		}

		/// <summary>
		/// Sets the displayed inventory and instantiates all UI.
		/// </summary>
		/// <param name="inventory">The inventory to be displayed</param>
		public void SetInventory(Inventory inventory)
		{
			_inventory = inventory;
			SpawnInventoryUI();
		}

		/// <summary>
		/// Renders the Inventory to the screen, showing the last opened bag with no Item selected.
		/// </summary>
		public void Show()
		{
			gameObject.SetActive(true);
			// No item is selected, so no description
			UpdateItemDescription("");
			// Only show the bag that was opened the last time inventory was shown (0 if its the first time)
			for (int i = 0; i < _inventoryBags.Count; i++)
			{
				if (i == _lastOpenBagIndex)
				{
					_inventoryBags[i].Show();
				}
				else
				{
					_inventoryBags[i].Hide();
				}
			}
		}

		/// <summary>
		/// Closes the inventory.
		/// </summary>
		public void Hide()
		{
			gameObject.SetActive(false);
		}
		
		private void SpawnInventoryUI()
		{
			// Instantiate bags and their respective buttons according to inventory SO
			foreach (ItemBag itemBag in _inventory.Bags)
			{
				InventoryBagUI bagUI = InstantiateBagUI(itemBag);
				InstantiateBagButton(bagUI);
			}
		}

		private InventoryBagUI InstantiateBagUI(ItemBag itemBag)
		{
			// Spawn the InventoryBagUI resembling the given bag, set its name to the name of the bag so we can identify it
			InventoryBagUI bagUI = Instantiate(_bagUIPrefab, _bagHolder);
			bagUI.name = itemBag.BagName;
			
			// Add bag to list of all bags
			_inventoryBags.Add(bagUI);
			
			// configure bag initially
			bagUI.SetupForBag(itemBag);
			
			return bagUI;
		}

		private void InstantiateBagButton(InventoryBagUI bagUI)
		{
			// Spawn a button opening the given bagUI, name it like the bag and change its text so we can identify it
			Button bagButton = Instantiate(_bagButtonPrefab, _bagButtonHolder);
			bagButton.name = $"{bagUI.name} Button";
			bagButton.GetComponentInChildren<TextMeshProUGUI>().text = bagUI.name;
			
			// If the bag button gets clicked, show the respective bag, hide all others and update _lastOpenBagIndex
			bagButton.onClick.AddListener(() =>
			                              {
				                              foreach (InventoryBagUI inventoryBagUI in _inventoryBags)
				                              {
					                              if (inventoryBagUI.Equals(bagUI))
					                              {
						                              inventoryBagUI.Show();
						                              _lastOpenBagIndex = _inventoryBags.IndexOf(inventoryBagUI);
					                              }
					                              else
					                              {
						                              inventoryBagUI.Hide();
					                              }
				                              }
			                              });
		}

		private void UpdateItemDescription(string text)
		{
			_itemDescription.text = text;
		}

		private void OnInventorySlotSelected(Item item)
		{
			// If no item is selected (empty slot hovered), description is empty, otherwise its the items desc
			string itemItemDescription = item != null ? item.ItemDescription : "";
			UpdateItemDescription(itemItemDescription);
		}
	}
}