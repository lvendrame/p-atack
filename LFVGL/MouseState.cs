using System;
using System.Collections.Generic;
using System.Text;

namespace LFVGL
{
	public class MouseState
	{

		public MouseState()
		{
			this.intX = System.Windows.Forms.Control.MousePosition.X;
			this.intY = System.Windows.Forms.Control.MousePosition.Y;
		}
		
		private int intX;
		public int X
		{
			get { return intX; }
		}

		private int intY;
		public int Y
		{
			get { return intY; }
		}


		
	}
}
