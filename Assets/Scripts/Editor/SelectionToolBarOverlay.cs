using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.Toolbars;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

[Overlay(typeof(SceneView), "Select By Tag")]
[Icon("Assets/Textures/EnviromentSelectionIcon")]
public class SelectionToolBarOverlay : ToolbarOverlay
{
    public override VisualElement CreatePanelContent()
	{
        var content = base.CreatePanelContent();

		foreach (var tag in InternalEditorUtility.tags)
            content.Add(new TagSelectionButton(tag));

		return content;
	}
}

[EditorToolbarElement("custom-toolbar/tagSelector", typeof(SceneView))]
public class TagSelectionButton : EditorToolbarButton
{
    public string tag;

    public TagSelectionButton(string tag)
    {
        this.tag = tag;
        text = $"Select All \'{tag}\'";
        tooltip = $"Press to select all GameObjects in scene with the tag \'{tag}\'";
        clicked += () =>
        {
            GameObject[] environmentsObjects = GameObject.FindGameObjectsWithTag(tag);

            Selection.objects = environmentsObjects;
        };
    }
}
