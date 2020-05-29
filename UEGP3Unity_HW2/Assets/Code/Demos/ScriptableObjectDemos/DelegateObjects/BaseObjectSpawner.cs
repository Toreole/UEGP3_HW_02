using UnityEngine;

namespace UEGP3.Demos.ScriptableObjectDemos.DelegateObjects
{
	/// <summary>
	/// A class that handles object spawning.
	/// </summary>
	public abstract class BaseObjectSpawner : ScriptableObject
	{
		/// <summary>
		/// The method used to spawn the given prefab amount times.
		/// The spawning logic changes based on the implementation used.
		/// </summary>
		/// <param name="prefab">The gameobject that is being spawned</param>
		/// <param name="amount">How many of that object should be spawned</param>
		public abstract void Spawn(GameObject prefab, int amount);
	}
}