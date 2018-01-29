using System;
using System.Collections.Generic;
using System.Text;
using LFVMapControler;

namespace LFVMapEdit.Paint
{
	public static class Util
	{
		public static bool IsValidPoint(PictureMap ppcm_Map, int x, int y)
		{
			if (x < 0)
				return false;
			if (y < 0)
				return false;
			if (x >= ppcm_Map.QtdColumns)
				return false;
			if (y >= ppcm_Map.QtdRows)
				return false;
			return true;
		}

		public static MapCell GetMapCell(Brick pbrk_Brick)
		{
			if (pbrk_Brick == null)
				return null;
			else
				return new MapCell(pbrk_Brick);
		}
	}
}
