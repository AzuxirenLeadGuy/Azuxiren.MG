namespace Azuxiren.MG.Drawing;

/// <summary>Denotes the count of pixels attempted and drawn by drawing function</summary>
public record struct DrawResult
{
	/// <summary>The number of pixels attempted to be drawn</summary>
	public uint Attempted;
	/// <summary>The number of pixels that were drawn successfully</summary>
	public uint Drawn;
	/// <summary>The number of pixels that were outside the bounds and could not be drawn</summary>
	public readonly uint Missed => Attempted - Drawn;
}