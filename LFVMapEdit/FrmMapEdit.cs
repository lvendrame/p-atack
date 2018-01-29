using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LFVMapEdit.Paint;
using LFVMapControler;
using Microsoft.Xna.Framework;

namespace LFVMapEdit
{
	public partial class FrmMapEdit : Form
	{
        public IntPtr DrawSurface
        {
            get
            {
                return this.panel1.Handle;
            }
        }

        private Game gmXnaGame;

        public Game XnaGame
        {
            get { return gmXnaGame; }
            set { gmXnaGame = value; }
        }

        void FrmMapEdit_Disposed(object sender, EventArgs e)
        {
            if (gmXnaGame != null)
                gmXnaGame.Exit();

            Application.Exit();
        }


		public FrmMapEdit()
		{            
			InitializeComponent();

            //XnaMapControl xmc = new XnaMapControl(this.panel1.Handle);
            //xmc.Run();
            //
            //xmc.AddToControl(this, this.panel1);
            this.Disposed += new EventHandler(FrmMapEdit_Disposed);
            this.panel1.Controls.Clear();

            this.brickGrid1.OnSelectArea += new EventHandler(brickGrid1_OnSelectArea);
            this.InitializeBrick();
			controler = AbstractPaint.Initialize<DotPaint>(this.pcmMapPaint);
		}

        void brickGrid1_OnSelectArea(object sender, EventArgs ev)
        {
            GridBrick gridBrick = pcmMapPaint.GridBrick;            
            BrickGrid.SelectArea e = brickGrid1.SelectedArea;
            gridBrick.QtdColumns = e.Width < brickGrid1.QtdColumns ? e.Width: brickGrid1.QtdColumns;
            gridBrick.QtdRows = e.Heigth < brickGrid1.QtdRows? e.Heigth : brickGrid1.QtdRows;
            
            gridBrick.Images = new Brick[e.Width][];
            for (int i = 0; i < e.Width; i++)
            {
                gridBrick.Images[i] = new Brick[e.Heigth];
                for (int j = 0; j < e.Heigth; j++)
                {
                    Brick brk = new Brick();
                    brk.Image = brickGrid1[e.Column + i, e.Row + j];
                    gridBrick.Images[i][j] = brk;
                }
            }
        }
        

        IPaint controler = null;

        private string fstrBrickPath;
        public string BrickPath
        {
            get { return fstrBrickPath; }
            set { fstrBrickPath = value; }
        }

        private string fstrMapPath;
        public string MapPath
        {
            get { return fstrMapPath; }
            set { fstrMapPath = value; }
        }

        private bool fblnIsNew = true;
        public bool IsNew
        {
            get { return fblnIsNew; }
            set { fblnIsNew = value; }
        }


		private void pcmMapPaint_MouseMove(object sender, MouseEventArgs e)
		{
			controler.MouseEventDraw(e, true, this.brk);
		}
		
		private void pcmMapPaint_MouseDown(object sender, MouseEventArgs e)
		{
			controler.MouseEventDraw(e, false, this.brk);
		}

        private void btnApplyChanges_Click(object sender, EventArgs e)
        {
            pcmMapPaint.SetTileSize(
                Convert.ToInt32(dudWidth.Text),
                Convert.ToInt32(dudHeigth.Text));

            pcmMapPaint.SetTableSize(
                Convert.ToInt32(nudColumns.Value),
                Convert.ToInt32(nudRows.Value));

            pcmMapPaint.Invalidate();

            this.lblSize.Text = pcmMapPaint.Width.ToString() + "x" + pcmMapPaint.Height.ToString();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        Brick brk;
        private void InitializeBrick()
        {                    
        }

        private void showGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pcmMapPaint.ShowGrid = showGridToolStripMenuItem.Checked;
            pcmMapPaint.Invalidate();
        }

        private void ShowGridInFrontToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            pcmMapPaint.ShowGridInFront = ShowGridInFrontToolStripMenuItem.Checked;
            pcmMapPaint.Invalidate();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.IsNew = true;
            pcmMapPaint.Clear();
        }

        private void pencilToolStripButton_Click(object sender, EventArgs e)
        {
			controler = AbstractPaint.Initialize<DotPaint>(this.pcmMapPaint);            
        }

        private void fillBrushToolStripButton_Click(object sender, EventArgs e)
        {
			controler = AbstractPaint.Initialize<FillFloodPaint>(this.pcmMapPaint);
        }

