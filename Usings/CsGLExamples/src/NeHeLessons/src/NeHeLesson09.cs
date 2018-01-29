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
 *		A HUGE Thanks To Fredric Echols For Cleaning Up
 *		And Optimizing The Base Code, Making It More Flexible!
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
[assembly: AssemblyDescription("NeHe Lesson 09")]
[assembly: AssemblyProduct("NeHe Lesson 09")]
[assembly: AssemblyTitle("NeHe Lesson 09")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace NeHeLessons {
	/// <summary>
	/// NeHe Lesson 09 -- Moving Bitmaps In 3D Space (http://nehe.gamedev.net)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class NeHeLesson09 : Model {
		// --- Fields ---
		#region Private Fields
		private static bool twinkle;													// Twinkling Stars On / Off
		private static bool tp;															// T Pressed?

		private const int num = 50;														// Number Of Stars To Draw

		private struct Star {															// Create A Structure For Star
			public byte r, g, b;														// Star's Color
			public float dist;															// Star's Distance From Center
			public float angle;															// Star's Current Angle
		}

		private static Star[] star = new Star[num];										// Make 'star' Array Of 'num' Using Info From The Structure 'Star'

		private static float zoom = -15.0f;												// Viewing Distance Away From Stars
		private static float tilt = 90.0f;												// Tilt The View
		private static float spin;														// Spin Stars

		private static int loop;														// General Loop Variable
		private static uint[] texture = new uint[1];									// Storage For One Texture

		private static Random rand = new Random();										// Randomizer
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Lesson title.
		/// </summary>
		public override string Title {
			get {
				return "NeHe Lesson 09 -- Moving Bitmaps In 3D Space";
			}
		}

		/// <summary>
		/// Lesson description.
		/// </summary>
		public override string Description {
			get {
				return "This lesson covers simple animation, more blending, drawing bitmaps to the screen, and moving them in 3D.  No spinning boxes will be found here.";
			}
		}

		/// <summary>
		/// Lesson URL.
		/// </summary>
		public override string Url {
			get {
				return "http://nehe.gamedev.net/tutorials/lesson.asp?l=09";
			}
		}
		#endregion Public Properties

		// --- Entry Point ---
		#region Main()
		/// <summary>
		/// Application's entry point, runs this NeHe lesson.
		/// </summary>
		public static void Main() {														// Entry Point
			App.Run(new NeHeLesson09());												// Run Our NeHe Lesson As A Windows Forms Application
		}
		#endregion Main()

		// --- Basecode Methods ---
		#region Initialize()
		/// <summary>
		/// Overrides OpenGL's initialization.
		/// </summary>
		public override void Initialize() {
			base.Initialize();															// Run The Base Initialization
			glDisable(GL_DEPTH_TEST);													// Override The Base Initialization's Depth Test
			glEnable(GL_TEXTURE_2D);													// Enable Texture Mapping
			glBlendFunc(GL_SRC_ALPHA, GL_ONE);											// Set The Blending Function For Translucency
			glEnable(GL_BLEND);															// Enable Blending

			LoadTextures();																// Jump To The Texture Loading Routine

			for(loop = 0; loop < num; loop++) {											// Loop Through All The Stars
				star[loop].angle = 0.0f;												// Start All The Stars At Angle Zero
				star[loop].dist = ((float) loop / num) * 5.0f;							// Calculate Distance From The Center
				star[loop].r = (byte) (rand.Next() % 256);								// Give star[loop] A Random Red Intensity
				star[loop].g = (byte) (rand.Next() % 256);								// Give star[loop] A Random Green Intensity
				star[loop].b = (byte) (rand.Next() % 256);								// Give star[loop] A Random Blue Intensity
			}
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws NeHe Lesson 09 scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);							// Clear Screen And Depth Buffer
			glBindTexture(GL_TEXTURE_2D, texture[0]);									// Select Our Texture

			for(loop = 0; loop < num; loop++) {											// Loop Through All The Stars
				glLoadIdentity();														// Reset The View Before We Draw Each Star
				glTranslatef(0.0f, 0.0f, zoom);											// Zoom Into The Screen (Using The Value In 'zoom')
				glRotatef(tilt, 1.0f, 0.0f, 0.0f);										// Tilt The View (Using The Value In 'tilt')
				glRotatef(star[loop].angle, 0.0f, 1.0f, 0.0f);							// Rotate To The Current Star's Angle
				glTranslatef(star[loop].dist, 0.0f, 0.0f);								// Move Forward On The X Plane
				glRotatef(-star[loop].angle, 0.0f, 1.0f, 0.0f);							// Cancel The Current Star's Angle
				glRotatef(-tilt, 1.0f, 0.0f, 0.0f);										// Cancel The Screen Tilt

				if(twinkle) {															// If Twinkling Stars Enabled
					// Assign A Color Using Bytes
					glColor4ub(star[(num-loop)-1].r, star[(num-loop)-1].g, star[(num-loop)-1].b, 255);
					glBegin(GL_QUADS);													// Begin Drawing The Textured Quad
						glTexCoord2f(0.0f, 0.0f); glVertex3f(-1.0f, -1.0f, 0.0f);
						glTexCoord2f(1.0f, 0.0f); glVertex3f( 1.0f, -1.0f, 0.0f);
						glTexCoord2f(1.0f, 1.0f); glVertex3f( 1.0f,  1.0f, 0.0f);
						glTexCoord2f(0.0f, 1.0f); glVertex3f(-1.0f,  1.0f, 0.0f);
					glEnd();															// Done Drawing The Textured Quad
				}

				glRotatef(spin, 0.0f, 0.0f, 1.0f);										// Rotate The Star On The Z Axis
				glColor4ub(star[loop].r, star[loop].g, star[loop].b, 255);				// Assign A Color Using Bytes
				glBegin(GL_QUADS);														// Begin Drawing The Textured Quad
					glTexCoord2f(0.0f, 0.0f); glVertex3f(-1.0f, -1.0f, 0.0f);
					glTexCoord2f(1.0f, 0.0f); glVertex3f( 1.0f, -1.0f, 0.0f);
					glTexCoord2f(1.0f, 1.0f); glVertex3f( 1.0f,  1.0f, 0.0f);
					glTexCoord2f(0.0f, 1.0f); glVertex3f(-1.0f,  1.0f, 0.0f);
				glEnd();																// Done Drawing The Textured Quad

				spin += 0.01f;															// Used To Spin The Stars
				star[loop].angle += ((float) loop / num);								// Changes The Angle Of A Star
				star[loop].dist -= 0.01f;												// Changes The Distance Of A Star

				if(star[loop].dist < 0.0f) {											// Is The Star In The Middle Yet
					star[loop].dist += 5.0f;											// Move The Star 5 Units From The Center
					star[loop].r = (byte) (rand.Next() % 256);							// Give It A New Red Value
					star[loop].g = (byte) (rand.Next() % 256);							// Give It A New Green Value
					star[loop].b = (byte) (rand.Next() % 256);							// Give It A New Blue Value
				}
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

			dataRow = InputHelpDataTable.NewRow();										// Up Arrow - Tilt Up
			dataRow["Input"] = "Up Arrow";
			dataRow["Effect"] = "Tilt Up";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Down Arrow - Tilt Down
			dataRow["Input"] = "Down Arrow";
			dataRow["Effect"] = "Tilt Down";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Page Up - Zoom Out
			dataRow["Input"] = "Page Up";
			dataRow["Effect"] = "Zoom Out";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Page Down - Zoom In
			dataRow["Input"] = "Page Down";
			dataRow["Effect"] = "Zoom In";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// T - Toggle Twinkling
			dataRow["Input"] = "T";
			dataRow["Effect"] = "Toggle Twinkling On / Off";
			if(twinkle) {
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

			if(KeyState[(int) Keys.Up]) {												// Is Up Arrow Being Pressed?
				tilt -= 0.5f;															// If So, Tilt Screen Up
			}

			if(KeyState[(int) Keys.Down]) {												// Is Down Arrow Being Pressed?
				tilt += 0.5f;															// If So, Tilt Screen Down
			}

			if(KeyState[(int) Keys.PageUp]) {											// Is Page Up Being Pressed?
				zoom -= 0.02f;															// If So, Zoom Out
			}

			if(KeyState[(int) Keys.PageDown]) {											// Is Page Down Being Pressed?
				zoom += 0.02f;															// If So, Zoom In
			}

			if(KeyState[(int) Keys.T] && !tp) {											// Is T Key Being Pressed And Not Held Down?
				tp = true;																// tp Becomes true
				twinkle = !twinkle;														// Toggle Twinkling true / false
				UpdateInputHelp();														// Update The Input Help Screen
			}
			if(!KeyState[(int) Keys.T]) {												// Has T Key Been Released?
				tp = false;																// If So, tp Becomes false
			}
		}
		#endregion ProcessInput()

		// --- Lesson Methods ---
		#region LoadTextures()
		/// <summary>
		/// Loads and creates the texture.
		/// </summary>
		private void LoadTextures() {
			string filename = @"..\..\data\NeHeLesson09\Star.bmp";						// The File To Load
			Bitmap bitmap = null;														// The Bitmap Image For Our Texture
			Rectangle rectangle;														// The Rectangle For Locking The Bitmap In Memory
			BitmapData bitmapData = null;												// The Bitmap's Pixel Data

			// Load The Bitmap
			try {
				bitmap = new Bitmap(filename);											// Load The File As A Bitmap
				bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);						// Flip The Bitmap Along The Y-Axis
				rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);			// Select The Whole Bitmap
				
				// Get The Pixel Data From The Locked Bitmap
				bitmapData = bitmap.LockBits(rectangle, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

				glGenTextures(1, texture);												// Create One Texture

				// Create Linear Filtered Texture
				glBindTexture(GL_TEXTURE_2D, texture[0]);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR); 
				glTexImage2D(GL_TEXTURE_2D, 0, (int) GL_RGB8, bitmap.Width, bitmap.Height, 0, GL_BGR_EXT, GL_UNSIGNED_BYTE, bitmapData.Scan0);
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