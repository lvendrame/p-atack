#region BSD License
/*
 BSD License
Copyright (c) 2002, Randy Ridge, The CsGL Development Team
http://csgl.sourceforge.net/
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions
are met:

1. Redistributions of source code must retain the above copyright notice,
   this list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice,
   this list of conditions and the following disclaimer in the documentation
   and/or other materials provided with the distribution.

3. Neither the name of The CsGL Development Team nor the names of its
   contributors may be used to endorse or promote products derived from this
   software without specific prior written permission.

   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS 
   "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
   LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS
   FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE
   COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
   INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,
   BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
   LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
   CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
   LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN
   ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
   POSSIBILITY OF SUCH DAMAGE.
 */
#endregion BSD License

#region Original Credits / License
/*
 *		This Code Was Created By Jeff Molofee 2000
 *		Modified by Shawn T. to handle (%3.2f, num) parameters.
 *		A HUGE Thanks To Fredric Echols For Cleaning Up
 *		And Optimizing The Base Code, Making It More Flexible!
 *		If You've Found This Code Useful, Please Let Me Know.
 *		Visit My Site At nehe.gamedev.net
 */
#endregion Original Credits / License

using CsGL.Basecode;
using CsGL.OpenGL;
using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;

#region AssemblyInfo
[assembly: AssemblyCompany("The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyCopyright("2002 The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyDescription("NeHe Lesson 13")]
[assembly: AssemblyProduct("NeHe Lesson 13")]
[assembly: AssemblyTitle("NeHe Lesson 13")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace NeHeLessons {
	/// <summary>
	/// NeHe Lesson 13 -- Bitmap Fonts (http://nehe.gamedev.net)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class NeHeLesson13 : Model {
		// --- Fields ---
		#region Private Fields
		private static uint dbase;														// Base Display List For The Font Set
		private static float cnt1;														// 1st Counter Used To Move Text & For Coloring
		private static float cnt2;														// 2nd Counter Used To Move Text & For Coloring
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Lesson title.
		/// </summary>
		public override string Title {
			get {
				return "NeHe Lesson 13 -- Bitmap Fonts";
			}
		}

		/// <summary>
		/// Lesson description.
		/// </summary>
		public override string Description {
			get {
				return "This lesson will teach you how to create and use bitmap fonts.  Bitmaps font's are 2D scalable fonts, they can not be rotated. They always face forward.";
			}
		}

		/// <summary>
		/// Lesson URL.
		/// </summary>
		public override string Url {
			get {
				return "http://nehe.gamedev.net/tutorials/lesson.asp?l=13";
			}
		}
		#endregion Public Properties

		// --- Entry Point ---
		#region Main()
		/// <summary>
		/// Application's entry point, runs this NeHe lesson.
		/// </summary>
		public static void Main() {														// Entry Point
			App.Run(new NeHeLesson13());												// Run Our NeHe Lesson As A Windows Forms Application
		}
		#endregion Main()

		// --- Basecode Methods ---
		#region Initialize()
		/// <summary>
		/// Overrides OpenGL's initialization.
		/// </summary>
		public override void Initialize() {
			base.Initialize();															// Run The Base Initialization

			BuildFont();																// Build The Font
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws NeHe Lesson 13 scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			Font font = new Font("Courier New", 24.0f, FontStyle.Bold);
			dbase = glGenLists(96);														// Storage For 96 Characters
			wglUseFontBitmaps(App.Form.Handle, 32, 96, dbase);
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);							// Clear Screen And Depth Buffer
			glLoadIdentity();															// Reset The Current Modelview Matrix
			glTranslatef(0.0f, 0.0f, -1.0f);											// Move One Unit Into The Screen
			// Pulsing Colors Based On Text Position
			glColor3f(1.0f * (float) Math.Cos(cnt1), 1.0f * (float) Math.Sin(cnt2), 1.0f - 0.5f * (float) Math.Cos(cnt1+cnt2));
			// Position The Text On The Screen
			glRasterPos2f(-0.45f + 0.05f * (float) Math.Cos(cnt1), 0.32f * (float) Math.Sin(cnt2));
 			GlPrint("Active OpenGL Text With NeHe - " + cnt1);							// Print GL Text To The Screen
			cnt1 += 0.051f;																// Increase The First Counter
			cnt2 += 0.005f;																// Increase The First Counter
		}
		#endregion Draw()

		// --- Lesson Methods ---
		private static void BuildFont() {
		}

		private static void GlPrint(string text) {										// Custom GL "Print" Routine
			glPushAttrib(GL_LIST_BIT);													// Pushes The Display List Bits
			glListBase(dbase - 32);														// Sets The Base Character to 32
			glCallLists(text.Length, GL_UNSIGNED_SHORT, text);							// Draws The Display List Text
			glPopAttrib();																// Pops The Display List Bits
		}

		// --- Externs ---
		[DllImport("opengl32.dll")]
		private static extern void wglUseFontBitmaps(IntPtr hdc, uint first, uint count, uint listBase);
	}
}