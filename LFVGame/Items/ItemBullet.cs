using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using LFVMath.Basic;
using LFVGame.Bullets;
using LFVGame.Ships;

namespace LFVGame.Items
{
	public class ItemBullet: Item
	{
		public ItemBullet()
		{
		}

		public ItemBullet(BulletType type, double force, Vector2D velocity, Vector2D increment, bool isParalel, int quantity, double interval)
		{
			this.BulletSettings.Type = type;
            this.BulletSettings.Force = force;
            this.BulletSettings.InitialVelocity = velocity;
            this.BulletSettings.Increment = increment;
            this.BulletSettings.IsParalel = isParalel;
            this.BulletSettings.Quantity = quantity;
            this.BulletSettings.Interval = interval;

			this.Width = 16;
			this.Heigth = 16;
            this.Initialize();
		}

        public BulletSettings BulletSettings;
        
        public void Initialize()
        {            
            switch (this.BulletSettings.Type)
            {
                case BulletType.PrimaryUser:
                case BulletType.PrimaryEnemy:
                case BulletType.AlphaStar:
                case BulletType.Laser:
                    this.SpriteImage = StaticImages.Sprites[0];
                    break;
            }
        }

		public override void ApplyChanges(double miliseconds, UserSpaceShip ssUser)
		{
			ssUser.BulletSettings = this.BulletSettings;
            ssUser.TimeAcummulator.MaxTime = this.BulletSettings.Interval;
		}
	}
}
