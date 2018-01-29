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
	public sealed class CoolEffect2 : ParticleEngine {
		// --- Fields ---
		#region Private Fields
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
		public CoolEffect2(int _numParticles, Vector3D _origin, float _width, float _depth, float _range, uint _textureID) {
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
		#region ResetParticle(int i)
		/// <summary>
		/// Resets a particle.
		/// </summary>
		/// <param name="i">The index of the particle to reset.</param>
		public override void ResetParticle(int i) {
			// Put particle in its initial position
			particles[i].Position = new Vector3D(width * (((rand.Next() % 1000) / 1000f) - 0.5f), 0, depth * (((rand.Next() % 1000) / 1000f) - 0.5f)) + origin;

			// Create a velocity (down and to the left/right)
			particles[i].Velocity = new Vector3D((((rand.Next() % 1000) / 1000f) * 0.001f) - 0.0005f, ((((rand.Next() % 1000) / 1000f) * 0.00050f) - 0.00025f), (((rand.Next() % 1000) / 1000f) * 0.001f) - 0.0005f);

			particles[i].R = ((rand.Next() % 1000) / 1000f);
			particles[i].G = ((rand.Next() % 1000) / 1000f);
			particles[i].B = ((rand.Next() % 1000) / 1000f);
		}
		#endregion ResetParticle(int i)

		#region Update(long timepassed)
		/// <summary>
		/// Updates particle engine.
		/// </summary>
		/// <param name="timepassed">Elapsed time.</param>
		public override void Update(long timepassed) {
			for(int i = 0; i < numParticles; i++) {
				particles[i].Position = particles[i].Position + (particles[i].Velocity * (float) timepassed);
				particles[i].Velocity = particles[i].Velocity + particles[i].Acceleration;

				if(! (particles[i].Position.X > x_min && particles[i].Position.X < x_max && particles[i].Position.Y > y_min &&
					particles[i].Position.Y < y_max && particles[i].Position.Z > z_min && particles[i].Position.Z < z_max) ) {
					ResetParticle(i);
				}
			}
		}
		#endregion Update(long timepassed)

		#region Render()
		/// <summary>
		/// Renders particles to the screen.
		/// </summary>
		public override void Render() {
			float[] mat = new float[16];
			float size = 1.5f;
			Vector3D temp;

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

			GL.glBegin(GL.GL_TRIANGLE_STRIP);											// Use Triangle Strips (Faster/Better Supported)
			for(long i = 0; i < numParticles; i++) {									// Draw Billboarded Particles
					GL.glColor4f(particles[i].R, particles[i].G, particles[i].B, 0.5f);
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
			}
			GL.glEnd();																	// Finished Drawing Triangle Strips
		}
		#endregion Render()
	}
}