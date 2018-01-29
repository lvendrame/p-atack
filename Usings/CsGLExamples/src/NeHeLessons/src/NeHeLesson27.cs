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
 *		Attention!!! This code is not for beginners.
 */
#endregion Original Credits / License

using CsGL.Basecode;
using CsGL.OpenGL;
using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

#region AssemblyInfo
[assembly: AssemblyCompany("The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyCopyright("2002 The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyDescription("NeHe Lesson 27")]
[assembly: AssemblyProduct("NeHe Lesson 27")]
[assembly: AssemblyTitle("NeHe Lesson 27")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace NeHeLessons {
	/// <summary>
	/// NeHe Lesson 27 -- Shadows (http://nehe.gamedev.net)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class NeHeLesson27 : Model {
		// --- Fields ---
		#region Private Fields
		private static bool sbp;														// Space Bar Pressed?

		private struct Point {															// Vertex In 3D-Coordinate System
			public float x, y, z;
		};

		private struct PlaneEq {														// Plane Equation, In The Format: ax + by + cz + d = 0
			public float a, b, c, d;
		};

		private struct Plane {															// Structure Describing An Object's Face
			public uint[] p;															// Index Of Each Vertex Within An Object That Makes Up The Triangle Of This Face
			public Point[] normals;														// Normals To Each Vertex
			public uint[] neigh;														// Index Of Each Face That Neighbors This One Within The Object
			public PlaneEq planeEq;														// Equation Of A Plane That Contains This Triangle
			public bool visible;														// Is The Face Visible By The Light?
		};

		private struct GlObject {														// Object Structure
			public uint nPlanes, nPoints;												// Number Of Planes And Points
			public Point[] points;														// The Points
			public Plane[] planes;														// The Planes
		};

		private static GlObject obj;													// Object
		private static float xrot = 0.0f, xspeed = 0.0f;								// X Rotation & X Speed
		private static float yrot = 0.0f, yspeed = 0.0f;								// Y Rotation & Y Speed

		private static float[] LightPos = { 0.0f,  5.0f, -4.0f, 1.0f};					// Light Position
		private static float[] LightAmb = { 0.2f,  0.2f,  0.2f, 1.0f};					// Ambient Light Values
		private static float[] LightDif = { 0.6f,  0.6f,  0.6f, 1.0f};					// Diffuse Light Values
		private static float[] LightSpc = {-0.2f, -0.2f, -0.2f, 1.0f};					// Specular Light Values

		private static float[] MatAmb = {0.4f, 0.4f, 0.4f, 1.0f};						// Material - Ambient Values
		private static float[] MatDif = {0.2f, 0.6f, 0.9f, 1.0f};						// Material - Diffuse Values
		private static float[] MatSpc = {0.0f, 0.0f, 0.0f, 1.0f};						// Material - Specular Values
		private static float[] MatShn = {0.0f};											// Material - Shininess

		private static float[] ObjPos = {-2.0f, -2.0f, -5.0f};							// Object Position

		private static GLUquadric q;													// Quadratic For Drawing A Sphere
		private static float[] SpherePos = {-4.0f, -5.0f, -6.0f};						// Sphere Position

		private static int currentobj = 0;												// The Current Object We're Displaying

		private static float[] Minv = new float[16];									// Matrix For Vector Manipulation
		private static float[] wlp = new float[4];										// Vector For World Local Coordinate
		private static float[] lp = new float[4];										// Vector For Light Position
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Lesson title.
		/// </summary>
		public override string Title {
			get {
				return "NeHe Lesson 27 -- Shadows";
			}
		}

		/// <summary>
		/// Lesson description.
		/// </summary>
		public override string Description {
			get {
				return "This advanced lesson demonstrates a method for producing shadows.  Make sure you are familiar with the concepts from the previous lessons, particularly the stencil buffer.  There's also quite a bit of 3D math in this one, so dust off your textbooks.";
			}
		}

		/// <summary>
		/// Lesson URL.
		/// </summary>
		public override string Url {
			get {
				return "http://nehe.gamedev.net/tutorials/lesson.asp?l=27";
			}
		}
		#endregion Public Properties

		// --- Entry Point ---
		#region Main()
		/// <summary>
		/// Application's entry point, runs this NeHe lesson.
		/// </summary>
		public static void Main() {														// Entry Point
			App.Run(new NeHeLesson27());												// Run Our NeHe Lesson As A Windows Forms Application
		}
		#endregion Main()

		// --- Basecode Methods ---
		#region Initialize()
		/// <summary>
		/// Overrides OpenGL's initialization.
		/// </summary>
		public override void Initialize() {
			base.Initialize();															// Run The Base Initialization
			glClearStencil(0);															// Stencil Buffer Setup

			InitObjects();																// Initialize Our Objects

			glLightfv(GL_LIGHT1, GL_POSITION, LightPos);								// Set Light1 Position
			glLightfv(GL_LIGHT1, GL_AMBIENT, LightAmb);									// Set Light1 Ambience
			glLightfv(GL_LIGHT1, GL_DIFFUSE, LightDif);									// Set Light1 Diffuse
			glLightfv(GL_LIGHT1, GL_SPECULAR, LightSpc);								// Set Light1 Specular
			glEnable(GL_LIGHT1);														// Enable Light1
			glEnable(GL_LIGHTING);														// Enable Lighting

			glMaterialfv(GL_FRONT, GL_AMBIENT, MatAmb);									// Set Material Ambience
			glMaterialfv(GL_FRONT, GL_DIFFUSE, MatDif);									// Set Material Diffuse
			glMaterialfv(GL_FRONT, GL_SPECULAR, MatSpc);								// Set Material Specular
			glMaterialfv(GL_FRONT, GL_SHININESS, MatShn);								// Set Material Shininess

			glCullFace(GL_BACK);														// Set Culling Face To Back Face
			glEnable(GL_CULL_FACE);														// Enable Culling
			glClearColor(0.1f, 1.0f, 0.5f, 1.0f);										// Set Clear Color (Greenish Color)

			q = gluNewQuadric();														// Initialize Quadratic
			gluQuadricNormals(q, GL_SMOOTH);											// Enable Smooth Normal Generation
			gluQuadricTexture(q, (byte) GL_FALSE);										// Disable Auto Texture Coords
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws NeHe Lesson 27 scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT | GL_STENCIL_BUFFER_BIT);	// Clear Color Buffer, Depth Buffer, Stencil Buffer
			glLoadIdentity();															// Reset Modelview Matrix

			glTranslatef(0.0f, 0.0f, -20.0f);											// Zoom Into Screen 20 Units
			glLightfv(GL_LIGHT1, GL_POSITION, LightPos);								// Position Light1
			glTranslatef(SpherePos[0], SpherePos[1], SpherePos[2]);						// Position The Sphere
			gluSphere(q, 1.5f, 32, 16);													// Draw A Sphere

			// Calculate Light's Position Relative To Local Coordinate System
			// We Build The Inversed Matrix By Doing All The Actions In Reverse Order
			// And With Reversed Parameters (Notice -xrot, -yrot, -ObjPos[], etc.)
			glLoadIdentity();															// Reset Matrix
			glRotatef(-yrot, 0.0f, 1.0f, 0.0f);											// Rotate By -yrot On Y Axis
			glRotatef(-xrot, 1.0f, 0.0f, 0.0f);											// Rotate By -xrot On X Axis
			glGetFloatv(GL_MODELVIEW_MATRIX, Minv);										// Retrieve ModelView Matrix (Stores In Minv)
			lp[0] = LightPos[0];														// Store Light Position X In lp[0]
			lp[1] = LightPos[1];														// Store Light Position Y In lp[1]
			lp[2] = LightPos[2];														// Store Light Position Z In lp[2]
			lp[3] = LightPos[3];														// Store Light Direction In lp[3]
			VMatMult(Minv, ref lp);														// We Store Rotated Light Vector In 'lp' Array
			glTranslatef(-ObjPos[0], -ObjPos[1], -ObjPos[2]);							// Move Negative On All Axis Based On ObjPos[] Values (X, Y, Z)
			glGetFloatv(GL_MODELVIEW_MATRIX, Minv);										// Retrieve ModelView Matrix From Minv
			wlp[0] = 0.0f;																// World Local Coord X To 0
			wlp[1] = 0.0f;																// World Local Coord Y To 0
			wlp[2] = 0.0f;																// World Local Coord Z To 0
			wlp[3] = 1.0f;
			VMatMult(Minv, ref wlp);													// We Store The Position Of The World Origin Relative To The
																						// Local Coord. System In 'wlp' Array
			lp[0] += wlp[0];															// Adding These Two Gives Us The
			lp[1] += wlp[1];															// Position Of The Light Relative To
			lp[2] += wlp[2];															// The Local Coordinate System

			glColor4f(0.7f, 0.4f, 0.0f, 1.0f);											// Set Color To An Orange
			glLoadIdentity();															// Reset Modelview Matrix
			glTranslatef(0.0f, 0.0f, -20.0f);											// Zoom Into The Screen 20 Units
			DrawRoom();																	// Draw The Room
			glTranslatef(ObjPos[0], ObjPos[1], ObjPos[2]);								// Position The Object
			glRotatef(xrot, 1.0f, 0.0f, 0.0f);											// Spin It On The X Axis By xrot
			glRotatef(yrot, 0.0f, 1.0f, 0.0f);											// Spin It On The Y Axis By yrot
			DrawObject(obj);															// Procedure For Drawing The Loaded Object
			CastShadow(obj, lp);														// Procedure For Casting The Shadow Based On The Silhouette

			glColor4f(0.7f, 0.4f, 0.0f, 1.0f);											// Set Color To Purplish Blue
			glDisable(GL_LIGHTING);														// Disable Lighting
			glDepthMask((byte) GL_FALSE);												// Disable Depth Mask
			glTranslatef(lp[0], lp[1], lp[2]);											// Translate To Light's Position
																						// Notice We're Still In Local Coordinate System
			gluSphere(q, 0.2f, 16, 8);													// Draw A Little Yellow Sphere (Represents Light)
			glEnable(GL_LIGHTING);														// Enable Lighting
			glDepthMask((byte) GL_TRUE);												// Enable Depth Mask

			xrot += xspeed;																// Increase xrot By xspeed
			yrot += yspeed;																// Increase yrot By yspeed

			glFlush();																	// Flush The OpenGL Pipeline
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

			dataRow = InputHelpDataTable.NewRow();										// NumPad 8 - Move Object Up
			dataRow["Input"] = "NumPad 8";
			dataRow["Effect"] = "Move Object Up";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// NumPad 5 - Move Object Down
			dataRow["Input"] = "NumPad 5";
			dataRow["Effect"] = "Move Object Down";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// NumPad 4 - Move Object Left
			dataRow["Input"] = "NumPad 4";
			dataRow["Effect"] = "Move Object Left";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// NumPad 6 - Move Object Right
			dataRow["Input"] = "NumPad 6";
			dataRow["Effect"] = "Move Object Right";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// NumPad 7 - Move Object Away
			dataRow["Input"] = "NumPad 7";
			dataRow["Effect"] = "Move Object Away";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// NumPad 9 - Move Object Closer
			dataRow["Input"] = "NumPad 9";
			dataRow["Effect"] = "Move Object Closer";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// I - Move Light Up
			dataRow["Input"] = "I";
			dataRow["Effect"] = "Move Light Up";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// K - Move Light Down
			dataRow["Input"] = "K";
			dataRow["Effect"] = "Move Light Down";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// J - Move Light Left
			dataRow["Input"] = "J";
			dataRow["Effect"] = "Move Light Left";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// L - Move Light Right
			dataRow["Input"] = "L";
			dataRow["Effect"] = "Move Light Right";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// U - Move Light Away
			dataRow["Input"] = "U";
			dataRow["Effect"] = "Move Light Away";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// O - Move Light Closer
			dataRow["Input"] = "O";
			dataRow["Effect"] = "Move Light Closer";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// W - Move Ball Up
			dataRow["Input"] = "W";
			dataRow["Effect"] = "Move Ball Up";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// S - Move Ball Down
			dataRow["Input"] = "S";
			dataRow["Effect"] = "Move Ball Down";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// A - Move Ball Left
			dataRow["Input"] = "A";
			dataRow["Effect"] = "Move Ball Left";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// D - Move Ball Right
			dataRow["Input"] = "D";
			dataRow["Effect"] = "Move Ball Right";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Q - Move Ball Away
			dataRow["Input"] = "Q";
			dataRow["Effect"] = "Move Ball Away";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// E - Move Ball Closer
			dataRow["Input"] = "E";
			dataRow["Effect"] = "Move Ball Closer";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Space Bar - Cycle Through Objects
			dataRow["Input"] = "Space Bar";
			dataRow["Effect"] = "Cycle Through Objects [0-2]";
			if(currentobj == 0) {
				dataRow["Current State"] = "0 (Jack)";
			}
			else if(currentobj == 1) {
				dataRow["Current State"] = "1 (Cross)";
			}
			else {
				dataRow["Current State"] = "2 (Cube)";
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

			if(KeyState[(int) Keys.Up]) {												// Is Up Arrow Key Being Pressed?
				xspeed -= 0.1f;															// Decrease xspeed
			}

			if(KeyState[(int) Keys.Down]) {												// Is Down Arrow Key Being Pressed?
				xspeed += 0.1f;															// Increase xspeed
			}

			if(KeyState[(int) Keys.Left]) {												// Is Left Arrow Key Being Pressed?
				yspeed -= 0.1f;															// Decrease yspeed
			}

			if(KeyState[(int) Keys.Right]) {											// Is Right Arrow Key Being Pressed?
				yspeed += 0.1f;															// Increase yspeed
			}

			if(KeyState[(int) Keys.NumPad8]) {											// Is NumPad 8 Key Being Pressed?
				ObjPos[1] += 0.05f;														// Move Object Up
			}

			if(KeyState[(int) Keys.NumPad5]) {											// Is NumPad 5 Key Being Pressed?
				ObjPos[1] -= 0.05f;														// Move Object Down
			}

			if(KeyState[(int) Keys.NumPad4]) {											// Is NumPad 4 Key Being Pressed?
				ObjPos[0] -= 0.05f;														// Move Object Left
			}

			if(KeyState[(int) Keys.NumPad6]) {											// Is NumPad 6 Key Being Pressed?
				ObjPos[0] += 0.05f;														// Move Object Right
			}

			if(KeyState[(int) Keys.NumPad7]) {											// Is NumPad 7 Key Being Pressed?
				ObjPos[2] -= 0.05f;														// Move Object Away From Viewer
			}

			if(KeyState[(int) Keys.NumPad9]) {											// Is NumPad 9 Key Being Pressed?
				ObjPos[2] += 0.05f;														// Move Object Toward Viewer
			}

			if(KeyState[(int) Keys.I]) {												// Is I Key Being Pressed?
				LightPos[1] += 0.05f;													// Moves Light Up
			}

			if(KeyState[(int) Keys.K]) {												// Is K Key Being Pressed?
				LightPos[1] -= 0.05f;													// Moves Light Down
			}

			if(KeyState[(int) Keys.J]) {												// Is J Key Being Pressed?
				LightPos[0] -= 0.05f;													// Moves Light Left
			}

			if(KeyState[(int) Keys.L]) {												// Is L Key Being Pressed?
				LightPos[0] += 0.05f;													// Moves Light Right
			}

			if(KeyState[(int) Keys.U]) {												// Is U Key Being Pressed?
				LightPos[2] -= 0.05f;													// Moves Light Away From Viewer
			}

			if(KeyState[(int) Keys.O]) {												// Is O Key Being Pressed?
				LightPos[2] += 0.05f;													// Moves Light Toward Viewer
			}

			if(KeyState[(int) Keys.W]) {												// Is W Key Being Pressed?
				SpherePos[1] += 0.05f;													// Move Ball Up
			}

			if(KeyState[(int) Keys.S]) {												// Is S Key Being Pressed?
				SpherePos[1] -= 0.05f;													// Move Ball Down
			}

			if(KeyState[(int) Keys.A]) {												// Is A Key Being Pressed?
				SpherePos[0] -= 0.05f;													// Move Ball Left
			}

			if(KeyState[(int) Keys.D]) {												// Is D Key Being Pressed?
				SpherePos[0] += 0.05f;													// Move Ball Right
			}

			if(KeyState[(int) Keys.Q]) {												// Is Q Key Being Pressed?
				SpherePos[2] -= 0.05f;													// Move Ball Away From Viewer
			}

			if(KeyState[(int) Keys.E]) {												// Is E Key Being Pressed?
				SpherePos[2] += 0.05f;													// Move Ball Toward Viewer
			}

			if(KeyState[(int) Keys.Space] && !sbp) {									// Is Space Bar Being Pressed?
				sbp = true;																// sbp Becomes true
				currentobj++;															// Cycle Through Objects
				if(currentobj > 2) {													// Is Value Greater Than 2?
					currentobj = 0;														// If So, Set currentobj To 0
				}
				InitObjects();															// Initialize The Objects
				UpdateInputHelp();														// Update The Input Help Screen
			}
			if(!KeyState[(int) Keys.Space]) {											// Has Space Bar Been Released?
				sbp = false;															// If So, sbp Becomes false
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
			// Modified The Near Clipping Plane For ATI Cards, Which Have A More Accurate Stencil Buffer
			App.NearClippingPlane = 0.001f;												// Change The Near Clipping Plane
		}
		#endregion Setup()

		// --- Lesson Methods ---
		#region CalcPlane(GlObject o, ref Plane plane)
		/// <summary>
		///  Computes a plane equation given 3 points.
		/// </summary>
		/// <param name="o">The GlObject.</param>
		/// <param name="plane">The plane to store the calculations into.</param>
		private void CalcPlane(GlObject o, ref Plane plane) {
			Point[] v = new Point[4];
			int i;

			for(i = 0; i < 3; i++) {
				v[i + 1].x = o.points[plane.p[i]].x;
				v[i + 1].y = o.points[plane.p[i]].y;
				v[i + 1].z = o.points[plane.p[i]].z;
			}

			plane.planeEq.a = v[1].y * (v[2].z - v[3].z) + v[2].y * (v[3].z - v[1].z) + v[3].y * (v[1].z - v[2].z);
			plane.planeEq.b = v[1].z * (v[2].x - v[3].x) + v[2].z * (v[3].x - v[1].x) + v[3].z * (v[1].x - v[2].x);
			plane.planeEq.c = v[1].x * (v[2].y - v[3].y) + v[2].x * (v[3].y - v[1].y) + v[3].x * (v[1].y - v[2].y);
			plane.planeEq.d = -(v[1].x * (v[2].y * v[3].z - v[3].y * v[2].z) + v[2].x * (v[3].y * v[1].z - v[1].y * v[3].z) + v[3].x * (v[1].y * v[2].z - v[2].y * v[1].z));
		}
		#endregion CalcPlane(GlObject o, ref Plane plane)

		#region CastShadow(GlObject o, float[] lp)
		/// <summary>
		/// Draws the shadows.
		/// </summary>
		/// <param name="o">The GlObject.</param>
		/// <param name="lp">The light position.</param>
		private void CastShadow(GlObject o, float[] lp) {
			uint i, j, k, jj;
			uint p1, p2;
			Point v1, v2;
			float side;

			// Set Visual Parameter
			for(i = 0; i < o.nPlanes; i++) {
				// Check To See If Light Is In Front Or Behind The Plane (Face Plane)
				side = o.planes[i].planeEq.a * lp[0] + o.planes[i].planeEq.b * lp[1] + o.planes[i].planeEq.c * lp[2] + o.planes[i].planeEq.d * lp[3];
				if(side > 0) {
					o.planes[i].visible = true;
				}
				else {
					o.planes[i].visible = false;
				}
			}

			glDisable(GL_LIGHTING);
			glDepthMask((byte) GL_FALSE);
			glDepthFunc(GL_LEQUAL);

			glEnable(GL_STENCIL_TEST);
			glColorMask(0, 0, 0, 0);
			glStencilFunc(GL_ALWAYS, 1, 0xffffffff);

			// First Pass, Stencil Operation Decreases Stencil Value
			glFrontFace(GL_CCW);
			glStencilOp(GL_KEEP, GL_KEEP, GL_INCR);
			for(i = 0; i < o.nPlanes; i++) {
				if(o.planes[i].visible) {
					for(j = 0; j < 3; j++) {
						k = o.planes[i].neigh[j];
						if((k <= 0) || (!o.planes[k-1].visible)) {
							// Here We Have An Edge, We Must Draw A Polygon
							p1 = o.planes[i].p[j];
							jj = (j + 1) % 3;
							p2 = o.planes[i].p[jj];

							// Calculate The Length Of The Vector
							v1.x = (o.points[p1].x - lp[0]) * 100;
							v1.y = (o.points[p1].y - lp[1]) * 100;
							v1.z = (o.points[p1].z - lp[2]) * 100;

							v2.x = (o.points[p2].x - lp[0]) * 100;
							v2.y = (o.points[p2].y - lp[1]) * 100;
							v2.z = (o.points[p2].z - lp[2]) * 100;

							// Draw The Polygon
							glBegin(GL_TRIANGLE_STRIP);
								glVertex3f(o.points[p1].x, o.points[p1].y, o.points[p1].z);
								glVertex3f(o.points[p1].x + v1.x, o.points[p1].y + v1.y, o.points[p1].z + v1.z);
								glVertex3f(o.points[p2].x, o.points[p2].y, o.points[p2].z);
								glVertex3f(o.points[p2].x + v2.x, o.points[p2].y + v2.y, o.points[p2].z + v2.z);
							glEnd();
						}
					}
				}
			}

			// Second Pass, Stencil Operation Increases Stencil Value
			glFrontFace(GL_CW);
			glStencilOp(GL_KEEP, GL_KEEP, GL_DECR);
			for(i = 0; i < o.nPlanes; i++) {
				if(o.planes[i].visible) {
					for(j = 0; j < 3; j++) {
						k = o.planes[i].neigh[j];
						if((k <= 0) || (!o.planes[k-1].visible)) {
							// Here We Have An Edge, We Must Draw A Polygon
							p1 = o.planes[i].p[j];
							jj = (j + 1) % 3;
							p2 = o.planes[i].p[jj];

							// Calculate The Length Of The Vector
							v1.x = (o.points[p1].x - lp[0]) * 100;
							v1.y = (o.points[p1].y - lp[1]) * 100;
							v1.z = (o.points[p1].z - lp[2]) * 100;

							v2.x = (o.points[p2].x - lp[0]) * 100;
							v2.y = (o.points[p2].y - lp[1]) * 100;
							v2.z = (o.points[p2].z - lp[2]) * 100;

							// Draw The Polygon
							glBegin(GL_TRIANGLE_STRIP);
								glVertex3f(o.points[p1].x, o.points[p1].y, o.points[p1].z);
								glVertex3f(o.points[p1].x + v1.x, o.points[p1].y + v1.y, o.points[p1].z + v1.z);
								glVertex3f(o.points[p2].x, o.points[p2].y, o.points[p2].z);
								glVertex3f(o.points[p2].x + v2.x, o.points[p2].y + v2.y, o.points[p2].z + v2.z);
							glEnd();
						}
					}
				}
			}

			glFrontFace(GL_CCW);
			glColorMask(1, 1, 1, 1);

			// Draw A Shadowing Rectangle Covering The Entire Screen
			glColor4f(0.0f, 0.0f, 0.0f, 0.4f);
			glEnable(GL_BLEND);
			glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
			glStencilFunc(GL_NOTEQUAL, 0, 0xffffffff);
			glStencilOp(GL_KEEP, GL_KEEP, GL_KEEP);
			glPushMatrix();
			glLoadIdentity();
			glBegin(GL_TRIANGLE_STRIP);
				glVertex3f(-0.1f,  0.1f, -0.10f);
				glVertex3f(-0.1f, -0.1f, -0.10f);
				glVertex3f( 0.1f,  0.1f, -0.10f);
				glVertex3f( 0.1f, -0.1f, -0.10f);
			glEnd();
			glPopMatrix();
			glDisable(GL_BLEND);

			glDepthFunc(GL_LEQUAL);
			glDepthMask((byte) GL_TRUE);
			glEnable(GL_LIGHTING);
			glDisable(GL_STENCIL_TEST);
			glShadeModel(GL_SMOOTH);
		}
		#endregion CastShadow(GlObject o, float[] lp)

		#region DrawObject(GlObject o)
		/// <summary>
		/// Draws the object.
		/// </summary>
		/// <param name="o">The GlObject to draw.</param>
		private void DrawObject(GlObject o) {
			uint i, j;

			glBegin(GL_TRIANGLES);
			for(i = 0; i < o.nPlanes; i++) {
				for(j = 0; j < 3; j++) {
					glNormal3f(o.planes[i].normals[j].x, o.planes[i].normals[j].y, o.planes[i].normals[j].z);
					glVertex3f(o.points[o.planes[i].p[j]].x, o.points[o.planes[i].p[j]].y, o.points[o.planes[i].p[j]].z);
				}
			}
			glEnd();
		}
		#endregion DrawObject(GlObject o)

		#region DrawRoom()
		/// <summary>
		/// Draws the room.
		/// </summary>
		private void DrawRoom() {														// Draw The Room (Box)
			glBegin(GL_QUADS);															// Begin Drawing Quads
				// Floor
				glNormal3f(0.0f, 1.0f, 0.0f);											// Normal Pointing Up
				glVertex3f(-10.0f, -10.0f, -20.0f);										// Back Left
				glVertex3f(-10.0f, -10.0f,  20.0f);										// Front Left
				glVertex3f( 10.0f, -10.0f,  20.0f);										// Front Right
				glVertex3f( 10.0f, -10.0f, -20.0f);										// Back Right
				// Ceiling
				glNormal3f(0.0f, -1.0f, 0.0f);											// Normal Point Down
				glVertex3f(-10.0f, 10.0f,  20.0f);										// Front Left
				glVertex3f(-10.0f, 10.0f, -20.0f);										// Back Left
				glVertex3f( 10.0f, 10.0f, -20.0f);										// Back Right
				glVertex3f( 10.0f, 10.0f,  20.0f);										// Front Right
				// Front Wall
				glNormal3f(0.0f, 0.0f, 1.0f);											// Normal Pointing Away From Viewer
				glVertex3f(-10.0f,  10.0f, -20.0f);										// Top Left
				glVertex3f(-10.0f, -10.0f, -20.0f);										// Bottom Left
				glVertex3f( 10.0f, -10.0f, -20.0f);										// Bottom Right
				glVertex3f( 10.0f,  10.0f, -20.0f);										// Top Right
				// Back Wall
				glNormal3f(0.0f, 0.0f, -1.0f);											// Normal Pointing Towards Viewer
				glVertex3f( 10.0f,  10.0f, 20.0f);										// Top Right
				glVertex3f( 10.0f, -10.0f, 20.0f);										// Bottom Right
				glVertex3f(-10.0f, -10.0f, 20.0f);										// Bottom Left
				glVertex3f(-10.0f,  10.0f, 20.0f);										// Top Left
				// Left Wall
				glNormal3f(1.0f, 0.0f, 0.0f);											// Normal Pointing Right
				glVertex3f(-10.0f,  10.0f,  20.0f);										// Top Front
				glVertex3f(-10.0f, -10.0f,  20.0f);										// Bottom Front
				glVertex3f(-10.0f, -10.0f, -20.0f);										// Bottom Back
				glVertex3f(-10.0f,  10.0f, -20.0f);										// Top Back
				// Right Wall
				glNormal3f(-1.0f, 0.0f, 0.0f);											// Normal Pointing Left
				glVertex3f( 10.0f,  10.0f, -20.0f);										// Top Back
				glVertex3f( 10.0f, -10.0f, -20.0f);										// Bottom Back
				glVertex3f( 10.0f, -10.0f,  20.0f);										// Bottom Front
				glVertex3f( 10.0f,  10.0f,  20.0f);										// Top Front
			glEnd();																	// Done Drawing Quads
		}
		#endregion DrawRoom()

		#region InitObjects()
		/// <summary>
		/// Initializes the objects.
		/// </summary>
		private void InitObjects() {													// Initialize Objects
			if(currentobj == 0) {														// If Object0
				ReadObject(@"..\..\data\NeHeLesson27\Object0CsGL.txt", ref obj);		// Read Object0 Into obj
			}
			else if(currentobj == 1) {													// If Object1
				ReadObject(@"..\..\data\NeHeLesson27\Object1CsGL.txt", ref obj);		// Read Object1 Into obj
			}
			else if(currentobj == 2) {													// If Object2
				ReadObject(@"..\..\data\NeHeLesson27\Object2CsGL.txt", ref obj);		// Read Object2 Into obj
			}

			SetConnectivity(ref obj);													// Set Face-To-Face Connectivity

			for(uint i = 0; i < obj.nPlanes; i++) {										// Loop Through All Object Planes
				CalcPlane(obj, ref obj.planes[i]);										// Compute Plane Equations For All Faces
			}
		}
		#endregion InitObjects()

		#region ReadObject(string filename, ref GlObject o)
		/// <summary>
		/// Loads an object from a file.
		/// </summary>
		/// <param name="filename">The file to load.</param>
		/// <param name="o">The GlObject to save the object to.</param>
		private void ReadObject(string filename, ref GlObject o) {						// Loads An Object From File (filename)
			// This Method Is A Little Messy.  The .Net Framework Doesn't Have An Analogous Implementation 
			// Of C/C++'s fscanf().  As Such You Have To Manually Parse A File, You Can Either Do So 
			// Procedurally Like I'm Doing Here, Or Use Some RegEx's.  To Make It A Bit Easier I Modified
			// The Text Files To Remove Comments, Empty Lines And Excess Spaces.  Sorry For The
			// Ugliness.  Let Me Know If You Have Redone This To Be Cleaner Or Find A Nice sscanf().

			int i;																		// Generic Loop Variable
			string oneline = "";														// The Line We've Read
			string[] splitter;															// Array For Split Values
			StreamReader reader = null;													// Our StreamReader
			ASCIIEncoding encoding = new ASCIIEncoding();								// ASCII Encoding

			try {
				reader = new StreamReader(filename, encoding);							// Open The File As ASCII Text

				oneline = reader.ReadLine();											// Read The First Line
				o.nPoints = UInt32.Parse(oneline);										// Save The Object's Number Of Points
				o.points = new Point[100];												// Create An Array To Hold Up To 100 Points

				for(i = 1; i <= o.nPoints; i++) {										// For Each Point, Specified By The Number Of Points
					oneline = reader.ReadLine();										// Read A Line
					splitter = oneline.Split();											// Split The Line On Spaces
					o.points[i].x = Single.Parse(splitter[0]);							// Save The x Coordinate As A Float
					o.points[i].y = Single.Parse(splitter[1]);							// Save The y Coordinate As A Float
					o.points[i].z = Single.Parse(splitter[2]);							// Save The z Coordinate As A Float
				}

				oneline = reader.ReadLine();											// Read The Next Line
				o.nPlanes = UInt32.Parse(oneline);										// Save The Object's Number Of Planes
				o.planes = new Plane[200];												// Create An Array To Hold Up To 200 Planes

				for(i = 0; i < o.nPlanes; i++) {										// For Each Plane, Specified By The Number Of Planes
					o.planes[i].p = new uint[3];										// Initialize The p (Point Index) Array Of The Plane
					o.planes[i].normals = new Point[3];									// Initialize The normals Array
					o.planes[i].neigh = new uint[3];									// Initialize The neigh Array

					oneline = reader.ReadLine();										// Read A Line
					splitter = oneline.Split();											// Split The Line On Spaces
					o.planes[i].p[0] = UInt32.Parse(splitter[0]);						// Parse In The Point Index
					o.planes[i].p[1] = UInt32.Parse(splitter[1]);
					o.planes[i].p[2] = UInt32.Parse(splitter[2]);

					o.planes[i].normals[0].x = Single.Parse(splitter[3]);				// Parse In The Normals
					o.planes[i].normals[0].y = Single.Parse(splitter[4]);
					o.planes[i].normals[0].z = Single.Parse(splitter[5]);
					o.planes[i].normals[1].x = Single.Parse(splitter[6]);
					o.planes[i].normals[1].y = Single.Parse(splitter[7]);
					o.planes[i].normals[1].z = Single.Parse(splitter[8]);
					o.planes[i].normals[2].x = Single.Parse(splitter[9]);
					o.planes[i].normals[2].y = Single.Parse(splitter[10]);
					o.planes[i].normals[2].z = Single.Parse(splitter[11]);
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
		#endregion ReadObject(string filename, ref GlObject o)

		#region SetConnectivity(ref GlObject o)
		/// <summary>
		/// Connectivity procedure, based on Gamasutra's article, hard to explain here.
		/// </summary>
		/// <param name="o">The GlObject to save to.</param>
		private void SetConnectivity(ref GlObject o){
			uint p1i, p2i, p1j, p2j;
			uint P1i, P2i, P1j, P2j;
			uint i, j, ki, kj;

			for(i = 0; i < (o.nPlanes - 1); i++) {
				for(j = (i + 1); j < o.nPlanes; j++) {
					for(ki = 0; ki < 3; ki++) {
						if(o.planes[i].neigh[ki] <= 0) {
							for(kj = 0; kj < 3; kj++){
								p1i = ki;
								p1j = kj;
								p2i = (ki + 1) % 3;
								p2j = (kj + 1) % 3;

								p1i = o.planes[i].p[p1i];
								p2i = o.planes[i].p[p2i];
								p1j = o.planes[j].p[p1j];
								p2j = o.planes[j].p[p2j];

								P1i = ((p1i + p2i) - (uint) Math.Abs(p1i - p2i)) / 2;
								P2i = ((p1i + p2i) + (uint) Math.Abs(p1i - p2i)) / 2;
								P1j = ((p1j + p2j) - (uint) Math.Abs(p1j - p2j)) / 2;
								P2j = ((p1j + p2j) + (uint) Math.Abs(p1j - p2j)) / 2;

								if((P1i == P1j) && (P2i == P2j)) {						// They Are Neighbours
									o.planes[i].neigh[ki] = j + 1;	  
									o.planes[j].neigh[kj] = i + 1;	  
								}
							}
						}
					}
				}
			}
		}
		#endregion SetConnectivity(ref GlObject o)

		#region VMatMult(float[] M, ref float[] v)
		/// <summary>
		/// Multiplies a vector by a matrix.
		/// </summary>
		/// <param name="M">The matrix.</param>
		/// <param name="v">The vector.</param>
		private void VMatMult(float[] M, ref float[] v) {
			float[] res = new float[4];													// Hold Calculated Results

			res[0] = M[0] * v[0] + M[4] * v[1] + M[8] * v[2] + M[12] * v [3];			// Multiply A Vector By A Matrix
			res[1] = M[1] * v[0] + M[5] * v[1] + M[9] * v[2] + M[13] * v[3];
			res[2] = M[2] * v[0] + M[6] * v[1] + M[10] * v[2] + M[14] * v[3];
			res[3] = M[3] * v[0] + M[7] * v[1] + M[11] * v[2] + M[15] * v[3];

			v[0] = res[0];																// Results Are Stored Back In v[]
			v[1] = res[1];
			v[2] = res[2];
			v[3] = res[3];																// Homogenous Coordinate
		}
		#endregion VMatMult(float[] M, ref float[] v)
	}
}