        private void ltvBricks_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            BrickListItem item = e.Item as BrickListItem;
            //e.Bounds = new Rectangle(e.Bounds.X, e.Bounds.Y, item.Brick.Image.Width,item.Brick.Image.Height);
            e.Graphics.DrawImage(item.Brick.Image, e.Bounds);
        }

        private void btnAddBrick_Click(object sender, EventArgs e)
        {
            if (ofdMapEdit.ShowDialog() == DialogResult.OK)
            {
                foreach (string fileName in ofdMapEdit.FileNames)
                {
                    Brick brk = new Brick();
                    brk.ImagePath = fileName;
                    brk.Image = Bitmap.FromFile(brk.ImagePath);

                    BrickListItem item = new BrickListItem();
                    item.Brick = brk;
                    item.Text = FileControler.GetFileName(brk.ImagePath);
                    this.ltvBricks.Items.Add(item);
                }
            }
        }

        private void ltvBricks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ltvBricks.SelectedItems.Count != 0)
            {
                BrickListItem item = ltvBricks.SelectedItems[0] as BrickListItem;
                this.brk = item.Brick;
            }
        }

		private void lineToolStripButton_Click(object sender, EventArgs e)
		{
			controler = AbstractPaint.Initialize<LinePaint>(this.pcmMapPaint);
		}

		private void circleToolStripButton1_Click(object sender, EventArgs e)
		{
			controler = AbstractPaint.Initialize<CirclePaint>(this.pcmMapPaint);
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
            bool save = true;
            if (this.IsNew)
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = FileControler.FILTER;
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        this.MapPath = sfd.FileName;
                    }
                    else
                    {
                        save = false;
                    }
                }
            }
            
			if (save)
			{
				try
				{
					FileControler.Save(this.MapPath, this.pcmMapPaint.Cells, this.GetBricks(), this.pcmMapPaint.TileWidth, this.pcmMapPaint.TileHeigth);
                    this.IsNew = false;
					MessageBox.Show("Os mapa foi salvo com sucesso.");
				}
				catch(Exception ex)
				{
					MessageBox.Show("Não foi possível salvar o mapa com sucesso. Tente novamente.\r\nDetalhes: " + ex.Message);
				}
			}
		}

		private List<Brick> GetBricks()
		{
			List<Brick> lstRet = new List<Brick>();
			for (int i = 0; i < ltvBricks.Items.Count; i++)
			{
				BrickListItem item = ltvBricks.Items[i] as BrickListItem;
				lstRet.Add(item.Brick);
			}
			return lstRet;
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = FileControler.FILTER;
			if (ofd.ShowDialog() == DialogResult.OK && System.IO.File.Exists(ofd.FileName))
			{
                this.IsNew = false;
                this.MapPath = ofd.FileName;
                this.BrickPath = FileControler.GetPath(ofd.FileName) + "Bricks\\";
				MapResponse response = FileControler.Open(ofd.FileName);
				this.pcmMapPaint.TileHeigth = response.TileHeight;
				this.pcmMapPaint.TileWidth = response.TileWidth;
				this.pcmMapPaint.QtdColumns = response.Matrix.Columns;
				this.pcmMapPaint.QtdRows = response.Matrix.Rows;
				this.pcmMapPaint.Cells = response.Matrix;

				this.ltvBricks.Items.Clear();
				for (int i = 0; i < response.Bricks.Count; i++)
				{
					Brick brk = response.Bricks[i];
                    brk.IsNew = false;
					brk.Image = Bitmap.FromFile(this.BrickPath + brk.ImagePath);

					BrickListItem item = new BrickListItem();
					item.Brick = brk;
					item.Text = brk.ImagePath;
					this.ltvBricks.Items.Add(item);
				}
				this.pcmMapPaint.Invalidate();

				dudWidth.Text = this.pcmMapPaint.TileHeigth.ToString();
				dudHeigth.Text = this.pcmMapPaint.TileWidth.ToString();
				nudColumns.Value = this.pcmMapPaint.QtdColumns;
				nudRows.Value = this.pcmMapPaint.QtdRows;

				this.lblSize.Text = pcmMapPaint.Width.ToString() + "x" + pcmMapPaint.Height.ToString();
			}
		}

		private void copyToolStripButton_Click(object sender, EventArgs e)
		{
			controler = new CopyPaint(this.pcmMapPaint.TileHeigth, this.pcmMapPaint.TileWidth);
			controler.Map = this.pcmMapPaint;
		}

        private void levelToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            pcmMapPaint.CurrentLayer = Convert.ToInt32(levelToolStripComboBox.SelectedItem) - 1;
        }

        private void btnEditGrid_Click(object sender, EventArgs e)
        {
            FrmGridEdit frm = new FrmGridEdit();
            if (frm.ShowDialog(this.brickGrid1) == DialogResult.OK)
            {
                this.brickGrid1.Image = frm.FinalImage;
                this.brickGrid1.InitializeImages();
            }
        }

        LFVPack.Wavefront.ObjectWavefrontReader.ResponseObj rep = null;
        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    LFVPack.Wavefront.ObjectWavefrontReader reader = new LFVPack.Wavefront.ObjectWavefrontReader();
                    System.IO.StreamReader obj = new System.IO.StreamReader(ofd.FileName);
                    rep = reader.Reader(obj);
                    obj.Close();
                    pcmMapPaint.Paint += new PaintEventHandler(pcmMapPaint_Paint);
                }
            }
        }

        void pcmMapPaint_Paint(object sender, PaintEventArgs e)
        {
            if (rep != null)
            {
                foreach (LFVPack.Wavefront.Group g in rep.Groups)
                {
                    foreach (LFVPack.Wavefront.Face f in g.Faces)
                    {
                        List<PointF> lst = new List<PointF>(f.Vertices.Length);
                        foreach (LFVPack.Wavefront.Vector3f v3f in f.Vertices)
                        {
                            lst.Add(new PointF((v3f.X / 50000)+300, (v3f.Y / 50000)+300));
                        }
                        e.Graphics.FillPolygon(Brushes.Black, lst.ToArray(), System.Drawing.Drawing2D.FillMode.Winding);
                    }
                }
            }
        }     
	}
}