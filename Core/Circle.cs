using System;

using Microsoft.Xna.Framework;

namespace Azuxiren.MG.Core;

/// <summary>Represents a cicle using floating points</summary>
public record struct Circle
{
	/// <summary>Center of the circle</summary>
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