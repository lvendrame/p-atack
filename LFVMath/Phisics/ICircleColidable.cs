using System;
using System.Collections.Generic;
using System.Text;
using LFVMath.Basic;

namespace LFVMath.Phisics
{
	public interface ICircleColidable
	{
		double Radius
		{
			get;
			set;
		}
				
		Vector2D Position
		{
			get;
			set;
		}

		bool Colide(AbstractColidable objOp);

		bool Colide(ICircleColidable objOp);

	}
}
