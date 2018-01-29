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
[assembly: AssemblyDescription("Redbook Texturesurf")]
[assembly: AssemblyProduct("Redbook Texturesurf")]
[assembly: AssemblyTitle("Redbook Texturesurf")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace RedbookExamples {
	/// <summary>
	/// Redbook Texturesurf -- Texture Coordinates (http://www.opengl.org/developers/code/examples/redbook/redbook.html)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class RedbookTexturesurf : Model {
		// --- Fields ---
		#region Private Fields
		private const int imageWidth = 64;
		private const int imageHeight = 64;
		private static byte[] image = new byte[3 * imageWidth * imageHeight];

		private static float[/*4*/,/*4*/, /*3*/] ctrlpoints = {
			{
				{-1.5f, -1.5f, 4.0f},
				{-0.5f, -1.5f, 2.0f},
				{0.5f, -1.5f, -1.0f},
				{1.5f, -1.5f, 2.0f}
			},
			{
				{-1.5f, -0.5f, 1.0f},
				{-0.5f, -0.5f, 3.0f},
				{0.5f, -0.5f, 0.0f},
				{1.5f, -0.5f, -1.0f}
			},
			{
				{-1.5f, 0.5f, 4.0f},
				{-0.5f, 0.5f, 0.0f},
				{0.5f, 0.5f, 3.0f},
				{1.5f, 0.5f, 4.0f}
			},
			{
				{-1.5f, 1.5f, -2.0f},
				{-0.5f, 1.5f, -2.0f},
				{0.5f, 1.5f, 0.0f},
				{1.5f, 1.5f, -1.0f}
			}
		};

		private static float[/*2*/, /*2*/, /*2*/] texpts = {
			{
				{0.0f, 0.0f},
				{0.0f, 1.0f}
			},
			{
				{1.0f, 0.0f},
				{1.0f, 1.0f}
			}
		};
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Example title.
		/// </summary>
		public override string Title {
			get {
				return "Redbook Texturesurf -- Texture Coordinates";
			}
		}

		/// <summary>
		/// Example description.
		/// </summary>
		public override string Description {
			get {
				return "This program uses evaluators to generate a curved surface and automatically generated texture coordinates.";
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
			App.Run(new RedbookTexturesurf());											// Run Our Example As A Windows Forms Application
		}
		#endregion Main()

		// --- Basecode Methods ---
		#region Initialize()
		/// <summary>
		/// Overrides OpenGL's initialization.
		/// </summary>
		public override void Initialize() {
			float[] cp = new float[4 * 4 * 3];
			float[] tp = new float[2 * 2 * 2];
			byte[] id = new byte[3 * imageWidth * imageHeight];

			int cnt = 0;
			for(int i = 0; i < 4; i++) {
				for(int j = 0; j < 4; j++) {
					for(int k = 0; k < 3; k++) {
						cp[cnt] = ctrlpoints[i, j, k];
						cnt++;
					}
				}
			}
			glMap2f(GL_MAP2_VERTEX_3, 0, 1, 3, 4, 0, 1, 12, 4, cp);

			cnt = 0;
			for(int i = 0; i < 2; i++) {
				for(int j = 0; j < 2; j++) {
					for(int k = 0; k < 2; k++) {
						tp[cnt] = texpts[i, j, k];
						cnt++;
					}
				}
			}
			glMap2f(GL_MAP2_TEXTURE_COORD_2, 0, 1, 2, 2, 0, 1, 4, 2, tp);
			glEnable(GL_MAP2_TEXTURE_COORD_2);
			glEnable(GL_MAP2_VERTEX_3);
			glMapGrid2f(20, 0.0f, 1.0f, 20, 0.0f, 1.0f);
			MakeImage();
			glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_DECAL);
			glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
			glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
			glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
			glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
			glTexImage2D(GL_TEXTURE_2D, 0, (int) GL_RGB, imageWidth, imageHeight, 0, GL_RGB, GL_UNSIGNED_BYTE, image);
			glEnable(GL_TEXTURE_2D);
			glEnable(GL_DEPTH_TEST);
			glShadeModel(GL_FLAT);
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws Redbook Texturesurf scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
			glColor3f(1.0f, 1.0f, 1.0f);
			glEvalMesh2(GL_FILL, 0, 20, 0, 20);
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
			if(width <= height) {
				glOrtho(-4.0f, 4.0f, -4.0f * (float) height / (float) width, 4.0f * (float) height / (float) width, -4.0f, 4.0f);
			}
			else {
				glOrtho(-4.0f * (float) width / (float) height, 4.0f * (float) width / (float) height, -4.0f, 4.0f, -4.0f, 4.0f);
			}
			glMatrixMode(GL_MODELVIEW);
			glLoadIdentity();
			glRotatef(85.0f, 1.0f, 1.0f, 1.0f);
		}
		#endregion Reshape(int width, int height)

		// --- Example Methods ---
		#region MakeImage()
		/// <summary>
		/// Makes the texture image.
		/// </summary>
		private static void MakeImage() {
			int i, j;
			float ti, tj;

			for(i = 0; i < imageWidth; i++) {
				ti = 2.0f * 3.14159265f * i / imageWidth;
				for(j = 0; j < imageHeight; j++) {
					tj = 2.0f * 3.14159265f * j / imageHeight;
					image[3 * (imageHeight * i + j)] = (byte) (127 * (1.0 + Math.Sin(ti)));
					image[3 * (imageHeight * i + j) + 1] = (byte) (127 * (1.0 + Math.Cos(2 * tj)));
					image[3 * (imageHeight * i + j) + 2] = (byte) (127 * (1.0 + Math.Cos(ti + tj)));
				}
			}
		}
		#endregion MakeImage()
	}
}