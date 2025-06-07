using UnityEditor;
using UnityEngine;

public class PrefabColoringWindow : EditorWindow
{

    private string prefabToLookFor = "";
    private GameObject PrefabToColor;
    private Color chosenColor;




    [MenuItem("Tools/Perab Coloring")]
    public static void ShowWindow()
    {
        PrefabColoringWindow window = GetWindow<PrefabColoringWindow>("Perab Coloring");
        window.titleContent = new GUIContent("Perab Coloring");
        window.minSize = new Vector2(500, 500);
    }


    private void OnGUI()
    {
        //label
        EditorGUILayout.LabelField("Perab Painter", EditorStyles.boldLabel);

        //prefab selector
        prefabToLookFor = EditorGUILayout.TextField("Prefab Name", prefabToLookFor);
        PrefabToColor = FindPrefabByName(prefabToLookFor);

        //show warning if prefab is not assigned
        if (prefabToLookFor == "")
        {
            EditorGUILayout.HelpBox("Please add a prefab by name for reference.", MessageType.Warning);
        }
        //show warning if enemy prefab was not found
        else if (prefabToLookFor != "" && PrefabToColor == null)
        {
            EditorGUILayout.HelpBox("Prefab by that name was not found.", MessageType.Warning);
        }
        else if (PrefabToColor != null)
        {
            Texture icon = AssetPreview.GetAssetPreview(PrefabToColor);
            if (icon != null)
            {
                GUILayout.Label(icon, GUILayout.Width(240), GUILayout.Height(240));
            }
        }

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Choose Color: ", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        chosenColor = EditorGUILayout.ColorField("New Color", chosenColor);

        if (GUILayout.Button("Apply Color"))
        {
            ApplyColor();
        }

    }


    private GameObject FindPrefabByName(string prefabName)
    {
        string[] searchAt = new string[] { "Assets/Prefabs/Enemies" };
        string[] assets = AssetDatabase.FindAssets(prefabName + " t:Prefab", searchAt);

        foreach (string asset in assets)
        {
            string path = AssetDatabase.GUIDToAssetPath(asset);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab != null && prefab.name == prefabName)
            {
                return prefab;
            }
        }
        return null;
    }

    private void ApplyColor()
    {
        PrefabToColor.GetComponent<Renderer>().sharedMaterial.color = chosenColor;
    }
}
