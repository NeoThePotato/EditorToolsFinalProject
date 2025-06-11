using UnityEngine;
using Unity.Mathematics;

public class SpringFollowTransform : MonoBehaviour
{
	[SerializeField] private Spring.Parameters _springParameters;
	[SerializeField] private Transform _toFollow;
	private float3 _velocity;

	private void Update()
	{
		float3 position = transform.position;
		Spring.Apply(ref position, ref _velocity, _toFollow.position, in _springParameters, Time.deltaTime);
		transform.position = position;
	}
}
