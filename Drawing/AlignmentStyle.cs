namespace Azuxiren.MG.Drawing;

/// <summary>The paramter for alignment of a text in a TextBox</summary>
public enum AlignmentStyle
{
	/// <summary>Streches the component in both axis to fit the region</summary>
	Stretch = 0,

	/// <summary>Aligns the component to the top left of region</summary>
	StartXStartY = 0x11,

	/// <summary>Aligns the component to the center of region</summary>
	CenterXStartY = 0x21,

	/// <summary>Aligns the component to the end of region</summary>
	EndXStartY = 0x31,

	/// <summary>Aligns the component to the start of region</summary>
	StartXCenterY = 0x12,

	/// <summary>Aligns the component to the center of region</summary>
	CenterXCenterY = 0x22,

	/// <summary>Aligns the component to the end of region</summary>
	EndXCenterY = 0x32,

	/// <summary>Aligns the component to the start of region</summary>
	StartXEndY = 0x13,

	/// <summary>Aligns the component to the center of region</summary>
	CenterXEndY = 0x23,

	/// <summary>Aligns the component to the end of region</summary>
	EndXEndY = 0x33,
}