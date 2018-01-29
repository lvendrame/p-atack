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
/**************************************
*                                     *
*   Jeff Molofee's Basecode Example   *
*          nehe.gamedev.net           *
*                2001                 *
*                                     *
*    All Code / Tutorial Commenting   *
*       by Jeff Molofee ( NeHe )      *
*                                     *
**************************************/
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
[assembly: AssemblyDescription("NeHe Lesson 36")]
[assembly: AssemblyProduct("NeHe Lesson 36")]
[assembly: AssemblyTitle("NeHe Lesson 36")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace NeHeLessons {
	/// <summary>
	/// NeHe Lesson 36 -- Radial Blur & Rendering To A Texture (http://nehe.gamedev.net)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class NeHeLesson36 : Model {
		// --- Fields ---
		#region Private Fields
		private static float angle = 0.0f;												// Used To Rotate The Helix
		private static float[ , ] vertices = new float[4, 3];							// An Array Of 4 Floats To Store The Vertex Data
		private static float[] normal = new float[3];									// An Array To Store The Normal Data
		private static uint blurTexture;												// An Unsigned Int To Store The Texture Number
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Lesson title.
		/// </summary>
		public override string Title {
			get {
				return "NeHe Lesson 36 -- Radial Blur & Rendering To A Texture";
			}
		}

		/// <summary>
		/// Lesson description.
		/// </summary>
		public override string Description {
			get {
				return "This lesson will show you how to create an impressive radial blur effect that doesn't require extensions and also how to render to a texture using the offscreen buffer.";
			}
		}

		/// <summary>
		/// Lesson URL.
		/// </summary>
		public override string Url {
			get {
				return "http://nehe.gamedev.net/tutorials/lesson.asp?l=36";
			}
		}
		#endregion Public Properties

		// --- Entry Point ---
		#region Main()
		/// <summary>
		/// Application's entry point, runs this NeHe lesson.
		/// </summary>
		public static void Main() {														// Entry Point
			App.Run(new NeHeLesson36());												// Run Our NeHe Lesson As A Windows Forms Application
		}
		#endregion Main()

		// --- Basecode Methods ---
		#region Initialize()
		/// <summary>
		/// Overrides OpenGL's initialization.
		/// </summary>
		public override void Initialize() {
			base.Initialize();															// Run The Base Initialization

			angle = 0.0f;																// Set Starting Angle To Zero

			blurTexture = EmptyTexture();												// Create Our Empty Texture

			float[] global_ambient = {0.2f, 0.2f, 0.2f, 1.0f};							// Set Ambient Lighting To Fairly Dark Light (No Color)
			float[] light0pos = {0.0f, 5.0f, 10.0f, 1.0f};								// Set The Light Position
			float[] light0ambient = {0.2f, 0.2f, 0.2f, 1.0f};							// More Ambient Light
			float[] light0diffuse = {0.3f, 0.3f, 0.3f, 1.0f};							// Set The Diffuse Light A Bit Brighter
			float[] light0specular = {0.8f, 0.8f, 0.8f, 1.0f};							// Fairly Bright Specular Lighting

			float[] lmodel_ambient = {0.2f, 0.2f, 0.2f, 1.0f};							// And More Ambient Light
			glLightModelfv(GL_LIGHT_MODEL_AMBIENT, lmodel_ambient);						// Set The Ambient Light Model

			glLightModelfv(GL_LIGHT_MODEL_AMBIENT, global_ambient);						// Set The Global Ambient Light Model
			glLightfv(GL_LIGHT0, GL_POSITION, light0pos);								// Set The Lights Position
			glLightfv(GL_LIGHT0, GL_AMBIENT, light0ambient);							// Set The Ambient Light
			glLightfv(GL_LIGHT0, GL_DIFFUSE, light0diffuse);							// Set The Diffuse Light
			glLightfv(GL_LIGHT0, GL_SPECULAR, light0specular);							// Set Up Specular Lighting
			glEnable(GL_LIGHTING);														// Enable Lighting
			glEnable(GL_LIGHT0);														// Enable Light0

			glMateriali(GL_FRONT, GL_SHININESS, 128);
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws NeHe Lesson 36 scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			angle += 5.0f;																// Update angle (We Should Do This Based On The Timer)
			glClearColor(0.0f, 0.0f, 0.0f, 0.5f);										// Set The Clear Color To Black
			glClear (GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);						// Clear Screen And Depth Buffer
			glLoadIdentity();															// Reset The View
			RenderToTexture();															// Render To A Texture
			ProcessHelix();																// Draw Our Helix
			DrawBlur(25, 0.02f);														// Draw The Blur Effect
			glFlush();																	// Flush The GL Rendering Pipeline
		}
		#endregion Draw()

		#region Setup()
		/// <summary>
		/// Overrides application and OpenGL settings and setup.
		/// </summary>
		public override void Setup() {
			base.Setup();																// Run The Base Setup
			App.NearClippingPlane = 5f;													// Override GLU's NearClippingPlane Distance
			App.FarClippingPlane = 2000f;												// Override GLU's FarClippingPlane Distance
		}
		#endregion Setup()

		// --- Lesson Methods ---
		#region CalculateNormal(float[ , ] v, ref float[] output)
		/// <summary>
		/// Calculates normal for a quad using 3 points, finds the vector between 2 points by subtracting
		/// the x, y, z coordinates from one point to another.
		/// </summary>
		/// <param name="v">The vectors.</param>
		/// <param name="output">The output vectors.</param>
		private void CalculateNormal(float[ , ] v, ref float[] output) {
			float[] v1 = new float[3];													// Vector 1 (x,y,z)
			float[] v2 = new float[3];													// Vector 2 (x,y,z)
			const int x = 0;															// Define X Coord
			const int y = 1;															// Define Y Coord
			const int z = 2;															// Define Z Coord

			// Calculate The Vector From Point 1 To Point 0
			v1[x] = v[0, x] - v[1, x];													// Vector 1.x = Vertex[0].x - Vertex[1].x
			v1[y] = v[0, y] - v[1, y];													// Vector 1.y = Vertex[0].y - Vertex[1].y
			v1[z] = v[0, z] - v[1, z];													// Vector 1.z = Vertex[0].y - Vertex[1].z

			// Calculate The Vector From Point 2 To Point 1
			v2[x] = v[1, x] - v[2, x];													// Vector 2.x = Vertex[0].x - Vertex[1].x
			v2[y] = v[1, y] - v[2, y];													// Vector 2.y = Vertex[0].y - Vertex[1].y
			v2[z] = v[1, z] - v[2, z];													// Vector 2.z = Vertex[0].z - Vertex[1].z

			// Compute The Cross Product To Give Us A Surface Normal
			output[x] = v1[y] * v2[z] - v1[z] * v2[y];									// Cross Product For Y - Z
			output[y] = v1[z] * v2[x] - v1[x] * v2[z];									// Cross Product For X - Z
			output[z] = v1[x] * v2[y] - v1[y] * v2[x];									// Cross Product For X - Y

			ReduceToUnit(output);														// Normalize The Vectors
		}
		#endregion CalculateNormal(float[ , ] v, ref float[] output)

		#region DrawBlur(int times, float inc)
		/// <summary>
		/// Draws the blurred image.
		/// </summary>
		/// <param name="times">Number of times to draw.</param>
		/// <param name="inc">The increment.</param>
		private void DrawBlur(int times, float inc) {
			float spost = 0.0f;															// Starting Texture Coordinate Offset
			float alphainc = 0.9f / times;												// Fade Speed For Alpha Blending
			float alpha = 0.2f;															// Starting Alpha Value

			// Disable AutoTexture Coordinates
			glDisable(GL_TEXTURE_GEN_S);
			glDisable(GL_TEXTURE_GEN_T);

			glEnable(GL_TEXTURE_2D);													// Enable 2D Texture Mapping
			glDisable(GL_DEPTH_TEST);													// Disable Depth Testing
			glBlendFunc(GL_SRC_ALPHA, GL_ONE);											// Set Blending Mode
			glEnable(GL_BLEND);															// Enable Blending
			glBindTexture(GL_TEXTURE_2D, blurTexture);									// Bind To The Blur Texture
			ViewOrtho();																// Switch To An Ortho View

			alphainc = alpha / times;													// alphainc = 0.2f / Times To Render Blur

			glBegin(GL_QUADS);															// Begin Drawing Quads
			for(int num = 0; num < times; num++) {										// Number Of Times To Render Blur
				glColor4f(1.0f, 1.0f, 1.0f, alpha);										// Set The Alpha Value (Starts At 0.2)
				glTexCoord2f(0 + spost, 1 - spost);										// Texture Coordinate (0, 1)
				glVertex2f(0, 0);														// First Vertex (0, 0)

				glTexCoord2f(0 + spost, 0 + spost);										// Texture Coordinate (0, 0)
				if(App.IsFullscreen) {													// If Fullscreen
					glVertex2f(0, App.Height);											// Second Vertex
				}
				else {
					glVertex2f(0, App.Form.ClientSize.Height);							// Second Vertex
				}

				glTexCoord2f(1 - spost, 0 + spost);										// Texture Coordinate (1, 0)
				if(App.IsFullscreen) {													// If Fullscreen
					glVertex2f(App.Width, App.Height);									// Third Vertex
				}
				else {
					// Third Vertex
					glVertex2f(App.Form.ClientSize.Width, App.Form.ClientSize.Height);
				}

				glTexCoord2f(1 - spost, 1 - spost);										// Texture Coordinate (1, 1)
				if(App.IsFullscreen) {													// If Fullscreen
					glVertex2f(App.Width, 0);											// Fourth Vertex
				}
				else {
					glVertex2f(App.Form.ClientSize.Width, 0);							// Fourth Vertex
				}

				spost += inc;															// Gradually Increase spost (Zooming Closer To Texture Center)
				alpha = alpha - alphainc;												// Gradually Decrease alpha (Gradually Fading Image Out)
			}
			glEnd();																	// Done Drawing Quads

			ViewPerspective();															// Switch To A Perspective View

			glEnable(GL_DEPTH_TEST);													// Enable Depth Testing
			glDisable(GL_TEXTURE_2D);													// Disable 2D Texture Mapping
			glDisable(GL_BLEND);														// Disable Blending
			glBindTexture(GL_TEXTURE_2D, 0);											// Unbind The Blur Texture
		}
		#endregion DrawBlur(int times, float inc)

		#region EmptyTexture()
		/// <summary>
		/// Creates an empty texture.
		/// </summary>
		/// <returns>The Texture Number.</returns>
		private uint EmptyTexture() {
			uint[] txtnumber = new uint[1];												// Texture ID

			glGenTextures(1, txtnumber);												// Create 1 Texture

			// Create A New Bitmap
			Bitmap bitmap = new Bitmap(128, 128);
			bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
			Rectangle rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
			BitmapData bitmapData = bitmap.LockBits(rectangle, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

			glBindTexture(GL_TEXTURE_2D, txtnumber[0]);									// Bind The Texture
			glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR); 
			glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
			glTexImage2D(GL_TEXTURE_2D, 0, (int)GL_RGB8, bitmap.Width, bitmap.Height, 0, GL_BGRA_EXT, GL_UNSIGNED_BYTE, bitmapData.Scan0);

			bitmap.UnlockBits(bitmapData);
			bitmap.Dispose();

			return txtnumber[0];														// Return The Texture ID
		}
		#endregion EmptyTexture()

		#region ProcessHelix()
		/// <summary>
		/// Draws the helix.
		/// </summary>
		private void ProcessHelix() {
			float x;																	// Helix x Coordinate
			float y;																	// Helix y Coordinate
			float z;																	// Helix z Coordinate
			float phi;																	// Angle
			float theta;																// Angle
			float u, v;																	// Angles
			float r;																	// Radius Of Twist
			int twists = 5;																// 5 Twists

			float[] glfMaterialColor = {0.4f, 0.2f, 0.8f, 1.0f};						// Set The Material Color
			float[] specular = {1.0f, 1.0f, 1.0f, 1.0f};								// Sets Up Specular Lighting

			glLoadIdentity();															// Reset The Modelview Matrix
			gluLookAt(0, 5, 50, 0, 0, 0, 0, 1, 0);										// Eye Position (0, 5, 50) Center Of Scene (0, 0, 0) Up On Y Axis.

			glPushMatrix();																// Push The Modelview Matrix
				glTranslatef(0, 0, -50);												// Translate 50 Units Into The Screen
				glRotatef(angle / 2.0f, 1,0, 0);										// Rotate By angle / 2 On The X-Axis
				glRotatef(angle / 3.0f, 0, 1, 0);										// Rotate By angle / 3 On The Y-Axis
				glMaterialfv(GL_FRONT_AND_BACK, GL_AMBIENT_AND_DIFFUSE, glfMaterialColor);
				glMaterialfv(GL_FRONT_AND_BACK, GL_SPECULAR, specular);
				r = 1.5f;																// Radius
				glBegin(GL_QUADS);														// Begin Drawing Quads
				for(phi = 0; phi <= 360; phi += 20) {									// 360 Degrees In Steps Of 20
					for(theta = 0; theta <= 360 * twists; theta += 20) {				// 360 Degrees * Number Of Twists In Steps Of 20
						v = (phi / 180.0f * 3.142f);									// Calculate Angle Of First Point
						u = (theta / 180.0f * 3.142f);									// Calculate Angle Of First Point
						x = (float) Math.Cos(u) * (2.0f + (float) Math.Cos(v)) * r;		// Calculate x Position (1st Point)
						y = (float) Math.Sin(u) * (2.0f + (float) Math.Cos(v)) * r;		// Calculate y Position (1st Point)
						z = (float) ((u - (2.0f * 3.142f)) + (float) Math.Sin(v)) * r;	// Calculate z Position (1st Point)
						vertices[0, 0] = x;												// Set x Value Of First Vertex
						vertices[0, 1] = y;												// Set y Value Of First Vertex
						vertices[0, 2] = z;												// Set z Value Of First Vertex

						v = (phi / 180.0f * 3.142f);									// Calculate Angle Of Second Point
						u = ((theta + 20) / 180.0f * 3.142f);							// Calculate Angle Of Second Point
						x = (float) Math.Cos(u) * (2.0f + (float) Math.Cos(v)) * r;		// Calculate x Position (2nd Point)
						y = (float) Math.Sin(u) * (2.0f + (float) Math.Cos(v)) * r;		// Calculate y Position (2nd Point)
						z = (float) ((u - (2.0f * 3.142f)) + (float) Math.Sin(v)) * r;	// Calculate z Position (2nd Point)
						vertices[1, 0] = x;												// Set x Value Of Second Vertex
						vertices[1, 1] = y;												// Set y Value Of Second Vertex
						vertices[1, 2] = z;												// Set z Value Of Second Vertex

						v = ((phi + 20) / 180.0f * 3.142f);								// Calculate Angle Of Third Point
						u = ((theta + 20) / 180.0f * 3.142f);							// Calculate Angle Of Third Point
						x = (float) Math.Cos(u) * (2.0f + (float) Math.Cos(v)) * r;		// Calculate x Position (3rd Point)
						y = (float) Math.Sin(u) * (2.0f + (float) Math.Cos(v)) * r;		// Calculate y Position (3rd Point)
						z = (float) ((u - (2.0f * 3.142f)) + (float) Math.Sin(v)) * r;	// Calculate z Position (3rd Point)
						vertices[2, 0] = x;												// Set x Value Of Third Vertex
						vertices[2, 1] = y;												// Set y Value Of Third Vertex
						vertices[2, 2] = z;												// Set z Value Of Third Vertex

						v = ((phi + 20) / 180.0f * 3.142f);								// Calculate Angle Of Fourth Point
						u = ((theta) / 180.0f * 3.142f);								// Calculate Angle Of Fourth Point
						x = (float) Math.Cos(u) * (2.0f + (float) Math.Cos(v)) * r;		// Calculate x Position (4th Point)
						y = (float) Math.Sin(u) * (2.0f + (float) Math.Cos(v)) * r;		// Calculate y Position (4th Point)
						z = (float) ((u - (2.0f * 3.142f)) + (float) Math.Sin(v)) * r;	// Calculate z Position (4th Point)
						vertices[3, 0] = x;												// Set x Value Of Fourth Vertex
						vertices[3, 1] = y;												// Set y Value Of Fourth Vertex
						vertices[3, 2] = z;												// Set z Value Of Fourth Vertex

						CalculateNormal(vertices, ref normal);							// Calculate The Quad Normal
						glNormal3f(normal[0], normal[1], normal[2]);					// Set The Normal

						// Render The Quad
						glVertex3f(vertices[0, 0], vertices[0, 1], vertices[0, 2]);
						glVertex3f(vertices[1, 0], vertices[1, 1], vertices[1, 2]);
						glVertex3f(vertices[2, 0], vertices[2, 1], vertices[2, 2]);
						glVertex3f(vertices[3, 0], vertices[3, 1], vertices[3, 2]);
					}
				}
				glEnd();																// Done Rendering Quads
			glPopMatrix();																// Pop The Matrix
		}
		#endregion ProcessHelix()

		#region RenderToTexture()
		/// <summary>
		/// Renders to a texture.
		/// </summary>
		private void RenderToTexture() {
			glViewport(0, 0, 128, 128);													// Set Our Viewport (Match Texture Size)

			ProcessHelix();																// Render The Helix

			glBindTexture(GL_TEXTURE_2D, blurTexture);									// Bind To The Blur Texture

			// Copy Our ViewPort To The Blur Texture (From 0,0 To 128,128... No Border)
			glCopyTexImage2D(GL_TEXTURE_2D, 0, GL_LUMINANCE, 0, 0, 128, 128, 0);

			glClearColor(0.0f, 0.0f, 0.5f, 0.5f);										// Set The Clear Color To Medium Blue
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);							// Clear The Screen And Depth Buffer

			if(App.IsFullscreen) {														// If Fullscreen
				glViewport(0, 0, App.Width, App.Height);								// Set Viewport, Based On Fullscreen Width & Height
			}
			else {																		// Otherwise, Windowed
				// Set Viewport, Based On Window's ClientSize Width & Height
				glViewport(0, 0, App.Form.ClientSize.Width, App.Form.ClientSize.Height);
			}
		}
		#endregion RenderToTexture()

		#region ReduceToUnit(float[] vector)
		/// <summary>
		/// Reduces a normal vector (3 coordinates) to a unit normal vector with a length of one.
		/// </summary>
		/// <param name="vector">The vector to reduce.</param>
		private void ReduceToUnit(float[] vector) {
			float length;																// Holds Unit Length

			// Calculates The Length Of The Vector
			length = (float) Math.Sqrt((vector[0] * vector[0]) + (vector[1] * vector[1]) + (vector[2] * vector[2]));

			if(length == 0.0f) {														// Prevents Divide By 0 Error By Providing
				length = 1.0f;															// An Acceptable Value For Vectors Too Close To 0.
			}
			vector[0] /= length;														// Dividing Each Element By
			vector[1] /= length;														// The Length Results In A
			vector[2] /= length;														// Unit Normal Vector.
		}
		#endregion ReduceToUnit(float[] vector)

		#region ViewOrtho()
		/// <summary>
		/// Sets up an ortho view.
		/// </summary>
		private void ViewOrtho() {
			glMatrixMode(GL_PROJECTION);												// Select Projection
			glPushMatrix();																// Push The Matrix
			glLoadIdentity();															// Reset The Matrix
			if(App.IsFullscreen) {														// If Fullscreen
				glOrtho(0, App.Width, App.Height, 0, -1, 1);							// Select Ortho Mode Based On Fullscreen Width & Height
			}
			else {																		// Otherwise, Windowed
				// Select Ortho Mode Based On Window's ClientSize Width & Height
				glOrtho(0, App.Form.ClientSize.Width, App.Form.ClientSize.Height, 0, -1, 1);
			}
			glMatrixMode(GL_MODELVIEW);													// Select Modelview Matrix
			glPushMatrix();																// Push The Matrix
			glLoadIdentity();															// Reset The Matrix
		}
		#endregion ViewOrtho()

		#region ViewPerspective()
		/// <summary>
		/// Sets up a perspective view.
		/// </summary>
		private void ViewPerspective() {
			glMatrixMode(GL_PROJECTION);												// Select Projection
			glPopMatrix();																// Pop The Matrix
			glMatrixMode(GL_MODELVIEW);													// Select Modelview
			glPopMatrix();																// Pop The Matrix
		}
		#endregion ViewPerspective()
	}
}