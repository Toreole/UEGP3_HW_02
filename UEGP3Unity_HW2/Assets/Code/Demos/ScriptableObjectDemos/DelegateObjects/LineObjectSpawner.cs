using UnityEngine;

namespace UEGP3.Demos.ScriptableObjectDemos.DelegateObjects
{
	/// <summary>
	/// An object spawner that spawns the objects in a straight line.
	/// </summary>
	[CreateAssetMenu(fileName = "StraightLineSpawner", menuName = "UEGP3/SODemos/LineSpawner", order = 0)]
	public class LineObjectSpawner : BaseObjectSpawner
	{
		[SerializeField] private float _distance;

		/// <summary>
		/// Spawns the object in a straight line with a distance of _distance between each
		/// instantiated object.
		/// </summary>
		/// <param name="prefab">The gameobject that is being spawned</param>
		/// <param name="amount">How many of that object should be spawned</param>
		public override void Spawn(GameObject prefab, int amount)
		{
			Vector3 spawnPosition = Vector3.zero;
			for (int i = 0; i < amount; i++)
			{
				Instantiate(prefab, spawnPosition, Quaternion.identity);
				spawnPosition.z += _distance;
			}
		}
	}
}