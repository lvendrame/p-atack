using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using LFVMapControler;

namespace LFVMapEdit
{
	public class PictureMap: PictureBox
	{
		public PictureMap()
		{
			this.Paint += new PaintEventHandler(PictureMap_Paint);
			this.Resize += new EventHandler(PictureMap_Resize);
            lstMtxCells = GetLayers();
			this.Restart();
		}

		void PictureMap_Resize(object sender, EventArgs e)
		{
			this.fint_QtdRows = this.fint_TileHeigth != 0 ? this.Height / fint_TileHeigth : 0;
			this.fint_QtdColumns = this.fint_TileWidth != 0 ? this.Width / fint_TileWidth : 0;

			if (this.fint_QtdColumns != 0 && this.fint_QtdRows != 0)
			{
				this.Width = fint_TileWidth * fint_QtdColumns;
				this.Height = fint_TileHeigth * fint_QtdRows;
			}

			this.Restart();
		}

		void PictureMap_Paint(object sender, PaintEventArgs e)
		{
            if (fbln_ShowGrid)
            {
                if (fbln_ShowGridInFront)
                {
                    this.DrawBricks(e.Graphics);
                    this.DrawCells(e.Graphics);
                }
                else
                {
                    this.DrawCells(e.Graphics);
                    this.DrawBricks(e.Graphics);
                }
            }
            else
            {
                this.DrawBricks(e.Graphics);
            }
            this.DrawSelectedBricks(e.Graphics);
		}

        private void DrawSelectedBricks(Graphics graphics)
        {
            for (int i = 0; i < gbGridBrick.QtdColumns; i++)
            {
                for (int j = 0; j < gbGridBrick.QtdRows; j++)
                {
                    graphics.DrawImage(gbGridBrick.Images[i][j].Image,
                         pt.X + (i * this.TileWidth),
                         pt.Y + (j * this.TileHeigth));
                }
            }
        }

        Point pt;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            pt = new Point(e.X, e.Y);
            this.Invalidate();
        }

		#region Properties
		private int fint_TileWidth = 16;
		public int TileWidth
		{
			get { return fint_TileWidth; }
			set
			{
				fint_TileWidth = value;
				this.Width = fint_TileWidth * fint_QtdColumns;
				this.Restart();
			}
		}

		private int fint_TileHeigth = 16;
		public int TileHeigth
		{
			get { return fint_TileHeigth; }
			set
			{
				fint_TileHeigth = value;
				this.Height = fint_TileHeigth * fint_QtdRows;
				this.Restart();
			}
		}

        private GridBrick gbGridBrick = new GridBrick();
        public GridBrick GridBrick
        {
            get { return gbGridBrick; }
            set { gbGridBrick = value; }
        }


		private List<MatrixMapCell> lstSubMatrixCells = new List<MatrixMapCell>();
		[Browsable(false)]
		public List<MatrixMapCell> SubCells
		{
			get { return lstSubMatrixCells; }
		}

        private int intCurrentLayer = 0;

        public int CurrentLayer
        {
            get { return intCurrentLayer; }
            set { intCurrentLayer = value; }
        }


		private List<MatrixMapCell> lstMtxCells = null;
        private List<MatrixMapCell> GetLayers()
        {
            List<MatrixMapCell> lstCells = new List<MatrixMapCell>();
            for (int i = 0; i < 4; i++)
            {
                lstCells.Add(new MatrixMapCell(this.QtdColumns, this.QtdRows));
            }
            return lstCells;
        }

		[Browsable(false)]
		public MatrixMapCell Cells
		{
            get { return lstMtxCells[intCurrentLayer]; }
            set { lstMtxCells[intCurrentLayer] = value; }
		}

		private int fint_QtdRows = 0;
		public int QtdRows
		{
			get { return fint_QtdRows; }
			set
			{
				fint_QtdRows = value;
				this.Height = fint_TileHeigth * fint_QtdRows;
				this.Restart();
			}
		}

		private int fint_QtdColumns = 0;
		public int QtdColumns
		{
			get { return fint_QtdColumns; }
			set 
			{ 
				fint_QtdColumns = value;
				this.Width = fint_TileWidth * fint_QtdColumns;
				this.Restart();
			}
		}

        private bool fbln_ShowGridInFront = true;

        public bool ShowGridInFront
        {
            get { return fbln_ShowGridInFront; }
            set { fbln_ShowGridInFront = value; }
        }

        private bool fbln_ShowGrid = true;

