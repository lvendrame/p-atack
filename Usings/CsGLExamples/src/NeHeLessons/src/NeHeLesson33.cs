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
/*
 *		This Code Was Created By Evan Pipho (Terminate) And Jeff Molofee (NeHe)
 *		If You've Found This Code Useful, Please Let Me Know.
 *		Visit My Site At nehe.gamedev.net
 */
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
[assembly: AssemblyDescription("NeHe Lesson 33")]
[assembly: AssemblyProduct("NeHe Lesson 33")]
[assembly: AssemblyTitle("NeHe Lesson 33")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace NeheLessons {
	/// <summary>
	/// NeHe Lesson 33 -- Loading Compressed And Uncompressed TGA's (http://nehe.gamedev.net)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class NeHeLesson33 : Model {
		// --- Fields ---
		#region Private Fields
		private struct Texture {														// Structure For Our Texture
			public uint width;															// Image Width
			public uint height;															// Image Height
			public uint bpp;															// Image Color Depth In Bits Per Pixel
			public byte[] imageData;													// Image Data
			public uint type;			 												// Image Type (GL_RGB Or GL_RGBA)
		};

		private struct TGA {															// Structure For A TGA
			public byte[] header ;														// First 6 Useful Bytes From The Header
			public uint height;															// Height Of Image
			public uint width;															// Width Of Image
			public uint bpp;															// Number Of Bits Per Pixel (24 Or 32)
			public uint bytesPerPixel;													// Number Of Bytes Per Pixel Used In The TGA File (3 Or 4)
			public uint imageSize;														// The Total Size Of The Image
		};

		private static TGA tga;															// TGA Image

		private static byte[] uTGAcompare = {0, 0,  2, 0, 0, 0, 0, 0, 0, 0, 0, 0};		// Uncompressed TGA Header
		private static byte[] cTGAcompare = {0, 0, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0};		// Compressed TGA Header

		private static uint[] texture  = new uint[2];									// 2 Textures
		private static Texture[] textureImage = new Texture[2];							// Storage For 2 TGA Textures

		private static float spin;														// Spin Variable
		#endregion

		#region Public Properties
		/// <summary>
		/// Lesson title.
		/// </summary>
		public override string Title {
			get {
				return "Lesson 33 -- Loading Compressed And Uncompressed TGA's";
			}
		}

		/// <summary>
		/// Lesson description.
		/// </summary>
		public override string Description {
			get {
				return "In lesson 24 you learned how to load 24/32 bit TGA's.  In this lesson you'll learn how to load uncompressed and RLE compressed TGA's.";
			}
		}

		/// <summary>
		/// Lesson URL.
		/// </summary>
		public override string Url {
			get {
				return "http://nehe.gamedev.net/tutorials/lesson.asp?l=33";
			}
		}
		#endregion Public Properties

		// --- Entry Point ---
		#region Main()
		/// <summary>
		/// Application's entry point, runs this NeHe lesson.
		/// </summary>
		public static void Main() {														// Entry Point
			App.Run(new NeHeLesson33());												// Run Our NeHe Lesson As A Windows Forms Application
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

			LoadTextures();																// Jump To Texture Loading Routine
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws NeHe Lesson s3 Scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);							// Clear The Screen And The Depth Buffer
			glLoadIdentity();															// Reset The Modelview Matrix
			glTranslatef(0.0f, 0.0f, -10.0f);											// Translate 10 Units Into The Screen

			spin += 0.05f;																// Increase Spin

			for(int loop = 0; loop < 20; loop++) {										// Loop Of 20
				glPushMatrix();															// Push The Matrix
					glRotatef(spin + loop * 18.0f, 1.0f, 0.0f, 0.0f);					// Rotate On The X-Axis (Up - Down)
					glTranslatef(-2.0f, 2.0f, 0.0f);									// Translate 2 Units Left And 2 Up
					glBindTexture(GL_TEXTURE_2D, texture[0]);							// Select The Uncompressed Texture
					glBegin(GL_QUADS);													// Draw Our Quad
						glTexCoord2f(0.0f, 1.0f); glVertex3f(-1.0f,  1.0f, 0.0f);
						glTexCoord2f(1.0f, 1.0f); glVertex3f( 1.0f,  1.0f, 0.0f);
						glTexCoord2f(1.0f, 0.0f); glVertex3f( 1.0f, -1.0f, 0.0f);
						glTexCoord2f(0.0f, 0.0f); glVertex3f(-1.0f, -1.0f, 0.0f);
					glEnd();															// Done Drawing The Quad
				glPopMatrix();															// Pop The Matrix

				glPushMatrix();															// Push The Matrix
					glTranslatef(2.0f, 0.0f, 0.0f);										// Translate 2 Units To The Right
					glRotatef(spin + loop * 36.0f, 0.0f, 1.0f, 0.0f);					// Rotate On The Y-Axis (Left - Right)
					glTranslatef(1.0f, 0.0f, 0.0f);										// Move One Unit Right
					glBindTexture(GL_TEXTURE_2D, texture[1]);							// Select The Compressed Texture
					glBegin(GL_QUADS);													// Draw Our Quad
						glTexCoord2f(0.0f, 0.0f); glVertex3f(-1.0f,  1.0f, 0.0f);
						glTexCoord2f(1.0f, 0.0f); glVertex3f( 1.0f,  1.0f, 0.0f);
						glTexCoord2f(1.0f, 1.0f); glVertex3f( 1.0f, -1.0f, 0.0f);
						glTexCoord2f(0.0f, 1.0f); glVertex3f(-1.0f, -1.0f, 0.0f);
					glEnd();															// Done Drawing The Quad
				glPopMatrix();															// Pop The Matrix
			}
		}
		#endregion Draw()

		// --- Lesson Methods ---
		#region bool Compare(byte[] a, byte[] b)
		/// <summary>
		/// Compares two arrays for equality.
		/// </summary>
		/// <param name="a">First array.</param>
		/// <param name="b">Second array.</param>
		/// <returns>true if equal, otherwise false.</returns>
		private bool Compare(byte[] a, byte[] b) {
			if(a.GetType() != b.GetType()) {											// Test Type Equality
				return false;
			}

			if(a.Length != b.Length) {													// Test Length Equality
				return false;
			}

			for(int i = 0; i < a.Length; i++) {											// Test Value Equality
				if(a[i] != b[i]) {
					return false;
				}
			}

			return true;																// Otherwise They're Equal
		}
		#endregion bool Compare(byte[] a, byte[] b)

		#region LoadCompressedTGA(Texture tex, BinaryReader reader)
		/// <summary>
		/// Loads a compressed .TGA file.
		/// </summary>
		/// <param name="tex">The Texture to save the image into.</param>
		/// <param name="reader">The reader.</param>
		private void LoadCompressedTGA(ref Texture tex, BinaryReader reader) {
			if(tga.header == null) {													// If The header Array Isn't Initialized
				tga.header = new byte[6];												// Create It
			}

			for(int i = 0; i < tga.header.Length; i++) {								// Read The Next 6 Header Bytes
				tga.header[i] = reader.ReadByte();
			}

			tex.width = (uint) tga.header[1] * 256 + tga.header[0];						// Determine The TGA Width
			tex.height = (uint) tga.header[3] * 256 + tga.header[2];					// Determine The TGA Height
			tex.bpp = tga.header[4];													// Determine The Bits Per Pixel
			tga.width = tex.width;														// Copy width Into Local Structure
			tga.height = tex.height;													// Copy height Into Local Structure
			tga.bpp = tex.bpp;															// Copy bpp Into Local Structure

			// Make Sure All Information Is Valid
			if((tex.width <= 0) || (tex.height <= 0) || ((tex.bpp != 24) && (tex.bpp != 32))) {
				throw new Exception();
			}

			if(tex.bpp == 24) {															// If bpp Of The Image Is 24
				tex.type = GL_RGB;														// Set Image type To GL_RGB
			}
			else{																		// Otherwise It's 32
				tex.type = GL_RGBA;														// Set Image type To GL_RGBA
			}

			tga.bytesPerPixel = (tga.bpp / 8);											// Compute Bytes Per Pixel
			tga.imageSize = tga.bytesPerPixel * tga.width * tga.height;					// Compute Total Image Size
			tex.imageData = new byte[tga.imageSize];									// Allocate That Much Memory
				
				
			uint pixelcount = tga.height * tga.width;									// Nuber Of Pixels In The Image
			uint currentpixel = 0;														// Current Pixel Being Read
			uint currentbyte = 0;														// Current Byte
			byte[] colorbuffer = new byte[tga.bytesPerPixel];							// Storage For 1 Pixel

			do {
				byte chunkheader = 0;													// Storage For "Chunk" Header
				chunkheader = reader.ReadByte();										// Read In The 1 Byte Header

				// If The Header Is < 128, It Means That It Is The Number Of RAW Color Packets Minus 1 That Follow The Header
				if(chunkheader < 128) {
					chunkheader++;														// Add 1 To Get The Number Of Following Color Values

					for(short counter = 0; counter < chunkheader; counter++) {			// Read RAW Color Values
						for(int i = 0; i < tga.bytesPerPixel && reader.PeekChar() != -1; i++) {
							colorbuffer[i] = reader.ReadByte();
						}

						tex.imageData[currentbyte] = colorbuffer[2];					// Flip R And B Color Values While We're At It
						tex.imageData[currentbyte + 1] = colorbuffer[1];
						tex.imageData[currentbyte + 2] = colorbuffer[0];

						if(tga.bytesPerPixel == 4) {									// If It's A 32bpp Image
							tex.imageData[currentbyte + 3] = colorbuffer[3];			// Copy The 4th Byte
						}

						currentbyte += tga.bytesPerPixel;								// Increase The Current Byte
						currentpixel++;													// Increase Current Pixel

						if(currentpixel > pixelcount) {									// Make Sure We Haven't Read Too Many Pixels
							throw new Exception();
						}
					}
				}
				// chunkheader > 128 Means RLE Data, Next Color Is Repeated chunkheader - 127 Times
				else {
					chunkheader -= 127;													// Subtact 127 To Get Rid Of The RLE ID Bit

					// Read RAW Color Values
					for(int i = 0; i < tga.bytesPerPixel && reader.PeekChar() != -1; i++) {
						colorbuffer[i] = reader.ReadByte();
					}

					for(short counter = 0; counter < chunkheader; counter++){
						tex.imageData[currentbyte] = colorbuffer[2];					// Flip R and B Color Values While We're At It
						tex.imageData[currentbyte + 1] = colorbuffer[1];
						tex.imageData[currentbyte + 2] = colorbuffer[0];

						if(tga.bytesPerPixel == 4) {									// If It's A 32bpp Image
							tex.imageData[currentbyte + 3] = colorbuffer[3];			// Copy The 4th Byte
						}

						currentbyte += tga.bytesPerPixel;								// Increase Current Byte
						currentpixel++;													// Increase Pixel Count

						if(currentpixel > pixelcount) {									// Make Sure We Haven't Read Too Many Pixels
							throw new Exception();
						}
					}
				}
			} while(currentpixel < pixelcount && reader.PeekChar() != -1);
		}
		#endregion LoadCompressedTGA(ref Texture tex, BinaryReader reader)

		#region LoadTGA(ref Texture tex, string filename)
		/// <summary>
		/// Attempts to load a .TGA image.
		/// </summary>
		/// <param name="tex">The Texture to save the image into.</param>
		/// <param name="filename">The file to open.</param>
		private void LoadTGA(ref Texture tex, string filename) {
			byte[] TGAcompare = new byte[12];											// Used To Compare TGA Header
			FileStream stream = null;													// Our Stream
			ASCIIEncoding encoding = new ASCIIEncoding();								// Our Encoding
			BinaryReader reader = null;													// Our Reader

			try {
				// Open The File
				stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
				reader = new BinaryReader(stream, encoding);

				for(int i = 0; i < TGAcompare.Length; i++) {							// Read The TGA Header
					TGAcompare[i] = reader.ReadByte();
				}

				if(Compare(TGAcompare, uTGAcompare)) {									// If The File Header Matches The Uncompressed Header
					LoadUncompressedTGA(ref tex, reader);								// Jump To Uncompressed TGA Loading Routine
				}
				else if(Compare(TGAcompare, cTGAcompare)) {								// If The File Header Matches The Compressed Header
					LoadCompressedTGA(ref tex, reader);									// Jump To Compressed TGA Loading Routine
				}
				else {																	// Otherwise We Can't Read It
					throw new Exception();												// Poop Out
				}
			}
			catch(Exception e) {
				// Handle Any Exceptions While Loading Textures, Exit App
				string errorMsg = "An Error Occurred While Loading TGA Texture:\n\t" + filename + "\n" + "\n\nStack Trace:\n\t" + e.StackTrace + "\n";
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
		#endregion LoadTGA(ref Texture tex, string filename)

		#region LoadTextures()
		/// <summary>
		/// Loads and creates the textures.
		/// </summary>
		private void LoadTextures() {													// Load TGAs And Convert To Textures
			string filename = null;														// The File To Load
			try {
				// Load The Bitmaps
				filename = @"..\..\data\NeHeLesson33\Uncompressed.tga";
				LoadTGA(ref textureImage[0], filename);
				filename = @"..\..\data\NeHeLesson33\Compressed.tga";
				LoadTGA(ref textureImage[1], filename);
			}
			catch(Exception e) {
				// Handle Any Exceptions While Loading .TGA's, Exit App
				string errorMsg = "An Error Occurred While Loading TGA Texture:\n\t" + filename + "\n" + "\n\nStack Trace:\n\t" + e.StackTrace + "\n";
				MessageBox.Show(errorMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				App.Terminate();
			}

			glGenTextures(2, texture);													// Create The Textures

			for(int loop = 0; loop < 2; loop++) {										// Loop Through Both Textures
				// Typical Texture Generation Using Data From The TGA
				glBindTexture(GL_TEXTURE_2D, texture[loop]);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
				glTexImage2D(GL_TEXTURE_2D, 0, (int) GL_RGB8,(int) textureImage[loop].width, (int)textureImage[loop].height, 0, textureImage[loop].type, GL_UNSIGNED_BYTE, textureImage[loop].imageData);
			}
		}
		#endregion LoadTextures()

		#region LoadUncompressedTGA(Texture tex, BinaryReader reader)
		/// <summary>
		/// Loads an uncompressed .TGA file.
		/// </summary>
		/// <param name="tex">The Texture to save the image into.</param>
		/// <param name="reader">The reader.</param>
		private void LoadUncompressedTGA(ref Texture tex, BinaryReader reader) {
			if(tga.header == null) {													// If The Header Array Isn't Initialized
				tga.header = new byte[6];												// Create It
			}

			for(int i = 0; i < tga.header.Length; i++) {								// Read The Next 6 Header Bytes
				tga.header[i] = reader.ReadByte();
			}

			tex.width = (uint) tga.header[1] * 256 + tga.header[0];						// Determine The TGA Width
			tex.height = (uint) tga.header[3] * 256 + tga.header[2];					// Determine The TGA Height
			tex.bpp = tga.header[4];													// Determine The Bits Per Pixel
			tga.width = tex.width;														// Copy width Into Local Structure
			tga.height = tex.height;													// Copy height Into Local Structure
			tga.bpp = tex.bpp;															// Copy bpp Into Local Structure

			// Make Sure All Information Is Valid
			if((tex.width <= 0) || (tex.height <= 0) || ((tex.bpp != 24) && (tex.bpp !=32))) {
				throw new Exception();
			}

			if(tex.bpp == 24) {															// If bpp Of The Image Is 24
				tex.type = GL_RGB;														// Set Image type To GL_RGB
			}
			else{																		// Otherwise It's 32
				tex.type = GL_RGBA;														// Set Image type To GL_RGBA
			}

			tga.bytesPerPixel = (tga.bpp / 8);											// Compute The Number Of Bytes Per Pixel
			tga.imageSize = (tga.bytesPerPixel * tga.width * tga.height);				// Compute The Total Size Of The Image
			tex.imageData = new byte[tga.imageSize];									// Allocate That Much Memory

			for(int i = 0; i < tga.imageSize && reader.PeekChar()!= -1; i++) {			// While We Haven't Read All The Data
				tex.imageData[i] = reader.ReadByte();									// Attempt To Read More imageGata
			}

			// Swap The 1st And 3rd Bytes (Red and Blue)
			uint temp;
			for(uint i = 0; i < tga.imageSize; i += tga.bytesPerPixel) {				// Loop Through The Image Data
				temp = tex.imageData[i];												// Temporarily Store The Value At imageData[i]
				tex.imageData[i] = tex.imageData[i + 2];								// Set The 1st Byte To The Value Of The 3rd Byte
				tex.imageData[i + 2] = (byte) temp;										// Set The 3rd Byte To The Value In temp (1st Byte Value)
			}
		}
		#endregion LoadUncompressedTGA(ref Texture tex, BinaryReader reader)
	}
}