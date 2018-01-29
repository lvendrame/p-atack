using System;
using System.Collections.Generic;
using System.Text;
using LFVMath.Basic;
using LFVGame.Items;
using LFVGame.Stages;
using LFVGame.Bullets;

namespace LFVGame.Ships
{
	public class EnemySpaceShip: SpaceShip
	{
		public EnemySpaceShip(Vector2D position)
		{
            this.Position = position;
            this.SpriteImage = new System.Drawing.Bitmap(Resource.Nave3D_1_pq);//StaticImages.Sprites[GameSprites.AirPlane_Yellow.GetSpriteIndex(SpritePositionEnum.DOWN, ObjectStatus.Normal)];
            this.SpriteImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
			this.Heigth = this.SpriteImage.Height;
			this.Width = this.SpriteImage.Width;
			this.Energy = 2.00;
            this.Lifes = 5;
			this.Force = 3;
            this.Velocity.Y = 50;
            this.Velocity.X = 10;

            this.AddPosBullet = this.Width / 2;

            this.BulletSettings.InitialVelocity.Y = 200;
            this.BulletSettings.Type = BulletType.PrimaryEnemy;
            this.BulletSettings.Quantity = 1;
            this.BulletSettings.Force = 3;
            this.BulletSettings.Interval = 0.50;
            this.BulletSettings.NominalVelocity = 200;

            TimeAcummulator.MaxTime = this.BulletSettings.Interval;
		}

		public bool HasItem
		{
            get { return itbItem != null; }
		}

		private Item itbItem = null;
		public Item Item
		{
			get { return itbItem; }
			set { itbItem = value; }
		}
				
		public CombatStatus Update(SpaceShip user, GenericStage stage, double timeElapsed)
		{
			SpaceShip enemy = this;
			this.Update(timeElapsed);

			if (this.Position.X >= 800)
			{
                this.Velocity.X = -this.Velocity.X;
                this.Position.Y += 34;
			}
			else if (this.Position.X <= 0)
			{
                this.Velocity.X = -this.Velocity.X;
                this.Position.Y += 34;
			}
			if (this.Position.Y >= (stage.Position.Y + 600))
			{
                this.Velocity.Y = -this.Velocity.Y;
				this.BulletSettings.InitialVelocity.Y = -this.BulletSettings.InitialVelocity.Y;
			}
			else if (this.Position.Y >= (stage.Position.Y + 600))
			{
                this.Velocity.Y = -this.Velocity.Y;
                this.BulletSettings.InitialVelocity.Y = -this.BulletSettings.InitialVelocity.Y;
			}
			if (this.Position.Y >= (stage.Position.Y + 800))
			{
                this.Velocity.Y = -this.Velocity.Y;
                this.BulletSettings.InitialVelocity.Y = -this.BulletSettings.InitialVelocity.Y;
				return CombatStatus.MeDie;
			}
			if (this.Position.Y <= stage.Position.Y)
			{
                this.Velocity.Y = -this.Velocity.Y;
                this.BulletSettings.InitialVelocity.Y = -this.BulletSettings.InitialVelocity.Y;
			}

			if (this.Position.Y > stage.Position.Y && this.Position.Y < (stage.Position.Y + 630))
			{
				if (TimeAcummulator.IsOverflow)
				{
                    this.BulletSettings.InitialVelocity = user.Position - this.Position;
                    //this.BulletSettings.InitialVelocity = this.BulletSettings.InitialVelocity.GetNormal() * this.BulletSettings.NominalVelocity;
                    this.BulletSettings.InitialVelocity = this.BulletSettings.InitialVelocity.GetMultNormal(this.BulletSettings.NominalVelocity);
                    Vector2D posB;
                    posB.X = this.Position.X + this.AddPosBullet;
                    posB.Y = this.Position.Y;
                    this.BulletSettings.AddBullets(stage.EnemiesBullets, posB, stage, false);
				}
			}

			if(this.HasItem)
                this.Item.Position = this.Position;

			CombatStatus statusUser = user.VerifyCombat(this);
			if (statusUser != CombatStatus.None)
			{
				stage.Explosions.Add(new Explosion(user.Position, ExplosionType.Two));
			}
			return statusUser;
		}

        public override void Dispose()
        {
            itbItem = null;
            base.Dispose();
        }
	}
}
