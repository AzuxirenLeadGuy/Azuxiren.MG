using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Azuxiren.MG.Drawing;

/// <summary>A general type to make regular polyon and generate it as textures</summary>
public struct IntPolygon
{
	private float _angle;

	/// <summary>The angle about the center of the polygon</summary>
	public float Angle { readonly get => _angle; set => _angle = DrawingExtensions.AngleMod(value); }

	/// <summary>Represents the center of the polygon</summary>
	public Point Center;

	/// <summary>The integer distance of the endpoints from its center</summary>
	public uint Radius;

	/// <summary>The number of sides for this polygon</summary>
	public readonly byte SideCount;

	/// <summary>Initializes a polygon</summary>
	/// <param name="sideCount">The number of sides in the polygon</param>
	/// <param name="center">The center point of the polygon</param>
	/// <param name="radius">The distance from the center to any of the equidistant vertex</param>
	/// <param name="angle">The angle between the zeroth vertex, center and the X-axis</param>
	public IntPolygon(byte sideCount, Point center, uint radius, float angle = 0)
	{
		if (sideCount < 3) throw new ArgumentException(
			"Expected sidecount to be at least 3",
			paramName: nameof(sideCount)
		);
		this.SideCount = sideCount;
		Center = center;
		Radius = radius;
		_angle = angle;
	}

	/// <summary>A circle that passes through all endpoints of the polygon</summary>
	/// <returns>Circle instance</returns>
	public readonly IntCircle Circumcircle => new()
	{
		Center = Center,
		Radius = (int)Radius,
	};

	/// <summary>The largest circle that is contained within the polygon</summary>
	/// <returns>Circle instance</returns>
	public readonly IntCircle Incircle => new()
	{
		Center = Center,
		Radius = (int)float.Round(Radius * float.CosPi(1F / SideCount)),
	};

	/// <summary>A rectangle that covers the polygon</summary>
	/// <returns></returns>
	public readonly Rectangle Bounds => new(
		Center.X - (int)Radius,
		Center.Y - (int)Radius,
		2 * (int)Radius,
		2 * (int)Radius
	);

	/// <summary>Return the integer points of all the endpoints of the vector</summary>
	/// <returns>Enumeration of all interger points of the polygon</returns>
	public readonly IEnumerable<Point> Endpoints()
	{
		float tAngle = 2 * float.Pi / SideCount;
		Vector2 startVec = new(0, Radius);
		for (int idx = 0; idx < SideCount; idx++)
		{
			yield return Vector2.Round(
				Vector2.Rotate
				(
					startVec,
					_angle + (idx * tAngle)
				)
			).ToPoint() + Center;
		}
	}
}