using System;
using UnityEngine;
using Unity.Burst;
using Unity.Mathematics;
using Extensions;
using static Spring;

[BurstCompile]
public static class Spring
{
	[BurstCompile]
	public static void Apply(ref float current, ref float velocity, in Parameters parameters, float deltaTime)
	{
		float distance = current - parameters.destination;
		float loss = parameters.damping * velocity;

		// Hooke's Law
		float force = -parameters.rigidness * distance - loss;
		velocity += force;
		current += velocity * deltaTime;
	}

	[Serializable]
	public struct Parameters
	{
		public float rigidness;
		[Range(0f, 1f)] public float damping;
		[NonSerialized] public float destination;

		public Parameters(float destination, float rigidness = 0.1f, float damping = 0.2f)
		{
			this.rigidness = rigidness;
			this.damping = damping;
			this.destination = destination;
		}
	}
}

[BurstCompile]
public static class SpringExtensions
{
	[BurstCompile]
	public static void ApplyCircular(ref float currentDegrees, ref  float velocity, in Parameters parameters, float deltaTime)
	{
		currentDegrees = parameters.destination.GetClosestAngle(currentDegrees);
		velocity = math.clamp(velocity, -Extensions.Math.DEGREES, Extensions.Math.DEGREES);
		Spring.Apply(ref currentDegrees, ref velocity, in parameters, deltaTime);
	}

	[BurstCompile]
	public static void ApplyCircular(ref float currentDegrees, ref Spring1 spring, float deltaTime) => ApplyCircular(ref currentDegrees, ref spring.velocity, in spring.parameters, deltaTime);

	[Serializable]
	public struct Spring1
	{
		public Parameters parameters;
		[NonSerialized] public float velocity;

		public Spring1(in Parameters parameters, float velocity)
		{
			this.parameters = parameters;
			this.velocity = velocity;
		}

		public Spring1(in Parameters parameters)
		{
			this.parameters = parameters;
			velocity = default;
		}
	}
}
