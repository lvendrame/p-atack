using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using LFVMath.Phisics;
using LFVMath.Basic;
using LFVGame.Bullets;

namespace LFVGame.Ships
{
	public class SpaceShip: CombatantDrawable
	{
		public SpaceShip()
		{			
		}

        private double dblAddPosBullet;

        public double AddPosBullet
        {
            get { return dblAddPosBullet; }
            set { dblAddPosBullet = value; }
        }


        public BulletSettings BulletSettings;
        public TimeAcumulator TimeAcummulator;

		protected ExplosionType enmExplosionType;
		public ExplosionType ExplosionType
		{
			get { return enmExplosionType; }
		}

        public override void Update(double timeElapsed)
        {
            TimeAcummulator.Update(timeElapsed);
            base.Update(timeElapsed);
        }
	}	
}
