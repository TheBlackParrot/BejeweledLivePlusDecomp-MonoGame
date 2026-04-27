using System;
using SexyFramework;

namespace BejeweledLivePlus
{
	public class QuasiRandom
	{
		public CurvedVal mChance = new CurvedVal();

		public float mSteps;

		public float mLastRoll;

		public QuasiRandom()
		{
			mSteps = 0f;
			mLastRoll = 0f;
		}

		public void Init(string theCurve)
		{
			mSteps = 0f;
			mLastRoll = 0f;
			mChance.SetCurve(theCurve);
		}

		public void Step()
		{
			Step(1f);
		}

		public void Step(float theCount)
		{
			mSteps += theCount;
			if ((double)mChance <= 0.0)
			{
				mLastRoll = 0f;
			}
			else
			{
				mLastRoll = (float)((double)mChance * (double)Math.Min((2.5f), Math.Max((0.2f), (float)Math.Pow((double)((1.5f) * mSteps) * (double)mChance, (1.2)))));
			}
		}

		public bool Check(float theRand)
		{
			if (theRand < mLastRoll)
			{
				mSteps = 0f;
				return true;
			}
			return false;
		}
	}
}
