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
using System.Data;
using System.Reflection;
using System.Windows.Forms;

#region AssemblyInfo
[assembly: AssemblyCompany("The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyCopyright("2002 The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyDescription("Redbook Planet")]
[assembly: AssemblyProduct("Redbook Planet")]
[assembly: AssemblyTitle("Redbook Planet")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace RedbookExamples {
	/// <summary>
	/// Redbook Planet -- Composite Transformations (http://www.opengl.org/developers/code/examples/redbook/redbook.html)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class RedbookPlanet : Model {
		// --- Fields ---
		#region Private Fields
		private static int year = 0, day = 0;
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Example title.
		/// </summary>
		public override string Title {
			get {
				return "Redbook Planet -- Composite Transformations";
			}
		}

		/// <summary>
		/// Example description.
		/// </summary>
		public override string Description {
			get {
				return "This program shows how to composite modeling transformations to draw translated and rotated models.  Interaction: pressing the d and y keys (day and year) alters the rotation of the planet around the sun.";
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
			App.Run(new RedbookPlanet());												// Run Our Example As A Windows Forms Application
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
		/// Draws Redbook Planet scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			glClear(GL_COLOR_BUFFER_BIT);
			glColor3f(1.0f, 1.0f, 1.0f);

			glPushMatrix();
				glutWireSphere(1.0f, 20, 16);											// Draw Sun
				glRotatef((float) year, 0.0f, 1.0f, 0.0f);
				glTranslatef(2.0f, 0.0f, 0.0f);
				glRotatef((float) day, 0.0f, 1.0f, 0.0f);
				glutWireSphere(0.2f, 10, 8);											// Draw Smaller Planet
			glPopMatrix();
		}
		#endregion Draw()

		#region InputHelp()
		/// <summary>
		/// Overrides default input help, supplying example-specific help information.
		/// </summary>
		public override void InputHelp() {
			base.InputHelp();															// Set Up The Default Input Help

			DataRow dataRow;															// Row To Add

			dataRow = InputHelpDataTable.NewRow();										// D - Increase Day
			dataRow["Input"] = "D";
			dataRow["Effect"] = "Increase Day";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// C - Decrease Day
			dataRow["Input"] = "C";
			dataRow["Effect"] = "Decrease Day";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Y - Increase Year
			dataRow["Input"] = "Y";
			dataRow["Effect"] = "Increase Year";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// H - Decrease Year
			dataRow["Input"] = "H";
			dataRow["Effect"] = "Decrease Year";
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

			if(KeyState[(int) Keys.D]) {												// Is D Key Being Pressed?
				KeyState[(int) Keys.D] = false;											// Mark As Handled
				day = (day + 10) % 360;													// Increase Day
			}

			if(KeyState[(int) Keys.C]) {												// Is C Key Being Pressed?
				KeyState[(int) Keys.C] = false;											// Mark As Handled
				day = (day - 10) % 360;													// Decrease Day
			}

			if(KeyState[(int) Keys.Y]) {												// Is Y Key Being Pressed?
				KeyState[(int) Keys.Y] = false;											// Mark As Handled
				year = (year + 5) % 360;												// Increase Year
			}

			if(KeyState[(int) Keys.H]) {												// Is H Key Being Pressed?
				KeyState[(int) Keys.H] = false;											// Mark As Handled
				year = (year - 5) % 360;												// Decrease Year
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
			gluPerspective(60.0f, (float) width / (float) height, 1.0f, 20.0f);
			glMatrixMode(GL_MODELVIEW);
			glLoadIdentity();
			gluLookAt(0.0f, 0.0f, 5.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f);
		}
		#endregion Reshape(int width, int height)
	}
}