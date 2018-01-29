using System;
using System.Collections.Generic;
using System.Text;
using LFVMath.Phisics;
using LFVMath.Basic;
using System.Drawing;
using LFVGame.Ships;
using LFVGame.Stages;

namespace LFVGame.Bullets
{
	public class Bullet: CombatantDrawable
	{
		#region Constructor		

		public Bullet(Vector2D position, BulletType type, double force, Vector2D velocity, GenericStage stage, bool isUserBullet)
		{
            this.Position = position;
			this.emnType = type;
			this.Force = force;
			this.Velocity = velocity;

			this.InitializeBullet(stage, isUserBullet);
			this.Heigth = this.SpriteImage.Height;
			this.Width = this.SpriteImage.Width;
		}
        
		#endregion

		protected virtual void InitializeBullet(GenericStage stage, bool isUserBullet)
		{
			switch (this.Type)
			{
				case BulletType.PrimaryUser:
					this.SpriteImage = Resource.bullet01;
					break;
				case BulletType.PrimaryEnemy:
					this.SpriteImage = Resource.bullet02;
					break;
				case BulletType.Laser:
					this.SpriteImage = Resource.Bullet03;
					break;
				case BulletType.AlphaStar:
					this.SpriteImage = Resource.bullet01;
					break;
				default:
					this.SpriteImage = Resource.bullet01;
					break;
			}
		}

		public override void Update(double timeElapsed)
		{			
			base.Update(timeElapsed);
		}
		
		private BulletType emnType;
		public BulletType Type
		{
			get { return emnType; }
		}		
	}

	public enum BulletType
	{
		PrimaryUser,
		Laser,
		PrimaryEnemy,
		AlphaStar
	}
}
