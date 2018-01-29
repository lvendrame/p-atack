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
/* Copyright (c) Mark J. Kilgard, 1994. */

/*
 * (c) Copyright 1993, Silicon Graphics, Inc.
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
 * OpenGL(TM) is a trademark of Silicon Graphics, Inc.
 */
#endregion Original Credits / License

using CsGL.Basecode;
using CsGL.OpenGL;
using System.Reflection;

#region AssemblyInfo
[assembly: AssemblyCompany("The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyCopyright("2002 The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyDescription("Redbook Nurbs")]
[assembly: AssemblyProduct("Redbook Nurbs")]
[assembly: AssemblyTitle("Redbook Nurbs")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace RedbookExamples {
	/// <summary>
	/// Redbook Nurbs -- NURBS (http://www.opengl.org/developers/code/examples/redbook/redbook.html)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class RedbookNurbs : Model {
		// --- Fields ---
		#region Private Fields
		private const int S_NUMPOINTS = 13;
		private const int S_ORDER = 3;
		private const int S_NUMKNOTS = (S_NUMPOINTS + S_ORDER);
		private const int T_NUMPOINTS = 3;
		private const int T_ORDER = 3;
		private const int T_NUMKNOTS = (T_NUMPOINTS + T_ORDER);
		private const float SQRT2 = 1.41421356237309504880f;

		private static float[] sknots = {
			-1.0f, -1.0f, -1.0f, 0.0f, 1.0f, 2.0f, 3.0f, 4.0f,
			4.0f,  5.0f,  6.0f, 7.0f, 8.0f, 9.0f, 9.0f, 9.0f
		};

		private static float[] tknots = {1.0f, 1.0f, 1.0f, 2.0f, 2.0f, 2.0f};

		private static float[] pointdata = new float[S_NUMPOINTS * T_NUMPOINTS * 4];
		private static GLUnurbs theNurb;
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Example title.
		/// </summary>
		public override string Title {
			get {
				return "Redbook Nurbs -- NURBS";
			}
		}

		/// <summary>
		/// Example description.
		/// </summary>
		public override string Description {
			get {
				return "This program shows a NURBS (Non-uniform rational B-splines) surface, shaped like a heart.";
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
			App.Run(new RedbookNurbs());												// Run Our Example As A Windows Forms Application
		}
		#endregion Main()

		// --- Basecode Methods ---
		#region Initialize()
		/// <summary>
		/// Overrides OpenGL's initialization.
		/// </summary>
		public override void Initialize() {
			// Initialize material property, light source, lighting model, and depth buffer
			float[] mat_ambient = {1.0f, 1.0f, 1.0f, 1.0f};
			float[] mat_diffuse = {1.0f, 0.2f, 1.0f, 1.0f};
			float[] mat_specular = {1.0f, 1.0f, 1.0f, 1.0f};
			float[] mat_shininess = {50.0f};

			float[] light0_position = {1.0f, 0.1f, 1.0f, 0.0f};
			float[] light1_position = {-1.0f, 0.1f, 1.0f, 0.0f};

			float[] lmodel_ambient = {0.3f, 0.3f, 0.3f, 1.0f};

			glMaterialfv(GL_FRONT, GL_AMBIENT, mat_ambient);
			glMaterialfv(GL_FRONT, GL_DIFFUSE, mat_diffuse);
			glMaterialfv(GL_FRONT, GL_SPECULAR, mat_specular);
			glMaterialfv(GL_FRONT, GL_SHININESS, mat_shininess);
			glLightfv(GL_LIGHT0, GL_POSITION, light0_position);
			glLightfv(GL_LIGHT1, GL_POSITION, light1_position);
			glLightModelfv(GL_LIGHT_MODEL_AMBIENT, lmodel_ambient);

			glEnable(GL_LIGHTING);
			glEnable(GL_LIGHT0);
			glEnable(GL_LIGHT1);
			glDepthFunc(GL_LESS);
			glEnable(GL_DEPTH_TEST);
			glEnable(GL_AUTO_NORMAL);

			theNurb = gluNewNurbsRenderer();

			gluNurbsProperty(theNurb, GLU_SAMPLING_TOLERANCE, 25.0f);
			gluNurbsProperty(theNurb, GLU_DISPLAY_MODE, GLU_FILL);

			float[/*S_NUMPOINTS*/, /*T_NUMPOINTS*/, /*4*/] ctlpoints = {
				{
					{4.0f, 2.0f, 2.0f, 1.0f},
					{4.0f, 1.6f, 2.5f, 1.0f},
					{4.0f, 2.0f, 3.0f, 1.0f}
				},
				{
					{5.0f, 4.0f, 2.0f, 1.0f},
					{5.0f, 4.0f, 2.5f, 1.0f},
					{5.0f, 4.0f, 3.0f, 1.0f}
				},
				{
					{6.0f, 5.0f, 2.0f, 1.0f},
					{6.0f, 5.0f, 2.5f, 1.0f},
					{6.0f, 5.0f, 3.0f, 1.0f}
				},
				{
					{SQRT2 * 6.0f, SQRT2 * 6.0f, SQRT2 * 2.0f, SQRT2},
					{SQRT2 * 6.0f, SQRT2 * 6.0f, SQRT2 * 2.5f, SQRT2},
					{SQRT2 * 6.0f, SQRT2 * 6.0f, SQRT2 * 3.0f, SQRT2}
				},
				{
					{5.2f, 6.7f, 2.0f, 1.0f},
					{5.2f, 6.7f, 2.5f, 1.0f},
					{5.2f, 6.7f, 3.0f, 1.0f}
				},
				{
					{SQRT2 * 4.0f, SQRT2 * 6.0f, SQRT2 * 2.0f, SQRT2},
					{SQRT2 * 4.0f, SQRT2 * 6.0f, SQRT2 * 2.5f, SQRT2},
					{SQRT2 * 4.0f, SQRT2 * 6.0f, SQRT2 * 3.0f, SQRT2}
				},
				{
					{4.0f, 5.2f, 2.0f, 1.0f},
					{4.0f, 4.6f, 2.5f, 1.0f},
					{4.0f, 5.2f, 3.0f, 1.0f}
				},
				{
					{SQRT2 * 4.0f, SQRT2 * 6.0f, SQRT2 * 2.0f, SQRT2},
					{SQRT2 * 4.0f, SQRT2 * 6.0f, SQRT2 * 2.5f, SQRT2},
					{SQRT2 * 4.0f, SQRT2 * 6.0f, SQRT2 * 3.0f, SQRT2}
				},
				{
					{2.8f, 6.7f, 2.0f, 1.0f},
					{2.8f, 6.7f, 2.5f, 1.0f},
					{2.8f, 6.7f, 3.0f, 1.0f}
				},
				{
					{SQRT2 * 2.0f, SQRT2 * 6.0f, SQRT2 * 2.0f, SQRT2},
					{SQRT2 * 2.0f, SQRT2 * 6.0f, SQRT2 * 2.5f, SQRT2},
					{SQRT2 * 2.0f, SQRT2 * 6.0f, SQRT2 * 3.0f, SQRT2}
				},
				{
					{2.0f, 5.0f, 2.0f, 1.0f},
					{2.0f, 5.0f, 2.5f, 1.0f},
					{2.0f, 5.0f, 3.0f, 1.0f}
				},
				{
					{3.0f, 4.0f, 2.0f, 1.0f},
					{3.0f, 4.0f, 2.5f, 1.0f},
					{3.0f, 4.0f, 3.0f, 1.0f}
				},
				{
					{4.0f, 2.0f, 2.0f, 1.0f},
					{4.0f, 1.6f, 2.5f, 1.0f},
					{4.0f, 2.0f, 3.0f, 1.0f}
				}
			};

			int cnt = 0;
			for(int i = 0; i < S_NUMPOINTS; i++) {
				for(int j = 0; j < T_NUMPOINTS; j++) {
					for(int k = 0; k < 4; k++) {
						pointdata[cnt] = ctlpoints[i, j, k];
						cnt++;
					}
				}
			}
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws Redbook Nurbs scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

			glPushMatrix();
				glTranslatef(4.0f, 4.5f, 2.5f);
				glRotatef(220.0f, 1.0f, 0.0f, 0.0f);
				glRotatef(115.0f, 0.0f, 1.0f, 0.0f);
				glTranslatef(-4.0f, -4.5f, -2.5f);

				gluBeginSurface(theNurb);
					gluNurbsSurface(theNurb, S_NUMKNOTS, sknots, T_NUMKNOTS, tknots, 4 * T_NUMPOINTS, 4, pointdata, S_ORDER, T_ORDER, GL_MAP2_VERTEX_4);
				gluEndSurface(theNurb);
			glPopMatrix();
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
			glMatrixMode(GL_PROJECTION);
			glLoadIdentity();
			glFrustum(-1.0f, 1.0f, -1.5f, 0.5f, 0.8f, 10.0f);

			glMatrixMode(GL_MODELVIEW);
			glLoadIdentity();
			gluLookAt(7.0f, 4.5f, 4.0f, 4.5f, 4.5f, 2.0f, 6.0f, -3.0f, 2.0f);
		}
		#endregion Reshape(int width, int height)
	}
}