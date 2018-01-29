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
 *  Vector3D implementation
 *
 *  Created by Robert Schaap <robert@vulcanus.its.tudelft.nl>
 *  http://vulcanus.its.tudelft.nl/robert
 */
#endregion Original Credits / License

namespace SchaapExamples {
	/// <summary>
	/// Represents a location in 3D space, contains all the standard 
	/// vector operations for a 3-dimensional vector.
	/// </summary>
	public struct Vector3D {
		// --- Fields ---
		#region Public Fields
		/// <summary>
		/// The X component of the vector.
		/// </summary>
		public float X;

		/// <summary>
		/// The Y component of the vector.
		/// </summary>
		public float Y;
		
		/// <summary>
		/// The Z component of the vector.
		/// </summary>
		public float Z;
		#endregion Public Fields

		// --- Methods ---
		#region Constructors
		/// <summary>
		/// Creates a new vector set to (x, y, z).
		/// </summary>
		/// <param name="x">The X coordinate.</param>
		/// <param name="y">The Y coordinate.</param>
		/// <param name="z">The Z coordinate.</param>
		public Vector3D(float x, float y, float z) {
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		/// <summary>
		/// Creates a new vector set to the values of the given vector.
		/// </summary>
		/// <param name="vector">The vector whose values we'll use.</param>
		public Vector3D(Vector3D vector) {
			this.X = vector.X;
			this.Y = vector.Y;
			this.Z = vector.Z;
		}
		#endregion Constructors

		// --- Operator Overloads ---
		#region Vector3D operator +(Vector3D a, Vector3D b)
		/// <summary>
		/// Adds two vectors.  Result is (a.X + b.X, a.Y + b.Y, a.Z + b.Z).
		/// </summary>
		/// <param name="a">First vector.</param>
		/// <param name="b">Second vector.</param>
		/// <returns></returns>
		public static Vector3D operator +(Vector3D a, Vector3D b) {
			return new Vector3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
		}
		#endregion Vector3D operator +(Vector3D a, Vector3D b)

		#region Vector3D operator *(Vector3D vector, float scalar)
		/// <summary>
		/// Multiply vector by a scalar.  Result is (vector.X * scalar, vector.Y * scalar, vector.Z * scalar).
		/// </summary>
		/// <param name="vector">The vector.</param>
		/// <param name="scalar">The scalar</param>
		/// <returns></returns>
		public static Vector3D operator *(Vector3D vector, float scalar) {
			return new Vector3D(vector.X * scalar, vector.Y * scalar, vector.Z * scalar);
		}
		#endregion Vector3D operator *(Vector3D vector, float scalar)

		#region Vector3D operator -(Vector3D a, Vector3D b)
		/// <summary>
		/// Subtract two vectors.  Result is (a.X - b.X, a.Y - b.Y, a.Z - b.Z).
		/// </summary>
		/// <param name="a">The first vector.</param>
		/// <param name="b">The second vector.</param>
		/// <returns></returns>
		public static Vector3D operator -(Vector3D a, Vector3D b) {
			return new Vector3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
		}
		#endregion Vector3D operator -(Vector3D a, Vector3D b)
	}
}