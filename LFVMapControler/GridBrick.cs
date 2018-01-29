using System;
using System.Collections.Generic;
using System.Text;

namespace LFVMapControler
{
    public class GridBrick
    {
        public GridBrick()
        {
        }

        private Brick[][] vecImages;
        public Brick[][] Images
        {
            get { return vecImages; }
            set { vecImages = value; }
        }

        private int fint_QtdRows = 0;
        public int QtdRows
        {
            get { return fint_QtdRows; }
            set
            {
                fint_QtdRows = value;
            }
        }

        private int fint_QtdColumns = 0;
        public int QtdColumns
        {
            get { return fint_QtdColumns; }
            set
            {
                fint_QtdColumns = value;
            }
        }

        public bool Equal(GridBrick grd)
        {
            if (this.QtdColumns != grd.QtdColumns)
                return false;
            if (this.QtdRows != grd.QtdRows)
                return false;

            for (int i = 0; i < QtdColumns; i++)
            {
                for (int j = 0; j < QtdRows; j++)
                {
                    if (vecImages[i][j] != grd.vecImages[i][j])
                        return false;
                }
            }
            return true;
        }

        public bool Equal(Brick[][] brks)
        {
            if (brks.Length != this.QtdColumns)
                return false;
            if (brks[0].Length != this.QtdRows)
                return false;

            for (int i = 0; i < QtdColumns; i++)
            {
                for (int j = 0; j < QtdRows; j++)
                {
                    if (vecImages[i][j] != brks[i][j])
                        return false;
                }
            }
            return true;
        }
    }
}
