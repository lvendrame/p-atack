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
 *		This Code Was Created By Jeff Molofee 2000
 *		If You've Found This Code Useful, Please Let Me Know.
 *		Visit My Site At nehe.gamedev.net
 */
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
[assembly: AssemblyDescription("NeHe Lesson 19")]
[assembly: AssemblyProduct("NeHe Lesson 19")]
[assembly: AssemblyTitle("NeHe Lesson 19")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace NeHeLessons {
	/// <summary>
	/// NeHe Lesson 19 -- Particle Engine Using Triangle Strips (http://nehe.gamedev.net)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class NeHeLesson19 : Model {
		// --- Fields ---
		#region Private Fields
		private const int MAX_PARTICLES = 1000;											// Number Of Particles To Create
		private static bool rainbow = true;												// Rainbow Mode?
		private static bool sbp;														// Space Bar Pressed?
		private static bool rp;															// R Pressed?

		private static float slowdown = 2.0f;											// Slow Down Particles
		private static float xspeed = 0.0f;												// Base X Speed (To Allow Keyboard Direction Of Tail)
		private static float yspeed = 0.0f;												// Base Y Speed (To Allow Keyboard Direction Of Tail)
		private static float zoom = -40.0f;												// Used To Zoom Out

		private static uint loop;														// Misc Loop Variable
		private static uint col;														// Current Color Selection
		private static uint delay;														// Rainbow Effect Delay
		private static uint[] texture = new uint[1];									// Our Particle Texture

		private struct Particle {														// Create A Structure For Particle
			public bool active;															// Active True / False
			public float life;															// Particle Life
			public float fade;															// Fade Speed
			public float r;																// Red Value
			public float g;																// Green Value
			public float b;																// Blue Value
			public float x;																// X Position
			public float y;																// Y Position
			public float z;																// Z Position
			public float xi;															// X Direction
			public float yi;															// Y Direction
			public float zi;															// Z Direction
			public float xg;															// X Gravity
			public float yg;															// Y Gravity
			public float zg;															// Z Gravity
		}

		private static Particle[] particle = new Particle[MAX_PARTICLES];				// Particle Array

		private static float[][] colors = new float[12][] {								// Rainbow Of Colors
			new float[] {1.0f,  0.5f,  0.5f},
			new float[] {1.0f,  0.75f, 0.5f},
			new float[] {1.0f,  1.0f,  0.5f},
			new float[] {0.75f, 1.0f,  0.5f},
			new float[] {0.5f,  1.0f,  0.5f},
			new float[] {0.5f,  1.0f,  0.75f},
			new float[] {0.5f,  1.0f,  1.0f},
			new float[] {0.5f,  0.75f, 1.0f},
			new float[] {0.5f,  0.5f,  1.0f},
			new float[] {0.75f, 0.5f,  1.0f},
			new float[] {1.0f,  0.5f,  1.0f},
			new float[] {1.0f,  0.5f,  0.75f}
		};

		private static Random rand = new Random();										// Random Number Generator
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Lesson title.
		/// </summary>
		public override string Title {
			get {
				return "NeHe Lesson 19 -- Particle Engine Using Triangle Strips";
			}
		}

		/// <summary>
		/// Lesson description.
		/// </summary>
		public override string Description {
			get {
				return "In this lesson you will learn how to create a nice looking particle effect using OpenGL's triangle strips.  You could use particles similar to this to produce explosions, flames, smoke, water, or whatever suits your fancy.  The usage of triangle strips is useful in other areas of OpenGL development, due to them being a high-performance method of drawing objects.";
			}
		}

		/// <summary>
		/// Lesson URL.
		/// </summary>
		public override string Url {
			get {
				return "http://nehe.gamedev.net/tutorials/lesson.asp?l=19";
			}
		}
		#endregion Public Properties

		// --- Entry Point ---
		#region Main()
		/// <summary>
		/// Application's entry point, runs this NeHe lesson.
		/// </summary>
		public static void Main() {														// Entry Point
			App.Run(new NeHeLesson19());												// Run Our NeHe Lesson As A Windows Forms Application
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
			glClearColor(0.0f, 0.0f, 0.0f, 0.0f);										// Black Background
			glDisable(GL_DEPTH_TEST);													// Disables Depth Testing
			glEnable(GL_BLEND);															// Enable Blending
			glBlendFunc(GL_SRC_ALPHA, GL_ONE);											// Type Of Blending To Perform
			glHint(GL_POINT_SMOOTH_HINT, GL_NICEST);									// Really Nice Point Smoothing

			LoadTextures();																// Jump To Texture Loading Routine

			for(loop = 0; loop < MAX_PARTICLES; loop++)						{			// Initialize All The Particles
				particle[loop].active = true;											// Make All The Particles Active
				particle[loop].life = 1.0f;												// Give All The Particles Full Life
				particle[loop].fade = (rand.Next() % 100) / 1000.0f + 0.003f;			// Random Fade Speed
				particle[loop].r = colors[loop * (12 / MAX_PARTICLES)][0];				// Select Red Rainbow Color
				particle[loop].g = colors[loop * (12 / MAX_PARTICLES)][1];				// Select Red Rainbow Color
				particle[loop].b = colors[loop * (12 / MAX_PARTICLES)][2];				// Select Red Rainbow Color
				particle[loop].xi = ((rand.Next() % 50) - 26.0f) * 10.0f;				// Random Speed On X Axis
				particle[loop].yi = ((rand.Next() % 50) - 25.0f) * 10.0f;				// Random Speed On Y Axis
				particle[loop].zi = ((rand.Next() % 50) - 25.0f) * 10.0f;				// Random Speed On Z Axis
				particle[loop].xg = 0.0f;												// Set Horizontal Pull To Zero
				particle[loop].yg = -0.8f;												// Set Vertical Pull Downward
				particle[loop].zg = 0.0f;												// Set Pull On Z Axis To Zero
			}

			if(App.IsFullscreen) {														// Are We In Fullscreen Mode
				slowdown = 1.0f;														// Speed Up The Particles (3dfx Issue)
			}
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws NeHe Lesson 19 scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);							// Clear Screen And Depth Buffer
			glLoadIdentity();															// Reset The Current Modelview Matrix

			glBindTexture(GL_TEXTURE_2D, texture[0]);									// Select Our Texture

			for(loop = 0; loop < MAX_PARTICLES; loop++) {								// Loop Through All The Particles
				if(particle[loop].active) {												// If The Particle Is Active
					float x = particle[loop].x;											// Grab Our Particle X Position
					float y = particle[loop].y;											// Grab Our Particle Y Position
					float z = particle[loop].z + zoom;									// Particle Z Pos + Zoom
					
					// Draw The Particle Using Our RGB Values, Fade The Particle Based On It's Life
					glColor4f(particle[loop].r, particle[loop].g, particle[loop].b, particle[loop].life);

					glBegin(GL_TRIANGLE_STRIP);											// Build Quad From A Triangle Strip
						glTexCoord2d(1, 1); glVertex3f(x + 0.5f, y + 0.5f, z);			// Top Right
						glTexCoord2d(0, 1); glVertex3f(x - 0.5f, y + 0.5f, z);			// Top Left
						glTexCoord2d(1, 0); glVertex3f(x + 0.5f, y - 0.5f, z);			// Bottom Right
						glTexCoord2d(0, 0); glVertex3f(x - 0.5f, y - 0.5f, z);			// Bottom Left
					glEnd();															// Done Building Triangle Strip

					particle[loop].x += particle[loop].xi / (slowdown * 1000);			// Move On The X Axis By X Speed
					particle[loop].y += particle[loop].yi / (slowdown * 1000);			// Move On The Y Axis By Y Speed
					particle[loop].z += particle[loop].zi / (slowdown * 1000);			// Move On The Z Axis By Z Speed
					particle[loop].xi += particle[loop].xg;								// Take Pull On X Axis Into Account
					particle[loop].yi += particle[loop].yg;								// Take Pull On Y Axis Into Account
					particle[loop].zi += particle[loop].zg;								// Take Pull On Z Axis Into Account
					particle[loop].life -= particle[loop].fade;							// Reduce Particles Life By 'Fade'
					if(particle[loop].life < 0.0f) {									// If Particle Is Burned Out
						particle[loop].life = 1.0f;										// Give It New Life
						particle[loop].fade = (rand.Next() % 100) / 1000.0f + 0.003f;	// Random Fade Value
						particle[loop].x = 0.0f;										// Center On X Axis
						particle[loop].y = 0.0f;										// Center On Y Axis
						particle[loop].z = 0.0f;										// Center On Z Axis
						particle[loop].xi = (xspeed + (rand.Next() % 60)) - 32.0f;		// X Axis Speed And Direction
						particle[loop].yi = (yspeed + (rand.Next() % 60)) - 30.0f;		// Y Axis Speed And Direction
						particle[loop].zi = (rand.Next() % 60) - 30.0f;					// Z Axis Speed And Direction
						particle[loop].r = colors[col][0];								// Select Red From Color Table
						particle[loop].g = colors[col][1];								// Select Green From Color Table
						particle[loop].b = colors[col][2];								// Select Blue From Color Table
					}

					// These Keys Affect All Particles, So We'll Process Them Here, Inside
					// This Loop For Convienience, Rather Than In ProcessInput()
					if(KeyState[(int) Keys.NumPad8] && particle[loop].yg < 1.5f) {		// If NumPad 8 Is Pressed And Y Gravity Is Less Than 1.5
						particle[loop].yg += 0.01f;										// Increase Pull Upwards
					}

					if(KeyState[(int) Keys.NumPad2] && particle[loop].yg > -1.5f) {		// If NumPad 2 Is Pressed And Y Gravity Is Greater Than -1.5
						particle[loop].yg -= 0.01f;										// Increase Pull Downwards
					}

					if(KeyState[(int) Keys.NumPad4] && particle[loop].xg > -1.5f) {		// If NumPad 4 Is Pressed And X Gravity Is Greater Than -1.5
						particle[loop].xg -= 0.01f;										// Increase Pull Left
					}

					if(KeyState[(int) Keys.NumPad6] && particle[loop].xg < 1.5f) {		// If NumPad 6 Is Pressed And X Gravity Is Less Than 1.5
						particle[loop].xg += 0.01f;										// Increase Pull Right
					}

					if(KeyState[(int) Keys.Tab]) {										// If Tab Is Pressed
						particle[loop].x = 0.0f;										// Center On X Axis
						particle[loop].y = 0.0f;										// Center On Y Axis
						particle[loop].z = 0.0f;										// Center On Z Axis
						particle[loop].xi = ((rand.Next() % 50) - 26.0f) * 10.0f;		// Random Speed On X Axis
						particle[loop].yi = ((rand.Next() % 50) - 25.0f) * 10.0f;		// Random Speed On Y Axis
						particle[loop].zi = ((rand.Next() % 50) - 25.0f) * 10.0f;		// Random Speed On Z Axis
					}
				}
			}
			delay++;																	// Increase Rainbow Mode Color Cycling Delay Counter
			if(rainbow && (delay > 25)) {												// If rainbow Is On And delay Is Too High
				delay = 0;																// Reset The Rainbow Color Cycling Delay
				col++;																	// Change The Particle Color
				if(col > 11) {															// If col Is Too High
					col = 0;															// Reset It
				}
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

			dataRow = InputHelpDataTable.NewRow();										// Up Arrow - Increase Upward Speed
			dataRow["Input"] = "Up Arrow";
			dataRow["Effect"] = "Increase Upward Speed";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Down Arrow - Increase Downward Speed
			dataRow["Input"] = "Down Arrow";
			dataRow["Effect"] = "Increase Downward Speed";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Left Arrow - Increase Speed To Left
			dataRow["Input"] = "Left Arrow";
			dataRow["Effect"] = "Increase Speed To Left";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Right Arrow - Increase Speed To Right
			dataRow["Input"] = "Right Arrow";
			dataRow["Effect"] = "Increase Speed To Right";
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

			dataRow = InputHelpDataTable.NewRow();										// Space Bar - Step Through Colors
			dataRow["Input"] = "Space Bar";
			dataRow["Effect"] = "Step Through Colors";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// R - Toggle Rainbow Mode
			dataRow["Input"] = "R";
			dataRow["Effect"] = "Toggle Rainbow Mode On / Off";
			if(rainbow) {
				dataRow["Current State"] = "On";
			}
			else {
				dataRow["Current State"] = "Off";
			}
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Tab - Particle Burst
			dataRow["Input"] = "Tab";
			dataRow["Effect"] = "Particle Burst";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// NumPad + - Speed Up Particles
			dataRow["Input"] = "NumPad +";
			dataRow["Effect"] = "Speed Up Particles";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// NumPad - - Slow Down Particles
			dataRow["Input"] = "NumPad -";
			dataRow["Effect"] = "Slow Down Particles";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// NumPad 8 - Increase Pull Up
			dataRow["Input"] = "NumPad 8";
			dataRow["Effect"] = "Increase Pull Up";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// NumPad 2 - Increase Pull Down
			dataRow["Input"] = "NumPad 2";
			dataRow["Effect"] = "Increase Pull Down";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// NumPad 4 - Increase Pull Left
			dataRow["Input"] = "NumPad 4";
			dataRow["Effect"] = "Increase Pull Left";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// NumPad 6 - Increase Pull Right
			dataRow["Input"] = "NumPad 6";
			dataRow["Effect"] = "Increase Pull Right";
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

			if(KeyState[(int) Keys.Up]) {												// Is Up Arrow Being Pressed?
				if(yspeed < 200) {														// If Y Speed Is Less Than 200
					yspeed += 1.0f;														// Increase Upward Speed
				}
			}

			if(KeyState[(int) Keys.Down]) {												// Is Down Arrow Being Pressed?
				if(yspeed > -200) {														// If Y Speed Is Greater Than -200
					yspeed -= 1.0f;														// Increase Downward Speed
				}
			}

			if(KeyState[(int) Keys.Left]) {												// Is Left Arrow Being Pressed?
				if(xspeed > -200) {														// If X Speed Is Greater Than -200
					xspeed -= 1.0f;														// Increase Speed To The Left
				}
			}

			if(KeyState[(int) Keys.Right]) {											// Is Right Arrow Being Pressed?
				if(xspeed < 200) {														// If X Speed Is Less Than 200
					xspeed += 1.0f;														// Increase Speed To The Right
				}
			}

			if(KeyState[(int) Keys.PageUp]) {											// Is Page Up Being Pressed?
				zoom -= 0.1f;															// If So, Move Into The Screen
			}

			if(KeyState[(int) Keys.PageDown]) {											// Is Page Down Being Pressed?
				zoom += 0.1f;															// If So, Move Towards The Viewer
			}

			if((KeyState[(int) Keys.Space] && !sbp) || (rainbow && (delay > 25))) {		// Space Or Rainbow Mode
				if(KeyState[(int) Keys.Space]) {										// If Space Bar Is Pressed
					rainbow = false;													// Disable Rainbow Mode
					sbp = true;															// Flag Marking Space Bar Pressed
					delay = 0;															// Reset The Rainbow Color Cycling Delay
					col++;																// Change The Particle Color
					if(col > 11) {														// If Color Is Too High
						col = 0;														// Reset It
					}
					UpdateInputHelp();													// Update The Input Help
				}
			}
			if(!KeyState[(int) Keys.Space]) {											// Has Space Bar Been Released?
				sbp = false;															// If So, sbp Becomes false
			}

			if(KeyState[(int) Keys.R] && !rp) {											// Is R Key Being Pressed And Not Held Down?
				rp = true;																// rp Becomes true
				rainbow = !rainbow;														// Toggle Rainbow true / false
				UpdateInputHelp();														// Update The Input Help Screen
			}
			if(!KeyState[(int) Keys.R]) {												// Has R Key Been Released?
				rp = false;																// If So, rp Becomes false
			}

			if(KeyState[(int) Keys.Add]) {												// Is NumPad + Being Pressed?
				if(slowdown > 1.0f) {													// If slowdown Is Greater Than 1
					slowdown -= 0.01f;													// Speed Up Particles
				}
			}

			if(KeyState[(int) Keys.Subtract]) {											// Is NumPad - Being Pressed?
				if(slowdown < 4.0f) {													// If slowdown Is Less Than 4
					slowdown += 0.01f;													// Slow Down Particles
				}
			}
		}
		#endregion ProcessInput()

		#region Setup()
		/// <summary>
		/// Overrides application and OpenGL settings and setup.
		/// </summary>
		public override void Setup() {
			base.Setup();																// Run The Base Setup
			App.FarClippingPlane = 200f;												// Override GLU's FarClippingPlane Distance
		}
		#endregion Setup()

		// --- Lesson Methods ---
		#region LoadTextures()
		/// <summary>
		/// Loads and creates the texture.
		/// </summary>
		private void LoadTextures() {
			string filename = @"..\..\data\NeHeLesson19\Particle.bmp";					// The File To Load
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

				glGenTextures(1, texture);												// Create 1 Texture

				// Create Linear Filtered Texture
				glBindTexture(GL_TEXTURE_2D, texture[0]);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
				glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR); 
				glTexImage2D(GL_TEXTURE_2D, 0, (int) GL_RGB8, bitmap.Width, bitmap.Height, 0, GL_BGR_EXT, GL_UNSIGNED_BYTE, bitmapData.Scan0);
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