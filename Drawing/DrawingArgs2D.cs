using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Azuxiren.MG.Drawing;

/// <summary>The arguments for drawing a Texture2D instance</summary>
/// <param name="SourceRegion">The rectangular area of the source of texture</param>
public record struct DrawingArgs2D(Rectangle SourceRegion)
{
	/// <summary>The rectangular area of the source of texture</summary>
	public Vector2 Location;

	/// <summary>The rotation with respect to the Anchor point</summary>
	public float Angle = 0;

	/// <summary>The anchor for the sprite</summary>
	public Vector2 Anchor = SourceRegion.Center.ToVector2();

	/// <summary>The scale of the texture</summary>
	public Vector2 Scale = Vector2.One;

	/// <summary>Draws a given texture with the detail in this struct</summary>
	/// <param name="drawer">The drawing instance</param>
	/// <param name="texture">The texture to draw</param>
	/// <param name="color">The color for tinting</param>
	/// <param name="effects">Additional effects to use</param>
	/// <param name="layerDepth">The depth to work with</param>
	public readonly void Draw(
		IBatchDrawer drawer,
		Texture2D texture,
		Color? color = null,
		SpriteEffects effects = SpriteEffects.None,
		float layerDepth = 0
	) => drawer.Draw(
		texture,
		Location,
		SourceRegion,
		color,
		Scale,
		Anchor,
		Angle,
		effects,
		layerDepth
	);
}