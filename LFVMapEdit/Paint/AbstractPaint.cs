using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using LFVMapControler;

namespace LFVMapEdit.Paint
{
	public abstract class AbstractPaint: IPaint
	{
		public static Paint Initialize<Paint>(PictureMap ppcm_Map) where Paint: IPaint, new()
		{
			Paint paint = new Paint();
			paint.Map = ppcm_Map;
			return paint;
		}

		#region IPaint Members

		private PictureMap fpcp_Map;
		public PictureMap Map
		{
			get
			{
				return fpcp_Map;
			}
			set
			{
				fpcp_Map = value;
			}
		}

		public abstract void MouseLeftDraw(int x, int y, Brick pbrk_NewBrick);

		public abstract void MouseRightDraw(int x, int y, Brick pbrk_NewBrick);

		public abstract void MouseLeftMoveDraw(int x, int y, Brick pbrk_NewBrick);

		public abstract void MouseRightMoveDraw(int x, int y, Brick pbrk_NewBrick);

		public virtual void MouseMoveDraw(int x, int y, Brick pbrk_NewBrick)
		{
		}

		public virtual void MouseEventDraw(System.Windows.Forms.MouseEventArgs e, bool move, Brick pbrk_NewBrick)
		{
			int x = fpcp_Map.GetMapIndexX(e.X);
			int y = fpcp_Map.GetMapIndexY(e.Y);
			if (Util.IsValidPoint(this.fpcp_Map, x, y))
			{
				if (move)
				{
					switch (e.Button)
					{
						case MouseButtons.Left:
							this.MouseLeftMoveDraw(x, y, pbrk_NewBrick);
							fpcp_Map.Invalidate();
							break;
						case MouseButtons.Right:
							this.MouseRightMoveDraw(x, y, pbrk_NewBrick);
							fpcp_Map.Invalidate();
							break;
						case MouseButtons.None:
							this.MouseMoveDraw(x, y, pbrk_NewBrick);
							fpcp_Map.Invalidate();
							break;
					}
				}
				else
				{
					switch (e.Button)
					{
						case MouseButtons.Left:
							this.MouseLeftDraw(x, y, pbrk_NewBrick);
							fpcp_Map.Invalidate();
							break;
						case MouseButtons.Right:
							this.MouseRightDraw(x, y, pbrk_NewBrick);
							fpcp_Map.Invalidate();
							break;
					}
				}
			}
		}

		#endregion
	}
}
