using System;
using System.Collections.Generic;
using System.Text;

namespace LFVMath.Phisics
{
	public abstract class AbstractColidable: AbstractMoveble
	{
		#region IColidable Members

        public double Heigth;
        public double Width;

        public virtual bool Colide(AbstractColidable objOp)
		{
			return UtilPhisics.Colide(this, objOp);
		}

		public virtual bool Colide(ICircleColidable objOp)
		{
			return UtilPhisics.Colide(objOp, this);
		}

		#endregion

        public override void Dispose()
        {
            base.Dispose();
            GC.SuppressFinalize(this);
        }
	}
}
