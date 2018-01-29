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
 *  Created by Robert Schaap <robert@vulcanus.its.tudelft.nl>
 *  http://vulcanus.its.tudelft.nl/robert
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
[assembly: AssemblyDescription("Schaap's Fireworks")]
[assembly: AssemblyProduct("Schaap's Fireworks")]
[assembly: AssemblyTitle("Schaap's Fireworks")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace SchaapExamples {
	/// <summary>
	/// Schaap's Fireworks -- Some Particle-Based Fireworks (http://vulcanus.its.tudelft.nl/robert/opengl.html)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class SchaapFireworks : Model {
		// --- Fields ---
		#region Private Fields
		private const int MAX_FIREWORKS = 10;											// Maximum Number Of Fireworks
		private const int MAX_PARTICLES = 40;											// Maximum Number Of Particles
		private static uint[] texture = new uint[1];									// Storage For One Texture
		private static float rotXAngle;													// X Rotation Angle
		private static float rotYAngle = 90;											// Y Rotation Angle
		private static float rotZAngle;													// Z Rotation Angle
		private static Firework[] fireworkArray;										// Particle Engine
		private static Random rand = new Random();										// Randomizer
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Lesson title.
		/// </summary>
		public override string Title {
			get {
				return "Schaap's Fireworks -- Some Particle-Based Fireworks";
			}
		}

		/// <summary>
		/// Lesson description.
		/// </summary>
		public override string Description {
			get {
				return "This example displays some particle-based fireworks.";
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
			App.Run(new SchaapFireworks());												// Run Our Example As A Windows Forms Application
		}
		#endregion Main()

		// --- Basecode Methods ---
		#region Initialize()
		/// <summary>
		/// Overrides OpenGL's initialization.
		/// </summary>
		public override void Initialize() {
			LoadTextures();																// Jump To The Texture Loading Routine

			glEnable(GL_TEXTURE_2D);													// Enable Texture Mapping
			glShadeModel(GL_SMOOTH);													// Enable Smooth Shading
			glClearColor(0.0f, 0.0f, 0.0f, 0.5f);										// Black Background
			glClearDepth(1.0f);															// Depth Buffer Setup
			glDisable(GL_DEPTH_TEST);													// Disables Depth Testing
			glDepthFunc(GL_LEQUAL);														// The Type Of Depth Testing To Do
			glHint(GL_PERSPECTIVE_CORRECTION_HINT, GL_NICEST);							// Really Nice Perspective Calculations

			glColor4f(1.0f, 1.0f, 1.0f, 0.5f);											// Full Brightness.  50% Alpha
			glBlendFunc(GL_ONE, GL_ONE);												// Set The Blending Function For Translucency

			fireworkArray = new Firework[MAX_FIREWORKS];

			Vector3D origin = new Vector3D(40 * (((rand.Next() % 1000) / 1000f) - 0.5f), 30 * (((rand.Next() % 1000) / 1000f) - 0.5f) + 20, -50 * ((rand.Next() % 1000) / 1000f) - 20);

			fireworkArray[0] = new Firework(MAX_PARTICLES, origin, 0.0f, -30, texture[0]);
			fireworkArray[0].Reset();

			for(int i = 0; i < MAX_FIREWORKS; i++) {
				fireworkArray[i] = new Firework();
			}
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws Schaap Fireworks scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			glMatrixMode(GL_MODELVIEW);													// Select The Modelview Matrix
			glLoadIdentity();															// Reset The Current Modelview Matrix

			glBindTexture(GL_TEXTURE_2D, texture[0]);

			glBlendFunc(GL_ONE, GL_ONE);												// Set The Blending Function For Translucency
			glColor4f(1.0f, 1.0f, 1.0f, 5.0f);											// Full Brightness, 50% Alpha
			glEnable(GL_BLEND);

			for(int i = 0; i < MAX_FIREWORKS; i++) {
				fireworkArray[i].Render();												// Render Particles
				fireworkArray[i].Update(2);												// Update Particles For Next Cycle

				if(fireworkArray[i].Done && ((rand.Next() % 1000) / 1000f) > 0.99f) {
					Vector3D origin = new Vector3D(40 * (((rand.Next() % 1000) / 1000f) - 0.5f), 30 * (((rand.Next() % 1000) / 1000f) - 0.5f) + 20, -50 * ((rand.Next() % 1000) / 1000f) - 20);
					fireworkArray[i] = new Firework(MAX_PARTICLES, origin, 0.0f, -30, texture[0]);
					fireworkArray[i].Reset();											// Resets (Initializes) Particle Engine
				}
			}

			// Update Rotation
			rotXAngle += 0.8f;
			rotYAngle += 0.95f;
			rotZAngle += 0.4f;

			glDisable(GL_TEXTURE_2D);
			glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

			glColor4f(0, 0, 0, 0.05f);
			const int BOUNDS = 10;
			glBegin(GL_TRIANGLE_STRIP);
				glVertex3f(-BOUNDS, BOUNDS, -10);
				glVertex3f(-BOUNDS, -BOUNDS, -10);
				glVertex3f(BOUNDS, BOUNDS, -10);
				glVertex3f(BOUNDS, -BOUNDS, -10);
			glEnd();

			glEnable(GL_TEXTURE_2D);
			glBlendFunc(GL_ONE, GL_ONE);
		}
		#endregion Draw()

		#region Reshape(int width, int height)
		/// <summary>
		/// Overrides OpenGL reshaping.
		/// </summary>
		/// <param name="width">New width.</param>
		/// <param name="height">New height.</param>
		public override void Reshape(int width, int height) {							// Resize And Initialize The GL Window
			base.Reshape(width, height);												// Call The Base Reshape
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);							// Clear Screen And Depth Buffer
		}
		#endregion Reshape(int width, int height)

		#region Setup()
		/// <summary>
		/// Overrides application and OpenGL settings and setup.
		/// </summary>
		public override void Setup() {
			base.Setup();																// Run The Base Setup
			App.FovY = 70f;																// Change FovY
		}
		#endregion Setup()

		// --- Lesson Methods ---
		#region LoadTextures()
		/// <summary>
		/// Loads and creates the texture.
		/// </summary>
		private void LoadTextures() {
			string filename = @"..\..\data\SchaapFireworks\Star.bmp";					// The File To Load
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

				// Create Mipmapped Texture
				glBindTexture(GL_TEXTURE_2D, texture[0]);
				glTexParameteri(GL_TEXTURE_2D,GL_TEXTURE_MAG_FILTER,GL_LINEAR);
				glTexParameteri(GL_TEXTURE_2D,GL_TEXTURE_MIN_FILTER,GL_LINEAR_MIPMAP_NEAREST);
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
	}
}