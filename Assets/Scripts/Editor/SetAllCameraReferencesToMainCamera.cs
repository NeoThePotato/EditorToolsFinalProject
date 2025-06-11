using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;

public static class SetAllCameraReferencesToMainCamera
{
	/// <summary>
	/// Finds any <see cref="Component"/> which has a <see cref="Camera"/> field which is <see langword="null"/> and sets it to <see cref="Camera.main"/>, if exists.
	/// </summary>
	[MenuItem("Tools/Set All Camera References To MainCamera")]
	public static void SetReferences()
	{
		var mainCamera = Camera.main;
		if (!mainCamera)
		{
			Debug.LogWarning("No Camera tagged with \'MainCamera\'.");
			return;
		}
		foreach (var component in Object.FindObjectsByType<Component>(FindObjectsSortMode.None))
		{
			// It's reflection-ing time!
			foreach (var field in component.GetType().GetTypeInfo().GetFields().Where(IsNullCameraReference))
			{
				field.SetValue(component, mainCamera);
				EditorUtility.SetDirty(component);
				Debug.Log($"Set \'{component}.{field.Name}\' reference to \'{mainCamera}\'.");
			}

			bool IsNullCameraReference(FieldInfo fieldInfo) => fieldInfo.FieldType == typeof(Camera) && fieldInfo.GetValue(component) as Camera == null;
		}
	}
}
