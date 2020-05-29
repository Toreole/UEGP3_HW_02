using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UEGP3.InventorySystem.UI
{
	/// <summary>
	/// Class resembling the UI for an inventory slot. Implements IPointerEnterHandler and ISelectHandler so it can handle
	/// UI events properly.
	/// </summary>
	public class InventorySlotUI : MonoBehaviour, IPointerEnterHandler, ISelectHandler
	{
		// Event to be fired when the slot gets hovered
		public delegate void HoverAction(Item slot);
		/// <summary>
		/// Gets invoked when a slot is being hovered. Passes the currently stored item.
		/// </summary>
		public static event HoverAction OnSlotHovered;
		
		[Tooltip("Reference to the image displaying the item")] [SerializeField]
		private Image _itemImage = default;
		[Tooltip("Displays the items name")] [SerializeField]
		private TextMeshProUGUI _itemName = default;
		[Tooltip("Displays how many of the given item are in this slot")] [SerializeField]
		private TextMeshProUGUI _itemCount = default;

		private Button _button;
		private Item _item;
		private int _count;

		private void Awake()
		{
			// Add onclick event to use the stored item
			_button = GetComponentInChildren<Button>();
			_button.onClick.AddListener(UseItem);
			
			// always reset current item count on awake
			_count = 0;
			
			// Listen to ItemUsed event, so we know when this item is being used
			Item.OnItemUsed += OnItemUsed;
		}

		private void OnDestroy()
		{
			// Remove event again
			Item.OnItemUsed -= OnItemUsed;
		}

		/// <summary>
		/// Displays an empty item in this slot
		/// </summary>
		public void ShowEmptyItem()
		{
			_item = null;
			ToggleUIElements(false);
		}

		/// <summary>
		/// Does the slot currently hold the given item?
		/// </summary>
		/// <param name="item">The item to check against</param>
		/// <returns>True if it holds the item</returns>
		public bool HasItem(Item item)
		{
			return (_item != null) && _item.Equals(item);
		}

		/// <summary>
		/// Is the slot currently empty?
		/// </summary>
		/// <returns>True if empty</returns>
		public bool IsEmpty()
		{
			return _item == null;
		}

		/// <summary>
		/// Removes the current item from the slot, if the item passed is the right one.
		/// </summary>
		/// <param name="item">The item to remove</param>
		public void RemoveItem(Item item)
		{
			if (item == null)
			{
				return;
			}
			
			ShowEmptyItem();
		}

		/// <summary>
		/// Adds the given item to the slot, if it is not null.
		/// </summary>
		/// <param name="item">The item to add</param>
		public void AddItem(Item item)
		{
			if (item == null)
			{
				return;
			}
			
			_item = item;
			_count++;
			UpdateUI();
		}

		private void ToggleUIElements(bool show)
		{
			// either show or hide all item related UI elements
			_itemImage.gameObject.SetActive(show);
			_itemName.gameObject.SetActive(show);
			_itemCount.gameObject.SetActive(show);
		}

		private void UpdateUI()
		{
			// update ui values respecting the current item
			_itemName.text = _item.ItemName;
			_itemCount.text = _count.ToString();
			_itemImage.sprite = _item.ItemSprite;
			
			ToggleUIElements(true);
		}

		private void OnItemUsed(Item item)
		{
			// if the item is null or not the one in the slot, don't do anything
			if ((item == null) || (_item != item))
			{
				return;
			}

			// decrease item count, update UI to resemble the used instance.
			if (item.ConsumeUponUse)
			{
				_count--;
				UpdateUI();
			}
		}

		private void UseItem()
		{
			// only use the item if not null
			if (_item != null)
				_item.UseItem();
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			// Forward OnPointerEnter event to whoever listens
			OnSlotHovered?.Invoke(_item);
		}

		public void OnSelect(BaseEventData eventData)
		{
			// Forward OnSelect event to whoever listens
			OnSlotHovered?.Invoke(_item);
		}
	}
}