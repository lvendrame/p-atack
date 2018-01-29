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
 *  Particle
 *
 *  Created by Robert Schaap <robert@vulcanus.its.tudelft.nl>
 *  http://vulcanus.its.tudelft.nl/robert
 */
#endregion Original Credits / License

namespace SchaapExamples {
	/// <summary>
	/// Used to store particle specific data in.
	/// </summary>
	public struct Particle {
		// --- Fields ---
		#region Public Fields
		/// <summary>
		/// Particle's current position.
		/// </summary>
		public Vector3D Position;

		/// <summary>
		/// Particle's previous position.
		/// </summary>
		public Vector3D OldPosition;

		/// <summary>
		/// Particle's velocity.
		/// </summary>
		public Vector3D Velocity;

		/// <summary>
		/// Particle's acceleration.
		/// </summary>
		public Vector3D Acceleration;

		/// <summary>
		/// Particle's red color value.
		/// </summary>
		public float R;

		/// <summary>
		/// Particle's green color value.
		/// </summary>
		public float G;

		/// <summary>
		/// Particle's blue color value.
		/// </summary>
		public float B;

		/// <summary>
		/// Particle's alpha color value.
		/// </summary>
		public float A;

		/// <summary>
		/// Particle's age.
		/// </summary>
		public long Age;

		/// <summary>
		/// Particle's size.
		/// </summary>
		public float Size;

		/// <summary>
		/// Is particle alive?
		/// </summary>
		public bool Alive;
		#endregion Public Fields
	}
}