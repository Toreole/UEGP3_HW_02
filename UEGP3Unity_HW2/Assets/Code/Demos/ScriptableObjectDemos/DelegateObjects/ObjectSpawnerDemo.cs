using UnityEngine;

namespace UEGP3.Demos.ScriptableObjectDemos.DelegateObjects
{
	/// <summary>
	/// Demonstrates the usage of BaseObjectSpawner.
	/// </summary>
	public class ObjectSpawnerDemo : MonoBehaviour
	{
		[Tooltip("The strategy used to spawn objects")]
		[SerializeField] 
		private BaseObjectSpawner _spawnerStrategy;
		[Tooltip("Amount of objects being spawned")]
		[SerializeField] 
		private int _amount;
		[Tooltip("The Prefab that is used for spawning")]
		[SerializeField] 
		private GameObject _prefab;

		private void Awake()
		{
			// Call to BaseObjectSpawner.Spawn in Awake()
			_spawnerStrategy.Spawn(_prefab, _amount);
		}
	}
}