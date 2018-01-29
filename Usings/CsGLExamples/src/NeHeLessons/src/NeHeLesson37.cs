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
/***************************************
*                                      *
*   Sami Hamlaoui's Cel-Shading Code   *
*       http://nehe.gamedev.net        *
*                 2001                 *
*                                      *
***************************************/

// Note: The original article for this code can be found at:
//     http://www.gamedev.net/reference/programming/features/celshading
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
[assembly: AssemblyDescription("NeHe Lesson 37")]
[assembly: AssemblyProduct("NeHe Lesson 37")]
[assembly: AssemblyTitle("NeHe Lesson 37")]
[assembly: AssemblyVersion("1.0.0.0")]
#endregion AssemblyInfo

namespace NeHeLessons {
	/// <summary>
	/// NeHe Lesson 37 -- Cel-Shading (http://nehe.gamedev.net)
	/// Implemented In C# By The CsGL Development Team (http://csgl.sourceforge.net)
	/// </summary>
	public sealed class NeHeLesson37 : Model {
		// --- Fields ---
		#region Private Fields
		private struct Matrix {															// A Structure To Hold An OpenGL Matrix
			public float[] data;														// We'll Use [16] Due To OpenGL's Matrix Format
		}

		private struct Vector {															// A Structure To Hold A Single Vector
			public float x, y, z;														// The Components Of The Vector
		}

		private struct Vertex {															// A Structure To Hold A Single Vertex
			public Vector nor;															// Vertex Normal
			public Vector pos;															// Vertex Position
		}

		private struct Polygon {														// A Structure To Hold A Single Polygon
			public Vertex[] verts;														// An Array For 3 Vertex Structures
		}

		private static bool outlineDraw = true;											// Flag To Draw The Outline
		private static bool outlineSmooth = false;										// Flag To Anti-Alias The Lines
		private static float[] outlineColor = {0.0f, 0.0f, 0.0f};						// Color Of The Lines
		private static float outlineWidth = 3.0f;										// Width Of The Lines

		private static Vector lightAngle;												// The Direction Of The Light

		private static float modelAngle = 0.0f;											// Y-Axis Angle Of The Model
		private static bool modelRotate = false;										// Flag To Rotate The Model

		private static Polygon[] polyData;												// Polygon Data
		private static int polyNum;														// Number Of Polygons

		private static uint[] shaderTexture = new uint[1];								// Storage For One Texture
		#endregion Private Fields

		#region Public Properties
		/// <summary>
		/// Lesson title.
		/// </summary>
		public override string Title {
			get {
				return "NeHe Lesson 37 -- Cel-Shading";
			}
		}

		/// <summary>
		/// Lesson description.
		/// </summary>
		public override string Description {
			get {
				return "This lesson teaches you how to load a model from a text file and texture it with a cartoon-like, cel shading method.";
			}
		}

		/// <summary>
		/// Lesson URL.
		/// </summary>
		public override string Url {
			get {
				return "http://nehe.gamedev.net/tutorials/lesson.asp?l=37";
			}
		}
		#endregion Public Properties

		// --- Entry Point ---
		#region Main()
		/// <summary>
		/// Application's entry point, runs this NeHe lesson.
		/// </summary>
		public static void Main() {														// Entry Point
			App.Run(new NeHeLesson37());												// Run Our NeHe Lesson As A Windows Forms Application
		}
		#endregion Main()

		// --- Basecode Methods ---
		#region Initialize()
		/// <summary>
		/// Overrides OpenGL's initialization.
		/// </summary>
		public override void Initialize() {
			base.Initialize();															// Run The Base Initialization
			glClearColor (0.7f, 0.7f, 0.7f, 0.0f);										// Light Grey Background
			glDepthFunc(GL_LESS);														// The Type Of Depth Test To Do
			glDisable(GL_LINE_SMOOTH);													// Initially Disable Line Smoothing
			glEnable(GL_CULL_FACE);														// Enable OpenGL Face Culling
			glDisable(GL_LIGHTING);														// Disable OpenGL Lighting

			lightAngle.x = 0.0f;														// Set The X Direction
			lightAngle.y = 0.0f;														// Set The Y Direction
			lightAngle.z = 1.0f;														// Set The Z Direction

			Normalize(ref lightAngle);													// Normalize The Light Direction
			ReadShader();																// Read The Shader
			ReadMesh();																	// Read The Mesh
		}
		#endregion Initialize()

