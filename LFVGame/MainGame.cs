using System;
using System.Collections.Generic;
using System.Text;
using LFVGL;
using LFVMath.Basic;
using LFVGame;

namespace LFVGame
{
	public class MainGameEventArgs
	{
		public MainGameEventArgs(double dblDiferenceTime, MainGame mgGame)
		{
			this.dblDiferenceTime = dblDiferenceTime;
			this.mgGame = mgGame;
		}

		private double dblDiferenceTime;
		public double DiferenceTime
		{
			get { return dblDiferenceTime; }
		}

		private MainGame mgGame;
		public MainGame Game
		{
			get { return mgGame; }
		}
	}
	public delegate void MainGameEvent(MainGameEventArgs e);
	public delegate void MainGameDrawEvent(Vector2D position, MainGameEventArgs e);

	public class MainGame: Main
	{
		private IGameStage stage = new LFVGame.Stages.Presentation();
		public IGameStage Stage
		{
			get { return stage; }
			set { stage = value; }
		}

		#region Flags
		private bool createBullet = false;
		#endregion		

		private MainGameEventArgs ev;
		private MainGameEventArgs GetEventArgs()
		{
			return new MainGameEventArgs(this.GameTimer.ElapsedTime, this);
		}

		private void Lose()
		{
			//TODO:Chamar estagio de perda
		}

		private void Win()
		{
            //TODO:Chamar estagio de congratulações
		}
		
		#region Main Methods
		public override void UnloadComponents()
		{
			stage.UnloadComponents();
		}

		public override void LoadComponents()
		{
			stage.LoadComponents();
			Explosion.Initialize();			
		}

		public override void Redraw(double elapsedTime)
		{			
			this.stage.Redraw(elapsedTime);

			ev = this.GetEventArgs();
			if (this.BeforeRedraw != null)
				this.BeforeRedraw(ev);
			
			if (this.AfterRedraw != null)
				this.AfterRedraw(ev);
		}

		public override void Update(double elapsedTime)
		{
			if (this.stage.IsFinish)
			{
				IGameStage newStage = new LFVGame.Stages.StageOne();
				newStage.LoadComponents();
                this.stage.Dispose();
				this.stage = newStage;
			}
			this.stage.Update(elapsedTime);
		}

		public override void CheckGameInputs()
		{
			if (this.OnCheckGameInputs != null)
				this.OnCheckGameInputs(this.GetEventArgs());
			stage.CheckGameInputs();
		}

		public override void CheckMainInputs()
		{
			stage.CheckMainInputs();
		} 
		#endregion

		#region Events
		public event MainGameEvent BeforeRedraw;
		public event MainGameEvent AfterRedraw;
		public event MainGameEvent OnCheckGameInputs;
		public event MainGameEvent OnLose;
		public event MainGameEvent OnWin;
		#endregion

	}
}
