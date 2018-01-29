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
[assembly: AssemblyDescription("Redbook Alpha3D")]
[assembly: AssemblyProduct("Redbook Alpha3D")]
[assembly: AssemblyTitle("Redbook Alpha3D")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace RedbookExamples {
	/// <summary>
	/// Redbook Alpha3D -- Demonstrates Intermixing Opaque and Alpha Blended Polygons (http://www.opengl.org/developers/code/examples/redbook/redbook.html)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class RedbookAlpha3D : Model {
		// --- Fields ---
		#region Private Fields
		private const float MAXZ = 8.0f;
		private const float MINZ = -8.0f;
		private const float ZINC = 0.4f;
		private static float solidZ = MAXZ;
		private static float transparentZ = MINZ;
		private static uint sphereList, cubeList;

		private static float[] mat_specular = {1.0f, 1.0f, 1.0f, 0.15f};
		private static float[] mat_shininess = {100.0f};
		private static float[] position = {0.5f, 0.5f, 1.0f, 0.0f};

		private static float[] mat_solid = {0.75f, 0.75f, 0.0f, 1.0f};
		private static float[] mat_zero = {0.0f, 0.0f, 0.0f, 1.0f};
		private static float[] mat_transparent = {0.0f, 0.8f, 0.8f, 0.6f};
		private static float[] mat_emission = {0.0f, 0.3f, 0.3f, 0.6f};
		
		private static bool animating = false;
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Example title.
		/// </summary>
		public override string Title {
			get {
				return "Redbook Alpha3D -- Demonstrates Intermixing Opaque and Alpha Blended Polygons";
			}
		}

		/// <summary>
		/// Example description.
		/// </summary>
		public override string Description {
			get {
				return "This example demonstrates how to intermix opaque and alpha blended polygons in the same scene, by using glDepthMask.";
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
			App.Run(new RedbookAlpha3D());												// Run Our Example As A Windows Forms Application
		}
		#endregion Main()

		// --- Basecode Methods ---
		#region Initialize()
		/// <summary>
		/// Overrides OpenGL's initialization.
		/// </summary>
		public override void Initialize() {
			glMaterialfv(GL_FRONT, GL_SPECULAR, mat_specular);
			glMaterialfv(GL_FRONT, GL_SHININESS, mat_shininess);
			glLightfv(GL_LIGHT0, GL_POSITION, position);

			glEnable(GL_LIGHTING);
			glEnable(GL_LIGHT0);
			glEnable(GL_DEPTH_TEST);

			sphereList = glGenLists(1);
			glNewList(sphereList, GL_COMPILE);
				glutSolidSphere(0.4f, 16, 16);
			glEndList();

			cubeList = glGenLists(1);
			glNewList(cubeList, GL_COMPILE);
				glutSolidCube(0.6f);
			glEndList();
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws Redbook Alpha3D scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

			glPushMatrix();
				glTranslatef(-0.15f, -0.15f, solidZ);
				glMaterialfv(GL_FRONT, GL_EMISSION, mat_zero);
				glMaterialfv(GL_FRONT, GL_DIFFUSE, mat_solid);
				glCallList(sphereList);
			glPopMatrix();

			glPushMatrix();
				glTranslatef(0.15f, 0.15f, transparentZ);
				glRotatef(15.0f, 1.0f, 1.0f, 0.0f);
				glRotatef(30.0f, 0.0f, 1.0f, 0.0f);
				glMaterialfv(GL_FRONT, GL_EMISSION, mat_emission);
				glMaterialfv(GL_FRONT, GL_DIFFUSE, mat_transparent);
				glEnable(GL_BLEND);
				glDepthMask((byte) GL_FALSE);
				glBlendFunc(GL_SRC_ALPHA, GL_ONE);
				glCallList(cubeList);
				glDepthMask((byte) GL_TRUE);
				glDisable(GL_BLEND);
			glPopMatrix();

			if(animating) {
				if(solidZ <= MINZ || transparentZ >= MAXZ) {
					animating = false;
				}
				else {
					solidZ -= ZINC;
					transparentZ += ZINC;
				}
			}
		}
		#endregion Draw()

		#region InputHelp()
		/// <summary>
		/// Overrides default input help, supplying example-specific help information.
		/// </summary>
		public override void InputHelp() {
			base.InputHelp();															// Set Up The Default Input Help

			DataRow dataRow;															// Row To Add

			dataRow = InputHelpDataTable.NewRow();										// A - Animate
			dataRow["Input"] = "A";
			dataRow["Effect"] = "Animate";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// R - Reset
			dataRow["Input"] = "R";
			dataRow["Effect"] = "Reset";
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

			if(KeyState[(int) Keys.A]) {												// Is A Key Being Pressed?
				KeyState[(int) Keys.A] = false;											// Mark As Handled
				solidZ = MAXZ;															// Animate
				transparentZ = MINZ;
				animating = true;
			}

			if(KeyState[(int) Keys.R]) {												// Is R Key Being Pressed?
				KeyState[(int) Keys.R] = false;											// Mark As Handled
				solidZ = MAXZ;															// Reset
				transparentZ = MINZ;
				animating = false;
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
				glOrtho(-1.5f, 1.5f, -1.5f * (float) height / (float) width, 1.5f * (float) height / (float) width, -10.0f, 10.0f);
			}
			else {
				glOrtho(-1.5f * (float) width / (float) height, 1.5f * (float) width / (float) height, -1.5f, 1.5f, -10.0f, 10.0f);
			}
			glMatrixMode(GL_MODELVIEW);
			glLoadIdentity();
		}
		#endregion Reshape(int width, int height)
	}
}