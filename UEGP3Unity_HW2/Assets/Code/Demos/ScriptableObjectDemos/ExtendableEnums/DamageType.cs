using System.Collections.Generic;
using UnityEngine;

namespace UEGP3.Demos.ScriptableObjectDemos.ExtendableEnums
{
	/// <summary>
	/// A scriptable object that we use to create new damage types. Damage types can be created by either
	/// designers or programmers with ease.
	/// </summary>
	[CreateAssetMenu(fileName = "NewDamageType", menuName = "UEGP3/SODemos/DamageType", order = 0)]
	public class DamageType : ScriptableObject
	{
		[Tooltip("Damage types this type is superior to")] [SerializeField]
		private List<DamageType> _beatenTypes;
		
		public List<DamageType> BeatenTypes => _beatenTypes;
	}
}