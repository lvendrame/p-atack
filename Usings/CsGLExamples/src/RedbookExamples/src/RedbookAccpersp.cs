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
[assembly: AssemblyDescription("Redbook Accpersp")]
[assembly: AssemblyProduct("Redbook Accpersp")]
[assembly: AssemblyTitle("Redbook Accpersp")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace RedbookExamples {
	/// <summary>
	/// Redbook Accpersp -- Full Scene Anti-Aliasing (http://www.opengl.org/developers/code/examples/redbook/redbook.html)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class RedbookAccpersp : Model {
		// --- Fields ---
		#region Private Fields
		private const float PI_ = 3.14159265358979323846f;
		private const int ACSIZE = 8;
		private static float[] mat_ambient = {1.0f, 1.0f, 1.0f, 1.0f};
		private static float[] mat_specular = {1.0f, 1.0f, 1.0f, 1.0f};
		private static float[] light_position = {0.0f, 0.0f, 10.0f, 1.0f};
		private static float[] lm_ambient = {0.2f, 0.2f, 0.2f, 1.0f};
		private static float[] torus_diffuse = {0.7f, 0.7f, 0.0f, 1.0f};
		private static float[] cube_diffuse = {0.0f, 0.7f, 0.7f, 1.0f};
		private static float[] sphere_diffuse = {0.7f, 0.0f, 0.7f, 1.0f};
		private static float[] octa_diffuse = {0.7f, 0.4f, 0.4f, 1.0f};
		private static int[] viewport = new int[4];
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Example title.
		/// </summary>
		public override string Title {
			get {
				return "Redbook Accpersp -- Full Scene Anti-Aliasing";
			}
		}

		/// <summary>
		/// Example description.
		/// </summary>
		public override string Description {
			get {
				return "Use the accumulation buffer to do full-scene antialiasing on a scene with perspective projection, using the special routines accFrustum() and accPerspective().";
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
			App.Run(new RedbookAccpersp());												// Run Our Example As A Windows Forms Application
		}
		#endregion Main()

		// --- Basecode Methods ---
		#region Initialize()
		/// <summary>
		/// Overrides OpenGL's initialization.
		/// </summary>
		public override void Initialize() {
			glMaterialfv(GL_FRONT, GL_AMBIENT, mat_ambient);
			glMaterialfv(GL_FRONT, GL_SPECULAR, mat_specular);
			glMaterialf(GL_FRONT, GL_SHININESS, 50.0f);
			glLightfv(GL_LIGHT0, GL_POSITION, light_position);
			glLightModelfv(GL_LIGHT_MODEL_AMBIENT, lm_ambient);

			glEnable(GL_LIGHTING);
			glEnable(GL_LIGHT0);
			glEnable(GL_DEPTH_TEST);
			glShadeModel(GL_FLAT);

			glClearColor(0.0f, 0.0f, 0.0f, 0.0f);
			glClearAccum(0.0f, 0.0f, 0.0f, 0.0f);
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws Redbook Accpersp scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			glGetIntegerv(GL_VIEWPORT, viewport);

			glClear(GL_ACCUM_BUFFER_BIT);
			for(int jitter = 0; jitter < ACSIZE; jitter++) {
				glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
				AccPerspective(50.0, (double) viewport[2] / (double) viewport[3], 1.0, 15.0, Jitter.j8[jitter].x, Jitter.j8[jitter].y, 0.0, 0.0, 1.0);
				DisplayObjects();
				glAccum(GL_ACCUM, 1.0f / ACSIZE);
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

		// --- Lesson Methods ---
		#region AccFrustum(double left, double right, double bottom, double top, double near, double far, double pixdx, double pixdy, double eyedx, double eyedy, double focus)
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
		#endregion AccFrustum(double left, double right, double bottom, double top, double near, double far, double pixdx, double pixdy, double eyedx, double eyedy, double focus)

		#region AccPerspective(double fovy, double aspect, double near, double far, double pixdx, double pixdy, double eyedx, double eyedy, double focus)
		/* AccPerspective()
		* 
		* The first 4 arguments are identical to the gluPerspective() call.
		* pixdx and pixdy are anti-alias jitter in pixels. 
		* Set both equal to 0.0 for no anti-alias jitter.
		* eyedx and eyedy are depth-of field jitter in pixels. 
		* Set both equal to 0.0 for no depth of field effects.
		*
		* focus is distance from eye to plane in focus. 
		* focus must be greater than, but not equal to 0.0.
		*
		* Note that accPerspective() calls accFrustum().
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
		#endregion AccPerspective(double fovy, double aspect, double near, double far, double pixdx, double pixdy, double eyedx, double eyedy, double focus)

		#region DisplayObjects()
		/// <summary>
		/// Draws the objects.
		/// </summary>
		private void DisplayObjects() {
			glPushMatrix();
				glTranslatef(0.0f, 0.0f, -5.0f);
				glRotatef(30.0f, 1.0f, 0.0f, 0.0f);

				glPushMatrix();
					glTranslatef(-0.80f, 0.35f, 0.0f);
					glRotatef(100.0f, 1.0f, 0.0f, 0.0f);
					glMaterialfv(GL_FRONT, GL_DIFFUSE, torus_diffuse);
					glutSolidTorus(0.275f, 0.85f, 16, 16);
				glPopMatrix();

				glPushMatrix();
					glTranslatef(-0.75f, -0.50f, 0.0f);
					glRotatef(45.0f, 0.0f, 0.0f, 1.0f);
					glRotatef(45.0f, 1.0f, 0.0f, 0.0f);
					glMaterialfv(GL_FRONT, GL_DIFFUSE, cube_diffuse);
					glutSolidCube(1.5f);
				glPopMatrix();

				glPushMatrix();
					glTranslatef(0.75f, 0.60f, 0.0f);
					glRotatef(30.0f, 1.0f, 0.0f, 0.0f);
					glMaterialfv(GL_FRONT, GL_DIFFUSE, sphere_diffuse);
					glutSolidSphere(1.0f, 16, 16);
				glPopMatrix();

				glPushMatrix();
					glTranslatef(0.70f, -0.90f, 0.25f);
					glMaterialfv(GL_FRONT, GL_DIFFUSE, octa_diffuse);
					glutSolidOctahedron();
				glPopMatrix();
			glPopMatrix();
		}
		#endregion DisplayObjects()
	}
}