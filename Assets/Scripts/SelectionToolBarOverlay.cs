using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.Toolbars;
using UnityEditorInternal;
using UnityEngine;

[Overlay(typeof(SceneView), "Custom Toolbar Overlay")]
public class SelectionToolBarOverlay : ToolbarOverlay
{
    public SelectionToolBarOverlay() : base("custom-toolbar/environmentSelector", "custom-toolbar/customTagSelector")
    {
    }
}

[EditorToolbarElement("custom-toolbar/environmentSelector", typeof(SceneView))]
public class EnviroumentSelectionButton : EditorToolbarButton
{
    public EnviroumentSelectionButton()
    {
        text = "Select All Environments";
        tooltip = "Press to select all gameobjects in scene with the tag 'Environment' ";
        clicked += () =>
        {
            GameObject[] environmentsObjects = GameObject.FindGameObjectsWithTag("environment");

            Selection.objects = environmentsObjects;
        };
    }
}

[EditorToolbarElement("custom-toolbar/customTagSelector", typeof(SceneView))]
public class CustomButton2 : EditorToolbarFloatField
{
    public CustomButton2()
    {
        text = "Custom tag selector";
        tooltip = "Enter the index of the wanted tag to select all objects in this tag";
        


        GameObject[] environmentsObjects = GameObject.FindGameObjectsWithTag(InternalEditorUtility.tags[int.Parse(textSelection.ToString())]);

        Selection.objects = environmentsObjects;
        


    }
}