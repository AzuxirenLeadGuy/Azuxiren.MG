using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Azuxiren.MG.Drawing;

/// <summary>Represents a Texture2D and additional data for drawing</summary>
public record class Sprite : IDisposable
{
	/// <summary>The texture for the sprite</summary>
	public readonly Texture2D Texture;

	/// <summary>The center for the rotation relative to Texture</summary>
	public Vector2 Anchor;

	/// <summary>The rotation angle in radians relative to the Anchor</summary>
	public float Angle;

	/// <summary>The tint color</summary>
	public Color Tint;

	/// <summary>The zoom/scale for the drawing</summary>
	public Vector2 Scale;

	/// <summary>The Vector on screen for drawing</summary>
	public Vector2 Location;

	/// <summary>The size of the texture as a Vector2</summary>
	protected Vector2 TextureSize => Texture.Bounds.Size.ToVector2();

	/// <summary>The size of the destination rectangle</summary>
	public Vector2 DestSize => Vector2.Multiply(Scale, TextureSize);

	/// <summary>The (top-left) position of the destination rectangle</summary>
	public Vector2 DestLocation => Location - (Scale * Anchor);

	/// <summary>Creates a Sprite object</summary>
	/// <param name="tex">The texture to initialize the sprite with</param>
	/// <param name="centeredAnchor">if true, the anchor point is set at the center of the texture</param>
	public Sprite(Texture2D tex, bool centeredAnchor = true)
	{
		if (tex == null || tex.Bounds.Size == Point.Zero)
		{
			throw new ArgumentException(
				"The texture must not be null or empty",
				nameof(tex)
			);
		}
		Texture = tex;
		Anchor = centeredAnchor ? tex.Bounds.Size.ToVector2() / 2 : Vector2.Zero;
		Location = Vector2.Zero;
		Tint = Color.White;
		Angle = 0;
	}

	/// <summary>Sets the scale such that the texture can fit in the given rectangle</summary>
	/// <param name="dest">The rectangle to fit the texture for</param>
	/// <param name="style">The fitting style of the position</param>
	/// <returns>Returns the target vector for the sprite to be drawn at</returns>
	public void SetDest(
		Rectangle dest,
		AlignmentStyle style = AlignmentStyle.CenterXCenterY
	)
	{
		Angle = 0;
		DrawingExtensions.SetPositionAndScale(
			TextureSize,
			dest.Location.ToVector2(),
			dest.Size.ToVector2(),
			out Location,
			out Scale,
			out Anchor,
			style
		);
	}
	/// <summary>Gets the Rectangle of the bounds of the sprite </summary>
	/// <returns>Rectangle with values rounded to the nearest integer</returns>
	public Rectangle GetDest() => new(
		Vector2.Round(Location - Anchor).ToPoint(),
		Vector2.Round(Texture.Bounds.Size.ToVector2() * Scale).ToPoint()
	);

	/// <summary>Draws the sprite with the BatchDrawer instance</summary>
	/// <param name="batch">The spritebatch instance to draw with</param>
	/// <param name="effect">The effects to use</param>
	/// <param name="depth">The depth to draw on</param>
	public void Draw(
		[NotNull] in IBatchDrawer batch,
		SpriteEffects effect = SpriteEffects.None,
		float depth = 0
	) => batch.Draw(
			Texture,
			Location,
			null,
			Tint,
			Scale,
			Anchor,
			Angle,
			effect,
			depth
		);

	/// <inheritdoc/>
	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	/// <inheritdoc/>
	protected virtual void Dispose(bool manual)
	{
		if (!Texture.IsDisposed || !manual) { return; }
		Texture.Dispose();
	}

	/// <summary>Finalizer for Sprite</summary>
	~Sprite() { Dispose(false); }
}