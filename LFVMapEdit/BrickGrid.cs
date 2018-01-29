using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace LFVMapEdit
{
    public class BrickGrid: PictureBox
    {
        public bool isLoading = false;
        public BrickGrid()
        {            
            this.Image = new Bitmap(this.QtdColumns * this.TileWidth, this.QtdRows * this.TileHeigth,
                PixelFormat.Format32bppArgb);
            this.InitializeImages();
        }

        public void InitializeImages()
        {
            vecImages = new Image[this.QtdColumns][];
            for (int i = 0; i < this.QtdColumns; i++)
            {
                vecImages[i] = new Image[this.QtdRows];
                Image[] vecRowImg = vecImages[i];
                for (int j = 0; j < this.QtdRows; j++)
                {
                    vecRowImg[j] = this.GetTileFromGrid(i, j);
                }
            }
        }

        private Image GetTileFromGrid(int column, int row)
        {
            Image retImg = new Bitmap(this.TileWidth, this.TileHeigth, PixelFormat.Format32bppArgb);
            int x = this.TileWidth * column;
            int y = this.TileHeigth * row;
            Graphics g = Graphics.FromImage(retImg);       
            g.DrawImage(this.Image, 0,0, new Rectangle(x, y, this.TileWidth, this.TileHeigth), GraphicsUnit.Pixel);
            g.Save();
            return retImg;
        }

        private Image[][] vecImages;
        public Image[][] Images
        {
            get { return vecImages; }
        }

        private int fint_QtdRows = 20;
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

        private int fint_QtdColumns = 20;
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

        public void Restart()
        {
            if (isLoading)
                return;
            int oldQtdRows = this.fint_QtdRows;
            int oldQtdColumns = this.fint_QtdColumns;

            this.fint_QtdRows = this.fint_TileHeigth != 0 ? this.Height / fint_TileHeigth : 0;
            this.fint_QtdColumns = this.fint_TileWidth != 0 ? this.Width / fint_TileWidth : 0;

            Image retImg = new Bitmap(this.QtdColumns * this.TileWidth, this.QtdRows * this.TileHeigth,
                PixelFormat.Format32bppArgb);            
            Graphics g = Graphics.FromImage(retImg);
            g.DrawImage(this.Image, 0, 0, 
                retImg.Width < this.Image.Width ? retImg.Width: this.Image.Width,
                retImg.Height < this.Image.Height ? retImg.Height : this.Image.Height);
            g.Save();
            this.Image = retImg;
            this.InitializeImages();
        }

        public int GetMapIndexX(int x)
        {
            int index = 0;
            for (int i = 0; i < x; i += this.fint_TileWidth)
            {
                index++;
            }
            return index - 1;
        }

        public int GetMapIndexY(int y)
        {
            int index = 0;
            for (int i = 0; i < y; i += this.fint_TileHeigth)
            {
                index++;
            }
            return index - 1;
        }

        public Image this[int column, int row]
        {
            get
            {
                return this.vecImages[column][row];
            }
            set
            {
                this.vecImages[column][row] = value;
            }
        }

        public SelectArea SelectedArea;

        public struct SelectArea
        {
            public SelectArea(int pRow, int pColumn, int pWidth, int pHeigth)
            {
                Row = pRow;
                Column = pColumn;
                Width = pWidth;
                Heigth = pHeigth;
                Active = true;
            }

            public bool Active;
            public int Row;
            public int Column;
            public int Width;
            public int Heigth;
        }

        private Image imgNewImage;
        public Image NewImage
        {
            get { return imgNewImage; }
            set { imgNewImage = value; }
        }


        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                if (NewImage != null)
                {
                    Graphics g = Graphics.FromImage(this.Image);
                    g.DrawImage(NewImage,
                        this.GetMapIndexX(e.X) * this.TileWidth,
                        this.GetMapIndexY(e.Y) * this.TileHeigth);
                    g.Save();
                    NewImage = null;
                    this.Invalidate();
                    return;
                }
                else
                {
                    this.SelectedArea.Active = true;
                    this.SelectedArea.Column = this.GetMapIndexX(e.X);
                    this.SelectedArea.Row = this.GetMapIndexY(e.Y);
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                this.SelectedArea.Active = false;
                this.Invalidate();
            }

        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button == MouseButtons.Left)
            {
                this.SelectedArea.Active = true;
                this.SelectedArea.Width = (this.GetMapIndexX(e.X) - this.SelectedArea.Column) + 1;
                this.SelectedArea.Heigth = (this.GetMapIndexY(e.Y) - this.SelectedArea.Row) + 1;
                this.Invalidate();
            }
            if (NewImage != null)
            {
                SelectedArea.Row = e.Y;
                SelectedArea.Column = e.X;
                this.Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Left)
            {
                this.SelectedArea.Active = true;
                this.SelectedArea.Width = (this.GetMapIndexX(e.X) - this.SelectedArea.Column) + 1;
                this.SelectedArea.Heigth = (this.GetMapIndexY(e.Y) - this.SelectedArea.Row) + 1;
                this.Invalidate();
                if (OnSelectArea != null)
                    this.OnSelectArea(this, new EventArgs());
            }
        }

        SolidBrush brushSel = new SolidBrush(Color.FromArgb(60, 20, 0, 200));
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            DrawCells(pe.Graphics);
            if (NewImage != null)
            {
                pe.Graphics.DrawImage(NewImage, SelectedArea.Column, SelectedArea.Row);
            }
            else if (SelectedArea.Active)
            {
                pe.Graphics.FillRectangle(brushSel, SelectedArea.Column * this.TileWidth,
                    SelectedArea.Row * this.TileHeigth,
                    SelectedArea.Width * this.TileWidth,
                    SelectedArea.Heigth * this.TileHeigth);
            }            
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

        public event EventHandler OnSelectArea;
    }
}
