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
[assembly: AssemblyDescription("Redbook Material")]
[assembly: AssemblyProduct("Redbook Material")]
[assembly: AssemblyTitle("Redbook Material")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace RedbookExamples {
	/// <summary>
	/// Redbook Material -- Lighting Model (http://www.opengl.org/developers/code/examples/redbook/redbook.html)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class RedbookMaterial : Model {
		// --- Fields ---
		#region Private Fields
		private static float[] no_mat = {0.0f, 0.0f, 0.0f, 1.0f};
		private static float[] mat_ambient = {0.7f, 0.7f, 0.7f, 1.0f};
		private static float[] mat_ambient_color = {0.8f, 0.8f, 0.2f, 1.0f};
		private static float[] mat_diffuse = {0.1f, 0.5f, 0.8f, 1.0f};
		private static float[] mat_specular = {1.0f, 1.0f, 1.0f, 1.0f};
		private static float[] no_shininess = {0.0f};
		private static float[] low_shininess = {5.0f};
		private static float[] high_shininess = {100.0f};
		private static float[] mat_emission = {0.3f, 0.2f, 0.2f, 0.0f};
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Example title.
		/// </summary>
		public override string Title {
			get {
				return "Redbook Material -- Lighting Model";
			}
		}

		/// <summary>
		/// Example description.
		/// </summary>
		public override string Description {
			get {
				return "This program demonstrates the use of the GL lighting model. Several objects are drawn using different material characteristics. A single light source illuminates the objects.";
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
			App.Run(new RedbookMaterial());												// Run Our Example As A Windows Forms Application
		}
		#endregion Main()

		// --- Basecode Methods ---
		#region Initialize()
		/// <summary>
		/// Overrides OpenGL's initialization.
		/// </summary>
		public override void Initialize() {
			// Initialize Z-buffer, projection matrix, light source, and lighting model.
			// Do not specify a material property here.
			float[] ambient = {0.0f, 0.0f, 0.0f, 1.0f};
			float[] diffuse = {1.0f, 1.0f, 1.0f, 1.0f};
			float[] specular = {1.0f, 1.0f, 1.0f, 1.0f};
			float[] position = {0.0f, 3.0f, 2.0f, 0.0f};
			float[] lmodel_ambient = {0.4f, 0.4f, 0.4f, 1.0f};
			float[] local_view = {0.0f};

			glClearColor(0.0f, 0.1f, 0.1f, 0.0f);
			glEnable(GL_DEPTH_TEST);
			glShadeModel(GL_SMOOTH);

			glLightfv(GL_LIGHT0, GL_AMBIENT, ambient);
			glLightfv(GL_LIGHT0, GL_DIFFUSE, diffuse);
			glLightfv(GL_LIGHT0, GL_POSITION, position);
			glLightModelfv(GL_LIGHT_MODEL_AMBIENT, lmodel_ambient);
			glLightModelfv(GL_LIGHT_MODEL_LOCAL_VIEWER, local_view);

			glEnable(GL_LIGHTING);
			glEnable(GL_LIGHT0);
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws Redbook Material scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			/*  Draw twelve spheres in 3 rows with 4 columns.  
			*   The spheres in the first row have materials with no ambient reflection.
			*   The second row has materials with significant ambient reflection.
			*   The third row has materials with colored ambient reflection.
			*
			*   The first column has materials with blue, diffuse reflection only.
			*   The second column has blue diffuse reflection, as well as specular
			*   reflection with a low shininess exponent.
			*   The third column has blue diffuse reflection, as well as specular
			*   reflection with a high shininess exponent (a more concentrated highlight).
			*   The fourth column has materials which also include an emissive component.
			*
			*   glTranslatef() is used to move spheres to their appropriate locations.
			*/

			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

			// draw sphere in first row, first column diffuse reflection only; no ambient or specular
			glPushMatrix();
				glTranslatef(-3.75f, 3.0f, 0.0f);
				glMaterialfv(GL_FRONT, GL_AMBIENT, no_mat);
				glMaterialfv(GL_FRONT, GL_DIFFUSE, mat_diffuse);
				glMaterialfv(GL_FRONT, GL_SPECULAR, no_mat);
				glMaterialfv(GL_FRONT, GL_SHININESS, no_shininess);
				glMaterialfv(GL_FRONT, GL_EMISSION, no_mat);
				glutSolidSphere(1.0f, 16, 16);
			glPopMatrix();

			// draw sphere in first row, second column diffuse and specular reflection; low shininess; no ambient
			glPushMatrix();
				glTranslatef(-1.25f, 3.0f, 0.0f);
				glMaterialfv(GL_FRONT, GL_AMBIENT, no_mat);
				glMaterialfv(GL_FRONT, GL_DIFFUSE, mat_diffuse);
				glMaterialfv(GL_FRONT, GL_SPECULAR, mat_specular);
				glMaterialfv(GL_FRONT, GL_SHININESS, low_shininess);
				glMaterialfv(GL_FRONT, GL_EMISSION, no_mat);
				glutSolidSphere(1.0f, 16, 16);
			glPopMatrix();

			// draw sphere in first row, third column diffuse and specular reflection; high shininess; no ambient
			glPushMatrix();
				glTranslatef(1.25f, 3.0f, 0.0f);
				glMaterialfv(GL_FRONT, GL_AMBIENT, no_mat);
				glMaterialfv(GL_FRONT, GL_DIFFUSE, mat_diffuse);
				glMaterialfv(GL_FRONT, GL_SPECULAR, mat_specular);
				glMaterialfv(GL_FRONT, GL_SHININESS, high_shininess);
				glMaterialfv(GL_FRONT, GL_EMISSION, no_mat);
				glutSolidSphere(1.0f, 16, 16);
			glPopMatrix();

			// draw sphere in first row, fourth column diffuse reflection; emission; no ambient or specular reflection
			glPushMatrix();
				glTranslatef(3.75f, 3.0f, 0.0f);
				glMaterialfv(GL_FRONT, GL_AMBIENT, no_mat);
				glMaterialfv(GL_FRONT, GL_DIFFUSE, mat_diffuse);
				glMaterialfv(GL_FRONT, GL_SPECULAR, no_mat);
				glMaterialfv(GL_FRONT, GL_SHININESS, no_shininess);
				glMaterialfv(GL_FRONT, GL_EMISSION, mat_emission);
				glutSolidSphere(1.0f, 16, 16);
			glPopMatrix();

			// draw sphere in second row, first column ambient and diffuse reflection; no specular
			glPushMatrix();
				glTranslatef(-3.75f, 0.0f, 0.0f);
				glMaterialfv(GL_FRONT, GL_AMBIENT, mat_ambient);
				glMaterialfv(GL_FRONT, GL_DIFFUSE, mat_diffuse);
				glMaterialfv(GL_FRONT, GL_SPECULAR, no_mat);
				glMaterialfv(GL_FRONT, GL_SHININESS, no_shininess);
				glMaterialfv(GL_FRONT, GL_EMISSION, no_mat);
				glutSolidSphere(1.0f, 16, 16);
			glPopMatrix();

			// draw sphere in second row, second column ambient, diffuse and specular reflection; low shininess
			glPushMatrix();
				glTranslatef(-1.25f, 0.0f, 0.0f);
				glMaterialfv(GL_FRONT, GL_AMBIENT, mat_ambient);
				glMaterialfv(GL_FRONT, GL_DIFFUSE, mat_diffuse);
				glMaterialfv(GL_FRONT, GL_SPECULAR, mat_specular);
				glMaterialfv(GL_FRONT, GL_SHININESS, low_shininess);
				glMaterialfv(GL_FRONT, GL_EMISSION, no_mat);
				glutSolidSphere(1.0f, 16, 16);
			glPopMatrix();

			// draw sphere in second row, third column ambient, diffuse and specular reflection; high shininess
			glPushMatrix();
			glTranslatef(1.25f, 0.0f, 0.0f);
				glMaterialfv(GL_FRONT, GL_AMBIENT, mat_ambient);
				glMaterialfv(GL_FRONT, GL_DIFFUSE, mat_diffuse);
				glMaterialfv(GL_FRONT, GL_SPECULAR, mat_specular);
				glMaterialfv(GL_FRONT, GL_SHININESS, high_shininess);
				glMaterialfv(GL_FRONT, GL_EMISSION, no_mat);
				glutSolidSphere(1.0f, 16, 16);
			glPopMatrix();

			// draw sphere in second row, fourth column ambient and diffuse reflection; emission; no specular
			glPushMatrix();
				glTranslatef(3.75f, 0.0f, 0.0f);
				glMaterialfv(GL_FRONT, GL_AMBIENT, mat_ambient);
				glMaterialfv(GL_FRONT, GL_DIFFUSE, mat_diffuse);
				glMaterialfv(GL_FRONT, GL_SPECULAR, no_mat);
				glMaterialfv(GL_FRONT, GL_SHININESS, no_shininess);
				glMaterialfv(GL_FRONT, GL_EMISSION, mat_emission);
				glutSolidSphere(1.0f, 16, 16);
			glPopMatrix();

			// draw sphere in third row, first column colored ambient and diffuse reflection; no specular
			glPushMatrix();
				glTranslatef(-3.75f, -3.0f, 0.0f);
				glMaterialfv(GL_FRONT, GL_AMBIENT, mat_ambient_color);
				glMaterialfv(GL_FRONT, GL_DIFFUSE, mat_diffuse);
				glMaterialfv(GL_FRONT, GL_SPECULAR, no_mat);
				glMaterialfv(GL_FRONT, GL_SHININESS, no_shininess);
				glMaterialfv(GL_FRONT, GL_EMISSION, no_mat);
				glutSolidSphere(1.0f, 16, 16);
			glPopMatrix();

			// draw sphere in third row, second column colored ambient, diffuse and specular reflection; low shininess
			glPushMatrix();
				glTranslatef(-1.25f, -3.0f, 0.0f);
				glMaterialfv(GL_FRONT, GL_AMBIENT, mat_ambient_color);
				glMaterialfv(GL_FRONT, GL_DIFFUSE, mat_diffuse);
				glMaterialfv(GL_FRONT, GL_SPECULAR, mat_specular);
				glMaterialfv(GL_FRONT, GL_SHININESS, low_shininess);
				glMaterialfv(GL_FRONT, GL_EMISSION, no_mat);
				glutSolidSphere(1.0f, 16, 16);
			glPopMatrix();

			// draw sphere in third row, third column colored ambient, diffuse and specular reflection; high shininess
			glPushMatrix();
				glTranslatef(1.25f, -3.0f, 0.0f);
				glMaterialfv(GL_FRONT, GL_AMBIENT, mat_ambient_color);
				glMaterialfv(GL_FRONT, GL_DIFFUSE, mat_diffuse);
				glMaterialfv(GL_FRONT, GL_SPECULAR, mat_specular);
				glMaterialfv(GL_FRONT, GL_SHININESS, high_shininess);
				glMaterialfv(GL_FRONT, GL_EMISSION, no_mat);
				glutSolidSphere(1.0f, 16, 16);
			glPopMatrix();

			// draw sphere in third row, fourth column colored ambient and diffuse reflection; emission; no specular
			glPushMatrix();
				glTranslatef(3.75f, -3.0f, 0.0f);
				glMaterialfv(GL_FRONT, GL_AMBIENT, mat_ambient_color);
				glMaterialfv(GL_FRONT, GL_DIFFUSE, mat_diffuse);
				glMaterialfv(GL_FRONT, GL_SPECULAR, no_mat);
				glMaterialfv(GL_FRONT, GL_SHININESS, no_shininess);
				glMaterialfv(GL_FRONT, GL_EMISSION, mat_emission);
				glutSolidSphere(1.0f, 16, 16);
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
			if(width <= (height * 2)) {
				glOrtho(-6.0f, 6.0f, -3.0f * ((float) height * 2) / (float) width, 3.0f * ((float) height * 2) / (float) width, -10.0f, 10.0f);
			}
			else {
				glOrtho(-6.0f * (float) width / ((float) height * 2), 6.0f * (float) width / ((float) height * 2), -3.0f, 3.0f, -10.0f, 10.0f);
			}
			glMatrixMode(GL_MODELVIEW);
			glLoadIdentity();
		}
		#endregion Reshape(int width, int height)
	}
}