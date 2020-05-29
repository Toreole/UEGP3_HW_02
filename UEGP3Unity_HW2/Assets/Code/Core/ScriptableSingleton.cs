using System.IO;
using UnityEditor;
using UnityEngine;

namespace UEGP3.Core
{
	/// <summary>
	/// Example of a Scriptable Singleton. Can be used by extending like so:
	/// public class MyScriptableSingleton : ScriptableSingleton<MyScriptableSingleton>
	///
	/// Uses Resources folder to store the created Scriptable Singletons. It also handles
	/// automatic creation of new Scriptable Singleton Instances, once they're requested.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class ScriptableSingleton<T> : ScriptableObject where T : ScriptableSingleton<T>
	{
		/// <summary>
		/// Scriptable Singletons are always named like their type.
		/// </summary>
		private static string FileName => typeof(T).Name;
#if UNITY_EDITOR
		/// <summary>
		/// Path where
		/// </summary>
		private static string AssetPath => "Assets/Resources/" + FileName + ".asset";
#endif

		/// <summary>
		/// Used for loading at runtime
		/// </summary>
		private static string ResourcePath => FileName;

		public static T Instance
		{
			get
			{
				// NOTE: In a real-world production this should be replaced by an AssetBundle system.
				// Resources folder easily bloats the build size and heavily increases loading times.
				if (_cachedInstance == null) 
				{
					_cachedInstance = Resources.Load(ResourcePath) as T;
				}
#if UNITY_EDITOR
				// if there is no available instance yet, it means we don't have a SO of this stored on disk
				// create one.
				if (_cachedInstance == null) 
				{
					_cachedInstance = CreateAndSave();
				}
#endif
				// if it is still null, create a fallback instance using default values
				if (_cachedInstance == null) 
				{
					Debug.LogWarning("No instance of " + FileName + " found, using default values");
					_cachedInstance = CreateInstance<T>();
					_cachedInstance.OnCreate();
				}

				return _cachedInstance;
			}
		}
		
		/// <summary>
		/// Cached Instance of the Scriptable Singleton. Used to reduce access to Resources.Load();
		/// </summary>
		private static T _cachedInstance;

#if UNITY_EDITOR
		/// <summary>
		/// Creates and saves an instance of the requested Scriptable Singleton
		/// </summary>
		/// <returns>The saved Scriptable Singleton instance</returns>
		private static T CreateAndSave()
		{
			T instance = CreateInstance<T>();
			instance.OnCreate();
			
			//Saving during Awake() will crash Unity, delay saving until next editor frame
			if (EditorApplication.isPlayingOrWillChangePlaymode)
			{
				EditorApplication.delayCall += () => SaveAsset(instance);
			} 
			else
			{
				SaveAsset(instance);
			}
			
			return instance;
		}

		/// <summary>
		/// Saves the in-memory SO to the disk.
		/// </summary>
		/// <param name="obj"></param>
		private static void SaveAsset(T obj)
		{
			string dirName = Path.GetDirectoryName(AssetPath);
			
			// check and validate directory
			if (!Directory.Exists(dirName))
			{
				Directory.CreateDirectory(dirName);
			}
			
			// Save asset to disk
			AssetDatabase.CreateAsset(obj, AssetPath);
			AssetDatabase.SaveAssets();
			Debug.Log("Saved " + FileName + " instance");
		}
#endif

		/// <summary>
		/// Setup specific to the scriptable singleton can be made in this method.
		/// </summary>
		protected virtual void OnCreate()
		{
		}
	}
}