using UEGP3.InventorySystem;

namespace UEGP3.Core
{
	/// <summary>
	/// Interface for collectible objects
	/// </summary>
	public interface ICollectible
	{
		/// <summary>
		/// Collects the collectible.
		/// </summary>
		/// <param name="inventory">The inventory to which the collectible item is added.</param>
		void Collect(Inventory inventory);
	}
}