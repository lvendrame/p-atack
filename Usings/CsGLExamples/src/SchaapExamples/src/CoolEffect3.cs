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
 *  CoolEffect implementation
 *
 *  Created by Robert Schaap <robert@vulcanus.its.tudelft.nl>
 *  http://vulcanus.its.tudelft.nl/robert
 */
#endregion Original Credits / License

using CsGL.OpenGL;
using System;

namespace SchaapExamples {
	/// <summary>
	/// Creates a cool effect.
	/// </summary>
	public sealed class CoolEffect3 : ParticleEngine {
		// --- Fields ---
		#region Private Fields
		private const int MAXAGE = 2000;												// Maximum Age In Milliseconds
		private const float MAXREVIVE = 0.1f;											// Number Of Particles Allowed To Be Revived Per Millisecond
		private static uint textureID;													// Texture ID
		private static float width, depth;												// Origin Dimensions
		private static float x_min, x_max, y_min, y_max, z_min, z_max;					// Particle System Borders
		private static Random rand = new Random();										// Randomizer
		#endregion Private Fields

		// --- Creation And Destruction Methods ---
		#region Constructor
		/// <summary>
		/// Creates a cool effect.
		/// </summary>
		/// <param name="_numParticles">Number of particles used in the engine.</param>
		/// <param name="_origin">The point (Vector3D) where the particles are born.</param>
		/// <param name="_width">Defines the width of the plane (or line) around the origin where particles can be initially placed.</param>
		/// <param name="_depth">Defines (with width) plane (or line) around the origin where particles can be initially placed.</param>
		/// <param name="_range">Used to define where the system ends, particles just die when they cross the system border.</param>
		/// <param name="_textureID">The texture ID used to map a texture onto the particle.</param>
		public CoolEffect3(int _numParticles, Vector3D _origin, float _width, float _depth, float _range, uint _textureID) {
			numParticles = _numParticles;
			origin = _origin;

			// Calculate borders
			x_min = origin.X - _width - _range;
			x_max = origin.X + _width + _range;
			y_min = origin.Y - _range;
			y_max = origin.Y + _range;
			z_min = origin.Z - _depth - _range;
			z_max = origin.Z + _depth + _range;

			width = _width;
			depth = _depth;
			textureID = _textureID;

			particles = new Particle[numParticles];
		}
		#endregion Constructor

		// --- Public Methods ---
		#region MoveOrigin(Vector3D translation)
		/// <summary>
		/// Moves the origin.
		/// </summary>
		/// <param name="translation">The translation.</param>
		public void MoveOrigin(Vector3D translation) {
			origin = origin + translation;
		}
		#endregion MoveOrigin(Vector3D translation)

		#region ResetParticle(int i)
		/// <summary>
		/// Resets a particle.
		/// </summary>
		/// <param name="i">The index of the particle to reset.</param>
		public override void ResetParticle(int i) {
			// Put particle in its initial position
			particles[i].Position = new Vector3D(width * (((rand.Next() % 1000) / 1000f) - 0.5f), 0, depth * (((rand.Next() % 1000) / 1000f) - 0.5f)) + origin;

			particles[i].Velocity = new Vector3D((((rand.Next() % 1000) / 1000f) * 0.001f) - 0.0005f, (((rand.Next() % 1000) / 1000f) * 0.005f), (((rand.Next() % 1000) / 1000f) * 0.001f) - 0.0005f);

			particles[i].R = 0.5f;
			particles[i].G = 0.3f;
			particles[i].B = 0.2f;

			particles[i].Alive = true;
			particles[i].Age = 0;
		}
		#endregion ResetParticle(int i)

		#region Update(long timepassed)
		/// <summary>
		/// Updates particle engine.
		/// </summary>
		/// <param name="timepassed">Elapsed time.</param>
		public override void Update(long timepassed) {
			int particlesRevived = 0;

			for(int i = 0; i < numParticles; i++) {
				if(!particles[i].Alive && particlesRevived < (MAXREVIVE * timepassed)) {
					ResetParticle(i);
					particlesRevived++;
				}

				if(particles[i].Age > MAXAGE) {
					particles[i].Alive = false;
				}

				particles[i].Position = particles[i].Position + (particles[i].Velocity * (float) timepassed);
				particles[i].Velocity = particles[i].Velocity + particles[i].Acceleration;

				particles[i].Age += timepassed;
			}
		}
		#endregion Update(long timepassed)

		#region Render()
		/// <summary>
		/// Renders particles to the screen.
		/// </summary>
		public override void Render() {
			float[] mat = new float[16];
			float size = 1.0f;
			Vector3D temp;
			float colorFade;

			// Billboarding
			GL.glGetFloatv(GL.GL_MODELVIEW_MATRIX, mat);								// Get Rotation Matrix

			Vector3D x = new Vector3D(mat[0], mat[4], mat[8]);							// Get X Rotation
			Vector3D y = new Vector3D(mat[1], mat[5], mat[9]);							// Get Y Rotation

			// Calculate Corner Points Of Polygon
			Vector3D topLeft = new Vector3D((new Vector3D() - x + y) * size);			// Upper left
			Vector3D bottomLeft = new Vector3D((new Vector3D() - x - y) * size);		// Lower left
			Vector3D topRight = new Vector3D((x + y) * size);							// Upper right
			Vector3D bottomRight = new Vector3D((x - y) * size);						// Lower right

			GL.glBindTexture(GL.GL_TEXTURE_2D, textureID);								// Select Texture
			GL.glEnable(GL.GL_BLEND);

			for(long i = 0; i < numParticles; i++) {									// Draw Billboarded Particles
				if(particles[i].Alive) {
					colorFade = particles[i].Age / (float) MAXAGE;

					GL.glBegin(GL.GL_TRIANGLE_STRIP);
						GL.glColor4f(particles[i].R - colorFade, particles[i].G - colorFade, particles[i].B - colorFade, 0.5f);
						Vector3D partCenter = particles[i].Position;

						// Upper Left Corner
						temp = partCenter + topLeft;
						GL.glTexCoord2f(0, 1);
						GL.glVertex3f(temp.X, temp.Y, temp.Z);

						// Lower Left Corner
						temp = partCenter + bottomLeft;
						GL.glTexCoord2f(0, 0);
						GL.glVertex3f(temp.X, temp.Y, temp.Z);

						// Upper Right Corner
						temp = partCenter + topRight;
						GL.glTexCoord2f(1, 1);
						GL.glVertex3f(temp.X, temp.Y, temp.Z);

						// Lower Right Corner
						temp = partCenter + bottomRight;
						GL.glTexCoord2f(1, 0);
						GL.glVertex3f(temp.X, temp.Y, temp.Z);
					GL.glEnd();
				}
			}
			GL.glDisable(GL.GL_BLEND);
		}
		#endregion Render()

		#region MoveOrigin(Vector3D translation)
		/// <summary>
		/// Sets the origin.
		/// </summary>
		/// <param name="neworigin">The new origin.</param>
		public void SetOrigin(Vector3D neworigin) {
			origin = neworigin;
		}
		#endregion MoveOrigin(Vector3D neworigin)
	}
}