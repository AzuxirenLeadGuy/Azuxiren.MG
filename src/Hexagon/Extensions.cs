using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Azuxiren.MG.Hex
{
	/// <summary>
	/// Provides extension methods for Hex related stuff
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// The smallest rectangle that contains this hexagon. <br/>
		/// Note: The rectangle is returned without taking the rotaion into account.
		/// /// </summary>
		/// <returns>The outer boundary of the hexagon</returns>
		public static Rectangle OuterBound(this Hexagon hex)
		{
			int h = (int)MathF.Round(Hexagon.Root3 * hex.SideLength);
			return new(hex.Center.X - hex.SideLength, hex.Center.Y - h / 2, hex.SideLength * 2, h);
		}
		/// <summary>The circle that circumscribes this hexagon</summary>
		/// <returns>Circle circumscribing this hexagon</returns>
		public static Circle OuterCircle(this Hexagon hex) => new() { Center = hex.Center, Radius = hex.SideLength };
		/// <summary>The largest circle bounded by this hexagon</summary>
		/// <returns>The largest circle bounded by this hexagon</returns>
		public static Circle InnerCircle(this Hexagon hex) => new() { Center = hex.Center, Radius = (int)MathF.Round(hex.SideLength * (Hexagon.Root3 / 2)) };
		/// <summary>
		/// The Manhattan distance between two points, that is:<br/>
		/// The number of hops on the hexagonal grid required to reach 
		/// at one GridPoint from the other.
		/// </summary>
		/// <param name="a">Point in the grid</param>
		/// <param name="b">Point in the grid</param>
		/// <returns>Manhatten distance between the two points</returns>
		public static int ManhattanDistance(this GridPoint a, GridPoint b)
		{
			int dq = a.Q - b.Q;
			int dr = a.R - b.R;
			return (Math.Abs(dq) + Math.Abs(dr) + Math.Abs(dq + dr)) >> 1;
		}
		/// <summary>
		/// Enumerates over the GridPoints that are encountered
		/// when drawing a line between the given two points
		/// </summary>
		/// <param name="source">The start of the line</param>
		/// <param name="end">The end of the line</param>
		/// <returns>
		/// Enumeration of the GridPoints encountered in 
		/// the line drawn between the given two points
		/// </returns>
		public static IEnumerable<GridPoint> GetLine(this GridPoint source, GridPoint end)
		{
			int dist = ManhattanDistance(source, end);
			float f_dist = dist;
			for (int i = 0; i < dist; i++)
			{
				float t = i / f_dist;
				float q = Lerp(source.Q, end.Q, t);
				float r = Lerp(source.R, end.R, t);
				float s = Lerp(source.S, end.S, t);
				yield return GridPoint.Round(q, r, s);
			}
			static float Lerp(int a, int b, float f) => a + ((b - a) * f);
		}
		/// <summary>
		/// Selects one of the 6 neighbour of this point
		/// </summary>
		/// <param name="a">The point to find neighbour of</param>
		/// <param name="n">The type of neighbour</param>
		/// <returns>The neighbouring point</returns>
		public static GridPoint Neighbour(this GridPoint a, HexGridNeighbours n)
		{
			return n switch
			{
				HexGridNeighbours.Top => new(a.Q, a.R - 1),
				HexGridNeighbours.Bottom => new(a.Q, a.R + 1),
				HexGridNeighbours.TopLeft => new(a.Q - 1, a.R),
				HexGridNeighbours.TopRight => new(a.Q + 1, a.R - 1),
				HexGridNeighbours.BottomLeft => new(a.Q - 1, a.R + 1),
				HexGridNeighbours.BottomRight => new(a.Q + 1, a.R),
				HexGridNeighbours.DiagonalLeft => new(a.Q - 2, a.R + 1),
				HexGridNeighbours.DiagonalRight => new(a.Q + 2, a.R - 1),
				HexGridNeighbours.DiagonalTopLeft => new(a.Q - 1, a.R - 1),
				HexGridNeighbours.DiagonalTopRight => new(a.Q + 1, a.R - 2),
				HexGridNeighbours.DiagonalBottomLeft => new(a.Q - 1, a.R + 2),
				HexGridNeighbours.DiagonalBottomRight => new(a.Q + 1, a.R + 1),
				_ => a,
			};
		}
		/// <summary>
		/// Gets all points forming a single ring at a distance `radius` from the
		/// given point
		/// </summary>
		/// <param name="point">The point to consider distance from</param>
		/// <param name="radius">The distance of all points of the ring from the center</param>
		/// <returns>The enumeration of all points forming a ring at the given distance</returns>
		public static IEnumerable<GridPoint> GetSingleRing(this GridPoint point, int radius)
		{
			if (radius <= 0) yield return point;
			else
			{
				int i = radius;
				while (i-- > 0) point = point.Neighbour(HexGridNeighbours.Top);
				HexGridNeighbours[] pts = new HexGridNeighbours[]
				{
					HexGridNeighbours.BottomLeft,
					HexGridNeighbours.Bottom,
					HexGridNeighbours.BottomRight,
					HexGridNeighbours.TopRight,
					HexGridNeighbours.Top,
					HexGridNeighbours.TopLeft
				};
				for (int j = 0; j < 6; j++)
				{
					for (i = 0; i < radius; i++)
					{
						point = point.Neighbour(pts[j]);
						yield return point;
					}
				}
			}
		}
		/// <summary>
		/// Gets the collection of all points having a distance less than or equal to
		/// the given value of radius
		/// </summary>
		/// <param name="point">The point to consider values from</param>
		/// <param name="radius">The distance from the center to all points</param>
		/// <returns>
		/// enumeration of all points having a distance less than or equal to
		/// the given value of radius
		/// </returns>
		public static IEnumerable<GridPoint> GetSpiralRing(this GridPoint point, int radius)
		{
			for (int i = 0; i < radius; i++)
			{
				foreach (GridPoint pt in point.GetSingleRing(i))
					yield return pt;
			}
		}
		/// <summary>Reflect this point about the Q-axis along the origin</summary>
		/// <param name="point">The point to reflect</param>
		/// <returns>The reflection point for this point</returns>
		public static GridPoint ReflectAxisQ(this GridPoint point) => new(point.Q, point.S);
		/// <summary>Reflect this point about the R-axis along the origin</summary>
		/// <param name="point">The point to reflect</param>
		/// <returns>The reflection point for this point</returns>
		public static GridPoint ReflectAxisR(this GridPoint point) => new(point.S, point.R);
		/// <summary>Reflect this point about the S-axis along the origin</summary>
		/// <param name="point">The point to reflect</param>
		/// <returns>The reflection point for this point</returns>
		public static GridPoint ReflectAxisS(this GridPoint point) => new(point.R, point.Q);
		/// <summary>Evaluates the Vector2 for the given hexagonal GridPoint</summary>
		/// <param name="coordinate">The hexagonal GridPoint</param>
		/// <returns>The Vector2 representation for the hexagonal GridPoint</returns>
		public static Vector2 GetVector2(this GridPoint coordinate)
		{
			Vector2 qv = new(1, 0);						// +S  \
			Vector2 rv = new(-0.5f, -Hexagon.Root3By2); //      O--> +Q
			Vector2 sv = new(-0.5f, Hexagon.Root3By2);	// +R  /
			return (coordinate.Q * qv) + (coordinate.R * rv) + (coordinate.S * sv);
		}
		/// <summary> Traverses from a given 2D point going along the Hexagonal "Axis"</summary>
		/// <param name="coordinate">The coordinate to traverse along</param>
		/// <param name="origin">The point to start from</param>
		/// <param name="hex_width">The width of the individual hexagons</param>
		/// <returns>The 2D point after traversing in the given directions</returns>
		public static Vector2 Traverse(this GridPoint coordinate, Vector2 origin, float hex_width) => origin + (hex_width * coordinate.GetVector2());
	}
}