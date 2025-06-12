using System;
using UnityEngine;

/// <summary>
/// The most annoying camera effect ever concieved.
/// </summary>
public class CameraProjectionSkew : MonoBehaviour
{
	[SerializeField] private CameraProjectionSkewSpring _cameraSkew;
	[SerializeField] private Camera _camera;
	private float _previousY;

	private void Start() => GetY();

	private void LateUpdate()
	{
		UpdateY(out var delta);
		_cameraSkew.velocity += delta;
		_cameraSkew.ApplyToCamera(_camera);
	}

	private float GetY() => _camera.transform.rotation.eulerAngles.y;

	private void UpdateY(out float delta)
	{
		var y = GetY();
		delta = Extensions.Math.AngleDelta(y, _previousY);
		_previousY = y;
	}

	[Serializable]
	public struct CameraProjectionSkewSpring
	{
		public Spring.Parameters spring;
		[Range(0f, 1f)] public float sensitivity;
		[NonSerialized] public float value;
		[NonSerialized] public float velocity;

		public void ApplyToCamera(Camera camera)
		{
			Spring.Apply(ref value, ref velocity, 0f, spring, Time.deltaTime);
			var matrix = camera.projectionMatrix;
			matrix.m01 = value * sensitivity;
			camera.projectionMatrix = matrix;
		}
	}
}
