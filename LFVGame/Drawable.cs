using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace LFVGame
{
	public class Drawable: IGameStage, IDisposable
	{
        #region IGameStage Members

        private StageState enmState;
        public StageState State
        {
            get { return enmState; }
            set { enmState = value; }
        }
        
		public bool IsLoad
		{
			get { return this.enmState != StageState.Loading; }
		}

		public bool IsFinish
		{
            get { return this.enmState == StageState.Finished; }
		}

		bool blnIsDrawing = false;
		public bool IsDrawing
		{
			get { return blnIsDrawing; }
			set { blnIsDrawing = true; }
		}

		public bool IsPaused
		{
			get { return this.enmState == StageState.Pause; }
		}

		public bool IsStopedMove
		{
			get { return this.enmState == StageState.Stop; }
		}

		public bool LockUser
		{
			get { return true; }
		}

        public bool ExitGame
        {
            get { return this.enmState == StageState.ExitGame; }
        }

		protected Image imgStageImage;
		public Image StageImage
		{
			get { return imgStageImage; }
		}

		public virtual void CheckGameInputs()
		{
			
		}

		public virtual void CheckMainInputs()
		{
			
		}

		public virtual void LoadComponents()
		{
			
		}

		public virtual void Redraw(double elapsedTime)
		{
			
		}

		public virtual void UnloadComponents()
		{
			
		}

		public virtual void Update(double elapsedTime)
		{
			
		}

		#endregion

        #region IDisposable Members

        public void Dispose()
        {
            this.imgStageImage = null;
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
