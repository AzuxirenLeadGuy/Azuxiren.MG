using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
namespace Azuxiren.MG.Hex;
/// <summary>
/// Represents a location in the HexGrid (Hexagonal grid).
/// This format is known as axial coordinate
/// </summary>
public struct GridPoint : IEquatable<GridPoint>, IComparable<GridPoint>
{
	/// <summary>Represents the neighbour directions
	/// from a single position in the grid.<br/>
	/// The hexagon are in odd-q (flat base) layout</summary>
	public enum HexGridNeighbours : byte
	{
		/// <summary>Invalid value</summary>
		None = 0,
		/// <summary>Hexagon at bottom of current</summary>
		Bottom = 0b1001000,
		/// <summary>Hexagon at top of current</summary>
		Top = 0b10000001,
		/// <summary>Hexagon at top left of current</summary>
		TopLeft = 0b10000011,
		/// <summary>Hexagon at top right of current</summary>
		TopRight = 0b10000101,
		/// <summary>Hexagon at bottom left of current</summary>
		BottomLeft = 0b10001010,
		/// <summary>Hexagon at bottom right of current</summary>
		BottomRight = 0b10001100,
		/// <summary>Hexagon at diagonal left of current</summary>
		DiagonalLeft = 0b11000010,
		/// <summary>Hexagon at diagonal top left of current</summary>
		DiagonalTopLeft = 0b11000011,
		/// <summary>Hexagon at diagonal right of current</summary>
		DiagonalRight = 0b11000100,
		/// <summary>Hexagon at diagonal top right of current</summary>
		DiagonalTopRight = 0b11000101,
		/// <summary>Hexagon at diagonal bottom left of current</summary>
		DiagonalBottomLeft = 0b11001010,
		/// <summary>Hexagon at diagonal bottom right of current</summary>
		DiagonalBottomRight = 0b11001100,
	}
	/// <summary>Independant Axial coordinate values</summary>
	public int Q, R;
	/// <summary>Dependant Cubic coordinate value</summary>
	public readonly int S => -Q - R;
	/// <summary> Constructor for a grid point</summary>
	/// <param name="q">The value for Q</param>
	/// <param name="r">The value for R</param>
	/// <returns>A gridpoint instance</returns>
	public GridPoint(int q, int r) => (Q, R) = (q, r);
	/// <summary>Checks for the equality compared to another HexGrid.Point</summary>
	/// <param name="other">The other position to compare</param>
	/// <returns>true if they are equal; false otherwise</returns>
	public readonly bool Equals(GridPoint other) => Q == other.Q && R == other.R;
	/// <summary>Checks for the equality compared to another object</summary>
	/// <param name="obj">The object to compare</param>
	/// <returns>true if they are equal; false otherwise</returns>
	public override readonly bool Equals(object? obj) => obj != null && obj is GridPoint pos && Equals(pos);
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
	public override readonly int GetHashCode()
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
	public readonly int CompareTo(GridPoint other) => Q != other.Q ? Q.CompareTo(other.Q) : R.CompareTo(other.R);
	/// <summary>Returns the string representation of the values of this hexagon grid position</summary>
	/// <returns>The string representation of the values of this hexagon grid position</returns>
	public override readonly string ToString() => $"\"Hexgrid\": ({Q}, {R})";
	/// <summary>Implicit conversion from tuple of integers to GridPoint</summary>
	/// <param name="x">Tuple of integers</param>
	public static implicit operator GridPoint((int, int) x) => new(x.Item1, x.Item2);
	/// <summary>Implicit conversion from tuple of sbytes to GridPoint</summary>
	/// <param name="x">Tuple of sbytes</param>
	public static implicit operator GridPoint((sbyte, sbyte) x) => new(x.Item1, x.Item2);
	/// <summary>Adds two gridpoints together</summary>
	/// <param name="a">LHS of addition</param>
	/// <param name="b">RHS of addition</param>
	/// <returns>The sum of the two GridPoint</returns>
	public static GridPoint operator +(GridPoint a, GridPoint b) => new(a.Q + b.Q, a.R + b.R);
	/// <summary>Negates a gridpoint</summary>
	/// <param name="a">The point to negate</param>
	/// <returns>The negation of the given point</returns>
	public static GridPoint operator -(GridPoint a) => new(-a.Q, -a.R);
	/// <summary>Subtracts two gridpoints together</summary>
	/// <param name="a">LHS of subtraction</param>
	/// <param name="b">RHS of subtraction</param>
	/// <returns>The difference of the two GridPoint</returns>
	public static GridPoint operator -(GridPoint a, GridPoint b) => a + (-b);
	/// <summary>Multiplies a Gridpoint with a given factor</summary>
	/// <param name="a">The gridpoint to multiply</param>
	/// <param name="f">The factor to multiply with</param>
	/// <returns>The multiplication with the given factor</returns>
	public static GridPoint operator *(GridPoint a, int f) => new(a.Q * f, a.R * f);
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
	/// <summary>
	/// The Manhattan distance between two points, that is:<br/>
	/// The number of hops on the hexagonal grid required to reach
	/// at one GridPoint from the other.
	/// </summary>
	/// <param name="b">Point in the grid</param>
	/// <returns>Manhatten distance between the two points</returns>
	public readonly int ManhattanDistanceTo(GridPoint b)
	{
		int dq = Q - b.Q;
		int dr = R - b.R;
		return (Math.Abs(dq) + Math.Abs(dr) + Math.Abs(dq + dr)) / 2;
	}
	/// <summary>
	/// Enumerates over the GridPoints that are encountered
	/// when drawing a line between the given two points
	/// </summary>
	/// <param name="end">The end of the line</param>
	/// <returns>
	/// Enumeration of the GridPoints encountered in
	/// the line drawn between the given two points
	/// </returns>
	public IEnumerable<GridPoint> GetLineTill(GridPoint end)
	{
		int dist = ManhattanDistanceTo(end);
		float f_dist = dist;
		for (int i = 0; i < dist; i++)
		{
			float t = i / f_dist;
			float q = Lerp(Q, end.Q, t);
			float r = Lerp(R, end.R, t);
			float s = Lerp(S, end.S, t);
			yield return GridPoint.Round(q, r, s);
		}
		static float Lerp(int a, int b, float f) => a + ((b - a) * f);
	}
	/// <summary>
	/// Selects one of the 6 neighbour of this point
	/// </summary>
	/// <param name="n">The type of neighbour</param>
	/// <returns>The neighbouring point</returns>
	public readonly GridPoint Neighbour(HexGridNeighbours n)
	{
		return n switch
		{
			HexGridNeighbours.Top => new(Q, R - 1),
			HexGridNeighbours.Bottom => new(Q, R + 1),
			HexGridNeighbours.TopLeft => new(Q - 1, R),
			HexGridNeighbours.TopRight => new(Q + 1, R - 1),
			HexGridNeighbours.BottomLeft => new(Q - 1, R + 1),
			HexGridNeighbours.BottomRight => new(Q + 1, R),
			HexGridNeighbours.DiagonalLeft => new(Q - 2, R + 1),
			HexGridNeighbours.DiagonalRight => new(Q + 2, R - 1),
			HexGridNeighbours.DiagonalTopLeft => new(Q - 1, R - 1),
			HexGridNeighbours.DiagonalTopRight => new(Q + 1, R - 2),
			HexGridNeighbours.DiagonalBottomLeft => new(Q - 1, R + 2),
			HexGridNeighbours.DiagonalBottomRight => new(Q + 1, R + 1),
			_ => this,
		};
	}
	/// <summary>
	/// Gets all points forming a single ring at a distance `radius` from the
	/// given point
	/// </summary>
	/// <param name="radius">The distance of all points of the ring from the center</param>
	/// <returns>The enumeration of all points forming a ring at the given distance</returns>
	public readonly IEnumerable<GridPoint> GetSingleRing(uint radius)
	{
		GridPoint point = this;
		if (radius <= 0) { yield return this; }
		else
		{
			uint idx_i = radius;
			while (idx_i-- > 0) point = point.Neighbour(HexGridNeighbours.Top);
			HexGridNeighbours[] pts =
			[
				HexGridNeighbours.BottomLeft,
				HexGridNeighbours.Bottom,
				HexGridNeighbours.BottomRight,
				HexGridNeighbours.TopRight,
				HexGridNeighbours.Top,
				HexGridNeighbours.TopLeft
			];
			for (int idx_j = 0; idx_j < 6; idx_j++)
			{
				for (idx_i = 0; idx_i < radius; idx_i++)
				{
					point = point.Neighbour(pts[idx_j]);
					yield return point;
				}
			}
		}
	}
	/// <summary>
	/// Gets the collection of all points having a distance less than or equal to
	/// the given value of radius
	/// </summary>
	/// <param name="radius">The distance from the center to all points</param>
	/// <returns>
	/// enumeration of all points having a distance less than or equal to
	/// the given value of radius
	/// </returns>
	public readonly IEnumerable<GridPoint> GetSpiralRing(uint radius)
	{
		for (uint i = 0; i < radius; i++)
		{
			foreach (GridPoint pt in GetSingleRing(i))
				yield return pt;
		}
	}

