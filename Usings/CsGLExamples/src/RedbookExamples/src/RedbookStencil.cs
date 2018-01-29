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
using System.Reflection;

#region AssemblyInfo
[assembly: AssemblyCompany("The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyCopyright("2002 The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyDescription("Redbook Stencil")]
[assembly: AssemblyProduct("Redbook Stencil")]
[assembly: AssemblyTitle("Redbook Stencil")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace RedbookExamples {
	/// <summary>
	/// Redbook Stencil -- Stencil Buffer (http://www.opengl.org/developers/code/examples/redbook/redbook.html)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class RedbookStencil : Model {
		// --- Fields ---
		#region Private Fields
		private const int YELLOWMAT = 1;
		private const int BLUEMAT = 2;
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Example title.
		/// </summary>
		public override string Title {
			get {
				return "Redbook Stencil -- Stencil Buffer";
			}
		}

		/// <summary>
		/// Example description.
		/// </summary>
		public override string Description {
			get {
				return "This program draws two rotated tori in a window.  A diamond in the center of the window masks out part of the scene.  Within this mask, a different model (a sphere) is drawn in a different color.";
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
			App.Run(new RedbookStencil());												// Run Our Example As A Windows Forms Application
		}
		#endregion Main()

		// --- Basecode Methods ---
		#region Initialize()
		/// <summary>
		/// Overrides OpenGL's initialization.
		/// </summary>
		public override void Initialize() {
			float[] yellow_diffuse = {0.7f, 0.7f, 0.0f, 1.0f};
			float[] yellow_specular = {1.0f, 1.0f, 1.0f, 1.0f};
			float[] blue_diffuse = {0.1f, 0.1f, 0.7f, 1.0f};
			float[] blue_specular = {0.1f, 1.0f, 1.0f, 1.0f};
			float[] position_one = {1.0f, 1.0f, 1.0f, 0.0f};

			glNewList(YELLOWMAT, GL_COMPILE);
				glMaterialfv(GL_FRONT, GL_DIFFUSE, yellow_diffuse);
				glMaterialfv(GL_FRONT, GL_SPECULAR, yellow_specular);
				glMaterialf(GL_FRONT, GL_SHININESS, 64.0f);
			glEndList();

			glNewList(BLUEMAT, GL_COMPILE);
				glMaterialfv(GL_FRONT, GL_DIFFUSE, blue_diffuse);
				glMaterialfv(GL_FRONT, GL_SPECULAR, blue_specular);
				glMaterialf(GL_FRONT, GL_SHININESS, 45.0f);
			glEndList();

			glLightfv(GL_LIGHT0, GL_POSITION, position_one);

			glEnable(GL_LIGHT0);
			glEnable(GL_LIGHTING);
			glEnable(GL_DEPTH_TEST);

			glClearStencil(0x0);
			glEnable(GL_STENCIL_TEST);
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws Redbook Stencil scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			// Draw a sphere in a diamond-shaped section in the middle of a window with 2 torii.
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

			// draw blue sphere where the stencil is 1
			glStencilFunc(GL_EQUAL, 0x1, 0x1);
			glStencilOp(GL_KEEP, GL_KEEP, GL_KEEP);
			glCallList(BLUEMAT);
			glutSolidSphere(0.5f, 15, 15);

			// draw the tori where the stencil is not 1
			glStencilFunc(GL_NOTEQUAL, 0x1, 0x1);
			glPushMatrix();
				glRotatef(45.0f, 0.0f, 0.0f, 1.0f);
				glRotatef(45.0f, 0.0f, 1.0f, 0.0f);
				glCallList(YELLOWMAT);
				glutSolidTorus(0.275f, 0.85f, 15, 15);
				glPushMatrix();
					glRotatef(90.0f, 1.0f, 0.0f, 0.0f);
					glutSolidTorus(0.275f, 0.85f, 15, 15);
				glPopMatrix();
			glPopMatrix();
		}
		#endregion Draw()

		#region Reshape(int width, int height)
		/// <summary>
		/// Overrides OpenGL reshaping.
		/// </summary>
		/// <param name="width">New width.</param>
		/// <param name="height">New height.</param>
		public override void Reshape(int width, int height) {							// Resize And Initialize The GL Window
			// Whenever the window is reshaped, redefine the coordinate system and redraw the stencil area.
			glViewport(0, 0, width, height);

			// create a diamond shaped stencil area
			glMatrixMode(GL_PROJECTION);
			glLoadIdentity();
			if(width <= height) {
				gluOrtho2D(-3.0f, 3.0f, -3.0f * (float) height / (float) width, 3.0f * (float) height / (float) width);
			}
			else {
				gluOrtho2D(-3.0f * (float) width / (float) height, 3.0f * (float) width / (float) height, -3.0f, 3.0f);
			}
			glMatrixMode(GL_MODELVIEW);
			glLoadIdentity();

			glClear(GL_STENCIL_BUFFER_BIT);
			glStencilFunc(GL_ALWAYS, 0x1, 0x1);
			glStencilOp(GL_REPLACE, GL_REPLACE, GL_REPLACE);
			glBegin(GL_QUADS);
				glVertex2f(-1.0f, 0.0f);
				glVertex2f(0.0f, 1.0f);
				glVertex2f(1.0f, 0.0f);
				glVertex2f(0.0f, -1.0f);
			glEnd();

			glMatrixMode(GL_PROJECTION);
			glLoadIdentity();
			gluPerspective(45.0f, (float) width / (float) height, 3.0f, 7.0f);
			glMatrixMode(GL_MODELVIEW);
			glLoadIdentity();
			glTranslatef(0.0f, 0.0f, -5.0f);
		}
		#endregion Reshape(int width, int height)

		#region Setup()
		/// <summary>
		/// Overrides application and OpenGL settings and setup.
		/// </summary>
		public override void Setup() {
			base.Setup();																// Run The Base Setup
			App.StencilDepth = 1;														// Setup An Stencil Bit
		}
		#endregion Setup()
	}
}