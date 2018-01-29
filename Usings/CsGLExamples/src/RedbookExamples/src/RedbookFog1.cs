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
using System.Data;
using System.Reflection;
using System.Windows.Forms;

#region AssemblyInfo
[assembly: AssemblyCompany("The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyCopyright("2002 The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyDescription("Redbook Fog1")]
[assembly: AssemblyProduct("Redbook Fog1")]
[assembly: AssemblyTitle("Redbook Fog1")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace RedbookExamples {
	/// <summary>
	/// Redbook Fog1 -- Fog Modes (http://www.opengl.org/developers/code/examples/redbook/redbook.html)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class RedbookFog1 : Model {
		// --- Fields ---
		#region Private Fields
		private static int fogMode;
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Example title.
		/// </summary>
		public override string Title {
			get {
				return "Redbook Fog1 -- Fog Modes";
			}
		}

		/// <summary>
		/// Example description.
		/// </summary>
		public override string Description {
			get {
				return "This program draws 5 red spheres, each at a different Z distance from the eye, in different types of fog.  Pressing the f key chooses between 3 types of fog:  exponential, exponential squared, and linear.  In this program, there is a fixed density value, as well as fixed start and end values for the linear fog.";
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
			App.Run(new RedbookFog1());													// Run Our Example As A Windows Forms Application
		}
		#endregion Main()

		// --- Basecode Methods ---
		#region Initialize()
		/// <summary>
		/// Overrides OpenGL's initialization.
		/// </summary>
		public override void Initialize() {
			// Initialize depth buffer, fog, light source, material property, and lighting model.
 			glEnable(GL_DEPTH_TEST);

			float[] position = {0.5f, 0.5f, 3.0f, 0.0f};
			glLightfv(GL_LIGHT0, GL_POSITION, position);
			glEnable(GL_LIGHTING);
			glEnable(GL_LIGHT0);

			float[] mat = {0.1745f, 0.01175f, 0.01175f};
			glMaterialfv(GL_FRONT, GL_AMBIENT, mat);

			mat[0] = 0.61424f;
			mat[1] = 0.04136f;
			mat[2] = 0.04136f;
			glMaterialfv(GL_FRONT, GL_DIFFUSE, mat);

			mat[0] = 0.727811f;
			mat[1] = 0.626959f;
			mat[2] = 0.626959f;
			glMaterialfv(GL_FRONT, GL_SPECULAR, mat);

			glMaterialf(GL_FRONT, GL_SHININESS, 0.6f * 128.0f);

			glEnable(GL_FOG);
			float[] fogColor = {0.5f, 0.5f, 0.5f, 1.0f};

			fogMode = (int) GL_EXP;
			glFogi(GL_FOG_MODE, fogMode);
			glFogfv(GL_FOG_COLOR, fogColor);
			glFogf(GL_FOG_DENSITY, 0.35f);
			glHint(GL_FOG_HINT, GL_DONT_CARE);
			glFogf(GL_FOG_START, 1.0f);
			glFogf(GL_FOG_END, 5.0f);

			glClearColor(0.5f, 0.5f, 0.5f, 1.0f);										// Fog Color
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws Redbook Fog1 scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			// draws 5 spheres at different z positions
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
			RenderSphere(-2.0f, -0.5f, -1.0f);
			RenderSphere(-1.0f, -0.5f, -2.0f);
			RenderSphere(0.0f, -0.5f, -3.0f);
			RenderSphere(1.0f, -0.5f, -4.0f);
			RenderSphere(2.0f, -0.5f, -5.0f);
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

			dataRow = InputHelpDataTable.NewRow();										// F - Change Fog Mode
			dataRow["Input"] = "F";
			dataRow["Effect"] = "Change Fog Mode";
			if(fogMode == (int) GL_EXP) {
				dataRow["Current State"] = "GL_EXP";
			}
			else if(fogMode == (int) GL_EXP2) {
				dataRow["Current State"] = "GL_EXP2";
			}
			else if(fogMode == (int) GL_LINEAR) {
				dataRow["Current State"] = "GL_LINEAR";
			}
			InputHelpDataTable.Rows.Add(dataRow);
		}
		#endregion InputHelp()

		#region ProcessInput()
		/// <summary>
		/// Overrides default input handling, adding example-specific input handling.
		/// </summary>
		public override void ProcessInput() {
			base.ProcessInput();														// Handle The Default Basecode Keys

			if(KeyState[(int) Keys.F]) {												// Is F Key Being Pressed?
				KeyState[(int) Keys.F] = false;											// Mark As Handled
				if(fogMode == (int) GL_EXP) {
					fogMode = (int) GL_EXP2;
				}
				else if(fogMode == (int) GL_EXP2) {
					fogMode = (int) GL_LINEAR;
				}
				else if(fogMode == (int) GL_LINEAR) {
					fogMode = (int) GL_EXP;
				}
				glFogi(GL_FOG_MODE, fogMode);
				UpdateInputHelp();
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
			if(width <= height) {
				glOrtho(-2.5f, 2.5f, -2.5f * (float) height / (float) width, 2.5f * (float) height / (float) width, -10.0f, 10.0f);
			}
			else {
				glOrtho(-2.5f * (float) width / (float) height, 2.5f * (float) width / (float) height, -2.5f, 2.5f, -10.0f, 10.0f);
			}
			glMatrixMode(GL_MODELVIEW);
			glLoadIdentity();
		}
		#endregion Reshape(int width, int height)

		// --- Example Methods ---
		#region RenderSphere(float x, float y, float z)
		/// <summary>
		/// Renders a sphere.
		/// </summary>
		/// <param name="x">X coordinate.</param>
		/// <param name="y">Y coordinate.</param>
		/// <param name="z">Z coordinate.</param>
		private static void RenderSphere(float x, float y, float z) {
			glPushMatrix();
				glTranslatef(x, y, z);
				glutSolidSphere(0.4f, 16, 16);
			glPopMatrix();
		}
		#endregion RenderSphere(float x, float y, float z)
	}
}