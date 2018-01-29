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
[assembly: AssemblyDescription("Redbook Picksquare")]
[assembly: AssemblyProduct("Redbook Picksquare")]
[assembly: AssemblyTitle("Redbook Picksquare")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace RedbookExamples {
	/// <summary>
	/// Redbook Picksquare -- Multiple Name Picking (http://www.opengl.org/developers/code/examples/redbook/redbook.html)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class RedbookPicksquare : Model {
		// --- Fields ---
		#region Private Fields
		private const int BUFSIZE = 512;
		private static int[ , ] board = new int[3, 3];									// Amount Of Color For Each Square
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Example title.
		/// </summary>
		public override string Title {
			get {
				return "Redbook Picksquare -- Multiple Name Picking";
			}
		}

		/// <summary>
		/// Example description.
		/// </summary>
		public override string Description {
			get {
				return "Use of multiple names and picking are demonstrated.  A 3x3 grid of squares is drawn.  When the left mouse button is pressed, all squares under the cursor position have their color changed.";
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
			App.Run(new RedbookPicksquare());											// Run Our Example As A Windows Forms Application
		}
		#endregion Main()

		// --- Basecode Methods ---
		#region Initialize()
		/// <summary>
		/// Overrides OpenGL's initialization.
		/// </summary>
		public override void Initialize() {
			// Clear color value for every square on the board
			for(int i = 0; i < 3; i++) {
				for(int j = 0; j < 3; j ++) {
					board[i, j] = 0;
				}
			}
			glClearColor(0.0f, 0.0f, 0.0f, 0.0f);
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws Redbook Picksquare scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			glClear(GL_COLOR_BUFFER_BIT);
			DrawSquares(GL_RENDER);
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

			dataRow = InputHelpDataTable.NewRow();										// Left Mouse Button - Picks A Square
			dataRow["Input"] = "Left Mouse Button";
			dataRow["Effect"] = "Picks A Square";
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
				PickSquares();															// Update Selection
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
			gluOrtho2D(0.0, 3.0, 0.0, 3.0);
			glMatrixMode(GL_MODELVIEW);
			glLoadIdentity();
		}
		#endregion Reshape(int width, int height)

		// --- Example Methods ---
		#region DrawSquare(uint mode)
		/// <summary>
		/// Draws the squares.
		/// </summary>
		/// <param name="mode">The selection mode.</param>
		private static void DrawSquares(uint mode) {
			// The nine squares are drawn.  In selection mode, each
			// square is given two names:  one for the row and the
			// other for the column on the grid.  The color of each
			// square is determined by its position on the grid, and
			// the value in the board[ , ] array.

			for(uint i = 0; i < 3; i++) {
				if(mode == GL_SELECT) {
					glLoadName(i);
				}
				for(uint j = 0; j < 3; j ++) {
					if(mode == GL_SELECT) {
						glPushName(j);
					}
					glColor3f((float) i / 3.0f, (float) j / 3.0f, (float) board[i, j] / 3.0f);
					glRecti((int) i, (int) j, (int) i + 1, (int) j + 1);
					if(mode == GL_SELECT) {
						glPopName();
					}
				}
			}
		}
		#endregion DrawSquare(uint mode)

		#region PickSquares()
		/// <summary>
		/// Updates pick selection.
		/// </summary>
		private static void PickSquares() {
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
				gluOrtho2D(0.0, 3.0, 0.0, 3.0);
				DrawSquares(GL_SELECT);
				glMatrixMode(GL_PROJECTION);
			glPopMatrix();
			glFlush();

			hits = glRenderMode(GL_RENDER);
			ProcessHits(hits, selectBuf);
		}
		#endregion PickSquares()

		#region ProcessHits(int hits, uint[] buffer)
		/// <summary>
		/// Displays hit data.
		/// </summary>
		/// <param name="hits">Number of hits.</param>
		/// <param name="buffer">The selection buffer.</param>
		private static void ProcessHits(int hits, uint[] buffer) {
			uint i, j;
			uint ii = 0, jj = 0, names;
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
					if(j == 0) {														// Set Row And Column
						ii = ptr[i];
					}
					else if(j == 1) {
						jj = ptr[i];
					}
					i++;
				}
				Console.Write("\n");
				board[ii, jj] = (board[ii, jj] + 1) % 3;
			}
			Console.Write("\n");
		}
		#endregion ProcessHits(int hits, uint[] buffer)
	}
}