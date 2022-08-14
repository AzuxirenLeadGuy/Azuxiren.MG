using Microsoft.Xna.Framework;

using System;
namespace Azuxiren.MG.Hexagon
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
		public float Angle { get => _angle; set => _angle = FloatMod(value, Pi); }
		/// <summary>Constant values for computation</summary>
		public const float Pi = MathF.PI, PiBy2 = Pi / 2, PiBy3 = Pi / 3, PiBy6 = Pi / 6, Root2 = 1.4142135623f, Root3 = 1.7320508f;
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
		public static Color[,] CreateFilledGrid(int sideLength)
		{
			throw new NotImplementedException();
		}
		public static Color[,] CreateBorderGrid(int sideLength)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// The smallest rectangle that contains this hexagon. <br/>
		/// Note: The rectangle is returned without taking the rotaion into account.
		/// </summary>
		/// <returns>The outer boundary of the hexagon</returns>
		public Rectangle OuterBound()
		{
			int h = (int)MathF.Round(Root3 * SideLength);
			return new(Center.X - SideLength, Center.Y - h / 2, SideLength * 2, h);
		}
		/// <summary>The circle that circumscribes this hexagon</summary>
		/// <returns>Circle circumscribing this hexagon</returns>
		public Circle OuterCircle() => new() { Center = Center, Radius = SideLength };
		/// <summary>Checks if two hexagon instances are equivalent to each other</summary>
		/// <param name="other">The other hexagon instance to compare with</param>
		/// <returns>true if both instances are equivalent; false otherwise</returns>
		public bool Equals(Hexagon other) => SideLength == other.SideLength && Center == other.Center && _angle == other._angle;
		/// <summary>Returns the SideLength as the hashcode</summary>
		/// <returns>SideLength as the hashcode</returns>
		public override int GetHashCode() => SideLength;
		/// <summary>Returns the string representation of the values of this hexagon</summary>
		/// <returns>The string representation of the values of this hexagon</returns>
		public override string ToString() => "Hexagon: { " + $"Centre: {(Center.X, Center.Y)}, SideLength = {SideLength}, Angle = {_angle}" + " }";
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
		public override bool Equals(object obj) => obj is Hexagon hex && Equals(hex);
	}
}