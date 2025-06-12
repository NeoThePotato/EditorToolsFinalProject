using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using static CameraProjectionSkew;

[CustomPropertyDrawer(typeof(CameraProjectionSkewSpring))]
public class CameraProjectionSkewDrawer : PropertyDrawer
{
	private SerializedProperty springProperty;
	private SerializedProperty sensitivityProperty;
	private HelpBox noSensitivityWarningBox;

	public override VisualElement CreatePropertyGUI(SerializedProperty property)
	{
		var container = new VisualElement();

		// Values
		springProperty = property.FindPropertyRelative("spring");
		sensitivityProperty = property.FindPropertyRelative("sensitivity");
		noSensitivityWarningBox = new HelpBox()
		{
			messageType = HelpBoxMessageType.Warning,
			text = "\'CameraProjectionSkewSpring\' has 0 sensitivity, effects will not be felt.\nIf this is what you intend - it's recommended to turn off its controlling \'CameraProjectionSkew\' MonoBehaviour to save on performance."
		};
		var rigidnessField = new PropertyField(springProperty);
		var dampingField = new PropertyField(sensitivityProperty);
		container.Add(rigidnessField);
		container.Add(dampingField);
		container.Add(noSensitivityWarningBox);
		UpdateWarning();
		container.RegisterCallback<SerializedPropertyChangeEvent>(UpdateWarning);

		return container;
	}

	private void UpdateWarning(SerializedPropertyChangeEvent _) => UpdateWarning();

	private void UpdateWarning() => noSensitivityWarningBox.visible = sensitivityProperty.floatValue <= 0f;
}