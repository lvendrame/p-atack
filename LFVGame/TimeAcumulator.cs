using System;
using System.Collections.Generic;
using System.Text;

namespace LFVGame
{
	public struct TimeAcumulator
	{
		public TimeAcumulator(double dblMaxTime)
		{
			this.dblMaxTime = dblMaxTime;
            dblAcumulatedTime = 0;
            blnIsOverflow = false;
		}

		private double dblMaxTime;
		public double MaxTime
		{
			get { return dblMaxTime; }
			set { dblMaxTime = value; }
		}

		private double dblAcumulatedTime;
		public double AcumulatedTime
		{
			get { return dblAcumulatedTime; }
			set { dblAcumulatedTime = value; }
		}

		private bool blnIsOverflow;
		public bool IsOverflow
		{
			get { return blnIsOverflow; }
		}

		public void Update(double timeElapsed)
		{
			if (blnIsOverflow)
				blnIsOverflow = false;

			this.dblAcumulatedTime += timeElapsed;
			if (dblAcumulatedTime > this.dblMaxTime)
			{
				dblAcumulatedTime = 0;
				this.blnIsOverflow = true;
			}
		}
	}
}
