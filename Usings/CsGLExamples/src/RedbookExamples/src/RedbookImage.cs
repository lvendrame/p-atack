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
using CsGL.OpenGL;
using System.Data;
using System.Reflection;
using System.Windows.Forms;

#region AssemblyInfo
[assembly: AssemblyCompany("The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyCopyright("2002 The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyDescription("Redbook Image")]
[assembly: AssemblyProduct("Redbook Image")]
[assembly: AssemblyTitle("Redbook Image")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace RedbookExamples {
	/// <summary>
	/// Redbook Image -- Drawing And Copying Pixels (http://www.opengl.org/developers/code/examples/redbook/redbook.html)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class RedbookImage : Model {
		// --- Fields ---
		#region Private Fields
		private const int checkImageWidth = 64;
		private const int checkImageHeight = 64;
		private static byte[ , , ] checkImage = new byte[checkImageHeight, checkImageWidth, 3];
		private static byte[] checkData = new byte[checkImageHeight * checkImageWidth * 3];
		private static double zoomFactor = 1.0;
		private static int wheight;
		private static int screeny;
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Example title.
		/// </summary>
		public override string Title {
			get {
				return "Redbook Image -- Drawing And Copying Pixels";
			}
		}

		/// <summary>
		/// Example description.
		/// </summary>
		public override string Description {
			get {
				return "This program demonstrates drawing pixels and shows the effect of glDrawPixels(), glCopyPixels(), and glPixelZoom().";
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
			App.Run(new RedbookImage());												// Run Our Example As A Windows Forms Application
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
			MakeCheckImage();
			glPixelStorei(GL_UNPACK_ALIGNMENT, 1);
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws Redbook Image scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			glClear(GL_COLOR_BUFFER_BIT);
			glRasterPos2i(0, 0);
			glDrawPixels(checkImageWidth, checkImageHeight, GL_RGB, GL_UNSIGNED_BYTE, checkData);
			glFlush();

			if(Model.Mouse.LeftButton) {
				screeny = wheight - Model.Mouse.Y;
				glRasterPos2i(Model.Mouse.X, screeny);
				glPixelZoom((float) zoomFactor, (float) zoomFactor);
				glCopyPixels(0, 0, checkImageWidth, checkImageHeight, GL_COLOR);
				glPixelZoom(1.0f, 1.0f);
				glFlush();
			}
		}
		#endregion Draw()

		#region InputHelp()
		/// <summary>
		/// Overrides default input help, supplying example-specific help information.
		/// </summary>
		public override void InputHelp() {
			base.InputHelp();															// Set Up The Default Input Help

			DataRow dataRow;															// Row To Add

			dataRow = InputHelpDataTable.NewRow();										// Left Mouse Button - Copy Image
			dataRow["Input"] = "Left Mouse Button";
			dataRow["Effect"] = "Hold To Copy Image";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// R - Reset Zoom Factor
			dataRow["Input"] = "R";
			dataRow["Effect"] = "Reset Copied Image Zoom";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Z - Increase Zoom
			dataRow["Input"] = "Z";
			dataRow["Effect"] = "Increase Copied Image Zoom";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// X - Decrease Zoom
			dataRow["Input"] = "X";
			dataRow["Effect"] = "Decrease Copied Image Zoom";
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

			if(KeyState[(int) Keys.R]) {												// Is R Key Being Pressed?
				KeyState[(int) Keys.R] = false;											// Mark As Handled
				zoomFactor = 1.0;														// Reset Zoom Factor
				Model.Mouse.LeftButton = false;
			}

			if(KeyState[(int) Keys.Z]) {												// Is Z Key Being Pressed?
				KeyState[(int) Keys.Z] = false;											// Mark As Handled
				zoomFactor += 0.5;														// Increase Zoom
				if(zoomFactor >= 3.0) {													// Cap Zoom
					zoomFactor = 3.0;
				}
			}
		
			if(KeyState[(int) Keys.X]) {												// Is X Key Being Pressed?
				KeyState[(int) Keys.X] = false;											// Mark As Handled
				zoomFactor -= 0.5;														// Decrease Zoom
				if(zoomFactor <= 0.5) {													// Cap Zoom
					zoomFactor = 0.5;
				}
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
			wheight = height;
			glMatrixMode(GL_PROJECTION);
			glLoadIdentity();
			gluOrtho2D(0.0, (double) width, 0.0, (double) height);
			glMatrixMode(GL_MODELVIEW);
			glLoadIdentity();
		}
		#endregion Reshape(int width, int height)

		// --- Example Methods ---
		#region MakeCheckImage()
		/// <summary>
		/// Makes the checkerboard image.
		/// </summary>
		private static void MakeCheckImage() {
			int i, j, c;

			for(i = 0; i < checkImageHeight; i++) {
				for(j = 0; j < checkImageWidth; j++) {
					if(((i & 0x8) == 0) ^ ((j & 0x8) == 0)) {
						c = 255;
					}
					else {
						c = 0;
					}
					checkImage[i, j, 0] = (byte) c;
					checkImage[i, j, 1] = (byte) c;
					checkImage[i, j, 2] = (byte) c;
				}
			}

			// Stuff the multidimensional array into a single dimensional array,
			// such that we can pass it to glDrawPixels...  HACK!
			int cnt = 0;
			for(i = 0; i < checkImageHeight; i++) {
				for(j = 0; j < checkImageWidth; j++) {
					for(int k = 0; k < 3; k++) {
						checkData[cnt] = checkImage[i, j, k];
						cnt++;
					}
				}
			}
		}
		#endregion MakeCheckImage()
	}
}