        public bool ShowGrid
        {
            get { return fbln_ShowGrid; }
            set { fbln_ShowGrid = value; }
        }

		#endregion

		private void Restart()
		{
			int oldQtdRows = this.fint_QtdRows;
			int oldQtdColumns = this.fint_QtdColumns;

			this.fint_QtdRows = this.fint_TileHeigth != 0 ? this.Height / fint_TileHeigth : 0;
			this.fint_QtdColumns = this.fint_TileWidth != 0 ? this.Width / fint_TileWidth : 0;

            for (int i = 0; i < lstMtxCells.Count; i++)
            {
                this.lstMtxCells[i] = this.ResizeMatrix(this.lstMtxCells[i], this.fint_QtdColumns, this.fint_QtdRows); ;
            }
		}

		private MatrixMapCell ResizeMatrix(MatrixMapCell pmtx_CellsToResize, int pint_QtdColumns, int pint_QtdRows)
		{
			MatrixMapCell oldMatrix = pmtx_CellsToResize;
			pmtx_CellsToResize = new MatrixMapCell(pint_QtdColumns, pint_QtdRows);
			if (oldMatrix != null)
				oldMatrix.CopyTo(pmtx_CellsToResize);
			return pmtx_CellsToResize;
		}

		private void DrawCells(Graphics gr)
		{
			if (fint_TileWidth > 0 && fint_TileHeigth > 0)
			{
				for (int x = 0; x <= this.Width; x += fint_TileWidth)
				{
					gr.DrawLine(Pens.Black, x, 0, x, this.Height);
				}

				for (int y = 0; y <= this.Height; y += fint_TileHeigth)
				{
					gr.DrawLine(Pens.Black, 0, y, this.Width, y);
				}
			}
		}

		public int GetMapIndexX(int x)
		{
			int index = 0;
			for (int i = 0; i < x; i+=this.fint_TileWidth)
			{
				index++;
			}
			return index-1;
		}

		public int GetMapIndexY(int y)
		{
			int index = 0;
			for (int i = 0; i < y; i += this.fint_TileHeigth)
			{
				index++;
			}
			return index-1;
        }

        public void SetTileSize(int pint_TileWidth, int pint_TileHeigth)
        {
            fint_TileWidth = pint_TileWidth;
            this.Width = fint_TileWidth * fint_QtdColumns;
            fint_TileHeigth = pint_TileHeigth;
            this.Height = fint_TileHeigth * fint_QtdRows;
            this.Restart();
        }

        public void SetTableSize(int pint_QtdColumns, int pint_QtdRows)
        {
            fint_QtdColumns = pint_QtdColumns;
            this.Width = fint_TileWidth * fint_QtdColumns;
            fint_QtdRows = pint_QtdRows;
            this.Height = fint_TileHeigth * fint_QtdRows;
            this.Restart();
        }

		private void DrawBricks(Graphics gr)
		{
            for (int i = 0; i < lstMtxCells.Count; i++)
            {
                DrawMatrix(this.lstMtxCells[i], gr);                 
            }			
			for (int i = 0; i < lstSubMatrixCells.Count; i++)
			{
				DrawMatrix(lstSubMatrixCells[i], gr);
			}
		}

		private void DrawMatrix(MatrixMapCell pmtx_MatrixCell, Graphics gr)
		{
			if (pmtx_MatrixCell != null)
			{
				int plotX = 0;
				for (int x = 0; x < pmtx_MatrixCell.Columns; x++)
				{
					int plotY = 0;
					for (int y = 0; y < pmtx_MatrixCell.Rows; y++)
					{
						MapCell cell = pmtx_MatrixCell[x, y];
						if (cell != null)
							gr.DrawImage(cell.Brick.Image, plotX, plotY);

						plotY += this.fint_TileHeigth;
					}
					plotX += this.fint_TileWidth;
				}
			}
		}

        public void Clear()
        {
            for (int i = 0; i < lstMtxCells.Count; i++)
            {
                lstMtxCells[i].Clear();
            }
            this.Invalidate();
        }

		public void ClearSubCell(int index)
		{
			for (int x = 0; x < this.QtdColumns; x++)
			{
				for (int y = 0; y < this.QtdRows; y++)
				{
					this.SubCells[index][x, y] = null;
				}
			}
		}

		public int AddSubCell()
		{
			int index = this.SubCells.Count;
			this.SubCells.Add(new MatrixMapCell(this.QtdColumns, this.QtdRows));
			return index;
		}
	}
}
