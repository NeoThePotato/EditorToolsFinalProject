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
	public static void Apply(ref float current, ref float velocity, float destination, in Parameters parameters, float deltaTime)
	{
		var distance = current - destination;
		var loss = parameters.damping * velocity;
		var force = -parameters.rigidness * distance - loss;
		velocity += force;
		current += velocity * deltaTime;
	}

	[BurstCompile]
	public static void Apply(ref float2 current, ref float2 velocity, in float2 destination, in Parameters parameters, float deltaTime)
	{
		var distance = current - destination;
		var loss = parameters.damping * velocity;
		var force = -parameters.rigidness * distance - loss;
		velocity += force;
		current += velocity * deltaTime;
	}

	[BurstCompile]
	public static void Apply(ref float3 current, ref float3 velocity, in float3 destination, in Parameters parameters, float deltaTime)
	{
		var distance = current - destination;
		var loss = parameters.damping * velocity;
		var force = -parameters.rigidness * distance - loss;
		velocity += force;
		current += velocity * deltaTime;
	}

	[BurstCompile]
	public static void Apply(ref float4 current, ref float4 velocity, in float4 destination, in Parameters parameters, float deltaTime)
	{
		var distance = current - destination;
		var loss = parameters.damping * velocity;
		var force = -parameters.rigidness * distance - loss;
		velocity += force;
		current += velocity * deltaTime;
	}

	[Serializable]
	public struct Parameters
	{
		public float rigidness;
		[Range(0f, 1f)] public float damping;

		public Parameters(float rigidness = 0.1f, float damping = 0.2f)
		{
			this.rigidness = rigidness;
			this.damping = damping;
		}
	}
}

[BurstCompile]
public static class SpringExtensions
{
	[BurstCompile]
	public static void ApplyCircular(ref float currentDegrees, ref  float velocity, in float destination, in Parameters parameters, float deltaTime)
	{
		currentDegrees = destination.GetClosestAngle(currentDegrees);
		velocity = math.clamp(velocity, -Extensions.Math.DEGREES, Extensions.Math.DEGREES);
		Spring.Apply(ref currentDegrees, ref velocity, destination, in parameters, deltaTime);
	}
}
