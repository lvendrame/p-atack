using System;
using System.Collections.Generic;
using System.Text;

namespace LFVPack
{
    public class MapPart: List<int>
    {
        
        public MapPart() { }

        public MapPart(Part part)
        {
            this.Read(part);
        }

        public MapPart(string name, int[][] vecMapBricks, int tileWidth, int tileHeight)
        {
            this.Name = name;
            this.TileHeight = tileHeight;
            this.TileWidth = tileWidth;
            this.Rows = vecMapBricks.Length;
            this.Columns = vecMapBricks[0].Length;
            for (int i = 0; i < this.Rows; i++)
            {
                int[] vecFinal = vecMapBricks[i];
                for (int j = 0; j < this.Columns; j++)
                {
                    this.Add(vecFinal[j]);
                }
            }
        }

        public int[][] ToIntMatrix()
        {
            int [][] retMat = new int[this.Rows][];
            int index = 0;
            for (int i = 0; i < this.Rows; i++)
            {
                int[] row = new int[this.Columns];
                for (int j = 0; j < this.Columns; j++)
                {
                    row[j] = this[index];
                    ++index;
                }
                retMat[i] = row;
            }
            return retMat;
        }

        public string Name;
        public int Columns;
        public int Rows;
        public int TileWidth;
        public int TileHeight;

        public void Write(Part part)
        {
            if (this.Count != this.Columns * this.Rows)
                throw new Exception("A quantidade de bricks deve ser a quantidade de colunas pela quantidade de linhas");

            Part mapPart = new Part(PartType.Map);
            mapPart.WriteInt(Columns);
            mapPart.WriteInt(Rows);
            mapPart.WriteInt(TileWidth);
            mapPart.WriteInt(TileHeight);
            mapPart.WriteString(Name);

            for (int i = 0; i < this.Count; i++)
                mapPart.WriteInt(this[i]);

            part.Add(mapPart);
        }

        public void Read(Part part)
        {
            this.Columns = part.ReadInt();
            this.Rows = part.ReadInt();
            this.TileWidth = part.ReadInt();
            this.TileHeight = part.ReadInt();
            this.Name = part.ReadString();
            int length = this.Columns * this.Rows;
            for (int i = 0; i < length; i++)
                this.Add(part.ReadInt());
        }
        
    }
}
