using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MyCharacterController))]
public class MyCharacterControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty cam = serializedObject.FindProperty("cam");
        SerializedProperty moveSpeed = serializedObject.FindProperty("moveSpeed");
        SerializedProperty canJump = serializedObject.FindProperty("canJump");
        SerializedProperty jumpHeight = serializedObject.FindProperty("jumpHeight");
        SerializedProperty customSensitivity = serializedObject.FindProperty("customSensitivity");
        SerializedProperty sensitivity = serializedObject.FindProperty("sensitivity");

        EditorGUILayout.PropertyField(cam);
        // Show error if camera is not assigned
        if (cam.objectReferenceValue == null)
        {
            EditorGUILayout.HelpBox("Please add a camera reference.", MessageType.Error);
        }

        EditorGUILayout.PropertyField(moveSpeed);
        

        if (canJump.boolValue)
        {
            //can jump color
            GUI.color = Color.green;
            EditorGUILayout.PropertyField(canJump);
            GUI.color = Color.white;

            EditorGUILayout.PropertyField(jumpHeight);
        }
        else
        {
            //can jump color
            GUI.color = Color.red;
            EditorGUILayout.PropertyField(canJump);
            GUI.color = Color.white;
        }

            

        if (customSensitivity.boolValue)
        {
            //custom check color
            GUI.color = Color.green;
            EditorGUILayout.PropertyField(customSensitivity);
            GUI.color = Color.white;

            //sensitivity color
            float value = Mathf.Clamp01(sensitivity.floatValue);
            GUI.color = Color.Lerp(Color.blue, Color.cyan, value);
            EditorGUILayout.PropertyField(sensitivity);
            GUI.color = Color.white;
        }
        else
        {
            //custom check color
            GUI.color = Color.red;
            EditorGUILayout.PropertyField(customSensitivity);
            GUI.color = Color.white;
        }

            serializedObject.ApplyModifiedProperties();
    }
}