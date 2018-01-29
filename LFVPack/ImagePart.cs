using System;
using System.Collections.Generic;
using System.Text;

namespace LFVPack
{
    public class ImagePart: List<ImagePart.Color>
    {
        public ImagePart() { }

        public ImagePart(Part part)
        {
            this.Read(part);
        }

        public int Width;
        public int Height;
        public string Name;

        public void Write(Part part)
        {
            Part imgPart = new Part(PartType.Image);
            imgPart.WriteInt(Width);
            imgPart.WriteInt(Height);
            imgPart.WriteString(Name);
            for (int i = 0; i < this.Count; i++)
                this[i].Write(imgPart);

            part.Add(imgPart);
        }

        public void Read(Part part)
        {
            this.Width = part.ReadInt();
            this.Height = part.ReadInt();
            this.Name = part.ReadString();
            int length = this.Width * this.Height;
            for (int i = 0; i < length; i++)
                this.Add(new Color(part));
        }
        

        public struct Color
        {
            public Color(Part part)
            {
                r = part.ReadInt();
                g = part.ReadInt();
                b = part.ReadInt();
                a = part.ReadInt();
            }

            public int r;
            public int g;
            public int b;
            public int a;

            public void Write(Part part)
            {
                part.WriteInt(r);
                part.WriteInt(g);
                part.WriteInt(b);
                part.WriteInt(a);
            }

            public void Read(Part part)
            {
                r = part.ReadInt();
                g = part.ReadInt();
                b = part.ReadInt();
                a = part.ReadInt();
            }
        }
        
    }
}
