using UnityEngine;
using Unity.Mathematics;
using Unity.Burst;

namespace Extensions
{
	[BurstCompile]
	static class Math
	{
		public const float DEGREES = 360f;
		public const float INV_DEGREES = 1f / DEGREES;
		public const float HALF_ROTATION = DEGREES/2f;

		[BurstCompile]
		public static void ClampEuler(ref float3 value, in float3 min, in float3 max)
		{
			value = new(ClampEuler(value.x, min.x, max.x), ClampEuler(value.y, min.y, max.y), ClampEuler(value.z, min.z, max.z));
		}

		[BurstCompile]
		public static float ClampEuler(float value, float min, float max)
		{
			min = min.ToPositiveAngle();
			max = max.ToPositiveAngle();
			value = value.ToPositiveAngle();
			if (min > max)
			{
				min -= DEGREES;
				if (value > HALF_ROTATION)
					value -= DEGREES;
			}
			return math.clamp(value, min, max);
		}

		/// <summary>
		/// Clamps <paramref name="value"/> so that its <see cref="Quaternion.eulerAngles"/> is between <paramref name="min"/> and <paramref name="max"/>.
		/// </summary>
		/// <param name="value">Quaternion to clamp.</param>
		/// <param name="min">Min euler angles in signed degrees.</param>
		/// <param name="max">Max euler angles in signed degrees.</param>
		[BurstCompile]
		public static void ClampQuaternion(ref quaternion value, in float3 min, in float3 max)
		{
			var euler = math.Euler(value);
			ClampEuler(ref euler, min, max);
			value = quaternion.Euler(euler);
		}

		/// <summary>Returns the result of a CLAMPING linear remapping of a value x from source range [srcStart, srcEnd] to the destination range [dstStart, dstEnd].</summary>
		/// <param name="srcStart">The start point of the source range [srcStart, srcEnd].</param>
		/// <param name="srcEnd">The end point of the source range [srcStart, srcEnd].</param>
		/// <param name="dstStart">The start point of the destination range [dstStart, dstEnd].</param>
		/// <param name="dstEnd">The end point of the destination range [dstStart, dstEnd].</param>
		/// <param name="x">The value to remap from the source to destination range.</param>
		/// <returns>The remap of input x from the source range to the destination range.</returns>
		[BurstCompile]
		public static float ClampedRemap(float srcStart, float srcEnd, float dstStart, float dstEnd, float x)
		{
			float lower = dstStart, upper = dstEnd;
			EnsureSize(ref lower, ref upper);
			return math.clamp(math.remap(srcStart, srcEnd, dstStart, dstEnd, x), lower, upper);
		}

		/// <summary>
		/// Loops the value <paramref name="t"/>, so that it is never larger than <paramref name="length"/> and never smaller than 0.
		/// </summary>
		[BurstCompile]
		public static float Repeat(float t, float length)
		{
			return math.clamp(t - math.floor(t / length) * length, 0f, length);
		}

		/// <summary>
		/// Ensures that <paramref name="low"/> is smaller than <paramref name="high"/>.
		/// </summary>
		[BurstCompile]
		public static void EnsureSize(ref float low, ref float high)
		{
			if (low > high)
				(low, high) = (high, low);
		}

		/// <summary>
		/// Subtracts <paramref name="subtract"/> from <paramref name="val"/> without underflowing.
		/// </summary>
		/// <param name="val">Value to substract from.</param>
		/// <param name="subtract">Value to subtract with.</param>
		/// <returns>The value actually subtracted without underflowing.</returns>
		[BurstCompile]
		public static uint SubtractNoUnderflow(ref uint val, uint subtract)
		{
			subtract = math.min(val, subtract);
			val -= subtract;
			return subtract;
		}

		/// <returns>An angle that is equivalent to <paramref name="relativeAngle"/> but is less or equal to 180 degrees away from <paramref name="angleInDegrees"/>.</returns>
		[BurstCompile]
		public static float GetClosestAngle(this float angleInDegrees, float relativeAngle)
		{
			var val = GetClosestZero(angleInDegrees) + ToSignedAngle(relativeAngle);
			var difference = val - angleInDegrees;
			return math.select(val, val - (DEGREES * math.sign(difference)), math.abs(difference) > HALF_ROTATION);
		}

		/// <returns>An angle that is equivalent to 0 but is less or equal to 180 degrees away from <paramref name="angleInDegrees"/>.</returns>
		[BurstCompile]
		public static float GetClosestZero(this float angleInDegrees)
		{
			return math.round(angleInDegrees * INV_DEGREES) * DEGREES;
		}

		/// <summary>
		/// Forces <paramref name="angleInDegrees"/> to a positive (0 to 360) angle.
		/// </summary>
		/// <returns><paramref name="angleInDegrees"/> in positive degrees.</returns>
		[BurstCompile]
		public static float ToPositiveAngle(this float angleInDegrees)
		{
			return Repeat(angleInDegrees, DEGREES);
		}

