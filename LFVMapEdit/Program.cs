using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LFVMapEdit
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			//Application.Run(new FrmMapEdit());

            FrmMapEdit frm = new FrmMapEdit();
            frm.Show();

            XnaMapControl xmc = new XnaMapControl(frm.DrawSurface);
            frm.XnaGame = xmc;
            xmc.Run();
		}
	}
}