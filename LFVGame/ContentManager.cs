using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using LFVMapControler;
using System.IO;

namespace LFVGame
{
    public static class ContentManager
    {
        public static string FOLDER_PATH = GetContentFolder();
        public static string BRICKS_PATH;
        public static string SPRITES_PATH;
        public static string SOUNDS_PATH;
        public static string MAPS_PATH;

        public static string GetContentFolder()
        {
            //string folder = AppDomain.CurrentDomain.BaseDirectory + "Content\\";
            string folder = System.IO.Path.Combine(System.Environment.CurrentDirectory, "Content");
            BRICKS_PATH = System.IO.Path.Combine(folder, "Bricks");
            SPRITES_PATH = System.IO.Path.Combine(folder, "Sprites");
            SOUNDS_PATH = System.IO.Path.Combine(folder, "Sounds");
            MAPS_PATH = System.IO.Path.Combine(folder, "Maps");
            return folder;
        }
        public static string GetPath(PathType type)
        {
            switch (type)
            {
                case PathType.Bricks:
                    return BRICKS_PATH;
                case PathType.Sprites:
                    return SPRITES_PATH;
                case PathType.Sounds:
                    return SOUNDS_PATH;
                case PathType.Maps:
                    return MAPS_PATH;
                case PathType.Root:
                default:
                    return FOLDER_PATH;
            }
        }

        public static T GetContent<T>(PathType contentType, string contentFileName) where T:class
        {
            string path = Path.Combine(GetPath(contentType), contentFileName);
            object retT  = null;
            Type tType = typeof(T);
            if (tType == typeof(Image))
                retT = Bitmap.FromFile(path);
            else if (tType == typeof(System.Media.SoundPlayer))
                retT = new System.Media.SoundPlayer(path);
            else if (tType == typeof(MapResponse))
                retT = FileControler.Open(path);
            else
                retT = System.IO.File.OpenRead(path);

            return retT as T;
        }

        public enum PathType
        {
            Root,
            Bricks,
            Sprites,
            Sounds,
            Maps
        }
    }
}
