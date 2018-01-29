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
 *		This Code Was Created By Jeff Molofee 2000
 *		If You've Found This Code Useful, Please Let Me Know.
 *		Visit My Site At nehe.gamedev.net
 */
#endregion Original Credits / License

using CsGL.Basecode;
using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

#region AssemblyInfo
[assembly: AssemblyCompany("The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyCopyright("2002 The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyDescription("NeHe Lesson 24")]
[assembly: AssemblyProduct("NeHe Lesson 24")]
[assembly: AssemblyTitle("NeHe Lesson 24")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace NeheLessons {
	/// <summary>
	/// NeHe Lesson 24 -- Tokens, Extensions, Scissor Testing And TGA Loading (http://nehe.gamedev.net)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class NeHeLesson24 : Model {
		// --- Fields ---
		#region Private Fields
		private int scroll;																// Used For Scrolling The Screen
		private int maxtokens;															// Keeps Track Of The Number Of Extensions Supported
		private int swidth;																// Scissor Width
		private int sheight;															// Scissor Height

		private uint dbase;																// Base Display List For The Font

		private struct TextureImage {													// Create A Structure For The Texture
			public byte[] imageData;													// Image Data
			public uint bpp;															// Image Color Depth In Bits Per Pixel
			public uint width;															// Image Width
			public uint height;															// Image Height
		}

		private uint[] texture = new uint[1];											// Storage For One Texture
		private TextureImage[] tgaTexture = new TextureImage[1];						// Storage For One TGA Texture
		#endregion

		#region Public Properties
		/// <summary>
		/// Lesson Title
		/// </summary>
		public override string Title {
			get {
				return "NeHe Lesson 24 -- Tokens, Extensions, Scissor Testing And TGA Loading";
			}
		}

		/// <summary>
		/// Lesson Description
		/// </summary>
		public override string Description {
			get {
				return "In this lesson you will learn how to read and parse supported OpenGL extensions, use scissor testing to perform scrolling, and how to parse and load a TGA image for texturing.";
			}
		}

		/// <summary>
		/// Lesson URL
		/// </summary>
		public override string Url {
			get {
				return "http://nehe.gamedev.net/tutorials/lesson.asp?l=24";
			}
		}
		#endregion Public Properties

		// --- Entry Point ---
		#region Main()
		/// <summary>
		/// Application Entry Point, Runs This NeHe Lesson
		/// </summary>
		public static void Main() {														// Entry Point
			App.Run(new NeHeLesson24());												// Run Our NeHe Lesson As A Windows Forms Application
		}
		#endregion Main()

		// --- Basecode Methods ---
		#region Initialize()
		public override void Initialize() {
			base.Initialize();															// Run The Base Initialization
			LoadTGA(tgaTexture[0], @"..\..\data\NeHeLesson24\Font.tga");				// Load The Font Texture
			BuildFont();																// Build The Font
			glBindTexture(GL_TEXTURE_2D, texture[0]);									// Select Our Font Texture
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draw NeHe Lesson 24 Scene
		/// </summary>
		public override void Draw(){
			string[] tokens;															// Storage For Our Tokens

			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);							// Clear Screen And Depth Buffer

			glColor3f(1.0f, 0.5f, 0.5f);												// Set Color To Bright Red
			GlPrint(50, 16, 1, "Renderer");												// Display Renderer
			GlPrint(80, 48, 1, "Vendor");												// Display Vendor Name
			GlPrint(66, 80, 1, "Version");												// Display Version

			glColor3f(1.0f, 0.7f, 0.4f);												// Set Color To Orange
			GlPrint(200, 16, 1, glGetString(GL_RENDERER));								// Display Renderer
			GlPrint(200, 48, 1, glGetString(GL_VENDOR));								// Display Vendor Name
			GlPrint(200, 80, 1, glGetString(GL_VERSION));								// Display Version

			glColor3f(0.5f, 0.5f, 1.0f);												// Set Color To Bright Blue
			GlPrint(192, 432, 1, "NeHe Productions");									// Write NeHe Productions At The Bottom Of The Screen

			glLoadIdentity();															// Reset The ModelView Matrix
			glColor3f(1.0f, 1.0f, 1.0f);												// Set The Color To White
			glBegin(GL_LINE_STRIP);														// Start Drawing Line Strips (Something New)
			glVertex2d(639, 417);														// Top Right Of Bottom Box
			glVertex2d(  0, 417);														// Top Left Of Bottom Box
			glVertex2d(  0, 480);														// Lower Left Of Bottom Box
			glVertex2d(639, 480);														// Lower Right Of Bottom Box
			glVertex2d(639, 128);														// Up To Bottom Right Of Top Box
			glEnd();																	// Done First Line Strip
			glBegin(GL_LINE_STRIP);														// Start Drawing Another Line Strip
			glVertex2d(  0, 128);														// Bottom Left Of Top Box
			glVertex2d(639, 128);														// Bottom Right Of Top Box
			glVertex2d(639,   1);														// Top Right Of Top Box
			glVertex2d(  0,   1);														// Top Left Of Top Box
			glVertex2d(  0, 417);														// Down To Top Left Of Bottom Box
			glEnd();																	// Done Second Line Strip

			// Define Scissor Region
			glScissor(1, (int) (0.135416f * sheight), (swidth - 2),(int) (0.597916f * sheight));
			glEnable(GL_SCISSOR_TEST);													// Enable Scissor Testing

			string text = glGetString(GL_EXTENSIONS);									// Grab The Extension List, Store In Text

			tokens = text.Split();														// Parse text For Words Seperated By A Space
			maxtokens = tokens.Length;													// Set maxtokens To The Number Of Found tokens

			for(int i = 0; i < maxtokens - 1; i++) {
				if(tokens[i] != null || tokens[i] != "") {
					glColor3f(0.5f, 1.0f, 0.5f);										// Set Color To Bright Green
					GlPrint(0, 96 + ((i + 1) * 32) - scroll, 0, (i + 1).ToString());	// Print Current Extension Number
					glColor3f(1.0f, 1.0f, 0.5f);										// Set Color To Yellow
					GlPrint(50, 96 + ((i + 1) * 32) - scroll, 0, tokens[i]);			// Print The Current Token (Parsed Extension Name)
				}
			}

			glDisable(GL_SCISSOR_TEST);													// Disable Scissor Testing

			glFlush();																	// Flush The Rendering Pipeline
		}
		#endregion

		#region InputHelp()
		/// <summary>
		/// Overrides The Default Input Help, Supplying Model-Specific Input Help.
		/// </summary>
		public override void InputHelp() {
			base.InputHelp();															// Set Up The Default Input Help

			DataRow dataRow;															// Row To Add

			dataRow = InputHelpDataTable.NewRow();										// Up Arrow - Scroll Up
			dataRow["Input"] = "Up Arrow";
			dataRow["Effect"] = "Scroll Up";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Down Arrow - Scroll Down
			dataRow["Input"] = "Down Arrow";
			dataRow["Effect"] = "Scroll Down";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);
		}
		#endregion InputHelp()

		#region ProcessInput()
		/// <summary>
		/// Lesson-Specific Input Handling
		/// </summary>
		public override void ProcessInput() {
			base.ProcessInput();														// Handle The Default Basecode Keys

			if(KeyState[(int) Keys.Up] && (scroll > 0)) {								// Is Up Arrow Being Pressed?
				scroll -= 2;															// If So, Scroll Up
			}

			if(KeyState[(int) Keys.Down] && (scroll < 32 * (maxtokens - 9))) {			// Is Down Arrow Being Pressed?
				scroll += 2;															// If So, Scroll Down
			}
		}
		#endregion ProcessInput()

		#region Reshape(int width, int height)
		/// <summary>
		/// Overrides OpenGL Reshape.
		/// </summary>
		/// <param name="width">New width.</param>
		/// <param name="height">New height.</param>
		public override void Reshape(int width, int height) {							// Resize And Initialize The GL Window
			if(height == 0) {															// If Height Is 0
				height = 1;																// Set To 1 To Prevent A Divide By Zero
			}

			swidth = width;																// Set The Scissor Width
			sheight = height;															// Set The Scissor Height

			glViewport(0, 0, width, height);											// Reset The Current Viewport

			glMatrixMode(GL_PROJECTION);												// Select The Projection Matris
			glLoadIdentity();															// Reset The Projection Matrix

			glOrtho(0.0f, 640, 480, 0.0f, -1.0f, 1.0f);									// Create Ortho View 640x480

			glMatrixMode(GL_MODELVIEW);													// Select The Modelview Matrix
			glLoadIdentity();															// Reset The Modelview Matrix
		}
		#endregion Reshape(int width, int height)

		// --- Lesson Methods ---
		#region BuildFont()
		public void BuildFont() {														// Build Our Font Display List
			dbase = glGenLists(256);													// Creating 256 Display Lists
			glBindTexture(GL_TEXTURE_2D, texture[0]);									// Select Our Font Texture

			for (int loop = 0; loop < 256; loop++) {									// Loop Through All 256 Lists
				float cx = (float)(loop % 16) / 16.0f;									// X Position Of Current Character
				float cy = (float)(loop / 16) / 16.0f;									// Y Position Of Current Character

				glNewList((uint)(dbase + loop), GL_COMPILE);							// Start Building A List
					glBegin(GL_QUADS);													// Use A Quad For Each Character
						glTexCoord2f(cx, 1.0f - cy - 0.0625f);							// Texture Coord (Bottom Left)
						glVertex2d(0, 16);												// Vertex Coord (Bottom Left)
						glTexCoord2f(cx + 0.0625f, 1.0f - cy - 0.0625f);				// Texture Coord (Bottom Right)
						glVertex2i(16, 16);												// Vertex Coord (Bottom Right)
						glTexCoord2f(cx + 0.0625f, 1.0f - cy - 0.001f);					// Texture Coord (Top Right)
						glVertex2i(16, 0);												// Vertex Coord (Top Right)
						glTexCoord2f(cx, 1.0f - cy - 0.001f);							// Texture Coord (Top Left)
						glVertex2i(0, 0);												// Vertex Coord (Top Left)
					glEnd();															// Done Building Our Quad (Character)
					glTranslated(14, 0, 0);												// Move To The Right Of The Character
				glEndList();															// Done Building The Display List
			}
		}
		#endregion BuildFont()

		#region LoadTGA(TextureImage textureImage, string filename)
		private void LoadTGA(TextureImage textureImage, string filename){
			byte[] TGAheader = {0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0};					// Uncompressed TGA Header
			byte[] TGAcompare = new byte[12];											// Used To Compare TGA Header
			byte[] header = new byte[6];												// First 6 Useful Bytes From The Header
			uint bytesPerPixel;															// Holds Number Of Bytes Per Pixel Used In The TGA File
			uint imageSize;																// Used To Store The Image Size When Setting Aside Ram
			uint temp;																	// Temporary Variable
			uint type = GL_RGBA;														// Set The Default GL Mode To RBGA (32 BPP)
			FileStream stream = null;													// Our Stream
			ASCIIEncoding encoding = new ASCIIEncoding();								// Our Encoding
			BinaryReader reader = null;													// Our Reader

			try {
				// Open The File
				stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
				reader = new BinaryReader(stream, encoding);

				for(int i = 0; i < TGAcompare.Length; i++) {							// Read First 12 Bytes From File Into TGACompare
					TGAcompare[i] = reader.ReadByte();
				}

				for(int i = 0; i < TGAcompare.Length; i++) {
					if(TGAcompare[i] != TGAheader[i]) {									// If The Header Does Not Match What We Want
						throw new Exception();
					}
				}

				for(int i = 0; i < 6; i++) {
					header[i] = reader.ReadByte();										// Read The Next 6 Header Bytes
				}

				textureImage.width = (uint) header[1] * 256 + header[0];				// Determine The TGA Width (highbyte * 256 + lowbyte)
				textureImage.height = (uint) header[3]* 256 + header[2];				// Determine The TGA Height (highbyte * 256 + lowbyte)

				if(textureImage.width <= 0 ||											// Is The Width Less Than Or Equal To Zero?
					textureImage.height <= 0 ||											// Is The Height Less Than Or Equal To Zero?
					(header[4] != 24 && header[4] != 32)) {								// Is The TGA Not 24 Or 32 Bit?
					throw new Exception();
				}

				textureImage.bpp = header[4];											// Grab The TGA's Bits Per Pixel (24 or 32)
				bytesPerPixel = textureImage.bpp / 8;									// Divide By 8 To Get The Bytes Per Pixel
				imageSize = textureImage.width * textureImage.height * bytesPerPixel;	// Calculate The Memory Required For The TGA Data

				textureImage.imageData = new byte[imageSize];							// Reserve Memory To Hold The TGA Data

				int loop = 0;
				while(reader.PeekChar() != -1 && loop < imageSize) {					// Read Until EOF Or We've Reached imageSize
					textureImage.imageData[loop] = reader.ReadByte();
					loop++;
				}

				for(uint i = 0; i < imageSize; i += bytesPerPixel) {					// Loop Through The Image Data
					// Swaps The 1st And 3rd Bytes (Red and Blue)
					temp = textureImage.imageData[i];									// Temporarily Store The Value At Image Data 'i'
					textureImage.imageData[i] = textureImage.imageData[i + 2];			// Set The 1st Byte To The Value Of The 3rd Byte
					textureImage.imageData[i + 2] = (byte) temp;						// Set The 3rd Byte To The Value In 'temp' (1st Byte Value)
				}

				// Build A Texture From The Data
				glGenTextures(1, texture);												// Generate 1 Texture

				glBindTexture(GL_TEXTURE_2D, texture[0]);								// Bind Our Texture
				glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);		// Linear Filtered
				glTexParameterf(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);		// Linear Filtered
			
				if(textureImage.bpp == 24) {											// Was The TGA 24 Bits
					type = GL_RGB;														// If So Set The 'type' To GL_RGB
				}

				glTexImage2D(GL_TEXTURE_2D, 0, (int) GL_RGB8, (int) textureImage.width, (int) textureImage.height, 0, type, GL_UNSIGNED_BYTE, textureImage.imageData);
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
		#endregion LoadTGA(TextureImage textureImage, string filename)

		#region GlPrint()
		public void GlPrint(int x, int y, int dset, string text) {						// Where The Printing Happens
			if(dset > 1) {																// Did User Choose An Invalid Character Set?
				dset = 1;																// If So, Select Set 1 (Italic)
			}

			glEnable(GL_TEXTURE_2D);													// Enable Texture Mapping
			glLoadIdentity();															// Reset The Modelview Matrix
			glTranslated(x, y, 0);														// Position The Text (0,0 - Top Left)
			glListBase((uint)(dbase - 32 + (128 * dset)));								// Choose The Font Set (0 or 1)
			glScalef(1.0f, 2.0f, 1.0f);													// Make The Text 2X Taller

			glCallLists(text.Length, GL_UNSIGNED_SHORT, text);							// Write The Text To The Screen
			glDisable(GL_TEXTURE_2D);													// Disable Texture Mapping
		}
		#endregion GlPrint()
	}
}