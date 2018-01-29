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
[assembly: AssemblyDescription("NeHe Lesson 12")]
[assembly: AssemblyProduct("NeHe Lesson 12")]
[assembly: AssemblyTitle("NeHe Lesson 12")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace NeHeLessons {
	/// <summary>
	/// NeHe Lesson 12 -- Display Lists (http://nehe.gamedev.net)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class NeHeLesson12 : Model {
		// --- Fields ---
		#region Private Fields
		private static uint box;														// The Box Display List
		private static uint top;														// The Top Display List
		private static uint xloop;														// Loop For X Axis
		private static uint yloop;														// Loop For Y Axis
		private static float xrot;														// Rotates Cube On X Axis
		private static float yrot;														// Rotates Cube On Y Axis

		private static float[][] boxcol = new float[5][] {								// Array For Box Colors
			new float[] {1.0f, 0.0f, 0.0f},												// Bright Red
			new float[] {1.0f, 0.5f, 0.0f},												// Bright Orange
			new float[] {1.0f, 1.0f, 0.0f},												// Bright Yellow
			new float[] {0.0f, 1.0f, 0.0f},												// Bright Green
			new float[] {0.0f, 1.0f, 1.0f}												// Bright Blue
		};

		private static float[][] topcol = new float[5][] {								// Array For Top Colors
			new float[] {0.5f, 0.0f,  0.0f},											// Dark Red
			new float[] {0.5f, 0.25f, 0.0f},											// Dark Orange
			new float[] {0.5f, 0.5f,  0.0f},											// Dark Yellow
			new float[] {0.0f, 0.5f,  0.0f},											// Dark Green
			new float[] {0.0f, 0.5f,  0.5f}												// Dark Blue
		};

		private static uint[] texture = new uint[1];									// Storage For 1 Texture
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Lesson title.
		/// </summary>
		public override string Title {
			get {
				return "NeHe Lesson 12 -- Display Lists";
			}
		}

		/// <summary>
		/// Lesson description.
		/// </summary>
		public override string Description {
			get {
				return "This lesson demonstrates using OpenGL's display lists.  Display lists allow you to define an object once and have it displayed multiple times, saving you coding and speeding up your programs.";
			}
		}

		/// <summary>
		/// Lesson URL.
		/// </summary>
		public override string Url {
			get {
				return "http://nehe.gamedev.net/tutorials/lesson.asp?l=12";
			}
		}
		#endregion Public Properties

		// --- Entry Point ---
		#region Main()
		/// <summary>
		/// Application's entry point, runs this NeHe lesson.
		/// </summary>
		public static void Main() {														// Entry Point
			App.Run(new NeHeLesson12());												// Run Our NeHe Lesson As A Windows Forms Application
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
			glEnable(GL_LIGHT0);														// Quick And Dirty Lighting (Assumes Light0 Is Set Up)
			glEnable(GL_LIGHTING);														// Enable Lighting
			glEnable(GL_COLOR_MATERIAL);												// Enable Material Coloring

			LoadTextures();																// Jump To Texture Loading Routine

			BuildLists();																// Jump To Display List Creation Routine
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws NeHe Lesson 12 scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);							// Clear Screen And Depth Buffer

			glBindTexture(GL_TEXTURE_2D, texture[0]);									// Select Our Texture

			for(yloop = 1; yloop < 6; yloop++) {										// Loop Through The Y Plane
				for(xloop = 0; xloop < yloop; xloop++) {								// Loop Through The X Plane
					glLoadIdentity();													// Reset The View
					// Position The Cubes On The Screen
					glTranslatef(1.4f + (xloop * 2.8f) - (yloop * 1.4f), ((6.0f - yloop) * 2.4f) - 7.0f, -20.0f);
					glRotatef(45.0f - (2.0f * yloop) + xrot, 1.0f, 0.0f, 0.0f);			// Tilt The Cubes Up And Down
					glRotatef(45.0f + yrot, 0.0f, 1.0f, 0.0f);							// Spin Cubes Left And Right
					glColor3fv(boxcol[yloop - 1]);										// Select A Box Color
					glCallList(box);													// Draw The Box
					glColor3fv(topcol[yloop - 1]);										// Select The Top Color
					glCallList(top);													// Draw The Top
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

			dataRow = InputHelpDataTable.NewRow();										// Left Arrow - Spin Left
			dataRow["Input"] = "Left Arrow";
			dataRow["Effect"] = "Spin Left";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Right Arrow - Spin Right
			dataRow["Input"] = "Right Arrow";
			dataRow["Effect"] = "Spin Right";
			dataRow["Current State"] = "";
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
				xrot -= 0.2f;															// Tilt Cubes Up
			}

			if(KeyState[(int) Keys.Down]) {												// Is Down Arrow Being Pressed?
				xrot += 0.2f;															// Tilt Cubes Down
			}

			if(KeyState[(int) Keys.Left]) {												// Is Left Arrow Being Pressed?
				yrot -= 0.2f;															// Spin Cubes Left
			}

			if(KeyState[(int) Keys.Right]) {											// Is Right Arrow Being Pressed?
				yrot += 0.2f;															// Spin Cubes Left
			}
		}
		#endregion ProcessInput()

		// --- Lesson Methods ---
		#region BuildLists()
		/// <summary>
		/// Creates display lists.
		/// </summary>
		public void BuildLists() {
			box = glGenLists(2);														// Generate 2 Display Lists

			glNewList(box, GL_COMPILE);													// Start With The box Display List
				glBegin(GL_QUADS);														// Start Drawing Quads
					// Bottom Face
					glNormal3f(0.0f, -1.0f, 0.0f);										// Normal Pointing Down
					glTexCoord2f(1.0f, 1.0f); glVertex3f(-1.0f, -1.0f, -1.0f);			// Top Right Of The Texture and Quad
					glTexCoord2f(0.0f, 1.0f); glVertex3f( 1.0f, -1.0f, -1.0f);			// Top Left Of The Texture and Quad
					glTexCoord2f(0.0f, 0.0f); glVertex3f( 1.0f, -1.0f,  1.0f);			// Bottom Left Of The Texture and Quad
					glTexCoord2f(1.0f, 0.0f); glVertex3f(-1.0f, -1.0f,  1.0f);			// Bottom Right Of The Texture and Quad
					// Front Face
					glNormal3f(0.0f, 0.0f, 1.0f);										// Normal Pointing Towards Viewer
					glTexCoord2f(0.0f, 0.0f); glVertex3f(-1.0f, -1.0f,  1.0f);			// Bottom Left Of The Texture and Quad
					glTexCoord2f(1.0f, 0.0f); glVertex3f( 1.0f, -1.0f,  1.0f);			// Bottom Right Of The Texture and Quad
					glTexCoord2f(1.0f, 1.0f); glVertex3f( 1.0f,  1.0f,  1.0f);			// Top Right Of The Texture and Quad
					glTexCoord2f(0.0f, 1.0f); glVertex3f(-1.0f,  1.0f,  1.0f);			// Top Left Of The Texture and Quad
					// Back Face
					glNormal3f(0.0f, 0.0f, -1.0f);										// Normal Pointing Away From Viewer
					glTexCoord2f(1.0f, 0.0f); glVertex3f(-1.0f, -1.0f, -1.0f);			// Bottom Right Of The Texture and Quad
					glTexCoord2f(1.0f, 1.0f); glVertex3f(-1.0f,  1.0f, -1.0f);			// Top Right Of The Texture and Quad
					glTexCoord2f(0.0f, 1.0f); glVertex3f( 1.0f,  1.0f, -1.0f);			// Top Left Of The Texture and Quad
					glTexCoord2f(0.0f, 0.0f); glVertex3f( 1.0f, -1.0f, -1.0f);			// Bottom Left Of The Texture and Quad
					// Right face
					glNormal3f(1.0f, 0.0f, 0.0f);										// Normal Pointing Right
					glTexCoord2f(1.0f, 0.0f); glVertex3f( 1.0f, -1.0f, -1.0f);			// Bottom Right Of The Texture and Quad
					glTexCoord2f(1.0f, 1.0f); glVertex3f( 1.0f,  1.0f, -1.0f);			// Top Right Of The Texture and Quad
					glTexCoord2f(0.0f, 1.0f); glVertex3f( 1.0f,  1.0f,  1.0f);			// Top Left Of The Texture and Quad
					glTexCoord2f(0.0f, 0.0f); glVertex3f( 1.0f, -1.0f,  1.0f);			// Bottom Left Of The Texture and Quad
					// Left Face
					glNormal3f(-1.0f, 0.0f, 0.0f);										// Normal Pointing Left
					glTexCoord2f(0.0f, 0.0f); glVertex3f(-1.0f, -1.0f, -1.0f);			// Bottom Left Of The Texture and Quad
					glTexCoord2f(1.0f, 0.0f); glVertex3f(-1.0f, -1.0f,  1.0f);			// Bottom Right Of The Texture and Quad
					glTexCoord2f(1.0f, 1.0f); glVertex3f(-1.0f,  1.0f,  1.0f);			// Top Right Of The Texture and Quad
					glTexCoord2f(0.0f, 1.0f); glVertex3f(-1.0f,  1.0f, -1.0f);			// Top Left Of The Texture and Quad
				glEnd();																// Done Drawing Quads
			glEndList();																// Done Building The box Display List

			top = box + 1;																// top List Value Is box List Value + 1

			glNewList(top, GL_COMPILE);													// Now The top Display List
				glBegin(GL_QUADS);														// Start Drawing Quad
					// Top Face
					glNormal3f(0.0f, 1.0f, 0.0f);										// Normal Pointing Up
					glTexCoord2f(0.0f, 1.0f); glVertex3f(-1.0f, 1.0f, -1.0f);			// Top Left Of The Texture and Quad
					glTexCoord2f(0.0f, 0.0f); glVertex3f(-1.0f, 1.0f,  1.0f);			// Bottom Left Of The Texture and Quad
					glTexCoord2f(1.0f, 0.0f); glVertex3f( 1.0f, 1.0f,  1.0f);			// Bottom Right Of The Texture and Quad
					glTexCoord2f(1.0f, 1.0f); glVertex3f( 1.0f, 1.0f, -1.0f);			// Top Right Of The Texture and Quad
				glEnd();																// Done Drawing Quad
			glEndList();																// Done Building The top Display List
		}
		#endregion BuildLists()

		#region LoadTextures()
		/// <summary>
		/// Loads and creates the texture.
		/// </summary>
		private void LoadTextures() {
			string filename = @"..\..\data\NeHeLesson12\Cube.bmp";						// The File To Load
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

				glGenTextures(1, texture);												// Create 1 Texture

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