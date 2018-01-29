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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#region AssemblyInfo
[assembly: AssemblyCompany("The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyCopyright("2002 The CsGL Development Team (http://csgl.sourceforge.net)")]
[assembly: AssemblyDescription("NeHe Lesson 21")]
[assembly: AssemblyProduct("NeHe Lesson 21")]
[assembly: AssemblyTitle("NeHe Lesson 21")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace NeheLessons {
	/// <summary>
	/// NeHe Lesson 21 -- Lines, Antialiasing, Timing, Ortho View And Simple Sounds (http://nehe.gamedev.net)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class NeHeLesson21 : Model {
		// --- Fields ---
		#region Private Fields
		private const int SND_SYNC = 0x0000;										// Play Sound Synchronously (default)
		private const int SND_ASYNC = 0x0001;										// Play Sound Asynchronously
		private const int SND_LOOP = 0x0008;										// Loop Sound Until Next SoundPlay

		private static bool[ , ] vline = new bool[11, 10];							// Keeps Track Of The Vertical Lines
		private static bool[ , ] hline = new bool[10, 11];							// Keeps Track Of The Horizontal Lines
		private static bool ap;														// A Pressed?
		private static bool filled;													// Done Filling In The Grid?
		private static bool gameover;												// Is The Game Over?
		private static bool anti = true;											// Antialiasing?

		private static int loop1;													// Generic Loop1
		private static int loop2;													// Generic Loop2
		private static int delay;													// Enemy Delay
		private static int adjust = 2;												// Speed Adjustment For Really Slow Video Cards
		private static int lives = 5;												// Player Lives
		private static int level = 1;												// Internal Game Level
		private static int level2 = 1;												// Displayed Game Level
		private static int stage = 1;												// Game Stage

		private struct pobject {													// Create A Structure For Our Player
			public int fx, fy;														// Fine Movement Position
			public int x, y;														// Current Player Position
			public float spin;														// Spin Direction
		};

		private static pobject player;												// Player Information
		private static pobject[] enemy = new pobject[9];							// Enemy Information
		private static pobject hourglass;											// Hourglass Information

		private static HighResolutionTimer timer = new HighResolutionTimer();		// High Resolution Timer
		private static float resolution = (float) timer.Resolution;					// Timer Resolution
		private static float start;													// Frame Start Time
		private static int[] steps = {1, 2, 4, 5, 10, 20};							// Stepping Values For Difficulty / Speed Adjustment
		private static uint[] texture = new uint[2];								// Font Texture Storage Space
		private static uint dbase;													// Base Display List For The Font

		private static Random rand = new Random();									// Random Numbers
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Lesson title.
		/// </summary>
		public override string Title {
			get {
				return "NeHe Lesson 21 -- Lines, Antialiasing, Timing, Ortho View And Simple Sounds";
			}
		}

		/// <summary>
		/// Lesson description.
		/// </summary>
		public override string Description {
			get {
				return "In this lesson you will learn about: Lines, Anti-Aliasing, Orthographic Projection, Timing, Basic Sound Effects, and Simple Game Logic.  Whew, this is a big lesson, but the end result is a bit of fun.";
			}
		}

		/// <summary>
		/// Lesson URL.
		/// </summary>
		public override string Url {
			get {
				return "http://nehe.gamedev.net/tutorials/lesson.asp?l=21";
			}
		}
		#endregion Public Properties

		// --- Entry Point ---
		#region Main()
		/// <summary>
		/// Application's entry point, runs this NeHe lesson.
		/// </summary>
		public static void Main() {														// Entry Point
			App.Run(new NeHeLesson21());												// Run Our NeHe Lesson As A Windows Forms Application
		}
		#endregion Main()

		// --- Basecode Methods ---
		#region Initialize()
		/// <summary>
		/// Overrides OpenGL's initialization.
		/// </summary>
		public override void Initialize() {
			base.Initialize();															// Run The Base Initialization
			glDisable(GL_DEPTH_TEST);													// Disable Depth Test
			glHint(GL_LINE_SMOOTH_HINT, GL_NICEST);										// Set Line Antialiasing
			glEnable(GL_BLEND);															// Enable Blending
			glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);							// Type Of Blending To Use

			LoadTextures();																// Jump To Texture Loading Routine
			BuildFont();																// Build The Font
			ResetObjects();																// Reset The Objects
			timer.Start();																// Start The Timer
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws NeHe Lesson 21 scene.
		/// </summary>
		public override void Draw() {
			start = (((timer.Count - timer.StartCount) * resolution) * 1000.0f);		// Start The Timer
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);							// Clear Screen And Depth Buffer
			glBindTexture(GL_TEXTURE_2D, texture[0]);									// Select Our Font Texture
			glColor3f(1.0f, 0.5f, 1.0f);												// Set Color To Purple
			GlPrint(207, 24, "GRID CRAZY", 0);											// Write GRID CRAZY On The Screen
			glColor3f(1.0f, 1.0f, 0.0f);												// Set Color To Yellow
			GlPrint(20, 20, "Level:" + level2, 1);										// Write Actual Level Stats
			GlPrint(20, 40, "Stage:" + stage, 1);										// Write Stage Stats

			if(gameover) {																// Is The Game Over?
				// Pick A Random Color
				glColor3ub((byte)(rand.Next() * 255), (byte)(rand.Next() * 255), (byte)(rand.Next() * 255));
				GlPrint(472, 20, "GAME OVER", 1);										// Write GAME OVER To The Screen
				GlPrint(456, 40, "PRESS SPACE", 1);										// Write PRESS SPACE To The Screen
			}

			for(loop1 = 0; loop1 < (lives - 1); loop1++) {								// Loop Through Lives Minus Current Life
				glLoadIdentity();														// Reset The View
				glTranslatef(490 + (loop1 * 40.0f), 40.0f, 0.0f);						// Move To The Right Of Our Title Text
				glRotatef(-player.spin, 0.0f, 0.0f, 1.0f);								// Rotate Counter Clockwise
				glColor3f(0.0f, 1.0f, 0.0f);											// Set Player Color To Light Green
				glBegin(GL_LINES);														// Start Drawing Our Player Using Lines
					glVertex2d(-5, -5);													// Top Left Of Player
					glVertex2d( 5,  5);													// Bottom Right Of Player
					glVertex2d( 5, -5);													// Top Right Of Player
					glVertex2d(-5,  5);													// Bottom Left Of Player
				glEnd();																// Done Drawing The Player
				
				glRotatef(-player.spin * 0.5f, 0.0f, 0.0f, 1.0f);						// Rotate Counter Clockwise
				glColor3f(0.0f, 0.75f, 0.0f);											// Set Player Color To Dark Green
				glBegin(GL_LINES);														// Start Drawing Our Player Using Lines
					glVertex2d(-7,  0);													// Left Center Of Player
					glVertex2d( 7,  0);													// Right Center Of Player
					glVertex2d( 0, -7);													// Top Center Of Player
					glVertex2d( 0,  7);													// Bottom Center Of Player
				glEnd();																// Done Drawing The Player
			}

			filled = true;																// Set Filled To True Before Testing
			glLineWidth(2.0f);															// Set Line Width For Cells To 2.0f
			glDisable(GL_LINE_SMOOTH);													// Disable Antialiasing
			glLoadIdentity();															// Reset The Current Modelview Matrix

			for(loop1 = 0; loop1 < 11; loop1++) {										// Loop From Left To Right
				for(loop2 = 0; loop2 < 11; loop2++) {									// Loop From Top To Bottom
					if(loop1 < 10) {													// Don't Draw Too Far Right
						glColor3f(0.0f, 0.5f, 1.0f);									// Set Line Color To Blue
						if(hline[loop1, loop2]) {										// Has The Horizontal Line Been Traced?
							glColor3f(1.0f, 1.0f, 1.0f);								// If So, Set Line Color To White
						}

						if(!hline[loop1, loop2]) {										// If A Horizontal Line Isn't Filled
							filled = false;												// filled Becomes False
						}
						glBegin(GL_LINES);												// Start Drawing Horizontal Cell Borders
							glVertex2d(20 + (loop1 * 60), 70 + (loop2 * 40));			// Left Side Of Horizontal Line
							glVertex2d(80 + (loop1 * 60), 70 + (loop2 * 40));			// Right Side Of Horizontal Line
						glEnd();														// Done Drawing Horizontal Cell Borders
					}

					if(loop2 < 10) {													// Don't Draw Too Far Down
						glColor3f(0.0f, 0.5f, 1.0f);									// Set Line Color To Blue
						if(vline[loop1, loop2]) {										// Has The Horizontal Line Been Traced
							glColor3f(1.0f, 1.0f, 1.0f);								// If So, Set Line Color To White
						}

						if(!vline[loop1, loop2]) {										// If A Vertical Line Isn't Filled
							filled = false;												// filled Becomes False
						}
						glBegin(GL_LINES);												// Start Drawing Vertical Cell Borders
							glVertex2d(20 + (loop1 * 60), 70 + (loop2 * 40));			// Left Side Of Horizontal Line
							glVertex2d(20 + (loop1 * 60), 110 + (loop2 * 40));			// Right Side Of Horizontal Line
						glEnd();														// Done Drawing Vertical Cell Borders
					}

					glEnable(GL_TEXTURE_2D);											// Enable Texture Mapping
					glColor3f(1.0f, 1.0f, 1.0f);										// Bright White Color
					glBindTexture(GL_TEXTURE_2D, texture[1]);							// Select The Tile Image
					if((loop1 < 10) && (loop2 < 10)) {									// If In Bounds, Fill In Traced Boxes
						// Are All Sides Of The Box Traced?
						if(hline[loop1, loop2] && hline[loop1, loop2 + 1] && vline[loop1, loop2] && vline[loop1 + 1, loop2]) {
							glBegin(GL_QUADS);											// Draw A Textured Quad
								glTexCoord2f((float)(loop1 / 10.0f) + 0.1f, 1.0f - ((float)(loop2 / 10.0f)));
								// Top Right
								glVertex2d(20 + (loop1 * 60) + 59, (70 + loop2 * 40 + 1));
								glTexCoord2f((float)(loop1 / 10.0f), 1.0f - ((float)(loop2 / 10.0f)));
								// Top Left
								glVertex2d(20 + (loop1 * 60) + 1, (70 + loop2 * 40 + 1));
								glTexCoord2f((float)(loop1 / 10.0f), 1.0f - ((float)(loop2 / 10.0f) + 0.1f));
								// Bottom Left
								glVertex2d(20 + (loop1 * 60) + 1, (70 + loop2 * 40) + 39);
								glTexCoord2f((float)(loop1 / 10.0f) + 0.1f, 1.0f - ((float)(loop2 / 10.0f) + 0.1f));
								// Bottom Right
								glVertex2d(20 + (loop1 * 60) + 59, (70 + loop2 * 40) + 39);
							glEnd();													// Done Texturing The Box
						}
					}
					glDisable(GL_TEXTURE_2D);											// Disable Texture Mapping
				}
			}
			glLineWidth(1.0f);															// Set The Line Width To 1.0f

			if(anti) {																	// Is Anti TRUE?
				glEnable(GL_LINE_SMOOTH);												// If So, Enable Antialiasing
			}

			if(hourglass.fx == 1) {														// If fx=1 Draw The Hourglass
				glLoadIdentity();														// Reset The Modelview Matrix
				// Move To The Fine Hourglass Position
				glTranslatef(20.0f + (hourglass.x * 60), 70.0f + (hourglass.y * 40), 0.0f);
				glRotatef(hourglass.spin, 0.0f, 0.0f, 1.0f);							// Rotate Clockwise
				// Pick A Random Color
				glColor3ub((byte)(rand.Next() * 255), (byte)(rand.Next() * 255), (byte)(rand.Next() * 255));
				glBegin(GL_LINES);														// Start Drawing Our Hourglass Using Lines
					glVertex2d(-5, -5);													// Top Left Of Hourglass
					glVertex2d( 5,  5);													// Bottom Right Of Hourglass
					glVertex2d( 5, -5);													// Top Right Of Hourglass
					glVertex2d(-5,  5);													// Bottom Left Of Hourglass
					glVertex2d(-5,  5);													// Bottom Left Of Hourglass
					glVertex2d( 5,  5);													// Bottom Right Of Hourglass
					glVertex2d(-5, -5);													// Top Left Of Hourglass
					glVertex2d( 5, -5);													// Top Right Of Hourglass
				glEnd();																// Done Drawing The Hourglass
			}

			glLoadIdentity();															// Reset The Modelview Matrix
			glTranslatef(player.fx + 20.0f, player.fy + 70.0f, 0.0f);					// Move To The Fine Player Position
			glRotatef(player.spin, 0.0f, 0.0f, 1.0f);									// Rotate Clockwise
			glColor3f(0.0f, 1.0f, 0.0f);												// Set Player Color To Light Green
			glBegin(GL_LINES);															// Start Drawing Our Player Using Lines
				glVertex2d(-5, -5);														// Top Left Of Player
				glVertex2d( 5,  5);														// Bottom Right Of Player
				glVertex2d( 5, -5);														// Top Right Of Player
				glVertex2d(-5,  5);														// Bottom Left Of Player
			glEnd();																	// Done Drawing The Player
			glRotatef(player.spin * 0.5f, 0.0f, 0.0f, 1.0f);							// Rotate Clockwise
			glColor3f(0.0f, 0.75f, 0.0f);												// Set Player Color To Dark Green
			glBegin(GL_LINES);															// Start Drawing Our Player Using Lines
				glVertex2d(-7,  0);														// Left Center Of Player
				glVertex2d( 7,  0);														// Right Center Of Player
				glVertex2d( 0, -7);														// Top Center Of Player
				glVertex2d( 0,  7);														// Bottom Center Of Player
			glEnd();																	// Done Drawing The Player

			for(loop1 = 0; loop1 < (stage * level); loop1++) {							// Loop To Draw Enemies
				glLoadIdentity();														// Reset The Modelview Matrix
				glTranslatef(enemy[loop1].fx + 20.0f, enemy[loop1].fy + 70.0f, 0.0f);
				glColor3f(1.0f, 0.5f, 0.5f);											// Make Enemy Body Pink
				glBegin(GL_LINES);														// Start Drawing Enemy
					glVertex2d( 0, -7);													// Top Point Of Body
					glVertex2d(-7,  0);													// Left Point Of Body
					glVertex2d(-7,  0);													// Left Point Of Body
					glVertex2d( 0,  7);													// Bottom Point Of Body
					glVertex2d( 0,  7);													// Bottom Point Of Body
					glVertex2d( 7,  0);													// Right Point Of Body
					glVertex2d( 7,  0);													// Right Point Of Body
					glVertex2d( 0, -7);													// Top Point Of Body
				glEnd();																// Done Drawing Enemy Body
				glRotatef(enemy[loop1].spin, 0.0f, 0.0f, 1.0f);							// Rotate The Enemy Blade
				glColor3f(1.0f, 0.0f, 0.0f);											// Make Enemy Blade Red
				glBegin(GL_LINES);														// Start Drawing Enemy Blade
					glVertex2d(-7, -7);													// Top Left Of Enemy
					glVertex2d( 7,  7);													// Bottom Right Of Enemy
					glVertex2d(-7,  7);													// Bottom Left Of Enemy
					glVertex2d( 7, -7);													// Top Right Of Enemy
				glEnd();																// Done Drawing Enemy Blade
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

			dataRow = InputHelpDataTable.NewRow();										// Up Arrow - Move Up
			dataRow["Input"] = "Up Arrow";
			dataRow["Effect"] = "Move Up";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Down Arrow - Move Down
			dataRow["Input"] = "Down Arrow";
			dataRow["Effect"] = "Move Down";
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

			dataRow = InputHelpDataTable.NewRow();										// Space Bar - New Game
			dataRow["Input"] = "Space Bar";
			dataRow["Effect"] = "New Game";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// A - Toggle Antialiasing
			dataRow["Input"] = "A";
			dataRow["Effect"] = "Toggle Antialiasing On / Off";
			if(anti) {
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

			// Waste Some Cycles On Fast Systems, Affects Framerate!
			while((((timer.Count - timer.StartCount) * resolution) * 1000.0f) < (start + (steps[adjust] * 2.0f))) {
			}

			if(KeyState[(int) Keys.A] && !ap) {											// Is A Key Being Pressed And Not Held Down?
				ap = true;																// ap Becomes true
				anti = !anti;															// Toggle Antialiasing true / false
				UpdateInputHelp();														// Update Help Screen
			}
			if(!KeyState[(int) Keys.A]) {												// Has A Key Been Released?
				ap = false;																// If So, ap Becomes false
			}

			if(!gameover) {																// If Game Isn't Over Move Objects
				for(loop1 = 0; loop1 < (stage * level); loop1++) {						// Loop Through The Different Stages
					if((enemy[loop1].x < player.x) && (enemy[loop1].fy == enemy[loop1].y * 40)) {
						enemy[loop1].x++;												// Move The Enemy Right
					}

					if((enemy[loop1].x > player.x) && (enemy[loop1].fy == enemy[loop1].y * 40)) {
						enemy[loop1].x--;												// Move The Enemy Left
					}

					if((enemy[loop1].y < player.y) && (enemy[loop1].fx == enemy[loop1].x * 60)) {
						enemy[loop1].y++;												// Move The Enemy Down
					}

					if((enemy[loop1].y > player.y) && (enemy[loop1].fx == enemy[loop1].x * 60)) {
						enemy[loop1].y--;												// Move The Enemy Up
					}

					if(delay > (3 - level) && (hourglass.fx != 2)) {					// If Our Delay Is Done And Player Doesn't Have Hourglass
						delay = 0;														// Reset The Delay Counter Back To Zero
						for(loop2 = 0; loop2 < (stage * level); loop2++) {				// Loop Through All The Enemies
							if(enemy[loop2].fx < enemy[loop2].x * 60) {					// Is Fine Position On X Axis Lower Than Intended Position?
								enemy[loop2].fx += steps[adjust];						// If So, Increase Fine Position On X Axis
								enemy[loop2].spin += steps[adjust];						// Spin Enemy Clockwise
							}

							if(enemy[loop2].fx > enemy[loop2].x * 60) {					// Is Fine Position On X Axis Higher Than Intended Position?
								enemy[loop2].fx -= steps[adjust];						// If So, Decrease Fine Position On X Axis
								enemy[loop2].spin -= steps[adjust];						// Spin Enemy Counter Clockwise
							}

							if(enemy[loop2].fy < enemy[loop2].y * 40) {					// Is Fine Position On Y Axis Lower Than Intended Position?
								enemy[loop2].fy += steps[adjust];						// If So, Increase Fine Position On Y Axis
								enemy[loop2].spin += steps[adjust];						// Spin Enemy Clockwise
							}

							if(enemy[loop2].fy > enemy[loop2].y * 40) {					// Is Fine Position On Y Axis Higher Than Intended Position?
								enemy[loop2].fy -= steps[adjust];						// If So, Decrease Fine Position On Y Axis
								enemy[loop2].spin -= steps[adjust];						// Spin Enemy Counter Clockwise
							}
						}
					}

					// Are Any Of The Enemies On Top Of The Player?
					if((enemy[loop1].fx == player.fx) && (enemy[loop1].fy == player.fy)) {
						lives--;														// If So, Player Loses A Life

						if(lives==0) {													// Are We Out Of Lives?
							gameover = true;											// If So, gameover Becomes TRUE
						}

						ResetObjects();													// Reset Player / Enemy Positions
						PlaySound(@"..\..\data\NeHeLesson21\Die.wav", SND_SYNC);		// Play The Death Sound
					}
				}

				if(KeyState[(int) Keys.Right] && (player.x < 10) && (player.fx == player.x * 60) && (player.fy == player.y * 40)) {
					hline[player.x, player.y] = true;									// Mark The Current Horizontal Border As Filled
					player.x++;															// Move The Player Right
				}

				if(KeyState[(int) Keys.Left] && (player.x > 0) && (player.fx == player.x * 60) && (player.fy == player.y * 40)) {
					player.x--;															// Move The Player Left
					hline[player.x, player.y] = true;									// Mark The Current Horizontal Border As Filled
				}

				if(KeyState[(int) Keys.Down] && (player.y < 10) && (player.fx == player.x * 60) && (player.fy == player.y * 40)) {
					vline[player.x, player.y] = true;									// Mark The Current Vertical Border As Filled
					player.y++;															// Move The Player Down
				}

				if(KeyState[(int) Keys.Up] && (player.y > 0) && (player.fx == player.x * 60) && (player.fy == player.y * 40)) {
					player.y--;															// Move The Player Up
					vline[player.x, player.y] = true;									// Mark The Current Vertical Border As Filled
				}

				if(player.fx < player.x * 60) {											// Is Fine Position On X Axis Lower Than Intended Position?
					player.fx += steps[adjust];											// If So, Increase The Fine X Position
				}

				if(player.fx > player.x * 60) {											// Is Fine Position On X Axis Greater Than Intended Position?
					player.fx -= steps[adjust];											// If So, Decrease The Fine X Position
				}

				if(player.fy < player.y * 40) {											// Is Fine Position On Y Axis Lower Than Intended Position?
					player.fy += steps[adjust];											// If So, Increase The Fine Y Position
				}
				if(player.fy > player.y * 40) {											// Is Fine Position On Y Axis Lower Than Intended Position?
					player.fy -= steps[adjust];											// If So, Decrease The Fine Y Position
				}
			}
			else {																		// Otherwise
				if(KeyState[(int) Keys.Space]) {										// If Space Bar Is Pressed
					gameover = false;													// gameover Becomes false
					filled = true;														// filled Becomes true
					level = 1;															// Starting Level Is Set Back To 1
					level2 = 1;															// Displayed Level Is Also Set Back To 1
					stage = 0;															// Game Stage Is Set To 0
					lives = 5;															// Lives Is Set Back To 5
				}
			}

			if(filled) {																// Is The Grid Filled In?
				PlaySound(@"..\..\data\NeHeLesson21\Complete.wav", SND_SYNC);			// If So, Play The Level Complete Sound
				stage++;																// Increase The Stage
				if(stage > 3) {															// Is The Stage Higher Than 3?
					stage = 1;															// If So, Set The Stage To One
					level++;															// Increase The Level
					level2++;															// Increase The Displayed Level
					if(level > 3) {														// Is The Level Greater Than 3?
						level = 3;														// If So, Set The Level To 3
						lives++;														// Give The Player A Free Life
						if(lives > 5) {													// Does The Player Have More Than 5 Lives?
							lives = 5;													// If So, Set Lives To Five
						}
					}
				}

				ResetObjects();															// Reset Player / Enemy Positions

				for(loop1 = 0; loop1 < 11; loop1++) {									// Loop Through The Grid X Coordinates
					for(loop2 = 0; loop2 < 11; loop2++) {								// Loop Through The Grid Y Coordinates
						if(loop1 < 10) {												// If X Coordinate Is Less Than 10
							hline[loop1, loop2] = false;								// Set The Current Horizontal Value To FALSE
						}
						if(loop2 < 10) {												// If Y Coordinate Is Less Than 10
							vline[loop1, loop2] = false;								// Set The Current Vertical Value To FALSE
						}
					}
				}
			}

			// If The Player Hits The Hourglass While It's Being Displayed On The Screen
			if((player.fx == hourglass.x * 60) && (player.fy == hourglass.y * 40) && (hourglass.fx == 1)) {
				PlaySound(@"..\..\data\NeHeLesson21\Freeze.wav", SND_ASYNC | SND_LOOP);	// Play Freeze Enemy Sound
				hourglass.fx = 2;														// Set The hourglass fx Variable To Two
				hourglass.fy = 0;														// Set The hourglass fy Variable To Zero
			}

			player.spin += 0.5f * steps[adjust];										// Spin The Player Clockwise
			if(player.spin > 360.0f) {													// Is The spin Value Greater Than 360?
				player.spin -= 360;														// If So, Subtract 360
			}

			hourglass.spin -= 0.25f * steps[adjust];									// Spin The Hourglass Counter Clockwise
			if(hourglass.spin < 0.0f) {													// Is The spin Value Less Than 0?
				hourglass.spin += 360.0f;												// If So, Add 360
			}

			hourglass.fy += steps[adjust];												// Increase The hourglass fy Variable
			if((hourglass.fx == 0) && (hourglass.fy > 6000 / level)) {					// Is The hourglass fx Variable Equal To 0 And The fy
																						// Variable Greater Than 6000 Divided By The Current Level?
				PlaySound(@"..\..\data\NeHeLesson21\Hourglass.wav", SND_ASYNC);			// If So, Play The Hourglass Appears Sound 
				hourglass.x = (int)(rand.Next() % 10) + 1;								// Give The Hourglass A Random X Value
				hourglass.y = (int)rand.Next() % 11;									// Give The Hourglass A Random Y Value
				hourglass.fx = 1;														// Set hourglass fx Variable To One (Hourglass Stage)
				hourglass.fy = 0;														// Set hourglass fy Variable To Zero (Counter)
			}

			if((hourglass.fx == 1) && (hourglass.fy > 6000 / level)) {					// Is The hourglass fx Variable Equal To 1 And The fy
																						// Variable Greater Than 6000 Divided By The Current Level?
				hourglass.fx = 0;														// If So, Set fx To Zero (Hourglass Will Vanish)
				hourglass.fy = 0;														// Set fy to Zero (Counter Is Reset)
			}

			if((hourglass.fx == 2) && (hourglass.fy > 500 + (500 * level))) {			// Is The hourglass fx Variable Equal To 2 And The fy
																						// Variable Greater Than 500 Plus 500 Times The Current Level?
				PlaySound(null, 0, 0);													// If So, Kill The Freeze Sound
				hourglass.fx = 0;														// Set hourglass fx Variable To Zero
				hourglass.fy = 0;														// Set hourglass fy Variable To Zero
			}

			delay++;																	// Increase The Enemy Delay Counter
		}
		#endregion ProcessInput()

		#region Reshape(int width, int height)
		/// <summary>
		/// Overrides OpenGL Reshaping.
		/// </summary>
		/// <param name="width">New width.</param>
		/// <param name="height">New height.</param>
		public override void Reshape(int width, int height) {							// Resize And Initialize The GL Window
			if(height == 0) {															// If Height Is 0
				height = 1;																// Set To 1 To Prevent A Divide By Zero
			}

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
		/// <summary>
		/// Builds the font.
		/// </summary>
		private void BuildFont() {														// Build Our Font Display List
			dbase = glGenLists(256);													// Creating 256 Display Lists
			glBindTexture(GL_TEXTURE_2D, texture[0]);									// Select Our Font Texture

			for(loop1 = 0; loop1 < 256; loop1++) {										// Loop Through All 256 Lists
				float cx = (float)(loop1 % 16) / 16.0f;									// X Position Of Current Character
				float cy = (float)(loop1 / 16) / 16.0f;									// Y Position Of Current Character

				glNewList((uint)(dbase + loop1), GL_COMPILE);							// Start Building A List
					glBegin(GL_QUADS);													// Use A Quad For Each Character
						glTexCoord2f(cx, 1.0f - cy - 0.0625f);							// Texture Coord (Bottom Left)
						glVertex2d(0, 16);												// Vertex Coord (Bottom Left)
						glTexCoord2f(cx + 0.0625f, 1.0f - cy - 0.0625f);				// Texture Coord (Bottom Right)
						glVertex2i(16, 16);												// Vertex Coord (Bottom Right)
						glTexCoord2f(cx + 0.0625f, 1.0f - cy);							// Texture Coord (Top Right)
						glVertex2i(16, 0);												// Vertex Coord (Top Right)
						glTexCoord2f(cx, 1.0f - cy);									// Texture Coord (Top Left)
						glVertex2i(0, 0);												// Vertex Coord (Top Left)
					glEnd();															// Done Building Our Quad (Character)
					glTranslated(15, 0, 0);												// Move To The Right Of The Character
				glEndList();															// Done Building The Display List
			}																			// Loop Until All 256 Are Built
		}
		#endregion BuildFont()

		#region GlPrint(int x, int y, string str, int cset)
		/// <summary>
		/// Prints some text.
		/// </summary>
		/// <param name="x">X position.</param>
		/// <param name="y">Y position.</param>
		/// <param name="str">Text to print.</param>
		/// <param name="cset">Which character set?  0 is normal, 1 is italics.</param>
		public void GlPrint(int x, int y, string str, int cset) {						// Where The Printing Happens
			if(cset > 1) {																// Did User Choose An Invalid Character Set?
				cset = 1;																// If So, Select Set 1 (Italic)
			}
			glEnable(GL_TEXTURE_2D);													// Enable Texture Mapping
			glLoadIdentity();															// Reset The Modelview Matrix
			glTranslated(x, y, 0);														// Position The Text (0,0 - Bottom Left)
			glListBase((uint)(dbase - 32 + (128 * cset)));								// Choose The Font Set (0 or 1)

			if(cset == 0) {																// If Set 0 Is Being Used Enlarge Font
				glScalef(1.5f, 2.0f, 1.0f);												// Enlarge Font Width And Height
			}
			glCallLists(str.Length, GL_UNSIGNED_SHORT, str);							// Write The Text To The Screen
			glDisable(GL_TEXTURE_2D);													// Disable Texture Mapping
		}
		#endregion GlPrint(int x, int y, string str, int cset)

		#region LoadTextures()
		/// <summary>
		/// Loads and creates the textures.
		/// </summary>
		private void LoadTextures() {
			// The Files To Load
			string[] filename = {@"..\..\data\NeHeLesson21\Font.bmp", @"..\..\data\NeHeLesson21\Image.bmp"};
			Bitmap bitmap = null;														// The Bitmap Image For Our Texture
			Rectangle rectangle;														// The Rectangle For Locking The Bitmap In Memory
			BitmapData bitmapData = null;												// The Bitmap's Pixel Data

			// Load The Bitmaps
			try {
				glGenTextures(2, texture);												// Create 2 Textures

				for(loop1 = 0; loop1 < 2; loop1++) {
					bitmap = new Bitmap(filename[loop1]);								// Load The File As A Bitmap
					bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);					// Flip The Bitmap Along The Y-Axis
					rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);		// Select The Whole Bitmap
				
					// Get The Pixel Data From The Locked Bitmap
					bitmapData = bitmap.LockBits(rectangle, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

					// Create Linear Filtered Texture
					glBindTexture(GL_TEXTURE_2D, texture[loop1]);
					glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
					glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR); 
					glTexImage2D(GL_TEXTURE_2D, 0, (int) GL_RGB8, bitmap.Width, bitmap.Height, 0, GL_BGR_EXT, GL_UNSIGNED_BYTE, bitmapData.Scan0);
				}
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

		#region ResetObjects()
		/// <summary>
		/// Resets the player and enemy positions.
		/// </summary>
		private void ResetObjects() {													// Reset Player And Enemies
			player.x = 0;																// Reset Player X Position To Far Left Of The Screen
			player.y = 0;																// Reset Player Y Position To The Top Of The Screen
			player.fx = 0;																// Set Fine X Position To Match
			player.fy = 0;																// Set Fine Y Position To Match

			for(loop1 = 0; loop1 < (stage * level); loop1++) {							// Loop Through All The Enemies
				enemy[loop1].x = 5 + ((rand.Next()) % 6);								// Select A Random X Position
				enemy[loop1].y = rand.Next() % 11;										// Select A Random Y Position
				enemy[loop1].fx = enemy[loop1].x * 60;									// Set Fine X To Match
				enemy[loop1].fy = enemy[loop1].y * 40;									// Set Fine Y To Match
			}
		}
		#endregion ResetObjects()

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