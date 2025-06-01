using System.Xml.Linq;
using UnityEditor;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public string uiName;
    public GameObject uiObject;

    private GameObject uiParent;

    private void OnValidate()
    {
        GameObject uiGO = FindPrefabByName(uiName);
        //if you tried and succeeded to find a ui by string name
        if (uiGO.name != null)
        {
            Debug.Log(uiGO.name + " was found");
            uiObject = uiGO;
        }
        //if failed
        else if (uiGO == null && uiName != null)
        {
            Debug.Log("ui object was not found");
            uiObject = null;
        }
    }

    private void Start()
    {
        if (uiObject != null)
        {
            AddUI(uiObject);
        }
    }

    private GameObject FindPrefabByName(string prefabName)
    {
        string[] searchAt = new string[] { "Assets/Prefabs/UI" };
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

        Debug.LogWarning($"Prefab named '{prefabName}' not found in Assets/Prefabs/UI.");
        return null;
    }

    private void AddUI(GameObject uiObj)
    {
        //There must be only 1 GO tagged "UI" in the scene for this to work
        if (uiParent == null)
        {
            uiParent = GameObject.FindGameObjectWithTag("UI");
            if (uiParent == null)
                Debug.Log("UI tag not found in scene");
        }

        uiObj = Instantiate(uiObj, uiParent.transform);
    }
}
