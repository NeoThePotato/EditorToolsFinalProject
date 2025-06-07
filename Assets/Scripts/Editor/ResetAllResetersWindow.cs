using System.Linq;
using UnityEditor;
using UnityEngine;

public class ResetAllResetersWindow
{
    [MenuItem("Tools/Reset ALL reseters")]
    public static void DestroyEnemies()
    {
        var reseteables = GameObject.FindObjectsByType<Component>(FindObjectsSortMode.None).OfType<IReseteable>();

        foreach (IReseteable resetable in reseteables)
        {
            resetable.Reset();
        }
    }
}
