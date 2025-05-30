using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MyCharacterController))]
public class MyCharacterControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty camera = serializedObject.FindProperty("camera");
        SerializedProperty moveSpeed = serializedObject.FindProperty("moveSpeed");
        SerializedProperty canJump = serializedObject.FindProperty("canJump");
        SerializedProperty jumpHeight = serializedObject.FindProperty("jumpHeight");
        SerializedProperty customSensitivity = serializedObject.FindProperty("customSensitivity");
        SerializedProperty sensitivity = serializedObject.FindProperty("sensitivity");

        EditorGUILayout.PropertyField(camera);
        // Show error if playerCamera is not assigned
        if (camera.objectReferenceValue == null)
        {
            EditorGUILayout.HelpBox("Please add a camera reference.", MessageType.Error);
        }

        EditorGUILayout.PropertyField(moveSpeed);
        EditorGUILayout.PropertyField(canJump);

        if (canJump.boolValue)
        {
            EditorGUILayout.PropertyField(jumpHeight);
        }

        EditorGUILayout.PropertyField(customSensitivity);

        if (customSensitivity.boolValue)
        {
            EditorGUILayout.PropertyField(sensitivity);
        }

        serializedObject.ApplyModifiedProperties();
    }
}