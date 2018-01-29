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
using System.Reflection;

#region AssemblyInfo
[assembly: AssemblyCompany("The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyCopyright("2002 The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyDescription("Redbook Dof")]
[assembly: AssemblyProduct("Redbook Dof")]
[assembly: AssemblyTitle("Redbook Dof")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace RedbookExamples {
	/// <summary>
	/// Redbook Dof -- Demonstrates Depth Of Field Effect (http://www.opengl.org/developers/code/examples/redbook/redbook.html)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class RedbookDof : Model {
		// --- Fields ---
		#region Private Fields
		private const float PI_ = 3.14159265358979323846f;
		private static uint teapotList;
		private static int[] viewport = new int[4];
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Example title.
		/// </summary>
		public override string Title {
			get {
				return "Redbook Dof -- Demonstrates Depth Of Field Effect";
			}
		}

		/// <summary>
		/// Example description.
		/// </summary>
		public override string Description {
			get {
				return "This program demonstrates use of the accumulation buffer to create an out-of-focus depth-of-field effect.  The teapots are drawn several times into the accumulation buffer.  The viewing volume is jittered, except at the focal point, where the viewing volume is at the same position, each time.  In this case, the gold teapot remains in focus.";
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
			App.Run(new RedbookDof());													// Run Our Example As A Windows Forms Application
		}
		#endregion Main()

		// --- Basecode Methods ---
		#region Initialize()
		/// <summary>
		/// Overrides OpenGL's initialization.
		/// </summary>
		public override void Initialize() {
			float[] ambient = {0.0f, 0.0f, 0.0f, 1.0f};
			float[] diffuse = {1.0f, 1.0f, 1.0f, 1.0f};
			float[] specular = {1.0f, 1.0f, 1.0f, 1.0f};
			float[] position = {0.0f, 3.0f, 3.0f, 0.0f};

			float[] lmodel_ambient = {0.2f, 0.2f, 0.2f, 1.0f};
			float[] local_view = {0.0f};

			glLightfv(GL_LIGHT0, GL_AMBIENT, ambient);
			glLightfv(GL_LIGHT0, GL_DIFFUSE, diffuse);
			glLightfv(GL_LIGHT0, GL_POSITION, position);

			glLightModelfv(GL_LIGHT_MODEL_AMBIENT, lmodel_ambient);
			glLightModelfv(GL_LIGHT_MODEL_LOCAL_VIEWER, local_view);

			glFrontFace(GL_CW);
			glEnable(GL_LIGHTING);
			glEnable(GL_LIGHT0);
			glEnable(GL_AUTO_NORMAL);
			glEnable(GL_NORMALIZE);
			glEnable(GL_DEPTH_TEST);

			glClearColor(0.0f, 0.0f, 0.0f, 0.0f);
			glClearAccum(0.0f, 0.0f, 0.0f, 0.0f); 
			// Make Teapot Display List
			teapotList = glGenLists(1);
			glNewList(teapotList, GL_COMPILE);
				glutSolidTeapot(0.5f);
			glEndList();
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws Redbook Dof scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			// draws 5 teapots into the accumulation buffer 
			// several times; each time with a jittered perspective.
			// The focal point is at z = 5.0, so the gold teapot will 
			// stay in focus.  The amount of jitter is adjusted by the
			// magnitude of the accPerspective() jitter; in this example, 0.33.
			// In this example, the teapots are drawn 8 times.  See jitter.cs

			glGetIntegerv(GL_VIEWPORT, viewport);
			glClear(GL_ACCUM_BUFFER_BIT);

			for(int jitter = 0; jitter < 8; jitter++) {
				glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
				AccPerspective(45.0, (double) viewport[2] / (double) viewport[3], 1.0, 15.0, 0.0, 0.0, 0.33 * Jitter.j8[jitter].x, 0.33 * Jitter.j8[jitter].y, 5.0);

				// ruby, gold, silver, emerald, and cyan teapots
				RenderTeapot(-1.1f, -0.5f, -4.5f, 0.1745f, 0.01175f, 0.01175f, 0.61424f, 0.04136f, 0.04136f, 0.727811f, 0.626959f, 0.626959f, 0.6f);
				RenderTeapot(-0.5f, -0.5f, -5.0f, 0.24725f, 0.1995f, 0.0745f, 0.75164f, 0.60648f, 0.22648f, 0.628281f, 0.555802f, 0.366065f, 0.4f);
				RenderTeapot(0.2f, -0.5f, -5.5f, 0.19225f, 0.19225f, 0.19225f, 0.50754f, 0.50754f, 0.50754f, 0.508273f, 0.508273f, 0.508273f, 0.4f);
				RenderTeapot(1.0f, -0.5f, -6.0f, 0.0215f, 0.1745f, 0.0215f, 0.07568f, 0.61424f, 0.07568f, 0.633f, 0.727811f, 0.633f, 0.6f);
				RenderTeapot(1.8f, -0.5f, -6.5f, 0.0f, 0.1f, 0.06f, 0.0f, 0.50980392f, 0.50980392f, 0.50196078f, 0.50196078f, 0.50196078f, 0.25f);
				glAccum(GL_ACCUM, 0.125f);
			}
			glAccum(GL_RETURN, 1.0f);
			glFlush();
		}
		#endregion Draw()

		#region Reshape(int width, int height)
		/// <summary>
		/// Overrides OpenGL reshaping.
		/// </summary>
		/// <param name="width">New width.</param>
		/// <param name="height">New height.</param>
		public override void Reshape(int width, int height) {							// Resize And Initialize The GL Window
			glViewport(0, 0, width, height);
		}
		#endregion Reshape(int width, int height)

		#region Setup()
		/// <summary>
		/// Overrides application and OpenGL settings and setup.
		/// </summary>
		public override void Setup() {
			base.Setup();																// Run The Base Setup
			App.AccumDepth = 32;														// Setup An Accum Bit
		}
		#endregion Setup()

		// --- Example Methods ---
		/* AccFrustum()
		* The first 6 arguments are identical to the glFrustum() call.
		*  
		* pixdx and pixdy are anti-alias jitter in pixels. 
		* Set both equal to 0.0 for no anti-alias jitter.
		* eyedx and eyedy are depth-of field jitter in pixels. 
		* Set both equal to 0.0 for no depth of field effects.
		*
		* focus is distance from eye to plane in focus. 
		* focus must be greater than, but not equal to 0.0.
		*
		* Note that accFrustum() calls glTranslatef().  You will 
		* probably want to insure that your ModelView matrix has been 
		* initialized to identity before calling accFrustum().
		*/
		private static void AccFrustum(double left, double right, double bottom, double top, double near, double far, double pixdx, double pixdy, double eyedx, double eyedy, double focus) {
			double xwsize, ywsize; 
			double dx, dy;
			int[] viewport = new int[4];

			glGetIntegerv(GL_VIEWPORT, viewport);
	
			xwsize = right - left;
			ywsize = top - bottom;
	
			dx = -(pixdx * xwsize / (double) viewport[2] + eyedx * near / focus);
			dy = -(pixdy * ywsize / (double) viewport[3] + eyedy * near / focus);
	
			glMatrixMode(GL_PROJECTION);
			glLoadIdentity();
			glFrustum(left + dx, right + dx, bottom + dy, top + dy, near, far);
			glMatrixMode(GL_MODELVIEW);
			glLoadIdentity();
			glTranslatef((float) -eyedx, (float) -eyedy, 0.0f);
		}

		/*  AccPerspective()
		* 
		*  The first 4 arguments are identical to the gluPerspective() call.
		*  pixdx and pixdy are anti-alias jitter in pixels. 
		*  Set both equal to 0.0 for no anti-alias jitter.
		*  eyedx and eyedy are depth-of field jitter in pixels. 
		*  Set both equal to 0.0 for no depth of field effects.
		*
		*  focus is distance from eye to plane in focus. 
		*  focus must be greater than, but not equal to 0.0.
		*
		*  Note that accPerspective() calls accFrustum().
		*/
		private static void AccPerspective(double fovy, double aspect, double near, double far, double pixdx, double pixdy, double eyedx, double eyedy, double focus) {
			double fov2, left, right, bottom, top;

			fov2 = ((fovy * PI_) / 180.0) / 2.0;

			top = near / (Math.Cos(fov2) / Math.Sin(fov2));
			bottom = -top;

			right = top * aspect;
			left = -right;

			AccFrustum(left, right, bottom, top, near, far, pixdx, pixdy, eyedx, eyedy, focus);
		}

		private void RenderTeapot(float x, float y, float z, float ambr, float ambg, float ambb, float difr, float difg, float difb, float specr, float specg, float specb, float shine) {
			float[] mat = new float[4];

			glPushMatrix();
			glTranslatef(x, y, z);
			mat[0] = ambr; mat[1] = ambg; mat[2] = ambb; mat[3] = 1.0f;
			glMaterialfv(GL_FRONT, GL_AMBIENT, mat);
			mat[0] = difr; mat[1] = difg; mat[2] = difb;
			glMaterialfv(GL_FRONT, GL_DIFFUSE, mat);
			mat[0] = specr; mat[1] = specg; mat[2] = specb;
			glMaterialfv(GL_FRONT, GL_SPECULAR, mat);
			glMaterialf(GL_FRONT, GL_SHININESS, shine * 128.0f);
			glCallList(teapotList);
			glPopMatrix();
		}

	}
}