using System;
using System.Collections.Generic;
using UnityEngine;

namespace UEGP3.Demos.ScriptableObjectDemos.ExtendableEnums
{
	/// <summary>
	/// Demonstrating the use case of scriptable object enums.
	/// </summary>
	public class ExtendableEnumDemo : MonoBehaviour
	{
		[Tooltip("Damage Type used for this object")] [SerializeField]
		private DamageType _damageType = null;
		[Tooltip("The object we want to compare to")] [SerializeField]
		private ExtendableEnumDemo _other;
		
		public DamageType DamageType => _damageType;
		
		private void Update()
		{
			// If LMB is pressed, execute comparison
			if (Input.GetButtonDown("Fire1"))
			{
				CompareToOther();
			}
		}

		private void CompareToOther()
		{
			// Only compare if something is referenced in _other
			if (_other == null)
			{
				return;
			}
			
			// The type set for this object beats the other. Destroy it!
			if (_damageType.BeatenTypes.Contains(_other.DamageType))
			{
				Debug.Log("Win!");
				Destroy(_other.gameObject);
			}
			// The type set is beaten by the other. Destroy us!
			else if (_other.DamageType.BeatenTypes.Contains(_damageType))
			{
				Debug.Log("Lost!");
				Destroy(gameObject);
			}
			// No one beats anyone. 
			else
			{
				Debug.Log("Draw!");
			}
		}
	}
}