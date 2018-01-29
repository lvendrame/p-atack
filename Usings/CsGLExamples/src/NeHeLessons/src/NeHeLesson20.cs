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
[assembly: AssemblyDescription("NeHe Lesson 20")]
[assembly: AssemblyProduct("NeHe Lesson 20")]
[assembly: AssemblyTitle("NeHe Lesson 20")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace NeHeLessons {
	/// <summary>
	/// NeHe Lesson 20 -- Masking (http://nehe.gamedev.net)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class NeHeLesson20 : Model {
		// --- Fields ---
		#region Private Fields
		private static bool masking = true;												// Masking On / Off
		private static bool mp;															// M Pressed?
		private static bool sbp;														// Space Bar Pressed?
		private static bool scene;														// Which Scene To Draw
		private static float roll;														// Rolling Texture
		private static int loop;														// Generic Loop Variable
		private static uint[] texture = new uint[5];									// Storage For Our 5 Textures
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Lesson title.
		/// </summary>
		public override string Title {
			get {
				return "NeHe Lesson 20 -- Masking";
			}
		}

		/// <summary>
		/// Lesson description.
		/// </summary>
		public override string Description {
			get {
				return "This lesson demonstrates image masking.  Previously we've been blending images onscreen, which produces a transparency effect, but now you'll learn how to composite images on top of one another, allowing the top image to retain its solidity and still be able to see images behind.";
			}
		}

		/// <summary>
		/// Lesson URL.
		/// </summary>
		public override string Url {
			get {
				return "http://nehe.gamedev.net/tutorials/lesson.asp?l=20";
			}
		}
		#endregion Public Properties

		// --- Entry Point ---
		#region Main()
		/// <summary>
		/// Application's entry point, runs this NeHe lesson.
		/// </summary>
		public static void Main() {														// Entry Point
			App.Run(new NeHeLesson20());												// Run Our NeHe Lesson As A Windows Forms Application
		}
		#endregion Main()

		// --- Basecode Methods ---
		#region Initialize()
		/// <summary>
		/// Overrides OpenGL's initialization.
		/// </summary>
		public override void Initialize() {
			base.Initialize();															// Run The Base Initialization
			glEnable(GL_TEXTURE_2D);													// Enable Texture Mapping

			LoadTextures();																// Jump To Texture Loading Routine
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws NeHe Lesson 20 scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);							// Clear The Screen And The Depth Buffer
			glLoadIdentity();															// Reset The Modelview Matrix
			glTranslatef(0.0f, 0.0f, -2.0f);											// Move Into The Screen 2 Units

			glBindTexture(GL_TEXTURE_2D, texture[0]);									// Select Our Logo Texture
			glBegin(GL_QUADS);															// Start Drawing A Textured Quad
				glTexCoord2f(0.0f, -roll + 0.0f); glVertex3f(-1.1f, -1.1f,  0.0f);		// Bottom Left
				glTexCoord2f(3.0f, -roll + 0.0f); glVertex3f( 1.1f, -1.1f,  0.0f);		// Bottom Right
				glTexCoord2f(3.0f, -roll + 3.0f); glVertex3f( 1.1f,  1.1f,  0.0f);		// Top Right
				glTexCoord2f(0.0f, -roll + 3.0f); glVertex3f(-1.1f,  1.1f,  0.0f);		// Top Left
			glEnd();																	// Done Drawing The Quad

			glEnable(GL_BLEND);															// Enable Blending
			glDisable(GL_DEPTH_TEST);													// Disable Depth Testing

			if(masking) {																// Is Masking Enabled?
				glBlendFunc(GL_DST_COLOR, GL_ZERO);										// Blend Screen Color With Zero (Black)
			}

			if(scene) {																	// Are We Drawing The Second Scene?
				glTranslatef(0.0f, 0.0f, -1.0f);										// Translate Into The Screen One Unit
				glRotatef(roll * 360, 0.0f, 0.0f, 1.0f);								// Rotate On The Z Axis 360 Degrees
				if(masking) {															// Is Masking On?
					glBindTexture(GL_TEXTURE_2D, texture[3]);							// Select The Second Mask Texture
					glBegin(GL_QUADS);													// Start Drawing A Textured Quad
						glTexCoord2f(0.0f, 0.0f); glVertex3f(-1.1f, -1.1f,  0.0f);		// Bottom Left
						glTexCoord2f(1.0f, 0.0f); glVertex3f( 1.1f, -1.1f,  0.0f);		// Bottom Right
						glTexCoord2f(1.0f, 1.0f); glVertex3f( 1.1f,  1.1f,  0.0f);		// Top Right
						glTexCoord2f(0.0f, 1.0f); glVertex3f(-1.1f,  1.1f,  0.0f);		// Top Left
					glEnd();															// Done Drawing The Quad
				}

				glBlendFunc(GL_ONE, GL_ONE);											// Copy Image 2 Color To The Screen
				glBindTexture(GL_TEXTURE_2D, texture[4]);								// Select The Second Image Texture
				glBegin(GL_QUADS);														// Start Drawing A Textured Quad
					glTexCoord2f(0.0f, 0.0f); glVertex3f(-1.1f, -1.1f,  0.0f);			// Bottom Left
					glTexCoord2f(1.0f, 0.0f); glVertex3f( 1.1f, -1.1f,  0.0f);			// Bottom Right
					glTexCoord2f(1.0f, 1.0f); glVertex3f( 1.1f,  1.1f,  0.0f);			// Top Right
					glTexCoord2f(0.0f, 1.0f); glVertex3f(-1.1f,  1.1f,  0.0f);			// Top Left
				glEnd();																// Done Drawing The Quad
			}
			else {																		// Otherwise
				if(masking) {															// Is Masking On?
					glBindTexture(GL_TEXTURE_2D, texture[1]);							// Select The First Mask Texture
					glBegin(GL_QUADS);													// Start Drawing A Textured Quad
						// Bottom Left
						glTexCoord2f(roll + 0.0f, 0.0f); glVertex3f(-1.1f, -1.1f,  0.0f);
						// Bottom Right
						glTexCoord2f(roll + 4.0f, 0.0f); glVertex3f( 1.1f, -1.1f,  0.0f);
						// Top Right
						glTexCoord2f(roll + 4.0f, 4.0f); glVertex3f( 1.1f,  1.1f,  0.0f);
						// Top Left
						glTexCoord2f(roll + 0.0f, 4.0f); glVertex3f(-1.1f,  1.1f,  0.0f);
					glEnd();															// Done Drawing The Quad
				}

				glBlendFunc(GL_ONE, GL_ONE);											// Copy Image 1 Color To The Screen
				glBindTexture(GL_TEXTURE_2D, texture[2]);								// Select The First Image Texture
				glBegin(GL_QUADS);														// Start Drawing A Textured Quad
					glTexCoord2f(roll + 0.0f, 0.0f); glVertex3f(-1.1f, -1.1f,  0.0f);	// Bottom Left
					glTexCoord2f(roll + 4.0f, 0.0f); glVertex3f( 1.1f, -1.1f,  0.0f);	// Bottom Right
					glTexCoord2f(roll + 4.0f, 4.0f); glVertex3f( 1.1f,  1.1f,  0.0f);	// Top Right
					glTexCoord2f(roll + 0.0f, 4.0f); glVertex3f(-1.1f,  1.1f,  0.0f);	// Top Left
				glEnd();																// Done Drawing The Quad
			}

			glEnable(GL_DEPTH_TEST);													// Enable Depth Testing
			glDisable(GL_BLEND);														// Disable Blending

			roll += 0.002f;																// Increase Our Texture Roll Variable
			if(roll > 1.0f) {															// Is Roll Greater Than One
				roll -= 1.0f;															// Subtract 1 From Roll
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

			dataRow = InputHelpDataTable.NewRow();										// Space Bar - Toggle Scene
			dataRow["Input"] = "Space Bar";
			dataRow["Effect"] = "Toggle Scene";
			if(scene) {
				dataRow["Current State"] = "Scene 1";
			}
			else {
				dataRow["Current State"] = "Scene 2";
			}
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// M - Toggle Masking
			dataRow["Input"] = "M";
			dataRow["Effect"] = "Toggle Masking On / Off";
			if(masking) {
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
				scene = !scene;															// Toggle scene true / false
				UpdateInputHelp();														// Update The Input Help Screen
			}
			if(!KeyState[(int) Keys.Space]) {											// Has Space Bar Key Been Released?
				sbp = false;															// If So, sbp Becomes false
			}

			if(KeyState[(int) Keys.M] && !mp) {											// Is M Key Being Pressed And Not Held Down?
				mp = true;																// mp Becomes true
				masking = !masking;														// Toggle masking true / false
				UpdateInputHelp();														// Update The Input Help Screen
			}
			if(!KeyState[(int) Keys.M]) {												// Has M Key Been Released?
				mp = false;																// If So, mp Becomes false
			}
		}
		#endregion ProcessInput()

		// --- Lesson Methods ---
		#region LoadTextures()
		/// <summary>
		/// Loads and creates the textures.
		/// </summary>
		private void LoadTextures() {
			string[] filename = {														// The Files To Load
				@"..\..\data\NeHeLesson20\Logo.bmp",									// Logo Texture
				@"..\..\data\NeHeLesson20\Mask1.bmp",									// First Mask
				@"..\..\data\NeHeLesson20\Image1.bmp",									// First Image
				@"..\..\data\NeHeLesson20\Mask2.bmp",									// Second Mask
				@"..\..\data\NeHeLesson20\Image2.bmp"									// Second Image
			};

			Bitmap bitmap = null;														// The Bitmap Image For Our Texture
			Rectangle rectangle;														// The Rectangle For Locking The Bitmap In Memory
			BitmapData bitmapData = null;												// The Bitmap's Pixel Data

			// Load The Bitmap
			try {
				glGenTextures(5, texture);												// Create 5 Textures

				for(loop = 0; loop < 5; loop++) {
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