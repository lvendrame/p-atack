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
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#region AssemblyInfo
[assembly: AssemblyCompany("The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyCopyright("2002 The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyDescription("Schaap's Fire")]
[assembly: AssemblyProduct("Schaap's Fire")]
[assembly: AssemblyTitle("Schaap's Fire")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace SchaapExamples {
	/// <summary>
	/// Schaap's Fire -- A Fire Effect (http://vulcanus.its.tudelft.nl/robert/opengl.html)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class SchaapFire : Model {
		// --- Fields ---
		#region Private Fields
		private static uint[] texture = new uint[1];									// Storage For One Texture
		private static CoolEffect3 coolEffect;											// Particle Engine
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Lesson title.
		/// </summary>
		public override string Title {
			get {
				return "Schaap's Fire -- A Fire Effect";
			}
		}

		/// <summary>
		/// Lesson description.
		/// </summary>
		public override string Description {
			get {
				return "This example displays a cool little fire effect.";
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
			App.Run(new SchaapFire());													// Run Our Example As A Windows Forms Application
		}
		#endregion Main()

		// --- Basecode Methods ---
		#region Initialize()
		/// <summary>
		/// Overrides OpenGL's initialization.
		/// </summary>
		public override void Initialize() {
			glMatrixMode(GL_PROJECTION);												// Select The Projection Matrix
			glLoadIdentity();															// Reset The Projection Matrix

			LoadTextures();																// Jump To The Texture Loading Routine

			glColor4f(1.0f, 1.0f, 1.0f, 0.5f);											// Full Brightness, 50% Alpha
			glEnable(GL_BLEND);															// Enable Blending
			glBlendFunc(GL_ONE, GL_ONE);												// Choose Blend Function
			glEnable(GL_TEXTURE_2D);													// Enable Texture Mapping
			glShadeModel(GL_SMOOTH);													// Enable Smooth Shading
			glClearColor(0.0f, 0.0f, 0.0f, 0.5f);										// Black Background
			glClearDepth(1.0f);															// Depth Buffer Setup
			glDisable(GL_DEPTH_TEST);													// Enables Depth Testing
			glDepthFunc(GL_LEQUAL);														// The Type Of Depth Testing To Do
			glHint(GL_PERSPECTIVE_CORRECTION_HINT, GL_NICEST);							// Really Nice Perspective Calculations

			// Create Instance Of CoolEffect Particle Engine
			coolEffect = new CoolEffect3(300, new Vector3D(0, 0, 0), 0.5f, 0.5f, 10, texture[0]);
			coolEffect.Reset();
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws Schaap Fire scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);							// Clear Screen And Depth Buffer
			glMatrixMode(GL_MODELVIEW);													// Select The Modelview Matrix
			glLoadIdentity();															// Reset The Current Modelview Matrix

			glTranslatef(0.0f, -3.0f, -7.0f);

			coolEffect.Render();														// Render Particles
			coolEffect.Update(30);														// Update Particles For Next Cycle
		}
		#endregion Draw()

		#region InputHelp()
		/// <summary>
		/// Overrides default input help, supplying lesson-specific help information.
		/// </summary>
		public override void InputHelp() {
			base.InputHelp();															// Set Up The Default Input Help

			DataRow dataRow;															// Row To Add

			dataRow = InputHelpDataTable.NewRow();										// Up Arrow - Move Up
			dataRow["Input"] = "Up Arrow";
			dataRow["Effect"] = "Move Up";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Down Arrow - Move Down
			dataRow["Input"] = "Down Arrow";
			dataRow["Effect"] = "Move Down";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Left Arrow - Move Left
			dataRow["Input"] = "Left Arrow";
			dataRow["Effect"] = "Move Left";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Right Arrow - Move Right
			dataRow["Input"] = "Right Arrow";
			dataRow["Effect"] = "Move Right";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// A - Zoom Out
			dataRow["Input"] = "A";
			dataRow["Effect"] = "Zoom Out";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Z - Zoom In
			dataRow["Input"] = "Z";
			dataRow["Effect"] = "Zoom In";
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

			float x = 0.0f, y = 0.0f, z = 0.0f;

			if(KeyState[(int) Keys.Up]) {												// Is Up Arrow Being Pressed?
				KeyState[(int) Keys.Up] = false;										// Mark It As Handled
				y = 0.5f;																// Move Up
			}

			if(KeyState[(int) Keys.Down]) {												// Is Down Arrow Being Pressed?
				KeyState[(int) Keys.Down] = false;										// Mark It As Handled
				y = -0.5f;																// Move Down
			}

			if(KeyState[(int) Keys.Left]) {												// Is Up Arrow Being Pressed?
				KeyState[(int) Keys.Left] = false;										// Mark It As Handled
				x = -0.5f;																// Move Left
			}

			if(KeyState[(int) Keys.Right]) {											// Is Down Arrow Being Pressed?
				KeyState[(int) Keys.Right] = false;										// Mark It As Handled
				x = 0.5f;																// Move Right
			}

			if(KeyState[(int) Keys.A]) {												// Is Up Arrow Being Pressed?
				KeyState[(int) Keys.A] = false;											// Mark It As Handled
				z = 0.5f;																// Zoom In
			}

			if(KeyState[(int) Keys.Z]) {												// Is Down Arrow Being Pressed?
				KeyState[(int) Keys.Z] = false;											// Mark It As Handled
				z = -0.5f;																// Zoom Out
			}

			coolEffect.MoveOrigin(new Vector3D(x, y, z));								// Update The Flame Position
		}
		#endregion ProcessInput()

		#region Setup()
		/// <summary>
		/// Overrides application and OpenGL settings and setup.
		/// </summary>
		public override void Setup() {
			base.Setup();																// Run The Base Setup
			App.FovY = 60f;																// Change FovY
			App.FarClippingPlane = 200.0f;												// Change FarClippingPlane
		}
		#endregion Setup()

		// --- Lesson Methods ---
		#region LoadTextures()
		/// <summary>
		/// Loads and creates the texture.
		/// </summary>
		private void LoadTextures() {
			string filename = @"..\..\data\SchaapFire\Fire.bmp";						// The File To Load
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