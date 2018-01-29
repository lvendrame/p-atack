using System;
using System.Collections.Generic;
using System.Text;

namespace LFVPack.Wavefront
{
    public struct Face
    {

        public Face(int length) 
        {
            this.Vertices = new Vector3f[length];
        }

        public Face(Vector3f[] pVertices)
        {
            this.Vertices = pVertices;
        }

        public Vector3f[] Vertices;
    }
}
