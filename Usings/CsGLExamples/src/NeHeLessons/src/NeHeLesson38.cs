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
/*************************************
*                                    *
*      Jeff Molofee's Lesson 38      *
*          nehe.gamedev.net          *
*                2002                *
*                                    *
*************************************/
#endregion Original Credits / License

using CsGL.Basecode;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Windows.Forms;

#region AssemblyInfo
[assembly: AssemblyCompany("The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyCopyright("2002 The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyDescription("NeHe Lesson 38")]
[assembly: AssemblyProduct("NeHe Lesson 38")]
[assembly: AssemblyTitle("NeHe Lesson 38")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace NeHeLessons {
	/// <summary>
	/// NeHe Lesson 38 -- Loading Textures From A Resource File & Texturing Triangles (http://nehe.gamedev.net)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class NeHeLesson38 : Model {
		// --- Fields ---
		#region Private Fields
		private static uint[] texture = new uint[3];									// Storage For 3 Textures

		private struct Butterfly {														// Create A Structure Called Butterfly
			public int tex;																// Integer Used To Select Our Texture
			public float x;																// X Position
			public float y;																// Y Position
			public float z;																// Z Position
			public float yi;															// Y Increase Speed (Fall Speed)
			public float spinz;															// Z Axis Spin
			public float spinzi;														// Z Axis Spin Speed
			public float flap;															// Flapping Triangles :)
			public float fi;															// Flap Direction (Increase Value)
		};

		private static Butterfly[] flies = new Butterfly[50];							// Create 50 Butterflies Using The Butterfly Structure

		private static bool sleep = true;												// Toggles Thread.Sleep
		private static Random rand = new Random();										// Random Number Generator
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Lesson title.
		/// </summary>
		public override string Title {
			get {
				return "NeHe Lesson 38 -- Loading Textures From A Resource File & Texturing Triangles";
			}
		}

		/// <summary>
		/// Lesson description.
		/// </summary>
		public override string Description {
			get {
				return "In this lesson you will learn how to load textures from an embedded resource file and how to texture map triangles.";
			}
		}

		/// <summary>
		/// Lesson URL.
		/// </summary>
		public override string Url {
			get {
				return "http://nehe.gamedev.net/tutorials/lesson.asp?l=38";
			}
		}
		#endregion Public Properties

		// --- Entry Point ---
		#region Main()
		/// <summary>
		/// Application's entry point, runs this NeHe lesson.
		/// </summary>
		public static void Main() {														// Entry Point
			App.Run(new NeHeLesson38());												// Run Our NeHe Lesson As A Windows Forms Application
		}
		#endregion Main()

		// --- Basecode Methods ---
		#region Initialize()
		/// <summary>
		/// Overrides OpenGL's initialization.
		/// </summary>
		public override void Initialize() {
			base.Initialize();															// Run The Base Initialization

			//LoadTextures();																// Load The Textures From Our Resource File

			glDisable(GL_DEPTH_TEST);													// Disable Depth Testing
			glEnable(GL_TEXTURE_2D);													// Enable Texture Mapping
			glBlendFunc(GL_ONE, GL_SRC_ALPHA);											// Set Blending Mode (Cheap / Quick)
			glEnable(GL_BLEND);															// Enable Blending

			for(int loop = 0; loop < 50; loop++) {										// Loop To Initialize 50 Objects
				SetObject(loop);														// Call SetObject To Assign New Random Values
			}
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws NeHe Lesson 38 scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);							// Clear Screen And Depth Buffer

			for(int loop = 0; loop < 50; loop++) {										// Loop Of 50 (Draw 50 Objects)
				glLoadIdentity();														// Reset The Modelview Matrix
				glBindTexture(GL_TEXTURE_2D, texture[flies[loop].tex]);					// Bind Our Texture
				glTranslatef(flies[loop].x, flies[loop].y, flies[loop].z);				// Position The Object
				glRotatef(45.0f, 1.0f, 0.0f, 0.0f);										// Rotate On The X-Axis
				glRotatef((flies[loop].spinz), 0.0f, 0.0f, 1.0f);						// Spin On The Z-Axis

				glBegin(GL_TRIANGLES);													// Begin Drawing Triangles
					// First Triangle
					glTexCoord2f(1.0f, 1.0f); glVertex3f( 1.0f, 1.0f, 0.0f);			// Point 1 (Top Right)
					glTexCoord2f(0.0f, 1.0f); glVertex3f(-1.0f, 1.0f, flies[loop].flap);// Point 2 (Top Left)
					glTexCoord2f(0.0f, 0.0f); glVertex3f(-1.0f, -1.0f, 0.0f);			// Point 3 (Bottom Left)
					// Second Triangle
					glTexCoord2f(1.0f, 1.0f); glVertex3f( 1.0f, 1.0f, 0.0f);			// Point 1 (Top Right)
					glTexCoord2f(0.0f, 0.0f); glVertex3f(-1.0f, -1.0f, 0.0f);			// Point 2 (Bottom Left)
					glTexCoord2f(1.0f, 0.0f); glVertex3f( 1.0f, -1.0f, flies[loop].flap);// Point 3 (Bottom Right)
				glEnd();																// Done Drawing Triangles

				flies[loop].y -= flies[loop].yi;										// Move Object Down The Screen
				flies[loop].spinz += flies[loop].spinzi;								// Increase Z Rotation By spinzi
				flies[loop].flap += flies[loop].fi;										// Increase flap Value By fi

				if(flies[loop].y < -18.0f) {											// Is Object Off The Screen?
					SetObject(loop);													// If So, Reassign New Values
				}

				if((flies[loop].flap > 1.0f) || (flies[loop].flap < -1.0f)) {			// Time To Change Flap Direction?
					flies[loop].fi = -flies[loop].fi;									// Change Direction By Making fi = -fi
				}
			}

			if(sleep) {
				System.Threading.Thread.Sleep(15);										// Create A Short Delay (15 Milliseconds)
			}
			glFlush();																	// Flush The GL Rendering Pipeline
		}
		#endregion Draw()

		#region InputHelp()
		/// <summary>
		/// Overrides default input help, supplying lesson-specific help information.
		/// </summary>
		public override void InputHelp() {
			base.InputHelp();															// Set Up The Default Input Help

			DataRow dataRow;															// Row To Add

			dataRow = InputHelpDataTable.NewRow();										// Space Bar - Toggle Thread.Sleep
			dataRow["Input"] = "Space Bar";
			dataRow["Effect"] = "Toggle Thread.Sleep On / Off";
			if(sleep) {
				dataRow["Current State"] = "On";
			}
			else {
				dataRow["Current State"] = "Off";
			}
			InputHelpDataTable.Rows.Add(dataRow);
		}
		#endregion InputHelp()

		#region ProcessInput()
		/// <summary>
		/// Overrides default input handling, adding lesson-specific input handling.
		/// </summary>
		public override void ProcessInput() {
			base.ProcessInput();														// Handle The Default Basecode Keys

			if(KeyState[(int) Keys.Space]) {											// If Space Bar Is Pressed
				sleep = !sleep;															// Toggle sleep
				KeyState[(int) Keys.Space] = false;										// Mark As Handled
				UpdateInputHelp();														// Update The Help Screen
			}
		}
		#endregion ProcessInput()

		// --- Lesson Methods ---
/*
		#region LoadTextures()
		private void LoadTextures()	{								// Creates Textures From Bitmaps In The Resource File
			// Ignoring Loading From Resources For Now, If You Like, Just Embed The Images
			glGenTextures(3, texture);

			
			Bitmap img = (Bitmap) LoadImage(@"..\..\data\Butterfly1.bmp").Clone();
			img.RotateFlip(RotateFlipType.RotateNoneFlipY);
			Rectangle rect = new Rectangle(0, 0, img.Width, img.Height);
			BitmapData tex = img.LockBits(rect, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			glBindTexture(GL_TEXTURE_2D, texture[0]);
			glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR); 
			glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
			gluBuild2DMipmaps(GL_TEXTURE_2D, (int) GL_RGB8, img.Width, img.Height, GL_BGRA_EXT, GL_UNSIGNED_BYTE, tex.Scan0);
			//glTexImage2D(GL_TEXTURE_2D, 0, (int)GL_RGB8, img.Width, img.Height, 0, GL_BGRA_EXT, GL_UNSIGNED_BYTE, tex.Scan0);

			img = (Bitmap) LoadImage(@"..\..\data\Butterfly2.bmp").Clone();
			img.RotateFlip(RotateFlipType.RotateNoneFlipY);
			rect = new Rectangle(0, 0, img.Width, img.Height);
			tex = img.LockBits(rect, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			glBindTexture(GL_TEXTURE_2D, texture[1]);
			glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR); 
			glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
			gluBuild2DMipmaps(GL_TEXTURE_2D, (int) GL_RGB8, img.Width, img.Height, GL_BGRA_EXT, GL_UNSIGNED_BYTE, tex.Scan0);
			//glTexImage2D(GL_TEXTURE_2D, 0, (int)GL_RGB8, img.Width, img.Height, 0, GL_BGRA_EXT, GL_UNSIGNED_BYTE, tex.Scan0);

			img = (Bitmap) LoadImage(@"..\..\data\Butterfly3.bmp").Clone();
			img.RotateFlip(RotateFlipType.RotateNoneFlipY);
			rect = new Rectangle(0, 0, img.Width, img.Height);
			tex = img.LockBits(rect, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			glBindTexture(GL_TEXTURE_2D, texture[2]);
			glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR); 
			glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
			gluBuild2DMipmaps(GL_TEXTURE_2D, (int) GL_RGB8, img.Width, img.Height, GL_BGRA_EXT, GL_UNSIGNED_BYTE, tex.Scan0);
			//glTexImage2D(GL_TEXTURE_2D, 0, (int)GL_RGB8, img.Width, img.Height, 0, GL_BGRA_EXT, GL_UNSIGNED_BYTE, tex.Scan0);
		}
		#endregion LoadTextures()
*/
		#region SetObject(int loop)
		/// <summary>
		/// Sets initial random values for an object.
		/// </summary>
		/// <param name="loop">The object to set.</param>
		private void SetObject(int loop) {												// Sets The Initial Value Of Each Object (Random)
			flies[loop].tex = rand.Next() % 3;											// Texture Can Be One Of 3 Textures
			flies[loop].x = rand.Next() % 34 - 17.0f;									// Random x Value From -17.0f To 17.0f
			flies[loop].y = 18.0f;														// Set y Position To 18 (Off Top Of Screen)
			flies[loop].z = -((rand.Next() % 30000 / 1000.0f) + 10.0f);					// z Is A Random Value From -10.0f To -40.0f
			flies[loop].spinzi = (rand.Next() % 10000) / 5000.0f -1.0f;					// spinzi Is A Random Value From -1.0f To 1.0f
			flies[loop].flap = 0.0f;													// flap Starts Off At 0.0f;
			flies[loop].fi = 0.05f + (rand.Next() % 100) / 1000.0f;						// fi Is A Random Value From 0.05f To 0.15f
			flies[loop].yi = 0.001f + (rand.Next() % 1000) / 10000.0f;					// yi Is A Random Value From 0.001f To 0.101f
		}
		#endregion SetObject(int loop)
	}
}