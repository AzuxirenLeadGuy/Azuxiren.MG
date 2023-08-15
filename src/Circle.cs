using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
namespace Azuxiren.MG;
/// <summary>Represents a circle</summary>
public struct Circle : IEquatable<Circle>
{
	/// <summary>Center of this circle</summary>
	public Point Center;
	internal int ActualRadius;
	/// <summary>Radius of this circle</summary>
	public int Radius
	{
		readonly get => ActualRadius;
		set => ActualRadius = value >= 0 ? value : throw new ArgumentException("Radius cannot be negative", nameof(value));
	}
	/// <summary>The Y coordinate of the top of this circle instance</summary>
	public readonly int Top => Center.Y - ActualRadius;
	/// <summary>The Y coordinate of the bottom of this circle instance</summary>
	public readonly int Bottom => Center.Y + ActualRadius;
	/// <summary>The X coordinate of the left of this circle instance</summary>
	public readonly int Left => Center.X - ActualRadius;
	/// <summary>The X coordinate of the right of this circle instance</summary>
	public readonly int Right => Center.X + ActualRadius;
	/// <summary>
	/// Draws a filled circle within the rectangular array of colors
	/// </summary>
	/// <param name="radius">The radius(r) of the circle.
	/// Note that the image dimension would be 2r x 2r</param>
	/// <param name="color">The color to fill the circle with</param>
	/// <returns>Color[,] rectangular array depicting a drawn circle</returns>
	public static Color[,] CreateFilledGrid(int radius, Color color)
	{
		int side = 1 + ((--radius) * 2), x;
		Color[,] grid = new Color[side, side];
		IEnumerator<Point> itr = Global.GetPointsOnCircle(radius, radius, radius).GetEnumerator();
		Point p, q;
		bool success = true;
		do
		{
			for (x = 0; x < 4; x++)
			{
				success = itr.MoveNext();
				p = itr.Current;
				success = success && itr.MoveNext();
				q = itr.Current;
				if (!success)
					break;
				for (int i = p.X, j = p.Y; j >= q.Y; j--)
				{
					grid[i, j] = color;
				}
			}
		} while (success);
		return grid;
	}
	/// <summary>
	/// Draws a circle border with given radius on the rectangular array of colors
	/// </summary>
	/// <param name="radius">The radius(r) of the circle.
	/// Note that the image dimension would be 2r x 2r</param>
	/// <param name="color">The color to fill the circle with</param>
	/// <returns>Color[,] rectangular array depicting a drawn circle border</returns>
	public static Color[,] CreateBorderGrid(int radius, Color color)
	{
		int side = 1 + ((--radius) * 2), thick = 1 + (radius / 32), i, r = radius;
		Color[,] grid = new Color[side, side];
		for (i = 0; i < thick; i++)
		{
			foreach (Point p in Global.GetPointsOnCircle(radius, radius, r--))
			{
				grid[p.X, p.Y] = color;
			}
		}
		return grid;
	}
	/// <summary> Checks if two circles are equal or not</summary>
	/// <param name="other">The other circle to compare</param>
	/// <returns>true if both circles are equal; false otherwise</returns>
	public readonly bool Equals(Circle other) => ActualRadius == other.ActualRadius && Center == other.Center;
	/// <summary>
	/// Checks if this circle contains the other circle. <br/><br/>
	/// This is not a commutative operation,
	/// i.e x.Contains(y) is not always equal to y.Contains(x) <br/>
	/// for any two circles x, y
	/// </summary>
	/// <param name="obj">The other object to compare with</param>
	/// <returns>true if both instances are equivalent; false otherwise</returns>
	public override readonly bool Equals(object? obj) => obj != null && obj is Circle circle && Equals(circle);
	/// <summary> Returns the radius value as the hash </summary>
	/// <returns>radius as hash</returns>
	public override readonly int GetHashCode() => ActualRadius + Center.X + Center.Y;
	/// <summary>
	/// Prints the value of this object
	/// </summary>
	/// <returns>string representation of the values contained by this instance</returns>
	public override readonly string ToString() => "Circle: { " + $"Center: {(Center.X, Center.Y)}, Radius: {ActualRadius}" + " }";
	/// <summary>
	/// Checks if two circles are equivalent to each other
	/// </summary>
	/// <param name="left">Left operand</param>
	/// <param name="right">Right operand</param>
	/// <returns>true if both Circles are equivalent to each other; false otherwise</returns>
	public static bool operator ==(Circle left, Circle right) => left.Equals(right);
	/// <summary>
	/// Checks if two circles are not equivalent to each other
	/// </summary>
	/// <param name="left">Left operand</param>
	/// <param name="right">Right operand</param>
	/// <returns>true if both Circles are not equivalent to each other; false otherwise</returns>
	public static bool operator !=(Circle left, Circle right) => !(left == right);
}