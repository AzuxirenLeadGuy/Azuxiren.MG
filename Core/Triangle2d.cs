using Microsoft.Xna.Framework;

namespace Azuxiren.MG.Core;

/// <summary>Represents a 2D Triangle</summary>
public record struct Triangle2d
{
	private Point _endA, _endB, _endC;

	/// <summary>An endpoint of the triangle</summary>
	public readonly Point VertexA => _endA;

	/// <summary>An endpoint of the triangle</summary>
	public readonly Point VertexB => _endB;

	/// <summary>An endpoint of the triangle</summary>
	public readonly Point VertexC => _endC;

	/// <summary>An endpoint of the triangle</summary>
	public required (Point, Point, Point) Endpoints
	{
		readonly get => (_endA, _endB, _endC);
		set => (_endA, _endB, _endC) = CoreExtensions.Collinear(
			value.Item1, value.Item2, value.Item3
		) ? value : throw IntTriangle2d.ColinearExp();
	}
}