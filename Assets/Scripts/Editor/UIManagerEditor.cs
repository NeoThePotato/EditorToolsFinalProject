using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIManager))]
public class UIManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty uiName = serializedObject.FindProperty("uiName");
        SerializedProperty uiObject = serializedObject.FindProperty("uiObject");

        EditorGUILayout.PropertyField(uiName);
        //show error if ui is not assigned
        if (uiName.stringValue == "")
        {
            EditorGUILayout.HelpBox("Please add a ui by name for reference.", MessageType.Error);
        }
        //show error if ui prefab was not found
        else if (uiName.stringValue != uiObject.objectReferenceValue.name)
        {
            EditorGUILayout.HelpBox("UI by that name was not found.", MessageType.Error);
        }
        else if (uiObject != null)
        {
            EditorGUILayout.PropertyField(uiObject);
        }

            serializedObject.ApplyModifiedProperties();
    }
}
