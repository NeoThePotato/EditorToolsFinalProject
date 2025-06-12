using UnityEngine;
using Unity.Mathematics;

[RequireComponent(typeof(Rigidbody))]
public class SpringFollowRigidbody : MonoBehaviour
{
	[SerializeField] private Spring.Parameters _springParameters;
	[SerializeField, HideInInspector] private Rigidbody _rigidbody;
	[SerializeField] private Transform _toFollow;

	private void OnValidate() => _rigidbody = GetComponent<Rigidbody>();

	private void Update()
	{
		float3 position = _rigidbody.position;
		float3 velocity = _rigidbody.linearVelocity;
		Spring.Apply(ref position, ref velocity, _toFollow.position, in _springParameters, Time.deltaTime);
		_rigidbody.linearVelocity = velocity;
	}
}
