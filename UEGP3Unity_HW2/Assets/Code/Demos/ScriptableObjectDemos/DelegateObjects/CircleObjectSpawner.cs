using UnityEngine;

namespace UEGP3.Demos.ScriptableObjectDemos.DelegateObjects
{
	/// <summary>
	/// An object spawner that spawns the object in a circular way.
	/// </summary>
	[CreateAssetMenu(fileName = "CircleSpawner", menuName = "UEGP3/SODemos/CircleSpawner", order = 0)]
	public class CircleObjectSpawner : BaseObjectSpawner
	{
		[Tooltip("Radius from the center of the circle")]
		[SerializeField]
		private float _radius;

		/// <summary>
		/// Spawns the object in a circle with a distance of 360/amount degree between each
		/// instantiated object.
		/// </summary>
		/// <param name="prefab">The gameobject that is being spawned</param>
		/// <param name="amount">How many of that object should be spawned</param>
		public override void Spawn(GameObject prefab, int amount)
		{
			// Store degree for calculations
			float currentDegree = 0;
			
			// Calculate degree steps
			float degreeBetweenSpawns = 360f / amount;
			
			for (int i = 0; i < amount; i++)
			{
				// SpawnPoint is rotated from the center by currentDegree
				Vector3 spawnPoint = Quaternion.Euler(0, currentDegree, 0) * Vector3.forward * _radius;
				Instantiate(prefab, spawnPoint, Quaternion.identity);
				currentDegree += degreeBetweenSpawns;
			}
		}
	}
}