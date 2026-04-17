using System;

using Microsoft.Xna.Framework;

namespace Azuxiren.MG.Core;

/// <summary>A general type to make regular polyon and generate it as textures</summary>
public record struct IntPolygon
{
	/// <summary>The angle about the center of the polygon</summary>
	public float Angle { readonly get; set => field = CoreExtensions.AngleMod(value); }

	/// <summary>Represents the center of the polygon</summary>
	public Point Center;

	/// <summary>The integer distance of the endpoints from its center</summary>
	public readonly required int Radius
	{
		init => field = value >= 0 ?
			value :
			throw new ArgumentException(
				"Cannot create a polygon with negative distance from its endpoints",
				nameof(value)
			);
		get;
	}

	/// <summary>The number of sides for this polygon</summary>
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
}