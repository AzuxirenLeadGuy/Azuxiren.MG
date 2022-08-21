using System;
namespace Azuxiren.MG.Hex
{
	/// <summary>
	/// Represents a location in the HexGrid (Hexagonal grid).
	/// This format is known as axial coordinate
	/// </summary>
	public struct GridPoint : IEquatable<GridPoint>, IComparable<GridPoint>
	{
		/// <summary>Independant Axial coordinate values</summary>
		public int Q, R;
		/// <summary>Dependant Cubic coordinate value</summary>
		public int S => -Q - R;
		/// <summary> Constructor for a grid point</summary>
		/// <param name="q">The value for Q</param>
		/// <param name="r">The value for R</param>
		/// <returns>A gridpoint instance</returns>
		public GridPoint(int q, int r) => (Q, R) = (q, r);
		/// <summary>Checks for the equality compared to another HexGrid.Point</summary>
		/// <param name="other">The other position to compare</param>
		/// <returns>true if they are equal; false otherwise</returns>
		public bool Equals(GridPoint other) => Q == other.Q && R == other.R;
		/// <summary>Checks for the equality compared to another object</summary>
		/// <param name="obj">The object to compare</param>
		/// <returns>true if they are equal; false otherwise</returns>
		public override bool Equals(object obj) => obj is GridPoint pos && Equals(pos);
		/// <summary>Checks for the equality of two instances</summary>
		/// <param name="left">The left operand</param>
		/// <param name="right">The right operand</param>
		/// <returns>true if equal; false otherwise</returns>
		public static bool operator ==(GridPoint left, GridPoint right) => left.Equals(right);
		/// <summary>Checks for the unequality of two instances</summary>
		/// <param name="left">The left operand</param>
		/// <param name="right">The right operand</param>
		/// <returns>true if unequal; false otherwise</returns>
		public static bool operator !=(GridPoint left, GridPoint right) => !(left == right);
		/// <summary>Computes a suitable hash for this positional value</summary>
		/// <returns>Hash for this position</returns>
		public override int GetHashCode()
		{
			ushort q = (ushort)Q, r = (ushort)R;
			return (q << 16) + r;
		}
		/// <summary>Compares this point to another instance of a point</summary>
		/// <param name="other">The other point to compare with</param>
		/// <returns>
		/// negative value if this element precedes the other;
		/// 0 if this element is in the same order;
		/// positive value otherwise 
		/// </returns>
		public int CompareTo(GridPoint other) => Q != other.Q ? Q.CompareTo(other.Q) : R.CompareTo(other.R);
		/// <summary>Returns the string representation of the values of this hexagon grid position</summary>
		/// <returns>The string representation of the values of this hexagon grid position</returns>
		public override string ToString() => $"\"Hexgrid\": ({Q}, {R})";
		/// <summary>
		/// Rounds a floating point coordinate to the nearest valid HexGrid point
		/// </summary>
		/// <param name="fq">float value of Q</param>
		/// <param name="fr">float value of R</param>
		/// <param name="fs">float value of S</param>
		/// <returns></returns>
		public static GridPoint Round(float fq, float fr, float fs)
		{
			int q = (int)MathF.Round(fq);
			int r = (int)MathF.Round(fr);
			int s = (int)MathF.Round(fs);
			float dq = MathF.Abs(q - fq), dr = MathF.Abs(r - fr), ds = MathF.Abs(s - fs);
			if (dq > dr && dq > ds)
				q = -r - s;
			else if (dr > ds)
				r = -q - s;
			return new() { Q = q, R = r };
		}
	}
}