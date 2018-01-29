using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace LFVGame
{
	public class Tile: Combatant
	{

		private bool blnIsSolid;
		public bool IsSolid
		{
			get { return blnIsSolid; }
			set { blnIsSolid = value; }
		}

		private int intIndex;
		public int Index
		{
			get { return intIndex; }
			set { intIndex = value; }
		}

		public Image TileImage
		{
			get { return StaticImages.Bricks[this.intIndex]; }
		}

        public override void Dispose()
        {            
            base.Dispose();
        }
	}
}
