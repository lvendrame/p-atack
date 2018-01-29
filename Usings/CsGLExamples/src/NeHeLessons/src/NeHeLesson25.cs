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
 *		This Code Was Created By Pet & Commented/Cleaned Up By Jeff Molofee
 *		If You've Found This Code Useful, Please Let Me Know.
 *		Visit NeHe Productions At http://nehe.gamedev.net
 */
#endregion Original Credits / License

using CsGL.Basecode;
using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

#region AssemblyInfo
[assembly: AssemblyCompany("The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyCopyright("2002 The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyDescription("NeHe Lesson 25")]
[assembly: AssemblyProduct("NeHe Lesson 25")]
[assembly: AssemblyTitle("NeHe Lesson 25")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace NeHeLessons {
	/// <summary>
	/// NeHe Lesson 25 -- Morphing & Loading Objects From A File (http://nehe.gamedev.net)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class NeHeLesson25 : Model {
		// --- Fields ---
		#region Private Fields
		private static float xrot, yrot, zrot;											// X, Y & Z Rotation
		private static float xspeed, yspeed, zspeed;									// X, Y & Z Spin Speed
		private static float cx = 0, cy = 0, cz = -15;									// X, Y & Z Position

		private static int currentkey = 1;												// Used To Make Sure Same Morph Key Is Not Pressed
		private static int step = 0, steps = 200;										// Step Counter And Maximum Number Of Steps
		private static bool morph = false;												// Default morph To False (Not Morphing)
	
		private struct Vertex {															// Structure For 3D Points
			public float x, y, z;														// 3D Coordinates
		}

		private struct PointObject {													// Structure For An PointObject
			public int verts;															// Number Of Vertices For The PointObject
			public Vertex[] points;														// One Vertex (x, y & z)
		}

		private static int maxver;														// Will Eventually Hold The Maximum Number Of Vertices
		private static PointObject morph1, morph2, morph3, morph4;						// Our 4 Morphable PointObjects (morph1, 2, 3 & 4)
		private static PointObject helper, source, destination;							// Helper PointObject, Source PointObject, Destination PointObject

		private static Random rand = new Random();										// Random Number Generator
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Lesson title.
		/// </summary>
		public override string Title {
			get {
				return "NeHe Lesson 25 -- Morphing & Loading Objects From A File";
			}
		}

		/// <summary>
		/// Lesson description.
		/// </summary>
		public override string Description {
			get {
				return "In this lesson you will learn how to load simple objects from a text file and morph them smoothly into one another.  You could use what you learn here to animate your objects or bend and twist them into new shapes.  You could also modify the code to use lines or solid polygons instead of points.";
			}
		}

		/// <summary>
		/// Lesson URL.
		/// </summary>
		public override string Url {
			get {
				return "http://nehe.gamedev.net/tutorials/lesson.asp?l=25";
			}
		}
		#endregion Public Properties

		// --- Entry Point ---
		#region Main()
		/// <summary>
		/// Application's entry point, runs this NeHe lesson.
		/// </summary>
		public static void Main() {														// Entry Point
			App.Run(new NeHeLesson25());												// Run Our NeHe Lesson As A Windows Forms Application
		}
		#endregion Main()

		// --- Basecode Methods ---
		#region Initialize()
		/// <summary>
		/// Overrides OpenGL's initialization.
		/// </summary>
		public override void Initialize() {
			base.Initialize();															// Run The Base Initialization
			glBlendFunc(GL_SRC_ALPHA, GL_ONE);											// Set The Blending Function For Translucency
			glDepthFunc(GL_LESS);														// The Type Of Depth Test To Do

			maxver = 0;																	// Sets Max Vertices To 0 By Default
			ObjLoad(@"..\..\data\NeHeLesson25\SphereCsGL.txt", ref morph1);				// Load The First PointObject Into morph1 From File SphereCsGL.txt
			ObjLoad(@"..\..\data\NeHeLesson25\TorusCsGL.txt", ref morph2);				// Load The Second PointObject Into morph2 From File TorusCsGL.txt
			ObjLoad(@"..\..\data\NeHeLesson25\TubeCsGL.txt", ref morph3);				// Load The Third PointObject Into morph3 From File TubeCsGL.txt
			ObjAllocate(ref morph4, 486);												// Manually Reserve Ram For A 4th 486 Vertice PointObject (morph4)

			for(int i = 0; i < 486; i++) {												// Loop Through All 486 Vertices
				morph4.points[i].x = ((float) (rand.Next() % 14000) / 1000) - 7;		// morph4 x Point Becomes A Random Float Value From -7 to 7
				morph4.points[i].y = ((float) (rand.Next() % 14000) / 1000) - 7;		// morph4 y Point Becomes A Random Float Value From -7 to 7
				morph4.points[i].z = ((float) (rand.Next() % 14000) / 1000) - 7;		// morph4 z Point Becomes A Random Float Value From -7 to 7
			}

			ObjLoad(@"..\..\data\NeHeLesson25\SphereCsGL.txt", ref helper);				// Load SphereCsGL.txt PointObject Into Helper (Used As Starting Point)
			source = destination = morph1;												// Source & Destination Are Set To Equal First PointObject (morph1)
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws NeHe Lesson 25 scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);							// Clear Screen And Depth Buffer
			glLoadIdentity();															// Reset The Current Modelview Matrix

			glTranslatef(cx, cy, cz);													// Translate The The Current Position To Start Drawing
			glRotatef(xrot, 1, 0, 0);													// Rotate On The X Axis By xrot
			glRotatef(yrot, 0, 1, 0);													// Rotate On The Y Axis By yrot
			glRotatef(zrot, 0, 0, 1);													// Rotate On The Z Axis By zrot

			xrot += xspeed; yrot += yspeed; zrot += zspeed;								// Increase xrot,yrot & zrot by xspeed, yspeed & zspeed

			float tx, ty, tz;															// Temp X, Y & Z Variables
			Vertex q;																	// Holds Returned Calculated Values For One Vertex

			glBegin(GL_POINTS);															// Begin Drawing Points
			for(int i = 0; i < morph1.verts; i++) {										// Loop Through All The Verts Of morph1 (All PointObjects Have
																						// The Same Amount Of Verts For Simplicity, Could Use maxver Also)
				if(morph) {																// If morph Is true
					q = Calculate(i);													// Calculate Movement
				}
				else {																	// Otherwise
					q.x = q.y = q.z = 0;												// Movement = 0
				}
				helper.points[i].x -= q.x;												// Subtract q.x Units From helper.points[i].x (Move On X Axis)
				helper.points[i].y -= q.y;												// Subtract q.y Units From helper.points[i].y (Move On Y Axis)
				helper.points[i].z -= q.z;												// Subtract q.z Units From helper.points[i].z (Move On Z Axis)
				tx = helper.points[i].x;												// Make Temp X Variable Equal To Helper's X Variable
				ty = helper.points[i].y;												// Make Temp Y Variable Equal To Helper's Y Variable
				tz = helper.points[i].z;												// Make Temp Z Variable Equal To Helper's Z Variable

				glColor3f(0, 1, 1);														// Set Color To A Bright Shade Of Off Blue
				glVertex3f(tx, ty, tz);													// Draw A Point At The Current Temp Values (Vertex)
				glColor3f(0, 0.5f, 1);													// Darken Color A Bit
				tx -= 2 * q.x; ty -= 2 * q.y; ty -= 2 * q.y;							// Calculate Two Positions Ahead
				glVertex3f(tx, ty, tz);													// Draw A Second Point At The Newly Calculate Position
				glColor3f(0, 0, 1);														// Set Color To A Very Dark Blue
				tx -= 2 * q.x; ty -= 2 * q.y; ty -= 2 * q.y;							// Calculate Two More Positions Ahead
				glVertex3f(tx, ty, tz);													// Draw A Third Point At The Second New Position
			}																			// This Creates A Ghostly Tail As Points Move
			glEnd();																	// Done Drawing Points

			// If We're Morphing And We Haven't Gone Through All 200 Steps Increase Our Step Counter
			// Otherwise Set Morphing To False, Make Source=Destination And Set The Step Counter Back To Zero.
			if(morph && step <= steps) {
				step++;
			}
			else {
				morph = false;
				source = destination;
				step = 0;
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

			dataRow = InputHelpDataTable.NewRow();										// Up Arrow - Decrease X Speed
			dataRow["Input"] = "Up Arrow";
			dataRow["Effect"] = "Decrease X Speed";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Down Arrow - Increase X Speed
			dataRow["Input"] = "Down Arrow";
			dataRow["Effect"] = "Increase X Speed";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Left Arrow - Decrease Y Speed
			dataRow["Input"] = "Left Arrow";
			dataRow["Effect"] = "Decrease Y Speed";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Right Arrow - Increase Y Speed
			dataRow["Input"] = "Right Arrow";
			dataRow["Effect"] = "Increase Y Speed";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Page Up - Increase Z Speed
			dataRow["Input"] = "Page Up";
			dataRow["Effect"] = "Increase Z Speed";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Page Down - Decrease Z Speed
			dataRow["Input"] = "Page Down";
			dataRow["Effect"] = "Decrease Z Speed";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Q - Move Object Away
			dataRow["Input"] = "Q";
			dataRow["Effect"] = "Move Object Away";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Z - Move Object Closer
			dataRow["Input"] = "Z";
			dataRow["Effect"] = "Move Object Closer";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// W - Move Object Up
			dataRow["Input"] = "W";
			dataRow["Effect"] = "Move Object Up";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// S - Move Object Down
			dataRow["Input"] = "S";
			dataRow["Effect"] = "Move Object Down";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// A - Move Object Left
			dataRow["Input"] = "A";
			dataRow["Effect"] = "Move Object Left";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// D - Move Object Right
			dataRow["Input"] = "D";
			dataRow["Effect"] = "Move Object Right";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// 1 - Select Object 1
			dataRow["Input"] = "1";
			dataRow["Effect"] = "Select Sphere";
			if(currentkey == 1) {
				dataRow["Current State"] = "Active";
			}
			else {
				dataRow["Current State"] = "";
			}
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// 2 - Select Object 2
			dataRow["Input"] = "2";
			dataRow["Effect"] = "Select Torus";
			if(currentkey == 2) {
				dataRow["Current State"] = "Active";
			}
			else {
				dataRow["Current State"] = "";
			}
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// 3 - Select Object 3
			dataRow["Input"] = "3";
			dataRow["Effect"] = "Select Tube";
			if(currentkey == 3) {
				dataRow["Current State"] = "Active";
			}
			else {
				dataRow["Current State"] = "";
			}
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// 4 - Select Object 4
			dataRow["Input"] = "4";
			dataRow["Effect"] = "Select Random Dispersion";
			if(currentkey == 4) {
				dataRow["Current State"] = "Active";
			}
			else {
				dataRow["Current State"] = "";
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
				xspeed -= 0.01f;														// Decrease X Speed
			}

			if(KeyState[(int) Keys.Down]) {												// Is Down Arrow Being Pressed?
				xspeed += 0.01f;														// Increase X Speed
			}

			if(KeyState[(int) Keys.Left]) {												// Is Left Arrow Being Pressed?
				yspeed -= 0.01f;														// Decrease Y Speed
			}

			if(KeyState[(int) Keys.Right]) {											// Is Right Arrow Being Pressed?
				yspeed += 0.01f;														// Increase Y Speed
			}

			if(KeyState[(int) Keys.PageUp]) {											// Is Page Up Being Pressed?
				zspeed += 0.01f;														// Increase Z Speed
			}

			if(KeyState[(int) Keys.PageDown]) {											// Is Page Down Being Pressed?
				zspeed -= 0.01f;														// Decrease Z Speed
			}

			if(KeyState[(int) Keys.Q]) {												// Is Q Being Pressed?
				cz -= 0.01f;															// Move Object Away From Viewer
			}

			if(KeyState[(int) Keys.Z]) {												// Is Z Being Pressed?
				cz += 0.01f;															// Move Object Towards Viewer
			}

			if(KeyState[(int) Keys.W]) {												// Is W Being Pressed?
				cy += 0.01f;															// Move Object Up
			}

			if(KeyState[(int) Keys.S]) {												// Is S Being Pressed?
				cy -= 0.01f;															// Move Object Down
			}

			if(KeyState[(int) Keys.A]) {												// Is A Being Pressed?
				cx -= 0.01f;															// Move Object Left
			}

			if(KeyState[(int) Keys.D]) {												// Is D Being Pressed?
				cx += 0.01f;															// Move Object Right
			}

			if(KeyState[(int) Keys.D1]) {												// Is 1 Being Pressed?
				if(currentkey != 1 && !morph) {
					currentkey = 1;														// Sets currentkey To 1 (To Prevent Pressing 1 2x In A Row)
					morph = true;														// Set morph To True (Starts Morphing Process)
					destination = morph1;												// Destination Object To Morph To Becomes morph1
					UpdateInputHelp();													// Update Help Screen
				}
			}

			if(KeyState[(int) Keys.D2]) {												// Is 2 Being Pressed?
				if(currentkey != 2 && !morph) {
					currentkey = 2;														// Sets currentkey To 2 (To Prevent Pressing 2 2x In A Row)
					morph = true;														// Set morph To True (Starts Morphing Process)
					destination = morph2;												// Destination Object To Morph To Becomes morph2
					UpdateInputHelp();													// Update Help Screen
				}
			}

			if(KeyState[(int) Keys.D3]) {												// Is 3 Being Pressed?
				if(currentkey != 3 && !morph) {
					currentkey = 3;														// Sets currentkey To 3 (To Prevent Pressing 3 2x In A Row)
					morph = true;														// Set morph To True (Starts Morphing Process)
					destination = morph3;												// Destination Object To Morph To Becomes morph3
					UpdateInputHelp();													// Update Help Screen
				}
			}

			if(KeyState[(int) Keys.D4]) {												// Is 4 Being Pressed?
				if(currentkey != 4 && !morph) {
					currentkey = 4;														// Sets currentkey To 4 (To Prevent Pressing 4 2x In A Row)
					morph = true;														// Set morph To True (Starts Morphing Process)
					destination = morph4;												// Destination Object To Morph To Becomes morph4
					UpdateInputHelp();													// Update Help Screen
				}
			}
		}
		#endregion ProcessInput()

		// --- Lesson Methods ---
		#region Vertex Calculate(int i)
		/// <summary>
		/// Calculates movement of points during morphing.
		/// </summary>
		/// <param name="i">The number of the point to calculate.</param>
		/// <returns>A Vertex.</returns>
		private Vertex Calculate(int i) {
			// This Makes Points Move At A Speed So They All Get To Their Destination At The Same Time
			Vertex a;																	// Temporary Vertex Called a
			a.x = (source.points[i].x - destination.points[i].x) / steps;				// a.x Value Equals Source x - Destination x Divided By Steps
			a.y = (source.points[i].y - destination.points[i].y) / steps;				// a.y Value Equals Source y - Destination y Divided By Steps
			a.z = (source.points[i].z - destination.points[i].z) / steps;				// a.z Value Equals Source z - Destination z Divided By Steps
			return a;																	// Return The Results
		}
		#endregion Vertex Calculate(int i)

		#region ObjAllocate(ref PointObject k, int n)
		/// <summary>
		/// Allocate memory for each PointObject and defines points.
		/// </summary>
		/// <param name="k">The PointObject to save to.</param>
		/// <param name="n">The number of vertices.</param>
		private void ObjAllocate(ref PointObject k, int n) {
			k.points = new Vertex[n];													// Sets points Equal To VERTEX * Number Of Vertices
		}
		#endregion ObjAllocate(ref PointObject k, int n)

		#region ObjFree(ref PointObject k)
		/// <summary>
		/// Frees the PointObject (releasing the memory).
		/// </summary>
		/// <param name="k">The PointObject to free.</param>
		private void ObjFree(ref PointObject k) {
			k.points = null;															// Frees Points
		}
		#endregion ObjFree(ref PointObject k)

		#region ObjLoad(string filename, ref PointObject k)
		/// <summary>
		/// Loads a PointObject from a file.
		/// </summary>
		/// <param name="filename">The file to load.</param>
		/// <param name="k">The PointObject to save to.</param>
		private void ObjLoad(string filename, ref PointObject k) {
			// This Method Is A Little Messy.  The .Net Framework Doesn't Have An Analogous Implementation 
			// Of C/C++'s sscanf().  As Such You Have To Manually Parse A File, You Can Either Do So 
			// Procedurally Like I'm Doing Here, Or Use Some RegEx's.  To Make It A Bit Easier I Modified
			// The Text Files To Remove Comments, Empty Lines And Excess Spaces.  Sorry For The
			// Ugliness.  Let Me Know If You Have Redone This To Be Cleaner Or Find A Nice sscanf().

			int ver;																	// Will Hold Vertice Count
			float rx, ry, rz;															// Hold Vertex X, Y & Z Position
			string oneline = "";														// The Line We've Read
			string[] splitter;															// Array For Split Values
			StreamReader reader = null;													// Our StreamReader
			ASCIIEncoding encoding = new ASCIIEncoding();								// ASCII Encoding

			try {
				reader = new StreamReader(filename, encoding);							// Open The File As ASCII Text

				oneline = reader.ReadLine();											// Read The First Line
				splitter = oneline.Split();												// Split The Line On Spaces

				// The First Item In The Array Will Contain The String "Vertices:", Which We Will Ignore
				ver = Convert.ToInt32(splitter[1]);										// Save The Number Of Triangles To ver As An int
				k.verts = ver;															// Sets PointObjects (k) verts Variable To Equal The Value Of ver
				ObjAllocate(ref k, ver);												// Jumps To Code That Allocates Ram To Hold The Object

				for(int vertloop = 0; vertloop < ver; vertloop++) {						// Loop Through The Vertices
					oneline = reader.ReadLine();										// Reads In The Next Line Of Text
					if(oneline != null) {												// If The Line's Not null
						splitter = oneline.Split();										// Split The Line On Spaces
						rx = Single.Parse(splitter[0]);									// Save The X Value As A Float
						ry = Single.Parse(splitter[1]);									// Save The Y Value As A Float
						rz = Single.Parse(splitter[2]);									// Save The Z Value As A Float
						k.points[vertloop].x = rx;										// Sets PointObjects (k) points.x Value To rx
						k.points[vertloop].y = ry;										// Sets PointObjects (k) points.y Value To ry
						k.points[vertloop].z = rz;										// Sets PointObjects (k) points.z Value To rz
					}
				}

				if(ver > maxver) {														// If ver Is Greater Than maxver
					// maxver Keeps Track Of The Highest Number Of Vertices Used In Any Of The Objects
					maxver = ver;														// Set maxver Equal To ver
				}
			}
			catch(Exception e) {
				// Handle Any Exceptions While Loading Object Data, Exit App
				string errorMsg = "An Error Occurred While Loading And Parsing Object Data:\n\t" + filename + "\n" + "\n\nStack Trace:\n\t" + e.StackTrace + "\n";
				MessageBox.Show(errorMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				App.Terminate();
			}
			finally {
				if(reader != null) {
					reader.Close();														// Close The StreamReader
				}
			}
		}
		#endregion ObjLoad(string filename, ref PointObject k)
	}
}