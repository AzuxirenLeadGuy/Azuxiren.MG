using Microsoft.Xna.Framework;

namespace Azuxiren.MG.Core;

/// <summary>Represents a 2D Triangle</summary>
public record struct Triangle2d
{
	private Vector2 _endA, _endB, _endC;

	/// <summary>An endpoint of the triangle</summary>
	public readonly Vector2 VertexA => _endA;

	/// <summary>An endpoint of the triangle</summary>
	public readonly Vector2 VertexB => _endB;

	/// <summary>An endpoint of the triangle</summary>
	public readonly Vector2 VertexC => _endC;

	/// <summary>An endpoint of the triangle</summary>
	public required (Vector2, Vector2, Vector2) Endpoints
	{
		readonly get => (_endA, _endB, _endC);
		set => (_endA, _endB, _endC) = CoreExtensions.Collinear(
			value.Item1, value.Item2, value.Item3
		) ? value : throw IntTriangle2d.ColinearExp();
	}
}