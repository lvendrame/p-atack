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
 * Copyright (c) 1993-1997, Silicon Graphics, Inc.
 * ALL RIGHTS RESERVED 
 * Permission to use, copy, modify, and distribute this software for 
 * any purpose and without fee is hereby granted, provided that the above
 * copyright notice appear in all copies and that both the copyright notice
 * and this permission notice appear in supporting documentation, and that 
 * the name of Silicon Graphics, Inc. not be used in advertising
 * or publicity pertaining to distribution of the software without specific,
 * written prior permission. 
 *
 * THE MATERIAL EMBODIED ON THIS SOFTWARE IS PROVIDED TO YOU "AS-IS"
 * AND WITHOUT WARRANTY OF ANY KIND, EXPRESS, IMPLIED OR OTHERWISE,
 * INCLUDING WITHOUT LIMITATION, ANY WARRANTY OF MERCHANTABILITY OR
 * FITNESS FOR A PARTICULAR PURPOSE.  IN NO EVENT SHALL SILICON
 * GRAPHICS, INC.  BE LIABLE TO YOU OR ANYONE ELSE FOR ANY DIRECT,
 * SPECIAL, INCIDENTAL, INDIRECT OR CONSEQUENTIAL DAMAGES OF ANY
 * KIND, OR ANY DAMAGES WHATSOEVER, INCLUDING WITHOUT LIMITATION,
 * LOSS OF PROFIT, LOSS OF USE, SAVINGS OR REVENUE, OR THE CLAIMS OF
 * THIRD PARTIES, WHETHER OR NOT SILICON GRAPHICS, INC.  HAS BEEN
 * ADVISED OF THE POSSIBILITY OF SUCH LOSS, HOWEVER CAUSED AND ON
 * ANY THEORY OF LIABILITY, ARISING OUT OF OR IN CONNECTION WITH THE
 * POSSESSION, USE OR PERFORMANCE OF THIS SOFTWARE.
 * 
 * US Government Users Restricted Rights 
 * Use, duplication, or disclosure by the Government is subject to
 * restrictions set forth in FAR 52.227.19(c)(2) or subparagraph
 * (c)(1)(ii) of the Rights in Technical Data and Computer Software
 * clause at DFARS 252.227-7013 and/or in similar or successor
 * clauses in the FAR or the DOD or NASA FAR Supplement.
 * Unpublished-- rights reserved under the copyright laws of the
 * United States.  Contractor/manufacturer is Silicon Graphics,
 * Inc., 2011 N.  Shoreline Blvd., Mountain View, CA 94039-7311.
 *
 * OpenGL(R) is a registered trademark of Silicon Graphics, Inc.
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
[assembly: AssemblyDescription("Redbook Polyoff")]
[assembly: AssemblyProduct("Redbook Polyoff")]
[assembly: AssemblyTitle("Redbook Polyoff")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace RedbookExamples {
	/// <summary>
	/// Redbook Polyoff -- Polygon Offset (http://www.opengl.org/developers/code/examples/redbook/redbook.html)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class RedbookPolyoff : Model {
		// --- Fields ---
		#region Private Fields
		private static uint list;
		private static int spinx = 0;
		private static int spiny = 0;
		private static float tdist = 0.0f;
		private static float polyfactor = 1.0f;
		private static float polyunits = 1.0f;
		private static float[] mat_ambient = {0.8f, 0.8f, 0.8f, 1.0f};
		private static float[] mat_diffuse = {1.0f, 0.0f, 0.5f, 1.0f};
		private static float[] mat_specular = {1.0f, 1.0f, 1.0f, 1.0f};
		private static float[] gray = {0.8f, 0.8f, 0.8f, 1.0f};
		private static float[] black = {0.0f, 0.0f, 0.0f, 1.0f};
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Example title.
		/// </summary>
		public override string Title {
			get {
				return "Redbook Polyoff -- Polygon Offset";
			}
		}

		/// <summary>
		/// Example description.
		/// </summary>
		public override string Description {
			get {
				return "This program demonstrates polygon offset to draw a shaded polygon and its wireframe counterpart without ugly visual artifacts (\"stitching\").";
			}
		}

		/// <summary>
		/// Example URL.
		/// </summary>
		public override string Url {
			get {
				return "http://www.opengl.org/developers/code/examples/redbook/redbook.html";
			}
		}
		#endregion Public Properties

		// --- Entry Point ---
		#region Main()
		/// <summary>
		/// Application's entry point, runs this Redbook example.
		/// </summary>
		public static void Main() {														// Entry Point
			App.Run(new RedbookPolyoff());												// Run Our Example As A Windows Forms Application
		}
		#endregion Main()

		// --- Basecode Methods ---
		#region Initialize()
		/// <summary>
		/// Overrides OpenGL's initialization.
		/// </summary>
		public override void Initialize() {
			// specify initial properties create display list with sphere
			// initialize lighting and depth buffer
			float[] light_ambient = {0.0f, 0.0f, 0.0f, 1.0f};
			float[] light_diffuse = {1.0f, 1.0f, 1.0f, 1.0f};
			float[] light_specular = {1.0f, 1.0f, 1.0f, 1.0f};
			float[] light_position = {1.0f, 1.0f, 1.0f, 0.0f};
			float[] global_ambient = {0.2f, 0.2f, 0.2f, 1.0f};

			glClearColor(0.0f, 0.0f, 0.0f, 1.0f);

			list = glGenLists(1);
			glNewList(list, GL_COMPILE);
				glutSolidSphere(1.0f, 20, 12);
			glEndList();

			glEnable(GL_DEPTH_TEST);

			glLightfv(GL_LIGHT0, GL_AMBIENT, light_ambient);
			glLightfv(GL_LIGHT0, GL_DIFFUSE, light_diffuse);
			glLightfv(GL_LIGHT0, GL_SPECULAR, light_specular);
			glLightfv(GL_LIGHT0, GL_POSITION, light_position);
			glLightModelfv(GL_LIGHT_MODEL_AMBIENT, global_ambient);
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws Redbook Polyoff scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			// draws two spheres, one with a gray, diffuse material, the other
			// sphere with a magenta material with a specular highlight.
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
			glPushMatrix();
				glTranslatef(0.0f, 0.0f, tdist);
				glRotatef((float) spinx, 1.0f, 0.0f, 0.0f);
				glRotatef((float) spiny, 0.0f, 1.0f, 0.0f);

				glMaterialfv(GL_FRONT, GL_AMBIENT_AND_DIFFUSE, gray);
				glMaterialfv(GL_FRONT, GL_SPECULAR, black);
				glMaterialf(GL_FRONT, GL_SHININESS, 0.0f);
				glEnable(GL_LIGHTING);
				glEnable(GL_LIGHT0);
				glEnable(GL_POLYGON_OFFSET_FILL);
				glPolygonOffset(polyfactor, polyunits);
				glCallList(list);
				glDisable(GL_POLYGON_OFFSET_FILL);

				glDisable(GL_LIGHTING);
				glDisable(GL_LIGHT0);
				glColor3f(1.0f, 1.0f, 1.0f);
				glPolygonMode(GL_FRONT_AND_BACK, GL_LINE);
				glCallList(list);
				glPolygonMode(GL_FRONT_AND_BACK, GL_FILL);
			glPopMatrix();
			glFlush();
		}
		#endregion Draw()

		#region InputHelp()
		/// <summary>
		/// Overrides default input help, supplying example-specific help information.
		/// </summary>
		public override void InputHelp() {
			base.InputHelp();															// Set Up The Default Input Help

			DataRow dataRow;															// Row To Add

			dataRow = InputHelpDataTable.NewRow();										// Left Mouse Button - Spin On X Axis
			dataRow["Input"] = "Left Mouse Button";
			dataRow["Effect"] = "Spin On X Axis";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Right Mouse Button - Spin On Y Axis
			dataRow["Input"] = "Right Mouse Button";
			dataRow["Effect"] = "Spin On Y Axis";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// T Key - Increase Distance
			dataRow["Input"] = "T";
			dataRow["Effect"] = "Increase Distance [-5.0 - 4.0]";
			dataRow["Current State"] = tdist;
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// G Key - Decrease Distance
			dataRow["Input"] = "G";
			dataRow["Effect"] = "Decrease Distance [-5.0 - 4.0]";
			dataRow["Current State"] = tdist;
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// F Key - Increase Poly Factor
			dataRow["Input"] = "F";
			dataRow["Effect"] = "Increase Poly Factor";
			dataRow["Current State"] = polyfactor;
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// V Key - Decrease Poly Factor
			dataRow["Input"] = "V";
			dataRow["Effect"] = "Decrease Poly Factor";
			dataRow["Current State"] = polyfactor;
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// U Key - Increase Poly Units
			dataRow["Input"] = "U";
			dataRow["Effect"] = "Increase Poly Units";
			dataRow["Current State"] = polyunits;
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// J Key - Decrease Poly Units
			dataRow["Input"] = "J";
			dataRow["Effect"] = "Decrease Poly Units";
			dataRow["Current State"] = polyunits;
			InputHelpDataTable.Rows.Add(dataRow);
		}
		#endregion InputHelp()

		#region ProcessInput()
		/// <summary>
		/// Overrides default input handling, adding example-specific input handling.
		/// </summary>
		public override void ProcessInput() {
			base.ProcessInput();														// Handle The Default Basecode Keys

			if(Model.Mouse.LeftButton) {												// If Left Mouse Button Is Being Pressed
				spinx = (spinx + 5) % 360;												// Spin On The X Axis
			}

			if(Model.Mouse.RightButton) {												// If Right Mouse Button Is Being Pressed
				spiny = (spiny + 5) % 360;												// Spin On The Y Axis
			}

			if(KeyState[(int) Keys.T]) {												// Is T Key Being Pressed?
				KeyState[(int) Keys.T] = false;											// Mark As Handled
				if(tdist < 4.0f) {														// Increase Distance
					tdist += 0.5f;
					UpdateInputHelp();													// Update Help Screen
				}
			}

			if(KeyState[(int) Keys.G]) {												// Is G Key Being Pressed?
				KeyState[(int) Keys.G] = false;											// Mark As Handled
				if(tdist > -5.0f) {														// Decrease Distance
					tdist -= 0.5f;
					UpdateInputHelp();													// Update Help Screen
				}
			}

			if(KeyState[(int) Keys.F]) {												// Is F Key Being Pressed?
				KeyState[(int) Keys.F] = false;											// Mark As Handled
				polyfactor += 0.1f;														// Increase Poly Factor
				UpdateInputHelp();													// Update Help Screen
			}

			if(KeyState[(int) Keys.V]) {												// Is V Key Being Pressed?
				KeyState[(int) Keys.V] = false;											// Mark As Handled
				polyfactor -= 0.1f;														// Increase Poly Factor
				UpdateInputHelp();													// Update Help Screen
			}

			if(KeyState[(int) Keys.U]) {												// Is U Key Being Pressed?
				KeyState[(int) Keys.U] = false;											// Mark As Handled
				polyunits += 1.0f;														// Increase Poly Units
				UpdateInputHelp();													// Update Help Screen
			}

			if(KeyState[(int) Keys.J]) {												// Is J Key Being Pressed?
				KeyState[(int) Keys.J] = false;											// Mark As Handled
				polyunits -= 1.0f;														// Decrease Poly Units
				UpdateInputHelp();													// Update Help Screen
			}
		}
		#endregion ProcessInput()

		#region Reshape(int width, int height)
		/// <summary>
		/// Overrides OpenGL reshaping.
		/// </summary>
		/// <param name="width">New width.</param>
		/// <param name="height">New height.</param>
		public override void Reshape(int width, int height) {							// Resize And Initialize The GL Window
			glViewport(0, 0, width, height);
			glMatrixMode(GL_PROJECTION);
			glLoadIdentity();
			gluPerspective(45.0f, (float) width / (float) height, 1.0f, 10.0f);
			glMatrixMode(GL_MODELVIEW);
			glLoadIdentity();
			gluLookAt(0.0f, 0.0f, 5.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f);
		}
		#endregion Reshape(int width, int height)
	}
}