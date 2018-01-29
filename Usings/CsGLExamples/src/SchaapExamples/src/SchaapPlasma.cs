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
 *	Plasma by Robert Schaap <robert@vulcanus.its.tudelft.nl>
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
[assembly: AssemblyProduct("Schaap's Plasma")]
[assembly: AssemblyTitle("Schaap's Plasma")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace SchaapExamples {
	/// <summary>
	/// Schaap's Plasma -- A Simple Plasma Effect (http://vulcanus.its.tudelft.nl/robert/opengl.html)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class SchaapPlasma : Model {
		// --- Fields ---
		#region Private Fields
		private static bool paused;
		private static byte[] tableX = new byte[256];
		private static byte[] tableY = new byte[256];
		private static byte[ , , ] plasma = new byte[256, 256, 3];
		private static int width, height, count;
		private static int t1, t2, t3, t4;
		private static byte red1 = 255, red2 = 255;
		private static byte green1 = 255, green2;
		private static byte blue1, blue2;
		private static uint[] texture = new uint[1];
		private static ulong starttime;
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Lesson title.
		/// </summary>
		public override string Title {
			get {
				return "Schaap's Plasma -- A Simple Plasma Effect";
			}
		}

		/// <summary>
		/// Lesson description.
		/// </summary>
		public override string Description {
			get {
				return "This lesson demonstrates a simple plasma effect.";
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
			App.Run(new SchaapPlasma());												// Run Our Example As A Windows Forms Application
		}
		#endregion Main()

		// --- Basecode Methods ---
		#region Initialize()
		/// <summary>
		/// Overrides OpenGL's initialization.
		/// </summary>
		public override void Initialize() {
			base.Initialize();															// Run The Base Initialization
			width  = 256;
			height = 256;
			count = 0;
			t1 = t2 = t3 = t4 = 0;

			// Load Tables
			for(int i = 0; i < width; i++) {
				tableX[i] = (byte) (32 * Math.Sin(i * Math.PI * 2 / width) + 32 * Math.Sin(i * Math.PI * 2 / width) + 100);
			}

			for(int i = 0; i < height; i++) {
				tableY[i] = (byte) (32 * Math.Cos(i * Math.PI * 2 / height) + 32 * Math.Cos(i * Math.PI * 2 / height) + 50);
			}

			LoadTextures();
			starttime = App.Timer.Count;
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws Schaap's Plasma scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			if(!paused) {
				glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);						// Clear Screen And Depth Buffer
				glPushMatrix();
					RenderScene();
				glPopMatrix();
				glFinish();
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

			dataRow = InputHelpDataTable.NewRow();										// Q - Increase Red 1
			dataRow["Input"] = "Q";
			dataRow["Effect"] = "Increase Red 1";
			dataRow["Current State"] = red1;
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// A - Decrease Red 1
			dataRow["Input"] = "A";
			dataRow["Effect"] = "Decrease Red 1";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// W - Increase Green 1
			dataRow["Input"] = "W";
			dataRow["Effect"] = "Increase Green 1";
			dataRow["Current State"] = green1;
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// S - Decrease Green 1
			dataRow["Input"] = "S";
			dataRow["Effect"] = "Decrease Green 1";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// E - Increase Blue 1
			dataRow["Input"] = "E";
			dataRow["Effect"] = "Increase Blue 1";
			dataRow["Current State"] = blue1;
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// D - Decrease Blue 1
			dataRow["Input"] = "D";
			dataRow["Effect"] = "Decrease Blue 1";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Y - Increase Red 2
			dataRow["Input"] = "Y";
			dataRow["Effect"] = "Increase Red 2";
			dataRow["Current State"] = red2;
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// H - Decrease Red 2
			dataRow["Input"] = "H";
			dataRow["Effect"] = "Decrease Red 2";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// U - Increase Green 2
			dataRow["Input"] = "U";
			dataRow["Effect"] = "Increase Green 2";
			dataRow["Current State"] = green2;
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// J - Decrease Green 2
			dataRow["Input"] = "J";
			dataRow["Effect"] = "Decrease Green 2";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// I - Increase Blue 2
			dataRow["Input"] = "I";
			dataRow["Effect"] = "Increase Blue 2";
			dataRow["Current State"] = blue2;
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// K - Decrease Blue 2
			dataRow["Input"] = "K";
			dataRow["Effect"] = "Decrease Blue 2";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Space Bar - Pause
			dataRow["Input"] = "Space Bar";
			dataRow["Effect"] = "Toggle Pause On / Off";
			if(paused) {
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

			if(KeyState[(int) Keys.Q]) {												// Is Q Key Being Pressed?
				KeyState[(int) Keys.Q] = false;											// Mark As Handled
				if(red1 < 255) {														// If So, Increase red1
					red1++;
					UpdateInputHelp();
				}
			}

			if(KeyState[(int) Keys.A]) {												// Is A Key Being Pressed?
				KeyState[(int) Keys.A] = false;											// Mark As Handled
				if(red1 > 0) {															// If So, Decrease red1
					red1--;
					UpdateInputHelp();
				}
			}

			if(KeyState[(int) Keys.W]) {												// Is W Key Being Pressed?
				KeyState[(int) Keys.W] = false;											// Mark As Handled
				if(green1 < 255) {														// If So, Increase green1
					green1++;
					UpdateInputHelp();
				}
			}

			if(KeyState[(int) Keys.S]) {												// Is S Key Being Pressed?
				KeyState[(int) Keys.S] = false;											// Mark As Handled
				if(green1 > 0) {														// If So, Decrease green1
					green1--;
					UpdateInputHelp();
				}
			}

			if(KeyState[(int) Keys.E]) {												// Is E Key Being Pressed?
				KeyState[(int) Keys.E] = false;											// Mark As Handled
				if(blue1 < 255) {														// If So, Increase blue1
					blue1++;
					UpdateInputHelp();
				}
			}

			if(KeyState[(int) Keys.D]) {												// Is D Key Being Pressed?
				KeyState[(int) Keys.D] = false;											// Mark As Handled
				if(blue1 > 0) {															// If So, Decrease blue1
					blue1--;
					UpdateInputHelp();
				}
			}

			if(KeyState[(int) Keys.Y]) {												// Is Y Key Being Pressed?
				KeyState[(int) Keys.Y] = false;											// Mark As Handled
				if(red2 < 255) {														// If So, Increase red2
					red2++;
					UpdateInputHelp();
				}
			}

			if(KeyState[(int) Keys.H]) {												// Is H Key Being Pressed?
				KeyState[(int) Keys.H] = false;											// Mark As Handled
				if(red2 > 0) {															// If So, Decrease red2
					red2--;
					UpdateInputHelp();
				}
			}

			if(KeyState[(int) Keys.U]) {												// Is U Key Being Pressed?
				KeyState[(int) Keys.U] = false;											// Mark As Handled
				if(green2 < 255) {														// If So, Increase green2
					green2++;
					UpdateInputHelp();
				}
			}

			if(KeyState[(int) Keys.J]) {												// Is J Key Being Pressed?
				KeyState[(int) Keys.J] = false;											// Mark As Handled
				if(green2 > 0) {														// If So, Decrease green2
					green2--;
					UpdateInputHelp();
				}
			}

			if(KeyState[(int) Keys.I]) {												// Is I Key Being Pressed?
				KeyState[(int) Keys.I] = false;											// Mark As Handled
				if(blue2 < 255) {														// If So, Increase blue2
					blue2++;
					UpdateInputHelp();
				}
			}

			if(KeyState[(int) Keys.K]) {												// Is K Key Being Pressed?
				KeyState[(int) Keys.K] = false;											// Mark As Handled
				if(blue2 > 0) {															// If So, Decrease blue2
					blue2--;
					UpdateInputHelp();
				}
			}

			if(KeyState[(int) Keys.Space]) {											// Is Space Bar Being Pressed?
				KeyState[(int) Keys.Space] = false;										// Mark As Handled
				paused = !paused;														// Toggle Paused
				UpdateInputHelp();
			}
		}
		#endregion ProcessInput()

		#region Reshape(int width, int height)
		/// <summary>
		/// Overrides OpenGL Reshaping.
		/// </summary>
		/// <param name="width">New width.</param>
		/// <param name="height">New height.</param>
		public override void Reshape(int width, int height) {							// Resize And Initialize The GL Window
			if(height == 0) {															// If Height Is 0
				height = 1;																// Set To 1 To Prevent A Divide By Zero
			}

			glViewport(0, 0, width, height);											// Reset The Current Viewport

			glMatrixMode(GL_PROJECTION);												// Select The Projection Matris
			glLoadIdentity();															// Reset The Projection Matrix

			gluOrtho2D(-1.0f, 1.0f, -1.0f, 1.0f);										// Create Ortho View

			glMatrixMode(GL_MODELVIEW);													// Select The Modelview Matrix
			glLoadIdentity();															// Reset The Modelview Matrix
		}
		#endregion Reshape(int width, int height)

		// --- Lesson Methods ---
		#region LoadTextures()
		/// <summary>
		/// Loads and creates the textures.
		/// </summary>
		private void LoadTextures() {
			string filename = @"..\..\data\SchaapPlasma\logo.bmp";						// The File To Load
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

				glBindTexture(GL_TEXTURE_2D, texture[0]);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
				glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_MODULATE);
				gluBuild2DMipmaps(GL_TEXTURE_2D, (int) GL_RGB8, bitmap.Width, bitmap.Height, GL_BGR_EXT, GL_UNSIGNED_BYTE, bitmapData.Scan0);
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

		#region RenderScene()
		/// <summary>
		/// Renders the plasma.
		/// </summary>
		private void RenderScene() {
			glLoadIdentity();

			glEnable(GL_TEXTURE_2D);
			glBindTexture(GL_TEXTURE_2D, texture[0]);

			if(App.Timer.Count - starttime > App.Timer.Frequency * 3) {
				for(int y = 0; y < height; y++) {
					int temp = tableY[t1] + tableY[t2];

					for(int x = 0; x < width; x++) {
						int color = (temp + tableX[t3] + tableX[t4] + count) * 2;
						color = color % 512;

						if(color >= 255) {
							color -= 255;
							color = 256 - color;
						}

						plasma[x, y, 0] = (byte) (red1 + ((((float)red2 - (float)red1) / 255) * color));
						plasma[x, y, 1] = (byte) (green1 + ((((float)green2 - (float)green1) / 255) * color));
						plasma[x, y, 2] = (byte) (blue1 + ((((float)blue2 - (float)blue1) / 255) * color));

						t3 = (t3 + 1) % width;
						t4 = (t4 + 2) % width;
					}

					t1 = (t1 + 2) % height;
					t2 = (t2 + 1) % height;
				}

				t1 = (t1 + 1) % height;
				t2 = (t2 + 2) % height;
				t3 = (t3 + 1) % width;
				t4 = (t4 + 2) % width;
				count++;

				glTexSubImage2D(GL_TEXTURE_2D, 0, 0, 0, 256, 256, GL_RGB, GL_UNSIGNED_BYTE, plasma);
			}
			glBegin(GL_QUADS);
				glTexCoord2f(0,0);
				glVertex3f(-1.0f, -1.0f, 0.0f);
				glTexCoord2f(1, 0);
				glVertex3f(1.0f, -1.0f, 0.0f);
				glTexCoord2f(1, 1);
				glVertex3f(1.0f, 1.0f, 0.0f);
				glTexCoord2f(0, 1);
				glVertex3f(-1.0f, 1.0f, 0.0f);
			glEnd();

			glDisable(GL_TEXTURE_2D);
		}
		#endregion RenderScene()
	}
}