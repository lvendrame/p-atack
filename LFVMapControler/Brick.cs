using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace LFVMapControler
{
	[Serializable()]
	public class Brick
	{
		[System.Xml.Serialization.XmlIgnore()]
		public int fint_Index;

		private string fstr_ImagePath;
		public string ImagePath
		{
			get { return fstr_ImagePath; }
			set { fstr_ImagePath = value; }
		}

        private bool fbln_IsNew = true;
        public bool IsNew
        {
            get { return fbln_IsNew; }
            set { fbln_IsNew = value; }
        }


		[System.Xml.Serialization.XmlIgnore()]
		private Image fimg_Image;

		[System.Xml.Serialization.XmlIgnore()]
		public Image Image
		{
			get { return fimg_Image; }
			set { fimg_Image = value; }
		}
	}
}
