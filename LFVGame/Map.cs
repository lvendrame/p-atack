using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Resources;
using LFVMath.Phisics;
using LFVMath.Basic;
using LFVMapControler;

namespace LFVGame
{
	public class Map: AbstractColidable
	{
		private List<Tile> lstTiles = new List<Tile>();
		public List<Tile> Tiles
		{
			get { return lstTiles; }
		}
		
		public Map()
		{
			this.Velocity.Y = -50;
		}

		public void Load(Image originalMap, int scale)
		{
			this.Heigth = originalMap.Height * scale;
			this.Width = originalMap.Width * scale;
			
			Bitmap map = new Bitmap(originalMap);
			lstTiles.Clear();
			for (int x = 0; x < map.Width; x++)
			{
				for (int y = 0; y < map.Height; y++)
				{
					Color colorPixel = map.GetPixel(x, y);
					lstTiles.Add(this.GetImage(colorPixel, scale, new Vector2D(x*scale, y*scale)));
				}
			}
		}

		public void Load(Image map)
		{
			this.Load(map, 16);
            this.Position.Y = this.Heigth - 600;
		}

		public void Load(string pstr_MapFileName)
		{
			MapResponse response = FileControler.Open(pstr_MapFileName);			
			this.Heigth = response.Matrix.Rows * response.TileHeight;
			this.Width = response.Matrix.Columns * response.TileWidth;			

			for (int i = 0; i < response.Bricks.Count; i++)
			{
				Brick brk = response.Bricks[i];
				//brk.Image = Bitmap.FromFile(ContentManager.BRICKS_PATH + brk.ImagePath);
                brk.Image = ContentManager.GetContent<Image>(ContentManager.PathType.Bricks, brk.ImagePath);
				brk.fint_Index = StaticImages.Bricks.Count;
				StaticImages.Bricks.Add(brk.Image);
			}

			lstTiles.Clear();
			for (int x = 0; x < response.Matrix.Columns; x++)
			{
				for (int y = 0; y < response.Matrix.Rows; y++)
				{
					MapCell cell = response.Matrix[x, y];
					if(cell != null)
						lstTiles.Add(this.GetImage(cell.Brick, response.TileHeight, response.TileWidth, new Vector2D(x * response.TileWidth, y * response.TileHeight)));
				}
			}

            this.Position.Y = this.Heigth - 600;
		}

		public void Load()
		{
			this.Load(Resource.map01);
		}
		
		private Tile GetImage(Color colorPixel, int scale, Vector2D position)
		{
			Tile tile = new Tile();

			tile.Width = scale;
			tile.Heigth = scale;
			tile.Position = position;

			if (colorPixel.R < 51)
				tile.Index = 0;
			else if (colorPixel.R < 102)
				tile.Index = 1;
			else if (colorPixel.R < 153)
				tile.Index = 2;
			else if (colorPixel.R < 204)
				tile.Index = 3;
			else
				tile.Index = 4;

			return tile;

		}

		private Tile GetImage(Brick pbrk_Brick, int tileHeight, int tileWidth, Vector2D position)
		{
			Tile tile = new Tile();

			tile.Width = tileWidth;
			tile.Heigth = tileHeight;
			tile.Position = position;

			tile.Index = pbrk_Brick.fint_Index;

			return tile;
		}
		
		public virtual void Draw(Graphics grPaint)
		{
            for (int i = 0; i < lstTiles.Count; i++)
            {
                Tile tile = lstTiles[i];
				int posY = (int)(tile.Position.Y - this.Position.Y);
				if(posY > -tile.Heigth && posY < (600+tile.Heigth))
					grPaint.DrawImage(tile.TileImage, (int)tile.Position.X, posY);
            }
		}

        public override void Dispose()
        {
            Util.FinalizeListItems(lstTiles);
            base.Dispose();
        }
		
	}
}
