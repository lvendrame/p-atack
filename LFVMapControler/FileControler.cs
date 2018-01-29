using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Runtime.InteropServices;
using System.IO;

namespace LFVMapControler
{
	public static class FileControler
	{
		public const string EXTENSION_FILE = "ppm";
		public const string NAME_EXTENSION_FILE = "Ponjo Ponjo Map File";
		public const string FILTER = FileControler.NAME_EXTENSION_FILE + "|*." + FileControler.EXTENSION_FILE;

		public static void Save(string pstr_FullPathFileName, MatrixMapCell mtxMapCell, List<Brick> lstBricks, int pint_TileWidth, int pint_TileHeight)
		{
            string path = FileControler.GetPath(pstr_FullPathFileName);
            SaveBricks(lstBricks, path);
			MapInformation info = new MapInformation(mtxMapCell, lstBricks, pint_TileWidth, pint_TileHeight);
			XmlSerializer xSerializer = new XmlSerializer(typeof(MapInformation));
			System.IO.StreamWriter sWriter = new System.IO.StreamWriter(pstr_FullPathFileName, false);
			xSerializer.Serialize(sWriter, info);
			sWriter.Close();
		}

        private static void SaveBricks(List<Brick> lstBricks, string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            path = Path.Combine(path, "Bricks");
            if (!Directory.Exists(path))
                dir.CreateSubdirectory("Bricks");

            for (int i = 0; i < lstBricks.Count; i++)
            {
                Brick brk = lstBricks[i];
                if (brk.IsNew && FileControler.GetPath(brk.ImagePath) != path)
                {
                    brk.Image.Save(Path.Combine(path, brk.ImagePath));
                }
                brk.IsNew = false;
                brk.ImagePath = FileControler.GetFileName(brk.ImagePath);
            }
        }

		public static MapResponse Open(string pstr_FullPathFileName)
		{
			XmlSerializer xSerializer = new XmlSerializer(typeof(MapInformation));
			System.IO.StreamReader sReader = new System.IO.StreamReader(pstr_FullPathFileName);
			MapInformation mrRet = xSerializer.Deserialize(sReader) as MapInformation;
			sReader.Close();
			
			return mrRet.ToMapResponse();
		}

		public static string GetPath(string pstr_FullPathFileName)
		{
            return Path.GetDirectoryName(pstr_FullPathFileName);
			//return pstr_FullPathFileName.Substring(0, pstr_FullPathFileName.LastIndexOf('\\') + 1);
		}

		public static string GetFileName(string pstr_FullPathFileName)
		{
            return Path.GetFileName(pstr_FullPathFileName);
			//return pstr_FullPathFileName.Substring(pstr_FullPathFileName.LastIndexOf('\\') + 1);
		}

		public static byte[] TToBytes<T>(T obj)
		{
			int len = Marshal.SizeOf(obj);
			byte[] buf = new byte[len];

			IntPtr p = Marshal.AllocHGlobal(len);
			Marshal.StructureToPtr(obj, p, false);
			Marshal.Copy(p, buf, 0, len);
			Marshal.FreeHGlobal(p);
			return buf;
		}

		public static T BytesToT<T>(byte[] dataIn)
		{
			GCHandle hDataIn = GCHandle.Alloc(dataIn, GCHandleType.Pinned);
			T ys = (T)Marshal.PtrToStructure(hDataIn.AddrOfPinnedObject(), typeof(T));
			hDataIn.Free();
			return ys;
		}
	}
}
