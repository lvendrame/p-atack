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
 *		This Code Was Created By Lionel Brits & Jeff Molofee 2000
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
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

#region AssemblyInfo
[assembly: AssemblyCompany("The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyCopyright("2002 The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyDescription("NeHe Lesson 10")]
[assembly: AssemblyProduct("NeHe Lesson 10")]
[assembly: AssemblyTitle("NeHe Lesson 10")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace NeHeLessons {
	/// <summary>
	/// NeHe Lesson 10 -- Loading And Moving Through A 3D World (http://nehe.gamedev.net)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class NeHeLesson10 : Model {
		// --- Fields ---
		#region Private Fields
		private static bool light = false;												// Lighting On / Off
		private static bool blend = false;												// Blending On / Off
		private static bool lp;															// L Pressed?
		private static bool bp;															// B Pressed?
		private static bool fp;															// F Pressed?

		private const float piover180 = 0.0174532925f;									// Conversion Facter For Converting Degrees To Radians
		private static float heading = 0.0f;											// Player's Heading
		private static float xpos = 0.0f;												// Player's X Position
		private static float zpos = 0.0f;												// Player's Y Position

		private static float yrot = 0.0f;												// Y Rotation
		private static float walkbias = 0.0f;											// Head Bobbing
		private static float walkbiasangle = 0.0f;										// Head Bobbing Angle
		private static float lookupdown = 0.0f;											// Up/Down Angle

		private static float[] LightAmbient = {0.5f, 0.5f, 0.5f, 1.0f};					// Ambient Light Values
		private static float[] LightDiffuse = {1.0f, 1.0f, 1.0f, 1.0f};					// Diffuse Light Values
		private static float[] LightPosition = {0.0f, 0.0f, 2.0f, 1.0f};				// Light Position

		private struct Vertex {															// Build Our Vertex Structure
			public float x, y, z;														// 3D Coordinates
			public float u, v;															// Texture Coordinates
		}

		private struct Triangle {														// Build Our Triangle Structure
			public Vertex[] vertex;														// Vertices Per Triangle (3)
		}

		private struct Sector {															// Build Our Sector Structure
			public int numtriangles;													// Triangles Per Sector
			public Triangle[] triangle;													// The Array Of Triangles Making Up This Sector
		}

		private static Sector sector1;													// Our World Model Goes Here

		private static int filter = 0;													// Which Filter To Use [0-2]
		private static uint[] texture = new uint[3];									// Storage For 3 Textures
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Lesson title.
		/// </summary>
		public override string Title {
			get {
				return "NeHe Lesson 10 -- Loading And Moving Through A 3D World";
			}
		}

		/// <summary>
		/// Lesson description.
		/// </summary>
		public override string Description {
			get {
				return "In this lesson you will learn how to load a simple, 3D world from a data file and how to navigate around in the world.";
			}
		}

		/// <summary>
		/// Lesson URL.
		/// </summary>
		public override string Url {
			get {
				return "http://nehe.gamedev.net/tutorials/lesson.asp?l=10";
			}
		}
		#endregion Public Properties

		// --- Entry Point ---
		#region Main()
		/// <summary>
		/// Application's entry point, runs this NeHe lesson.
		/// </summary>
		public static void Main() {														// Entry Point
			App.Run(new NeHeLesson10());												// Run Our NeHe Lesson As A Windows Forms Application
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
			glBlendFunc(GL_SRC_ALPHA, GL_ONE);											// Set The Blending Function For Translucency
			glClearColor(0.0f, 0.0f, 0.0f, 0.0f);										// Override The Base Clear Color
			glDepthFunc(GL_LESS);														// Override The Base Depth Test To Do

			glLightfv(GL_LIGHT1, GL_AMBIENT, LightAmbient);								// Setup The Ambient Light
			glLightfv(GL_LIGHT1, GL_DIFFUSE, LightDiffuse);								// Setup The Diffuse Light
			glLightfv(GL_LIGHT1, GL_POSITION, LightPosition);							// Position The Light
			glEnable(GL_LIGHT1);														// Enable Light One

			glColor4f(1.0f, 1.0f, 1.0f, 0.5f);											// Full Brightness.  50% Alpha

			LoadTextures();																// Jump To Texture Loading Routine

			SetupWorld();																// Load The World
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws NeHe Lesson 10 scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);							// Clear Screen And Depth Buffer
			glLoadIdentity();															// Reset The Current Modelview Matrix

			float x_m, y_m, z_m, u_m, v_m;												// Floating Point For Temp X, Y, Z, U And V Vertices
			float xtrans = -xpos;														// Used For Player Translation On The X Axis
			float ztrans = -zpos;														// Used For Player Translation On The Z Axis
			float ytrans = -walkbias - 0.25f;											// Used For Bouncing Motion Up And Down
			float sceneroty = 360.0f - yrot;											// 360 Degree Angle For Player Direction

			int numtriangles;															// Integer To Hold The Number Of Triangles

			glRotatef(lookupdown, 1.0f, 0, 0);											// Rotate Up And Down To Look Up And Down
			glRotatef(sceneroty, 0, 1.0f, 0);											// Rotate Depending On Direction Player Is Facing

			glTranslatef(xtrans, ytrans, ztrans);										// Translate The Scene Based On Player Position
			glBindTexture(GL_TEXTURE_2D, texture[filter]);								// Select A Texture Based On filter

			numtriangles = sector1.numtriangles;										// Get The Number Of Triangles In Sector 1

			// Process Each Triangle
			for (int loop_m = 0; loop_m < numtriangles; loop_m++) {						// Loop Through All The Triangles
				glBegin(GL_TRIANGLES);													// Start Drawing Triangles
					glNormal3f( 0.0f, 0.0f, 1.0f);										// Normal Pointing Forward
					x_m = sector1.triangle[loop_m].vertex[0].x;							// X Vertex Of 1st Point
					y_m = sector1.triangle[loop_m].vertex[0].y;							// Y Vertex Of 1st Point
					z_m = sector1.triangle[loop_m].vertex[0].z;							// Z Vertex Of 1st Point
					u_m = sector1.triangle[loop_m].vertex[0].u;							// U Texture Coord Of 1st Point
					v_m = sector1.triangle[loop_m].vertex[0].v;							// V Texture Coord Of 1st Point
					glTexCoord2f(u_m, v_m); glVertex3f(x_m, y_m, z_m);					// Set The TexCoord And Vertex

					x_m = sector1.triangle[loop_m].vertex[1].x;							// X Vertex Of 2nd Point
					y_m = sector1.triangle[loop_m].vertex[1].y;							// Y Vertex Of 2nd Point
					z_m = sector1.triangle[loop_m].vertex[1].z;							// Z Vertex Of 2nd Point
					u_m = sector1.triangle[loop_m].vertex[1].u;							// U Texture Coord Of 2nd Point
					v_m = sector1.triangle[loop_m].vertex[1].v;							// V Texture Coord Of 2nd Point
					glTexCoord2f(u_m, v_m); glVertex3f(x_m, y_m, z_m);					// Set The TexCoord And Vertex

					x_m = sector1.triangle[loop_m].vertex[2].x;							// X Vertex Of 3rd Point
					y_m = sector1.triangle[loop_m].vertex[2].y;							// Y Vertex Of 3rd Point
					z_m = sector1.triangle[loop_m].vertex[2].z;							// Z Vertex Of 3rd Point
					u_m = sector1.triangle[loop_m].vertex[2].u;							// U Texture Coord Of 3rd Point
					v_m = sector1.triangle[loop_m].vertex[2].v;							// V Texture Coord Of 3rd Point
					glTexCoord2f(u_m, v_m); glVertex3f(x_m, y_m, z_m);					// Set The TexCoord And Vertice
				glEnd();																// Done Drawing Triangles
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

			dataRow = InputHelpDataTable.NewRow();										// Up Arrow - Move Forward
			dataRow["Input"] = "Up Arrow";
			dataRow["Effect"] = "Move Forward";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Down Arrow - Move Backward
			dataRow["Input"] = "Down Arrow";
			dataRow["Effect"] = "Move Backward";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Left Arrow - Turn Left
			dataRow["Input"] = "Left Arrow";
			dataRow["Effect"] = "Turn Left";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Right Arrow - Turn Right
			dataRow["Input"] = "Right Arrow";
			dataRow["Effect"] = "Turn Right";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Page Up - Look Up
			dataRow["Input"] = "Page Up";
			dataRow["Effect"] = "Look Up";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Page Down - Look Down
			dataRow["Input"] = "Page Down";
			dataRow["Effect"] = "Look Down";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Space - Reset Position
			dataRow["Input"] = "Space Bar";
			dataRow["Effect"] = "Reset Position";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// L - Toggle Light
			dataRow["Input"] = "L";
			dataRow["Effect"] = "Toggle Light On / Off";
			if(light) {
				dataRow["Current State"] = "On";
			}
			else {
				dataRow["Current State"] = "Off";
			}
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// F - Cycle Through Texture Filters
			dataRow["Input"] = "F";
			dataRow["Effect"] = "Cycle Through Texture Filters [0-2]";
			if(filter == 0) {
				dataRow["Current State"] = "0 (Nearest)";
			}
			else if(filter == 1) {
				dataRow["Current State"] = "1 (Linear)";
			}
			else {
				dataRow["Current State"] = "2 (Mipmapped)";
			}
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// B - Toggle Blending
			dataRow["Input"] = "B";
			dataRow["Effect"] = "Toggle Blending On / Off";
			if(blend) {
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
				xpos -= (float) Math.Sin(heading * piover180) * 0.05f;					// Move On The X-Plane Based On Player Direction
				zpos -= (float) Math.Cos(heading * piover180) * 0.05f;					// Move On The Z-Plane Based On Player Direction
				if(walkbiasangle >= 359.0f) {											// Is walkbiasangle >= 359?
					walkbiasangle = 0.0f;												// Make walkbiasangle Equal 0
				}
				else {																	// Otherwise
					walkbiasangle += 10;												// Increase walkbiasangle By 10
				}
				walkbias = (float) Math.Sin(walkbiasangle * piover180) / 20.0f;			// Causes The Player To Bounce
			}

			if(KeyState[(int) Keys.Down]) {												// Is Down Arrow Being Pressed?
				xpos += (float) Math.Sin(heading * piover180) * 0.05f;					// Move On The X-Plane Based On Player Direction
				zpos += (float) Math.Cos(heading * piover180) * 0.05f;					// Move On The Z-Plane Based On Player Direction
				if(walkbiasangle <= 1.0f) {												// Is walkbiasangle <= 1?
					walkbiasangle = 359.0f;												// Make walkbiasangle Equal 359
				}
				else {																	// Otherwise
					walkbiasangle -= 10;												// Decrease walkbiasangle By 10
				}
				walkbias = (float) Math.Sin(walkbiasangle * piover180) / 20.0f;			// Causes The Player To Bounce
			}

			if(KeyState[(int) Keys.Left]) {												// Is Left Arrow Being Pressed?
				heading += 1.0f;														// Rotate The Scene To The Right
				yrot = heading;
			}

			if(KeyState[(int) Keys.Right]) {											// Is Right Arrow Being Pressed?
				heading -= 1.0f;														// Rotate The Scene To The Left
				yrot = heading;
			}

			if(KeyState[(int) Keys.PageUp]) {											// Is Page Up Being Pressed?
				lookupdown -= 1.0f;														// If So, Look Up
			}

			if(KeyState[(int) Keys.PageDown]) {											// Is Page Down Being Pressed?
				lookupdown += 1.0f;														// If So, Look Down
			}

			if(KeyState[(int) Keys.Space]) {											// Is Space Bar Being Pressed?
				heading = 0.0f;															// If So, Reset Position
				xpos = 0.0f;
				zpos = 0.0f;
				yrot = 0.0f;
				walkbias = 0.0f;
				walkbiasangle = 0.0f;
				lookupdown = 0.0f;
			}

			if(KeyState[(int) Keys.L] && !lp) {											// Is L Key Being Pressed And Not Held Down?
				lp = true;																// lp Becomes true
				light = !light;															// Toggle Light true / false
				if(!light) {															// If Not Light
					glDisable(GL_LIGHTING);												// Disable Lighting
				}
				else {																	// Otherwise
					glEnable(GL_LIGHTING);												// Enable Lighting
				}
				UpdateInputHelp();														// Update The Input Help Screen
			}
			if(!KeyState[(int) Keys.L]) {												// Has L Key Been Released?
				lp = false;																// If So, lp Becomes false
			}

			if(KeyState[(int) Keys.F] && !fp) {											// Is F Key Being Pressed And Not Held Down?
				fp = true;																// fp Becomes true
				filter += 1;															// filter Value Increases By One
				if(filter > 2) {														// Is Value Greater Than 2?
					filter = 0;															// If So, Set filter To 0
				}
				UpdateInputHelp();														// Update The Input Help Screen
			}
			if(!KeyState[(int) Keys.F]) {												// Has F Key Been Released?
				fp = false;																// If So, fp Becomes false
			}

			if(KeyState[(int) Keys.B] && !bp) {											// Is B Key Being Pressed And Not Held Down?
				bp = true;																// bp Becomes true
				blend = !blend;															// Toggle Blending true / false
				if(blend) {																// If Blending
					glEnable(GL_BLEND);													// Turn Blending On
					glDisable(GL_DEPTH_TEST);											// Turn Depth Testing Off
				}
				else {																	// Otherwise
					glDisable(GL_BLEND);												// Turn Blending Off
					glEnable(GL_DEPTH_TEST);											// Turn Depth Testing On
				}
				UpdateInputHelp();														// Update The Input Help Screen
			}
			if(!KeyState[(int) Keys.B]) {												// Has B Key Been Released?
				bp = false;																// If So, bp Becomes false
			}
		}
		#endregion ProcessInput()

		// --- Lesson Methods ---
		#region LoadTextures()
		/// <summary>
		/// Loads and creates the textures.
		/// </summary>
		private void LoadTextures() {
			string filename = @"..\..\data\NeHeLesson10\Mud.bmp";						// The File To Load
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

				glGenTextures(3, texture);												// Create Three Textures

				// Create Nearest Filtered Texture
				glBindTexture(GL_TEXTURE_2D, texture[0]);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
				glTexImage2D(GL_TEXTURE_2D, 0, (int) GL_RGB8, bitmap.Width, bitmap.Height, 0, GL_BGR_EXT, GL_UNSIGNED_BYTE, bitmapData.Scan0);

				// Create Linear Filtered Texture
				glBindTexture(GL_TEXTURE_2D, texture[1]);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR); 
				glTexImage2D(GL_TEXTURE_2D, 0, (int) GL_RGB8, bitmap.Width, bitmap.Height, 0, GL_BGR_EXT, GL_UNSIGNED_BYTE, bitmapData.Scan0);

				// Create MipMapped Texture
				glBindTexture(GL_TEXTURE_2D, texture[2]);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_NEAREST);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
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

		#region SetupWorld()
		/// <summary>
		/// Loads and parses the world.
		/// </summary>
		private void SetupWorld() {
			// This Method Is A Little Messy.  The .Net Framework Doesn't Have An Analogous Implementation 
			// Of C/C++'s sscanf().  As Such You Have To Manually Parse A File, You Can Either Do So 
			// Procedurally Like I'm Doing Here, Or Use Some RegEx's.  To Make It A Bit Easier I Modified
			// The World.txt File To Remove Comments, Empty Lines And Excess Spaces.  Sorry For The
			// Ugliness.  Let Me Know If You Have Redone This To Be Cleaner Or Find A Nice sscanf().

			float x, y, z, u, v;														// Local Vertex Information
			int numtriangles;															// Local Number Of Triangles
			string oneline = "";														// The Line We've Read
			string[] splitter;															// Array For Split Values
			StreamReader reader = null;													// Our StreamReader
			ASCIIEncoding encoding = new ASCIIEncoding();								// ASCII Encoding
			string filename = @"..\..\data\NeHeLesson10\WorldCsGL.txt";					// The File To Load

			try {
				reader = new StreamReader(filename, encoding);							// Open The File As ASCII Text

				oneline = reader.ReadLine();											// Read The First Line
				splitter = oneline.Split();												// Split The Line On Spaces

				// The First Item In The Array Will Contain The String "NUMPOLLIES", Which We Will Ignore
				numtriangles = Convert.ToInt32(splitter[1]);							// Save The Number Of Triangles As An int

				sector1.triangle = new Triangle[numtriangles];							// Initialize The Triangles And Save To sector1
				sector1.numtriangles = numtriangles;									// Save The Number Of Triangles In sector1

				for(int triloop = 0; triloop < numtriangles; triloop++) {				// For Every Triangle
					sector1.triangle[triloop].vertex = new Vertex[3];					// Initialize The Vertices In sector1
					for(int vertloop = 0; vertloop < 3; vertloop++) {					// For Every Vertex
						oneline = reader.ReadLine();									// Read A Line
						if(oneline != null) {											// If The Line Isn't null
							splitter = oneline.Split();									// Split The Line On Spaces
							x = Single.Parse(splitter[0]);								// Save x As A float
							y = Single.Parse(splitter[1]);								// Save y As A float
							z = Single.Parse(splitter[2]);								// Save z As A float
							u = Single.Parse(splitter[3]);								// Save u As A float
							v = Single.Parse(splitter[4]);								// Save v As A float
							sector1.triangle[triloop].vertex[vertloop].x = x;			// Save x To sector1's Current triangle's vertex x
							sector1.triangle[triloop].vertex[vertloop].y = y;			// Save y To sector1's Current triangle's vertex y
							sector1.triangle[triloop].vertex[vertloop].z = z;			// Save z To sector1's Current triangle's vertex z
							sector1.triangle[triloop].vertex[vertloop].u = u;			// Save u To sector1's Current triangle's vertex u
							sector1.triangle[triloop].vertex[vertloop].v = v;			// Save v To sector1's Current triangle's vertex v
						}
					}
				}
			}
			catch(Exception e) {
				// Handle Any Exceptions While Loading World Data, Exit App
				string errorMsg = "An Error Occurred While Loading And Parsing World Data:\n\t" + filename + "\n" + "\n\nStack Trace:\n\t" + e.StackTrace + "\n";
				MessageBox.Show(errorMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				App.Terminate();
			}
			finally {
				if(reader != null) {
					reader.Close();														// Close The StreamReader
				}
			}
		}
		#endregion SetupWorld()
	}
}