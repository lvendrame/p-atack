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
/* rings.c
 *
 * Homework 4, Part 1: perspective, hierarchical coords, moving eye pos.
 *
 * Do a slow zoom on a bunch of rings (ala Superman III?)
 *
 * Philip Winston - 2/21/95
 * pwinston@hmc.edu
 * http://www.cs.hmc.edu/people/pwinston
 *
 */
#endregion Original Credits / License

using CsGL.Basecode;
using System;
using System.Data;
using System.Reflection;
using System.Windows.Forms;

#region AssemblyInfo
[assembly: AssemblyCompany("The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyCopyright("2002 The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyDescription("GLUT Contrib Rings")]
[assembly: AssemblyProduct("GLUT Contrib Rings")]
[assembly: AssemblyTitle("GLUT Contrib Rings")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace GlutContrib {
	/// <summary>
	/// GLUT Contrib Rings -- Spinning Rings (http://www.opengl.org/developers/code/glut_examples/contrib/contrib.html)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class GlutContribRings : Model {
		// --- Fields ---
		#region Private Fields
		private const float M_PI = 3.14159265f;
		private static int STEPS = 30;
		private static bool Fade = true;
		private static float Axis = 0, AxisInc = (float) (2.0 * M_PI / STEPS);
		private static float InnerRad, OutterRad, Tilt, Trans, TransCone, Dist;
		private static uint CONE = 1, TORUS = 2, INNERMAT = 3, OUTTERMAT = 4;
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Example title.
		/// </summary>
		public override string Title {
			get {
				return "GLUT Contrib Rings -- Spinning Rings";
			}
		}

		/// <summary>
		/// Example description.
		/// </summary>
		public override string Description {
			get {
				return "Magic rings balancing on each other precariously.";
			}
		}

		/// <summary>
		/// Example URL.
		/// </summary>
		public override string Url {
			get {
				return "http://www.opengl.org/developers/code/glut_examples/contrib/contrib.html";
			}
		}
		#endregion Public Properties

		// --- Entry Point ---
		#region Main()
		/// <summary>
		/// Application's entry point, runs this GLUT example.
		/// </summary>
		public static void Main() {														// Entry Point
			App.Run(new GlutContribRings());											// Run Our Example As A Windows Forms Application
		}
		#endregion Main()

		// --- Basecode Methods ---
		#region Initialize()
		/// <summary>
		/// Overrides OpenGL's initialization.
		/// </summary>
		public override void Initialize() {
			// mainly computes the translation amount as a function of the tilt angle and torus radii
			float sinoftilt;
			InnerRad = 0.70f;
			OutterRad = 5.0f;
			Tilt = 15.0f;
			Dist = 10.0f;
			sinoftilt = (float) Math.Sin(Tilt * M_PI * 2 / 360);
			Trans = (2 * OutterRad + InnerRad) * sinoftilt + InnerRad + ((1 - sinoftilt) * InnerRad) - (InnerRad * 1 / 10);
			TransCone = Trans + (OutterRad * sinoftilt + InnerRad);

			// I used code from the book's accnot.c as a starting point for lighting.
			// I have one positional light in center, then one directional 
			float[] light0_position = {1.0f, 0.2f, 1.0f, 0.0f};
			float[] light1_position = {0.0f, 0.0f, 0.0f, 1.0f};
			float[] light1_diffuse = {1.0f, 1.0f, 1.0f, 1.0f};
			float[] light1_specular = {1.0f, 1.0f, 1.0f, 1.0f};
			float[] lm_ambient = {0.2f, 0.2f, 0.2f, 1.0f};

			glEnable(GL_NORMALIZE);
			glMatrixMode(GL_PROJECTION);
			glLoadIdentity();
			glMatrixMode(GL_MODELVIEW);
			glLoadIdentity();

			glLightfv(GL_LIGHT0, GL_POSITION, light0_position);
			glLightfv(GL_LIGHT1, GL_POSITION, light1_position);
			glLightfv(GL_LIGHT1, GL_DIFFUSE,  light1_diffuse);
			glLightfv(GL_LIGHT1, GL_SPECULAR, light1_specular);
			glLightf(GL_LIGHT1, GL_LINEAR_ATTENUATION, 0.2f);
			glLightModelfv(GL_LIGHT_MODEL_AMBIENT, lm_ambient);

			glEnable(GL_LIGHTING);
			glEnable(GL_LIGHT0);
			glEnable(GL_LIGHT1);

			glDepthFunc(GL_LESS);
			glEnable(GL_DEPTH_TEST);

			glFlush();

			MakeDisplayLists();
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws GLUT Contrib Rings scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			// Draw the inner thing, then glScale and draw 3 huge rings
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

			glMatrixMode(GL_PROJECTION);
			glLoadIdentity();
			glFrustum(-1, 1, -1, 1, 10, 1000);
			gluLookAt(0, 0, Dist, 0, 0, 0, 0, 1, 0);

			glMatrixMode(GL_MODELVIEW);

			glCallList(INNERMAT);
			DrawRings(Axis);
			glCallList(CONE);

			glCallList(OUTTERMAT);
			glPushMatrix();
			glScalef(10, 10, 10);
			DrawRings(Axis / 3);
			glPopMatrix();

			glFlush();

			System.Threading.Thread.Sleep(15);											// Slow Down The Animation

			// rotate the axis and adjust position if nec.
			Axis += AxisInc;

			if(Dist < 15 && Fade) {														// Start Slow
				Dist += 0.1f;
			}
			else if(Dist < 800 && Fade) {												// Don't Go Back Too Far
				Dist *= 1.005f;
			}
		}
		#endregion Draw()

/*
		#region InputHelp()
		/// <summary>
		/// Overrides default input help, supplying example-specific help information.
		/// </summary>
		public override void InputHelp() {
			base.InputHelp();															// Set Up The Default Input Help

			DataRow dataRow;															// Row To Add

			dataRow = InputHelpDataTable.NewRow();										// R - Rotate Lines
			dataRow["Input"] = "R";
			dataRow["Effect"] = "Rotate Lines";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);
		}
		#endregion InputHelp()

		#region ProcessInput()
		/// <summary>
		/// Overrides default input handling, adding example-specific input handling.
		/// </summary>
		public override void ProcessInput() {
			base.ProcessInput();														// Handle The Default Basecode Keys

			if(KeyState[(int) Keys.R]) {												// Is R Key Being Pressed?
				KeyState[(int) Keys.R] = false;											// Mark As Handled
				rotAngle += 20.0f;														// Rotate Lines
				if(rotAngle >= 360.0f) {
					rotAngle = 0.0f;
				}
			}
		}
		#endregion ProcessInput()
*/

		#region Reshape(int width, int height)
		/// <summary>
		/// Overrides OpenGL reshaping.
		/// </summary>
		/// <param name="width">New width.</param>
		/// <param name="height">New height.</param>
		public override void Reshape(int width, int height) {							// Resize And Initialize The GL Window
			glViewport(0, 0, width, height);
			glFlush();
		}
		#endregion Reshape(int width, int height)

		// --- Example Methods ---
		#region DrawRings(float axis)
		/// <summary>
		/// Draws the rings.
		/// </summary>
		/// <param name="axis">The axis.</param>
		private static void DrawRings(float axis) {
			// Draw three rings, rotated and translated so they look cool
			float x = (float) Math.Sin(axis);
			float y = (float) Math.Cos(axis);

			glPushMatrix();
				glTranslatef(0, Trans, 0);
				glRotatef(Tilt, x, 0, y);
				glCallList(TORUS);
			glPopMatrix();

			glPushMatrix();
				glRotatef(-Tilt, x, 0, y);
				glCallList(TORUS);
			glPopMatrix();

			glPushMatrix();
				glTranslatef(0, -Trans, 0);
				glRotatef(Tilt, x, 0, y);
				glCallList(TORUS);
			glPopMatrix();
		}
		#endregion DrawRings(float axis)

		#region MakeDisplayLists()
		/// <summary>
		/// Creates the display lists.
		/// </summary>
		private static void MakeDisplayLists() {
			// setup display lists to change material for inner/outter rings and to draw a single torus or cone
			float[] cone_diffuse = {0.0f, 0.7f, 0.7f, 1.0f};
			float[] mat1_ambient = {1.0f, 1.0f, 1.0f, 1.0f};
			float[] mat2_ambient = {0.0f, 0.0f, 0.0f, 0.0f};
			float[] torus1_diffuse = {0.7f, 0.7f, 0.0f, 1.0f};
			float[] torus2_diffuse = {0.3f, 0.0f, 0.0f, 1.0f};
			float[] mat1_specular = {1.0f, 1.0f, 1.0f, 1.0f};
			float[] mat2_specular = {0.5f, 0.5f, 0.5f, 1.0f};

			glNewList(INNERMAT, GL_COMPILE);
				glMaterialfv(GL_FRONT, GL_SPECULAR, mat1_specular);
				glMaterialf(GL_FRONT, GL_SHININESS, 50.0f);
				glMaterialfv(GL_FRONT, GL_DIFFUSE, torus1_diffuse);
				glMaterialfv(GL_FRONT, GL_AMBIENT, mat1_ambient);
			glEndList();

			glNewList(OUTTERMAT, GL_COMPILE);
				glMaterialfv(GL_FRONT, GL_SPECULAR, mat2_specular);
				glMaterialf(GL_FRONT, GL_SHININESS, 25.0f);
				glMaterialfv(GL_FRONT, GL_DIFFUSE, torus2_diffuse);
				glMaterialfv(GL_FRONT, GL_AMBIENT, mat2_ambient);
			glEndList();

			glNewList(CONE, GL_COMPILE);
				glMaterialfv(GL_FRONT, GL_DIFFUSE, cone_diffuse);
				glPushMatrix();
					glTranslatef(0, -TransCone, 0);
					glRotatef(90, 1, 0, 0);
					glutSolidCone(OutterRad, 10, 8, 8);
				glPopMatrix();
			glEndList();

			glNewList(TORUS, GL_COMPILE);
				glPushMatrix();
					glRotatef(90, 1, 0, 0);
					glutSolidTorus(InnerRad, OutterRad, 15, 25);
				glPopMatrix();
			glEndList();
		}
		#endregion MakeDisplayLists()
	}
}