		/// <returns><paramref name="eulerAngles"/> in positive degrees.</returns>
		[BurstCompile]
		public static void ToPositiveAngle(ref float2 eulerAngles)
		{
			eulerAngles = new(ToPositiveAngle(eulerAngles.x), ToPositiveAngle(eulerAngles.y));
		}

		/// <returns><paramref name="eulerAngles"/> in positive degrees.</returns>
		[BurstCompile]
		public static void ToPositiveAngle(ref float3 eulerAngles)
		{
			eulerAngles = new(ToPositiveAngle(eulerAngles.x), ToPositiveAngle(eulerAngles.y), ToPositiveAngle(eulerAngles.z));
		}

		/// <returns><paramref name="eulerAngles"/> in positive degrees.</returns>
		[BurstCompile]
		public static void ToPositiveAngle(ref float4 eulerAngles)
		{
			eulerAngles = new(ToPositiveAngle(eulerAngles.x), ToPositiveAngle(eulerAngles.y), ToPositiveAngle(eulerAngles.z), ToPositiveAngle(eulerAngles.w));
		}

		/// <summary>
		/// Forces <paramref name="angleInDegrees"/> to a signed (-180 to +180) angle.
		/// </summary>
		/// <returns><paramref name="angleInDegrees"/> in signed degrees.</returns>
		[BurstCompile]
		public static float ToSignedAngle(this float angleInDegrees)
		{
			return (angleInDegrees + HALF_ROTATION).ToPositiveAngle() - HALF_ROTATION;
		}

		/// <returns><paramref name="eulerAngles"/> in signed degrees.</returns>
		[BurstCompile]
		public static void ToSignedAngle(ref float2 eulerAngles)
		{
			eulerAngles = new(ToSignedAngle(eulerAngles.x), ToSignedAngle(eulerAngles.y));
		}

		/// <returns><paramref name="eulerAngles"/> in signed degrees.</returns>
		[BurstCompile]
		public static void ToSignedAngle(ref float3 eulerAngles)
		{
			eulerAngles = new(ToSignedAngle(eulerAngles.x), ToSignedAngle(eulerAngles.y), ToSignedAngle(eulerAngles.z));
		}

		/// <returns><paramref name="eulerAngles"/> in signed degrees.</returns>
		[BurstCompile]
		public static void ToSignedAngle(ref float4 eulerAngles)
		{
			eulerAngles = new(ToSignedAngle(eulerAngles.x), ToSignedAngle(eulerAngles.y), ToSignedAngle(eulerAngles.z), ToSignedAngle(eulerAngles.w));
		}

		[BurstCompile]
		public static float AngleDegrees(in float2 from, in float2 to) => math.degrees(AngleRadians(from, to));

		[BurstCompile]
		public static float AngleRadians(in float2 from, in float2 to)
		{
			float num = math.sqrt(math.lengthsq(from) * math.lengthsq(to));
			if (num < 1E-15f)
				return 0f;
			float num2 = math.clamp(math.dot(from, to) / num, -1f, 1f);
			return math.acos(num2);
		}

		[BurstCompile]
		public static float Cross(in float2 A, in float2 B) => A.y * B.x - A.x * B.y;

		[BurstCompile]
		public static float TriangleWave(float f) => 2 * math.abs(f - math.floor(f + 0.5f)) - 1;

		[BurstCompile]
		public static float SquareWave(float f) => 2 * (2 * math.floor(f) - math.floor(2 * f)) + 1;

		/// <param name="rect">Rectangle to lerp over.</param>
		/// <param name="t">Relative point on the perimeter.</param>
		/// <returns>Point on <paramref name="rect"/>'s perimeter corresponding to <paramref name="t"/>, starting at the top-left corner and moving clockwise.</returns>
		public static Vector2 LerpPerimeter(this Rect rect, float t)
		{
			Vector2 center = rect.center;
			rect.center = Vector2.zero;
			float halfPerimeter = rect.width + rect.height;
			float perimeter = halfPerimeter * 2f;
			t = Mathf.Repeat(t, 1f) * perimeter;
			bool overHalf = t >= halfPerimeter;
			t = Mathf.Repeat(t, halfPerimeter);
			bool isHeight = t >= rect.width;
			Vector2 perimeterPoint;
			if (isHeight)
				perimeterPoint = new(rect.xMax, rect.yMax - (t - rect.width));
			else
				perimeterPoint = new(rect.xMin + t, rect.yMax);
			if (overHalf)
				perimeterPoint = new(-perimeterPoint.x, -perimeterPoint.y);
			return center + perimeterPoint;
		}

		/// <returns>The corner of <paramref name="bounds"/> that is farthest away from <paramref name="point"/>.</returns>
		public static float3 FarthestPoint(this in Bounds bounds, in float3 point)
		{
			return math.select(bounds.max, bounds.min, point > bounds.center);
		}
	}
}
