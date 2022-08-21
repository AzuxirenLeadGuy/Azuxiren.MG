using System;
namespace Azuxiren.MG.Hex
{
	/// <summary>Represents the neighbour directions 
	/// from a single position in the grid.<br/>
	/// The hexagon are in odd-q (flat base) layout</summary>
	[Flags]
	public enum HexGridNeighbours : byte
	{
		/// <summary>Invalid value</summary>
		None = 0,
		/// <summary>Hexagon at top of current</summary>
		Top = 0b10000001,
		/// <summary>Hexagon at top left of current</summary>
		TopLeft = 0b10000011,
		/// <summary>Hexagon at top right of current</summary>
		TopRight = 0b10000101,
		/// <summary>Hexagon at bottom left of current</summary>
		BottomLeft = 0b10001010,
		/// <summary>Hexagon at bottom right of current</summary>
		BottomRight = 0b10001100,
		/// <summary>Hexagon at bottom of current</summary>
		Bottom = 0b1001000,
		/// <summary>Hexagon at diagonal left of current</summary>
		DiagonalLeft = 0b11000010,
		/// <summary>Hexagon at diagonal right of current</summary>
		DiagonalRight = 0b11000100,
		/// <summary>Hexagon at diagonal top left of current</summary>
		DiagonalTopLeft = 0b11000011,
		/// <summary>Hexagon at diagonal top right of current</summary>
		DiagonalTopRight = 0b11000101,
		/// <summary>Hexagon at diagonal bottom left of current</summary>
		DiagonalBottomLeft = 0b11001010,
		/// <summary>Hexagon at diagonal bottom right of current</summary>
		DiagonalBottomRight = 0b11001100,
	}
}