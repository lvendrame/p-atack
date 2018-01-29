using System;
using System.Collections.Generic;
using System.Text;

namespace LFVMapControler
{
	public class MapCell
	{
		public MapCell(Brick pbrk_Brick)
		{
			this.fbrk_Brick = pbrk_Brick;
		}		

		private Brick fbrk_Brick;
		public Brick Brick
		{
			get { return fbrk_Brick; }
		}
	}
}
