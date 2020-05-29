using System.IO;
using UnityEditor;
using UnityEngine;

namespace UEGP3.Demos.ScriptableObjectDemos.GenericScriptableObjectCreator
{
	public static class ScriptableObjectCreateInstanceDemo
	{
		[MenuItem("Assets/Create/NewMyScriptableObjectInstance", priority = -100)]
		public static void CreateNewMyScriptableObjectInstance()
		{
			CreateScriptableObject<MyScriptableObject>();
		}
		
		public static void CreateScriptableObject<T>() where T : ScriptableObject
		{
			T scriptableObjectInstance = ScriptableObject.CreateInstance<T>();
			
			string path = AssetDatabase.GetAssetPath(Selection.activeObject);
			if (path == "")
			{
				path = "Assets";
			}
			else if (Path.GetExtension(path) != "")
			{
				path = path.Replace(Path.GetFileName(path), "");
			}

			if (!path[path.Length - 1].Equals('/'))
			{
				path += "/";
			}
			
			string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath($"{path}/New {typeof(T).Name}.asset");

			AssetDatabase.CreateAsset(scriptableObjectInstance, assetPathAndName);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			
			EditorUtility.FocusProjectWindow();
			Selection.activeObject = scriptableObjectInstance;
		}
	}
}