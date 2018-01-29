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
/*		This code has been created by Banu Cosmin aka Choko - 20 may 2000
 *		and uses NeHe tutorials as a starting point (window initialization,
 *		texture loading, GL initialization and code for keypresses) - very good
 *		tutorials, Jeff. If anyone is interested about the presented algorithm
 *		please e-mail me at boct@romwest.ro
 *
 *		Code Commmenting And Clean Up By Jeff Molofee ( NeHe )
 *		NeHe Productions	...		http://nehe.gamedev.net
 */
#endregion Original Credits / License

using CsGL.Basecode;
using CsGL.OpenGL;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Windows.Forms;

#region AssemblyInfo
[assembly: AssemblyCompany("The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyCopyright("2002 The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyDescription("NeHe Lesson 26")]
[assembly: AssemblyProduct("NeHe Lesson 26")]
[assembly: AssemblyTitle("NeHe Lesson 26")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace NeHeLessons {
	/// <summary>
	/// NeHe Lesson 26 -- Clipping & Reflections Using The Stencil Buffer (http://nehe.gamedev.net)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class NeHeLesson26 : Model {
		// --- Fields ---
		#region Private Fields
		private static float[] LightAmbient = {0.7f, 0.7f, 0.7f, 1.0f};					// Ambient Light Values
		private static float[] LightDiffuse = {1.0f, 1.0f, 1.0f, 1.0f};					// Diffuse Light Values
		private static float[] LightPosition = {4.0f, 4.0f, 6.0f, 1.0f};				// Light Position

		private static GLUquadric quadratic;											// Quadratic For Drawing A Sphere

		private static float xrot = 0.0f;												// X Rotation
		private static float yrot = 0.0f;												// Y Rotation
		private static float xrotspeed = 0.0f;											// X Rotation Speed
		private static float yrotspeed = 0.0f;											// Y Rotation Speed
		private static float zoom = -7.0f;												// Depth Into Screen
		private static float height = 2.0f;												// Height Of Ball From Floor

		private static uint[] texture = new uint[3];									// Storage For 3 Textures
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Lesson title.
		/// </summary>
		public override string Title {
			get {
				return "NeHe Lesson 26 -- Clipping & Reflections Using The Stencil Buffer";
			}
		}

		/// <summary>
		/// Lesson description.
		/// </summary>
		public override string Description {
			get {
				return "This lesson demonstrates using the stencil buffer, clipping, and multi-texturing to produce reflections.  This lesson is somewhat advanced, so make sure you've understood the previous ones.  This lesson also requires support for the stencil buffer and 32-bit color depth, most modern cards should support these features.";
			}
		}

		/// <summary>
		/// Lesson URL.
		/// </summary>
		public override string Url {
			get {
				return "http://nehe.gamedev.net/tutorials/lesson.asp?l=26";
			}
		}
		#endregion Public Properties

		// --- Entry Point ---
		#region Main()
		/// <summary>
		/// Application's entry point, runs this NeHe lesson.
		/// </summary>
		public static void Main() {														// Entry Point
			App.Run(new NeHeLesson26());												// Run Our NeHe Lesson As A Windows Forms Application
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
			glClearColor(0.2f, 0.5f, 1.0f, 1.0f);										// Override Background
			glClearStencil(0);															// Clear The Stencil Buffer To 0

			LoadTextures();																// Jump To Texture Loading Routine

			glLightfv(GL_LIGHT0, GL_AMBIENT, LightAmbient);								// Set The Ambient Lighting For Light0
			glLightfv(GL_LIGHT0, GL_DIFFUSE, LightDiffuse);								// Set The Diffuse Lighting For Light0
			glLightfv(GL_LIGHT0, GL_POSITION, LightPosition);							// Set The Position For Light0

			glEnable(GL_LIGHT0);														// Enable Light 0
			glEnable(GL_LIGHTING);														// Enable Lighting

			quadratic = gluNewQuadric();												// Create A New Quadratic
			gluQuadricNormals(quadratic, GLU_SMOOTH);									// Generate Smooth Normals For The Quad
			gluQuadricTexture(quadratic, (byte) GL_TRUE);								// Enable Texture Coords For The Quad
			glTexGeni(GL_S, GL_TEXTURE_GEN_MODE, (int) GL_SPHERE_MAP);					// Set Up Sphere Mapping
			glTexGeni(GL_T, GL_TEXTURE_GEN_MODE, (int) GL_SPHERE_MAP);					// Set Up Sphere Mapping
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws NeHe Lesson 26 scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT | GL_STENCIL_BUFFER_BIT);	// Clear Screen And Depth Buffer And Stencil Buffer

			// Clip Plane Equations
			double[] eqr = {0.0f, -1.0f, 0.0f, 0.0f};									// Plane Equation To Use For The Reflected Objects

			glLoadIdentity();															// Reset The Current Modelview Matrix
			glTranslatef(0.0f, -0.6f, zoom);											// Zoom And Raise Camera Above The Floor (Up 0.6 Units)

			glColorMask(0, 0, 0, 0);													// Set Color Mask

			glEnable(GL_STENCIL_TEST);													// Enable Stencil Buffer For "marking" The Floor
			glStencilFunc(GL_ALWAYS, 1, 1);												// Always Passes, 1 Bit Plane, 1 As Mask
			glStencilOp(GL_KEEP, GL_KEEP, GL_REPLACE);									// We Set The Stencil Buffer To 1 Where We Draw Any Polygon
																						// Keep If Test Fails, Keep If Test Passes But Buffer Test Fails
																						// Replace If Test Passes
			glDisable(GL_DEPTH_TEST);													// Disable Depth Testing
			DrawFloor();																// Draw The Floor (Draws To The Stencil Buffer)
																						// We Only Want To Mark It In The Stencil Buffer

			glEnable(GL_DEPTH_TEST);													// Enable Depth Testing
			glColorMask(1, 1, 1, 1);													// Set Color Mask to TRUE, TRUE, TRUE, TRUE
			glStencilFunc(GL_EQUAL, 1, 1);												// We Draw Only Where The Stencil Is 1
																						// (I.E. Where The Floor Was Drawn)
			glStencilOp(GL_KEEP, GL_KEEP, GL_KEEP);										// Don't Change The Stencil Buffer

			glEnable(GL_CLIP_PLANE0);													// Enable Clip Plane For Removing Artifacts
																						// (When The Object Crosses The Floor)
			glClipPlane(GL_CLIP_PLANE0, eqr);											// Equation For Reflected Objects
			glPushMatrix();																// Push The Matrix Onto The Stack
				glScalef(1.0f, -1.0f, 1.0f);											// Mirror Y Axis
				glLightfv(GL_LIGHT0, GL_POSITION, LightPosition);						// Set Up Light0
				glTranslatef(0.0f, height, 0.0f);										// Position The Object
				glRotatef(xrot, 1.0f, 0.0f, 0.0f);										// Rotate Local Coordinate System On X Axis
				glRotatef(yrot, 0.0f, 1.0f, 0.0f);										// Rotate Local Coordinate System On Y Axis
				DrawBall();																// Draw The Sphere (Reflection)
			glPopMatrix();																// Pop The Matrix Off The Stack
			glDisable(GL_CLIP_PLANE0);													// Disable Clip Plane For Drawing The Floor
			glDisable(GL_STENCIL_TEST);													// We Don't Need The Stencil Buffer Any More (Disable)

			glLightfv(GL_LIGHT0, GL_POSITION, LightPosition);							// Set Up Light0 Position
			glEnable(GL_BLEND);															// Enable Blending (Otherwise The Reflected Object Wont Show)
			glDisable(GL_LIGHTING);														// Since We Use Blending, We Disable Lighting
			glColor4f(1.0f, 1.0f, 1.0f, 0.8f);											// Set Color To White With 80% Alpha
			glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);							// Blending Based On Source Alpha And 1 Minus Dest Alpha
			DrawFloor();																// Draw The Floor To The Screen

			glEnable(GL_LIGHTING);														// Enable Lighting
			glDisable(GL_BLEND);														// Disable Blending
			glTranslatef(0.0f, height, 0.0f);											// Position The Ball At Proper Height
			glRotatef(xrot, 1.0f, 0.0f, 0.0f);											// Rotate On The X Axis
			glRotatef(yrot, 0.0f, 1.0f, 0.0f);											// Rotate On The Y Axis
			DrawBall();																	// Draw The Ball

			xrot += xrotspeed;															// Update X Rotation Angle By xrotspeed
			yrot += yrotspeed;															// Update Y Rotation Angle By yrotspeed
			glFlush();																	// Flush The GL Pipeline
		}
		#endregion Draw()

		#region InputHelp()
		/// <summary>
		/// Overrides default input help, supplying lesson-specific help information.
		/// </summary>
		public override void InputHelp() {
			base.InputHelp();															// Set Up The Default Input Help

			DataRow dataRow;															// Row To Add

			dataRow = InputHelpDataTable.NewRow();										// Up Arrow - Decrease X Rotation
			dataRow["Input"] = "Up Arrow";
			dataRow["Effect"] = "Decrease X Rotation";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Down Arrow - Increase X Rotation
			dataRow["Input"] = "Down Arrow";
			dataRow["Effect"] = "Increase X Rotation";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Left Arrow - Decrease Y Rotation
			dataRow["Input"] = "Left Arrow";
			dataRow["Effect"] = "Decrease Y Rotation";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Right Arrow - Increase Y Rotation
			dataRow["Input"] = "Right Arrow";
			dataRow["Effect"] = "Increase Y Rotation";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Page Up - Move Ball Up
			dataRow["Input"] = "Page Up";
			dataRow["Effect"] = "Move Ball Up";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Page Down - Move Ball Down
			dataRow["Input"] = "Page Down";
			dataRow["Effect"] = "Move Ball Down";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Left Arrow - Decrease Y Rotation
			dataRow["Input"] = "Left Arrow";
			dataRow["Effect"] = "Decrease Y Rotation";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// A - Zoom In
			dataRow["Input"] = "A";
			dataRow["Effect"] = "Zoom In";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Z - Zoom Out
			dataRow["Input"] = "Z";
			dataRow["Effect"] = "Zoom Out";
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

			if(KeyState[(int) Keys.Up]) {												// Is Up Arrow Key Being Pressed?
				xrotspeed -= 0.08f;														// Decrease xrotspeed
			}

			if(KeyState[(int) Keys.Down]) {												// Is Down Arrow Key Being Pressed?
				xrotspeed += 0.08f;														// Increase xrotspeed
			}

			if(KeyState[(int) Keys.Left]) {												// Is Left Arrow Key Being Pressed?
				yrotspeed -= 0.08f;														// Decrease yrotspeed
			}

			if(KeyState[(int) Keys.Right]) {											// Is Right Arrow Key Being Pressed?
				yrotspeed += 0.08f;														// Increase yrotspeed
			}

			if(KeyState[(int) Keys.PageUp]) {											// Is Page Up Key Being Pressed?
				height += 0.03f;														// Move Ball Up
			}

			if(KeyState[(int) Keys.PageDown]) {											// Is Page Down Key Being Pressed?
				height -= 0.03f;														// Move Ball Down
			}

			if(KeyState[(int) Keys.A]) {												// Is A Key Being Pressed?
				zoom += 0.05f;															// Zoom In
			}

			if(KeyState[(int) Keys.Z]) {												// Is Z Key Being Pressed?
				zoom -= 0.05f;															// Zoom Out
			}
		}
		#endregion ProcessInput()

		#region Setup()
		/// <summary>
		/// Overrides application and OpenGL settings and setup.
		/// </summary>
		public override void Setup() {
			base.Setup();																// Run The Base Setup
			App.StencilDepth = 1;														// Setup A Stencil Bit
			App.ColorDepth = 32;														// Let's Try Out 32 Bit Color
		}
		#endregion Setup()

		// --- Lesson Methods ---
		#region DrawBall()
		/// <summary>
		/// Draw the ball.
		/// </summary>
		private void DrawBall() {														// Draw Our Ball
			glColor3f(1.0f, 1.0f, 1.0f);												// Set Color To White
			glBindTexture(GL_TEXTURE_2D, texture[1]);									// Select Texture 2 (1)
			gluSphere(quadratic, 0.35f, 32, 16);										// Draw First Sphere

			glBindTexture(GL_TEXTURE_2D, texture[2]);									// Select Texture 3 (2)
			glColor4f(1.0f, 1.0f, 1.0f, 0.4f);											// Set Color To White With 40% Alpha
			glEnable(GL_BLEND);															// Enable Blending
			glBlendFunc(GL_SRC_ALPHA, GL_ONE);											// Set Blending Mode To Mix Based On SRC Alpha
			glEnable(GL_TEXTURE_GEN_S);													// Enable Sphere Mapping
			glEnable(GL_TEXTURE_GEN_T);													// Enable Sphere Mapping

			gluSphere(quadratic, 0.35f, 32, 16);										// Draw Another Sphere Using New Texture
																						// Textures Will Mix Creating A MultiTexture Effect (Reflection)
			glDisable(GL_TEXTURE_GEN_S);												// Disable Sphere Mapping
			glDisable(GL_TEXTURE_GEN_T);												// Disable Sphere Mapping
			glDisable(GL_BLEND);														// Disable Blending
		}
		#endregion DrawBall()

		#region DrawFloor()
		/// <summary>
		/// Draw the floor.
		/// </summary>
		private void DrawFloor() {														// Draws The Floor
			glBindTexture(GL_TEXTURE_2D, texture[0]);									// Select Texture 1 (0)
			glBegin(GL_QUADS);															// Begin Drawing A Quad
				glNormal3f(0.0f, 1.0f, 0.0f);											// Normal Pointing Up
				glTexCoord2f(0.0f, 1.0f);												// Bottom Left Of Texture
				glVertex3f(-2.0f, 0.0f, 2.0f);											// Bottom Left Corner Of Floor
				glTexCoord2f(0.0f, 0.0f);												// Top Left Of Texture
				glVertex3f(-2.0f, 0.0f, -2.0f);											// Top Left Corner Of Floor
				glTexCoord2f(1.0f, 0.0f);												// Top Right Of Texture
				glVertex3f(2.0f, 0.0f, -2.0f);											// Top Right Corner Of Floor
				glTexCoord2f(1.0f, 1.0f);												// Bottom Right Of Texture
				glVertex3f(2.0f, 0.0f, 2.0f);											// Bottom Right Corner Of Floor
			glEnd();																	// Done Drawing The Quad
		}
		#endregion DrawFloor()

		#region LoadTextures()
		/// <summary>
		/// Loads and creates the textures.
		/// </summary>
		private void LoadTextures() {
			// The Files To Load
			string[] filename = {
				@"..\..\data\NeHeLesson26\Envwall.bmp",									// The Floor Texture
				@"..\..\data\NeHeLesson26\Ball.bmp",									// The Light Texture
				@"..\..\data\NeHeLesson26\Envroll.bmp",									// The Wall Texture
			};
			Bitmap bitmap = null;														// The Bitmap Image For Our Texture
			Rectangle rectangle;														// The Rectangle For Locking The Bitmap In Memory
			BitmapData bitmapData = null;												// The Bitmap's Pixel Data

			// Load The Bitmaps
			try {
				glGenTextures(3, texture);												// Create 3 Textures

				for(int loop = 0; loop < 3; loop++) {
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