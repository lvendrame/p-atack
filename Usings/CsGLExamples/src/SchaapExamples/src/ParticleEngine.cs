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
 *  Particle Engine implementation
 *
 *  Created by Robert Schaap <robert@vulcanus.its.tudelft.nl>
 *  http://vulcanus.its.tudelft.nl/robert
 */
#endregion Original Credits / License

namespace SchaapExamples {
	/// <summary>
	/// Standard particle engine, used to create specific particle engines.  
	/// Not supposed to be instantiated as an actual particle system.
	/// </summary>
	public class ParticleEngine {
		// --- Fields ---
		#region Protected Fields
		protected Vector3D origin;														// Particle Engine Origin
		protected Vector3D[] shape = new Vector3D[4];									// For Particle Systems That Use Specific Shapes
		protected Particle[] particles;													// List Of Particles
		protected int numParticles;														// Number Of Particles Used In Particle List
		#endregion Protected Fields

		// --- Creation And Destruction Methods ---
		#region Constructor
		/// <summary>
		/// Empty constructor.
		/// </summary>
		public ParticleEngine() {
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="_numParticles">Number of particles.</param>
		/// <param name="_origin">Origin of particles.</param>
		public ParticleEngine(int _numParticles, Vector3D _origin) {
			numParticles = _numParticles;												// Set Number Of Particles
			origin = _origin;															// Set Origin Of Particles
		}
		#endregion Constructor

		// --- Methods ---
		#region Reset()
		/// <summary>
		/// Resets entire particle system.
		/// </summary>
		public virtual void Reset() {
			for(int i = 0; i < numParticles; i++) {
				particles[i].Alive = false;												// Kill The Particle
				ResetParticle(i);														// Reset Every Particle
			}
		}
		#endregion #region Reset()

		#region ResetParticle(int i)
		/// <summary>
		/// Resets specific particle.
		/// </summary>
		/// <param name="i">Index of particle to reset.</param>
		public virtual void ResetParticle(int i) {
		}
		#endregion ResetParticle(int i)

		#region Update(long timepassed)
		/// <summary>
		/// Updates particle system.
		/// </summary>
		/// <param name="timepassed">Elapsed time.</param>
		public virtual void Update(long timepassed) {
		}
		#endregion Update(long timepassed)

		#region Render()
		/// <summary>
		/// Render particles to the screen.
		/// </summary>
		public virtual void Render() {
		}
		#endregion Render()
	}
}