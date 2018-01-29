using System;
using System.Collections.Generic;
using System.Text;
using LFVMapControler;

namespace LFVMapEdit.Paint
{
	public class FillFloodPaint: AbstractPaint
	{
		public override void MouseLeftDraw(int x, int y, Brick pbrk_NewBrick)
		{
			FloodFill(pbrk_NewBrick, x, y);
		}

		public override void MouseRightDraw(int x, int y, Brick pbrk_NewBrick)
		{
		}

		public override void MouseLeftMoveDraw(int x, int y, Brick pbrk_NewBrick)
		{
		}

		public override void MouseRightMoveDraw(int x, int y, Brick pbrk_NewBrick)
		{
		}

		public void FloodFill(Brick fillBrick, int x, int y)
		{
			if (x < 0 || x >= Map.QtdColumns || y < 0 || y >= Map.QtdRows) return;

			Brick oldBrick = null;
			if(Map.Cells[x, y] != null)
				oldBrick =  Map.Cells[x, y].Brick;

			// Checks trivial case where loc is of the fill color
			if (IsEqualBrick(fillBrick, oldBrick)) return;

			FloodLoop(x, y, fillBrick, oldBrick);
		}

		// Recursively fills surrounding pixels of the old color
		private void FloodLoop(int x, int y, Brick fillBrick, Brick oldBrick)
		{
			// finds the left side, fillBricking along the way
			int fillL = x;
			do
			{
				Map.Cells[fillL, y] = Util.GetMapCell(fillBrick);
				fillL--;
			} while (fillL >= 0 && IsEqualBrick(Map.Cells[fillL, y], oldBrick));
			fillL++;

			// find the right right side, fillBricking along the way
			int fillR = x;
			do
			{
				Map.Cells[fillR, y] = Util.GetMapCell(fillBrick);
				fillR++;
			} while (fillR < Map.QtdColumns && IsEqualBrick(Map.Cells[fillR, y], oldBrick));
			fillR--;

			// checks if applicable up or down
			for (int i = fillL; i <= fillR; i++)
			{
				if (y > 0 && IsEqualBrick(Map.Cells[i, y - 1], oldBrick))
					FloodLoop(i, y - 1, fillBrick, oldBrick);

				if (y < Map.QtdRows - 1 && IsEqualBrick(Map.Cells[i, y + 1], oldBrick))
					FloodLoop(i, y + 1, fillBrick, oldBrick);
			}
		}

		private bool IsEqualBrick(Brick pbrk1, Brick pbrk2)
		{
			return pbrk1 == pbrk2;
		}

		private bool IsEqualBrick(MapCell cell, Brick pbrk2)
		{
			if (cell == null && pbrk2 == null)
				return true;
			else if (cell == null)
				return false;
			else
				return IsEqualBrick(cell.Brick, pbrk2);
		}
	}
}
