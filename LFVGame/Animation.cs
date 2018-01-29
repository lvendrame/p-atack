using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using LFVMath.Basic;

namespace LFVGame
{
    public class Animation
    {
        public Animation(List<Image> plstImages, double dblFrameTime)
        {
            this.lstImages = plstImages.ToArray();
            taFrameTime.MaxTime = dblFrameTime;
        }

        public Animation(Image[] pvecImages, double dblFrameTime)
        {
            this.lstImages = new Image[pvecImages.Length];
            pvecImages.CopyTo(this.lstImages, 0);
            taFrameTime.MaxTime = dblFrameTime;
        }

        private int intCurrentIndex = 0;
        public int CurrentIndex
        {
            get { return intCurrentIndex; }
            set 
            {
                if(value < this.NumberFrames)                    
                    intCurrentIndex = value;
            }
        }

        public int NumberFrames
        {
            get { return this.lstImages.Length; }
        }

        private TimeAcumulator taFrameTime;
        public double FrameTime
        {
            get { return taFrameTime.MaxTime; }
            set { taFrameTime.MaxTime = value; }
        }

        private Image[] lstImages;
        public Image[] Images
        {
            get { return lstImages; }
            set { lstImages = value; }
        }

        public Image GetImage(double dblElapsedTime)
        {
            taFrameTime.Update(dblElapsedTime);
            if (taFrameTime.IsOverflow)
            {
                this.Next();
            }
            return lstImages[intCurrentIndex];
        }

        private void Next()
        {
            intCurrentIndex++;
            if (intCurrentIndex >= NumberFrames)
                intCurrentIndex = 0;
        }

        public void Draw(Graphics grPaint, Vector2D pv2d_Position, Vector2D pv2d_MapPosition, double dblElapsedTime)
        {
            grPaint.DrawImage(this.GetImage(dblElapsedTime), (float)pv2d_Position.X, (float)(pv2d_Position.Y - pv2d_MapPosition.Y));
        }
    }
}
