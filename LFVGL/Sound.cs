using System;
using System.Collections.Generic;
using System.Text;

namespace LFVGL
{
    public class Sound
    {
        public Sound(string fileName)
        {
            strFileName = fileName;
            this.LoadSound();
        }

        private void LoadSound()
        {
            using (System.IO.FileStream rd = new System.IO.FileStream(this.strFileName, System.IO.FileMode.Open))
            {
                this.dataSound = new byte[rd.Length];
                rd.Read(this.dataSound, 0, this.dataSound.Length);
                rd.Close();
            }
        }

        private string strFileName;
        public string FileName
        {
            get { return strFileName; }
            set { strFileName = value; }
        }

        private byte[] dataSound;

        public void Play()
        {
            WinAPIUtil.PlaySound(dataSound, IntPtr.Zero, WinAPIUtil.SND_ASYNC);
        }
	
    }
}
