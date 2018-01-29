using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using LFVMapControler;

namespace LFVMapEdit.Paint
{
	public class CopyPaint: AbstractPaint
	{
		bool select = false;
		bool copy = false;
		int x = -1;
		int y = -1;
		int tileHeight, tileWidth;
		private Color SelectColor = Color.FromArgb(70, 15, 0, 240);
		private Brick brk;
		int subMatrixIndex = -1;

		private Brick GetBrick()
		{
			Bitmap bmp = new Bitmap(tileWidth, tileHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			for (int x = 0; x < tileWidth; x++)
			{
				for (int y = 0; y < tileHeight; y++)
				{
					bmp.SetPixel(x, y, SelectColor);
				}
			}
			Brick brkret = new Brick();
			brkret.Image = bmp;
			return brkret;
		}

		public CopyPaint(int tileHeight, int tileWidth)
		{
			this.tileHeight = tileHeight;
			this.tileWidth = tileWidth;
			brk = this.GetBrick();
		}

		private MatrixMapCell mmcCopyCells;
		public MatrixMapCell CopyCells
		{
			get { return mmcCopyCells; }
			set { mmcCopyCells = value; }
		}


		public override void MouseLeftDraw(int x, int y, LFVMapControler.Brick pbrk_NewBrick)
		{
			if (select)
			{
				if (copy)
				{
					this.Paste(x, y);
				}
				else
				{
					this.Copy(this.x, this.y, x, y);
					copy = true;
				}
			}
			else
			{
				this.subMatrixIndex = this.Map.AddSubCell();
				select = true;
				this.x = x;
				this.y = y;
				this.Create(x, y, x, y);
			}
		}

		public override void MouseRightDraw(int x, int y, LFVMapControler.Brick pbrk_NewBrick)
		{
			copy = false;
			select = false;
			this.Map.SubCells.RemoveAt(subMatrixIndex);
		}

		public override void MouseLeftMoveDraw(int x, int y, LFVMapControler.Brick pbrk_NewBrick)
		{
		}

		public override void MouseRightMoveDraw(int x, int y, LFVMapControler.Brick pbrk_NewBrick)
		{
		}

		public override void MouseMoveDraw(int x, int y, LFVMapControler.Brick pbrk_NewBrick)
		{
			if (select)
			{
				this.Create(this.x, this.y, x, y);
			}
		}

		private void Copy(int x1, int y1, int x2, int y2)
		{
			int startX = x1 < x2 ? x1 : x2;
			int startY = y1 < y2 ? y1 : y2;
			int columns = Math.Abs(x1 - x2) + 1;
			int rows = Math.Abs(y1 - y2) + 1;

			this.mmcCopyCells = new MatrixMapCell(columns, rows);

			columns += startX;
			rows += startY;

			int nx = 0;			
			for (int x = startX; x < columns; x++)
			{
				int ny = 0;
				for (int y = startY; y < rows; y++)
				{
					MapCell cell = this.Map.Cells[x, y];
					if(cell != null)
						this.mmcCopyCells[nx, ny] = new MapCell(cell.Brick);
					ny++;
				}
				nx++;
			}
		}

		private void Paste(int x1, int y1)
		{
			int startX = x1;
			int startY = y1;

			int maxColumn = startX + this.mmcCopyCells.Columns;
			int maxRow = startY + this.mmcCopyCells.Rows;

			MatrixMapCell mmc = this.Map.Cells;
			if (mmc.Columns < maxColumn)
				maxColumn = mmc.Columns;
			if (mmc.Rows < maxRow)
				maxRow = mmc.Rows;
			
			int nx = 0;
			for (int x = startX; x < maxColumn; x++)
			{
				int ny = 0;
				for (int y = startY; y < maxRow; y++)
				{
					MapCell cell = this.mmcCopyCells[nx, ny];
					if (cell != null)
						mmc[x, y] = cell;
					ny++;
				}
				nx++;
			}
		}

		private void Create(int x1, int y1, int x2, int y2)
		{
			if (copy)
			{
				int startX = x2;
				int startY = y2;

				int maxColumn = startX + this.mmcCopyCells.Columns;
				int maxRow = startY + this.mmcCopyCells.Rows;
				
				MatrixMapCell mmc = this.Map.SubCells[subMatrixIndex];
				if (mmc.Columns < maxColumn)
					maxColumn = mmc.Columns;
				if (mmc.Rows < maxRow)
					maxRow = mmc.Rows;

				mmc.Clear();
				int nx = 0;
				for (int x = startX; x < maxColumn; x++)
				{
					int ny = 0;
					for (int y = startY; y < maxRow; y++)
					{
						mmc[x, y] = this.mmcCopyCells[nx, ny];
						ny++;
					}
					nx++;
				}
			}
			else
			{
				int startX = x1 < x2 ? x1 : x2;
				int startY = y1 < y2 ? y1 : y2;
				int columns = Math.Abs(x1 - x2) + startX;
				int rows = Math.Abs(y1 - y2) + startY;


				MatrixMapCell mmc = this.Map.SubCells[subMatrixIndex];
				if (mmc.Columns < columns)
					columns = mmc.Columns;
				if (mmc.Rows < rows)
					rows = mmc.Rows;

				mmc.Clear();
				for (int x = startX; x <= columns; x++)
				{
					for (int y = startY; y <= rows; y++)
					{
						mmc[x, y] = new MapCell(this.brk);
					}
				}
			}
		}
	}
}
