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
/**************************************
*                                     *
*   Jeff Molofee's Basecode Example   *
*          nehe.gamedev.net           *
*                2001                 *
*                                     *
**************************************/
#endregion Original Credits / License

using CsGL.Basecode;
using System.Reflection;

#region AssemblyInfo
[assembly: AssemblyCompany("The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyCopyright("2002 The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyDescription("NeHe Basecode Example")]
[assembly: AssemblyProduct("NeHe Basecode Example")]
[assembly: AssemblyTitle("NeHe Basecode Example")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace NeHeLessons {
	/// <summary>
	/// NeHe Basecode Example (http://nehe.gamedev.net)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class NeHeBasecodeExample : Model {
		// --- Fields ---
		#region Private Fields
		private static float angle = 0.0f;												// Used To Rotate The Triangles
		private static int rot1 = 0, rot2 = 0;											// Counter Variables
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Example's description.
		/// </summary>
		public override string Description {
			get {
				return "A simple example using the CsGL Basecode layer, as presented in NeHe's NeHeGL Basecode, consisting of a few rotating, smooth-colored triangles.";
			}
		}

		/// <summary>
		/// Example's title.
		/// </summary>
		public override string Title {
			get {
				return "NeHe Basecode Example";
			}
		}

		/// <summary>
		/// Example's URL.
		/// </summary>
		public override string Url {
			get {
				return "http://nehe.gamedev.net/";
			}
		}
		#endregion Public Properties

		// --- Entry Point ---
		#region Main()
		/// <summary>
		/// Application's entry point.  Runs this example.
		/// </summary>
		public static void Main() {
			App.Run(new NeHeBasecodeExample());											// Run Our Example As A Windows Forms Application
		}
		#endregion Main()

		// --- Basecode Methods ---
		#region Draw()
		/// <summary>
		/// Draws the NeHe Basecode Example scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);							// Clear Screen And Depth Buffer
			glLoadIdentity();															// Reset The Current Modelview Matrix

			glTranslatef(0.0f, 0.0f, -6.0f);											// Move Into The Screen 6.0

			glRotatef(angle, 0.0f, 1.0f, 0.0f);											// Rotate The Triangle On The Y Axis By angle
			for(rot1 = 0; rot1 < 2; rot1++) {											// 2 Passes For Two Sets Of Triangles
				glRotatef( 90.0f, 0.0f, 1.0f, 0.0f);									// Rotate 90 Degrees On The Y-Axis
				glRotatef(180.0f, 1.0f, 0.0f, 0.0f);									// Rotate 180 Degress On The X-Axis
				for(rot2 = 0; rot2 < 2; rot2++) {										// 2 Passes For Each Of The Sets, Draw Two Triangles
					glRotatef(180.0f, 0.0f, 1.0f, 0.0f);								// Rotate 180 Degrees On The Y-Axis
					glBegin(GL_TRIANGLES);												// Begin Drawing Triangles
					glColor3f(1.0f, 0.0f, 0.0f);  glVertex3f( 0.0f,  1.0f, 0.0f);		// Draw The Red Vertex
					glColor3f(0.0f, 1.0f, 0.0f);  glVertex3f(-1.0f, -1.0f, 1.0f);		// Draw The Green Vertex
					glColor3f(0.0f, 0.0f, 1.0f);  glVertex3f( 1.0f, -1.0f, 1.0f);		// Draw The Blue Vertex
					glEnd ();															// Done Drawing Triangles
				}
			}

			angle += 0.2f;																// Update angle
			glFlush();																	// Flush The GL Rendering Pipeline
		}
		#endregion Draw()
	}
}