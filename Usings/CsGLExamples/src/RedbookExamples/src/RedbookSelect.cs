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
[assembly: AssemblyDescription("Redbook Select")]
[assembly: AssemblyProduct("Redbook Select")]
[assembly: AssemblyTitle("Redbook Select")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace RedbookExamples {
	/// <summary>
	/// Redbook Select -- Selection Mode (http://www.opengl.org/developers/code/examples/redbook/redbook.html)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class RedbookSelect : Model {
		// --- Fields ---
		#region Private Fields
		private const int BUFSIZE = 512;
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Example title.
		/// </summary>
		public override string Title {
			get {
				return "Redbook Select -- Selection Mode";
			}
		}

		/// <summary>
		/// Example description.
		/// </summary>
		public override string Description {
			get {
				return "This is an illustration of the selection mode and name stack, which detects whether objects which collide with a viewing volume.  First, four triangles and a rectangular box representing a viewing volume are drawn (DrawScene routine).  The green triangle and yellow triangles appear to lie within the viewing volume, but the red triangle appears to lie outside it.  Then the selection mode is entered (SelectObjects routine).  Drawing to the screen ceases.  To see if any collisions occur, the four triangles are called.  In this example, the green triangle causes one hit with the name 1, and the yellow triangles cause one hit with the name 3.";
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
			App.Run(new RedbookSelect());												// Run Our Example As A Windows Forms Application
		}
		#endregion Main()

		// --- Basecode Methods ---
		#region Initialize()
		/// <summary>
		/// Overrides OpenGL's initialization.
		/// </summary>
		public override void Initialize() {
			glEnable(GL_DEPTH_TEST);
			glShadeModel(GL_FLAT);
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws Redbook Select scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			glClearColor(0.0f, 0.0f, 0.0f, 0.0f);
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
			DrawScene();
			SelectObjects();
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
			// Do Nothing
		}
		#endregion Reshape(int width, int height)

		// --- Example Methods ---
		#region DrawScene()
		/// <summary>
		/// Draws 4 triangles and a wire frame which represents the viewing volume.
		/// </summary>
		private static void DrawScene() {
			glMatrixMode(GL_PROJECTION);
			glLoadIdentity();
			gluPerspective(40.0f, 4.0f / 3.0f, 1.0f, 100.0f);

			glMatrixMode(GL_MODELVIEW);
			glLoadIdentity();
			gluLookAt(7.5f, 7.5f, 12.5f, 2.5f, 2.5f, -5.0f, 0.0f, 1.0f, 0.0f);
			glColor3f(0.0f, 1.0f, 0.0f);												// Green Triangle
			DrawTriangle(2.0f, 2.0f, 3.0f, 2.0f, 2.5f, 3.0f, -5.0f);
			glColor3f(1.0f, 0.0f, 0.0f);												// Red Triangle
			DrawTriangle(2.0f, 7.0f, 3.0f, 7.0f, 2.5f, 8.0f, -5.0f);
			glColor3f(1.0f, 1.0f, 0.0f);												// Yellow Triangles
			DrawTriangle(2.0f, 2.0f, 3.0f, 2.0f, 2.5f, 3.0f, 0.0f);
			DrawTriangle(2.0f, 2.0f, 3.0f, 2.0f, 2.5f, 3.0f, -10.0f);
			DrawViewVolume(0.0f, 5.0f, 0.0f, 5.0f, 0.0f, 10.0f);
		}
		#endregion DrawScene()

		#region DrawTriangle(float x1, float y1, float x2, float y2, float x3, float y3, float z)
		/// <summary>
		/// Draw a triangle with vertices at (x1, y1), (x2, y2) and (x3, y3) at z units away from the origin.
		/// </summary>
		/// <param name="x1">X coordinate, first vertex.</param>
		/// <param name="y1">Y coordinate, first vertex.</param>
		/// <param name="x2">X coordinate, second vertex.</param>
		/// <param name="y2">Y coordinate, second vertex.</param>
		/// <param name="x3">X coordinate, third vertex.</param>
		/// <param name="y3">Y coordinate, third vertex.</param>
		/// <param name="z">Distance from origin.</param>
		private static void DrawTriangle(float x1, float y1, float x2, float y2, float x3, float y3, float z) {
			glBegin(GL_TRIANGLES);
				glVertex3f(x1, y1, z);
				glVertex3f(x2, y2, z);
				glVertex3f(x3, y3, z);
			glEnd();
		}
		#endregion DrawTriangle(float x1, float y1, float x2, float y2, float x3, float y3, float z)

		#region DrawViewVolume(float x1, float x2, float y1, float y2, float z1, float z2)
		/// <summary>
		/// Draw a rectangular box with these outer x, y, and z values.
		/// </summary>
		/// <param name="x1">X coordinate, first vertex.</param>
		/// <param name="x2">X coordinate, second vertex.</param>
		/// <param name="y1">Y coordinate, first vertex.</param>
		/// <param name="y2">Y coordinate, second vertex.</param>
		/// <param name="z1">Z coordinate, first vertex.</param>
		/// <param name="z2">Z coordinate, second vertex.</param>
		private static void DrawViewVolume(float x1, float x2, float y1, float y2, float z1, float z2) {
			glColor3f(1.0f, 1.0f, 1.0f);
			glBegin(GL_LINE_LOOP);
				glVertex3f(x1, y1, -z1);
				glVertex3f(x2, y1, -z1);
				glVertex3f(x2, y2, -z1);
				glVertex3f(x1, y2, -z1);
			glEnd();

			glBegin(GL_LINE_LOOP);
				glVertex3f(x1, y1, -z2);
				glVertex3f(x2, y1, -z2);
				glVertex3f(x2, y2, -z2);
				glVertex3f(x1, y2, -z2);
			glEnd();

			glBegin(GL_LINES);
				glVertex3f(x1, y1, -z1);
				glVertex3f(x1, y1, -z2);
				glVertex3f(x1, y2, -z1);
				glVertex3f(x1, y2, -z2);
				glVertex3f(x2, y1, -z1);
				glVertex3f(x2, y1, -z2);
				glVertex3f(x2, y2, -z1);
				glVertex3f(x2, y2, -z2);
			glEnd();
		}
		#endregion DrawViewVolume(float x1, float x2, float y1, float y2, float z1, float z2)

		#region ProcessHits(int hits, uint[] buffer)
		/// <summary>
		/// Displays hit data.
		/// </summary>
		/// <param name="hits">Number of hits.</param>
		/// <param name="buffer">The selection buffer.</param>
		private static void ProcessHits(int hits, uint[] buffer) {
			uint i, j;
			uint names;
			uint[] ptr;

			Console.WriteLine("hits = {0}", hits);
			ptr = buffer;
			for(i = 0; i < hits; i++) {													// For Each Hit
				names = ptr[i];
				Console.WriteLine(" number of names for hit = {0}", names);
				i++;
				Console.WriteLine(" z1 is {0}", (float) ptr[i] / 0x7fffffff);
				i++;
				Console.WriteLine(" z2 is {0}", (float) ptr[i] / 0x7fffffff);
				i++;
				Console.Write(" the name is ");
				for(j = 0; j < names; j++) {											// For Each Name
					Console.Write("{0}", ptr[i]);
					i++;
				}
				Console.Write("\n");
			}
			Console.Write("\n");
		}
		#endregion ProcessHits(int hits, uint[] buffer)

		#region SelectObjects()
		/// <summary>
		/// "Draws" the triangles in selection mode,  assigning names for the triangles.  Note that the third
		///  and fourth triangles share one name, so that if either or both triangles intersects the
		///  viewing/clipping volume, only one hit will be registered.
		/// </summary>
		private static void SelectObjects() {
			uint[] selectBuf = new uint[BUFSIZE];
			int hits;

			glSelectBuffer(BUFSIZE, selectBuf);
			glRenderMode(GL_SELECT);

			glInitNames();
			glPushName(0);

			glPushMatrix();
				glMatrixMode(GL_PROJECTION);
				glLoadIdentity();
				glOrtho(0.0f, 5.0f, 0.0f, 5.0f, 0.0f, 10.0f);
				glMatrixMode(GL_MODELVIEW);
				glLoadIdentity();
				glLoadName(1);
				DrawTriangle(2.0f, 2.0f, 3.0f, 2.0f, 2.5f, 3.0f, -5.0f);
				glLoadName(2);
				DrawTriangle(2.0f, 7.0f, 3.0f, 7.0f, 2.5f, 8.0f, -5.0f);
				glLoadName(3);
				DrawTriangle(2.0f, 2.0f, 3.0f, 2.0f, 2.5f, 3.0f, 0.0f);
				DrawTriangle(2.0f, 2.0f, 3.0f, 2.0f, 2.5f, 3.0f, -10.0f);
			glPopMatrix();
			glFlush();

			hits = glRenderMode(GL_RENDER);
			ProcessHits(hits, selectBuf);
		}
		#endregion SelectObjects()

	}
}