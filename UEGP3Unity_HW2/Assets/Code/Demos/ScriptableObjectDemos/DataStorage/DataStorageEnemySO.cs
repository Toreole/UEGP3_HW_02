using UnityEngine;

namespace UEGP3.Demos.ScriptableObjectDemos.DataStorage
{
	/// <summary>
	/// A scriptable object that stores the data for our prototypical enemy.
	/// </summary>
	[CreateAssetMenu(fileName = "DataStorageEnemy", menuName = "UEGP3/SODemos/DataStorageEnemy", order = 0)]
	public class DataStorageEnemySO : ScriptableObject
	{
		[Tooltip("How many hit points does this creature have at maximum?")] [SerializeField] private float _maximumHealth;
		
		public float MaximumHealth => _maximumHealth;
	}
}