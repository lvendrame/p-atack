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
 *		This Code Was Created By Jeff Molofee and GB Schmick 2000
 *		A HUGE Thanks To Fredric Echols For Cleaning Up
 *		And Optimizing The Base Code, Making It More Flexible!
 *		If You've Found This Code Useful, Please Let Me Know.
 *		Visit Our Sites At www.tiptup.com and nehe.gamedev.net
 */
#endregion Original Credits / License

using CsGL.Basecode;
using CsGL.OpenGL;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Windows.Forms;

#region AssemblyInfo
[assembly: AssemblyCompany("The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyCopyright("2002 The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyDescription("NeHe Lesson 18")]
[assembly: AssemblyProduct("NeHe Lesson 18")]
[assembly: AssemblyTitle("NeHe Lesson 18")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace NeHeLessons {
	/// <summary>
	/// NeHe Lesson 18 -- Quadrics (http://nehe.gamedev.net)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class NeHeLesson18 : Model {
		// --- Fields ---
		#region Private Fields
		private static bool light;														// Lighting On / Off
		private static bool lp;															// L Pressed?
		private static bool fp;															// F Pressed?
		private static bool sbp;														// Space Bar Pressed?

		private static int part1;														// Start Of Disc
		private static int part2;														// End Of Disc
		private static int p1 = 0;														// Increase 1
		private static int p2 = 1;														// Increase 2

		private static float xrot;														// X Rotation
		private static float yrot;														// Y Rotation
		private static float xspeed;													// X Rotation Speed
		private static float yspeed;													// Y Rotation Speed
		private static float z = -5.0f;													// Depth Into Screen

		private static float[] LightAmbient = {0.5f, 0.5f, 0.5f, 1.0f};					// Ambient Light Values
		private static float[] LightDiffuse = {1.0f, 1.0f, 1.0f, 1.0f};					// Diffuse Light Values
		private static float[] LightPosition = {0.0f, 0.0f, 2.0f, 1.0f};				// Light Position

		private static GLUquadric quadratic;											// Our Quadratic Object

		private static uint qobject;													// Which Quadratic Object To Draw
		private static int filter = 0;													// Which Filter To Use
		private static uint[] texture = new uint[3];									// Storage For 3 Textures
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Lesson title.
		/// </summary>
		public override string Title {
			get {
				return "NeHe Lesson 18 -- Quadrics";
			}
		}

		/// <summary>
		/// Lesson description.
		/// </summary>
		public override string Description {
			get {
				return "This lesson demonstrates OpenGL's quadrics.  With quadrics you can easily create complex objects such as spheres, discs, cylinders and cones with as little as one line of code.";
			}
		}

		/// <summary>
		/// Lesson URL.
		/// </summary>
		public override string Url {
			get {
				return "http://nehe.gamedev.net/tutorials/lesson.asp?l=18";
			}
		}
		#endregion Public Properties

		// --- Entry Point ---
		#region Main()
		/// <summary>
		/// Application's entry point, runs this NeHe lesson.
		/// </summary>
		public static void Main() {														// Entry Point
			App.Run(new NeHeLesson18());												// Run Our NeHe Lesson As A Windows Forms Application
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

			glLightfv(GL_LIGHT1, GL_AMBIENT, LightAmbient);								// Setup The Ambient Light
			glLightfv(GL_LIGHT1, GL_DIFFUSE, LightDiffuse);								// Setup The Diffuse Light
			glLightfv(GL_LIGHT1, GL_POSITION, LightPosition);							// Position The Light
			glEnable(GL_LIGHT1);														// Enable Light One

			quadratic = gluNewQuadric();												// Create A Quadric Object
			gluQuadricNormals(quadratic, GLU_SMOOTH);									// Create Smooth Normals
			gluQuadricTexture(quadratic, (byte) GL_TRUE);								// Create Texture Coords
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws NeHe Lesson 18 scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);							// Clear Screen And Depth Buffer
			glLoadIdentity();															// Reset The Current Modelview Matrix

			glTranslatef(0.0f, 0.0f, z);												// Translate Into/Out Of The Screen By z

			glRotatef(xrot, 1.0f, 0.0f, 0.0f);											// Rotate On The X Axis By xrot
			glRotatef(yrot, 0.0f, 1.0f, 0.0f);											// Rotate On The Y Axis By yrot

			glBindTexture(GL_TEXTURE_2D, texture[filter]);								// Select A Texture Based On filter

			switch(qobject) {															// Check qobject To Find Out What To Draw
				case 0:																	// Drawing Object 0
					DrawCube();															// Draw Our Cube
					break;																// Done
				case 1:																	// Drawing Object 1
					glTranslatef(0.0f, 0.0f, -1.5f);									// Center The Cylinder
					gluCylinder(quadratic, 1.0f, 1.0f, 3.0f, 32, 32);					// Draw Our Cylinder
					break;																// Done
				case 2:																	// Drawing Object 2
					gluDisk(quadratic, 0.5f, 1.5f, 32, 32);								// Draw A Disc (CD Shape)
					break;																// Done
				case 3:																	// Drawing Object 3
					gluSphere(quadratic, 1.3f, 32, 32);									// Draw A Sphere
					break;																// Done
				case 4:																	// Drawing Object 4
					glTranslatef(0.0f, 0.0f, -1.5f);									// Center The Cone
					gluCylinder(quadratic, 1.0f, 0.0f, 3.0f, 32, 32);					// A Cone With A Bottom Radius Of .5 And A Height Of 2
					break;																// Done
				case 5:																	// Drawing Object 5
					part1 += p1;														// Increase Start Angle
					part2 += p2;														// Increase Sweep Angle
					if(part1 > 359) {													// 360 Degrees
						p1 = 0;															// Stop Increasing Start Angle
						part1 = 0;														// Set Start Angle To Zero
						p2 = 1;															// Start Increasing Sweep Angle
						part2 = 0;														// Start Sweep Angle At Zero
					}
					if(part2 > 359) {													// 360 Degrees
						p1 = 1;															// Start Increasing Start Angle
						p2 = 0;															// Stop Increasing Sweep Angle
					}
					gluPartialDisk(quadratic, 0.5f, 1.5f, 32, 32, part1, part2 - part1);// A Disk Like The One Before
					break;																// Done
			}

			xrot += xspeed;																// Add xspeed To xrot
			yrot += yspeed;																// Add yspeed To yrot
		}
		#endregion Draw()

		#region InputHelp()
		/// <summary>
		/// Overrides default input help, supplying lesson-specific help information.
		/// </summary>
		public override void InputHelp() {
			base.InputHelp();															// Set Up The Default Input Help

			DataRow dataRow;															// Row To Add

			dataRow = InputHelpDataTable.NewRow();										// Up Arrow - Decrease X Speed
			dataRow["Input"] = "Up Arrow";
			dataRow["Effect"] = "Decrease X Speed";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Down Arrow - Increase X Speed
			dataRow["Input"] = "Down Arrow";
			dataRow["Effect"] = "Increase X Speed";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Left Arrow - Decrease Y Speed
			dataRow["Input"] = "Left Arrow";
			dataRow["Effect"] = "Decrease Y Speed";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Right Arrow - Increase Y Speed
			dataRow["Input"] = "Right Arrow";
			dataRow["Effect"] = "Increase Y Speed";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Page Up - Zoom Out
			dataRow["Input"] = "Page Up";
			dataRow["Effect"] = "Zoom Out";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Page Down - Zoom In
			dataRow["Input"] = "Page Down";
			dataRow["Effect"] = "Zoom In";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Space Bar - Cycle Through Objects
			dataRow["Input"] = "Space Bar";
			dataRow["Effect"] = "Cycle Through Objects [0-5]";
			if(qobject == 0) {
				dataRow["Current State"] = "0 (Cube)";
			}
			else if(qobject == 1) {
				dataRow["Current State"] = "1 (Cylinder)";
			}
			else if(qobject == 2) {
				dataRow["Current State"] = "2 (Disk)";
			}
			else if(qobject == 3) {
				dataRow["Current State"] = "3 (Sphere)";
			}
			else if(qobject == 4) {
				dataRow["Current State"] = "4 (Cone)";
			}
			else if(qobject == 5) {
				dataRow["Current State"] = "5 (Partial Disk)";
			}
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// L - Toggle Light
			dataRow["Input"] = "L";
			dataRow["Effect"] = "Toggle Light On / Off";
			if(light) {
				dataRow["Current State"] = "On";
			}
			else {
				dataRow["Current State"] = "Off";
			}
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// F - Cycle Through Texture Filters
			dataRow["Input"] = "F";
			dataRow["Effect"] = "Cycle Through Texture Filters [0-2]";
			if(filter == 0) {
				dataRow["Current State"] = "0 (Nearest)";
			}
			else if(filter == 1) {
				dataRow["Current State"] = "1 (Linear)";
			}
			else {
				dataRow["Current State"] = "2 (Mipmapped)";
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

			if(KeyState[(int) Keys.Up]) {												// Is Up Arrow Being Pressed?
				xspeed -= 0.01f;														// If So, Decrease xspeed
			}

			if(KeyState[(int) Keys.Down]) {												// Is Down Arrow Being Pressed?
				xspeed += 0.01f;														// If So, Increase xspeed
			}

			if(KeyState[(int) Keys.Left]) {												// Is Left Arrow Being Pressed?
				yspeed -= 0.01f;														// If So, Decrease yspeed
			}

			if(KeyState[(int) Keys.Right]) {											// Is Right Arrow Being Pressed?
				yspeed += 0.01f;														// If So, Increase yspeed
			}

			if(KeyState[(int) Keys.PageUp]) {											// Is Page Up Being Pressed?
				z -= 0.02f;																// If So, Move Into The Screen
			}

			if(KeyState[(int) Keys.PageDown]) {											// Is Page Down Being Pressed?
				z += 0.02f;																// If So, Move Towards The Viewer
			}

			if(KeyState[(int) Keys.Space] && !sbp) {									// Is Space Bar Being Pressed?
				sbp = true;																// sbp Becomes true
				qobject += 1;															// qobject Increases By One
				if(qobject > 5) {														// Is Value Greater Than 5?
					qobject = 0;														// If So, Set qobject To 0
				}
				UpdateInputHelp();														// Update The Input Help
			}
			if(!KeyState[(int) Keys.Space]) {											// Has Space Bar Been Released?
				sbp = false;															// If So, sbp Becomes false
			}

			if(KeyState[(int) Keys.L] && !lp) {											// Is L Key Being Pressed And Not Held Down?
				lp = true;																// lp Becomes true
				light = !light;															// Toggle Light true / false
				if(!light) {															// If Not Light
					glDisable(GL_LIGHTING);												// Disable Lighting
				}
				else {																	// Otherwise
					glEnable(GL_LIGHTING);												// Enable Lighting
				}
				UpdateInputHelp();														// Update The Input Help Screen
			}
			if(!KeyState[(int) Keys.L]) {												// Has L Key Been Released?
				lp = false;																// If So, lp Becomes false
			}

			if(KeyState[(int) Keys.F] && !fp) {											// Is F Key Being Pressed And Not Held Down?
				fp = true;																// fp Becomes true
				filter += 1;															// filter Value Increases By One
				if(filter > 2) {														// Is Value Greater Than 2?
					filter = 0;															// If So, Set filter To 0
				}
				UpdateInputHelp();														// Update The Input Help Screen
			}
			if(!KeyState[(int) Keys.F]) {												// Has F Key Been Released?
				fp = false;																// If So, fp Becomes false
			}
		}
		#endregion ProcessInput()

		// --- Lesson Methods ---
		#region DrawCube()
		/// <summary>
		/// Draws the cube.
		/// </summary>
		private void DrawCube() {
			glBegin(GL_QUADS);															// Start Drawing Quads
				// Front Face
				glNormal3f(0.0f, 0.0f, 1.0f);											// Normal Pointing Towards Viewer
				glTexCoord2f(0.0f, 0.0f); glVertex3f(-1.0f, -1.0f,  1.0f);				// Point 1 (Front)
				glTexCoord2f(1.0f, 0.0f); glVertex3f( 1.0f, -1.0f,  1.0f);				// Point 2 (Front)
				glTexCoord2f(1.0f, 1.0f); glVertex3f( 1.0f,  1.0f,  1.0f);				// Point 3 (Front)
				glTexCoord2f(0.0f, 1.0f); glVertex3f(-1.0f,  1.0f,  1.0f);				// Point 4 (Front)
				// Back Face
				glNormal3f(0.0f, 0.0f, -1.0f);											// Normal Pointing Away From Viewer
				glTexCoord2f(1.0f, 0.0f); glVertex3f(-1.0f, -1.0f, -1.0f);				// Point 1 (Back)
				glTexCoord2f(1.0f, 1.0f); glVertex3f(-1.0f,  1.0f, -1.0f);				// Point 2 (Back)
				glTexCoord2f(0.0f, 1.0f); glVertex3f( 1.0f,  1.0f, -1.0f);				// Point 3 (Back)
				glTexCoord2f(0.0f, 0.0f); glVertex3f( 1.0f, -1.0f, -1.0f);				// Point 4 (Back)
				// Top Face
				glNormal3f(0.0f, 1.0f, 0.0f);											// Normal Pointing Up
				glTexCoord2f(0.0f, 1.0f); glVertex3f(-1.0f,  1.0f, -1.0f);				// Point 1 (Top)
				glTexCoord2f(0.0f, 0.0f); glVertex3f(-1.0f,  1.0f,  1.0f);				// Point 2 (Top)
				glTexCoord2f(1.0f, 0.0f); glVertex3f( 1.0f,  1.0f,  1.0f);				// Point 3 (Top)
				glTexCoord2f(1.0f, 1.0f); glVertex3f( 1.0f,  1.0f, -1.0f);				// Point 4 (Top)
				// Bottom Face
				glNormal3f(0.0f, -1.0f, 0.0f);											// Normal Pointing Down
				glTexCoord2f(1.0f, 1.0f); glVertex3f(-1.0f, -1.0f, -1.0f);				// Point 1 (Bottom)
				glTexCoord2f(0.0f, 1.0f); glVertex3f( 1.0f, -1.0f, -1.0f);				// Point 2 (Bottom)
				glTexCoord2f(0.0f, 0.0f); glVertex3f( 1.0f, -1.0f,  1.0f);				// Point 3 (Bottom)
				glTexCoord2f(1.0f, 0.0f); glVertex3f(-1.0f, -1.0f,  1.0f);				// Point 4 (Bottom)
				// Right face
				glNormal3f(1.0f, 0.0f, 0.0f);											// Normal Pointing Right
				glTexCoord2f(1.0f, 0.0f); glVertex3f( 1.0f, -1.0f, -1.0f);				// Point 1 (Right)
				glTexCoord2f(1.0f, 1.0f); glVertex3f( 1.0f,  1.0f, -1.0f);				// Point 2 (Right)
				glTexCoord2f(0.0f, 1.0f); glVertex3f( 1.0f,  1.0f,  1.0f);				// Point 3 (Right)
				glTexCoord2f(0.0f, 0.0f); glVertex3f( 1.0f, -1.0f,  1.0f);				// Point 4 (Right)
				// Left Face
				glNormal3f(-1.0f, 0.0f, 0.0f);											// Normal Pointing Left
				glTexCoord2f(0.0f, 0.0f); glVertex3f(-1.0f, -1.0f, -1.0f);				// Point 1 (Left)
				glTexCoord2f(1.0f, 0.0f); glVertex3f(-1.0f, -1.0f,  1.0f);				// Point 2 (Left)
				glTexCoord2f(1.0f, 1.0f); glVertex3f(-1.0f,  1.0f,  1.0f);				// Point 3 (Left)
				glTexCoord2f(0.0f, 1.0f); glVertex3f(-1.0f,  1.0f, -1.0f);				// Point 4 (Left)
			glEnd();																	// Done Drawing Quads
		}
		#endregion DrawCube()

		#region LoadTextures()
		/// <summary>
		/// Loads and creates the textures.
		/// </summary>
		private void LoadTextures() {
			string filename = @"..\..\data\NeHeLesson18\Wall.bmp";						// The File To Load
			Bitmap bitmap = null;														// The Bitmap Image For Our Texture
			Rectangle rectangle;														// The Rectangle For Locking The Bitmap In Memory
			BitmapData bitmapData = null;												// The Bitmap's Pixel Data

			// Load The Bitmap
			try {
				bitmap = new Bitmap(filename);											// Load The File As A Bitmap
				bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);						// Flip The Bitmap Along The Y-Axis
				rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);			// Select The Whole Bitmap
				
				// Get The Pixel Data From The Locked Bitmap
				bitmapData = bitmap.LockBits(rectangle, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

				glGenTextures(3, texture);												// Create 3 Textures

				// Create Nearest Filtered Texture
				glBindTexture(GL_TEXTURE_2D, texture[0]);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST); 
				glTexImage2D(GL_TEXTURE_2D, 0, (int) GL_RGB8, bitmap.Width, bitmap.Height, 0, GL_BGR_EXT, GL_UNSIGNED_BYTE, bitmapData.Scan0);

				// Create Linear Filtered Texture
				glBindTexture(GL_TEXTURE_2D, texture[1]);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR); 
				glTexImage2D(GL_TEXTURE_2D, 0, (int) GL_RGB8, bitmap.Width, bitmap.Height, 0, GL_BGR_EXT, GL_UNSIGNED_BYTE, bitmapData.Scan0);

				// Create MipMapped Texture
				glBindTexture(GL_TEXTURE_2D, texture[2]);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_NEAREST);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR); 
				gluBuild2DMipmaps(GL_TEXTURE_2D, (int) GL_RGB8, bitmap.Width, bitmap.Height, GL_BGR_EXT, GL_UNSIGNED_BYTE, bitmapData.Scan0);
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
	}
}