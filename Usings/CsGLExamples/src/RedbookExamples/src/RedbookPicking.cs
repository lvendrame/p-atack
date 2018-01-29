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
using System.Data;
using System.Reflection;

#region AssemblyInfo
[assembly: AssemblyCompany("The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyCopyright("2002 The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyDescription("Redbook Picking")]
[assembly: AssemblyProduct("Redbook Picking")]
[assembly: AssemblyTitle("Redbook Picking")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace RedbookExamples {
	/// <summary>
	/// Redbook Picking -- Picking (http://www.opengl.org/developers/code/examples/redbook/redbook.html)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class RedbookPicking : Model {
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
				return "Redbook Picking -- Picking";
			}
		}

		/// <summary>
		/// Example description.
		/// </summary>
		public override string Description {
			get {
				return "Picking is demonstrated in this program.  In rendering mode, three overlapping rectangles are drawn.  When the left mouse button is pressed, selection mode is entered with the picking matrix.  Rectangles which are drawn under the cursor position are \"picked.\"  Pay special attention to the depth value range, which is returned.";
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
			App.Run(new RedbookPicking());												// Run Our Example As A Windows Forms Application
		}
		#endregion Main()

		// --- Basecode Methods ---
		#region Initialize()
		/// <summary>
		/// Overrides OpenGL's initialization.
		/// </summary>
		public override void Initialize() {
			glClearColor(0.0f, 0.0f, 0.0f, 0.0f);
			glEnable(GL_DEPTH_TEST);
			glShadeModel(GL_FLAT);
			glDepthRange(0.0f, 1.0f);													// The Default Z Mapping
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws Redbook Picking scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
			DrawRects(GL_RENDER);
			glFlush();
		}
		#endregion Draw()

		#region InputHelp()
		/// <summary>
		/// Overrides default input help, supplying example-specific help information.
		/// </summary>
		public override void InputHelp() {
			base.InputHelp();															// Set Up The Default Input Help

			DataRow dataRow;															// Row To Add

			dataRow = InputHelpDataTable.NewRow();										// Left Mouse Button - Picks A Rectangle
			dataRow["Input"] = "Left Mouse Button";
			dataRow["Effect"] = "Picks A Rectangle";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);
		}
		#endregion InputHelp()

		#region ProcessInput()
		/// <summary>
		/// Overrides default input handling, adding example-specific input handling.
		/// </summary>
		public override void ProcessInput() {
			base.ProcessInput();														// Handle The Default Basecode Keys

			if(Model.Mouse.LeftButton) {												// If Left Mouse Button Is Being Pressed
				Model.Mouse.LeftButton = false;											// Mark It As Handled
				PickRects();															// Update Selection
			}
		}
		#endregion ProcessInput()

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
			glOrtho(0.0f, 8.0f, 0.0f, 8.0f, -0.5f, 2.5f);
			glMatrixMode(GL_MODELVIEW);
			glLoadIdentity();
		}
		#endregion Reshape(int width, int height)

		// --- Example Methods ---
		#region DrawRects(uint mode)
		/// <summary>
		/// Draws the rectangles.
		/// </summary>
		/// <param name="mode">The selection mode.</param>
		private static void DrawRects(uint mode) {
			// The three rectangles are drawn.  In selection mode, each rectangle is given
			// the same name.  Note that each rectangle is drawn with a different z value.
			if(mode == GL_SELECT) {
				glLoadName(1);
			}
			glBegin(GL_QUADS);
				glColor3f(1.0f, 1.0f, 0.0f);
				glVertex3i(2, 0, 0);
				glVertex3i(2, 6, 0);
				glVertex3i(6, 6, 0);
				glVertex3i(6, 0, 0);
			glEnd();

			if(mode == GL_SELECT) {
				glLoadName(2);
			}
			glBegin(GL_QUADS);
				glColor3f(0.0f, 1.0f, 1.0f);
				glVertex3i(3, 2, -1);
				glVertex3i(3, 8, -1);
				glVertex3i(8, 8, -1);
				glVertex3i(8, 2, -1);
			glEnd();

			if(mode == GL_SELECT) {
				glLoadName(3);
			}
			glBegin(GL_QUADS);
				glColor3f(1.0f, 0.0f, 1.0f);
				glVertex3i(0, 2, -2);
				glVertex3i(0, 7, -2);
				glVertex3i(5, 7, -2);
				glVertex3i(5, 2, -2);
			glEnd();
		}
		#endregion DrawRects(uint mode)

		#region PickRects()
		/// <summary>
		/// Updates pick selection.
		/// </summary>
		private static void PickRects() {
			// sets up selection mode, name stack,  and projection matrix for picking.  Then the objects are drawn
			uint[] selectBuf = new uint[BUFSIZE];
			int hits;
			int[] viewport = new int[4];

			glGetIntegerv(GL_VIEWPORT, viewport);

			glSelectBuffer(BUFSIZE, selectBuf);
			glRenderMode(GL_SELECT);

			glInitNames();
			glPushName(0);

			glMatrixMode(GL_PROJECTION);
			glPushMatrix();
				glLoadIdentity();
				// create 5x5 pixel picking region near cursor location
				gluPickMatrix((double) Model.Mouse.X, (double) (viewport[3] - Model.Mouse.Y), 5.0, 5.0, viewport);
				glOrtho(0.0f, 8.0f, 0.0f, 8.0f, -0.5f, 2.5f);
				DrawRects(GL_SELECT);
			glPopMatrix();
			glFlush();

			hits = glRenderMode(GL_RENDER);
			ProcessHits(hits, selectBuf);
		}
		#endregion PickRects()

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
					Console.Write("{0} ", ptr[i]);
					i++;
				}
				Console.Write("\n");
			}
			Console.Write("\n");
		}
		#endregion ProcessHits(int hits, uint[] buffer)
	}
}