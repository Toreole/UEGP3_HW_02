using System;
using System.ComponentModel;
using Unity.Collections;
using UnityEngine;

namespace UEGP3.Demos.ScriptableObjectDemos.DataStorage
{
	/// <summary>
	/// A prototypical enemy class to showcase the use Scriptable Objects for storing data.
	/// </summary>
	public class DataStorageEnemy : MonoBehaviour
	{
		[Tooltip("Pressing the set key executes the damage method")] [SerializeField] 
		private KeyCode _doDamageKeyCode;
		[Tooltip("Pressing the set key executes the damage method")] [SerializeField] 
		private KeyCode _resetHealthKeyCode;
		[Tooltip("Naive implementation of maximum health property. If no SO is set below, this value is being used.")] [SerializeField] 
		private float _maxHealth = 100f;
		[Tooltip("Reference to the scriptable object that stores data for this enemy")] [SerializeField] 
		private DataStorageEnemySO maxDataStorageEnemySo = null;
		
		private bool _maxHealthFromScriptableObject;
		private float _currentHealth;

		private void Awake()
		{
			_maxHealthFromScriptableObject = maxDataStorageEnemySo != null;
			// depending on if a SO had been set in awake, refill health to the value set in the SO or in the inspector
			_currentHealth = _maxHealthFromScriptableObject ? maxDataStorageEnemySo.MaximumHealth : _maxHealth;
		}

		private void Update()
		{
			// If this button is pressed, refill the health to full health
			if (Input.GetKeyDown(_resetHealthKeyCode))
			{
				// depending on if a SO had been set in awake, refill health to the value set in the SO or in the inspector
				_currentHealth = _maxHealthFromScriptableObject ? maxDataStorageEnemySo.MaximumHealth : _maxHealth;
			}
			
			// If this button is pressed, refill the health to full health
			if (Input.GetKeyDown(_doDamageKeyCode))
			{
				_currentHealth -= 10f;
			}

			// If no HP are left, kill the object
			if (_currentHealth <= 0)
			{
				Debug.Log($"{name} has 0 HP left and died!");
				Destroy(gameObject);
			}
		}
	}
}