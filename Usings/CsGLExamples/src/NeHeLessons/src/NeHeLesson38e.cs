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
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;

#region AssemblyInfo
[assembly: AssemblyCompany("The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyCopyright("2002 The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyDescription("NeHe Lesson 38e")]
[assembly: AssemblyProduct("NeHe Lesson 38e")]
[assembly: AssemblyTitle("NeHe Lesson 38e")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace NeHeLessons 
{
	/// <summary>
	/// NeHe Lesson 38e -- Loading Textures From A Resource File & Texturing Triangles (http://nehe.gamedev.net)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class Lesson38e : Model 
	{
		// --- Fields ---
		#region Private Fields
		private uint[] texture = new uint[4];						// Storage For 4 Textures
		private float tilt = 45.0f;									// Butterfly Fall Angle(NEW)
		private int loop;
		private string[] pics = new string[]{@"Data\Butterfly1.bmp",  //path way to our pics(NEW)
												@"Data\Butterfly2.bmp",
												@"Data\Butterfly3.bmp",
												@"Data\ButterflyMask.bmp"};
											 
		Butterfly[] flies = new Butterfly[50];						// Create 50 Butterflies Using The Butterfly Structure

		private bool sleep = true;									// Toggle Thread.Sleep
		private Random rand = new Random();							// Random Number Generator
		private StringBuilder stringBuilder = new StringBuilder();	// Common StringBuilder
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Lesson Title
		/// </summary>
		public override string Title 
		{
			get 
			{
				return "NeHe Lesson 38 -- Loading Textures From A Resource File & Texturing Triangles(E) ";
			}
		}

		/// <summary>
		/// Lesson Help
		/// </summary>
		public override string Help 
		{
			get 
			{
				stringBuilder.Remove(0, stringBuilder.Length);

				stringBuilder.Append("Up\t->\tTilt ButterFlies(+)\n");
				stringBuilder.Append("Down\t->\tTilt ButterFlies(-)\n");
				stringBuilder.Append("Space\t->\tToggle Thread.Sleep");

				if(sleep) 
				{
					stringBuilder.Append("(On)\n");
				}
				else 
				{
					stringBuilder.Append("(Off)\n");
				}

				return stringBuilder.ToString();
			}
		}
		#endregion Public Properties

		// --- Methods ---
		#region Main()
		/// <summary>
		/// Application Entry Point, Runs This NeHe Lesson
		/// </summary>
		public static void Main() 
		{									// Entry Point
			App.Run(new Lesson38e());								// Run Our NeHe Lesson As A Windows Forms Application
		}
		#endregion Main()

		#region Init()
		/// <summary>
		/// Overrides OpenGL Initialization
		/// </summary>
		public override void Init() 
		{
			base.Init();

			LoadTextures();											// Load The Textures From Our Resource File

			glDisable(GL_DEPTH_TEST);								// Disable Depth Testing
			glShadeModel (GL_SMOOTH);								// Select Smooth Shading
			glEnable(GL_TEXTURE_2D);								// Enable Texture Mapping
			glBlendFunc(GL_ONE, GL_SRC_ALPHA);						// Set Blending Mode (Cheap / Quick)
			glEnable(GL_BLEND);										// Enable Blending}

			for(int loop = 0; loop < 50; loop++)					// Loop To Initialize 50 Objects
			{									
				SetObject(loop);									// Call SetObject To Assign New Random Values
			}
		}
		#endregion Init()

		#region LoadTextures()
		private void LoadTextures()			// Creates Textures From Bitmaps In The Resource File
		{								
			Bitmap img;						// Bitmap 
			glGenTextures(4, texture);		//get ready to load up 4 Textures
			for(loop =0;loop<4;loop++)
			{
			
				// Ignoring Loading From Resources For Now, If You Like, Just Embed The Images
				
				img = (Bitmap) LoadImage(pics[loop]).Clone();		//load up image use pics(Array)to get the path (NEW)

						
				img.RotateFlip(RotateFlipType.RotateNoneFlipY);
				Rectangle rect = new Rectangle(0, 0, img.Width, img.Height);
				BitmapData tex = img.LockBits(rect, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

				glPixelStorei(GL_UNPACK_ALIGNMENT,4);
				glBindTexture(GL_TEXTURE_2D, texture[loop]);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR); 
				glTexParameteri(GL_TEXTURE_2D,GL_TEXTURE_MIN_FILTER,GL_LINEAR_MIPMAP_LINEAR);
				
				gluBuild2DMipmaps(GL_TEXTURE_2D, (int) GL_RGB8, img.Width, img.Height, GL_BGRA_EXT, GL_UNSIGNED_BYTE, tex.Scan0);
				img.Dispose();
			}
			
		}
		#endregion LoadTextures()

		#region Draw()
		/// <summary>
		/// Draw Lesson 38e Scene
		/// </summary>
		public override void Draw()									// Here's Where We Do All The Drawing
		{								
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);		// Clear Screen And Depth Buffer

			for(int loop = 0; loop < 50; loop++)					// Loop Of 50 (Draw 50 Objects)
			{					
				glLoadIdentity();									// Reset The Modelview Matrix
				glTranslatef(flies[loop].x, flies[loop].y, flies[loop].z);	// Position The Object
				glRotatef(tilt, 1.0f, 0.0f, 0.0f);					// Rotate On The X-Axis(NEW)
				glRotatef((flies[loop].spinz), 0.0f, 0.0f, 1.0f);	// Spin On The Z-Axis

				glBlendFunc(GL_DST_COLOR,GL_ZERO);						// Set Blending Mask Cancels Screen Objects(NEW)
				glBindTexture(GL_TEXTURE_2D, texture[3]);				// Bind Our Texture (Mask Texture)(NEW)

				glBegin(GL_TRIANGLES);								// Begin Drawing Triangles(NEW)
				// First Triangle
				glTexCoord2f(1.0f, 1.0f); glVertex3f( 1.0f, 1.0f, 0.0f);	// Point 1 (Top Right)(NEW)
				glTexCoord2f(0.0f, 1.0f); glVertex3f(-1.0f, 1.0f, flies[loop].flap);	// Point 2 (Top Left)(NEW)
				glTexCoord2f(0.0f, 0.0f); glVertex3f(-1.0f, -1.0f, 0.0f);	// Point 3 (Bottom Left)(NEW)
				// Second Triangle
				glTexCoord2f(1.0f, 1.0f); glVertex3f( 1.0f, 1.0f, 0.0f);	// Point 1 (Top Right)(NEW)
				glTexCoord2f(0.0f, 0.0f); glVertex3f(-1.0f, -1.0f, 0.0f);	// Point 2 (Bottom Left)(NEW)
				glTexCoord2f(1.0f, 0.0f); glVertex3f( 1.0f, -1.0f, flies[loop].flap);	// Point 3 (Bottom Right)(NEW)
				glEnd();											// Done Drawing Triangles(NEW)
				
				glBlendFunc(GL_ONE, GL_ONE);							// Object Only Maps To Mask(NEW)
				glBindTexture(GL_TEXTURE_2D, texture[flies[loop].tex]);	// Bind Our Texture (Random Butterfly)(NEW)
				
				glBegin(GL_TRIANGLES);									// Begin Drawing Triangles
				// First Triangle														    _____
				glTexCoord2f(1.0f,1.0f); glVertex3f( 1.0f, 1.0f, 0.0f);				//	(2)|    / (1)
				glTexCoord2f(0.0f,1.0f); glVertex3f(-1.0f, 1.0f, flies[loop].flap);	//	   |  /
				glTexCoord2f(0.0f,0.0f); glVertex3f(-1.0f,-1.0f, 0.0f);				//	(3)|/

				// Second Triangle
				glTexCoord2f(1.0f,1.0f); glVertex3f( 1.0f, 1.0f, 0.0f);				//	       /|(1)
				glTexCoord2f(0.0f,0.0f); glVertex3f(-1.0f,-1.0f, 0.0f);				//	     /  |
				glTexCoord2f(1.0f,0.0f); glVertex3f( 1.0f,-1.0f, flies[loop].flap);	//	(2)/____|(3)

				glEnd();												// Done Drawing Triangles

				flies[loop].y -= flies[loop].yi;					// Move Object Down The Screen
				flies[loop].spinz += flies[loop].spinzi;			// Increase Z Rotation By spinzi
				flies[loop].flap += flies[loop].fi;					// Increase flap Value By fi

				if(flies[loop].y < -18.0f)							// Is Object Off The Screen?
				{						
					SetObject(loop);								// If So, Reassign New Values
				}

				if((flies[loop].flap > 1.0f) || (flies[loop].flap < -1.0f)) // Time To Change Flap Direction?
				{	
					flies[loop].fi = -flies[loop].fi;				// Change Direction By Making fi = -fi
				}
			}
			
			if(sleep) 
			{
				System.Threading.Thread.Sleep(10);					// Create A Short Delay (10 Milliseconds)
			}
			glFlush();												// Flush The GL Rendering Pipeline
		}
		#endregion Draw()

		#region HandleKey(Keys key)
		/// <summary>
		/// Lesson Specific Key Handling
		/// </summary>
		/// <param name="key">The Key To Handle</param>
		public override void HandleKey(Keys key) 
		{
			switch(key) 
			{
				case Keys.Space:
					sleep = !sleep;									// Toggle sleep
					break;
				
				case Keys.Up:
					tilt+=1.0f;										//Tilt Butterflies
					break;
				
				case Keys.Down:
					tilt-=1.0f;										//Tilt Butterflies
					break;
			}
		}
		#endregion HandleKey(Keys key)

		#region SetObject(int loop)
		private void SetObject(int loop) 
		{															// Sets The Initial Value Of Each Object (Random)
			flies[loop].tex = rand.Next() % 3;						// Texture Can Be One Of 3 Textures
			flies[loop].x = rand.Next() % 34 - 17.0f;				// Random x Value From -17.0f To 17.0f
			flies[loop].y = 18.0f;									// Set y Position To 18 (Off Top Of Screen)
			flies[loop].z = -((rand.Next() % 30000 / 1000.0f) + 10.0f);	// z Is A Random Value From -10.0f To -40.0f
			flies[loop].spinzi = (rand.Next() % 10000) / 5000.0f -1.0f;	// spinzi Is A Random Value From -1.0f To 1.0f
			flies[loop].flap = 0.0f;								// flap Starts Off At 0.0f;
			flies[loop].fi = 0.05f + (rand.Next() % 100) / 1000.0f;	// fi Is A Random Value From 0.05f To 0.15f
			flies[loop].yi = 0.001f + (rand.Next() % 1000) / 10000.0f;	// yi Is A Random Value From 0.001f To 0.101f
			Array.Sort(flies, new ButterflyComparer());				// Perform Sort (50 Objects)
		}
		#endregion SetObject(int loop)
	}
	
	public struct Butterfly { 										// Create A Structure Called Butterfly
		public int tex;												// Integer Used To Select Our Texture
		public float x;												// X Position
		public float y;												// Y Position
		public float z;												// Z Position
		public float yi;											// Y Increase Speed (Fall Speed)
		public float spinz;											// Z Axis Spin
		public float spinzi;										// Z Axis Spin Speed
		public float flap;											// Flapping Triangles :)
		public float fi;											// Flap Direction (Increase Value)
	};

	public class ButterflyComparer : IComparer {
		public int Compare(object elem1, object elem2) {			// Compare Function
			if(!(elem1 is Butterfly || elem2 is Butterfly)) {		// Make Sure We Have Butterfly Objects
				throw new ArgumentException("The objects to compare must be of type 'Butterfly'");
			}
			if(((Butterfly) elem1).z < ((Butterfly) elem2).z) {		// If First Structure distance Is Less Than The Second
				return -1;											// Return -1
			}
			else if(((Butterfly) elem1).z > ((Butterfly) elem2).z) {// If First Structure distance Is Greater Than The Second
				return 1;											// Return 1
			}
			else {													// Otherwise (If The distance Is Equal)
				return 0;											// Return 0
			}
		}
	}
}