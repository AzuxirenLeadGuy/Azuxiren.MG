using System;

using Microsoft.Xna.Framework;

namespace Azuxiren.MG.Core;

/// <summary>A general type to make regular polyon and generate it as textures</summary>
public record struct IntPolygon
{
	private float _angle;
	/// <summary>The angle about the center of the polygon</summary>
	public float Angle { readonly get => _angle; set => _angle = CoreExtensions.AngleMod(value); }

	/// <summary>Represents the center of the polygon</summary>
	public Point Center;

	private readonly int _radius;

	/// <summary>The integer distance of the endpoints from its center</summary>
	public readonly required int Radius
	{
		init => _radius = value >= 0 ?
			value :
			throw new ArgumentException(
				"Cannot create a polygon with negative distance from its endpoints",
				nameof(value)
			);
		get => _radius;
	}

	private readonly byte _sides;

	/// <summary>The number of sides for this polygon</summary>
	public readonly required byte SideCount
	{
		init => _sides = value >= 3 ?
			value :
			throw new ArgumentException(
				"Cannot create a polygon with less than 3 sides",
				nameof(value)
			);
		get => _sides;
	}
}