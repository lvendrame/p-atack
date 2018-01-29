using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace LFVGL
{
	public abstract class Main
	{
		private Time timer;

		public Time GameTimer
		{
			get { return timer; }
		}

		private Thread thGameThread;
		public Thread GameThread
		{
			get { return thGameThread; }
		}

		private object objParent;
		public object Parent
		{
			get { return objParent; }
			set { objParent = value; }
		}

		private bool isRunning = false;
		public bool IsRunning
		{
			get { return isRunning; }
			set { isRunning = value; }
		}

		private bool isLoading = false;

		public bool IsLoading
		{
			get { return isLoading; }
		}


		public void StartGame(object parent)
		{
			this.objParent = parent;
			thGameThread = new Thread(this.MainLoop);
			thGameThread.Start();
		}

		public void Reset()
		{
			this.isRunning = false;
			restart = true;
		}

		private void Initialize()
		{
			timer = new Time();
		}

		private bool restart = false;
		private double incDiference = 0.00;
		private void MainLoop()
		{
			this.isLoading = false;
			this.Initialize();
			this.LoadComponents();
			this.isRunning = true;
			this.isLoading = true;
			while (isRunning)
			{
				timer.Update();
				this.CheckMainInputs();
				incDiference += timer.ElapsedTime;				
				lock (objParent)
				{
					this.CheckGameInputs();
					this.Update(timer.ElapsedTime);
					if (incDiference > 0.04)
					{
						this.Redraw(timer.ElapsedTime);
						incDiference = 0;
					}
				}
			}
			this.UnloadComponents();
			GC.Collect();
		}

		public abstract void UnloadComponents();
		public abstract void LoadComponents();
		public abstract void Redraw(double elapsedTime);
		public abstract void Update(double elapsedTime);
		public abstract void CheckGameInputs();
		public abstract void CheckMainInputs();
	}
}
