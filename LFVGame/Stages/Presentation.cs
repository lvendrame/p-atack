using System;
using System.Collections.Generic;
using System.Text;
using LFVGL;
using System.Drawing;

namespace LFVGame.Stages
{
	public class Presentation: Drawable
	{
		public Presentation()
		{
		}

		public override void LoadComponents()
		{
			base.LoadComponents();
		}

        private Image logoImage = Resource.logo2;

		TimeAcumulator taAcumulator = new TimeAcumulator(0.35);
		
		public override void Update(double elapsedTime)
		{			
			base.Update(elapsedTime);

			taAcumulator.Update(elapsedTime);
			if (taAcumulator.IsOverflow)
				brushDraw = brushDraw == Brushes.Red ? Brushes.Blue : Brushes.Red;
		}

		short startStage = WinAPIUtil.GetKeyState(13);
		public override void CheckMainInputs()
		{
			base.CheckMainInputs();			

			if (WinAPIUtil.GetKeyState(13) != startStage)
				this.State = StageState.Finished;
            if (WinAPIUtil.GetKeyState(27) < 0)
            {
                this.State = StageState.ExitGame;
                brushDraw = Brushes.Coral;                
            }
		}

		Brush brushDraw = Brushes.Red;
		Font fontDraw = new Font("Arial", 20f, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Pixel);
		public override void Redraw(double elapsedTime)
		{
            base.Redraw(elapsedTime);

            Image img = new Bitmap(800, 600, System.Drawing.Imaging.PixelFormat.Format32bppRgb);			
            Graphics gr = Graphics.FromImage(img);
            gr.DrawImage(logoImage, 0, -30, 800, 600);
			gr.DrawString("Press ENTER to start", fontDraw, brushDraw, 290, 470);
            gr.DrawString("Press ESC to exit", fontDraw, brushDraw, 305, 500);
            gr.Save();

            this.imgStageImage = img;
		}		
	}
}
