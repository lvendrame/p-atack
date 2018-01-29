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
 *
 *	WaterEffect by Robert Schaap <robert@vulcanus.its.tudelft.nl>
 *
 *	Visit my homepage at: http://vulcanus.its.tudelft.nl/robert
 *
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
[assembly: AssemblyDescription("Schaap's Water Effect")]
[assembly: AssemblyProduct("Schaap's Water Effect")]
[assembly: AssemblyTitle("Schaap's Water Effect")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace SchaapExamples {
	/// <summary>
	/// Schaap's Water Effect -- A Simple Water Effect (http://vulcanus.its.tudelft.nl/robert/opengl.html)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class SchaapWaterEffect : Model {
		// --- Fields ---
		#region Private Fields
		private static float roty;
		private static float zoom;
		private static uint[] texture = new uint[1];									// Storage For 1 Texture
		private const int MAXX = 64;
		private const int MAXY = 64;
		private const int MAX = 1;
		private const int DAMP = 42;
		private const float FACT = 1.5f;
		private float[ , , ] waveMap = new float[2, MAXX + 1, MAXY + 1];
		private int currentMap = 0;
		private uint mode = GL_LINE;
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Lesson title.
		/// </summary>
		public override string Title {
			get {
				return "Schaap's Water Effect -- A Simple Water Effect";
			}
		}

		/// <summary>
		/// Lesson description.
		/// </summary>
		public override string Description {
			get {
				return "This lesson demonstrates a simple water effect.";
			}
		}

		/// <summary>
		/// Lesson URL.
		/// </summary>
		public override string Url {
			get {
				return "http://vulcanus.its.tudelft.nl/robert/opengl.html";
			}
		}
		#endregion Public Properties

		// --- Entry Point ---
		#region Main()
		/// <summary>
		/// Application's entry point, runs this Schaap example.
		/// </summary>
		public static void Main() {														// Entry Point
			App.Run(new SchaapWaterEffect());											// Run Our Example As A Windows Forms Application
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
			ResetWaveMaps();
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws Schaap's Water Effect scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			glMatrixMode(GL_MODELVIEW);
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);							// Clear Screen And Depth Buffer
			glLoadIdentity();															// Reset The Current Modelview Matrix

			glColor3f(1, 1, 1);
			glTranslatef(0.0f, -0.15f, -1.5f);
			glRotatef(10 + zoom, 1, 0, 0);
			glRotatef(roty, 0, 1, 0);

			glPolygonMode(GL_FRONT_AND_BACK, mode);

			float x, y, u, v, dx, dy, du, dv, nexty, nextv;
			int i, j;

			dx = 1.0f / (MAXX - 1);
			dy = 1.0f / (MAXY - 1);
			du = 1.0f / MAXX;
			dv = 1.0f / MAXY;

			y = -0.5f;
			v = 0.0f;

			glBindTexture(GL_TEXTURE_2D, texture[0]);

			for(j = 0; j < MAXY - 1; j++) {
				x = -0.5f;
				u = 0.0f;
				nexty = y + dy;
				nextv = v + dv;

				glBegin(GL_TRIANGLE_STRIP);
					for(i = 0; i < MAXX+1; i++) { 
						glNormal3f(0, 1, 0);

						glTexCoord2f(u, v);
						glVertex3f(x, waveMap[currentMap, i, j], y);

						glTexCoord2f(u, nextv);
						glVertex3f(x, waveMap[currentMap, i, j + 1], nexty); 

						x += dx;
						u += du;
					}
				glEnd();

				y = nexty;
				v = nextv;
			}
			UpdateWaveMap();
		}
		#endregion Draw()

		#region InputHelp()
		/// <summary>
		/// Overrides default input help, supplying lesson-specific help information.
		/// </summary>
		public override void InputHelp() {
			base.InputHelp();															// Set Up The Default Input Help

			DataRow dataRow;															// Row To Add

			dataRow = InputHelpDataTable.NewRow();										// Up Arrow - Tilt Down
			dataRow["Input"] = "Up Arrow";
			dataRow["Effect"] = "Tilt Down";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Down Arrow - Tilt Up
			dataRow["Input"] = "Down Arrow";
			dataRow["Effect"] = "Tilt Up";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Left Arrow - Rotate Left
			dataRow["Input"] = "Left Arrow";
			dataRow["Effect"] = "Rotate Left";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Right Arrow - Rotate Right
			dataRow["Input"] = "Right Arrow";
			dataRow["Effect"] = "Rotate Right";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Space Bar - Cycle Through Rendering Modes
			dataRow["Input"] = "Space Bar";
			dataRow["Effect"] = "Cycle Through Rendering Modes";
			if(mode == GL_FILL) {
				dataRow["Current State"] = "GL_FILL";
			}
			else if(mode == GL_POINT) {
				dataRow["Current State"] = "GL_POINT";
			}
			else {
				dataRow["Current State"] = "GL_LINE";
			}
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// 1 - Drop In Center
			dataRow["Input"] = "1";
			dataRow["Effect"] = "Drop In Center";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// 2 - Drop In Back Left
			dataRow["Input"] = "2";
			dataRow["Effect"] = "Drop In Back Left";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// 3 - Drop In Front Right
			dataRow["Input"] = "3";
			dataRow["Effect"] = "Drop In Front Right";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// 4 - Drop In Front Left
			dataRow["Input"] = "4";
			dataRow["Effect"] = "Drop In Front Left";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// 5 - Drop In Back Right
			dataRow["Input"] = "5";
			dataRow["Effect"] = "Drop In Back Right";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// R - Reset
			dataRow["Input"] = "R";
			dataRow["Effect"] = "Reset";
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
				zoom += 1.0f;															// Increase Zoom
			}

			if(KeyState[(int) Keys.Down]) {												// Is Down Arrow Being Pressed?
				zoom -= 1.0f;															// Decrease Zoom
			}

			if(KeyState[(int) Keys.Left]) {												// Is Left Arrow Being Pressed?
				roty -= 1.0f;															// Rotate Left
			}

			if(KeyState[(int) Keys.Right]) {											// Is Right Arrow Being Pressed?
				roty += 1.0f;															// Rotate Right
			}

			if(KeyState[(int) Keys.Space]) {											// Is Space Bar Being Pressed?
				KeyState[(int) Keys.Space] = false;										// Mark As Handled
				if(mode == GL_LINE) {													// Cycle Through Rendering Mode
					mode = GL_FILL;
				}
				else if(mode == GL_FILL) {
					mode = GL_POINT;
				}
				else if(mode == GL_POINT) {
					mode = GL_LINE;
				}
				UpdateInputHelp();
			}

			if(KeyState[(int) Keys.D1]) {												// Is 1 Key Being Pressed?
				KeyState[(int) Keys.D1] = false;										// Mark As Handled
				waveMap[currentMap, MAXX / 2, MAXY / 2] = MAX;
			}

			if(KeyState[(int) Keys.D2]) {												// Is 2 Key Being Pressed?
				KeyState[(int) Keys.D2] = false;										// Mark As Handled
				waveMap[currentMap, 1, 1] = MAX;
			}

			if(KeyState[(int) Keys.D3]) {												// Is 3 Key Being Pressed?
				KeyState[(int) Keys.D3] = false;										// Mark As Handled
				waveMap[currentMap, MAXX - 2, MAXY - 2] = MAX;
			}

			if(KeyState[(int) Keys.D4]) {												// Is 4 Key Being Pressed?
				KeyState[(int) Keys.D4] = false;										// Mark As Handled
				waveMap[currentMap, 1, MAXY - 2] = MAX;
			}

			if(KeyState[(int) Keys.D5]) {												// Is 5 Key Being Pressed?
				KeyState[(int) Keys.D5] = false;										// Mark As Handled
				waveMap[currentMap, MAXX - 2, 1] = MAX;
			}

			if(KeyState[(int) Keys.R]) {												// Is R Key Being Pressed?
				KeyState[(int) Keys.R] = false;											// Mark As Handled
				ResetWaveMaps();
			}
		}
		#endregion ProcessInput()

		// --- Lesson Methods ---
		#region LoadTextures()
		/// <summary>
		/// Loads and creates the textures.
		/// </summary>
		private void LoadTextures() {
			string filename = @"..\..\data\SchaapWaterEffect\Water.bmp";				// The File To Load
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

		#region ResetWaveMaps()
		/// <summary>
		/// Resets the wave maps.
		/// </summary>
		private void ResetWaveMaps() {
			int x, y;

			for(y = 0; y <= MAXY; y++) {
				for(x = 0; x <= MAXX; x++) {
					waveMap[currentMap, x, y] = 0;
					waveMap[1 - currentMap, x, y] = 0;
				}
			}
		}
		#endregion ResetWaveMaps()

		#region UpdateWaveMap()
		/// <summary>
		/// Updates the wave map.
		/// </summary>
		private void UpdateWaveMap() {
			int x, y;
			float n;

			for(y = 1; y < MAXY - 1; y++) {
				for(x = 1; x < MAXX; x++) {
					n = (waveMap[currentMap, x - 1, y] + waveMap[currentMap, x + 1, y] + waveMap[currentMap, x, y - 1] +
						waveMap[currentMap, x, y + 1] + waveMap[currentMap, x - 1, y - 1] + waveMap[currentMap, x + 1, y - 1] +
						waveMap[currentMap, x - 1, y + 1] + waveMap[currentMap, x + 1, y + 1] ) / 4 - waveMap[1 - currentMap, x, y];

					n = n - (n / DAMP);

					waveMap[1 - currentMap, x, y] = n;
				}
			}

			currentMap = 1 - currentMap;
		}
		#endregion UpdateWaveMap()
	}
}