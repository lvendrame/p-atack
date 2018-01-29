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
 *		This Code Was Published By Jeff Molofee 2000
 *		Code Was Created By David Nikdel For NeHe Productions
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
[assembly: AssemblyDescription("NeHe Lesson 28")]
[assembly: AssemblyProduct("NeHe Lesson 28")]
[assembly: AssemblyTitle("NeHe Lesson 28")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace NeHeLessons {
	/// <summary>
	/// NeHe Lesson 28 -- Bezier Patches (http://nehe.gamedev.net)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class NeHeLesson28 : Model {
		// --- Fields ---
		#region Private Fields
		private struct Point3d {														// Structure For A 3-Dimensional Point
			public double x, y, z;														// Coordinates Of The Point
		};

		private struct Patch {															// Structure For A 3rd Degree Bezier Patch
			public Point3d[ , ] anchors;												// 4x4 Grid Of Anchor Points
			public uint dlBPatch;														// Display List For Bezier Patch
		};

		private static float rotz = 0.0f;												// Rotation About The Z Axis
		private static Patch mybezier;													// The Bezier Patch We're Going To Use
		private static bool showCPoints = true;											// Toggles Displaying The Control Point Grid
		private static int divs = 7;													// Number Of Intrapolations (Controls Poly Resolution)

		private static uint[] texture = new uint[1];									// Texture For The Patch
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Lesson title.
		/// </summary>
		public override string Title {
			get {
				return "NeHe Lesson 28 -- Bezier Patches";
			}
		}

		/// <summary>
		/// Lesson description.
		/// </summary>
		public override string Description {
			get {
				return "In this lesson you will learn how to create bezier patches, how to alter a surface by modifying control points, and how to texture map and animate such a surface.";
			}
		}

		/// <summary>
		/// Lesson URL.
		/// </summary>
		public override string Url {
			get {
				return "http://nehe.gamedev.net/tutorials/lesson.asp?l=28";
			}
		}
		#endregion Public Properties

		// --- Entry Point ---
		#region Main()
		/// <summary>
		/// Application's entry point, runs this NeHe lesson.
		/// </summary>
		public static void Main() {														// Entry Point
			App.Run(new NeHeLesson28());												// Run Our NeHe Lesson As A Windows Forms Application
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

			LoadTextures();																// Jump To The Texture Loading Routine
			InitBezier();																// Initialize the Bezier's Control Grid
			mybezier.dlBPatch = GenBezier(mybezier, divs);								// Generate The Patch
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws NeHe Lesson 28 scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			int i, j;																	// Generic Loop Variables
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);							// Clear Screen And Depth Buffer
			glLoadIdentity();															// Reset The Current Modelview Matrix

			glTranslatef(0.0f, 0.0f, -4.0f);											// Move Into The Screen 4.0
			glRotatef(-75.0f, 1.0f, 0.0f, 0.0f);
			glRotatef(rotz, 0.0f, 0.0f, 1.0f);											// Rotate The Triangle On The Z-Axis

			glCallList(mybezier.dlBPatch);												// Call The Bezier's Display List
																						// This Need Only Be Updated When The Patch Changes

			if(showCPoints) {															// If Drawing The Grid Is Toggled On
				glDisable(GL_TEXTURE_2D);												// Disable Texturing
				glColor3f(1.0f, 0.0f, 0.0f);											// Red
				for(i = 0; i < 4; i++) {												// Draw The Horizontal Lines
					glBegin(GL_LINE_STRIP);
					for(j = 0; j < 4; j++) {
						glVertex3d(mybezier.anchors[i, j].x, mybezier.anchors[i, j].y, mybezier.anchors[i, j].z);
					}
					glEnd();
				}
				for(i = 0; i < 4; i++) {												// Draw The Vertical Lines
					glBegin(GL_LINE_STRIP);
					for(j = 0; j < 4; j++) {
						glVertex3d(mybezier.anchors[j, i].x, mybezier.anchors[j, i].y, mybezier.anchors[j, i].z);
					}
					glEnd();
				}
				glColor3f(1.0f, 1.0f, 1.0f);											// White
				glEnable(GL_TEXTURE_2D);												// Reenable Texturing
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

			dataRow = InputHelpDataTable.NewRow();										// Up Arrow - Increase Divisions
			dataRow["Input"] = "Up Arrow";
			dataRow["Effect"] = "Increase Divisions";
			dataRow["Current State"] = divs.ToString() + " Divisions";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Down Arrow - Decrease Divisions
			dataRow["Input"] = "Down Arrow";
			dataRow["Effect"] = "Decrease Divisions";
			dataRow["Current State"] = divs.ToString() + " Divisions";
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

			dataRow = InputHelpDataTable.NewRow();										// Space Bar - Toggle Control Points
			dataRow["Input"] = "Space Bar";
			dataRow["Effect"] = "Toggle Control Points On / Off";
			if(showCPoints) {
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
				divs++;																	// Increase Divisions
				mybezier.dlBPatch = GenBezier(mybezier, divs);							// Update The Patch
				UpdateInputHelp();														// Update Input Help Screen
				KeyState[(int) Keys.Up] = false;										// Set Key To False
			}

			if(KeyState[(int) Keys.Down]) {												// Is Down Arrow Being Pressed?
				if(divs > 1) {															// If We Have At Least 2 Divisions
					divs--;																// Decrease Divisions
				}
				mybezier.dlBPatch = GenBezier(mybezier, divs);							// Update The Patch
				UpdateInputHelp();														// Update Input Help Screen
				KeyState[(int) Keys.Down] = false;										// Set Key To False
			}

			if(KeyState[(int) Keys.Left]) {												// Is Left Arrow Being Pressed?
				rotz -= 0.8f;															// Rotate Left
			}

			if(KeyState[(int) Keys.Right]) {											// Is Right Arrow Being Pressed?
				rotz += 0.8f;															// Rotate Right
			}

			if(KeyState[(int) Keys.Space]) {											// Is Space Bar Being Pressed?
				showCPoints = !showCPoints;												// Toggle Display Of Control Points
				UpdateInputHelp();														// Update Input Help Screen
				KeyState[(int) Keys.Space] = false;										// Set Key To False
			}
		}
		#endregion ProcessInput()

		// --- Lesson Methods ---
		#region Point3d Bernstein(float u, Point3d[] p)
		/// Calculates 3rd-degree polynomial based on array if 4 points and a single variable (u) which is generally between 0 and 1.
		/// </summary>
		/// <param name="u">The transformation.</param>
		/// <param name="p">The Point3d.</param>
		/// <returns>A new Point3d.</returns>
		private Point3d Bernstein(float u, Point3d[] p) {
			Point3d a, b, c, d, r;

			a = PointTimes(Math.Pow(u, 3), p[0]);
			b = PointTimes(3 * Math.Pow(u, 2) * (1-u), p[1]);
			c = PointTimes(3 * u * Math.Pow((1 - u), 2), p[2]);
			d = PointTimes(Math.Pow((1 - u), 3), p[3]);
			r = PointAdd(PointAdd(a, b), PointAdd(c, d));

			return r;
		}
		#endregion Point3d Bernstein(float u, Point3d[] p)

		#region uint GenBezier(Patch patch, int divs)
		/// <summary>
		/// Generates a display list based on the data in the Patch and the number of divisions.
		/// </summary>
		/// <param name="patch">The Patch.</param>
		/// <param name="divs">The number of divisions.</param>
		/// <returns>A uint referring to the new display list.</returns>
		private uint GenBezier(Patch patch, int divs) {
			int u, v;																	// Texture Coordinates
			float py, px, pyold;														// Percents Along The Axes
			Point3d[] temp = new Point3d[4];
			Point3d[] blah = new Point3d[4];
			Point3d[] last = new Point3d[divs+1];

			uint drawlist = glGenLists(1);												// Make The Display List

			if(patch.dlBPatch != 0) {													// If We Have An Old Display List
				glDeleteLists(patch.dlBPatch, 1);										// Get Rid Of It
			}

			temp[0] = patch.anchors[0, 3];												// The First Derived Curve (Along X-Axis)
			temp[1] = patch.anchors[1, 3];
			temp[2] = patch.anchors[2, 3];
			temp[3] = patch.anchors[3, 3];

			for(v = 0; v <= divs; v++) {												// Create The First Line Of Points
				px = (float) v / (float) divs;											// Percent Along Y-Axis
				// Use The 4 Points From The Derived Curve To Calculate The Points Along That Curve
				last[v] = Bernstein(px, temp);
			}

			glNewList(drawlist, GL_COMPILE);											// Start A New Display List
			glBindTexture(GL_TEXTURE_2D, texture[0]);									// Bind The Texture

			for(u = 1; u <= divs; u++) {												// For Each Division
				py = (float) u / (float) divs;											// Percent Along Y-Axis
				pyold = ((float) u - 1.0f) / (float) divs;								// Percent Along Old Y Axis

				// This Crap With blah Is An Ugly Hack, Someone Send Me A Good Fix
				// I Was Too Tired To Come Up With The Proper Solution

				// Calculate New Bezier Points
				blah[0] = patch.anchors[0, 0];
				blah[1] = patch.anchors[0, 1];
				blah[2] = patch.anchors[0, 2];
				blah[3] = patch.anchors[0, 3];
				temp[0] = Bernstein(py, blah);
				patch.anchors[0, 0] = blah[0];
				patch.anchors[0, 1] = blah[1];
				patch.anchors[0, 2] = blah[2];
				patch.anchors[0, 3] = blah[3];

				blah[0] = patch.anchors[1, 0];
				blah[1] = patch.anchors[1, 1];
				blah[2] = patch.anchors[1, 2];
				blah[3] = patch.anchors[1, 3];
				temp[1] = Bernstein(py, blah);
				patch.anchors[1, 0] = blah[0];
				patch.anchors[1, 1] = blah[1];
				patch.anchors[1, 2] = blah[2];
				patch.anchors[1, 3] = blah[3];

				blah[0] = patch.anchors[2, 0];
				blah[1] = patch.anchors[2, 1];
				blah[2] = patch.anchors[2, 2];
				blah[3] = patch.anchors[2, 3];
				temp[2] = Bernstein(py, blah);
				patch.anchors[2, 0] = blah[0];
				patch.anchors[2, 1] = blah[1];
				patch.anchors[2, 2] = blah[2];
				patch.anchors[2, 3] = blah[3];

				blah[0] = patch.anchors[3, 0];
				blah[1] = patch.anchors[3, 1];
				blah[2] = patch.anchors[3, 2];
				blah[3] = patch.anchors[3, 3];
				temp[3] = Bernstein(py, blah);
				patch.anchors[3, 0] = blah[0];
				patch.anchors[3, 1] = blah[1];
				patch.anchors[3, 2] = blah[2];
				patch.anchors[3, 3] = blah[3];

				glBegin(GL_TRIANGLE_STRIP);												// Begin A New Triangle Strip
				for(v = 0; v <= divs; v++) {											// For Each Division
					px = (float) v / (float) divs;										// Percent Along The X-Axis

					glTexCoord2f(pyold, px);											// Apply The Old Texture Coords
					glVertex3d(last[v].x, last[v].y, last[v].z);						// Old Point

					last[v] = Bernstein(px, temp);										// Generate New Point
					glTexCoord2f(py, px);												// Apply The New Texture Coords
					glVertex3d(last[v].x, last[v].y, last[v].z);						// New Point
				}
				glEnd();																// End The Triangle Strip
			}
	
			glEndList();																// End The List

			return drawlist;															// Return The New Display List
		}
		#endregion uint GenBezier(Patch patch, int divs)

		#region InitBezier()
		/// <summary>
		/// Initializes a new bezier patch.
		/// </summary>
		private void InitBezier() {
			mybezier.anchors = new Point3d[4, 4];										// Initialize The anchors Array

			// Set The Bezier Vertices
			mybezier.anchors[0, 0] = MakePoint(-0.75, -0.75, -0.50);
			mybezier.anchors[0, 1] = MakePoint(-0.25, -0.75,  0.00);
			mybezier.anchors[0, 2] = MakePoint( 0.25, -0.75,  0.00);
			mybezier.anchors[0, 3] = MakePoint( 0.75, -0.75, -0.50);
			mybezier.anchors[1, 0] = MakePoint(-0.75, -0.25, -0.75);
			mybezier.anchors[1, 1] = MakePoint(-0.25, -0.25,  0.50);
			mybezier.anchors[1, 2] = MakePoint( 0.25, -0.25,  0.50);
			mybezier.anchors[1, 3] = MakePoint( 0.75, -0.25, -0.75);
			mybezier.anchors[2, 0] = MakePoint(-0.75,  0.25,  0.00);
			mybezier.anchors[2, 1] = MakePoint(-0.25,  0.25, -0.50);
			mybezier.anchors[2, 2] = MakePoint( 0.25,  0.25, -0.50);
			mybezier.anchors[2, 3] = MakePoint( 0.75,  0.25,  0.00);
			mybezier.anchors[3, 0] = MakePoint(-0.75,  0.75, -0.50);
			mybezier.anchors[3, 1] = MakePoint(-0.25,  0.75, -1.00);
			mybezier.anchors[3, 2] = MakePoint( 0.25,  0.75, -1.00);
			mybezier.anchors[3, 3] = MakePoint( 0.75,  0.75, -0.50);
			mybezier.dlBPatch = 0;
		}
		#endregion InitBezier()

		#region LoadTextures()
		/// <summary>
		/// Loads and creates the texture.
		/// </summary>
		private void LoadTextures() {
			string filename = @"..\..\data\NeHeLesson28\NeHe.bmp";						// The File To Load
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

		#region Point3d MakePoint(double x, double y, double z)
		/// <summary>
		/// Makes a new Point3d.
		/// </summary>
		/// <param name="x">New X coordinate.</param>
		/// <param name="y">New Y coordinate.</param>
		/// <param name="z">New Z coordinate.</param>
		/// <returns>A new Point3d.</returns>
		private Point3d MakePoint(double x, double y, double z) {
			Point3d p;
			p.x = x;
			p.y = y;
			p.z = z;
			return p;
		}
		#endregion Point3d MakePoint(double x, double y, double z)

		#region Point3d PointAdd(Point3d p, Point3d q)
		/// <summary>
		/// Adds two Point3d's.
		/// </summary>
		/// <param name="p">First Point3d.</param>
		/// <param name="q">Second Point3d.</param>
		/// <returns>A new Point3d.</returns>
		private Point3d PointAdd(Point3d p, Point3d q) {
			p.x += q.x;
			p.y += q.y;
			p.z += q.z;
			return p;
		}
		#endregion Point3d PointAdd(Point3d p, Point3d q)

		#region Point3d PointTimes(double c, Point3d p)
		/// <summary>
		/// Multiplies a Point3d by a constant.
		/// </summary>
		/// <param name="c">The constant.</param>
		/// <param name="p">The Point3d.</param>
		/// <returns>A new Point3d.</returns>
 		private Point3d PointTimes(double c, Point3d p) {
			p.x *= c;
			p.y *= c;
			p.z *= c;
			return p;
		}
		#endregion Point3d PointTimes(double c, Point3d p)
	}
}