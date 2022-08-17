using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
namespace Azuxiren.MG
{
	/// <summary>Represents a circle</summary>
	public struct Circle : IEquatable<Circle>
	{
		/// <summary>Center of this circle</summary>
		public Point Center;
		private int _radius;
		/// <summary>Radius of this circle</summary>
		public int Radius
		{
			get => _radius;
			set => _radius = value >= 0 ? value : throw new ArgumentException("Radius cannot be negative", nameof(value));
		}
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
					if (success == false)
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
		/// <summary>The smallest rectangle which bounds/contains this circle</summary>
		/// <returns>The smallest rectangle bounding/containing this circle</returns>
		public Rectangle OuterBound() => new(Center.X - _radius, Center.Y - _radius, 2 * _radius, 2 * _radius);
		/// <summary>The largest rectangle bounded/contained by this circle</summary>
		/// <returns>The largest rectangle bounded/contained by this circle</returns>
		public Rectangle InnerBound()
		{
			int half_side = (int)MathF.Round(_radius * 0.707106781f);
			return new(Center.X - half_side, Center.Y - half_side, 2 * half_side, 2 * half_side);
		}
		/// <summary>Checks if two circles intersect.<br/>
		/// This is a commutative operation.
		/// i.e x.Intersect(y) is always equal to y.Intersect(x)
		/// for any two circles x, y <br/>
		/// If one circle is inside the other
		/// </summary>
		/// <param name="other">The other circle for which we wish to know the intersection</param>
		/// <returns>true if the circles intersect; false otherwise</returns>
		public bool Intersect(Circle other)
		{
			int dx = Center.X - other.Center.X;
			int dy = Center.Y - other.Center.Y;
			int sq_dist = (dx * dx) + (dy * dy);
			int sq_rad_sum = _radius + other._radius;
			sq_rad_sum *= sq_rad_sum;
			return sq_dist <= sq_rad_sum;
		}
		/// <summary>
		/// Checks if this circle contains the other circle. <br/><br/>
		/// This is not a commutative operation, 
		/// i.e x.Contains(y) is not always equal to y.Contains(x) <br/>
		/// for any two circles x, y
		/// </summary>
		/// <param name="other">The other circle to compare with</param>
		/// <returns>true if `other` circle is contained within `this` circle; false otherwise</returns>
		public bool Contains(Circle other)
		{
			int dx = Center.X - other.Center.X;
			int dy = Center.Y - other.Center.Y;
			int sq_dist = (dx * dx) + (dy * dy);
			int sq_rad_diff = _radius - other._radius;
			sq_rad_diff *= sq_rad_diff;
			return sq_dist <= sq_rad_diff;
		}
		/// <summary> Checks if two circles are equal or not</summary>
		/// <param name="other">The other circle to compare</param>
		/// <returns>true if both circles are equal; false otherwise</returns>
		public bool Equals(Circle other) => _radius == other._radius && Center == other.Center;
		/// <summary>
		/// Checks if this circle contains the other circle. <br/><br/>
		/// This is not a commutative operation, 
		/// i.e x.Contains(y) is not always equal to y.Contains(x) <br/>
		/// for any two circles x, y
		/// </summary>
		/// <param name="obj">The other object to compare with</param>
		/// <returns>true if both instances are equivalent; false otherwise</returns>
		public override bool Equals(object obj) => obj is Circle circle && Equals(circle);
		/// <summary> Returns the radius value as the hash </summary>
		/// <returns>radius as hash</returns>
		public override int GetHashCode() => _radius + Center.X + Center.Y;
		/// <summary>
		/// Prints the value of this object
		/// </summary>
		/// <returns>string representation of the values contained by this instance</returns>
		public override string ToString() => "Circle: { " + $"Center: {(Center.X, Center.Y)}, Radius: {_radius}" + " }";
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
}