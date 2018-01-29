using System;
using System.Collections.Generic;
using System.Text;
using LFVMath.Phisics;
using LFVMath.Basic;
using System.Drawing;

namespace LFVGame
{
	public class Explosion: AbstractColidableDrawable
	{
		private int intTime = 0;
		public int Time
		{
			get { return intTime; }
		}
		
		private static System.Media.SoundPlayer soundOne = new System.Media.SoundPlayer(Resource.explosion1);
		
		public static void Initialize()
		{
			soundOne.Load();
			while (!soundOne.IsLoadCompleted)
				System.Threading.Thread.Sleep(100);
		}

		public Explosion(Vector2D position, ExplosionType type)
		{
			this.PlaySound(type);
            this.Position = position;
            this.anim = new Animation(new Image[]
                {
                    StaticImages.Sprites[GameSprites.ExplosionSprites.StageOne],
                    StaticImages.Sprites[GameSprites.ExplosionSprites.StageTwo],
                    StaticImages.Sprites[GameSprites.ExplosionSprites.StageThree],
                    StaticImages.Sprites[GameSprites.ExplosionSprites.StageFour],
                    StaticImages.Sprites[GameSprites.ExplosionSprites.StageFive],
                    StaticImages.Sprites[GameSprites.ExplosionSprites.StageSix]
                }, 0.1);
		}

		private void PlaySound(ExplosionType type)
		{
			switch (type)
			{
				case ExplosionType.One:
					soundOne.Play();
					break;
				case ExplosionType.Two:
					soundOne.Play();
					break;
			}
		}

		public override void Draw(Graphics grPaint, Vector2D pv2d_MapPosition)
		{
			//this.SpriteImage = StaticImages.Sprites[GameSprites.ExplosionSprites.GetIndex(this.intTime)]; 
			base.Draw(grPaint, pv2d_MapPosition);            
		}
		
		public void Update(double diference)
		{
            this.SpriteImage = anim.GetImage(diference);
			this.intTime++;
		}

		public bool Destroy()
		{
			return this.anim.CurrentIndex == 5;
		}

        Animation anim;

	}

	public enum ExplosionType
	{
		One,
		Two
	}
}
