using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
namespace Azuxiren.MG.Hex
{
	/// <summary> Represents a Regular Hexagon</summary>
	public struct Hexagon : IEquatable<Hexagon>
	{
		/// <summary>Center of the Hexagon</summary>
		public Point Center;
		/// <summary>Length of a side</summary>
		public int SideLength;
		private float _angle;
		/// <summary>The angle with which the hexagon is rotated about its center</summary>
		public float Angle { readonly get => _angle; set => _angle = FloatMod(value, Pi); }
		/// <summary>Constant values for computation</summary>
		public const float Pi = MathF.PI, Root3 = 1.7320508075688f, Root3By2 = 0.8660254037844f;
		private static float FloatMod(float givenAngle, float mod)
		{
			if (givenAngle > mod)
			{
				do { givenAngle -= mod; } while (givenAngle > mod);
			}
			else
			{
				while (givenAngle < 0) givenAngle += mod;
			}
			return givenAngle;
		}
		/// <summary>
		/// Returns a rectangular array of Color instances
		/// depicting a filled hexagon
		/// </summary>
		/// <param name="sideLength">Length of a single side of the regular hexagon</param>
		/// <param name="color">The color to draw the hexagon with</param>
		/// <returns>Color[,] array depicting a filled hexagon</returns>
		public static Color[,] CreateFilledGrid(int sideLength, Color color)
		{
			int width = 2 * sideLength, height = (int)MathF.Round(sideLength * Root3);
			if ((height & 1) == 1)
				height++;// Keeping height even
			Color[,] grid = new Color[width, height];
			int topleftX = sideLength / 2, i, j;
			foreach (Point p in Global.GetPointsOnLine(topleftX, 0, 0, (height + 1) / 2))
			{
				for (i = p.X, j = width - p.X - 1; i <= j; i++)
				{
					grid[i, p.Y] = color;
					grid[i, height - p.Y - 1] = color;
				}
			}
			return grid;
		}
		/// <summary>
		/// Returns a rectangular array of Color instances
		/// depicting a hexagon with colored border
		/// </summary>
		/// <param name="sideLength">Length of a single side of the regular hexagon</param>
		/// <param name="color">The color of border of hexagon</param>
		/// <returns>Color[,] array depicting a filled hexagon</returns>
		public static Color[,] CreateBorderGrid(int sideLength, Color color)
		{
			int width = 2 * sideLength, height = (int)MathF.Round(sideLength * Root3);
			if ((height & 1) == 1)
				height++;// Keeping height even
			Color[,] grid = new Color[width, height];
			int topleftX = sideLength / 2, thick = 1 + (sideLength / 32), i, j, k, y;
			foreach (Point p in Global.GetPointsOnLine(topleftX, 0, 0, (height + 1) / 2))
			{
				for (i = p.X, j = width - p.X - 1, k = 0; k < thick; i++, j--, k++)
				{
					grid[i, p.Y] = color;
					grid[j, p.Y] = color;
					y = height - p.Y - 1;
					grid[i, y] = color;
					grid[j, y] = color;
				}
			}
			for (j = width - 1 - topleftX, k = 0; k < thick; k++)
			{
				y = height - 1 - k;
				for (i = topleftX; i <= j; i++)
				{
					grid[i, k] = color;
					grid[i, y] = color;
				}
			}
			return grid;
		}
		/// <summary>Checks if two hexagon instances are equivalent to each other</summary>
		/// <param name="other">The other hexagon instance to compare with</param>
		/// <returns>true if both instances are equivalent; false otherwise</returns>
		public readonly bool Equals(Hexagon other) => SideLength == other.SideLength && Center == other.Center && _angle == other._angle;
		/// <summary>Returns the SideLength as the hashcode</summary>
		/// <returns>SideLength as the hashcode</returns>
		public override readonly int GetHashCode() => SideLength;
		/// <summary>Returns the string representation of the values of this hexagon</summary>
		/// <returns>The string representation of the values of this hexagon</returns>
		public override readonly string ToString() => "Hexagon: { " + $"Centre: {(Center.X, Center.Y)}, SideLength = {SideLength}, Angle = {_angle}" + " }";
		/// <summary> Checks if two hexagon instances are equivalent</summary>
		/// <param name="left">The left operand</param>
		/// <param name="right">The right operand</param>
		/// <returns>true if both instances are equivalent; false otherwise</returns>
		public static bool operator ==(Hexagon left, Hexagon right) => left.Equals(right);
		/// <summary> Checks if two hexagon instances are not equivalent</summary>
		/// <param name="left">The left operand</param>
		/// <param name="right">The right operand</param>
		/// <returns>true if both instances are not equivalent; false otherwise</returns>
		public static bool operator !=(Hexagon left, Hexagon right) => !(left == right);
		/// <summary>Checks if two hexagon instances are equivalent to each other</summary>
		/// <param name="obj">The other object to compare with</param>
		/// <returns>true if both instances are equivalent; false otherwise</returns>
		public override readonly bool Equals(object? obj) => obj!=null && obj is Hexagon hex && Equals(hex);
	}
}