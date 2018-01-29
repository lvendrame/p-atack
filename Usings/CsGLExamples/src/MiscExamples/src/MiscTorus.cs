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
// UNKNOWN!
#endregion Original Credits / License

using CsGL.Basecode;
using System;
using System.Reflection;

#region AssemblyInfo
[assembly: AssemblyCompany("The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyCopyright("2002 The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyDescription("Misc Torus")]
[assembly: AssemblyProduct("Misc Torus")]
[assembly: AssemblyTitle("Misc Torus")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace MiscExamples {
	/// <summary>
	/// Torus Example (http://nehe.gamedev.net)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class MiscTorus : Model {
		// --- Fields ---
		#region Private Fields
		private static float[] lightPos = {0.70f, 0.70f, 1.25f, 0.00f};
		private static float[] mat0Ambient = {0.01f, 0.01f, 0.01f, 1.00f};
		private static float[] mat0Diffuse  = {0.65f, 0.05f, 0.20f, 0.60f};
		private static float[] mat0Specular = {0.50f, 0.50f, 0.50f, 1.00f};
		private static float mat0Shine = 20.0f;
		private static float[] mat1Ambient  = {0.01f, 0.01f, 0.01f, 1.00f};
		private static float[] mat1Diffuse  = {0.05f, 0.70f, 0.20f, 0.80f};
		private static float[] mat1Specular = {0.50f, 0.50f, 0.50f, 1.00f};
		private static float mat1Shine = 20.0f;
		private static float angle = 0;
		private static float step = 1.35f;
		private static float majorRadius = 0.6f;
		private static float minorRadius = 0.2f;
		private static int numMajor = 32;
		private static int numMinor = 24;

		internal struct fvector {
			public float x, y, z;
		}

		private static fvector[] vertices;
		private static fvector[] normals;
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Example's description.
		/// </summary>
		public override string Description {
			get {
				return "A spinning torus!";
			}
		}

		/// <summary>
		/// Example's title.
		/// </summary>
		public override string Title {
			get {
				return "Misc Torus Example";
			}
		}
		#endregion Public Properties

		// --- Entry Point ---
		#region Main()
		/// <summary>
		/// Application's entry point.  Runs this example.
		/// </summary>
		public static void Main() {
			App.Run(new MiscTorus());													// Run Our Example As A Windows Forms Application
		}
		#endregion Main()

		// --- Basecode Methods ---
		#region Initialize()
		/// <summary>
		/// Overrides OpenGL's initialization.
		/// </summary>
		public override void Initialize() {
			base.Initialize();															// Run The Base Initialization

			glLightfv(GL_LIGHT0, GL_POSITION, lightPos);
			glEnable(GL_LIGHTING);
			glEnable(GL_LIGHT0);

			Precalculate();
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws the Misc Torus scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			int baseMat = 0;
			int indx = 0;

			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
			glPushMatrix();
			glMatrixMode(GL_MODELVIEW);
			glTranslatef(0.0f, 0.0f, -2.0f);
			glRotatef(angle, 0.0f, 1.0f, 0.0f);
			angle += step;
			glRotatef(angle, 1.0f, 0.0f, 0.0f);
			for (int i = numMajor; --i >= 0;) {
				baseMat = i & 1;
				glBegin (GL_TRIANGLE_STRIP);
				for (int j = numMinor + 1; --j >= 0;) {
					SelectMaterial(baseMat ^ (j & 1));

					fvector normal0 = normals [indx];
					fvector vertex0 = vertices [indx++];

					fvector normal1 = normals [indx];
					fvector vertex1 = vertices [indx++];

					glNormal3f(normal0.x, normal0.y, normal0.z);
					glVertex3f(vertex0.x, vertex0.y, vertex0.z);

					glNormal3f(normal1.x, normal1.y, normal1.z);
					glVertex3f(vertex1.x, vertex1.y, vertex1.z);
				}
				glEnd();
			}
			glPopMatrix();
		}
		#endregion Draw()

		#region Precalculate()
		/// <summary>
		/// Do some calculations ahead of time.
		/// </summary>
		private void Precalculate() {
			vertices = new fvector[numMajor * (1 + numMinor) * 2];
			normals = new fvector[vertices.Length];

			double pi2 = 2 * Math.PI;
			float minorStep = (float) (pi2 / numMinor);
			float majorStep = (float) (pi2 / numMajor);;
			float a, b;
			float sin0, cos0, sin1, cos1;

			fvector normal0;
			fvector normal1;
			fvector vertex0;
			fvector vertex1;

			int indx = 0;


			for (int i = numMajor; --i >= 0;) {
				a = i * majorStep;
				b = a + majorStep;

				sin0 = (float) Math.Sin(a);
				cos0 = (float) Math.Cos(a);
				sin1 = (float) Math.Sin(b);
				cos1 = (float) Math.Cos(b);

				for (int j = numMinor + 1; --j >= 0;) {
					float x = j * minorStep;
					float sx = (float) Math.Sin(x);
					float cx = (float) Math.Cos(x);
					float r = minorRadius * cx + majorRadius;
					float z = minorRadius * sx;

					normal0.x = cx * cos0;
					normal0.y = cx * sin0;
					normal0.z = z / minorRadius;

					normal1.x = cx * cos1;
					normal1.y = cx * sin1;
					normal1.z = normal0.z;

					vertex0.x = r * cos0;
					vertex0.y = r * sin0;
					vertex0.z = z;

					vertex1.x = r * cos1;
					vertex1.y = r * sin1;
					vertex1.z = z;


					normals[indx] = normal0;
					vertices[indx++] = vertex0;

					normals[indx] = normal1;
					vertices[indx++] = vertex1;
				}
			}
		}
		#endregion Precalculate()

		#region SelectMaterial(int matNum)
		/// <summary>
		/// Select a material.
		/// </summary>
		/// <param name="matNum">The material to select.</param>
		private static void SelectMaterial(int matNum) {
			glMaterialfv(GL_FRONT, GL_AMBIENT, (matNum == 0) ? mat0Ambient : mat1Ambient);
			glMaterialfv(GL_FRONT, GL_DIFFUSE, (matNum == 0) ? mat0Diffuse : mat1Diffuse);
			glMaterialfv(GL_FRONT, GL_SPECULAR, (matNum == 0) ? mat0Specular : mat1Specular);
			glMaterialf(GL_FRONT, GL_SHININESS, (matNum == 0) ? mat0Shine : mat1Shine);
		}
		#endregion SelectMaterial(int matNum)
	}
}