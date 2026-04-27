namespace SexyFramework.Misc
{
	public class SexyMath
	{
		public static float DegToRad(float inX)
		{
			return inX * (float)Math.PI / 180f;
		}

		public static float RadToDeg(float inX)
		{
			return inX * 180f / (float)Math.PI;
		}

		public static bool ApproxEquals(float inL, float inR, float inTol)
		{
			return MathF.Abs(inL - inR) <= inTol;
		}

		public static bool ApproxEquals(double inL, double inR, double inTol)
		{
			return Math.Abs(inL - inR) <= inTol;
		}

		public static float Lerp(float inA, float inB, float inAlpha)
		{
			return inA + (inB - inA) * inAlpha;
		}

		public static double Lerp(double inA, double inB, double inAlpha)
		{
			return inA + (inB - inA) * inAlpha;
		}

		public static bool IsPowerOfTwo(uint inX)
		{
			if (inX != 0)
			{
				return (inX & (inX - 1)) == 0;
			}
			return false;
		}
	}
}
