using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LFVMapEdit
{
    public partial class FrmGridEdit : Form
    {
        public FrmGridEdit()
        {
            InitializeComponent();
            this.Shown += new EventHandler(FrmGridEdit_Shown);
        }
        
        void FrmGridEdit_Shown(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "All Images|*.bmp;*.jpg;*.gif;*.png|Bmp|*.bmp| Jpg|*.jpg| Gif|*.gif| Png|*.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    brickGrid1.NewImage = Bitmap.FromFile(ofd.FileName);
                }
                else
                {
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }
        }

        public DialogResult ShowDialog(BrickGrid brkGrid)
        {
            this.brickGrid1.isLoading = true;
            this.brickGrid1.TileHeigth=brkGrid.TileHeigth;
            this.brickGrid1.TileWidth=brkGrid.TileWidth;
            this.brickGrid1.QtdColumns=brkGrid.QtdColumns;
            this.brickGrid1.QtdRows=brkGrid.QtdRows;
            this.brickGrid1.Image = brkGrid.Image;
            this.brickGrid1.isLoading = false;
            this.brickGrid1.Restart();
            return base.ShowDialog();
        }

        public Image FinalImage
        {
            get { return this.brickGrid1.Image; }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnAddBrick_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "All Images|*.bmp;*.jpg;*.gif;*.png|Bmp|*.bmp| Jpg|*.jpg| Gif|*.gif| Png|*.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    brickGrid1.NewImage = Bitmap.FromFile(ofd.FileName);
                }
            }
        }

    }
}