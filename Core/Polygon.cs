using System;

using Microsoft.Xna.Framework;

namespace Azuxiren.MG.Core;

/// <summary>Represents a fixed sized regular polygon</summary>
public record struct Polygon
{
	private byte _sides;
	/// <summary>The number of sides, or number of vertices of the polygon</summary>
	public required byte SideCount
	{
		set => _sides = value >= 3 ?
			value :
			throw new ArgumentException(
				"Cannot create a polygon with less than 3 sides",
				nameof(value)
			);
		readonly get => _sides;
	}

	private float _angle;

	/// <summary>The angle of rotation from the center</summary>
	public float Angle
	{
		readonly get => _angle;
		set => _angle = CoreExtensions.AngleMod(value);
	}

	/// <summary>Center of the polygon</summary>
	public Vector2 Center;

	private float _radius;

	/// <summary>Radius of the circle</summary>
	public float Radius
	{
		readonly get => _radius;
		set => _radius = value >= 0 ?
			value :
			throw new ArgumentException(
				"Radius cannot be negative",
				nameof(value)
			);
	}
}