		#region Draw()
		/// <summary>
		/// Draws NeHe Lesson 37 scene.
		/// </summary>
		public override void Draw() {													// Here's Where We Do All The Drawing
			float tmpShade;																// Temporary Shader Value
			Matrix tmpMatrix;															// Temporary Matrix Structure
			tmpMatrix.data = new float[16];												// Initialize Matrix data

			Vector tmpVector, tmpNormal;												// Temporary Vector Structures
			tmpVector.x = 0.0f;															// Set Some Initial Values So We Don't Get A Warning
			tmpVector.y = 0.0f;
			tmpVector.z = 0.0f;

			if(modelRotate) {															// Check To See If Rotation Is Enabled
				modelAngle += 2.0f;														// Update Angle
			}

			glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);							// Clear Screen And Depth Buffer
			glLoadIdentity();															// Reset The View

			if(outlineSmooth) {															// Check To See If We Want Anti-Aliased Lines
				glHint(GL_LINE_SMOOTH_HINT, GL_NICEST);									// Use The Good Calculations
				glEnable(GL_LINE_SMOOTH);												// Enable Anti-Aliasing
			}
			else {																		// We Don't Want Smooth Lines
				glDisable(GL_LINE_SMOOTH);												// Disable Anti-Aliasing
			}

			glTranslatef(0.0f, 0.0f, -2.0f);											// Move 2 Units Away From The Screen
			glRotatef(modelAngle, 0.0f, 1.0f, 0.0f);									// Rotate The Model On It's Y-Axis
			glGetFloatv(GL_MODELVIEW_MATRIX, tmpMatrix.data);							// Get The Generated Matrix

			// Cel-Shading Code
			glEnable(GL_TEXTURE_1D);													// Enable 1D Texturing
			glBindTexture(GL_TEXTURE_1D, shaderTexture[0]);								// Bind Our Texture
			glColor3f(1.0f, 1.0f, 1.0f);												// Set The Color Of The Model

			glBegin(GL_TRIANGLES);														// Tell OpenGL That We're Drawing Triangles
			for(int i = 0; i < polyNum; i++) {											// Loop Through Each Polygon
				for(int j = 0; j < 3; j++) {											// Loop Through Each Vertex
					tmpNormal.x = polyData[i].verts[j].nor.x;							// Fill Up The tmpNormal Structure With The
					tmpNormal.y = polyData[i].verts[j].nor.y;							// Current Vertices' Normal Values
					tmpNormal.z = polyData[i].verts[j].nor.z;

					RotateVector(tmpMatrix, tmpNormal, ref tmpVector);					// Rotate This By The Matrix
					Normalize(ref tmpVector);											// Normalize The New Normal

					tmpShade = DotProduct(tmpVector, lightAngle);						// Calculate The Shade Value
					if(tmpShade < 0.0f) {
						tmpShade = 0.0f;												// Clamp The Value to 0 If Negative
					}

					glTexCoord1f(tmpShade);												// Set The Texture Co-ordinate As The Shade Value
					// Send The Vertex Position
					glVertex3f(polyData[i].verts[j].pos.x, polyData[i].verts[j].pos.y, polyData[i].verts[j].pos.z);
				}
			}
			glEnd();																	// Tell OpenGL To Finish Drawing

			glDisable(GL_TEXTURE_1D);													// Disable 1D Textures

			// Outline Code
			if(outlineDraw) {															// Check To See If We Want To Draw The Outline
				glEnable(GL_BLEND);														// Enable Blending
				glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);						// Set The Blend Mode

				glPolygonMode(GL_BACK, GL_LINE);										// Draw Backfacing Polygons As Wireframes
				glLineWidth(outlineWidth);												// Set The Line Width
				glCullFace(GL_FRONT);													// Don't Draw Any Front-Facing Polygons
				glDepthFunc(GL_LEQUAL);													// Change The Depth Mode
				glColor3fv(outlineColor);												// Set The Outline Color
				glBegin(GL_TRIANGLES);													// Tell OpenGL What We Want To Draw
				for(int i = 0; i < polyNum; i++) {										// Loop Through Each Polygon
					for(int j = 0; j < 3; j++) {										// Loop Through Each Vertex
						// Send The Vertex Position
						glVertex3f(polyData[i].verts[j].pos.x, polyData[i].verts[j].pos.y, polyData[i].verts[j].pos.z);
					}
				}
				glEnd();																// Tell OpenGL We've Finished

				glDepthFunc(GL_LESS);													// Reset The Depth-Testing Mode
				glCullFace(GL_BACK);													// Reset The Face To Be Culled
				glPolygonMode(GL_BACK, GL_FILL);										// Reset Back-Facing Polygon Drawing Mode
				glDisable(GL_BLEND);													// Disable Blending
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

