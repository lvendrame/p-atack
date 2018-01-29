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
 *		This Code Was Created By bosco / Jeff Molofee 2000
 *		A HUGE Thanks To Fredric Echols For Cleaning Up
 *		And Optimizing The Base Code, Making It More Flexible!
 *		If You've Found This Code Useful, Please Let Me Know.
 *		Visit My Site At nehe.gamedev.net
 */
#endregion Original Credits / License

using CsGL.Basecode;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Windows.Forms;

#region AssemblyInfo
[assembly: AssemblyCompany("The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyCopyright("2002 The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyDescription("NeHe Lesson 11")]
[assembly: AssemblyProduct("NeHe Lesson 11")]
[assembly: AssemblyTitle("NeHe Lesson 11")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace NeHeLessons {
	/// <summary>
	/// NeHe Lesson 11 -- Flag Effect (Waving Texture) (http://nehe.gamedev.net)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class NeHeLesson11 : Model {
		// --- Fields ---
		#region Private Fields
		private static float[ , , ] points = new float[45, 45, 3];						// The Array For The Points On The Grid Of Our "Wave"
		private static int wiggle_count = 0;											// Counter Used To Control How Fast Flag Waves
		private static float xrot = 0;													// X Rotation
		private static float yrot = 0;													// Y Rotation
		private static float zrot = 0;													// Z Rotation
		private static float hold = 0.0f;												// Temporarily Holds A Floating Point Value
		private static int x, y;														// Loop Variables
		private static float float_x, float_y, float_xb, float_yb;						// Used To Break The Flag Into Tiny Quads
		private static uint[] texture = new uint[1];									// Our Texture
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Lesson title.
		/// </summary>
		public override string Title {
			get {
				return "NeHe Lesson 11 -- Flag Effect (Waving Texture)";
			}
		}

		/// <summary>
		/// Lesson description.
		/// </summary>
		public override string Description {
			get {
				return "This lesson builds on what you learned in lesson 06.  It teaches you how to bend, fold, and manipulate textures to your heart's delight.";
			}
		}

		/// <summary>
		/// Lesson URL.
		/// </summary>
		public override string Url {
			get {
				return "http://nehe.gamedev.net/tutorials/lesson.asp?l=11";
			}
		}
		#endregion Public Properties

		// --- Entry Point ---
		#region Main()
		/// <summary>
		/// Application's entry point, runs this NeHe lesson.
		/// </summary>
		public static void Main() {														// Entry Point
			App.Run(new NeHeLesson11());												// Run Our NeHe Lesson As A Windows Forms Application
		}
		#endregion Main()

		// --- Methods ---
		#region Initialize()
		/// <summary>
		/// Overrides OpenGL's initialization.
		/// </summary>
		public override void Initialize() {
			base.Initialize();															// Run The Base Initialization
			glEnable(GL_TEXTURE_2D);													// Enable Texture Mapping
			glPolygonMode(GL_BACK, GL_FILL);											// Back Face Is Filled In
			glPolygonMode(GL_FRONT, GL_LINE);											// Front Face Is Drawn With Lines

			LoadTextures();																// Jump To Texture Loading Routine

			for(x = 0; x < 45; x++) {													// Loop Through The X Plane
				for(y = 0; y < 45; y++) {												// Loop Through The Y Plane
					// Apply The Wave To Our Mesh
					points[x, y, 0] = (float) (x / 5.0f) - 4.5f;
					points[x, y, 1] = (float) (y / 5.0f) - 4.5f;
					points[x, y, 2] = (float) Math.Sin((((x / 5.0f) * 40.0f) / 360.0f) * 3.141592654 * 2.0f);
				}
			}
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws NeHe Lesson 11 scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);							// Clear Screen And Depth Buffer
			glLoadIdentity();															// Reset The Current Modelview Matrix

			glTranslatef(0.0f, 0.0f, -12.0f);											// Move Into The Screen 12.0 Units

			glRotatef(xrot, 1.0f, 0.0f, 0.0f);											// Rotate On The X Axis
			glRotatef(yrot, 0.0f, 1.0f, 0.0f);											// Rotate On The Y Axis
			glRotatef(zrot, 0.0f, 0.0f, 1.0f);											// Rotate On The Z Axis

			glBindTexture(GL_TEXTURE_2D, texture[0]);									// Select Our Texture

			glBegin(GL_QUADS);															// Start Drawing Our Quads
				for(x = 0; x < 44; x++ ) {												// Loop Through The X Plane 0-44 (45 Points)
					for(y = 0; y < 44; y++ ) {											// Loop Through The Y Plane 0-44 (45 Points)
						float_x = x / 44.0f;											// Create A Floating Point X Value
						float_y = y / 44.0f;											// Create A Floating Point Y Value
						float_xb = (x + 1) / 44.0f;										// Create A Floating Point Y Value+0.0227f
						float_yb = (y + 1) / 44.0f;										// Create A Floating Point Y Value+0.0227f

						glTexCoord2f(float_x, float_y);									// First Texture Coordinate (Bottom Left)
						glVertex3f(points[x, y, 0], points[x, y, 1], points[x, y, 2]);

						glTexCoord2f(float_x, float_yb);								// Second Texture Coordinate (Top Left)
						glVertex3f(points[x, y + 1, 0], points[x, y + 1, 1], points[x, y + 1, 2]);

						glTexCoord2f(float_xb, float_yb);								// Third Texture Coordinate (Top Right)
						glVertex3f(points[x + 1, y + 1, 0], points[x + 1, y + 1, 1], points[x + 1, y + 1, 2]);

						glTexCoord2f(float_xb, float_y);								// Fourth Texture Coordinate (Bottom Right)
						glVertex3f(points[x + 1, y, 0], points[x + 1, y, 1], points[x + 1, y, 2]);
					}
				}
			glEnd();																	// Done Drawing Our Quads

			if(wiggle_count == 2) {														// Used To Slow Down The Wave (Every 2nd Frame Only)
				for(y = 0; y < 45; y++) {												// Loop Through The Y Plane
					hold = points[0, y, 2];												// Store Current Value One Left Side Of Wave
					for(x = 0; x < 44; x++) {											// Loop Through The X Plane
						points[x, y, 2] = points[x + 1, y, 2];							// Current Wave Value Equals Value To The Right
					}
					points[44, y, 2] = hold;											// Last Value Becomes The Far Left Stored Value
				}
				wiggle_count = 0;														// Set Counter Back To Zero
			}
			wiggle_count++;																// Increase The Counter

			xrot += 0.3f;																// X Axis Rotation
			yrot += 0.2f;																// Y Axis Rotation
			zrot += 0.4f;																// Z Axis Rotation
		}
		#endregion Draw()

		// --- Lesson Methods ---
		#region LoadTextures()
		/// <summary>
		/// Loads and creates the textures.
		/// </summary>
		private void LoadTextures() {
			string filename = @"..\..\data\NeHeLesson11\Tim.bmp";						// The File To Load
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

				glBindTexture(GL_TEXTURE_2D, texture[0]);								// Typical Texture Generation Using Data From The Bitmap
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);		// Linear Filtering
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);		// Linear Filtering
				// Generate The Texture
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