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
 * 3-D gear wheels.  This program is in the public domain.
 *
 * Brian Paul
 */
/* Conversion to GLUT by Mark J. Kilgard */
#endregion Original Credits / License

using CsGL.Basecode;
using System;
using System.Reflection;

#region AssemblyInfo
[assembly: AssemblyCompany("The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyCopyright("2002 The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyDescription("Mesa Gears")]
[assembly: AssemblyProduct("Mesa Gears")]
[assembly: AssemblyTitle("Mesa Gears")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace MesaDemos {
	/// <summary>
	/// The Mesa Spinning Gears Demo (http://www.mesa3d.org)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class MesaGears : Model {
		// --- Fields ---
		#region Private Fields
		private static double rotx = 20.0;												// View's X-Axis Rotation
		private static double roty = 30.0;												// View's Y-Axis Rotation
		private static double rotz = 0.0;												// View's Z-Axis Rotation
		private static uint gear1;														// Display List For Red Gear
		private static uint gear2;														// Display List For Green Gear
		private static uint gear3;														// Display List For Blue Gear
		private static float rAngle = 0.0f;												// Rotation Angle
		private static float[] pos = {5.0f, 5.0f, 10.0f, 0.0f};							// Light Position
		private static float[] red = {0.8f, 0.1f, 0.0f, 1.0f};							// Red Material
		private static float[] green = {0.0f, 0.8f, 0.2f, 1.0f};						// Green Material
		private static float[] blue = {0.2f, 0.2f, 1.0f, 1.0f};							// Blue Material
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Example's description.
		/// </summary>
		public override string Description {
			get {
				return "The Mesa spinning gears demo.";
			}
		}

		/// <summary>
		/// Example's title.
		/// </summary>
		public override string Title {
			get {
				return "Mesa Gears";
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
			App.Run(new MesaGears());													// Run Our Example As A Windows Forms Application
		}
		#endregion Main()

		// --- Basecode Methods ---
		#region Initialize()
		/// <summary>
		/// Overrides OpenGL's initialization.
		/// </summary>
		public override void Initialize() {
			base.Initialize();															// Run The Base Initialization

			glLightfv(GL_LIGHT0, GL_POSITION, pos);										// Create A Light
			glEnable(GL_CULL_FACE);														// Enable Culling
			glEnable(GL_LIGHTING);														// Enable Lighting
			glEnable(GL_LIGHT0);														// Enable Light Zero

			MakeGears();																// Make The Gears

			glEnable(GL_NORMALIZE);														// Enable Normalized Normals
			glTranslated(0.0, 0.0, -20.0);												// Move Into The Screen 20 Units
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws the Mesa Gears scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);							// Clear Screen And Depth Buffer

			glPushMatrix();
			glRotated(rotx, 1.0, 0.0, 0.0);												// Position The World
			glRotated(roty, 0.0, 1.0, 0.0);
			glRotated(rotz, 0.0, 0.0, 1.0);
			glTranslated(10.0, -7.0, -14.0);

			glPushMatrix();
			glTranslated(-3.0, -2.0, 0.0);												// Position The Red Gear
			glRotatef(rAngle, 0.0f, 0.0f, 1.0f);										// Rotate The Red Gear
			glCallList(gear1);															// Draw The Red Gear
			glPopMatrix();

			glPushMatrix();
			glTranslated(3.1, -2.0, 0.0);												// Position The Green Gear
			glRotated(-2.0 * rAngle - 9.0, 0.0, 0.0, 1.0);								// Rotate The Green Gear
			glCallList(gear2);															// Draw The Green Gear
			glPopMatrix();

			glPushMatrix();
			glTranslated(-3.1, 4.2, 0.0);												// Position The Blue Gear
			glRotated(-2.0 * rAngle - 25.0, 0.0, 0.0, 1.0);								// Rotate The Blue Gear
			glCallList(gear3);															// Draw The Blue Gear
			glPopMatrix();
			glPopMatrix();

			rAngle += 0.2f;																// Increase The Rotation
		}
		#endregion Draw()

		// --- Lesson Methods ---
		#region MakeGear(double inner_radius, double outer_radius, double width, int teeth, double tooth_depth)
		/// <summary>
		/// Creates a single gear.
		/// </summary>
		/// <param name="inner_radius">Radius of hole at center.</param>
		/// <param name="outer_radius">Radius at center of teeth.</param>
		/// <param name="width">Width of gear.</param>
		/// <param name="teeth">Number of teeth.</param>
		/// <param name="tooth_depth">Depth of tooth.</param>
		private static void MakeGear(double inner_radius, double outer_radius, double width, int teeth, double tooth_depth) {
			int i;
			double r0;
			double r1;
			double r2;
			double angle;
			double da;
			double u;
			double v;
			double len;

			r0 = inner_radius;
			r1 = outer_radius - tooth_depth / 2.0;
			r2 = outer_radius + tooth_depth / 2.0;

			da = 2.0 * Math.PI / teeth / 4.0;
			glShadeModel(GL_FLAT);

			glNormal3d(0.0, 0.0, 1.0);

			/* draw front face */
			glBegin(GL_QUAD_STRIP);
			for(i = 0; i <= teeth; i++) {
				angle = i * 2.0 * Math.PI / teeth;
				glVertex3d(r0 * Math.Cos(angle), r0 * Math.Sin(angle), width * 0.5);
				glVertex3d(r1 * Math.Cos(angle), r1 * Math.Sin(angle), width * 0.5);
				glVertex3d(r0 * Math.Cos(angle), r0 * Math.Sin(angle), width * 0.5);
				glVertex3d(r1 * Math.Cos(angle + 3 * da), r1 * Math.Sin(angle + 3 * da), width * 0.5);
			}
			glEnd();

			/* draw front sides of teeth */
			glBegin(GL_QUADS);
			da = 2.0 * Math.PI / teeth / 4.0;
			for(i = 0; i < teeth; i++) {
				angle = i * 2.0 * Math.PI / teeth;

				glVertex3d(r1 * Math.Cos(angle), r1 * Math.Sin(angle), width * 0.5);
				glVertex3d(r2 * Math.Cos(angle + da), r2 * Math.Sin(angle + da), width * 0.5);
				glVertex3d(r2 * Math.Cos(angle + 2 * da), r2 * Math.Sin(angle + 2 * da), width * 0.5);
				glVertex3d(r1 * Math.Cos(angle + 3 * da), r1 *Math.Sin(angle + 3 * da), width * 0.5);
			}
			glEnd();

			glNormal3d(0.0, 0.0, -1.0);

			/* draw back face */
			glBegin(GL_QUAD_STRIP);
			for(i = 0; i <= teeth; i++) {
				angle = i * 2.0 * Math.PI / teeth;
				glVertex3d(r1 * Math.Cos(angle), r1 * Math.Sin(angle), -width * 0.5);
				glVertex3d(r0 * Math.Cos(angle), r0 * Math.Sin(angle), -width * 0.5);
				glVertex3d(r1 * Math.Cos(angle + 3 * da), r1 * Math.Sin(angle + 3 * da), -width * 0.5);
				glVertex3d(r0 * Math.Cos(angle), r0 * Math.Sin(angle), -width * 0.5);
			}
			glEnd();

			/* draw back sides of teeth */
			glBegin(GL_QUADS);
			da = 2.0 * Math.PI / teeth / 4.0;
			for(i = 0; i < teeth; i++) {
				angle = i * 2.0 * Math.PI / teeth;

				glVertex3d(r1 * Math.Cos(angle + 3 * da), r1 * Math.Sin(angle + 3 * da), -width * 0.5);
				glVertex3d(r2 * Math.Cos(angle + 2 * da), r2 * Math.Sin(angle + 2 * da), -width * 0.5);
				glVertex3d(r2 * Math.Cos(angle + da), r2 * Math.Sin(angle + da), -width * 0.5);
				glVertex3d(r1 * Math.Cos(angle), r1 * Math.Sin(angle), -width * 0.5);
			}
			glEnd();

			/* draw outward faces of teeth */
			glBegin(GL_QUAD_STRIP);
			for(i = 0; i < teeth; i++) {
				angle = i * 2.0 * Math.PI / teeth;

				glVertex3d(r1 * Math.Cos(angle), r1 * Math.Sin(angle), width * 0.5);
				glVertex3d(r1 * Math.Cos(angle), r1 * Math.Sin(angle), -width * 0.5);
				u = r2 * Math.Cos(angle + da) - r1 * Math.Cos(angle);
				v = r2 * Math.Sin(angle + da) - r1 * Math.Sin(angle);
				len = Math.Sqrt(u * u + v * v);
				u /= len;
				v /= len;
				glNormal3d(v, -u, 0.0);
				glVertex3d(r2 * Math.Cos(angle + da), r2 * Math.Sin(angle + da), width * 0.5);
				glVertex3d(r2 * Math.Cos(angle + da), r2 * Math.Sin(angle + da), -width * 0.5);
				glNormal3d(Math.Cos(angle), Math.Sin(angle), 0.0);
				glVertex3d(r2 * Math.Cos(angle + 2 * da), r2 * Math.Sin(angle + 2 * da), width * 0.5);
				glVertex3d(r2 * Math.Cos(angle + 2 * da), r2 * Math.Sin(angle + 2 * da), -width * 0.5);
				u = r1 * Math.Cos(angle + 3 * da) - r2 * Math.Cos(angle + 2 * da);
				v = r1 * Math.Sin(angle + 3 * da) - r2 * Math.Sin(angle + 2 * da);
				glNormal3d(v, -u, 0.0);
				glVertex3d(r1 * Math.Cos(angle + 3 * da), r1 * Math.Sin(angle + 3 * da), width * 0.5);
				glVertex3d(r1 * Math.Cos(angle + 3 * da), r1 * Math.Sin(angle + 3 * da), -width * 0.5);
				glNormal3d(Math.Cos(angle), Math.Sin(angle), 0.0);
			}

			glVertex3d(r1 * Math.Cos(0), r1 * Math.Sin(0), width * 0.5);
			glVertex3d(r1 * Math.Cos(0), r1 * Math.Sin(0), -width * 0.5);

			glEnd();

			glShadeModel(GL_SMOOTH);

			/* draw inside radius cylinder */
			glBegin(GL_QUAD_STRIP);
			for(i = 0; i <= teeth; i++) {
				angle = i * 2.0 * Math.PI / teeth;
				glNormal3d(-Math.Cos(angle), -Math.Sin(angle), 0.0);
				glVertex3d(r0 * Math.Cos(angle), r0 * Math.Sin(angle), -width * 0.5);
				glVertex3d(r0 * Math.Cos(angle), r0 * Math.Sin(angle), width * 0.5);
			}
			glEnd();
		}
		#endregion MakeGear(double inner_radius, double outer_radius, double width, int teeth, double tooth_depth)

		#region MakeGears()
		/// <summary>
		/// Creates the red, green, and blue gears.
		/// </summary>
		private void MakeGears() {
			// Make The Gears
			gear1 = glGenLists(1);														// Generate A Display List For The Red Gear
			glNewList(gear1, GL_COMPILE);												// Create The Display List
			glMaterialfv(GL_FRONT, GL_AMBIENT_AND_DIFFUSE, red);						// Create A Red Material
			MakeGear(1.0, 4.0, 1.0, 20, 0.7);											// Make The Gear
			glEndList();																// Done Building The Red Gear's Display List

			gear2 = glGenLists(1);														// Generate A Display List For The Green Gear
			glNewList(gear2, GL_COMPILE);												// Create The Display List
			glMaterialfv(GL_FRONT, GL_AMBIENT_AND_DIFFUSE, green);						// Create A Green Material
			MakeGear(0.5, 2.0, 2.0, 10, 0.7);											// Make The Gear
			glEndList();																// Done Building The Green Gear's Display List

			gear3 = glGenLists(1);														// Generate A Display List For The Blue Gear
			glNewList(gear3, GL_COMPILE);												// Create The Display List
			glMaterialfv(GL_FRONT, GL_AMBIENT_AND_DIFFUSE, blue);						// Create A Blue Material
			MakeGear(1.3, 2.0, 0.5, 10, 0.7);											// Make The Gear
			glEndList();																// Done Building The Blue Gear's Display List
		}
		#endregion MakeGears()
	}
}