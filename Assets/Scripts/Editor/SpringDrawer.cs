using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static Spring;

[CustomPropertyDrawer(typeof(Parameters))]
public class SpringDrawer : PropertyDrawer, IDisposable
{
	private const string RIGIDNESS = "_Rigidness", DAMPING = "_Damping";
	private static Shader SPRING_SHADER => Shader.Find("Shader Graphs/Spring");

	private SerializedProperty rigidnessProperty;
	private SerializedProperty dampingProperty;
	private HelpBox negativeRigidnessWarningBox, dampingInfoBox;
	private Material springMaterial;
	private CustomRenderTexture springTexture;
	private CommandBuffer cmd;

	public override VisualElement CreatePropertyGUI(SerializedProperty property)
	{
		var container = new TwoPaneSplitView(0, 120, TwoPaneSplitViewOrientation.Horizontal);

		// Texture & Material
		springMaterial = new(SPRING_SHADER);
		springTexture = new(128, 128)
		{
			enableRandomWrite = true,
			material = springMaterial,
			initializationColor = Color.white,
			dimension = TextureDimension.Tex2D,
			updateMode = CustomRenderTextureUpdateMode.OnDemand
		};
		springTexture.Create();

		// Image
		var image = new Image()
		{
			image = springTexture
		};
		cmd = new();

		// Values
		var values = new VisualElement();
		rigidnessProperty = property.FindPropertyRelative("rigidness");
		dampingProperty = property.FindPropertyRelative("damping");
		var rigidnessField = new PropertyField(rigidnessProperty);
		var dampingField = new PropertyField(dampingProperty);
		negativeRigidnessWarningBox = new HelpBox()
		{
			messageType = HelpBoxMessageType.Warning,
			text = "Field \'rigidness\' should not be negative."
		};
		dampingInfoBox = new HelpBox()
		{
			messageType = HelpBoxMessageType.Info
		};
		UpdateWarning();
		values.Add(rigidnessField);
		values.Add(dampingField);
		values.Add(negativeRigidnessWarningBox);
		values.Add(dampingInfoBox);

		// Container
		container.Add(image);
		container.Add(values);
		container.RegisterCallback<SerializedPropertyChangeEvent>(SpringChanged);

		return container;
	}

	public void Dispose()
	{
		springTexture.Release();
		cmd.Dispose();
	}

	private void SpringChanged(SerializedPropertyChangeEvent evt)
	{
		UpdateWarning();
		var materialProperty = (evt.changedProperty.name) switch
		{
			"rigidness" => RIGIDNESS,
			"damping" => DAMPING,
			_ => string.Empty
		};
		if (string.IsNullOrEmpty(materialProperty))
			return;
		springMaterial.SetFloat(materialProperty, evt.changedProperty.floatValue);
		UpdateTexture();
	}

	private void UpdateTexture()
	{
		cmd.Clear();
		cmd.Blit(null, springTexture, springMaterial);
		Graphics.ExecuteCommandBuffer(cmd);
		springTexture.Update();
	}

	private void UpdateWarning()
	{
		if (negativeRigidnessWarningBox.visible = rigidnessProperty.floatValue < 0f)
		{
			Debug.LogWarning(negativeRigidnessWarningBox.text);
			rigidnessProperty.floatValue = 0f;
		}
		var undamped = dampingProperty.floatValue == 0f;
		var criticallyDamped = dampingProperty.floatValue >= 1f;
		if (dampingInfoBox.visible = undamped || criticallyDamped)
			dampingInfoBox.text = undamped ? "Spring is undamped." : "Spring is critically damped.";
	}
}