	/// <summary>Reflect this point about the Q-axis along the origin</summary>
	/// <returns>The reflection point for this point</returns>
	public readonly GridPoint ReflectAxisQ() => new(Q, S);
	/// <summary>Reflect this point about the R-axis along the origin</summary>
	/// <returns>The reflection point for this point</returns>
	public readonly GridPoint ReflectAxisR() => new(S, R);
	/// <summary>Reflect this point about the S-axis along the origin</summary>
	/// <returns>The reflection point for this point</returns>
	public readonly GridPoint ReflectAxisS() => new(R, Q);
	/// <summary>Evaluates the Vector2 for the given hexagonal GridPoint</summary>
	/// <returns>The Vector2 representation for the hexagonal GridPoint</returns>
	public readonly Vector2 GetVector2()
	{
		Vector2 qv = new(1, 0);                     // +S  \
		Vector2 rv = new(-0.5f, -Hexagon.Root3By2); //      O--> +Q
		Vector2 sv = new(-0.5f, Hexagon.Root3By2);  // +R  /
		return (Q * qv) + (R * rv) + (S * sv);
	}
	/// <summary> Traverses from a given 2D point going along the Hexagonal "Axis"</summary>
	/// <param name="origin">The point to start from</param>
	/// <param name="hex_width">The width of the individual hexagons</param>
	/// <returns>The 2D point after traversing in the given directions</returns>
	public readonly Vector2 Traverse(Vector2 origin, float hex_width) => origin + (hex_width * GetVector2());
}