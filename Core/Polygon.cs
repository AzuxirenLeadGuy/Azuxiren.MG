using System;

using Microsoft.Xna.Framework;

namespace Azuxiren.MG.Core;

/// <summary>Represents a fixed sized regular polygon</summary>
public record struct Polygon
{
	/// <summary>The number of sides, or number of vertices of the polygon</summary>
	public readonly required byte SideCount
	{
		init => field = value >= 3 ?
			value :
			throw new ArgumentException(
				"Cannot create a polygon with less than 3 sides",
				nameof(value)
			);
		get;
	}

	/// <summary>The angle of rotation from the center</summary>
	public float Angle { get; set => field = CoreExtensions.AngleMod(value); }

	/// <summary>Center of the polygon</summary>
	public Vector2 Center;

	/// <summary>Radius of the circle</summary>
	public readonly float Radius
	{
		get;
		init => field = value >= 0 ?
			value :
			throw new ArgumentException(
				"Radius cannot be negative",
				nameof(value)
			);
	}
}