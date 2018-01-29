#region BSD License
/*
 BSD License
Copyright (c) 2002, Amir Ghezelbash, Randy Ridge, The CsGL Development Team
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
/////////////////////////////////////////////////////////////////
//                                                             //
//  The OpenGL Basecode Used In This Project Was Created By	   //
//  Jeff Molofee ( NeHe ).  1997-2000.  If You Find This Code  //
//  Useful, Please Let Me Know.								   //
//                                                             //
//	Original Code & Tutorial Text By Andreas Löffler           //
//  Excellent Job Andreas!									   //
//                                                             //
//  Code Heavily Modified By Rob Fletcher ( rpf1@york.ac.uk )  //
//  Proper Image Structure, Better Blitter Code, Misc Fixes    //
//  Thanks Rob!												   //
//															   //
//	0% CPU Usage While Minimized Thanks To Jim Strong		   //
//  ( jim@scn.net ).  Thanks Jim!                              //
//                                                             //
//  This Code Also Has The ATI Fullscreen Fix!                 //
//                                                             //
//  Visit Me At nehe.gamedev.net							   //
//                                                             //
/////////////////////////////////////////////////////////////////
#endregion Original Credits / License

using CsGL.Basecode;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

#region AssemblyInfo
[assembly: AssemblyCompany("The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyCopyright("2002 The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyDescription("NeHe Lesson 29")]
[assembly: AssemblyProduct("NeHe Lesson 29")]
[assembly: AssemblyTitle("NeHe Lesson 29")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace NeHeLessons {
	/// <summary>
	/// NeHe Lesson 29 --  Blitter Function & .RAW Texture Loading (http://nehe.gamedev.net)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class NeHeLesson29 : Model {
		// --- Fields ---
		#region Private Fields
		private static float xrot;														// X Rotation
		private static float yrot;														// Y Rotation
		private static float zrot;														// Z Rotation

		private static uint[] texture = new uint[1];									// Storage For 1 Texture

		private struct TextureImage {													// Structure For The .RAW Image As A Texture
			public int width;															// Width Of Image In Pixels
			public int height;															// Height Of Image In Pixels
			public int format;															// Number Of Bytes Per Pixel
			public byte[] data;															// Texture Data
		};

		private static TextureImage t1;													// A TextureImage
		private static TextureImage t2;													// Another TextureImage
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Lesson title.
		/// </summary>
		public override string Title {
			get {
				return "NeHe Lesson 29 --  Blitter Function & .RAW Texture Loading";
			}
		}

		/// <summary>
		/// Lesson description.
		/// </summary>
		public override string Description {
			get {
				return "This lesson demonstrates how to load .RAW images for use as textures, as well as, how to write your own blitter routine to modify textures after they have been loaded.  You can copy sections of the first texture into a second texture, you can blend textures together, and you can stretch textures.  Here we load two textures and composite one on top of the other.";
			}
		}

		/// <summary>
		/// Lesson URL.
		/// </summary>
		public override string Url {
			get {
				return "http://nehe.gamedev.net/tutorials/lesson.asp?l=29";
			}
		}
		#endregion Public Properties

		// --- Entry Point ---
		#region Main()
		/// <summary>
		/// Application's entry point, runs this NeHe lesson.
		/// </summary>
		public static void Main() {														// Entry Point
			App.Run(new NeHeLesson29());												// Run Our NeHe Lesson As A Windows Forms Application
		}
		#endregion Main()

		// --- Basecode Methods ---
		#region Initialize()
		/// <summary>
		/// Overrides OpenGL's initialization.
		/// </summary>
		public override void Initialize() {
			base.Initialize();															// Run The Base Initialization
			glEnable(GL_TEXTURE_2D);													// Enable Texture Mapping
			glDepthFunc(GL_LESS);														// The Type Of Depth Test To Do

			t1 = AllocateTextureBuffer(256, 256, 4);									// Get An Image Structure
			ReadTextureData(@"..\..\data\NeHeLesson29\Monitor.raw", ref t1);			// Read In The Texture

			t2 = AllocateTextureBuffer(256, 256, 4);									// Second Image Structure
			ReadTextureData(@"..\..\data\NeHeLesson29\GL.raw", ref t2);					// Read In The Texture

			Blit(t2, ref t1, 127, 127, 128, 128, 64, 64, true, 127);					// Call The Blitter Routine

			BuildTexture(t1);															// Load The Texture Map Into Texture Memory
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws NeHe Lesson 29 scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);							// Clear Screen And Depth Buffer
			glLoadIdentity();															// Reset The Current Modelview Matrix

			glTranslatef(0.0f, 0.0f, -5.0f);											// Translate Into The Screen 5.0

			glRotatef(xrot, 1.0f, 0.0f, 0.0f);											// Rotate X Axis
			glRotatef(yrot, 0.0f, 1.0f, 0.0f);											// Rotate Y Axis
			glRotatef(zrot, 0.0f, 0.0f, 1.0f);											// Rotate Z Axis

			glBindTexture(GL_TEXTURE_2D, texture[0]);									// Select The Texture

			glBegin(GL_QUADS);															// Begin Drawing With Quads
				// Front Face
				glNormal3f(0.0f, 0.0f, 1.0f);
				glTexCoord2f(1.0f, 1.0f); glVertex3f( 1.0f,  1.0f,  1.0f);
				glTexCoord2f(0.0f, 1.0f); glVertex3f(-1.0f,  1.0f,  1.0f);
				glTexCoord2f(0.0f, 0.0f); glVertex3f(-1.0f, -1.0f,  1.0f);
				glTexCoord2f(1.0f, 0.0f); glVertex3f( 1.0f, -1.0f,  1.0f);
				// Back Face
				glNormal3f(0.0f, 0.0f,-1.0f);
				glTexCoord2f(1.0f, 1.0f); glVertex3f(-1.0f,  1.0f, -1.0f);
				glTexCoord2f(0.0f, 1.0f); glVertex3f( 1.0f,  1.0f, -1.0f);
				glTexCoord2f(0.0f, 0.0f); glVertex3f( 1.0f, -1.0f, -1.0f);
				glTexCoord2f(1.0f, 0.0f); glVertex3f(-1.0f, -1.0f, -1.0f);
				// Top Face
				glNormal3f(0.0f, 1.0f, 0.0f);
				glTexCoord2f(1.0f, 1.0f); glVertex3f( 1.0f,  1.0f, -1.0f);
				glTexCoord2f(0.0f, 1.0f); glVertex3f(-1.0f,  1.0f, -1.0f);
				glTexCoord2f(0.0f, 0.0f); glVertex3f(-1.0f,  1.0f,  1.0f);
				glTexCoord2f(1.0f, 0.0f); glVertex3f( 1.0f,  1.0f,  1.0f);
				// Bottom Face
				glNormal3f(0.0f,-1.0f, 0.0f);
				glTexCoord2f(0.0f, 0.0f); glVertex3f( 1.0f, -1.0f,  1.0f);
				glTexCoord2f(1.0f, 0.0f); glVertex3f(-1.0f, -1.0f,  1.0f);
				glTexCoord2f(1.0f, 1.0f); glVertex3f(-1.0f, -1.0f, -1.0f);
				glTexCoord2f(0.0f, 1.0f); glVertex3f( 1.0f, -1.0f, -1.0f);
				// Right Face
				glNormal3f(1.0f, 0.0f, 0.0f);
				glTexCoord2f(1.0f, 0.0f); glVertex3f( 1.0f, -1.0f, -1.0f);
				glTexCoord2f(1.0f, 1.0f); glVertex3f( 1.0f,  1.0f, -1.0f);
				glTexCoord2f(0.0f, 1.0f); glVertex3f( 1.0f,  1.0f,  1.0f);
				glTexCoord2f(0.0f, 0.0f); glVertex3f( 1.0f, -1.0f,  1.0f);
				// Left Face
				glNormal3f(-1.0f, 0.0f, 0.0f);
				glTexCoord2f(0.0f, 0.0f); glVertex3f(-1.0f, -1.0f, -1.0f);
				glTexCoord2f(1.0f, 0.0f); glVertex3f(-1.0f, -1.0f,  1.0f);
				glTexCoord2f(1.0f, 1.0f); glVertex3f(-1.0f,  1.0f,  1.0f);
				glTexCoord2f(0.0f, 1.0f); glVertex3f(-1.0f,  1.0f, -1.0f);
			glEnd();																	// Done Drawing Quads

			xrot += 0.3f;																// Increase X Axis Rotation
			yrot += 0.2f;																// Increase Y Axis Rotation
			zrot += 0.4f;																// Increase Z Axis Rotation
		}
		#endregion Draw()

		// --- Lesson Methods ---
		#region TextureImage AllocateTextureBuffer(int w, int h, int f)
		/// <summary>
		/// Allocate an image structure and allocates its memory requirements.
		/// </summary>
		/// <param name="w">The width.</param>
		/// <param name="h">The height.</param>
		/// <param name="f">The format (bpp).</param>
		/// <returns>A TextureImage.</returns>
		private TextureImage AllocateTextureBuffer(int w, int h, int f) {
			TextureImage tmp;															// Temporary Image Struct

			tmp.width = w;																// Set Width
			tmp.height = h;																// Set Height
			tmp.format = f;																// Set Format
			tmp.data = new byte[(w * h * f)];											// Initialize data Array

			return tmp;
		}
		#endregion TextureImage AllocateTextureBuffer(int w, int h, int f)

		#region Blit(TextureImage src, ref TextureImage dst, int src_xstart, int src_ystart, int src_width, int src_height, int dst_xstart, int dst_ystart, bool blend, int alpha)
		/// <summary>
		/// Copies a section of a texture and pastes it onto another.
		/// </summary>
		/// <param name="src">The source TextureImage.</param>
		/// <param name="dst">The destination TextureImage.</param>
		/// <param name="src_xstart">The X-axis start position for the copy.</param>
		/// <param name="src_ystart">The Y-axis start position for the copy.</param>
		/// <param name="src_width">The width of the section to copy.</param>
		/// <param name="src_height">The height of the section to copy.</param>
		/// <param name="dst_xstart">The X-axis position of the destination of the copied section.</param>
		/// <param name="dst_ystart">The Y-axis position of the destination of the copied section.</param>
		/// <param name="blend">If true, the two images will be blended.</param>
		/// <param name="alpha">Sets how tranparent the copied image will be when it's mapped onto the destination image.  0 is completely clear and 255 is solid.</param>
		private void Blit(TextureImage src, ref TextureImage dst, int src_xstart, int src_ystart, int src_width, int src_height, int dst_xstart, int dst_ystart, bool blend, int alpha) {
			int i, j;																	// Generic Loop Variables
			int arrayposition;															// Current Position In The Single-Dimensional Array
			int spos1, spos2;															// Current Position In The Source Multi-Dimensional Array
			int dpos1, dpos2;															// Current Position In The Destination Multi-Dimensional Array

			// Clamp Alpha If Value Is Out Of Range
			if(alpha > 255) {
				alpha = 255;
			}
			else if(alpha < 0) {
				alpha = 0;
			}

			// Temporary Source And Destination Multi-Dimensional Arrays
			byte[ , ] stmp = new byte[src.height, (src.width * src.format)];
			byte[ , ] dtmp = new byte[dst.height, (dst.width * dst.format)];

			// Read Source data Into Multi-Dimensional Array
			arrayposition = 0;															// Reset Array Position
			for(i = 0; i < src.height; i++) {											// Loop Through The Height
				for(j = 0; j < (src.width * src.format); j++) {							// Loop Through The Width & Format
					stmp[i, j] = src.data[arrayposition];								// Read In A Byte
					arrayposition++;													// Increment Array Position
				}
			}

			// Read Destination data Into Multi-Dimensional Array
			arrayposition = 0;															// Reset Array Position
			for(i = 0; i < dst.height; i++) {											// Loop Through The Height
				for(j = 0; j < (dst.width * dst.format); j++) {							// Loop Through The Width & Format
					dtmp[i, j] = dst.data[arrayposition];								// Read In A Byte
					arrayposition++;													// Increment Array Position
				}
			}

			spos1 = src_ystart;															// Starting Row Of Source
			spos2 = src_xstart * src.format;											// Beginning Of Width Position

			dpos1 = dst_ystart;															// Starting Row Of Destination
			dpos2 = dst_xstart * dst.format;											// Beginning Of Width Position

			for(i = 0; i < src_height; i++) {											// Height Loop
				for(j = 0; j < (src_width * src.format); j++) {							// Width & Format Loop
					if(blend) {															// If Blending Is On
						// Multiply Src Data * alpha And Add Dst Data * (255 - alpha), Keep in 0-255 Range With >> 8
						dtmp[dpos1, dpos2] = (byte) (((stmp[spos1, spos2] * alpha) + (dtmp[dpos1, dpos2] * (255 - alpha))) >> 8);
					}
					else {
						// No Blending Just Do A Straight Copy
						dtmp[dpos1, dpos2] = stmp[spos1, spos2];
					}
					spos2++;															// Increment Source Width Position
					dpos2++;															// Increment Destination Width Position
				}

				spos1++;																// Increment Source Row Position
				dpos1++;																// Increment Destination Row Position

				// Set Width Position Back To Beginning
				spos2 = src_xstart * src.format;
				dpos2 = dst_xstart * dst.format;
			}

			// Put Destination Back Into Single-Dimensional data Array
			arrayposition = 0;															// Reset Array Position
			for(i = 0; i < dst.height; i++) {											// Loop Through Height
				for(j = 0; j < (dst.width * dst.format); j++) {							// Loop Through Width & Format
					dst.data[arrayposition] = dtmp[i, j];								// Stick It In The Top Of The data Array
					arrayposition++;													// Increment Array Position
				}
			}
		}
		#endregion Blit(TextureImage src, ref TextureImage dst, int src_xstart, int src_ystart, int src_width, int src_height, int dst_xstart, int dst_ystart, bool blend, int alpha)

		#region BuildTexture(TextureImage tex)
		/// <summary>
		/// Build the OpenGL texture.
		/// </summary>
		/// <param name="tex">The TextureImage to create texture from.</param>
		private void BuildTexture(TextureImage tex) {
			glGenTextures(1, texture);													// Generate 1 Texture
			
			// Create Linear Filtered Texture
			glBindTexture(GL_TEXTURE_2D, texture[0]);
			glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
			glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
			gluBuild2DMipmaps(GL_TEXTURE_2D, (int) GL_RGB8, tex.width, tex.height, GL_RGBA, GL_UNSIGNED_BYTE, tex.data);
		}
		#endregion BuildTexture(TextureImage tex)

		#region ReadTextureData(string filename, ref TextureImage buffer)
		/// <summary>
		/// Reads a .RAW file in to the allocated image buffer using data in the image structure.  Flips the image top to bottom.
		/// </summary>
		/// <param name="filename">The filename to load.</param>
		/// <param name="buffer">The TextureImage to save it to.</param>
		private void ReadTextureData(string filename, ref TextureImage buffer) {
			int i, j, k, offset;														// Generic Loop Variables
			int stride = buffer.width * buffer.format;									// Size Of A Row (Width * Bytes Per Pixel)
			byte[ , ] p = new byte[buffer.height, (buffer.width * buffer.format)];		// The Pixel Data
			FileStream stream = null;													// Our Stream
			ASCIIEncoding encoding = new ASCIIEncoding();								// Our Encoding
			BinaryReader reader = null;													// Our Reader

			try {
				// Open The File
				stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
				reader = new BinaryReader(stream, encoding);

				// Read Data Into Multi-Dimensional Array
				for(i = 0; i < buffer.height; i++) {									// Loop Through The Height
					offset = 0;															// Reset The Offset
					for(j = 0; j < buffer.width; j++) {									// Loop Through The Width
						for(k = 0; k < buffer.format - 1; k++) {						// Loop Through Format
							p[i, j + k + offset] = reader.ReadByte();					// Read A Byte
						}
						p[i, j + k + offset] = 255;										// Add An Alpha Value
						offset += buffer.format - 1;									// Calculate New Offset
					}
				}

				// Flip Bottom To Top And Put Back Into Single-Dimensional data Array
				int arrayposition = 0;
				for(i = buffer.height - 1; i >= 0; i--) {								// Loop Through Height Starting At The Bottom
					for(j = 0; j < stride; j++) {										// Loop Through Width & Format (Stride)
						buffer.data[arrayposition] = p[i, j];							// Stick It In The Top Of The data Array (Now It's Flipped)
						arrayposition++;												// Increment Array Position
					}
				}
			}
			catch(Exception e) {
				// Handle Any Exceptions While Loading Textures, Exit App
				string errorMsg = "An Error Occurred While Loading RAW Texture:\n\t" + filename + "\n" + "\n\nStack Trace:\n\t" + e.StackTrace + "\n";
				MessageBox.Show(errorMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				App.Terminate();
			}
			finally {
				if(reader != null) {
					reader.Close();														// Close The File
				}
				if(stream != null) {
					stream.Close();
				}
			}
		}
		#endregion ReadTextureData(string filename, ref Texture_Image buffer)
	}
}