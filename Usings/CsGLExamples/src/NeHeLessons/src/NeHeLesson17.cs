#region BSD License
/*
 BSD License
Copyright (c) 2002, Amir Ghezelbash, Lloyd Dupont, Randy Ridge, The CsGL Development Team
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
 *		And Modified By Giuseppe D'Agata (waveform@tiscalinet.it)
 *		If You've Found This Code Useful, Please Let Me Know.
 *		Visit My Site At nehe.gamedev.net
 */
#endregion Original Credits / License

using CsGL.Basecode;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Windows.Forms;

#region AssemblyInfo
[assembly: AssemblyCompany("The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyCopyright("2002 The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyDescription("NeHe Lesson 17")]
[assembly: AssemblyProduct("NeHe Lesson 17")]
[assembly: AssemblyTitle("NeHe Lesson 17")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace NeheLessons {
	///<Summery>
	///NeHe Lesson 17 -- 2D Texture Font (http://nehe.gamedev.net)
	///Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class NeHeLesson17 : Model {
		// --- Fields ---
		#region Private Fields
		private static bool sbp;														// Space Bar Pressed?
		private static uint dbase;														// Base Display List For The Font
		private static uint loop;														// Generic Loop Variable
		private static float cnt1;														// 1st Counter Used To Move Text & For Coloring
		private static float cnt2;														// 2nd Counter Used To Move Text & For Coloring
		private static float cnt3;														// 3rd Counter Used To Move Text & For Coloring
		private static bool movement = true;											// Move The Text?
		private static uint[] texture = new uint[2];									// Storage For Our Font Texture
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Lesson title.
		/// </summary>
		public override string Title {
			get {
				return "NeHe Lesson 17 -- 2D Texture Font";
			}
		}

		/// <summary>
		/// Lesson description.
		/// </summary>
		public override string Description {
			get {
				return "This lesson demonstrates using texture mapped quads to display text.  You will learn how to read one of 256 different characters from a 256x256 texture map, and finally, how to place each character on the screen using pixels rather than units.";
			}
		}

		/// <summary>
		/// Lesson URL.
		/// </summary>
		public override string Url {
			get {
				return "http://nehe.gamedev.net/tutorials/lesson.asp?l=17";
			}
		}
		#endregion Public Properties

		// --- Entry Point ---
		#region Main()
		/// <summary>
		/// Application's entry point, runs this NeHe lesson.
		/// </summary>
		public static void Main() {														// Entry Point
			App.Run(new NeHeLesson17());												// Run Our NeHe Lesson As A Windows Forms Application
		}
		#endregion Main()

		// --- Basecode Methods ---
		#region Initialize()
		/// <summary>
		/// Overrides OpenGL's initialization.
		/// </summary>
		public override void Initialize() {
			base.Initialize();															// Run The Base Initialization
			glEnable(GL_TEXTURE_2D);													// Enable 2D Texture Mapping
			glBlendFunc(GL_SRC_ALPHA, GL_ONE);											// Select The Type Of Blending
			
			LoadTextures();																// Jump To Texture Loading Routine

			BuildFont();																// Build The Font
		}
		#endregion Initialize

		#region Draw()
		/// <summary>
		/// Draws NeHe Lesson 27 scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All Our Drawing
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);							// Clear The Screen And The Depth Buffer
			glLoadIdentity();															// Reset The Modelview Matrix

			glBindTexture(GL_TEXTURE_2D, texture[1]);									// Select Our Second Texture
			glTranslatef(0.0f, 0.0f, -5.0f);											// Move Into The Screen 5 Units
			glRotatef(45.0f, 0.0f, 0.0f, 1.0f);											// Rotate On The Z Axis 45 Degrees (Clockwise)
			glRotatef(cnt1 * 30.0f, 1.0f, 1.0f, 0.0f);									// Rotate On The X & Y Axis By cnt1 (Left To Right)
			glDisable(GL_BLEND);														// Disable Blending Before We Draw In 3D
			glColor3f(1.0f, 1.0f, 1.0f);												// Bright White
			glBegin(GL_QUADS);															// Draw Our First Texture Mapped Quad
				glTexCoord2d(0.0f, 0.0f);												// First Texture Coord
				glVertex2f(-1.0f, 1.0f);												// First Vertex
				glTexCoord2d(1.0f, 0.0f);												// Second Texture Coord
				glVertex2f(1.0f, 1.0f);													// Second Vertex
				glTexCoord2d(1.0f, 1.0f);												// Third Texture Coord
				glVertex2f(1.0f, -1.0f);												// Third Vertex
				glTexCoord2d(0.0f, 1.0f);												// Fourth Texture Coord
				glVertex2f(-1.0f, -1.0f);												// Fourth Vertex
			glEnd();																	// Done Drawing The First Quad
			glRotatef(90.0f, 1.0f, 1.0f, 0.0f);											// Rotate On The X & Y Axis By 90 Degrees (Left To Right)
			glBegin(GL_QUADS);															// Draw Our Second Texture Mapped Quad
				glTexCoord2d(0.0f, 0.0f);												// First Texture Coord
				glVertex2f(-1.0f, 1.0f);												// First Vertex
				glTexCoord2d(1.0f, 0.0f);												// Second Texture Coord
				glVertex2f(1.0f, 1.0f);													// Second Vertex
				glTexCoord2d(1.0f, 1.0f);												// Third Texture Coord
				glVertex2f(1.0f, -1.0f);												// Third Vertex
				glTexCoord2d(0.0f, 1.0f);												// Fourth Texture Coord
				glVertex2f(-1.0f, -1.0f);												// Fourth Vertex
			glEnd();																	// Done Drawing Our Second Quad
			glEnable(GL_BLEND);															// Enable Blending

			glLoadIdentity();															// Reset The View
			// Pulsing Colors Based On Text Position
			glColor3f(1.0f * (float)(Math.Cos(cnt1)), 1.0f * (float)(Math.Sin(cnt2)), 1.0f - 0.5f * (float)(Math.Cos(cnt1 + cnt2)));
			// Print NeHe Text To The Screen
			GlPrint((int)((280 + 250 * Math.Cos(cnt1))), (int)(235 + 200 * Math.Sin(cnt2)), "NeHe", 0);

			// Pulsing Colors Based On Text Position
			glColor3f(1.0f * (float)(Math.Sin(cnt2)), 1.0f - 0.5f * (float)(Math.Cos(cnt1 + cnt2)), 1.0f * (float)(Math.Cos(cnt1)));
			// Print GL Text To The Screen
			GlPrint((int)((280 + 230 * Math.Cos(cnt2))), (int)(235 + 200 * Math.Sin(cnt1)), "OpenGL", 1);

			glColor3f(1.0f, 1.0f, 1.0f);												// Set Color To White
			// Print CsGL Text To The Screen
			GlPrint((int)((280 + 230 * Math.Cos(cnt3))), (int)(235 + 200 * Math.Sin(cnt3)), "CsGL", 1);

			glColor3f(0.0f, 0.0f, 1.0f);												// Set Color To Blue
			// Print Giuseppe D'Agata Text To The Screen
			GlPrint((int)(240 + 200 * Math.Cos((cnt2 + cnt1) / 5)), 2, "Giuseppe D'Agata", 0);

			glColor3f(1.0f, 1.0f, 1.0f);												// Set Color To White
			// Print Giuseppe D'Agata Offset Text To The Screen
			GlPrint((int)(242 + 200 * Math.Cos((cnt2 + cnt1) / 5)), 2, "Giuseppe D'Agata", 0);

			if(movement) {																// If Text Should Mode
				cnt1 += 0.01f;															// Increase The First Counter
				cnt2 += 0.0081f;														// Increase The Second Counter
				cnt3 += 0.00081f;														// Increase The Third Counter
			}
		}
		#endregion Draw()

		#region InputHelp()
		/// <summary>
		/// Overrides default input help, supplying lesson-specific help information.
		/// </summary>
		public override void InputHelp() {
			base.InputHelp();															// Set Up The Default Input Help

			DataRow dataRow;															// Row To Add

			dataRow = InputHelpDataTable.NewRow();										// Space Bar - Toggle Movement
			dataRow["Input"] = "Space Bar";
			dataRow["Effect"] = "Toggle Movement On / Off";
			if(movement) {
				dataRow["Current State"] = "On";
			}
			else {
				dataRow["Current State"] = "Off";
			}
			InputHelpDataTable.Rows.Add(dataRow);
		}
		#endregion InputHelp()

		#region ProcessInput()
		/// <summary>
		/// Overrides default input handling, adding lesson-specific input handling.
		/// </summary>
		public override void ProcessInput() {
			base.ProcessInput();														// Handle The Default Basecode Keys

			if(KeyState[(int) Keys.Space] && !sbp) {									// Is Space Bar Key Being Pressed And Not Held Down?
				sbp = true;																// sbp Becomes true
				movement = !movement;													// Toggle Movement true / false
				UpdateInputHelp();														// Update The Input Help Screen
			}
			if(!KeyState[(int) Keys.Space]) {											// Has Space Bar Key Been Released?
				sbp = false;															// If So, sbp Becomes false
			}
		}
		#endregion ProcessInput()

		// --- Lesson Methods ---
		#region BuildFont()
		/// <summary>
		/// Builds the font.
		/// </summary>
		private void BuildFont() {
			float cx;																	// Holds Our X Character Coordinate
			float cy;																	// Holds Our Y Character Coordinate

			dbase = glGenLists(256);													// Create 256 Display Lists
			glBindTexture(GL_TEXTURE_2D, texture[0]);									// Select Our Font Texture

			for(loop = 0; loop < 256; loop++) {											// Loop Through All 256 Display Lists
				cx = (float)(loop % 16) / 16.0f;										// X Position Of Current Character
				cy = (float)(loop / 16) / 16.0f;										// Y Position Of Current Character

				glNewList(dbase + loop, GL_COMPILE);									// Start Building A List
					glBegin(GL_QUADS);													// Use A Quad For Each Character
						glTexCoord2f(cx, 1 - cy - 0.0625f);								// Texture Coord (Bottom Left)
						glVertex2i(0, 0);												// Vertex Coord (Bottom Left)
						glTexCoord2f(cx + 0.0625f, 1 - cy - 0.0625f);					// Texture Coord (Bottom Right)
						glVertex2i(16, 0);												// Vertex Coord (Bottom Right)
						glTexCoord2f(cx + 0.0625f, 1 - cy);								// Texture Coord (Top Right)
						glVertex2i(16, 16);												// Vertex Coord (Top Right)
						glTexCoord2f(cx, 1 - cy);										// Texture Coord (Top Left)
						glVertex2i(0, 16);												// Vertex Coord (Top Left)
					glEnd();															// Done Building Our Quad (Character)
					glTranslated(10, 0, 0);												// Move To The Right Of The Character
				glEndList();															// Done Building The Display List
			}
		}
		#endregion BuildFont()

		#region GlPrint
		/// <summary>
		/// Prints some text.
		/// </summary>
		/// <param name="x">X position.</param>
		/// <param name="y">Y position.</param>
		/// <param name="str">Text to print.</param>
		/// <param name="cset">Which character set?  0 is normal, 1 is italics.</param>
		private void GlPrint(int x, int y, string str, int cset) {
			if (cset > 1) {																// If cset Is Invalid
				cset = 1;																// Set It To A Valid Value
			}
			glBindTexture(GL_TEXTURE_2D, texture[0]);									// Select Our Font Texture
			glDisable(GL_DEPTH_TEST);													// Disables Depth Testing
			glMatrixMode(GL_PROJECTION);												// Select The Projection Matrix
			glPushMatrix();																// Store The Projection Matrix
			glLoadIdentity();															// Reset The Projection Matrix
			glOrtho(0, 640, 0, 480, -1, 1);												// Set Up An Ortho Screen
			glMatrixMode(GL_MODELVIEW);													// Select The Modelview Matrix
			glPushMatrix();																// Store The Modelview Matrix
			glLoadIdentity();															// Reset The Modelview Matrix
			glTranslated(x, y, 0);														// Position The Text (0, 0 - Bottom Left)
			glListBase((uint)(dbase - 32 + (128 * cset)));								// Choose The Font Set (0 or 1)
			glCallLists(str.Length, GL_UNSIGNED_SHORT, str);							// Write The Text To The Screen
			glMatrixMode(GL_PROJECTION);												// Select The Projection Matrix
			glPopMatrix();																// Restore The Old Projection Matrix
			glMatrixMode(GL_MODELVIEW);													// Select The Modelview Matrix
			glPopMatrix();																// Restore The Old Projection Matrix
			glEnable(GL_DEPTH_TEST);													// Enables Depth Testing
		}
		#endregion GlPrint(int x, int y, string str, int cset)

		#region LoadTextures()
		/// <summary>
		/// Loads and creates the textures.
		/// </summary>
		private void LoadTextures() {
			// The Files To Load
			string[] filename = {@"..\..\data\NeHeLesson17\Font.bmp", @"..\..\data\NeHeLesson17\Bumps.bmp"};
			Bitmap bitmap = null;														// The Bitmap Image For Our Texture
			Rectangle rectangle;														// The Rectangle For Locking The Bitmap In Memory
			BitmapData bitmapData = null;												// The Bitmap's Pixel Data

			// Load The Bitmaps
			try {
				glGenTextures(2, texture);												// Create 2 Textures

				for(loop = 0; loop < 2; loop++) {
					bitmap = new Bitmap(filename[loop]);								// Load The File As A Bitmap
					bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);					// Flip The Bitmap Along The Y-Axis
					rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);		// Select The Whole Bitmap
				
					// Get The Pixel Data From The Locked Bitmap
					bitmapData = bitmap.LockBits(rectangle, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

					// Create Linear Filtered Texture
					glBindTexture(GL_TEXTURE_2D, texture[loop]);
					glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
					glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR); 
					glTexImage2D(GL_TEXTURE_2D, 0, (int) GL_RGB8, bitmap.Width, bitmap.Height, 0, GL_BGR_EXT, GL_UNSIGNED_BYTE, bitmapData.Scan0);
				}
			}
			catch(Exception e) {
				// Handle Any Exceptions While Loading Textures, Exit App
				string errorMsg = "An Error Occurred While Loading Texture:\n\t" + filename + "\n" + "\n\nStack Trace:\n\t" + e.StackTrace + "\n";
				MessageBox.Show(errorMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				App.Terminate();
			}
			finally {
				if(bitmap != null) {
					bitmap.UnlockBits(bitmapData);										// Unlock The Pixel Data From Memory
					bitmap.Dispose();													// Clean Up The Bitmap
				}
			}
		}
		#endregion LoadTextures()
	}
}