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
[assembly: AssemblyDescription("Redbook Mipmap")]
[assembly: AssemblyProduct("Redbook Mipmap")]
[assembly: AssemblyTitle("Redbook Mipmap")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace RedbookExamples {
	/// <summary>
	/// Redbook Mipmap -- Mipmaps For Texture Mapping (http://www.opengl.org/developers/code/examples/redbook/redbook.html)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class RedbookMipmap : Model {
		// --- Fields ---
		#region Private Fields
		private static byte[] mipmapImage32 = new byte[32 * 32 * 4];
		private static byte[] mipmapImage16 = new byte[16 * 16 * 4];
		private static byte[] mipmapImage8 = new byte[8 * 8 * 4];
		private static byte[] mipmapImage4 = new byte[4 * 4 * 4];
		private static byte[] mipmapImage2 = new byte[2 * 2 * 4];
		private static byte[] mipmapImage1 = new byte[1 * 1 * 4];
		private static uint[] texName = new uint[1];
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Example title.
		/// </summary>
		public override string Title {
			get {
				return "Redbook Mipmap -- Mipmaps For Texture Mapping";
			}
		}

		/// <summary>
		/// Example description.
		/// </summary>
		public override string Description {
			get {
				return "This program demonstrates using mipmaps for texture maps.  To overtly show the effect of mipmaps, each mipmap reduction level has a solidly colored, contrasting texture image.  Thus, the quadrilateral which is drawn is drawn with several different colors.";
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
			App.Run(new RedbookMipmap());												// Run Our Example As A Windows Forms Application
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

			glTranslatef(0.0f, 0.0f, -3.6f);
			MakeImages();
			glPixelStorei(GL_UNPACK_ALIGNMENT, 1);

			glGenTextures(1, texName);
			glBindTexture(GL_TEXTURE_2D, texName[0]);
			
			glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
			glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
			glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
			glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST_MIPMAP_NEAREST);

			glTexImage2D(GL_TEXTURE_2D, 0, (int) GL_RGBA, 32, 32, 0, GL_RGBA, GL_UNSIGNED_BYTE, mipmapImage32);
			glTexImage2D(GL_TEXTURE_2D, 1, (int) GL_RGBA, 16, 16, 0, GL_RGBA, GL_UNSIGNED_BYTE, mipmapImage16);
			glTexImage2D(GL_TEXTURE_2D, 2, (int) GL_RGBA, 8, 8, 0, GL_RGBA, GL_UNSIGNED_BYTE, mipmapImage8);
			glTexImage2D(GL_TEXTURE_2D, 3, (int) GL_RGBA, 4, 4, 0, GL_RGBA, GL_UNSIGNED_BYTE, mipmapImage4);
			glTexImage2D(GL_TEXTURE_2D, 4, (int) GL_RGBA, 2, 2, 0, GL_RGBA, GL_UNSIGNED_BYTE, mipmapImage2);
			glTexImage2D(GL_TEXTURE_2D, 5, (int) GL_RGBA, 1, 1, 0, GL_RGBA, GL_UNSIGNED_BYTE, mipmapImage1);

			glTexEnvf(GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_DECAL);
			glEnable(GL_TEXTURE_2D);
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws Redbook Mipmap scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
			glBindTexture(GL_TEXTURE_2D, texName[0]);
			glBegin(GL_QUADS);
				glTexCoord2f(0.0f, 0.0f); glVertex3f(-2.0f, -1.0f, 0.0f);
				glTexCoord2f(0.0f, 8.0f); glVertex3f(-2.0f, 1.0f, 0.0f);
				glTexCoord2f(8.0f, 8.0f); glVertex3f(2000.0f, 1.0f, -6000.0f);
				glTexCoord2f(8.0f, 0.0f); glVertex3f(2000.0f, -1.0f, -6000.0f);
			glEnd();
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
			gluPerspective(60.0f, (float)width / (float) height, 1.0f, 30000.0f);
			glMatrixMode(GL_MODELVIEW);
			glLoadIdentity();
		}
		#endregion Reshape(int width, int height)

		// --- Example Methods ---
		#region MakeImages()
		/// <summary>
		/// Makes the mipmaps.
		/// </summary>
		private static void MakeImages() {
			int i, j, k, cnt;;
			byte[ , , ] amipmapImage32 = new byte[32, 32, 4];
			byte[ , , ] amipmapImage16 = new byte[16, 16, 4];
			byte[ , , ] amipmapImage8 = new byte[8, 8, 4];
			byte[ , , ] amipmapImage4 = new byte[4, 4, 4];
			byte[ , , ] amipmapImage2 = new byte[2, 2, 4];
			byte[ , , ] amipmapImage1 = new byte[1, 1, 4];

			for(i = 0; i < 32; i++) {
				for(j = 0; j < 32; j++) {
					amipmapImage32[i, j, 0] = 255;
					amipmapImage32[i, j, 1] = 255;
					amipmapImage32[i, j, 2] = 0;
					amipmapImage32[i, j, 3] = 255;
				}
			}

			// Stuff into single dimensional array
			cnt = 0;
			for(i = 0; i < 32; i++) {
				for(j = 0; j < 32; j++) {
					for(k = 0; k < 4; k++) {
						mipmapImage32[cnt] = amipmapImage32[i, j, k];
						cnt++;
					}
				}
			}

			for(i = 0; i < 16; i++) {
				for(j = 0; j < 16; j++) {
					amipmapImage16[i, j, 0] = 255;
					amipmapImage16[i, j, 1] = 0;
					amipmapImage16[i, j, 2] = 255;
					amipmapImage16[i, j, 3] = 255;
				}
			}

			// Stuff into single dimensional array
			cnt = 0;
			for(i = 0; i < 16; i++) {
				for(j = 0; j < 16; j++) {
					for(k = 0; k < 4; k++) {
						mipmapImage16[cnt] = amipmapImage16[i, j, k];
						cnt++;
					}
				}
			}

			for(i = 0; i < 8; i++) {
				for(j = 0; j < 8; j++) {
					amipmapImage8[i, j, 0] = 255;
					amipmapImage8[i, j, 1] = 0;
					amipmapImage8[i, j, 2] = 0;
					amipmapImage8[i, j, 3] = 255;
				}
			}

			// Stuff into single dimensional array
			cnt = 0;
			for(i = 0; i < 8; i++) {
				for(j = 0; j < 8; j++) {
					for(k = 0; k < 4; k++) {
						mipmapImage8[cnt] = amipmapImage8[i, j, k];
						cnt++;
					}
				}
			}

			for(i = 0; i < 4; i++) {
				for(j = 0; j < 4; j++) {
					amipmapImage4[i, j, 0] = 0;
					amipmapImage4[i, j, 1] = 255;
					amipmapImage4[i, j, 2] = 0;
					amipmapImage4[i, j, 3] = 255;
				}
			}

			// Stuff into single dimensional array
			cnt = 0;
			for(i = 0; i < 4; i++) {
				for(j = 0; j < 4; j++) {
					for(k = 0; k < 4; k++) {
						mipmapImage4[cnt] = amipmapImage4[i, j, k];
						cnt++;
					}
				}
			}

			for(i = 0; i < 2; i++) {
				for(j = 0; j < 2; j++) {
					amipmapImage2[i, j, 0] = 0;
					amipmapImage2[i, j, 1] = 0;
					amipmapImage2[i, j, 2] = 255;
					amipmapImage2[i, j, 3] = 255;
				}
			}

			// Stuff into single dimensional array
			cnt = 0;
			for(i = 0; i < 2; i++) {
				for(j = 0; j < 2; j++) {
					for(k = 0; k < 4; k++) {
						mipmapImage2[cnt] = amipmapImage2[i, j, k];
						cnt++;
					}
				}
			}

			amipmapImage1[0, 0, 0] = 255;
			amipmapImage1[0, 0, 1] = 255;
			amipmapImage1[0, 0, 2] = 255;
			amipmapImage1[0, 0, 3] = 255;

			// Stuff into single dimensional array
			cnt = 0;
			for(i = 0; i < 1; i++) {
				for(j = 0; j < 1; j++) {
					for(k = 0; k < 4; k++) {
						mipmapImage1[cnt] = amipmapImage1[i, j, k];
						cnt++;
					}
				}
			}
		}
		#endregion MakeImages()
	}
}