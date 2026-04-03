using System;
using System.Collections.Generic;

using Azuxiren.MG.Drawing;

using Microsoft.Xna.Framework;

namespace Azuxiren.MG.Hex;
/// <summary>
/// Provides extension methods for Hex related stuff
/// </summary>
public static class Extensions
{
	/// <summary>
	/// Gets a Grid of Vector2 that would store the
	/// Vector2 denoting the center of each hexagon of a Hexgrid.
	/// Should be useful for drawing a Hexgrid
	/// </summary>
	/// <param name="origin">The center of the hexagon at (0, 0, [0])</param>
	/// <param name="radius">The radius of the Hexgrid</param>
	/// <param name="width">The uniform width of each radius in the grid</param>
	/// <returns>A Hexgrid of Vector2 points that denote the center of each hexagon in the grid</returns>
	public static HexGrid<Vector2> HexCenters(Vector2 origin, byte radius, float width)
	{
		HexGrid<Vector2> points = new(Vector2.Zero, radius);
		foreach (var key in points)
			points[key.Key] = key.Key.Traverse(origin, width);
		return points;
	}
}