			dataRow = InputHelpDataTable.NewRow();										// Up Arrow - Increase Line Width
			dataRow["Input"] = "Up Arrow";
			dataRow["Effect"] = "Increase Line Width";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Down Arrow - Decrease Line Width
			dataRow["Input"] = "Down Arrow";
			dataRow["Effect"] = "Decrease Line Width";
			dataRow["Current State"] = "";
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// Space Bar - Toggle Model Rotation
			dataRow["Input"] = "Space Bar";
			dataRow["Effect"] = "Toggle Model Rotatation On / Off";
			if(modelRotate) {
				dataRow["Current State"] = "On";
			}
			else {
				dataRow["Current State"] = "Off";
			}
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// 1 - Toggle Outline Drawing
			dataRow["Input"] = "1";
			dataRow["Effect"] = "Toggle Outline Drawing On / Off";
			if(outlineDraw) {
				dataRow["Current State"] = "On";
			}
			else {
				dataRow["Current State"] = "Off";
			}
			InputHelpDataTable.Rows.Add(dataRow);

			dataRow = InputHelpDataTable.NewRow();										// 1 - Toggle Antialiasing
			dataRow["Input"] = "2";
			dataRow["Effect"] = "Toggle Antialiasing On / Off";
			if(outlineSmooth) {
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

			if(KeyState[(int) Keys.Up]) {												// Up Arrow Pressed?
				outlineWidth++;															// Increase Line Width
			}

			if(KeyState[(int) Keys.Down]) {												// Down Arrow Pressed?
				if(outlineWidth <= 1.0f) {												// Ensure A Valid Width
					outlineWidth = 1.0f;
				}
				else {
					outlineWidth--;														// Decrease Line Width
				}
			}

			if(KeyState[(int) Keys.Space]) {											// Space Bar Pressed?
				modelRotate = !modelRotate;												// Toggle Model Rotation
				KeyState[(int) Keys.Space] = false;										// Mark As Handled
				UpdateInputHelp();														// Update Help Screen
			}

			if(KeyState[(int) Keys.D1]) {												// 1 Pressed?
				outlineDraw = !outlineDraw;												// Toggle Outline Drawing
				KeyState[(int) Keys.D1] = false;										// Mark As Handled
				UpdateInputHelp();														// Update Help Screen
			}

			if(KeyState[(int) Keys.D2]) {												// 2 Pressed?
				outlineSmooth = !outlineSmooth;											// Toggle Anti-Aliasing
				KeyState[(int) Keys.D2] = false;										// Mark As Handled
				UpdateInputHelp();														// Update Help Screen
			}
		}
		#endregion ProcessInput()

		// --- Lesson Methods ---
		#region float DotProduct(Vector V1, Vector V2)
		/// <summary>
		/// Calculates the angle between two Vectors.
		/// </summary>
		/// <param name="V1">The first Vector.</param>
		/// <param name="V2">The second Vector.</param>
		/// <returns>The calculated angle.</returns>
		private float DotProduct(Vector V1, Vector V2) {								// Calculate The Angle Between The 2 Vectors
			return V1.x * V2.x + V1.y * V2.y + V1.z * V2.z;								// Return The Angle
		}
		#endregion float DotProduct(Vector V1, Vector V2)

		#region float Magnitude(Vector V)
		/// <summary>
		/// Calculates the length of a Vector.
		/// </summary>
		/// <param name="V">The Vector.</param>
		/// <returns>The calculated length.</returns>
		private float Magnitude(Vector V) {												// Calculate The Length Of The Vector
			return (float) Math.Sqrt(V.x * V.x + V.y * V.y + V.z * V.z);				// Return The Length Of The Vector
		}
		#endregion float Magnitude(Vector V)

		#region Normalize(ref Vector V)
		/// <summary>
		/// Creates a normalized Vector.
		/// </summary>
		/// <param name="V">The Vector to normalize.</param>
		private void Normalize(ref Vector V) {											// Creates A Vector With A Unit Length Of 1
			float M = Magnitude(V);														// Calculate The Length Of The Vector 

			if(M != 0.0f) {																// Make Sure We Don't Divide By 0 
				V.x /= M;																// Normalize The 3 Components 
				V.y /= M;
				V.z /= M;
			}
		}
		#endregion Normalize(ref Vector V)

