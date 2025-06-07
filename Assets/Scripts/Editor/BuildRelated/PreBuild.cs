using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class PreBuild : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        var reseteables = GameObject.FindObjectsByType<Component>(FindObjectsSortMode.None).OfType<IReseteable>();

        foreach(IReseteable resetable in reseteables)
        {
            resetable.Reset();
        }

        int resetersAmount = reseteables.Count();

        if ( resetersAmount<= 3)
        {
            EditorUtility.DisplayDialog("Build Warning", $"Too little resets! you only have {resetersAmount} of them!", "Roger that!");
        }
    }
}
