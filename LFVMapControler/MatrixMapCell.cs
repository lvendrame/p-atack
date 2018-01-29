using System;
using System.Collections.Generic;
using System.Text;

namespace LFVMapControler
{
	public class MatrixMapCell
	{
		private int fintColumns;
		public int Columns
		{
			get { return fintColumns; }
			set { fintColumns = value; }
		}

		private int fintRows;
		public int Rows
		{
			get { return fintRows; }
			set { fintRows = value; }
		}

		public MatrixMapCell(int columns, int rows)
		{
			mtxCell = new MapCell[columns, rows];
			this.fintColumns = columns;
			this.fintRows = rows;
		}

		private MapCell[,] mtxCell;
		public MapCell this[int column, int row]
		{
			get
			{
				return mtxCell[column, row];
			}
			set
			{
				mtxCell[column, row] = value;
			}
		}

		public void AddBrick(int column, int row, Brick pbrk_Brick)
		{
			if (pbrk_Brick == null)
				this[column, row] = null;
			else
				this[column, row] = new MapCell(pbrk_Brick);
		}

		public void Clear()
		{
			for (int x = 0; x < this.Columns; x++)
			{
				for (int y = 0; y < this.Rows; y++)
				{
					this[x, y] = null;
				}
			}
		}

		public void CopyTo(MatrixMapCell mtxMapCell)
		{
			int columns = this.Columns < mtxMapCell.Columns ? this.Columns : mtxMapCell.Columns;
			int rows = this.Rows < mtxMapCell.Rows ? this.Rows : mtxMapCell.Rows;

			for (int x = 0; x < columns; x++)
			{
				for (int y = 0; y < rows; y++)
				{
					mtxMapCell[x, y] = this[x, y];
				}
			}
		}

		public void CopyTo(MatrixMapCell mtxMapCell, int startX, int startY)
		{
			int columns = this.Columns < mtxMapCell.Columns ? this.Columns : mtxMapCell.Columns;
			int rows = this.Rows < mtxMapCell.Rows ? this.Rows : mtxMapCell.Rows;

			for (int x = startX; x < columns; x++)
			{
				for (int y = startY; y < rows; y++)
				{
					mtxMapCell[x, y] = this[x, y];
				}
			}
		}

		public static MatrixMapCell FromMatrixMapCell(MatrixMapCell mmcMatrix, int pint_Columns, int pint_Rows)
		{
			MatrixMapCell newMatrix = new MatrixMapCell(pint_Columns, pint_Rows);
			mmcMatrix.CopyTo(newMatrix);
			return newMatrix;
		}

		public static MatrixMapCell FromMatrixMapCell(MatrixMapCell mmcMatrix, int pint_Columns, int pint_Rows, int startX, int startY)
		{
			MatrixMapCell newMatrix = new MatrixMapCell(pint_Columns, pint_Rows);
			mmcMatrix.CopyTo(newMatrix, startX, startY);
			return newMatrix;
		}
	}
}
