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
 * Bouncing ball demo.
 *
 * This program is in the public domain
 *
 * Brian Paul
 *
 * Conversion to GLUT by Mark J. Kilgard
 */
#endregion Original Credits / License

using CsGL.Basecode;
using System;
using System.Reflection;

#region AssemblyInfo
[assembly: AssemblyCompany("The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyCopyright("2002 The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyDescription("Mesa Bounce")]
[assembly: AssemblyProduct("Mesa Bounce")]
[assembly: AssemblyTitle("Mesa Bounce")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace MesaDemos {
	/// <summary>
	/// The Mesa Bouncing Ball Demo (http://www.mesa3d.org)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class MesaBounce : Model {
		// --- Fields ---
		#region Private Fields
		private const int RED = 1;
		private const int WHITE = 2;
		private const int CYAN = 3;
		private static uint Ball;
		private static float Zrot = 0.0f, Zstep = 6.0f;
		private static float Xpos = 0.0f, Ypos = 1.0f;
		private static float Xvel = 0.2f, Yvel = 0.0f;
		private static float Xmin = -4.0f, Xmax = 4.0f;
		private static float Ymin = -3.8f;
		private static float G = -0.1f;
		private static float vel0 = -100.0f;
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Example's description.
		/// </summary>
		public override string Description {
			get {
				return "The Mesa bouncing ball demo.";
			}
		}

		/// <summary>
		/// Example's title.
		/// </summary>
		public override string Title {
			get {
				return "Mesa Bounce";
			}
		}

		/// <summary>
		/// Example's URL.
		/// </summary>
		public override string Url {
			get {
				return "http://www.mesa3d.org";
			}
		}
		#endregion Public Properties

		// --- Entry Point ---
		#region Main()
		/// <summary>
		/// Application's entry point.  Runs this example.
		/// </summary>
		public static void Main() {
			App.Run(new MesaBounce());													// Run Our Example As A Windows Forms Application
		}
		#endregion Main()

		// --- Basecode Methods ---
		#region Initialize()
		/// <summary>
		/// Overrides OpenGL's initialization.
		/// </summary>
		public override void Initialize() {
			Ball = MakeBall();
			glCullFace(GL_BACK);
			glEnable(GL_CULL_FACE);
			glDisable(GL_DITHER);
			glShadeModel(GL_FLAT);
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws the Mesa Bounce scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			int i;

			glClear(GL_COLOR_BUFFER_BIT);

			glIndexi(CYAN);
			glColor3f(0, 1, 1);
			glBegin(GL_LINES);
				for(i = -5; i <= 5; i++) {
					glVertex2i(i, -5);
					glVertex2i(i, 5);
				}
				for(i = -5; i <= 5; i++) {
					glVertex2i(-5, i);
					glVertex2i(5, i);
				}
				for(i = -5; i <= 5; i++) {
					glVertex2i(i, -5);
					glVertex2f(i * 1.15f, -5.9f);
				}
				glVertex2f(-5.3f, -5.35f);
				glVertex2f(5.3f, -5.35f);
				glVertex2f(-5.75f, -5.9f);
				glVertex2f(5.75f, -5.9f);
			glEnd();

			glPushMatrix();
				glTranslatef(Xpos, Ypos, 0.0f);
				glScalef(2.0f, 2.0f, 2.0f);
				glRotatef(8.0f, 0.0f, 0.0f, 1.0f);
				glRotatef(90.0f, 1.0f, 0.0f, 0.0f);
				glRotatef(Zrot, 0.0f, 0.0f, 1.0f);
				glCallList(Ball);
			glPopMatrix();

			glFlush();

			Zrot += Zstep;
			Xpos += Xvel;
			if(Xpos >= Xmax) {
				Xpos = Xmax;
				Xvel = -Xvel;
				Zstep = -Zstep;
			}
			if(Xpos <= Xmin) {
				Xpos = Xmin;
				Xvel = -Xvel;
				Zstep = -Zstep;
			}
			Ypos += Yvel;
			Yvel += G;
			if(Ypos < Ymin) {
				Ypos = Ymin;
				if(vel0 == -100.0f) {
					vel0 = Math.Abs(Yvel);
				}
				Yvel = vel0;
			}
			System.Threading.Thread.Sleep(15);											// Added A Sleep Call To Slow Down The Animation
		}
		#endregion Draw()

		#region Reshape(int width, int height)
		/// <summary>
		/// Overrides OpenGL reshaping.
		/// </summary>
		/// <param name="width">New width.</param>
		/// <param name="height">New height.</param>
		public override void Reshape(int width, int height) {							// Resize And Initialize The GL Window
			float aspect = (float) width / (float) height;
			glViewport(0, 0, width, height);
			glMatrixMode(GL_PROJECTION);
			glLoadIdentity();
			glOrtho(-6.0 * aspect, 6.0 * aspect, -6.0, 6.0, -6.0, 6.0);
			glMatrixMode(GL_MODELVIEW);
		}
		#endregion Reshape(int width, int height)

		// --- Lesson Methods ---
		#region COS(float f)
		/// <summary>
		/// Performa a modified Cos.
		/// </summary>
		private static float COS(float f) {
			return (float) Math.Cos(f * 3.14159f / 180.0f);
		}
		#endregion COS(float value)

		#region MakeBall()
		/// <summary>
		/// Makes the ball.
		/// </summary>
		/// <returns>The display list.</returns>
		private static uint MakeBall() {
			uint list;
			float a, b;
			float da = 18.0f, db = 18.0f;
			float radius = 1.0f;
			uint color;
			float x, y, z;

			list = glGenLists(1);

			glNewList(list, GL_COMPILE);
			color = 0;
			for(a = -90.0f; a + da <= 90.0f; a += da) {
				glBegin(GL_QUAD_STRIP);
				for(b = 0.0f; b <= 360.0; b += db) {
					if(color != 0) {
						glIndexi(RED);
						glColor3f(1, 0, 0);
					}
					else {
						glIndexi(WHITE);
						glColor3f(1, 1, 1);
					}

					x = radius * COS(b) * COS(a);
					y = radius * SIN(b) * COS(a);
					z = radius * SIN(a);
					glVertex3f(x, y, z);

					x = radius * COS(b) * COS(a + da);
					y = radius * SIN(b) * COS(a + da);
					z = radius * SIN(a + da);
					glVertex3f(x, y, z);

					color = 1 - color;
				}
				glEnd();
			}
			glEndList();

			return list;
		}
		#endregion MakeBall()

		#region SIN(float f)
		/// <summary>
		/// Performs a modified Sin.
		/// </summary>
		private static float SIN(float f) {
			return (float) Math.Sin(f * 3.14159f / 180.0f);
		}
		#endregion SIN(float value)
	}
}