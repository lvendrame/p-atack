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
[assembly: AssemblyDescription("Redbook Lines")]
[assembly: AssemblyProduct("Redbook Lines")]
[assembly: AssemblyTitle("Redbook Lines")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace RedbookExamples {
	/// <summary>
	/// Redbook Lines -- Primitive Lines (http://www.opengl.org/developers/code/examples/redbook/redbook.html)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class RedbookLines : Model {
		// --- Fields ---
		#region Public Properties
		/// <summary>
		/// Example title.
		/// </summary>
		public override string Title {
			get {
				return "Redbook Lines -- Primitive Lines";
			}
		}

		/// <summary>
		/// Example description.
		/// </summary>
		public override string Description {
			get {
				return "This program demonstrates geometric primitives and their attributes.";
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
			App.Run(new RedbookLines());												// Run Our Example As A Windows Forms Application
		}
		#endregion Main()

		// --- Basecode Methods ---
		#region Initialize()
		/// <summary>
		/// Overrides OpenGL's initialization.
		/// </summary>
		public override void Initialize() {
			glClearColor(0.0f, 0.0f, 0.0f, 0.0f);
			glShadeModel(GL_FLAT);
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws Redbook Lines scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			glClear(GL_COLOR_BUFFER_BIT);

			// select white for all lines
			glColor3f(1.0f, 1.0f, 1.0f);

			// in 1st row, 3 lines, each with a different stipple
			glEnable(GL_LINE_STIPPLE);

			glLineStipple(1, 0x0101);													// Dotted
			DrawOneLine(50.0f, 125.0f, 150.0f, 125.0f);
			glLineStipple(1, 0x00FF);													// Dashed
			DrawOneLine(150.0f, 125.0f, 250.0f, 125.0f);
			glLineStipple(1, 0x1C47);													// Dash/Dot/Dash
			DrawOneLine(250.0f, 125.0f, 350.0f, 125.0f);

			// in 2nd row, 3 wide lines, each with different stipple
			glLineWidth(5.0f);
			glLineStipple(1, 0x0101);													// Dotted
			DrawOneLine(50.0f, 100.0f, 150.0f, 100.0f);
			glLineStipple(1, 0x00FF);													// Dashed
			DrawOneLine(150.0f, 100.0f, 250.0f, 100.0f);
			glLineStipple(1, 0x1C47);													// Dash/Dot/Dash
			DrawOneLine(250.0f, 100.0f, 350.0f, 100.0f);
			glLineWidth(1.0f);

			// in 3rd row, 6 lines, with dash/dot/dash stipple as part of a single connected line strip
			glLineStipple(1, 0x1C47);													// Dash/Dot/Dash
			glBegin(GL_LINE_STRIP);
				for(int i = 0; i < 7; i++) {
					glVertex2f(50.0f + ((float) i * 50.0f), 75.0f);
				}
			glEnd ();

			// in 4th row, 6 independent lines with same stipple
			for(int i = 0; i < 6; i++) {
				DrawOneLine(50.0f + ((float) i * 50.0f), 50.0f, 50.0f + ((float)(i + 1) * 50.0f), 50.0f);
			}

			// in 5th row, 1 line, with dash/dot/dash stipple and a stipple repeat factor of 5
			glLineStipple(5, 0x1C47);													// Dash/Dot/Dash
			DrawOneLine(50.0f, 25.0f, 350.0f, 25.0f);

			glDisable(GL_LINE_STIPPLE);
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
			gluOrtho2D(0.0, (double) width, 0.0, (double) height);
		}
		#endregion Reshape(int width, int height)

		// --- Example Methods ---
		#region DrawOneLine(float x1, float y1, float x2, float y2)
		/// <summary>
		/// Draws a line.
		/// </summary>
		/// <param name="x1">X start coordinate.</param>
		/// <param name="y1">Y start coordinate.</param>
		/// <param name="x2">X end coordinate.</param>
		/// <param name="y2">Y end coordinate.</param>
		private static void DrawOneLine(float x1, float y1, float x2, float y2) {
			glBegin(GL_LINES);
			glVertex2f(x1, y1);
			glVertex2f(x2, y2);
			glEnd();
		}
		#endregion DrawOneLine(float x1, float y1, float x2, float y2)
	}
}