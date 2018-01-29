using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using LFVMath.Basic;

namespace LFVGame
{
	public interface IDrawable
	{
		bool IsVisible
		{
			get;
			set;
		}

		Image SpriteImage
		{
			get;
			set;
		}

		void Draw(Graphics grPaint, Vector2D pv2d_MapPosition);
	}
}
