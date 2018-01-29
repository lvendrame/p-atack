using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace LFVPack.Wavefront
{
    public class ObjectWavefrontReader
    {
        const string GROUP = "o";
        const string VERTEX = "v";
        const string VERTEX_NORMAL = "vn";
        const string VERTEX_TEXTURE = "vf";
        const string FACE = "f";
        const string COMMENT = "#";
        const string USE_MATERIAL = "usemtl";
        const string UNKNOW = "s";


        public ResponseObj Reader(StreamReader obj)
        {
            ResponseObj ret = new ResponseObj();
            ret.Groups = new List<Group>();
            ret.Materials = new MaterialList();

            lstVertices = new List<Vector3f>();
            lstNormalVertices = new List<Vector3f>();
            lstTextureVertices = new List<Vector3f>();
            dataLine = this.ReaderLine(obj);
            while (!obj.EndOfStream)
            {
                TypeLine tp = this.GetTypeLine(dataLine[0]);
                switch (tp)
                {
                    case TypeLine.GROUP:
                        ret.Groups.Add(this.ReaderGroup(obj, true));
                        break;
                    case TypeLine.VERTEX:
                    case TypeLine.VERTEX_NORMAL:
                    case TypeLine.VERTEX_TEXTURE:
                    case TypeLine.FACE:
                        ret.Groups.Add(this.ReaderGroup(obj, false));
                        break;
                    case TypeLine.OTHER:
                        ret.Materials.Add(this.ReaderMaterial(obj));
                        break;
                    default:
                        dataLine = this.ReaderLine(obj);
                        break;
                }
            }

            lstVertices.Clear();
            lstNormalVertices.Clear();
            lstTextureVertices.Clear();

            return ret;
        }

        private Material ReaderMaterial(StreamReader obj)
        {
            TypeLine tp = TypeLine.OTHER;

            while (tp == TypeLine.OTHER)
            {
                dataLine = ReaderLine(obj);
                tp = this.GetTypeLine(dataLine[0]);
            }
            return new Material();
        }

        string[] dataLine = null;
        List<Vector3f> lstVertices = null;
        List<Vector3f> lstNormalVertices = null;
        List<Vector3f> lstTextureVertices = null;
        public Group ReaderGroup(StreamReader obj, bool addName)
        {
            List<Face> lstFaces = new List<Face>();
            Material material = null;
            string name = string.Empty;
            if (addName)
            {
                name = dataLine[1].Trim();
                dataLine = this.ReaderLine(obj);
            }

            TypeLine type = this.GetTypeLine(dataLine[0]);
            while (type != TypeLine.OTHER && type != TypeLine.GROUP)
            {
                switch (type)
                {
                    case TypeLine.VERTEX:
                        lstVertices.Add(this.ReadVertex(dataLine));
                        break;
                    case TypeLine.VERTEX_NORMAL:
                        lstVertices.Add(this.ReadVertex(dataLine));
                        break;
                    case TypeLine.VERTEX_TEXTURE:
                        lstTextureVertices.Add(this.ReadVertex(dataLine));
                        break;
                    case TypeLine.FACE:
                        lstFaces.Add(this.ReadFace(dataLine, lstVertices));
                        break;
                }

                if (type == TypeLine.OTHER)
                    break;

                dataLine = this.ReaderLine(obj);
                type = this.GetTypeLine(dataLine[0]);
            }
            return this.BuilderGroup(name, lstFaces, lstNormalVertices, lstTextureVertices, material);
        }

        private Face ReadFace(string[] dataLine, List<Vector3f> lstVertices)
        {
            Face f = new Face(dataLine.Length - 1);
            for (int i = 1; i < dataLine.Length; i++)
            {
                string[] parts = dataLine[i].Split('/');
                f.Vertices[i - 1] = lstVertices[int.Parse(parts[0]) - 1];
            }
            return f;
        }

        private Vector3f ReadVertex(string[] dataLine)
        {
            Vector3f v;
            v.X = float.Parse(dataLine[1]);
            v.Y = float.Parse(dataLine[2]);
            v.Z = float.Parse(dataLine[3]);
            return v;
        }

        private Group BuilderGroup(string name, List<Face> lstFaces, List<Vector3f> lstNormalVertices, List<Vector3f> lstTextureVertices, Material material)
        {
            Group g = new Group();
            g.Name = name;
            g.Faces = lstFaces.ToArray();
            g.TextureMap = lstTextureVertices.ToArray();
            g.Material = material;
            return g;
        }

        public TypeLine GetTypeLine(string line)
        {
            string tp = line.Trim();
            switch (tp)
            {
                case VERTEX:
                    return TypeLine.VERTEX;
                case FACE:
                    return TypeLine.FACE;
                case VERTEX_TEXTURE:
                    return TypeLine.VERTEX_TEXTURE;
                case VERTEX_NORMAL:
                    return TypeLine.VERTEX_NORMAL;
                case GROUP:
                    return TypeLine.GROUP;
                case COMMENT:
                    return TypeLine.COMMENT;
                case USE_MATERIAL:
                    return TypeLine.USER_MATERIAL;
                case UNKNOW:
                    return TypeLine.UNKNOW;
                default:
                    return TypeLine.OTHER;
            }
        }

        public string[] ReaderLine(StreamReader obj)
        {
            if (obj.EndOfStream)
                return new string[] { "END" };

            return obj.ReadLine().Split(' ');
        }

        public class ResponseObj
        {
            public List<Group> Groups;
            public MaterialList Materials;
        }
    }

    public enum TypeLine
    {
        GROUP,
        VERTEX,
        VERTEX_NORMAL,
        VERTEX_TEXTURE,
        FACE,
        COMMENT,
        USER_MATERIAL,
        UNKNOW,
        OTHER
    }

    
}
