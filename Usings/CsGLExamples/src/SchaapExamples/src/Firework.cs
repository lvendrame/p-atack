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
 *  Firework implementation
 *
 *  Created by Robert Schaap <robert@vulcanus.its.tudelft.nl>
 *  http://vulcanus.its.tudelft.nl/robert
 */
#endregion Original Credits / License

using CsGL.OpenGL;
using System;

namespace SchaapExamples {
	/// <summary>
	/// Creates a firework explosion.
	/// </summary>
	public sealed class Firework : ParticleEngine {
		// --- Fields ---
		#region Private Fields
		private uint textureID;															// Texture ID
		private float width, depth;														// Origin Dimensions
		private float range, ground;
		private long bodycount;															// Number Of Dead Particles
		private long maxAge;															// Maximum Age Of A Particle
		private Vector3D acceleration;													// Particle Acceleration, Every Particle Influenced By Gravity
		private float red, green, blue;
		private static Random rand = new Random();										// Randomizer
		private const float CENTERR = 1.0f;
		private const float CENTERG = 1.0f;
		private const float CENTERB = 1.0f;
		private const float CENTERA = 0.8f;
		private const float CORNERR = 0.96f;
		private const float CORNERG = 0.52f;
		private const float CORNERB = 0.0f;
		private const float CORNERA = 0.5f;
		private const float PARTSIZEMULT = 0.8f;
		#endregion Private Fields

		#region Public Fields
		public bool Done;
		#endregion Public Fields

		// --- Creation And Destruction Methods ---
		#region Constructors
		/// <summary>
		/// Empty constructor.
		/// </summary>
		public Firework() {
			Done = true;
			if(width != 0 || depth != 0) {
				width = 0;
				depth = 0;
			}
		}

		/// <summary>
		/// Creates a firework.
		/// </summary>
		/// <param name="_numParticles">Number of particles used in the engine.</param>
		/// <param name="_origin">The point (Vector3D) where the particles are born.</param>
		/// <param name="_range">Used to define where the system ends, particles just die when they cross the system border.</param>
		/// <param name="_textureID">The texture ID used to map a texture onto the particle.</param>
		public Firework(int _numParticles, Vector3D _origin, float _range, float _ground, uint _textureID) {
			bodycount = 0;
			maxAge = 500;
			acceleration = new Vector3D(0, -0.0005f, 0);

			numParticles = _numParticles;
			origin = _origin;

			range = _range;
			ground = _ground;

			textureID = _textureID;

			particles = new Particle[numParticles];

			Done = false;

			red = ((rand.Next() % 1000) / 1000f);
			green = ((rand.Next() % 1000) / 1000f);
			blue = ((rand.Next() % 1000) / 1000f);
		}
		#endregion Constructors

		// --- Public Methods ---
		#region ResetParticle(int i)
		/// <summary>
		/// Resets particle.
		/// </summary>
		/// <param name="i">Index of particle to reset.</param>
		public override void ResetParticle(int i) {
			// Put particle in its initial position
			particles[i].Position = new Vector3D(range * (((rand.Next() % 1000) / 1000f) - 0.5f), range * (((rand.Next() % 1000) / 1000f) - 0.5f), range * (((rand.Next() % 1000) / 1000f) - 0.5f)) + origin;

			// Create a velocity (down and to the left/right)
			particles[i].Velocity = new Vector3D((((rand.Next() % 1000) / 1000f) * 0.04f) - 0.02f, ((((rand.Next() % 1000) / 1000f) * 0.04f) - 0.02f), (((rand.Next() % 1000) / 1000f) * 0.04f) - 0.02f);

			particles[i].R = red + (((rand.Next() % 1000) / 1000f) * 0.02f) - 0.01f;
			particles[i].G = green + (((rand.Next() % 1000) / 1000f) * 0.02f) - 0.01f;
			particles[i].B = blue + (((rand.Next() % 1000) / 1000f) * 0.02f) - 0.01f;

			particles[i].Age = maxAge;
			particles[i].Alive = true;
		}
		#endregion ResetParticle(int i)

		#region Reset()
		/// <summary>
		/// Reset entire particle system.
		/// </summary>
 		public override void Reset() {
			bodycount = 0;
			Done = false;

			for(int i = 0; i < numParticles; i++) {
				ResetParticle(i);														// Reset Every Particle
			}
		}
		#endregion Reset()

