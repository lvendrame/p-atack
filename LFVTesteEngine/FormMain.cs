using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LFVMath.Phisics;
using System.Threading;
using LFVMath.Basic;
using LFVGL;
using LFVGame;
using LFVGame.Stages;

namespace LFVTesteEngine
{
	public partial class FormMain : Form
	{	
		public FormMain()
		{
			InitializeComponent();            

			pictureBox1.Resize += new EventHandler(pictureBox1_Resize);
			this.Resize += new EventHandler(pictureBox1_Resize);
			pictureBox1.Paint += new PaintEventHandler(pictureBox1_Paint);		
			aspX = (float)this.pictureBox1.Width / 800f;
            aspY = (float)this.pictureBox1.Height / 600f;
            drX = (int)(800 / aspX);
            drY = (int)(600 / aspY);
			this.Disposed += new EventHandler(FormMain_Disposed);
			this.InitializeMainGame();
			while (!game.IsLoading)
			{
				Thread.Sleep(100); 
			}
			Thread.Sleep(100);
			pictureBox1.Refresh();
			game.Stage.State = StageState.Running;
		}

		void FormMain_Disposed(object sender, EventArgs e)
		{
			game.IsRunning = false;
			Thread.Sleep(1000);
			if(game.GameThread != null)
				game.GameThread.Abort();
		}

		private double GetXWithAspect(int x)
		{
			float ret = aspX * (float)x;
			return (double)ret;
		}
		private double GetYWithAspect(int y)
		{
			float ret = aspY * (float)y;
			return (double)ret;
		}

		float aspX = 1;
		float aspY = 1;
		void pictureBox1_Resize(object sender, EventArgs e)
		{
			aspX = 800f / (float)this.pictureBox1.Width;
			aspY = 600f / (float)this.pictureBox1.Height;
            drX = (int)(800 / aspX);
            drY = (int)(600 / aspY);
			pictureBox1.Refresh();
		}


		Font fontLive = new Font("arial", 12, FontStyle.Bold, GraphicsUnit.Pixel);

        int drX = 0;
        int drY = 0;
		void pictureBox1_Paint(object sender, PaintEventArgs e)
		{
			try
			{
				if(game.Stage.StageImage != null)
					e.Graphics.DrawImage(game.Stage.StageImage, 0, 0, drX, drY);
				GenericStage stage = game.Stage as GenericStage;
				if (stage != null)
				{
					e.Graphics.FillRectangle(Brushes.Snow, 0, 0, 100, 40);
					e.Graphics.DrawString("Energia: " + stage.User.Energy.ToString(), fontLive, Brushes.Black, 5f, 5f);
					e.Graphics.DrawString("Inimigos: " + stage.Enemies.Count.ToString(), fontLive, Brushes.Black, 5f, 20f);
					e.Graphics.DrawString("Itens: " + stage.Items.Count.ToString(), fontLive, Brushes.Black, 5f, 35f);
				}
				
				if(game.Stage.IsPaused)
					e.Graphics.DrawString("PAUSED", fontLive, Brushes.BlueViolet, 390f, 290f);
			}
			catch (Exception ex)
			{
				throw ex;
			}			
			
		}

		#region MainGame

		MainGame game = null;
		private void InitializeMainGame()
		{
			game = new MainGame();
			game.OnCheckGameInputs += new MainGameEvent(game_OnCheckGameInputs);

			game.BeforeRedraw += new MainGameEvent(game_BeforeRedraw);
            if(game.Stage != null)
			    game.Stage.State = StageState.Loading;
			game.StartGame(pictureBox1);
		}

		void game_BeforeRedraw(MainGameEventArgs e)
		{
			pictureBox1.Refresh();
		}

		void game_OnCheckGameInputs(MainGameEventArgs e)
		{
			WinAPIUtil.POINT pt = WinAPIUtil.GetCursorPos();
			GenericStage stage = e.Game.Stage as GenericStage;
			if (stage != null)
			{
				stage.NextUserPosition = new Vector2D(
					this.GetXWithAspect(pt.x - this.Left),
					stage.Position.Y + this.GetYWithAspect(pt.y - (this.Top+20)));
			}
		}



		#endregion
	}
}