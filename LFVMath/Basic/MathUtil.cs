using System;
using System.Collections.Generic;
using System.Text;

namespace LFVMath.Basic
{
	static class MathUtil
	{

		#region Get Areas
		public static double GetSquareArea(double vertice)
		{
			return vertice * vertice;
		}

		public static double GetCircleArea(double radius)
		{
			return Constants.PI * radius * radius;
		}

		public static double GetRectangleArea(double p_base, double heigth)
		{
			return p_base * heigth;
		}

		public static double GetTriangleArea(double p_base, double heigth)
		{
			return (p_base * heigth) / 2;
		}

		public static double GetTrapeziumArea(double higherBase, double smallerBase, double heigth)
		{
			return ((higherBase + smallerBase) / 2) * heigth;
		} 
		#endregion

		#region Get Volumes

		public static double GetCylinderVolume(double heigth, double radius)
		{
			return GetCircleArea(radius) * heigth;
		}

		#endregion

		public static class Constants
		{
			public const double PI = 3.1415927;
			public const double TwoPowerPI = 9.8696044;
			public const double TwoPI = 6.2831853;
			public const double E = 0;
			public const double OneAngleInRadian = 0.0174533;
		}
	}
}
