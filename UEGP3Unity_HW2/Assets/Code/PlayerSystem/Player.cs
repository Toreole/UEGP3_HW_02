using UEGP3.Core;
using UEGP3.InventorySystem;
using UnityEngine;

namespace UEGP3.PlayerSystem
{
	/// <summary>
	/// Player containing additional logic like item pick ups etc.
	/// </summary>
	public class Player : MonoBehaviour
	{
		[Tooltip("Inventory to be used for the player")] [SerializeField] 
		private Inventory _playerInventory;

		private void Awake()
		{
			// Create inventory
			_playerInventory.Create();
		}

		private void OnDestroy()
		{
			// Destroy ivnentory
			_playerInventory.Destroy();
		}

		private void Update()
		{
			// Show Inventory if button is pressed
			if (Input.GetButtonDown("Inventory"))
			{
				_playerInventory.ToggleInventory();
			}

			if (Input.GetButtonDown("ItemQuickAccess"))
			{
				_playerInventory.UseQuickAccessItem();
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			// If we collide with a collectible item, collect it
			ICollectible collectible = other.gameObject.GetComponent<ICollectible>();
			if (collectible != null)
			{
				Collect(collectible);
			}
		}

		private void Collect(ICollectible collectible)
		{
			collectible.Collect(_playerInventory);
		}
	}
}