		#region Update(long timepassed)
		/// <summary>
		/// Updates particle engine.
		/// </summary>
		/// <param name="timepassed">Elapsed time.</param>
		public override void Update(long timepassed) {
			if(bodycount >= numParticles) {
				Done = true;
			}

			for(int i = 0; i < numParticles; i++) {
				if(particles[i].Position.Y <= ground && particles[i].Alive) {
					// Make Particles Stop When On The Ground
					if((particles[i].Velocity.Y >= -0.01f) && (particles[i].Velocity.Y <= 0.01f)) {
						particles[i].Velocity.X = 0;
						particles[i].Velocity.Z = 0;
					}

					// Particles May Only Go When They Are Black
					if((particles[i].R == 0) && (particles[i].G == 0) && (particles[i].B == 0)) {
						bodycount++;
						particles[i].Alive = false;
					}
					else {
						// Update Color When On Ground
						particles[i].Velocity.Y = -(particles[i].Velocity.Y * 0.4f);
						particles[i].R -= 0.01f;
						if(particles[i].R < 0) {
							particles[i].R = 0;
						}
						
						particles[i].G -= 0.01f;
						if(particles[i].G < 0) {
							particles[i].G = 0;
						}

						particles[i].B -= 0.01f;
						if(particles[i].B < 0) {
							particles[i].B = 0;
						}
					}
				}

				// Update Color When Near Ground
				if(particles[i].Position.Y <= ground + 15) {
					particles[i].R -= 0.01f;
					if(particles[i].R < 0) {
						particles[i].R = 0;
					}

					particles[i].G -= 0.01f;
					if(particles[i].G < 0) {
						particles[i].G = 0;
					}

					particles[i].B -= 0.01f;
					if(particles[i].B < 0) {
						particles[i].B = 0;
					}
				}

				if(particles[i].Alive) {
					particles[i].Position = particles[i].Position + (particles[i].Velocity * timepassed);
					particles[i].Velocity = particles[i].Velocity + acceleration;
					particles[i].Age--;
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
			float size = 0.75f;
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

			for(int i = 0; i < numParticles; i++) {										// Draw Billboarded Particles
				GL.glBegin(GL.GL_TRIANGLE_FAN);											// Use Triangle Fan
					GL.glColor4f(particles[i].R, particles[i].G, particles[i].B, 0.5f);
					Vector3D partCenter = particles[i].Position;

					// Center
					GL.glColor4f(CENTERR * particles[i].R, CENTERG * particles[i].G, CENTERB * particles[i].B, CENTERA);
					GL.glTexCoord2f(0.5f, 0.5f);
					GL.glVertex3f(partCenter.X, partCenter.Y, partCenter.Z);

					// Upper Left Corner
					GL.glColor4f(CENTERR * particles[i].R, CORNERG * particles[i].G, CORNERB * particles[i].B, CORNERA);
					temp = partCenter + topLeft * PARTSIZEMULT;
					GL.glTexCoord2f(0, 1);
					GL.glVertex3f(temp.X, temp.Y, temp.Z);

					// Lower Left Corner
					GL.glColor4f(CENTERR * particles[i].R, CORNERG * particles[i].G, CORNERB * particles[i].B, CORNERA);
					temp = partCenter + bottomLeft * PARTSIZEMULT;
					GL.glTexCoord2f(0, 0);
					GL.glVertex3f(temp.X, temp.Y, temp.Z);

					// Lower Right Corner
					GL.glColor4f(CENTERR * particles[i].R, CORNERG * particles[i].G, CORNERB * particles[i].B, CORNERA);
					temp = partCenter + bottomRight * PARTSIZEMULT;
					GL.glTexCoord2f(1, 0);
					GL.glVertex3f(temp.X, temp.Y, temp.Z);

					// Upper Right Corner
					GL.glColor4f(CENTERR * particles[i].R, CORNERG * particles[i].G, CORNERB * particles[i].B, CORNERA);
					temp = partCenter + topRight * PARTSIZEMULT;
					GL.glTexCoord2f(1, 1);
					GL.glVertex3f(temp.X, temp.Y, temp.Z);

					// Upper Left Corner
					GL.glColor4f(CENTERR * particles[i].R, CORNERG * particles[i].G, CORNERB * particles[i].B, CORNERA);
					temp = partCenter + topLeft * PARTSIZEMULT;
					GL.glTexCoord2f(0, 1);
					GL.glVertex3f(temp.X, temp.Y, temp.Z);
				GL.glEnd();																// Finished Drawing Triangle Fans
			}
		}
		#endregion Render()
	}
}