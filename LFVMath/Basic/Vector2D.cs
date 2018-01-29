using System;
using System.Collections.Generic;
using System.Text;

namespace LFVMath.Basic
{
	public struct Vector2D
	{
		public Vector2D(double x, double y)
		{
			this.X = x;
			this.Y = y;
		}

        public static Vector2D FromAngle(double angle, double length)
        {
            return new Vector2D(Math.Cos(angle) * length,
                                Math.Sin(angle) * length);
        }

		#region Properties
		public double X;
		public double Y;

        public bool IsZero
        {
            get
            {
                return X == 0 && Y == 0;
            }
        }
		#endregion

		#region Methods

        public double GetAngle()
        {
            return Math.Atan2(this.Y, this.X);
        }

		public double GetPythagoreanDistance(Vector2D vecTwo)
		{
			double numX = this.X - vecTwo.X;
			double numY = this.Y - vecTwo.Y;
			if (numX == 0)
				return Math.Abs(numY);
			else if (numY == 0)
				return Math.Abs(numX);
			else
				return
					Math.Sqrt(
						numX*numX +
						numY*numY
					);
        }

		public double GetMagnitude()
		{
            return Math.Sqrt(this.X * this.X + this.Y * this.Y);
		}

		public Vector2D GetNormal()
		{
			double magnitude = this.GetMagnitude();
			return new Vector2D(this.X / magnitude, this.Y / magnitude);
		}

        public Vector2D GetMultNormal(double dblMult)
        {
            dblMult /= this.GetMagnitude();
            return new Vector2D(this.X * dblMult, this.Y * dblMult);
        }

        public Vector2D GetVersor()
        {
            return this / this.GetNormal();
        }
        
        public double GetScalar(Vector2D vc2)
        {
            Vector2D vc = this * vc2;
            return vc.X + vc.Y;
        }

        public double GetAngleBetween(Vector2D vc2)
        {            
            return Math.Acos(this.GetScalar(vc2) / (this.GetMagnitude() * vc2.GetMagnitude()));
            //return this.Y * vc2.X > this.X * vc2.Y ? -angle : angle;
        }

		public static Vector2D operator*(Vector2D vecOne, Vector2D vecTwo)
		{
			return new Vector2D(vecOne.X * vecTwo.X, vecOne.Y * vecTwo.Y);
		}

		public static Vector2D operator*(Vector2D vecOne, double value)
		{
			return new Vector2D(vecOne.X * value, vecOne.Y * value);
		}

		public static Vector2D operator*(double value, Vector2D vecOne)
		{
			return new Vector2D(vecOne.X * value, vecOne.Y * value);
        }

        public static Vector2D operator /(Vector2D vecOne, Vector2D vecTwo)
        {
            return new Vector2D(vecOne.X / vecTwo.X, vecOne.Y / vecTwo.Y);
        }

        public static Vector2D operator /(Vector2D vecOne, double value)
        {
            return new Vector2D(vecOne.X / value, vecOne.Y / value);
        }

		public static Vector2D operator +(Vector2D vecOne, Vector2D vecTwo)
		{
			return new Vector2D(vecOne.X + vecTwo.X, vecOne.Y + vecTwo.Y);
		}

		public static Vector2D operator -(Vector2D vecOne, Vector2D vecTwo)
		{
			return new Vector2D(vecOne.X - vecTwo.X, vecOne.Y - vecTwo.Y);
		}
		
		#endregion

	}
}
