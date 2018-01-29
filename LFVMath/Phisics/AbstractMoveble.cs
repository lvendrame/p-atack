using System;
using System.Collections.Generic;
using System.Text;

namespace LFVMath.Phisics
{
	public abstract class AbstractMoveble: IDisposable
	{
		#region IMoveble Members

		public LFVMath.Basic.Vector2D Velocity;
        public LFVMath.Basic.Vector2D Position;

        public virtual void Update(double timeElapsed)
		{
            this.Position.X += this.Velocity.X * timeElapsed;
            this.Position.Y += this.Velocity.Y * timeElapsed;
		}

        public virtual void UpdateX(double timeElapsed)
		{
            this.Position.X += this.Velocity.X * timeElapsed;
		}

        public virtual void UpdateY(double timeElapsed)
		{
            this.Position.Y += this.Velocity.Y * timeElapsed;
		}

		#endregion

        #region IDisposable Members

        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
