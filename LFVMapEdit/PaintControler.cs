using System;
using System.Collections.Generic;
using System.Text;

namespace LFVMapEdit
{
    public class PaintControler
    {
        public PaintControler(PictureMap ppcm_Map)
        {
            fpcm_Map = ppcm_Map;
        }

        private PictureMap fpcm_Map;
        public PictureMap Map
        {
            get { return fpcm_Map; }
            set { fpcm_Map = value; }
        }

        public void SimplePaint(int x, int y, Brick pbrk_Brick)
        {
            if (x >= 0 && y >= 0 && x < fpcm_Map.QtdColumns && y < fpcm_Map.QtdRows)
            {
                fpcm_Map.Cells[x, y] = new MapCell(pbrk_Brick);
                fpcm_Map.Invalidate();
            }
        }

        private bool SimplePaintToFill(int x, int y, Brick pbrk_Brick)
        {
            if(fpcm_Map.Cells[x, y] == null || fpcm_Map.Cells[x, y].Brick != pbrk_Brick)
            {
                fpcm_Map.Cells[x, y] = new MapCell(pbrk_Brick);
                return true;
            }
            return false;
        }

        public void FillPaint(int x, int y, Brick pbrk_Brick)
        {
            for (int indexX = x; indexX < fpcm_Map.QtdColumns; indexX++)
            {
                if (fpcm_Map.Cells[indexX, y] != null && fpcm_Map.Cells[indexX, y].Brick == pbrk_Brick)
                    break;
                for (int indexY = y; indexY < fpcm_Map.QtdRows && SimplePaintToFill(indexX, indexY, pbrk_Brick); indexY++) ;                
            }
            for (int indexX = x-1; indexX >= 0; indexX--)
            {
                if (fpcm_Map.Cells[indexX, y] != null && fpcm_Map.Cells[indexX, y].Brick == pbrk_Brick)
                    break;
                for (int indexY = y - 1; indexY >= 0 && SimplePaintToFill(indexX, indexY, pbrk_Brick); indexY--) ;
            }
            for (int indexX = x - 1; indexX >= 0; indexX--)
            {
                if (fpcm_Map.Cells[indexX, y] != null && fpcm_Map.Cells[indexX, y].Brick == pbrk_Brick)
                    break;
                for (int indexY = y; indexY < fpcm_Map.QtdRows && SimplePaintToFill(indexX, indexY, pbrk_Brick); indexY++) ;

            }
            for (int indexX = x + 1; indexX < fpcm_Map.QtdColumns; indexX++)
            {
                if (fpcm_Map.Cells[indexX, y] != null && fpcm_Map.Cells[indexX, y].Brick == pbrk_Brick)
                    break;
                for (int indexY = y - 1; indexY >= 0 && SimplePaintToFill(indexX, indexY, pbrk_Brick); indexY--) ;
            }


            for (int indexY = y - 1; indexY >= 0; indexY--)
            {
                if (fpcm_Map.Cells[x, indexY] != null && fpcm_Map.Cells[x, indexY].Brick == pbrk_Brick)
                    break;
                for (int indexX = x; indexX < fpcm_Map.QtdColumns && SimplePaintToFill(indexX, indexY, pbrk_Brick); indexX++) ;
            }
            fpcm_Map.Invalidate();
        }


        
        private BrushType fenm_BrushType = BrushType.Pencil;
        public BrushType BrushType
        {
            get { return fenm_BrushType; }
            set { fenm_BrushType = value; }
        }

        public void Paint(int x, int y, Brick pbrk_Brick)
        {
            switch (this.fenm_BrushType)
            {
                case BrushType.Pencil:
                    this.SimplePaint(x, y, pbrk_Brick);
                    break;
                case BrushType.FillBrush:
                    this.FillPaint(x, y, pbrk_Brick);
                    break;
            }
        }

        public void Delete(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < fpcm_Map.QtdColumns && y < fpcm_Map.QtdRows)
            {
                fpcm_Map.Cells[x, y] = null;
                fpcm_Map.Invalidate();
            }
        }
    }

    public enum BrushType
    {
        Pencil,
        FillBrush
    }
}
