using System;
using System.Collections.Generic;
using System.Text;

namespace LFVMath.Phisics
{
	public static class UtilPhisics
	{
        public static bool Colide(AbstractColidable objA, AbstractColidable objB)
		{
			if((objA.Position.X + objA.Width) < objB.Position.X)
				return false;
			if(objA.Position.X > (objB.Position.X + objB.Width))
				return false;
			if((objA.Position.Y + objA.Heigth) < objB.Position.Y)
				return false;
			if(objA.Position.Y > (objB.Position.Y + objB.Heigth))
				return false;

			return true;
		}

		public static bool Colide(ICircleColidable objA, ICircleColidable objB)
		{
			double x = objA.Position.X - objB.Position.X;
			double y = objA.Position.Y - objB.Position.Y;
            double radius = objA.Radius + objB.Radius;
			return(x * x + y * y) < (radius*radius);
		}

        public static bool Colide(ICircleColidable objA, AbstractColidable objB)
		{
			throw new Exception("Ainda tem que implementar");
		}
	}
}
