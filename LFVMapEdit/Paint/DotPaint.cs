using System;
using System.Collections.Generic;
using System.Text;
using LFVMapControler;

namespace LFVMapEdit.Paint
{
	public class DotPaint: AbstractPaint
	{
		int lastX = -1;
		int lastY = -1;

		public override void MouseLeftDraw(int x, int y, Brick pbrk_NewBrick)
		{
            if (x != lastX || y != lastY)
            {
                lastX = x;
                lastY = y;
                for (int i = 0; i < this.Map.GridBrick.QtdColumns; i++)
                {
                    y = lastY;
                    for (int j = 0; j < this.Map.GridBrick.QtdRows; j++)
                    {
                        if (x < Map.Cells.Columns && y < Map.Cells.Rows)
                        {
                            Map.Cells[x, y] = Util.GetMapCell(this.Map.GridBrick.Images[i][j]);
                        }
                        y++;
                    }
                    x++;
                }
            }
		}

		public override void MouseRightDraw(int x, int y, Brick pbrk_NewBrick)
		{
			if (x != lastX || y != lastY)
			{
				Map.Cells[x, y] = null;
				lastX = x;
				lastY = y;
			}
		}

		public override void MouseLeftMoveDraw(int x, int y, Brick pbrk_NewBrick)
		{
			MouseLeftDraw(x, y, pbrk_NewBrick);			
		}

		public override void MouseRightMoveDraw(int x, int y, Brick pbrk_NewBrick)
		{
			MouseRightDraw(x, y, pbrk_NewBrick);		
		}
	}
}
