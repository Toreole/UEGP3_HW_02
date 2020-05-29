using System.Collections.Generic;
using UnityEngine;

namespace UEGP3.InventorySystem.UI
{
	/// <summary>
	/// Class resembling the UI representation of an ItemBag
	/// </summary>
	[RequireComponent(typeof(CanvasGroup))]
	public class InventoryBagUI : MonoBehaviour
	{
		[Tooltip("The prefab object used for inventory slots")] [SerializeField]
		private InventorySlotUI _inventorySlotUIPrefab = default;
		[Tooltip("The transform holding all inventory slots")] [SerializeField]
		private Transform _slotHolder = default;

		// Store all inventorySlots in a list for access
		private List<InventorySlotUI> _inventorySlots = new List<InventorySlotUI>();
		// the bag associated with the ui
		private ItemBag _itemBag;
		// canvas group used to enable/disable the bag
		private CanvasGroup _canvasGroup;

		private void Awake()
		{
			_canvasGroup = GetComponent<CanvasGroup>();

			// listen to item added/removed events
			ItemBag.OnItemAdded += OnItemAdded;
			ItemBag.OnItemRemoved += OnItemRemoved;
		}

		private void OnDestroy()
		{
			// remove events
			ItemBag.OnItemAdded -= OnItemAdded;
			ItemBag.OnItemRemoved -= OnItemRemoved;
		}

		/// <summary>
		/// Sets up the UI with the given bag.
		/// </summary>
		/// <param name="bag">The bag to be displayed</param>
		public void SetupForBag(ItemBag bag)
		{
			_itemBag = bag;

			InstantiateSlots();
			Hide();
		}

		/// <summary>
		/// Shows this bag.
		/// </summary>
		public void Show()
		{
			_canvasGroup.alpha = 1.0f;
			_canvasGroup.interactable = true;
			_canvasGroup.blocksRaycasts = true;
		}

		/// <summary>
		/// Hides this bag.
		/// </summary>
		public void Hide()
		{
			_canvasGroup.alpha = 0f;
			_canvasGroup.interactable = false;
			_canvasGroup.blocksRaycasts = false;
		}

		private void InstantiateSlots()
		{
			// Create one empty slot until we hit the bags capacity
			for (int i = 0; i < _itemBag.Capacity; i++)
			{
				InventorySlotUI uiSlot = Instantiate(_inventorySlotUIPrefab, _slotHolder);
				uiSlot.ShowEmptyItem();
				_inventorySlots.Add(uiSlot);
			}
		}
		
		private void OnItemRemoved(Item item, ItemBag bag)
		{
			// item can only be removed if it is contained in this bag and its not null
			if ((item == null) || (bag != _itemBag))
			{
				return;
			}
			
			// Search for the inventory slot with the given item
			InventorySlotUI uiSlot = FindInventorySlotUIForItem(item);
			if (uiSlot != null)
			{
				// remove it from that slot
				uiSlot.RemoveItem(item);
			}
		}

		private void OnItemAdded(Item item, ItemBag bag)
		{
			// item can only be added if it is contained in this bag and its not null
			if ((item == null) || (bag != _itemBag))
			{
				return;
			}

			// Search for the inventory slot with the given item
			InventorySlotUI uiSlot = FindInventorySlotUIForItem(item);
			if (uiSlot != null)
			{
				// add it to that slot
				uiSlot.AddItem(item);
			}
			else
			{
				// if it is in no slot, find the first empty one and add it there
				uiSlot = FindFirstEmptyInventorySlot();
				uiSlot.AddItem(item);
			}
		}

		/// <summary>
		/// Searches for the first inventory slot holding the given item. 
		/// </summary>
		/// <param name="item">The item to search for</param>
		/// <returns>The slot holding the item</returns>
		private InventorySlotUI FindInventorySlotUIForItem(Item item)
		{
			foreach (InventorySlotUI inventorySlotUI in _inventorySlots)
			{
				if (inventorySlotUI.HasItem(item))
				{
					return inventorySlotUI;
				}
			}

			return null;
		}

		/// <summary>
		/// Searches for the first empty slot.
		/// </summary>
		/// <returns>The first empty slot.</returns>
		private InventorySlotUI FindFirstEmptyInventorySlot()
		{
			foreach (InventorySlotUI inventorySlotUI in _inventorySlots)
			{
				if (inventorySlotUI.IsEmpty())
				{
					return inventorySlotUI;
				}
			}

			return null;
		}
	}
}