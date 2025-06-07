using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System.IO;

public class PostBuild : IPostprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPostprocessBuild(BuildReport report)
    {
        string buildPath = report.summary.outputPath;
        string buildDirectory = Path.GetDirectoryName(buildPath);

        if (!(buildDirectory.Contains("Build") || buildDirectory.Contains("build")))
        {
            EditorUtility.DisplayDialog("Did you just build?!", "your path does not have any build in it, are you sure that you chose the correct folder?", "Task failed successfully!");
        }
    }
}
