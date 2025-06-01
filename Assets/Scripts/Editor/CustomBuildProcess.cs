using UnityEngine;
using UnityEditor;
using System.Linq;

[InitializeOnLoad]
public class CustomBuildProcess
{
    static CustomBuildProcess()
    {
        BuildPlayerWindow.RegisterBuildPlayerHandler(BuildPlayerWithValidation);
    }

    private static void BuildPlayerWithValidation(BuildPlayerOptions options)
    {
        bool dialogueChoice = true;
        if (!CheckValidatedObjectsAboveFive())
        {
            dialogueChoice = EditorUtility.DisplayDialog("Build Error:","You have 5 or less IValidateables in the scene, are you sure it's intented?",
                "It's fine!", "Oops! Nope! Redo");
        }

        if (dialogueChoice)
            BuildPipeline.BuildPlayer(options);
    }

    public static bool CheckValidatedObjectsAboveFive()
    {
        var validateables = GameObject.FindObjectsOfType<MonoBehaviour>().OfType<IValidateable>();

        return validateables.Count() >5;
    }
}
