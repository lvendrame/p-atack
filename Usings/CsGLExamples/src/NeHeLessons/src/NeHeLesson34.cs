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
 *		This Code Was Created By Ben Humphrey 2001
 *		If You've Found This Code Useful, Please Let Me Know.
 *		Visit NeHe Productions At http://nehe.gamedev.net
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
[assembly: AssemblyDescription("NeHe Lesson 34")]
[assembly: AssemblyProduct("NeHe Lesson 34")]
[assembly: AssemblyTitle("NeHe Lesson 34")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace NeHeLessons {
	/// <summary>
	/// NeHe Lesson 34 -- Beautiful Landscapes By Means Of Height Mapping (http://nehe.gamedev.net)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class NeHeLesson34 : Model {
		// --- Fields ---
		#region Private Fields
		private const int MAP_SIZE = 1024;												// Size Of Our .RAW Height Map
		private const int STEP_SIZE = 16;												// Width And Height Of Each Quad
		private const float HEIGHT_RATIO = 1.5f;										// Ratio That The Y Is Scaled According To The X And Z
		private static bool render = true;												// Polygon Flag Set To true By Default
		private static byte[] heightmap = new byte[MAP_SIZE * MAP_SIZE];				// Holds The Height Map Data
		private static float scaleValue = 0.15f;										// Scale Value For The Terrain
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Lesson title.
		/// </summary>
		public override string Title {
			get {
				return "NeHe Lesson 34 -- Beautiful Landscapes By Means Of Height Mapping";
			}
		}

		/// <summary>
		/// Lesson description.
		/// </summary>
		public override string Description {
			get {
				return "This lesson teaches you how to convert a 2D greyscale height map image into a simple 3D landscape.";
			}
		}

		/// <summary>
		/// Lesson URL
		/// </summary>
		public override string Url {
			get {
				return "http://nehe.gamedev.net/tutorials/lesson.asp?l=34";
			}
		}
		#endregion Public Properties

		// --- Entry Point ---
		#region Main()
		/// <summary>
		/// Application's entry point, runs this NeHe lesson.
		/// </summary>
		public static void Main() {														// Entry Point
			App.Run(new NeHeLesson34());												// Run Our NeHe Lesson As A Windows Forms Application
		}
		#endregion Main()

		// --- Basecode Methods ---
		#region Initialize()
		/// <summary>
		/// Overrides OpenGL's initialization.
		/// </summary>
		public override void Initialize() {
			base.Initialize();															// Run The Base Initialization

			// Read In The Height Map From The .RAW File And Put It In Our
			// heightmap Array.  We Also Pass In The Size Of The .RAW File (1024)
			LoadRawFile(@"..\..\data\NeHeLesson34\Terrain.raw", MAP_SIZE * MAP_SIZE);
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws NeHe Lesson 34 scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);							// Clear Screen And Depth Buffer
			glLoadIdentity();															// Reset The Current Modelview Matrix

			gluLookAt(212, 60, 194, 186, 55, 171, 0, 1, 0);								// This Determines The Camera's Position And View
			glScalef(scaleValue, scaleValue * HEIGHT_RATIO, scaleValue);				// Scale The Scene

			RenderHeightmap(heightmap);													// Render The Height Map
		}
		#endregion Draw()

		#region InputHelp()
		/// <summary>
		/// Overrides default input help, supplying lesson-specific help information.
		/// </summary>
		public override void InputHelp() {
			base.InputHelp();															// Set Up The Default Input Help

			DataRow dataRow;															// Row To Add

			dataRow = InputHelpDataTable.NewRow();										// Up Arrow - Zoom In
			dataRow["Input"] = "Up Arrow";
			dataRow["Effect"] = "Zoom In";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Down Arrow - Zoom Out
			dataRow["Input"] = "Down Arrow";
			dataRow["Effect"] = "Zoom Out";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Space Bar - Toggle Filled / Wireframe
			dataRow["Input"] = "Space Bar";
			dataRow["Effect"] = "Toggle Filled / Wireframe";
			if(render) {
				dataRow["Current State"] = "Filled";
			}
			else {
				dataRow["Current State"] = "Wireframe";
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
				scaleValue += 0.001f;													// Increase The Scale Value To Zoom In
			}

			if(KeyState[(int) Keys.Down]) {												// Is Down Arrow Being Pressed?
				scaleValue -= 0.001f;													// Decrease The Scale Value To Zoom Out
			}

			if(KeyState[(int) Keys.Space]) {											// Is Space Bar Being Pressed?
				render = !render;														// Change Rendering State Between Filled & Wireframe
				KeyState[(int) Keys.Space] = false;										// Mark Space Bar Off
				UpdateInputHelp();														// Update Input Help Screen
			}
		}
		#endregion ProcessInput()

		#region Setup()
		/// <summary>
		/// Overrides application and OpenGL settings and setup.
		/// </summary>
		public override void Setup() {
			base.Setup();																// Run The Regular Setup
			App.FarClippingPlane = 500f;												// Override GLU's FarClippingPlane Distance
		}
		#endregion Setup()

		// --- Lesson Methods ---
		#region int Height(byte[] aHeightmap, int X, int Y)
		/// <summary>
		/// Returns the height from a height map index.
		/// </summary>
		/// <param name="aHeightmap">The heightmap.</param>
		/// <param name="X">The X coordinate.</param>
		/// <param name="Y">The Y coordinate.</param>
		/// <returns></returns>
		private int Height(byte[] aHeightmap, int X, int Y) {
			int x = X % MAP_SIZE;														// Error Check Our x Value
			int y = Y % MAP_SIZE;														// Error Check Our y Value

			if(aHeightmap == null) {
				return 0;																// Make Sure Our Data Is Valid
			}
			
			return aHeightmap[x + (y * MAP_SIZE)];										// Index Into Our Height Array And Return The Height
		}
		#endregion int Height(byte[] aHeightmap, int X, int Y)

		#region LoadRawFile(string filename, int size)
		/// <summary>
		/// Loads the .RAW file and stores it in the heightmap.
		/// </summary>
		/// <param name="filename">File to read.</param>
		/// <param name="size">Number of bytes to read.</param>
		private void LoadRawFile(string filename, int size) {
			FileStream stream = null;													// Our FileStream
			ASCIIEncoding encoding = new ASCIIEncoding();								// ASCII Encoding
			BinaryReader reader = null;													// Our Reader

			try {
				// Open The File
				stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
				reader = new BinaryReader(stream, encoding);
				heightmap = reader.ReadBytes(size);										// Load The .RAW File Into Our heightmap Data Array
			}
			catch(Exception e) {
				// Handle Any Exceptions While Loading Terrain Data, Exit App
				string errorMsg = "An Error Occurred While Loading And Parsing World Data:\n\t" + filename + "\n" + "\n\nStack Trace:\n\t" + e.StackTrace + "\n";
				MessageBox.Show(errorMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				App.Terminate();
			}
			finally {
				if(reader != null) {
					reader.Close();														// Close The BinaryReader
				}
				if(stream != null) {
					stream.Close();														// Close The FileStream
				}
			}
		}
		#endregion LoadRawFile(string filename, int size)

		#region RenderHeightmap(byte[] aHeightmap)
		/// <summary>
		/// Renders the heightmap as quads.
		/// </summary>
		/// <param name="aHeightmap">The heightmap to render.</param>
		private void RenderHeightmap(byte[] aHeightmap) {
			int X, Y;																	// Create Some Variables To Walk The Array With.
			int x, y, z;																// Create Some Variables For Readability

			if(aHeightmap == null) {													// Make Sure Our Height Data Is Valid
				return;
			}

			if(render) {																// What We Want To Render
				glBegin(GL_QUADS);														// Render Polygons
			}
			else {
				glBegin(GL_LINES);														// Render Lines Instead
			}

			for(X = 0; X < MAP_SIZE; X += STEP_SIZE)									// Loop The X Coordinates
				for (Y = 0; Y < MAP_SIZE; Y += STEP_SIZE) {								// Loop The Y Coordinates
					// Get The (X, Y, Z) Value For The Bottom Left Vertex
					x = X;
					y = Height(aHeightmap, X, Y);
					z = Y;
					SetVertexColor(aHeightmap, x, z);									// Set The Color Value Of The Current Vertex
					glVertex3i(x, y, z);												// Send This Vertex To OpenGL To Be Rendered

					// Get The (X, Y, Z) Value For The Top Left Vertex
					x = X;
					y = Height(aHeightmap, X, Y + STEP_SIZE);
					z = Y + STEP_SIZE;
					SetVertexColor(aHeightmap, x, z);									// Set The Color Value Of The Current Vertex
					glVertex3i(x, y, z);												// Send This Vertex To OpenGL To Be Rendered

					// Get The (X, Y, Z) Value For The Top Right Vertex
					x = X + STEP_SIZE;
					y = Height(aHeightmap, X + STEP_SIZE, Y + STEP_SIZE);
					z = Y + STEP_SIZE;
					SetVertexColor(aHeightmap, x, z);									// Set The Color Value Of The Current Vertex
					glVertex3i(x, y, z);												// Send This Vertex To OpenGL To Be Rendered

					// Get The (X, Y, Z) Value For The Bottom Right Vertex
					x = X + STEP_SIZE;
					y = Height(aHeightmap, X + STEP_SIZE, Y);
					z = Y;
					SetVertexColor(aHeightmap, x, z);									// Set The Color Value Of The Current Vertex
					glVertex3i(x, y, z);												// Send This Vertex To OpenGL To Be Rendered
				}
			glEnd();																	// Done Rendering Either Quads Or Lines
			glColor4f(1.0f, 1.0f, 1.0f, 1.0f);											// Reset The Color
		}
		#endregion Renderheightmap(byte[] aHeightmap)

		#region SetVertexColor(byte[] aHeightmap, int x, int y)
		/// <summary>
		/// Sets the color value for a particular index depending on the height index.
		/// </summary>
		/// <param name="aHeightmap">The heightmap.</param>
		/// <param name="x">The X coordinate.</param>
		/// <param name="y">The Y coordinate.</param>
		private void SetVertexColor(byte[] aHeightmap, int x, int y) {
			if(aHeightmap == null) {													// Make Sure Our Height Data Is Valid
				return;
			}

			float fColor = -0.15f + (Height(aHeightmap, x, y ) / 256.0f);				// Calculate Appropriate Blue Color

			glColor3f(0.0f, 0.0f, fColor);												// Assign This Blue Shade To The Current Vertex
		}
		#endregion SetVertexColor(byte[] aHeightmap, int x, int y)
	}
}