using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using LFVMath.Basic;

namespace LFVGame
{
	public static class StaticImages
	{
		private static List<Image> lstBricks = InitializeBricks();
		public static List<Image> Bricks
		{
			get { return lstBricks; }
		}

		private static List<Image> lstSprites = InitializeSprites();
		public static List<Image> Sprites
		{
			get { return lstSprites; }
		}

		public static List<Image> InitializeBricks()
		{
			List<Image> lst = new List<Image>();

			lst.Add(Resource.brick01);
			lst.Add(Resource.brick02);
			lst.Add(Resource.brick03);
			lst.Add(Resource.brick04);
			lst.Add(Resource.brick05);

			return lst;
		}

		public static List<Image> InitializeSprites()
		{
			List<Image> lst = new List<Image>();

			lst.Add(Resource.NaveBolha);
			lst.Add(Resource.NaveRoxa);

			return lst;
		}

		public static int AddBrickFromGrid(int x, int y, int width, int height, Image imgGrid)
		{
			lstBricks.Add(GetImageFromGrid(x, y, width, height, imgGrid, Color.White, RotateFlipType.RotateNoneFlipNone, 0));
			return lstBricks.Count - 1;
		}

		public static int AddSpriteFromGrid(int x, int y, int width, int height, Image imgGrid)
		{
			lstSprites.Add(GetImageFromGrid(x, y, width, height, imgGrid, Color.White, RotateFlipType.RotateNoneFlipNone, 0));
			return lstSprites.Count - 1;
		}

		public static int AddBrickFromGrid(int x, int y, int width, int height, Image imgGrid, Color transparentColor)
		{
			lstBricks.Add(GetImageFromGrid(x, y, width, height, imgGrid, transparentColor, RotateFlipType.RotateNoneFlipNone, 0));
			return lstBricks.Count - 1;
		}

		public static int AddSpriteFromGrid(int x, int y, int width, int height, Image imgGrid, Color transparentColor)
		{
			lstSprites.Add(GetImageFromGrid(x, y, width, height, imgGrid, transparentColor, RotateFlipType.RotateNoneFlipNone, 0));
			return lstSprites.Count - 1;
		}

		public static int AddBrickFromGrid(int x, int y, int width, int height, Image imgGrid, Color transparentColor, RotateFlipType flip)
		{
			lstBricks.Add(GetImageFromGrid(x, y, width, height, imgGrid, transparentColor, flip, 0f));
			return lstBricks.Count - 1;
		}

		public static int AddSpriteFromGrid(int x, int y, int width, int height, Image imgGrid, Color transparentColor, RotateFlipType flip)
		{
			lstSprites.Add(GetImageFromGrid(x, y, width, height, imgGrid, transparentColor, flip, 0f));
			return lstSprites.Count - 1;
		}

		public static int AddBrickFromGrid(int x, int y, int width, int height, Image imgGrid, Color transparentColor, RotateFlipType flip, float rotateAngle)
		{
			lstBricks.Add(GetImageFromGrid(x, y, width, height, imgGrid, transparentColor, flip, rotateAngle));
			return lstBricks.Count - 1;
		}

		public static int AddSpriteFromGrid(int x, int y, int width, int height, Image imgGrid, Color transparentColor, RotateFlipType flip, float rotateAngle)
		{
			lstSprites.Add(GetImageFromGrid(x, y, width, height, imgGrid, transparentColor, flip, rotateAngle));
			return lstSprites.Count - 1;
		}

		private static Image GetImageFromGrid(int x, int y, int width, int height, Image imgGrid, Color transparentColor, RotateFlipType flip, float rotateAngle)
		{
			Bitmap img = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			using (Graphics g = Graphics.FromImage(img))
			{
				g.DrawImage(imgGrid, 0, 0, new Rectangle(x, y, width, height), GraphicsUnit.Pixel);
				img.MakeTransparent(transparentColor);
				img.RotateFlip(flip);
				if(rotateAngle != 0)
					g.RotateTransform(rotateAngle);
				g.Save();
			}
			return img;
        }

        public static List<Image> GetImagesFromGrid(Image imgGrid, Vector2D size, List<Vector2D> lstStartPoints, Color transparentColor, RotateFlipType flip, float rotateAngle)
        {
            List<Image> lstRets = new List<Image>(lstStartPoints.Count);
            Size sz = new Size((int)size.X, (int)size.Y);
            for (int i = 0; i < lstStartPoints.Count; i++)
            {
                Vector2D vp = lstStartPoints[i];
                Rectangle rect = new Rectangle((int)vp.X, (int)vp.Y, sz.Width, sz.Height);

                Bitmap img = new Bitmap(sz.Width, sz.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                using (Graphics g = Graphics.FromImage(img))
                {
                    g.DrawImage(imgGrid, 0, 0, rect, GraphicsUnit.Pixel);
                    img.MakeTransparent(transparentColor);
                    img.RotateFlip(flip);
                    if (rotateAngle != 0)
                        g.RotateTransform(rotateAngle);
                    g.Save();
                }
                lstRets.Add(img);
            }
            return lstRets;
        }
	}
}