		#region ReadMesh()
		/// <summary>
		/// Loads the mesh.
		/// </summary>
		private void ReadMesh() {
			// Read In The Shader File
			FileStream stream = null;
			ASCIIEncoding encoding = new ASCIIEncoding();
			BinaryReader reader = null;

			try {
				stream = new FileStream(@"..\..\data\NeHeLesson37\Model.txt", FileMode.Open, FileAccess.Read, FileShare.Read);
				reader = new BinaryReader(stream, encoding);

				polyNum = reader.ReadInt32();											// Read The Header (i.e. Number Of Polygons)
				polyData = new Polygon[polyNum];										// Initialize polyData
				for(int i = 0; i < polyNum; i++) {
					polyData[i].verts = new Vertex[3];									// Initialize The verts Array
					for(int j = 0; j < 3; j++) {										// Read In The polyData
						polyData[i].verts[j].nor.x = reader.ReadSingle();
						polyData[i].verts[j].nor.y = reader.ReadSingle();
						polyData[i].verts[j].nor.z = reader.ReadSingle();
						polyData[i].verts[j].pos.x = reader.ReadSingle();
						polyData[i].verts[j].pos.y = reader.ReadSingle();
						polyData[i].verts[j].pos.z = reader.ReadSingle();
					}
				}
			}
			catch(Exception e) {
				// Handle Any Exceptions While Loading Mesh, Exit App
				string errorMsg = "An Error Occurred While Loading Mesh:\n\t" + @"..\..\data\NeHeLesson37\Model.txt" + "\n" + "\n\nStack Trace:\n\t" + e.StackTrace + "\n";
				MessageBox.Show(errorMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				App.Terminate();
			}
			finally {
				// Close The File
				if(reader != null) {
					reader.Close();
				}
				if(stream != null) {
					stream.Close();
				}
			}
		}
		#endregion ReadMesh()

		#region ReadShader()
		/// <summary>
		/// Loads the shader.
		/// </summary>
		public void ReadShader() {
			// Read In The Shader File
			float[ , ] shaderData = new float[32, 3];									// Storage For The 96 Shader Values
			string oneline = "";
			StreamReader reader = null;
			ASCIIEncoding encoding = new ASCIIEncoding();

			try {
				reader = new StreamReader(@"..\..\data\NeHeLesson37\Shader.txt", encoding);

				for(int i = 0; i < 32; i++) {
					oneline = reader.ReadLine();
					shaderData[i, 0] = shaderData[i, 1] = shaderData[i, 2] = Single.Parse(oneline);
				}

				glGenTextures(1, shaderTexture);										// Get A Free Texture ID
				glBindTexture(GL_TEXTURE_1D, shaderTexture[0]);							// Bind This Texture. From Now On It Will Be 1D

				// For Crying Out Loud Don't Let OpenGL Use Bi/Trilinear Filtering!
				glTexParameteri(GL_TEXTURE_1D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
				glTexParameteri(GL_TEXTURE_1D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);

				// Upload Texture
				glTexImage1D(GL_TEXTURE_1D, 0, (int) GL_RGB, 32, 0, GL_RGB, GL_FLOAT, shaderData);
			}
			catch(Exception e) {
				// Handle Any Exceptions While Loading Shader, Exit App
				string errorMsg = "An Error Occurred While Loading Shader:\n\t" + @"..\..\data\NeHeLesson37\Shader.txt" + "\n" + "\n\nStack Trace:\n\t" + e.StackTrace + "\n";
				MessageBox.Show(errorMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				App.Terminate();
			}
			finally {
				// Close The File
				if(reader != null) {
					reader.Close();
				}
			}
		}
		#endregion ReadShader()

		#region RotateVector(Matrix M, Vector V, ref Vector D)
		/// <summary>
		/// Rotates a Vector using the supplied matrix.
		/// </summary>
		/// <param name="M">The matrix.</param>
		/// <param name="V">The input Vector.</param>
		/// <param name="D">The rotated Vector.</param>
		private void RotateVector(Matrix M, Vector V, ref Vector D) {					// Rotate A Vector Using The Supplied Matrix
			D.x = (M.data[0] * V.x) + (M.data[4] * V.y) + (M.data[8] * V.z);			// Rotate Around The X Axis
			D.y = (M.data[1] * V.x) + (M.data[5] * V.y) + (M.data[9] * V.z);			// Rotate Around The Y Axis
			D.z = (M.data[2] * V.x) + (M.data[6] * V.y) + (M.data[10] * V.z);			// Rotate Around The Z Axis
		}
		#endregion RotateVector(Matrix M, Vector V, ref Vector D)
	}
}