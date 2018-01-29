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
[assembly: AssemblyDescription("Redbook Feedback")]
[assembly: AssemblyProduct("Redbook Feedback")]
[assembly: AssemblyTitle("Redbook Feedback")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace RedbookExamples {
	/// <summary>
	/// Redbook Feedback -- OpenGL Feedback (http://www.opengl.org/developers/code/examples/redbook/redbook.html)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class RedbookFeedback : Model {
		// --- Fields ---
		#region Private Fields
		private static byte[] rasters = {
			0xc0, 0x00, 0xc0, 0x00, 0xc0, 0x00, 0xc0, 0x00, 0xc0, 0x00,
			0xff, 0x00, 0xff, 0x00, 0xc0, 0x00, 0xc0, 0x00, 0xc0, 0x00,
			0xff, 0xc0, 0xff, 0xc0
		};

		private static float[] feedBuffer = new float[1024];
		private static int size;
		private static bool oneTime;
		private static string info = "";
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Example title.
		/// </summary>
		public override string Title {
			get {
				return "Redbook Feedback -- OpenGL Feedback";
			}
		}

		/// <summary>
		/// Example description.
		/// </summary>
		public override string Description {
			get {
				return "This program demonstrates use of OpenGL feedback.  First, a lighting environment is set up and a few lines are drawn.  Then feedback mode is entered, and the same lines are drawn.  The results in the feedback buffer are printed to the help screen.\n\nFeedback:\n" + info;
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
			App.Run(new RedbookFeedback());												// Run Our Example As A Windows Forms Application
		}
		#endregion Main()

		// --- Basecode Methods ---
		#region Initialize()
		/// <summary>
		/// Overrides OpenGL's initialization.
		/// </summary>
		public override void Initialize() {
			glEnable(GL_LIGHTING);
			glEnable(GL_LIGHT0);

			oneTime = true;
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws Redbook Feedback scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			glMatrixMode(GL_PROJECTION);
			glLoadIdentity();
			glOrtho(0.0f, 100.0f, 0.0f, 100.0f, 0.0f, 1.0f);

			glClearColor(0.0f, 0.0f, 0.0f, 0.0f);
			glClear(GL_COLOR_BUFFER_BIT);
			DrawGeometry(GL_RENDER);

			glFeedbackBuffer(1024, GL_3D_COLOR, feedBuffer);
			glRenderMode(GL_FEEDBACK);
			DrawGeometry(GL_FEEDBACK);

			size = glRenderMode(GL_RENDER);

			if(oneTime) {
				PrintBuffer(size, feedBuffer);
				oneTime = false;
			}
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
			glOrtho(0, width, 0, height, -1.0f, 1.0f);
			glMatrixMode(GL_MODELVIEW);
		}
		#endregion Reshape(int width, int height)

		// --- Example Methods ---
		#region PrintBuffer(int size, float[] buffer)
		/// <summary>
		/// Write contents of entire buffer, parsing the tokens.
		/// </summary>
		/// <param name="size"></param>
		/// <param name="buffer"></param>
		private static void PrintBuffer(int size, float[] buffer) {
			// Could use a StringBuilder here, but was too lazy.
			int count;
			float token;

			count = size;
			while(count != 0) {
				token = buffer[size - count];
				count--;
				if(token == GL_PASS_THROUGH_TOKEN) {
					info += "GL_PASS_THROUGH_TOKEN\n";
					info += "  " + buffer[size - count] + "\n";
					count--;
				}
				else if(token == GL_POINT_TOKEN) {
					info += "GL_POINT_TOKEN\n";
					Print3dColorVertex(size, count, buffer);
				}
				else if(token == GL_LINE_TOKEN) {
					info += "GL_LINE_TOKEN\n";
					Print3dColorVertex(size, count, buffer);
					Print3dColorVertex(size, count, buffer);
				}
				else if(token == GL_LINE_RESET_TOKEN) {
					info += "GL_LINE_RESET_TOKEN\n";
					Print3dColorVertex(size, count, buffer);
					Print3dColorVertex(size, count, buffer);
				}
			}
		}
		#endregion PrintBuffer(int size, float[] buffer)

		#region Print3dColorVertex(int size, int count, float[] buffer)
		/// <summary>
		/// Writes contents of one vertex to info.
		/// </summary>
		/// <param name="size"></param>
		/// <param name="count"></param>
		/// <param name="buffer"></param>
		private static void Print3dColorVertex(int size, int count, float[] buffer) {
			info += "  ";
			for(int i = 0; i < 7; i++) {
				info += buffer[size - count];
				count = count - 1;
			}
			info += "\n";
		}
		#endregion Print3dColorVertex(int size, int count, float[] buffer)

		#region DrawGeometry(uint mode)
		/// <summary>
		/// Draw a few lines and two points, one of which will be clipped.
		/// If in feedback mode, a passthrough token is issued between
		/// each primitive.
		/// </summary>
		/// <param name="mode"></param>
		private static void DrawGeometry(uint mode) {
			glBegin(GL_LINE_STRIP);
				glNormal3f(0.0f, 0.0f, 1.0f);
				glVertex3f(30.0f, 30.0f, 0.0f);
				glVertex3f(50.0f, 60.0f, 0.0f);
				glVertex3f(70.0f, 40.0f, 0.0f);
			glEnd();
			if(mode == GL_FEEDBACK) {
				glPassThrough(1.0f);
			}
			glBegin(GL_POINTS);
				glVertex3f(-100.0f, -100.0f, -100.0f);									//  Will Be Clipped
			glEnd();
			if(mode == GL_FEEDBACK) {
				glPassThrough(2.0f);
			}
			glBegin(GL_POINTS);
				glNormal3f(0.0f, 0.0f, 1.0f);
				glVertex3f(50.0f, 50.0f, 0.0f);
			glEnd();
		}
		#endregion DrawGeometry(uint mode)
	}
}