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
Pong Mania- A 3D OpenGL pong game.
Copyright (C) 2001  Steve Wortham

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation version 2

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.

 		Pong Mania
 		by: Steve Wortham
 		website: http://www.gldomain.com/Programs/PongMania.htm
 		email: steve@gldomain.com
 		
*/
#endregion Original Credits / License

using CsGL.Basecode;
using CsGL.OpenGL;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#region AssemblyInfo
[assembly: AssemblyCompany("The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyCopyright("2002 The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyDescription("Misc Pong Mania")]
[assembly: AssemblyProduct("Misc Pong Mania")]
[assembly: AssemblyTitle("Misc Pong Mania")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace MiscExamples {
	/// <summary>
	/// Misc Pong Mania -- 3D Pong! (http://www.gldomain.com/Programs/PongMania.htm)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class MiscPongMania : Model {
		// --- Fields ---
		#region Private Fields
		private const int SND_SYNC = 0x0000;											// Play Sound Synchronously (default)
		private const int SND_ASYNC = 0x0001;											// Play Sound Asynchronously
		private const int SND_LOOP = 0x0008;											// Loop Sound Until Next SoundPlay

		private static bool up;															// Up Arrow Being Pressed?
		private static bool dn;															// Down Arrow Being Pressed?
		private static bool rn;															// Enter/Return Being Pressed?

		private static bool MENU = true;												// Showing The Menu?
		private static int item = 1;													// Which Menu Item Is Current?
		private static bool PlayAgain = true;											// Start New Game?
		private static bool Options = false;											// Game Options?
		private static bool Exit = false;												// Exit Game?

		private static GLUquadric quadratic;											// Storage For Our Quadratic Objects

		private static uint[] texture = new uint[6];									// Storage For 6 Textures
		private static HighResolutionTimer timer = new HighResolutionTimer();			// Our Timer

		private static float x = 0, y = -1.75f, z = -15.0f;

		private static float xp = 1, zp = 1.3f;

		private static float mx = 0, my = -1.75f, mz = -6+.25f;
		private static float mxp = 0, mzp = 0;

		private static float cmx = 0, cmy = -1.75f, cmz = -16-.25f;
		private static float cmxp = 0;

		private static float r = 0, g = 0, b = 0;

		private static float xrot, zrot;
		private static float xspeed = 0, zspeed = 0;
		private static float RoomZrot = 0;

		private static float Dist, cDist, Speed;

		private static float alpha1 = 0.25f, alpha2 = 0.25f, alpha3 = 0.25f;
		private static float alpha1a = 0, alpha2a = 0, alpha3a = 0;
		private static float alpha1ap = 0, alpha2ap = 0, alpha3ap = 0;

		private static float menuZ = 0;

		private static float Time1;
		private static float Time2;
		private static float DiffTime;
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Example title.
		/// </summary>
		public override string Title {
			get {
				return "Misc Pong Mania -- 3D Pong!";
			}
		}

		/// <summary>
		/// Example description.
		/// </summary>
		public override string Description {
			get {
				return "Steve Wortham's Pong Mania!  Note, as per Steve's version, the Options menu item currently does nothing.";
			}
		}

		/// <summary>
		/// Example URL.
		/// </summary>
		public override string Url {
			get {
				return "http://www.gldomain.com/Programs/PongMania.htm";
			}
		}
		#endregion Public Properties

		// --- Entry Point ---
		#region Main()
		/// <summary>
		/// Application's entry point, runs this Misc example.
		/// </summary>
		public static void Main() {														// Entry Point
			App.Run(new MiscPongMania());												// Run Our Example As A Windows Forms Application
		}
		#endregion Main()

		// --- Basecode Methods ---
		#region Initialize()
		/// <summary>
		/// Overrides OpenGL's initialization.
		/// </summary>
		public override void Initialize() {
			LoadTextures();																// Jump To Texture Loading Routine
			glEnable(GL_TEXTURE_2D);													// Enable Texture Mapping

			glClearColor(0.7f, 0.7f, 0.8f, 1.0f);										// Background Color
			glClearDepth(1.0f);															// Depth Buffer Setup

			glEnable(GL_DEPTH_TEST);
			glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);							// Type Of Blending To Perform

			glHint(GL_PERSPECTIVE_CORRECTION_HINT, GL_NICEST);							// Really Nice Perspective Calculations

			float[] fogColor = {0.7f, 0.7f, 0.8f, 0.0f};								// Fog Color
			glFogi(GL_FOG_MODE, (int) GL_EXP2);											// Fog Mode
			glFogfv(GL_FOG_COLOR, fogColor);											// Set Fog Color
			glFogf(GL_FOG_DENSITY, 0.05f);												// How Dense Will The Fog Be
			glHint(GL_FOG_HINT, GL_DONT_CARE);											// Fog Hint Value
			glFogf(GL_FOG_START, 0.0f);													// Fog Start Depth
			glFogf(GL_FOG_END, 10.0f);													// Fog End Depth
			glEnable(GL_FOG);															// Enables GL_FOG

			float[] m_ambient = {0.1f, 0.1f, 0.1f, 1.0f};
			float[] m_diffuse = {0.5f, 0.5f, 0.5f, 1.0f};
			float[] m_specular = {1.0f, 1.0f, 1.0f, 1.0f};
			float m_shininess = 10.0f;
			glMaterialfv(GL_FRONT_AND_BACK, GL_AMBIENT, m_ambient);
			glMaterialfv(GL_FRONT_AND_BACK, GL_DIFFUSE, m_diffuse);
			glMaterialfv(GL_FRONT_AND_BACK, GL_SPECULAR, m_specular);
			glMaterialf (GL_FRONT_AND_BACK, GL_SHININESS, m_shininess);

			float[] LightAmbient = {0.2f, 0.2f, 0.2f, 1.0f};
			float[] LightDiffuse = {0.5f, 0.5f, 0.5f, 1.0f};
			float[] LightSpecular = {1.0f, 1.0f, 1.0f, 1.0f};
			float[] LightPosition = {0.0f, 0.0f, -11.0f, 1.0f};
			glLightModeli(GL_LIGHT_MODEL_LOCAL_VIEWER, (int) GL_TRUE);
			glLightfv(GL_LIGHT0, GL_AMBIENT, LightAmbient);								// Setup The Ambient Light
			glLightfv(GL_LIGHT0, GL_DIFFUSE, LightDiffuse);								// Setup The Diffuse Light
			glLightfv(GL_LIGHT0, GL_SPECULAR, LightSpecular);							// Setup The Diffuse Light
			glLightfv(GL_LIGHT0, GL_POSITION, LightPosition);							// Position The Light
			glEnable(GL_LIGHT0);
			glEnable(GL_COLOR_MATERIAL);
			glEnable(GL_LIGHTING);

			quadratic = gluNewQuadric();												// Create A Pointer To The Quadric Object (Return 0 If No Memory)
			gluQuadricNormals(quadratic, GLU_SMOOTH);									// Create Smooth Normals
			gluQuadricTexture(quadratic, (byte) GL_TRUE);								// Create Texture Coords

			timer.Start();
			Time1 = TimerGetTime();

			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws Misc Pong Mania scene.
		/// </summary>
		public override void Draw() {
			glClear(GL_DEPTH_BUFFER_BIT | GL_COLOR_BUFFER_BIT);							// Clear The Screen And The Depth Buffer
			glLoadIdentity();															// Reset The View
	
			if(!MENU) {
				r -= 0.01f;
				g -= 0.01f;
				if(r < 0) {
					r = 0;
				}
				if(g < 0) {
					g = 0;
				}

				Dist = Hypot(x - mx, z - mz);
				cDist = Hypot(x - cmx, z - cmz);

				if(z > 1) {
					PlaySound(@"..\..\data\MiscPongMania\youlost.wav", SND_SYNC);
					x = 0;
					y = -1.75f;
					z = -15.0f;
					xp = 1;
					zp = 1.3f;
					menuZ = 0;
					MENU = true;
				}
				else if(z < -19) {
					PlaySound(@"..\..\data\MiscPongMania\youwin.wav", SND_SYNC);
					x = 0;
					y = -1.75f;
					z = -15.0f;
					xp = 1;
					zp = 1.3f;
					menuZ = 0;
					MENU = true;
				}

				if(zp > 0 && Dist < 0.75f) {
					PlaySound(@"..\..\data\MiscPongMania\tennis.wav", SND_ASYNC);
					g = 1;
					zp = -zp;
					Speed = Hypot(Math.Abs(xp), Math.Abs(zp));
					xp += ((x - mx) * (Speed * 5)) / 3.14f;
					xp += mxp;
					zp += mzp * 5;
					if(xp > 0.25f || xp < - 0.25f) {
						xp *= 0.99f;
					}
					if(zp > 0.25f || zp < - 0.25f) {
						zp *= 0.99f;
					}
					z += zp /10;
				}
				else if(zp < 0 && cDist < 0.75f) {
					PlaySound(@"..\..\data\MiscPongMania\tennis.wav", SND_ASYNC);
					r = 1;
					zp = -zp;
					z += zp /10;
				}

				if(x > 2.5f) {
					x = 2.5f;
					xp = -xp;
					PlaySound(@"..\..\data\MiscPongMania\tennis1.wav", SND_ASYNC);
				}
				else if(x < -2.5f) {
					x = -2.5f;
					xp = -xp;
					PlaySound(@"..\..\data\MiscPongMania\tennis1.wav", SND_ASYNC);
				}

				x += xp / 25;
				z += zp / 25;
				mxp *= 0.95f;
				mzp *= 0.93f;
				mx += mxp;
				mz += mzp;

				if(mx >  2.25f) {
					mx =  2.25f;
					mxp = 0;
				}
				if(mx < -2.25f) {
					mx = -2.25f;
					mxp = 0;
				}
				if(mz < -7) {
					mz = -7;
					mzp = 0;
				}
				if(mz > -6+.25f) {
					mz = -6+.25f;
					mzp = 0;
				}
				if(cmx >  2.25f) {
					cmx =  2.25f;
					cmxp = 0;
				}
				if(cmx < -2.25f) {
					cmx = -2.25f;
					cmxp = 0;
				}

				cmxp += (x - cmx) / 5;
				cmxp *= 0.95f;
				cmx += cmxp / 25;
			}

			float[] LightDiffuse1 = {0.5f + r, 0.5f + g, 0.5f + b, 1.0f};

			glLightfv(GL_LIGHT0, GL_DIFFUSE, LightDiffuse1);

			glLoadIdentity();
			glTranslatef(0, -1.75f, -30);
			glColor4f(0.25f, 0.25f, 0.25f, 1.0f);
			glBindTexture(GL_TEXTURE_2D, texture[2]);

			RoomZrot += 2;
			glRotatef(RoomZrot, 0.0f, 0.0f, 1.0f);
			glColor3f(0.75f, 0.75f, 0.75f);

			gluCylinder(quadratic, 2.75f, 2.75f, 30, 32, 1);

			glEnable(GL_BLEND);
			glDisable(GL_DEPTH_TEST);

			glLoadIdentity();
			glColor4f(0.75f, 0.75f, 0.75f, 1.0f);

			glBindTexture(GL_TEXTURE_2D, texture[1]);
			glBegin(GL_QUADS);
				glNormal3f(0, 1, 0);
				glTexCoord2f(1, 1); glVertex3f(-2.75f, -2.0f, -30);
				glTexCoord2f(0, 1); glVertex3f( 2.75f, -2.0f, -30);
				glTexCoord2f(0, 0); glVertex3f( 2.75f, -2.0f,  0);
				glTexCoord2f(1, 0); glVertex3f(-2.75f, -2.0f,  0);
			glEnd();

			glDisable(GL_LIGHTING);
			glLoadIdentity();
			glTranslatef(x, -2.25f, z);

			glRotatef(180, 0.0f, 0.0f, 1.0f);
			glRotatef(xrot, 1.0f, 0.0f, 0.0f);
			glRotatef(zrot, 0.0f, 0.0f, 1.0f);

			// Reflected Puck
			glColor4f(0.25f, 0.25f, 0.25f, 0.25f);
			glBindTexture(GL_TEXTURE_2D, texture[0]);
			gluSphere(quadratic, 0.25f, 16, 16);
			glEnable(GL_LIGHTING);

			glDisable(GL_BLEND);
			glEnable(GL_DEPTH_TEST);

			glLoadIdentity();
			glTranslatef(x, y, z);
 
			glRotatef(xrot, 1.0f, 0.0f, 0.0f);
			glRotatef(zrot, 0.0f, 0.0f, 1.0f);

			// Puck
			glColor3f(1, 1, 1);
			gluSphere(quadratic, 0.25f, 16, 16);

			// Your Paddle
			glLoadIdentity();
			glColor3f(0.5f, 1, 0.5f);
			glBindTexture(GL_TEXTURE_2D, texture[2]);
			glTranslatef(mx, my, mz);
			glRotatef(180, 1.0f, 1.0f, 0.0f);
			gluSphere(quadratic, 0.5f, 2, 32);
			glLoadIdentity();
			glTranslatef(mx, my, mz);
			glRotatef(90, 1.0f, 0.0f, 0.0f);
			gluCylinder(quadratic, 0.5f, 0.5f, 0.3f, 32, 1);

			// Opponent's Paddle
			glLoadIdentity();
			glColor3f(1, 0.5f, 0.5f);
			glTranslatef(cmx, cmy, cmz);
			glRotatef(180, 1.0f, 1.0f, 0.0f);
			gluSphere(quadratic, 0.5f, 2, 16);
			glLoadIdentity();
			glTranslatef(cmx, cmy, cmz);
			glRotatef(90, 1.0f, 0.0f, 0.0f);
			gluCylinder(quadratic, 0.5f, 0.5f, 0.3f, 16, 1);

			xspeed += zp / 5;
			if(xspeed > Math.Abs(zp * 5)) {
				xspeed = Math.Abs(zp * 5);
			}
			if(xspeed < -Math.Abs(zp * 5)) {
				xspeed = -Math.Abs(zp * 5);
			}
			zspeed += xp / 5;
			if(zspeed > Math.Abs(xp * 5)) {
				zspeed = Math.Abs(xp * 5);
			}
			if(zspeed < -Math.Abs(xp * 5)) {
				zspeed = -Math.Abs(xp * 5);
			}
			xspeed *= 0.99f;
			zspeed *= 0.99f;
			xrot += xspeed;
			zrot -= zspeed;

			Time2 = TimerGetTime() / 1000;
			DiffTime = Math.Abs(Time2 - Time1);
			while(DiffTime < 0.015f) {													// 0.015f = 66 Frames Per Second
				System.Threading.Thread.Sleep(1);
				Time2 = TimerGetTime() / 1000;
				DiffTime = Math.Abs(Time2 - Time1);
			}
			Time1 = TimerGetTime() / 1000;

			if(MENU) {
				Menu();
			}
		}
		#endregion Draw()

		#region InputHelp()
		/// <summary>
		/// Overrides default input help, supplying lesson-specific help information.
		/// </summary>
		public override void InputHelp() {
			base.InputHelp();															// Set Up The Default Input Help

			DataRow dataRow;															// Row To Add

			dataRow = InputHelpDataTable.NewRow();										// Up Arrow - Move Forward / Menu Up
			dataRow["Input"] = "Up Arrow";
			dataRow["Effect"] = "Move Forward / Menu Up";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Down Arrow - Move Backward / Menu Down
			dataRow["Input"] = "Down Arrow";
			dataRow["Effect"] = "Move Backward / Menu Down";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Left Arrow - Move Left
			dataRow["Input"] = "Left Arrow";
			dataRow["Effect"] = "Move Left";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Right Arrow - Move Right
			dataRow["Input"] = "Right Arrow";
			dataRow["Effect"] = "Move Right";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Return - Select Menu Item
			dataRow["Input"] = "Enter";
			dataRow["Effect"] = "Select Menu Item";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);
		}
		#endregion InputHelp()

		#region ProcessInput()
		/// <summary>
		/// Overrides default input handling, adding lesson-specific input handling.
		/// </summary>
		public override void ProcessInput() {
			base.ProcessInput();														// Handle The Default Basecode Keys

			if(KeyState[(int) Keys.Up] && !up && MENU) {								// Is Up Arrow Being Pressed And Not Held Down And Menu Active?
				PlaySound(@"..\..\data\MiscPongMania\type.wav", SND_ASYNC);
				up = true;
				item--;
				if(item < 1) {
					item = 3;
				}
			}
			if(!KeyState[(int) Keys.Up]) {												// Has Up Arrow Been Released?
				up = false;																// If So, up Becomes false
			}

			if(KeyState[(int) Keys.Down] && !dn && MENU) {								// Is Down Arrow Being Pressed And Not Held Down And Menu Active?
				PlaySound(@"..\..\data\MiscPongMania\type.wav", SND_ASYNC);
				dn = true;
				item++;
				if(item > 3) {
					item = 1;
				}
			}
			if(!KeyState[(int) Keys.Down]) {											// Has Down Arrow Been Released?
				dn = false;																// If So, dn Becomes false
			}

			if(KeyState[(int) Keys.Return] && !rn && MENU) {							// Is Return Key Being Pressed And Not Held Down And Menu Active?
				rn = true;
				if(PlayAgain) {
					MENU = !MENU;
				}
				else if(Exit) {
					App.Terminate();
				}
			}
			if(!KeyState[(int) Keys.Return]) {											// Has Return Key Been Released?
				rn = false;																// If So, rn Becomes false
			}

			switch(item) {
				case 1:
					PlayAgain = true;
					Options = false;
					Exit = false;
					break;
				case 2:
					PlayAgain = false;
					Options = true;
					Exit = false;
					break;
				case 3:
					PlayAgain = false;
					Options = false;
					Exit = true;
					break;
			}

			if(KeyState[(int) Keys.Up] && !MENU) {										// Is Up Arrow Being Pressed And Menu Not Active?
				mzp -= 0.0055f;
			}

			if(KeyState[(int) Keys.Down] && !MENU) {									// Is Down Arrow Being Pressed And Menu Not Active?
				mzp += 0.0055f;
			}

			if(KeyState[(int) Keys.Left] && !MENU) {									// Is Left Arrow Being Pressed And Menu Not Active?
				mxp -= 0.0055f;
			}

			if(KeyState[(int) Keys.Right] && !MENU) {									// Is Right Arrow Being Pressed And Menu Not Active?
				mxp += 0.0055f;
			}
		}
		#endregion ProcessInput()

		// --- Example Methods ---
		#region float Hypot(float a, float b)
		/// <summary>
		/// Find the hypotenuse.
		/// </summary>
		/// <param name="a">First point.</param>
		/// <param name="b">Second point.</param>
		/// <returns></returns>
		private static float Hypot(float a, float b) {
			return (float) Math.Sqrt((a * a) + (b * b));
		}
		#endregion float Hypot(float a, float b)

		#region LoadTextures()
		/// <summary>
		/// Loads and creates the textures.
		/// </summary>
		private void LoadTextures() {
			// The Files To Load
			string filename = @"..\..\data\MiscPongMania\BlueGrain.bmp";
			Bitmap bitmap = null;														// The Bitmap Image For Our Texture
			Rectangle rectangle;														// The Rectangle For Locking The Bitmap In Memory
			BitmapData bitmapData = null;												// The Bitmap's Pixel Data

			// Load The Bitmaps
			try {
				glGenTextures(6, texture);												// Create 6 Textures

				bitmap = new Bitmap(filename);											// Load The File As A Bitmap
				bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);						// Flip The Bitmap Along The Y-Axis
				rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);			// Select The Whole Bitmap

				// Get The Pixel Data From The Locked Bitmap
				bitmapData = bitmap.LockBits(rectangle, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

				// Create MipMapped Texture
				glBindTexture(GL_TEXTURE_2D, texture[0]);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_NEAREST);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR); 
				gluBuild2DMipmaps(GL_TEXTURE_2D, 3, bitmap.Width, bitmap.Height, GL_BGR_EXT, GL_UNSIGNED_BYTE, bitmapData.Scan0);

				filename = @"..\..\data\MiscPongMania\wood5.bmp";
				bitmap = new Bitmap(filename);											// Load The File As A Bitmap
				bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);						// Flip The Bitmap Along The Y-Axis
				rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);			// Select The Whole Bitmap

				// Get The Pixel Data From The Locked Bitmap
				bitmapData = bitmap.LockBits(rectangle, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

				// Create MipMapped Texture
				glBindTexture(GL_TEXTURE_2D, texture[1]);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_NEAREST);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR); 
				gluBuild2DMipmaps(GL_TEXTURE_2D, 3, bitmap.Width, bitmap.Height, GL_BGR_EXT, GL_UNSIGNED_BYTE, bitmapData.Scan0);

				filename = @"..\..\data\MiscPongMania\wood4.bmp";
				bitmap = new Bitmap(filename);											// Load The File As A Bitmap
				bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);						// Flip The Bitmap Along The Y-Axis
				rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);			// Select The Whole Bitmap

				// Get The Pixel Data From The Locked Bitmap
				bitmapData = bitmap.LockBits(rectangle, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

				// Create MipMapped Texture
				glBindTexture(GL_TEXTURE_2D, texture[2]);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_NEAREST);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR); 
				gluBuild2DMipmaps(GL_TEXTURE_2D, 3, bitmap.Width, bitmap.Height, GL_BGR_EXT, GL_UNSIGNED_BYTE, bitmapData.Scan0);

				filename = @"..\..\data\MiscPongMania\PlayAgain.bmp";
				bitmap = new Bitmap(filename);											// Load The File As A Bitmap
				bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);						// Flip The Bitmap Along The Y-Axis
				rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);			// Select The Whole Bitmap

				// Get The Pixel Data From The Locked Bitmap
				bitmapData = bitmap.LockBits(rectangle, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

				// Create MipMapped Texture
				glBindTexture(GL_TEXTURE_2D, texture[3]);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_NEAREST);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR); 
				gluBuild2DMipmaps(GL_TEXTURE_2D, 3, bitmap.Width, bitmap.Height, GL_BGR_EXT, GL_UNSIGNED_BYTE, bitmapData.Scan0);

				filename = @"..\..\data\MiscPongMania\Options.bmp";
				bitmap = new Bitmap(filename);											// Load The File As A Bitmap
				bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);						// Flip The Bitmap Along The Y-Axis
				rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);			// Select The Whole Bitmap

				// Get The Pixel Data From The Locked Bitmap
				bitmapData = bitmap.LockBits(rectangle, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

				// Create MipMapped Texture
				glBindTexture(GL_TEXTURE_2D, texture[4]);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_NEAREST);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR); 
				gluBuild2DMipmaps(GL_TEXTURE_2D, 3, bitmap.Width, bitmap.Height, GL_BGR_EXT, GL_UNSIGNED_BYTE, bitmapData.Scan0);

				filename = @"..\..\data\MiscPongMania\Exit.bmp";
				bitmap = new Bitmap(filename);											// Load The File As A Bitmap
				bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);						// Flip The Bitmap Along The Y-Axis
				rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);			// Select The Whole Bitmap

				// Get The Pixel Data From The Locked Bitmap
				bitmapData = bitmap.LockBits(rectangle, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

				// Create MipMapped Texture
				glBindTexture(GL_TEXTURE_2D, texture[5]);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_NEAREST);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR); 
				gluBuild2DMipmaps(GL_TEXTURE_2D, 3, bitmap.Width, bitmap.Height, GL_BGR_EXT, GL_UNSIGNED_BYTE, bitmapData.Scan0);
			}
			catch(Exception e) {
				// Handle Any Exceptions While Loading Textures, Exit App
				string errorMsg = "An Error Occurred While Loading Texture:\n\t" + filename + "\n" + "\n\nStack Trace:\n\t" + e.StackTrace + "\n";
				MessageBox.Show(errorMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				App.Terminate();
			}
			finally {
				if(bitmap != null) {
					bitmap.UnlockBits(bitmapData);										// Unlock The Pixel Data From Memory
					bitmap.Dispose();													// Clean Up The Bitmap
				}
			}
		}
		#endregion LoadTextures()

		#region Menu()
		/// <summary>
		/// Displays the menu.
		/// </summary>
		private static void Menu() {
			if(PlayAgain) {
				alpha1 += 0.025f;
				alpha2 -= 0.010f;
				alpha3 -= 0.010f;
				if(alpha1 > 0.75f) {
					alpha1 = 0.75f;
				}
				if(alpha2 < 0.5f) {
					alpha2 = 0.5f;
				}
				if(alpha3 < 0.5f) {
					alpha3 = 0.5f;
				}
				alpha1ap += 0.5f - alpha1a;
				alpha1a += alpha1ap / 250;
				alpha2a = 0;
				alpha2ap = 0;
				alpha3a = 0;
				alpha3ap = 0;
			}
			else if(Options) {
				alpha1 -= 0.010f;
				alpha2 += 0.025f;
				alpha3 -= 0.010f;
				if(alpha1 < 0.5f) {
					alpha1 = 0.5f;
				}
				if(alpha2 >  0.75f) {
					alpha2 = 0.75f;
				}
				if(alpha3 < 0.5f) {
					alpha3 = 0.5f;
				}
				alpha2ap += 0.5f - alpha2a;
				alpha2a += alpha2ap / 250;
				alpha1a = 0;
				alpha1ap = 0;
				alpha3a = 0;
				alpha3ap = 0;
			}
			else if(Exit) {
				alpha1 -= 0.010f;
				alpha2 -= 0.010f;
				alpha3 += 0.025f;
				if(alpha1 < 0.5f) {
					alpha1 = 0.5f;
				}
				if(alpha2 < 0.5f) {
					alpha2 = 0.5f;
				}
				if(alpha3 > 0.75f) {
					alpha3 = 0.75f;
				}
				alpha3ap += 0.5f - alpha3a;
				alpha3a += alpha3ap / 250;
				alpha1a = 0;
				alpha1ap = 0;
				alpha2a = 0;
				alpha2ap = 0;
			}

			glEnable(GL_BLEND);
			glDisable(GL_DEPTH_TEST);
			glDisable(GL_LIGHTING);

			int scalemenu = 2;

			menuZ -= 0.1f;
			if(menuZ < -9) {
				menuZ = -9;
			}

			glLoadIdentity();
			glTranslatef(0, 0, menuZ - 1);

			//Backdrop
			glBindTexture(GL_TEXTURE_2D, texture[2]);
			glColor4f(0, 0, 1, 1);
			glBegin(GL_TRIANGLE_STRIP);
				glTexCoord2f(1, 1); glVertex3f(1.0f * scalemenu, (0.8f) * scalemenu, 0);
				glTexCoord2f(1, 0); glVertex3f(1.0f * scalemenu, (-0.4f) * scalemenu, 0);
				glTexCoord2f(0, 1); glVertex3f(-1.0f * scalemenu, (0.8f) * scalemenu, 0);
				glTexCoord2f(0, 0); glVertex3f(-1.0f * scalemenu, (-0.4f) * scalemenu, 0);
			glEnd();

			// Play Again
			glBindTexture(GL_TEXTURE_2D, texture[3]);
			glColor4f(1, 1, 1, (alpha1 + alpha1a) / 2);
			glBegin(GL_TRIANGLE_STRIP);
				glTexCoord2f(1, 1); glVertex3f(1.0f * scalemenu, (0.4f + 0.4f) * scalemenu, 0);
				glTexCoord2f(1, 0); glVertex3f(1.0f * scalemenu, 0.4f * scalemenu, 0);
				glTexCoord2f(0, 1); glVertex3f(-1.0f * scalemenu, (0.4f + 0.4f) * scalemenu, 0);
				glTexCoord2f(0, 0); glVertex3f(-1.0f * scalemenu, 0.4f * scalemenu, 0);
			glEnd();

			// Options
			glBindTexture(GL_TEXTURE_2D, texture[4]);
			glColor4f(1, 1, 1, (alpha2 + alpha2a) / 2);
			glBegin(GL_TRIANGLE_STRIP);
				glTexCoord2f(1, 1); glVertex3f(1.0f * scalemenu, 0.4f * scalemenu, 0);
				glTexCoord2f(1, 0); glVertex3f(1.0f * scalemenu, 0, 0);
				glTexCoord2f(0, 1); glVertex3f(-1.0f * scalemenu, 0.4f * scalemenu, 0);
				glTexCoord2f(0, 0); glVertex3f(-1.0f * scalemenu, 0, 0);
			glEnd();

			// Exit
			glBindTexture(GL_TEXTURE_2D, texture[5]);
			glColor4f(1, 1, 1, (alpha3 + alpha3a) / 2);
			glBegin(GL_TRIANGLE_STRIP);
				glTexCoord2f(1, 1); glVertex3f(1.0f * scalemenu, (0.4f - 0.4f) * scalemenu, 0);
				glTexCoord2f(1, 0); glVertex3f(1.0f * scalemenu, (0 - 0.4f) * scalemenu, 0);
				glTexCoord2f(0, 1); glVertex3f(-1.0f * scalemenu, (0.4f - 0.4f) * scalemenu, 0);
				glTexCoord2f(0, 0); glVertex3f(-1.0f * scalemenu, (0 - 0.4f) * scalemenu, 0);
			glEnd();

			glEnable(GL_LIGHTING);
			glEnable(GL_DEPTH_TEST);
		}
		#endregion Menu()

		#region PlaySound(string filename, int flags)
		/// <summary>
		/// Plays a sound.
		/// </summary>
		/// <param name="filename">Filename of the sound.</param>
		/// <param name="flags">Flags modifying the playback of the sound.</param>
		private void PlaySound(string filename, int flags) {
			try {
				PlaySound(filename, 0, flags);											// Call The Extern PlaySound Method
			}
			catch(Exception e) {
				// Handle Any Exceptions While Playing Sound, Exit App
				string errorMsg = "An Error Occurred While Attempting To Play Sound:\n\t" + filename + "\n" + "\n\nStack Trace:\n\t" + e.StackTrace + "\n";
				MessageBox.Show(errorMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				App.Terminate();
			}
		}
		#endregion PlaySound(string filename, int flags)

		#region float TimerGetTime()
		/// <summary>
		/// Returns current time.
		/// </summary>
		/// <returns>A float representing current time.</returns>
		private static float TimerGetTime() {
			return ((float) (timer.Count - timer.StartCount) * (float) timer.Resolution) * 1000.0f;
		}
		#endregion float TimerGetTime()

		// --- Externs ---
		#region PlaySound(string filename, int module, int flags)
		/// <summary>
		/// Plays a sound.
		/// </summary>
		[DllImport("winmm.dll")]
		public static extern void PlaySound(string filename, int module, int flags);
		#endregion PlaySound(string filename, int module, int flags)
	}
}