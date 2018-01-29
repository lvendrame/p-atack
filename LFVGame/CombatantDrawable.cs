using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using LFVMath.Basic;

namespace LFVGame
{
	public class CombatantDrawable: Combatant, IDrawable
	{
		#region IDrawable Members

		private bool blnIsVisible = true;
		public bool IsVisible
		{
			get
			{
				return blnIsVisible;
			}
			set
			{
				blnIsVisible = value;
			}
		}

		Image imgSpriteImage;
		public Image SpriteImage
		{
			get
			{
				return imgSpriteImage;
			}
			set
			{
				imgSpriteImage = value;
			}
		}

		public virtual void Draw(Graphics grPaint, Vector2D pv2d_MapPosition)
		{
			if(this.blnIsVisible)
                grPaint.DrawImage(imgSpriteImage, (float)this.Position.X, (float)(this.Position.Y - pv2d_MapPosition.Y));
		}

		#endregion

        public override void Dispose()
        {
            this.imgSpriteImage = null;
            base.Dispose();
        }
	}
}
