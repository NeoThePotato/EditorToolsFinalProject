using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

public class EnemySpawnerWindow: EditorWindow
{
    private int fromX;
    private int toY;

    private string prefabToLookFor = "";
    private GameObject enemyPrefab;

    private List<GameObject> enemyList;

    [MenuItem("Tools/Enemy Spawner")]
    public static void ShowWindow()
    {
        EnemySpawnerWindow window = GetWindow<EnemySpawnerWindow>("Enemy Spawner");
        window.titleContent = new GUIContent("Enemy Spawner");
        window.minSize = new Vector2(500, 500);
    }

    private void OnGUI()
    {
        //label
        EditorGUILayout.LabelField("Enemy Spawner", EditorStyles.boldLabel);

        //random bound in scene
        fromX = EditorGUILayout.IntField("From X", fromX);
        toY = EditorGUILayout.IntField("To Y", toY);

        EditorGUILayout.Space();

        //enemy selector
        prefabToLookFor = EditorGUILayout.TextField("Enemy Type", prefabToLookFor);
        enemyPrefab = FindPrefabByName(prefabToLookFor);

        //show error if prefab is not assigned
        if (prefabToLookFor == "")
        {
            EditorGUILayout.HelpBox("Please add a prefab by name for reference.", MessageType.Error);
        }
        //show error if enemy prefab was not found
        else if (prefabToLookFor != "" && enemyPrefab == null)
        {
            EditorGUILayout.HelpBox("Prefab by that name was not found.", MessageType.Error);
        }
        else if (enemyPrefab != null)
        {
            Texture icon = AssetPreview.GetAssetPreview(enemyPrefab);
            if (icon != null)
            {
                GUILayout.Label(icon, GUILayout.Width(240), GUILayout.Height(240));
            }
        }

        EditorGUILayout.Space();

        //enemy spawner
        if (GUILayout.Button("Spawn Enemy"))
        {
            SpawnEnemyAt();
        }

        EditorGUILayout.Space();

        //enemy destroyer
        if (GUILayout.Button("Destroy Enemies"))
        {
            DestroyEnemies();
        }
    }

    private void SpawnEnemyAt()
    {
        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(enemyPrefab);
        Debug.Log(instance.name);
       
        float randomX = UnityEngine.Random.Range(-fromX, fromX);
        float randomY = UnityEngine.Random.Range(-toY, toY);
        Vector3 origin = new Vector3(randomX, 100, randomY);
        
        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("PlaneBlock")))
        {
            instance.transform.position = new Vector3(randomX, hit.transform.position.y, randomY);
        }
        else
        {
            Debug.Log("Raycast didn't hit the ground, your bounding box may be too big.");
        }
        enemyList.Add(instance);
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

    private void DestroyEnemies()
    {
        foreach(GameObject enemy in enemyList)
        {

            Debug.Log(enemy.name);
            DestroyImmediate(enemy);
        }
        enemyList.Clear();
    }
}