using System;
using System.Collections.Generic;
using System.Text;
using LFVMath.Basic;
using LFVGame.Bullets;

namespace LFVGame.Ships
{
	public class UserSpaceShip: SpaceShip
	{
		public UserSpaceShip()
		{
            this.Position = new Vector2D(0, 0);
			this.SpriteImage = Resource.Nave2;
			this.Heigth = this.SpriteImage.Height;
			this.Width = this.SpriteImage.Width;
			this.Energy = this.MaxEnergy = 20.00;
			this.Force = 2;
            this.Lifes = 5;

            this.AddPosBullet = this.Width / 2;

            BulletSettings.Force = 1;
            BulletSettings.Increment = new Vector2D(20, 15);
            BulletSettings.InitialVelocity = new Vector2D(0, -200);
            BulletSettings.IsParalel = false;
            BulletSettings.Quantity = 4;
            BulletSettings.Type = BulletType.PrimaryUser;
            BulletSettings.Interval = 0.25;

            TimeAcummulator.MaxTime = BulletSettings.Interval;            
			this.InitializeShips();
		}

		/*public override void Draw(System.Drawing.Graphics grPaint, Vector2D pv2d_MapPosition)
		{
			imgIndex++;
			if (imgIndex >= 10)
				imgIndex = 0;

			this.SpriteImage = StaticImages.Sprites[this.imgStaticIndex[imgIndex]];
			base.Draw(grPaint, pv2d_MapPosition);
		}*/

		private void InitializeShips()
		{
			for (int i = 0; i < 300; i+=30)
			{
				imgStaticIndex.Add(StaticImages.AddSpriteFromGrid(i, 0, 30, 34, Resource.NaveRoxaMove));
			}
		}

		int imgIndex = 0;
		List<int> imgStaticIndex = new List<int>();

        public override void Dispose()
        {
            imgStaticIndex.Clear();
            GC.SuppressFinalize(imgStaticIndex);
            base.Dispose();
        }
	}
}
