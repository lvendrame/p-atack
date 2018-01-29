using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using LFVMapControler;

namespace LFVMapEdit.Paint
{
	public interface IPaint
	{
		PictureMap Map
		{
			get;
			set;
		}

		void MouseLeftDraw(int x, int y, Brick pbrk_NewBrick);
		void MouseRightDraw(int x, int y, Brick pbrk_NewBrick);
		void MouseLeftMoveDraw(int x, int y, Brick pbrk_NewBrick);
		void MouseRightMoveDraw(int x, int y, Brick pbrk_NewBrick);
		void MouseMoveDraw(int x, int y, Brick pbrk_NewBrick);
		void MouseEventDraw(MouseEventArgs e, bool move, Brick pbrk_NewBrick);
	}
}
