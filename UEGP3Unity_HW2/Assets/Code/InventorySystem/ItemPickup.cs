using System;
using UEGP3.Core;
using UnityEngine;

namespace UEGP3.InventorySystem
{
	[RequireComponent(typeof(SphereCollider))]
	public class ItemPickup : MonoBehaviour, ICollectible
	{
		[Tooltip("The scriptable object of the item to be picked up.")] [SerializeField]
		private Item _itemToPickup;
		[Tooltip("Range in which the pick up is performed.")] [SerializeField] 
		private float _pickupRange;
		[Tooltip("SFX that is being played when items are being picked up")] [SerializeField] 
		private ScriptableAudioEvent _pickupSFX;

		private AudioSource _audioSource;
		private SphereCollider _pickupCollider;

		private void Awake()
		{
			// Change Mesh Collider based on the set pick up
			GetComponentInChildren<MeshFilter>().mesh = _itemToPickup.ItemMesh;
			
			// Get collider on same object
			_pickupCollider = GetComponent<SphereCollider>();
			_audioSource = FindObjectOfType<AudioSource>();
			
			// Ensure collider values are set accordingly
			_pickupCollider.radius = _pickupRange;
			_pickupCollider.isTrigger = true;
		}

		public void Collect(Inventory inventory)
		{
			// Add item to inventory
			bool wasPickedUp = inventory.TryAddItem(_itemToPickup);

			// Destroy the pickup once the object has been successfully picked up
			if (wasPickedUp)
			{
				// Play the pickup SFX when picking up the item
				_pickupSFX.Play(_audioSource);
				Destroy(gameObject);
			}
		}
		
#if UNITY_EDITOR
		private void OnValidate()
		{
			if (!_pickupCollider)
			{
				_pickupCollider = GetComponent<SphereCollider>();
			}
			
			// Ensure radius is set correctly
			_pickupCollider.radius = _pickupRange;
		}
#endif
	}
}