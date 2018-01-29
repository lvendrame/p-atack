using System;
using System.Collections.Generic;
using System.Text;

namespace LFVMapControler
{
	[Serializable()]
	public class MapInformation
	{
		public int columns;
		public int rows;
		public int tileWidth;
		public int tileHeight;
		public int[] brickIndex;
		public Brick[] bricks;

		public MapInformation(MatrixMapCell mtxMapCell, List<Brick> lstBricks, int pint_TileWidth, int pint_TileHeight)
		{
			columns = mtxMapCell.Columns;
			rows = mtxMapCell.Rows;
			tileWidth = pint_TileWidth;
			tileHeight = pint_TileHeight;
			brickIndex = new int[columns * rows];

			#region bricks
			bricks = new Brick[lstBricks.Count];
			for (int i = 0; i < lstBricks.Count; i++)
			{
				Brick brk = lstBricks[i];
				brk.fint_Index = i;
				bricks[i] = brk;
			} 
			#endregion

			#region brickIndex
			int index = 0;
			for (int x = 0; x < columns; x++)
			{
				for (int y = 0; y < rows; y++)
				{
					MapCell cell = mtxMapCell[x, y];
					brickIndex[index] = (cell != null) ? mtxMapCell[x, y].Brick.fint_Index : -1;
					index++;
				}
			} 
			#endregion
		}
		public MapInformation() { }

		private List<Brick> ToBrickList()
		{
			List<Brick> lst = new List<Brick>();
			lst.AddRange(bricks);
			return lst;
		}

		public MapResponse ToMapResponse()
		{
			MatrixMapCell mtx = new MatrixMapCell(columns, rows);
			List<Brick> lstBricks = this.ToBrickList();

			int ind = 0;
			for (int x = 0; x < columns; x++)
			{
				for (int y = 0; y < rows; y++)
				{
					int brkInd = brickIndex[ind];
					if(brkInd != -1)
						mtx.AddBrick(x, y, lstBricks[brkInd]);
					ind++;
				}
			}

			return new MapResponse(lstBricks, mtx, tileWidth, tileHeight);
		}
	}

	public class MapResponse
	{
		public MapResponse(List<Brick> plst_Bricks, MatrixMapCell pmtx_MapCell, int pint_TileWidth, int pint_TileHeight)
		{
			fint_TileWidth = pint_TileWidth;
			fint_TileHeight = pint_TileHeight;
			lstBricks = plst_Bricks;
			mtxMapCell = pmtx_MapCell;
		}

		private List<Brick> lstBricks;
		public List<Brick> Bricks
		{
			get { return lstBricks; }
			set { lstBricks = value; }
		}

		private MatrixMapCell mtxMapCell;
		public MatrixMapCell Matrix
		{
			get { return mtxMapCell; }
			set { mtxMapCell = value; }
		}

		private int fint_TileWidth;
		public int TileWidth
		{
			get { return fint_TileWidth; }
			set { fint_TileWidth = value; }
		}

		private int fint_TileHeight;
		public int TileHeight
		{
			get { return fint_TileHeight; }
			set { fint_TileHeight = value; }
		}

	}
}
