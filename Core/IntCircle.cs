using System;

using Microsoft.Xna.Framework;
namespace Azuxiren.MG.Core;
/// <summary>Represents a circle</summary>
public record struct IntCircle
{
	/// <summary>Center of this circle</summary>
	public Point Center;

	/// <summary>Radius of this circle</summary>
	public readonly required int Radius
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