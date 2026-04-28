using System.Diagnostics.CodeAnalysis;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Azuxiren.MG.Drawing;
/// <summary>Represents a spritesheet</summary>
/// <remarks>Initializes a Spritesheet</remarks>
/// <param name="tex">The texture for the spritesheet</param>
public abstract class SpriteSheet(Texture2D tex) : Sprite(tex)
{
	/// <summary>The position of the source rectangle for this spritesheet</summary>
	public Rectangle SourceRect { get; protected set; }

	/// <summary>
	/// The update function of the spritesheet should reposition
	/// the source rectangle as needed.
	/// </summary>
	/// <param name="time">The time delta since the last update</param>
	public abstract void Update(GameTime time);

	/// <summary>Sets the scale such that the texture can fit in the given rectangle</summary>
	/// <param name="dest">The rectangle to draw the texture at</param>
	/// <param name="style">The fitting style</param>
	public override void SetDest(
		Rectangle dest,
		AlignmentStyle style = AlignmentStyle.CenterXCenterY
	)
	{
		Angle = 0;
		DrawingExtensions.SetPositionAndScale(
			SourceRect.Size.ToVector2(),
			dest.Location.ToVector2(),
			dest.Size.ToVector2(),
			out Location,
			out Scale,
			out Anchor,
			style
		);
	}

	/// <inheritdoc/>
	public override void Draw(
		[NotNull] in IBatchDrawer batch,
		SpriteEffects effect = SpriteEffects.None,
		float depth = 0
	) => batch.Draw(
			Texture,
			Location,
			SourceRect,
			Tint,
			Scale,
			Anchor,
			Angle,
			effects: effect,
			layerDepth: depth
		);

}