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
/*		This Code Was Created by Jens Schneider (WizardSoft) 2000 
 *		Lesson22 to the series of OpenGL tutorials by NeHe-Production
 *
 *		This Code is loosely based upon Lesson06 by Jeff Molofee.
 *
 *		contact me at: schneide@pool.informatik.rwth-aachen.de
 *
 *		Basecode Was Created By Jeff Molofee 2000
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
[assembly: AssemblyDescription("NeHe Lesson 22")]
[assembly: AssemblyProduct("NeHe Lesson 22")]
[assembly: AssemblyTitle("NeHe Lesson 22")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace NeHeLessons {
	/// <summary>
	/// NeHe Lesson 22 -- Bump-Mapping, Multi-Texturing & Extensions (http://nehe.gamedev.net)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class NeHeLesson22 : Model {
		// --- Fields ---
		#region Private Fields
		private static float MAX_EMBOSS = 0.008f;										// Maximum Emboss-Translate. Increase To Get Higher Immersion
		private static bool ARB_ENABLE = true;											// Used To Disable ARB Extensions Entirely If false
		private static bool EXT_INFO = false;											// Set To true To See Your Extensions At Start-Up?

		private static bool multitextureSupported = false;								// Flag Indicating Whether Multitexturing Is Supported
		private static bool useMultitexture = true;										// Use It If It Is Supported?
		private static int maxTexelUnits = 1;											// Number Of Texel-Pipelines. This Is At Least 1.

		private static bool emboss = false;												// Emboss Only, No Basetexture?
		private static bool bumps = true;												// Do Bumpmapping?

		private static float xrot;														// X Rotation
		private static float yrot;														// Y Rotation
		private static float xspeed;													// X Rotation Speed
		private static float yspeed;													// Y Rotation Speed
		private static float z = -5.0f;													// Depth Into The Screen

		private static uint filter = 1;													// Which Filter To Use
		private static uint[] texture = new uint[3];									// Storage For 3 Textures
		private static uint[] bump = new uint[3];										// Our Bumpmappings
		private static uint[] invbump = new uint[3];									// Inverted Bumpmaps
		private static uint[] glLogo = new uint[1];										// Handle For OpenGL-Logo
		private static uint[] multiLogo = new uint[1];									// Handle For Multitexture-Enabled-Logo

		private static float[] LightAmbient = {0.2f, 0.2f, 0.2f};						// Ambient Light Is 20% White
		private static float[] LightDiffuse = {1.0f, 1.0f, 1.0f};						// Diffuse Light Is White
		private static float[] LightPosition = {0.0f, 0.0f, 2.0f};						// Position Is Somewhat In Front Of Screen
		private static float[] Gray = {0.5f, 0.5f, 0.5f, 1.0f};							// Gray :P

		// data Contains The Faces Of The Cube In Format 2xTexCoord, 3xVertex.
		// Note That The Tesselation Of The Cube Is Only Absolute Minimum.
		private static float[] data = {
			// FRONT FACE
			0.0f, 0.0f, -1.0f, -1.0f, +1.0f,
			1.0f, 0.0f, +1.0f, -1.0f, +1.0f,
			1.0f, 1.0f, +1.0f, +1.0f, +1.0f,
			0.0f, 1.0f, -1.0f, +1.0f, +1.0f,
			// BACK FACE
			1.0f, 0.0f, -1.0f, -1.0f, -1.0f,
			1.0f, 1.0f, -1.0f, +1.0f, -1.0f,
			0.0f, 1.0f, +1.0f, +1.0f, -1.0f,
			0.0f, 0.0f, +1.0f, -1.0f, -1.0f,
			// Top Face
			0.0f, 1.0f, -1.0f, +1.0f, -1.0f,
			0.0f, 0.0f, -1.0f, +1.0f, +1.0f,
			1.0f, 0.0f, +1.0f, +1.0f, +1.0f,
			1.0f, 1.0f, +1.0f, +1.0f, -1.0f,
			// Bottom Face
			1.0f, 1.0f, -1.0f, -1.0f, -1.0f,
			0.0f, 1.0f, +1.0f, -1.0f, -1.0f,
			0.0f, 0.0f, +1.0f, -1.0f, +1.0f,
			1.0f, 0.0f, -1.0f, -1.0f, +1.0f,
			// Right Face
			1.0f, 0.0f, +1.0f, -1.0f, -1.0f,
			1.0f, 1.0f, +1.0f, +1.0f, -1.0f,
			0.0f, 1.0f, +1.0f, +1.0f, +1.0f,
			0.0f, 0.0f, +1.0f, -1.0f, +1.0f,
			// Left Face
			0.0f, 0.0f, -1.0f, -1.0f, -1.0f,
			1.0f, 0.0f, -1.0f, -1.0f, +1.0f,
			1.0f, 1.0f, -1.0f, +1.0f, +1.0f,
			0.0f, 1.0f, -1.0f, +1.0f, -1.0f
		};
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Lesson title.
		/// </summary>
		public override string Title {
			get {
				return "NeHe Lesson 22 -- Bump-Mapping, Multi-Texturing & Extensions";
			}
		}

		/// <summary>
		/// Lesson description.
		/// </summary>
		public override string Description {
			get {
				return "In this lesson you will modify the code from lesson 06 to support hardware multi-texturing on cards that support it, learn about OpenGL's extensions, and bump-mapping.  This is an advanced lesson.  Make sure you've understood the preceding lessons.";
			}
		}

		/// <summary>
		/// Lesson URL.
		/// </summary>
		public override string Url {
			get {
				return "http://nehe.gamedev.net/tutorials/lesson.asp?l=22";
			}
		}
		#endregion Public Properties

		// --- Entry Point ---
		#region Main()
		/// <summary>
		/// Application's entry point, runs this NeHe lesson.
		/// </summary>
		public static void Main() {														// Entry Point
			App.Run(new NeHeLesson22());												// Run Our NeHe Lesson As A Windows Forms Application
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

			multitextureSupported = InitMultitexture();									// Initialize MultiTexturing
			LoadTextures();																// Jump To Texture Loading Routine
			InitLights();																// Initialize OpenGL Light
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws NeHe Lesson 22 scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			if(bumps) {
				if(useMultitexture && maxTexelUnits > 1) {
					DoMesh2TexelUnits();
				}
				else {
					DoMesh1TexelUnits();
				}
			}
			else {
				DoMeshNoBumps();
			}
		}
		#endregion Draw()

		#region InputHelp()
		/// <summary>
		/// Overrides The Default Input Help, Supplying Model-Specific Input Help.
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

			dataRow = InputHelpDataTable.NewRow();										// Page Up - Decrease Z Distance
			dataRow["Input"] = "Page Up";
			dataRow["Effect"] = "Decrease Z Distance";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Page Down - Increase Z Distance
			dataRow["Input"] = "Page Down";
			dataRow["Effect"] = "Increase Z Distance";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// E - Toggle Embossing
			dataRow["Input"] = "E";
			dataRow["Effect"] = "Toggle Embossing";
			if(emboss) {
				dataRow["Current State"] = "On";
			}
			else {
				dataRow["Current State"] = "Off";
			}
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// M - Toggle Multitexturing
			dataRow["Input"] = "M";
			dataRow["Effect"] = "Toggle Multitexturing";
			if(useMultitexture && multitextureSupported) {
				dataRow["Current State"] = "On";
			}
			else {
				dataRow["Current State"] = "Off";
			}
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// B - Toggle Bumpmapping
			dataRow["Input"] = "B";
			dataRow["Effect"] = "Toggle Bumpmapping";
			if(bumps) {
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
		}
		#endregion InputHelp()

		#region ProcessInput()
		/// <summary>
		/// Lesson-Specific Input Handling
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

			if(KeyState[(int) Keys.PageUp]) {											// Is PageUp Key Being Pressed?
				z -= 0.02f;																// Decrease Z Distance
			}

			if(KeyState[(int) Keys.PageDown]) {											// Is PageDown Key Being Pressed?
				z += 0.02f;																// Increase Z Distance
			}

			if(KeyState[(int) Keys.E]) {												// Is E Key Being Pressed?
				emboss = !emboss;														// Toggle Embossing
				UpdateInputHelp();														// Update The Help Screen
				KeyState[(int) Keys.E] = false;											// Mark As Handled
			}

			if(KeyState[(int) Keys.M]) {												// Is M Key Being Pressed?
				useMultitexture = ((!useMultitexture) && multitextureSupported);		// Toggle Multitexture
				UpdateInputHelp();														// Update The Help Screen
				KeyState[(int) Keys.M] = false;											// Mark As Handled
			}

			if(KeyState[(int) Keys.B]) {												// Is B Key Being Pressed?
				z += 0.02f;																// Increase Z Distance
				bumps = !bumps;															// Toggle Bumpmapping
				UpdateInputHelp();														// Update The Help Screen
				KeyState[(int) Keys.B] = false;											// Mark As Handled
			}

			if(KeyState[(int) Keys.F]) {												// Is F Key Being Pressed?
				filter++;																// Cycle Filters
				filter %= 3;
				UpdateInputHelp();														// Update The Help Screen
				KeyState[(int) Keys.F] = false;											// Mark As Handled
			}
		}
		#endregion ProcessInput()

		// --- Lesson Methods ---
		#region DoCube()
		/// <summary>
		/// Drawa the cube.
		/// </summary>
		private void DoCube() {
			int i;
			glBegin(GL_QUADS);															// Begin Drawing Quads
			// Front Face
			glNormal3f(0.0f, 0.0f, +1.0f);
			for (i = 0; i < 4; i++) {
				glTexCoord2f(data[5 * i], data[5 * i + 1]);
				glVertex3f(data[5 * i + 2], data[5 * i + 3], data[5 * i + 4]);
			}
			// Back Face
			glNormal3f(0.0f, 0.0f, -1.0f);
			for (i = 4; i < 8; i++) {
				glTexCoord2f(data[5 * i], data[5 * i + 1]);
				glVertex3f(data[5 * i + 2], data[5 * i + 3], data[5 * i + 4]);
			}
			// Top Face
			glNormal3f(0.0f, 1.0f, 0.0f);
			for (i = 8; i < 12; i++) {
				glTexCoord2f(data[5 * i], data[5 * i + 1]);
				glVertex3f(data[5 * i + 2], data[5 * i + 3], data[5 * i + 4]);
			}
			// Bottom Face
			glNormal3f(0.0f,-1.0f, 0.0f);
			for (i = 12; i < 16; i++) {
				glTexCoord2f(data[5 * i], data[5 * i + 1]);
				glVertex3f(data[5 * i + 2], data[5 * i + 3], data[5 * i + 4]);
			}
			// Right Face
			glNormal3f(1.0f, 0.0f, 0.0f);
			for (i = 16; i < 20; i++) {
				glTexCoord2f(data[5 * i], data[5 * i + 1]);
				glVertex3f(data[5 * i + 2], data[5 * i + 3], data[5 * i + 4]);
			}
			// Left Face
			glNormal3f(-1.0f, 0.0f, 0.0f);
			for (i = 20; i < 24; i++) {
				glTexCoord2f(data[5 * i], data[5 * i + 1]);
				glVertex3f(data[5 * i + 2], data[5 * i + 3], data[5 * i + 4]);
			}
			glEnd();
		}
		#endregion DoCube()

		#region DoLogo()
		/// <summary>
		/// Billboards the two logos.  Must call this last!
		/// </summary>
		private void DoLogo() {
			glDepthFunc(GL_ALWAYS);
			glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
			glEnable(GL_BLEND);
			glDisable(GL_LIGHTING);
			glLoadIdentity();
			glBindTexture(GL_TEXTURE_2D, glLogo[0]);
			glBegin(GL_QUADS);
				glTexCoord2f(0.0f, 0.0f);	glVertex3f(0.23f, -0.4f,  -1.0f);
				glTexCoord2f(1.0f, 0.0f);	glVertex3f(0.53f, -0.4f,  -1.0f);
				glTexCoord2f(1.0f, 1.0f);	glVertex3f(0.53f, -0.25f, -1.0f);
				glTexCoord2f(0.0f, 1.0f);	glVertex3f(0.23f, -0.25f, -1.0f);
			glEnd();

			if (useMultitexture) {
				glBindTexture(GL_TEXTURE_2D, multiLogo[0]);
				glBegin(GL_QUADS);
					glTexCoord2f(0.0f, 0.0f);	glVertex3f(-0.53f, -0.25f, -1.0f);
					glTexCoord2f(1.0f, 0.0f);	glVertex3f(-0.33f, -0.25f, -1.0f);
					glTexCoord2f(1.0f, 1.0f);	glVertex3f(-0.33f, -0.15f, -1.0f);
					glTexCoord2f(0.0f, 1.0f);	glVertex3f(-0.53f, -0.15f, -1.0f);
				glEnd();
			}
		}
		#endregion DoLogo()

		#region DoMesh1TexelUnits()
		/// <summary>
		/// Does bumpmapping without multi-texturing using three passes.
		/// </summary>
		private void DoMesh1TexelUnits() {
			float[] c = {0.0f, 0.0f, 0.0f, 1.0f};										// Holds Current Vertex
			float[] n = {0.0f, 0.0f, 0.0f, 1.0f};										// Normalized Normal Of Current Surface
			float[] s = {0.0f, 0.0f, 0.0f, 1.0f};										// s-Texture Coordinate Direction, Normalized
			float[] t = {0.0f, 0.0f, 0.0f, 1.0f};										// t-Texture Coordinate Direction, Normalized
			float[] l = new float[4];													// Holds Our Lightposition To Be Transformed Into Object Space
			float[] Minv = new float[16];												// Holds The Inverted Modelview Matrix To Do So.
			int i;

			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);							// Clear The Screen And The Depth Buffer

			// Build Inverse Modelview Matrix First. This Substitutes One Push/Pop With One glLoadIdentity();
			// Simply Build It By Doing All Transformations Negated And In Reverse Order.
			glLoadIdentity();
			glRotatef(-yrot, 0.0f, 1.0f, 0.0f);
			glRotatef(-xrot, 1.0f, 0.0f, 0.0f);
			glTranslatef(0.0f, 0.0f, -z);
			glGetFloatv(GL_MODELVIEW_MATRIX, Minv);
			glLoadIdentity();
			glTranslatef(0.0f, 0.0f, z);

			glRotatef(xrot, 1.0f, 0.0f, 0.0f);
			glRotatef(yrot, 0.0f, 1.0f, 0.0f);	

			// Transform The Lightposition Into Object Coordinates:
			l[0] = LightPosition[0];
			l[1] = LightPosition[1];
			l[2] = LightPosition[2];
			l[3] = 1.0f;																// Homogenous Coordinate
			VMatMult(Minv, ref l);

			// PASS #1: Use Texture "Bump", No Blend, No Lighting, No Offset Texture-Coordinates
			glBindTexture(GL_TEXTURE_2D, bump[filter]);
			glDisable(GL_BLEND);
			glDisable(GL_LIGHTING);
			DoCube();

			// PASS #2: Use Texture "Invbump", Blend GL_ONE To GL_ONE, No Lighting, Offset Texture Coordinates
			glBindTexture(GL_TEXTURE_2D, invbump[filter]);
			glBlendFunc(GL_ONE, GL_ONE);
			glDepthFunc(GL_LEQUAL);
			glEnable(GL_BLEND);

			glBegin(GL_QUADS);
			// Front Face
			n[0] = 0.0f;
			n[1] = 0.0f;
			n[2] = 1.0f;
			s[0] = 1.0f;
			s[1] = 0.0f;
			s[2] = 0.0f;
			t[0] = 0.0f;
			t[1] = 1.0f;
			t[2] = 0.0f;
			for(i = 0; i < 4; i++) {
				c[0] = data[5 * i + 2];
				c[1] = data[5 * i + 3];
				c[2] = data[5 * i + 4];
				SetUpBumps(n, ref c, l, s, t);
				glTexCoord2f(data[5 * i] + c[0], data[5 * i + 1] + c[1]);
				glVertex3f(data[5 * i + 2], data[5 * i + 3], data[5 * i + 4]);
			}

			// Back Face
			n[0] =  0.0f;
			n[1] = 0.0f;
			n[2] = -1.0f;
			s[0] = -1.0f;
			s[1] = 0.0f;
			s[2] =  0.0f;
			t[0] =  0.0f;
			t[1] = 1.0f;
			t[2] =  0.0f;
			for(i = 4; i < 8; i++) {
				c[0] = data[5 * i + 2];
				c[1] = data[5 * i + 3];
				c[2] = data[5 * i + 4];
				SetUpBumps(n, ref c, l, s, t);
				glTexCoord2f(data[5 * i] + c[0], data[5 * i + 1] + c[1]);
				glVertex3f(data[5 * i + 2], data[5 * i + 3], data[5 * i + 4]);
			}

			// Top Face
			n[0] = 0.0f;
			n[1] = 1.0f;
			n[2] =  0.0f;
			s[0] = 1.0f;
			s[1] = 0.0f;
			s[2] =  0.0f;
			t[0] = 0.0f;
			t[1] = 0.0f;
			t[2] = -1.0f;
			for(i = 8; i < 12; i++) {
				c[0] = data[5 * i + 2];
				c[1] = data[5 * i + 3];
				c[2] = data[5 * i + 4];
				SetUpBumps(n, ref c, l, s, t);
				glTexCoord2f(data[5 * i] + c[0], data[5 * i + 1] + c[1]);
				glVertex3f(data[5 * i + 2], data[5 * i + 3], data[5 * i + 4]);
			}

			// Bottom Face
			n[0] =  0.0f;
			n[1] = -1.0f;
			n[2] =  0.0f;
			s[0] = -1.0f;
			s[1] =  0.0f;
			s[2] =  0.0f;
			t[0] =  0.0f;
			t[1] =  0.0f;
			t[2] = -1.0f;
			for(i = 12; i < 16; i++) {
				c[0] = data[5 * i + 2];
				c[1] = data[5 * i + 3];
				c[2] = data[5 * i + 4];
				SetUpBumps(n, ref c, l, s, t);
				glTexCoord2f(data[5 * i] + c[0], data[5 * i + 1] + c[1]); 
				glVertex3f(data[5 * i + 2], data[5 * i + 3], data[5 * i + 4]);
			}

			// Right Face
			n[0] = 1.0f;
			n[1] = 0.0f;
			n[2] =  0.0f;
			s[0] = 0.0f;
			s[1] = 0.0f;
			s[2] = -1.0f;
			t[0] = 0.0f;
			t[1] = 1.0f;
			t[2] =  0.0f;
			for(i = 16; i < 20; i++) {
				c[0] = data[5 * i + 2];
				c[1] = data[5 * i + 3];
				c[2] = data[5 * i + 4];
				SetUpBumps(n, ref c, l, s, t);
				glTexCoord2f(data[5 * i] + c[0], data[5 * i + 1] + c[1]);
				glVertex3f(data[5 * i + 2], data[5 * i + 3], data[5 * i + 4]);
			}

			// Left Face
			n[0] = -1.0f;
			n[1] = 0.0f;
			n[2] = 0.0f;
			s[0] =  0.0f;
			s[1] = 0.0f;
			s[2] = 1.0f;
			t[0] =  0.0f;
			t[1] = 1.0f;
			t[2] = 0.0f;
			for(i = 20; i < 24; i++) {
				c[0] = data[5 * i + 2];
				c[1] = data[5 * i + 3];
				c[2] = data[5 * i + 4];
				SetUpBumps(n, ref c, l, s, t);
				glTexCoord2f(data[5 * i] + c[0], data[5 * i + 1] + c[1]);
				glVertex3f(data[5 * i + 2], data[5 * i + 3], data[5 * i + 4]);
			}
			glEnd();

			// PASS #3: Use Texture "Base", Blend GL_DST_COLOR To GL_SRC_COLOR (Multiplies By 2), Lighting Enabled, No Offset Texture-Coordinates
			if(!emboss) {
				glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_MODULATE);
				glBindTexture(GL_TEXTURE_2D, texture[filter]);
				glBlendFunc(GL_DST_COLOR,GL_SRC_COLOR);
				glEnable(GL_LIGHTING);
				DoCube();
			}

			xrot += xspeed;
			yrot += yspeed;
			if(xrot > 360.0f) {
				xrot -= 360.0f;
			}
			if(xrot < 0.0f) {
				xrot += 360.0f;
			}
			if(yrot > 360.0f) {
				yrot -= 360.0f;
			}
			if(yrot < 0.0f) {
				yrot += 360.0f;
			}

			// LAST PASS: Do The Logos!
//			DoLogo();
		}
		#endregion DoMesh1TexelUnits()

		#region DoMesh2TexelUnits()
		/// <summary>
		/// Does bumpmapping with multi-texturing using two passes and two texel units.
		/// </summary>
		private void DoMesh2TexelUnits() {
			float[] c = {0.0f, 0.0f, 0.0f, 1.0f};										// Holds Current Vertex
			float[] n = {0.0f, 0.0f, 0.0f, 1.0f};										// Normalized Normal Of Current Surface
			float[] s = {0.0f, 0.0f, 0.0f, 1.0f};										// s-Texture Coordinate Direction, Normalized
			float[] t = {0.0f, 0.0f, 0.0f, 1.0f};										// t-Texture Coordinate Direction, Normalized
			float[] l = new float[4];													// Holds Our Light Position To Be Transformed Into Object Space
			float[] Minv = new float[16];												// Holds The Inverted Modelview Matrix To Do So
			int i;

			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);							// Clear The Screen And The Depth Buffer

			// Build Inverse Modelview Matrix First.  This Substitutes One Push/Pop With One glLoadIdentity();
			// Simply Build It By Doing All Transformations Negated And In Reverse Order.
			glLoadIdentity();
			glRotatef(-yrot, 0.0f, 1.0f, 0.0f);
			glRotatef(-xrot, 1.0f, 0.0f, 0.0f);
			glTranslatef(0.0f, 0.0f, -z);
			glGetFloatv(GL_MODELVIEW_MATRIX, Minv);
			glLoadIdentity();
			glTranslatef(0.0f, 0.0f, z);

			glRotatef(xrot, 1.0f, 0.0f, 0.0f);
			glRotatef(yrot, 0.0f, 1.0f, 0.0f);

			// Transform The Lightposition Into Object Coordinates:
			l[0] = LightPosition[0];
			l[1] = LightPosition[1];
			l[2] = LightPosition[2];
			l[3] = 1.0f;																// Homogenous Coordinate
			VMatMult(Minv, ref l);

			// PASS #1: Texel-Unit 0: Use Texture "Bump", No Blend, No Lighting, No Offset Texture-Coordinates, Texture-Operation "Replace"
			//			Texel-Unit 1: Use Texture "Invbump", No Lighting, Offset Texture Coordinates, Texture-Operation "Replace"

			// TEXTURE-UNIT #0
			glActiveTextureARB(GL_TEXTURE0_ARB);
			glEnable(GL_TEXTURE_2D);
			glBindTexture(GL_TEXTURE_2D, bump[filter]);
			glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_COMBINE_ARB);
			glTexEnvf(GL_TEXTURE_ENV, GL_COMBINE_RGB_ARB, GL_REPLACE);
			
			// TEXTURE-UNIT #1:
			glActiveTextureARB(GL_TEXTURE1_ARB);
			glEnable(GL_TEXTURE_2D);
			glBindTexture(GL_TEXTURE_2D, invbump[filter]);
			glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_COMBINE_ARB);
			glTexEnvf(GL_TEXTURE_ENV, GL_COMBINE_RGB_ARB, GL_ADD);

			// General Switches:
			glDisable(GL_BLEND);
			glDisable(GL_LIGHTING);
			glBegin(GL_QUADS);

			// Front Face
			n[0] = 0.0f;
			n[1] = 0.0f;
			n[2] = 1.0f;
			s[0] = 1.0f;
			s[1] = 0.0f;
			s[2] = 0.0f;
			t[0] = 0.0f;
			t[1] = 1.0f;
			t[2] = 0.0f;
			for(i = 0; i < 4; i++) {
				c[0] = data[5 * i + 2];
				c[1] = data[5 * i + 3];
				c[2] = data[5 * i + 4];
				SetUpBumps(n, ref c, l, s, t);
				glMultiTexCoord2fARB(GL_TEXTURE0_ARB, data[5 * i], data[5 * i + 1]);
				glMultiTexCoord2fARB(GL_TEXTURE1_ARB, data[5 * i] + c[0], data[5 * i + 1] + c[1]);
				glVertex3f(data[5 * i + 2], data[5 * i + 3], data[5 * i + 4]);
			}

			// Back Face
			n[0] =  0.0f;
			n[1] = 0.0f;
			n[2] = -1.0f;
			s[0] = -1.0f;
			s[1] = 0.0f;
			s[2] =  0.0f;
			t[0] =  0.0f;
			t[1] = 1.0f;
			t[2] =  0.0f;
			for(i = 4; i < 8; i++) {
				c[0] = data[5 * i + 2];
				c[1] = data[5 * i + 3];
				c[2] = data[5 * i + 4];
				SetUpBumps(n, ref c, l, s, t);
				glMultiTexCoord2fARB(GL_TEXTURE0_ARB, data[5 * i], data[5 * i + 1]);
				glMultiTexCoord2fARB(GL_TEXTURE1_ARB, data[5 * i] + c[0], data[5 * i + 1] + c[1]);
				glVertex3f(data[5 * i + 2], data[5 * i + 3], data[5 * i + 4]);
			}

			// Top Face
			n[0] = 0.0f;
			n[1] = 1.0f;
			n[2] =  0.0f;
			s[0] = 1.0f;
			s[1] = 0.0f;
			s[2] =  0.0f;
			t[0] = 0.0f;
			t[1] = 0.0f;
			t[2] = -1.0f;
			for(i = 8; i < 12; i++) {
				c[0] = data[5 * i + 2];
				c[1] = data[5 * i + 3];
				c[2] = data[5 * i + 4];
				SetUpBumps(n, ref c, l, s, t);
				glMultiTexCoord2fARB(GL_TEXTURE0_ARB, data[5 * i], data[5 * i + 1]);
				glMultiTexCoord2fARB(GL_TEXTURE1_ARB, data[5 * i] + c[0], data[5 * i + 1] +c[1]);
				glVertex3f(data[5 * i + 2], data[5 * i + 3], data[5 * i + 4]);
			}

			// Bottom Face
			n[0] =  0.0f;
			n[1] = -1.0f;
			n[2] =  0.0f;
			s[0] = -1.0f;
			s[1] =  0.0f;
			s[2] =  0.0f;
			t[0] =  0.0f;
			t[1] =  0.0f;
			t[2] = -1.0f;
			for(i = 12; i < 16; i++) {
				c[0] = data[5 * i + 2];
				c[1] = data[5 * i + 3];
				c[2] = data[5 * i + 4];
				SetUpBumps(n, ref c, l, s, t);
				glMultiTexCoord2fARB(GL_TEXTURE0_ARB, data[5 * i], data[5 * i + 1]);
				glMultiTexCoord2fARB(GL_TEXTURE1_ARB, data[5 * i] + c[0], data[5 * i + 1] + c[1]);
				glVertex3f(data[5 * i + 2], data[5 * i + 3], data[5 * i + 4]);
			}

			// Right Face
			n[0] = 1.0f;
			n[1] = 0.0f;
			n[2] =  0.0f;
			s[0] = 0.0f;
			s[1] = 0.0f;
			s[2] = -1.0f;
			t[0] = 0.0f;
			t[1] = 1.0f;
			t[2] =  0.0f;
			for(i = 16; i < 20; i++) {
				c[0] = data[5 * i + 2];
				c[1] = data[5 * i + 3];
				c[2] = data[5 * i + 4];
				SetUpBumps(n, ref c, l, s, t);
				glMultiTexCoord2fARB(GL_TEXTURE0_ARB, data[5 * i], data[5 * i + 1]);
				glMultiTexCoord2fARB(GL_TEXTURE1_ARB, data[5 * i] + c[0], data[5 * i + 1] +c[1]);
				glVertex3f(data[5 * i + 2], data[5 * i + 3], data[5 * i + 4]);
			}

			// Left Face
			n[0] = -1.0f;
			n[1] = 0.0f;
			n[2] = 0.0f;
			s[0] =  0.0f;
			s[1] = 0.0f;
			s[2] = 1.0f;
			t[0] =  0.0f;
			t[1] = 1.0f;
			t[2] = 0.0f;
			for(i = 20; i < 24; i++) {
				c[0] = data[5 * i + 2];
				c[1] = data[5 * i + 3];
				c[2] = data[5 * i + 4];
				SetUpBumps(n, ref c, l, s, t);
				glMultiTexCoord2fARB(GL_TEXTURE0_ARB, data[5 * i], data[5 * i + 1]);
				glMultiTexCoord2fARB(GL_TEXTURE1_ARB, data[5 * i] + c[0], data[5 * i + 1] + c[1]);
				glVertex3f(data[5 * i + 2], data[5 * i + 3], data[5 * i + 4]);
			}
			glEnd();

			// PASS #2: Use Texture "Base", Blend GL_DST_COLOR To GL_SRC_COLOR (Multiplies By 2), Lighting Enabled, No Offset Texture-Coordinates
			glActiveTextureARB(GL_TEXTURE1_ARB);
			glDisable(GL_TEXTURE_2D);
			glActiveTextureARB(GL_TEXTURE0_ARB);
			if(!emboss) {
				glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_MODULATE);
				glBindTexture(GL_TEXTURE_2D, texture[filter]);
				glBlendFunc(GL_DST_COLOR, GL_SRC_COLOR);
				glEnable(GL_BLEND);
				glEnable(GL_LIGHTING);
				DoCube();
			}

			xrot += xspeed;
			yrot += yspeed;
			if(xrot > 360.0f) {
				xrot -= 360.0f;
			}
			if(xrot < 0.0f) {
				xrot += 360.0f;
			}
			if(yrot > 360.0f) {
				yrot -= 360.0f;
			}
			if(yrot < 0.0f) {
				yrot += 360.0f;
			}

			glGetError();

			// LAST PASS: Do The Logos!
//			DoLogo();
		}
		#endregion DoMesh2TexelUnits()

		#region DoMeshNoBumps()
		/// <summary>
		/// Draws the mesh with no bumpmapping at all.
		/// </summary>
		private void DoMeshNoBumps() {
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);							// Clear The Screen And The Depth Buffer
			glLoadIdentity();															// Reset The View
			glTranslatef(0.0f, 0.0f, z);

			glRotatef(xrot, 1.0f, 0.0f, 0.0f);
			glRotatef(yrot, 0.0f, 1.0f, 0.0f);
			if(useMultitexture) {
				glActiveTextureARB(GL_TEXTURE1_ARB);
				glDisable(GL_TEXTURE_2D);
				glActiveTextureARB(GL_TEXTURE0_ARB);
			}
			glDisable(GL_BLEND);
			glBindTexture(GL_TEXTURE_2D, texture[filter]);
			glBlendFunc(GL_DST_COLOR, GL_SRC_COLOR);
			glEnable(GL_LIGHTING);
			DoCube();

			xrot += xspeed;
			yrot += yspeed;
			if(xrot > 360.0f) {
				xrot -= 360.0f;
			}
			if(xrot < 0.0f) {
				xrot += 360.0f;
			}
			if(yrot > 360.0f) {
				yrot -= 360.0f;
			}
			if(yrot < 0.0f) {
				yrot += 360.0f;
			}

			// LAST PASS: Do The Logos!
//			DoLogo();
		}
		#endregion DoMeshNoBumps()

		#region InitLights()
		/// <summary>
		/// Sets up our light.
		/// </summary>
		private void InitLights() {
			glLightfv(GL_LIGHT1, GL_AMBIENT, LightAmbient);								// Load Light-Parameters into GL_LIGHT1
			glLightfv(GL_LIGHT1, GL_DIFFUSE, LightDiffuse);
			glLightfv(GL_LIGHT1, GL_POSITION, LightPosition);
			glEnable(GL_LIGHT1);														// Enable Light One
		}
		#endregion InitLights()

		#region bool InitMultitexture()
		/// <summary>
		/// Checks at runtime to ensure multitexturing is supported.
		/// </summary>
		/// <returns>Returns true indicating multitexturing is supported, false otherwise.</returns>
		private bool InitMultitexture() {
			string extensions;

			extensions = glGetString(GL_EXTENSIONS);									// Fetch Extension String

			extensions = extensions.Replace(" ", "\n");									// Replace Spaces With Newlines

			if(EXT_INFO) {																// Check If We Should Display Extensions On Startup
				// Show Them All
				MessageBox.Show(extensions, "Supported OpenGL Extensions", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}

			if(extensions.IndexOf("GL_ARB_multitexture") != -1) {						// Is Multitexturing Supported?
				if(ARB_ENABLE) {														// Override Flag
					if(extensions.IndexOf("GL_ARB_texture_env_combine") != -1) {		// Is texture-environment-combining Supported?
						glGetIntegerv(GL_MAX_TEXTURE_UNITS_ARB, out maxTexelUnits);		// Find Out How Many Texel Units Are Available
						if(EXT_INFO) {													// If We're Debugging
							// Show Confirmation
							MessageBox.Show("Texel Units: " + maxTexelUnits, "Feature Supported!", MessageBoxButtons.OK, MessageBoxIcon.Information);
							MessageBox.Show("The GL_ARB_multitexture extension will be used.", "Feature Supported!", MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
						return true;													// Multitexturing Supported!
					}
				}
			}

			useMultitexture = false;													// We Can't Use It If It Isn't Supported!
			return false;																// Not Supported!
		}
		#endregion bool InitMultitexture()

		#region LoadTextures()
		/// <summary>
		/// Loads and creates the textures.
		/// </summary>
		private void LoadTextures() {
			string filename = @"..\..\data\NeHeLesson22\Base.bmp";						// The File To Load
			Bitmap bitmap = null;														// The Bitmap Image For Our Texture
			Rectangle rectangle;														// The Rectangle For Locking The Bitmap In Memory
			BitmapData bitmapData = null;												// The Bitmap's Pixel Data

			// Load The Bitmaps
			try {
				// First Bitmap, The Tile Bitmap For Base Texture
				bitmap = new Bitmap(filename);											// Load The File As A Bitmap
				bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);						// Flip The Bitmap Along The Y-Axis
				rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);			// Select The Whole Bitmap
				
				// Get The Pixel Data From The Locked Bitmap
				bitmapData = bitmap.LockBits(rectangle, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

				glGenTextures(3, texture);												// Create 3 Textures

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

				bitmap.UnlockBits(bitmapData);											// Unlock The Pixel Data From Memory

				// Second Bitmap, The Bumpmaps
				glPixelTransferf(GL_RED_SCALE, 0.5f);									// Scale RGB By 50%, So That We Have Only
				glPixelTransferf(GL_GREEN_SCALE, 0.5f);									// Half Intenstity
				glPixelTransferf(GL_BLUE_SCALE, 0.5f);

				// No Wrapping, Please!
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP);
				glTexParameterfv(GL_TEXTURE_2D, GL_TEXTURE_BORDER_COLOR, Gray);

				filename = @"..\..\data\NeHeLesson22\Bump.bmp";							// The File To Load
				bitmap = new Bitmap(filename);											// Load The File As A Bitmap
				bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);						// Flip The Bitmap Along The Y-Axis
				rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);			// Select The Whole Bitmap
				
				// Get The Pixel Data From The Locked Bitmap
				bitmapData = bitmap.LockBits(rectangle, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

				glGenTextures(3, bump);													// Create 3 Textures

				// Create Nearest Filtered Texture
				glBindTexture(GL_TEXTURE_2D, bump[0]);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST); 
				glTexImage2D(GL_TEXTURE_2D, 0, (int) GL_RGB8, bitmap.Width, bitmap.Height, 0, GL_BGR_EXT, GL_UNSIGNED_BYTE, bitmapData.Scan0);

				// Create Linear Filtered Texture
				glBindTexture(GL_TEXTURE_2D, bump[1]);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR); 
				glTexImage2D(GL_TEXTURE_2D, 0, (int) GL_RGB8, bitmap.Width, bitmap.Height, 0, GL_BGR_EXT, GL_UNSIGNED_BYTE, bitmapData.Scan0);

				// Create MipMapped Texture
				glBindTexture(GL_TEXTURE_2D, bump[2]);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_NEAREST);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR); 
				gluBuild2DMipmaps(GL_TEXTURE_2D, (int) GL_RGB8, bitmap.Width, bitmap.Height, GL_BGR_EXT, GL_UNSIGNED_BYTE, bitmapData.Scan0);

				bitmap.UnlockBits(bitmapData);											// Unlock The Pixel Data From Memory

				// Third Bitmap, The Inverted Bumpmap
				// Invert The Bumpmap
				bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);						// Flip The Bitmap Back Along The Y-Axis
				rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);			// Select The Whole Bitmap
				
				// Get The Pixel Data From The Locked Bitmap
				bitmapData = bitmap.LockBits(rectangle, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

				glGenTextures(3, invbump);												// Create 3 Textures

				// Create Nearest Filtered Texture
				glBindTexture(GL_TEXTURE_2D, invbump[0]);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST); 
				glTexImage2D(GL_TEXTURE_2D, 0, (int) GL_RGB8, bitmap.Width, bitmap.Height, 0, GL_BGR_EXT, GL_UNSIGNED_BYTE, bitmapData.Scan0);

				// Create Linear Filtered Texture
				glBindTexture(GL_TEXTURE_2D, invbump[1]);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR); 
				glTexImage2D(GL_TEXTURE_2D, 0, (int) GL_RGB8, bitmap.Width, bitmap.Height, 0, GL_BGR_EXT, GL_UNSIGNED_BYTE, bitmapData.Scan0);

				// Create MipMapped Texture
				glBindTexture(GL_TEXTURE_2D, invbump[2]);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_NEAREST);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR); 
				gluBuild2DMipmaps(GL_TEXTURE_2D, (int) GL_RGB8, bitmap.Width, bitmap.Height, GL_BGR_EXT, GL_UNSIGNED_BYTE, bitmapData.Scan0);

				glPixelTransferf(GL_RED_SCALE, 1.0f);									// Scale RGB Back To 100% Again
				glPixelTransferf(GL_GREEN_SCALE, 1.0f);
				glPixelTransferf(GL_BLUE_SCALE, 1.0f);

/*
				// Load The Logo Bitmap
				filename = @"..\..\data\NeHeLesson22\OpenGL_Alpha.bmp";					// The File To Load
				bitmap = new Bitmap(filename);											// Load The File As A Bitmap
				rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);			// Select The Whole Bitmap
				
				// Get The Pixel Data From The Locked Bitmap
				bitmapData = bitmap.LockBits(rectangle, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

				glGenTextures(1, glLogo);												// Create One Texture

				// Create Linear Filtered RGBA8-Texture
				glBindTexture(GL_TEXTURE_2D, glLogo[0]);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
				glTexImage2D(GL_TEXTURE_2D, 0, (int) GL_RGBA8, bitmap.Width, bitmap.Height, 0, GL_BGRA_EXT, GL_UNSIGNED_BYTE, bitmapData.Scan0);


				// Load The Extension Enabled Logo
				filename = @"..\..\data\NeHeLesson22\Multi_On_Alpha.bmp";				// The File To Load
				bitmap = new Bitmap(filename);											// Load The File As A Bitmap
				rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);			// Select The Whole Bitmap
				
				// Get The Pixel Data From The Locked Bitmap
				bitmapData = bitmap.LockBits(rectangle, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

				glGenTextures(1, multiLogo);											// Create One Texture

				// Create Linear Filtered RGBA8-Texture
				glBindTexture(GL_TEXTURE_2D, multiLogo[0]);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
				glTexImage2D(GL_TEXTURE_2D, 0, (int) GL_RGBA8, bitmap.Width, bitmap.Height, 0, GL_BGRA_EXT, GL_UNSIGNED_BYTE, bitmapData.Scan0);
*/
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

		#region SetUpBumps(float[] n, ref float[] c, float[] l, float[] s, float[] t)
		/// <summary>
		/// Sets up the texture offsets.
		/// </summary>
		/// <param name="n">Normal on surface.  Must be of length 1.</param>
		/// <param name="c">Current vertex on surface.</param>
		/// <param name="l">Lightposition.</param>
		/// <param name="s">Direction of s-texture-coordinate in object space (must be normalized).</param>
		/// <param name="t">Direction of t-texture-coordinate in object space (must be normalized).</param>
		private void SetUpBumps(float[] n, ref float[] c, float[] l, float[] s, float[] t) {
			float[] v = new float[3];													// Vertex From Current Position To Light
			float lenQ;																	// Used To Normalize

			// Calculate v From Current Vector c To Lightposition And Normalize v
			v[0] = l[0] - c[0];
			v[1] = l[1] - c[1];
			v[2] = l[2] - c[2];
			lenQ = (float) Math.Sqrt(v[0] * v[0] + v[1] * v[1] + v[2] * v[2]);
			v[0] /= lenQ;
			v[1] /= lenQ;
			v[2] /= lenQ;
			// Project v Such That We Get Two Values Along Each Texture-Coordinat Axis.
			c[0] = (s[0] * v[0] + s[1] * v[1] + s[2] * v[2]) * MAX_EMBOSS;
			c[1] = (t[0] * v[0] + t[1] * v[1] + t[2] * v[2]) * MAX_EMBOSS;
		}
		#endregion SetUpBumps(float[] n, ref float[] c, float[] l, float[] s, float[] t)

		#region VMatMult(float[] M, ref float[] v)
		/// <summary>
		/// // Calculates v = vM.
		/// </summary>
		/// <param name="M">M is 4x4 in column-major.</param>
		/// <param name="v">v is 4-dimensional row (i.e. "transposed").</param>
		private void VMatMult(float[] M, ref float[] v) {
			float[] res = new float[3];

			res[0] = M[0] * v[0] + M[1] * v[1] + M[2] * v[2] + M[3] * v[3];
			res[1] = M[4] * v[0] + M[5] * v[1] + M[6] * v[2] + M[7] * v[3];
			res[2] = M[8] * v[0] + M[9] * v[1] + M[10] * v[2] + M[11] * v[3];
			v[0] = res[0];
			v[1] = res[1];
			v[2] = res[2];
			v[3] = M[15];																// Homogenous Coordinate
		}
		#endregion VMatMult(float[] M, ref float[] v)
	}
}