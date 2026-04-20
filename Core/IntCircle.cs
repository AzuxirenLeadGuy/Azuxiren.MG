using System;

using Microsoft.Xna.Framework;
namespace Azuxiren.MG.Core;
/// <summary>Represents a circle</summary>
public record struct IntCircle
{
	/// <summary>Center of this circle</summary>
	public Point Center;

	private int _radius;
	/// <summary>Radius of this circle</summary>
	public int Radius
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