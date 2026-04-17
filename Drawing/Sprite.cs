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
	public float Rotation;
	/// <summary>The tint color</summary>
	public Color Tint;
	/// <summary>The zoom/scale for the drawing</summary>
	public Vector2 Scale;
	/// <summary>The Vector on screen for drawing</summary>
	public Vector2 Location;

	/// <summary>The size of the texture as a Vector2</summary>
	protected Vector2 TextureSize => Texture.Bounds.Size.ToVector2();

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
		Rotation = 0;
	}

	/// <summary>Sets the scale such that the texture can fit in the given rectangle</summary>
	/// <param name="dest">The rectangle to fit the texture for</param>
	/// <param name="maintainRatio">if true, the aspect ratio of the texture will be maintained</param>
	/// <returns>Returns the target vector for the sprite to be drawn at</returns>
	public void SetDest(Rectangle dest, bool maintainRatio = true)
	{
		Point cur_size = Texture.Bounds.Size;
		Point target_size = dest.Size;
		if (maintainRatio)
		{
			Scale.X = Scale.Y = float.Min(
				(float)target_size.X / cur_size.X,
				(float)target_size.Y / cur_size.Y
			);
		}
		else
		{
			Scale.X = (float)target_size.X / cur_size.X;
			Scale.Y = (float)target_size.Y / cur_size.Y;
		}
		Location = dest.Location.ToVector2() + Anchor;
	}
	/// <summary>Gets the Rectangle of the bounds of the sprite </summary>
	/// <returns>Rectangle with values rounded to the nearest integer</returns>
	public Rectangle GetDest()
		=> new(
			Vector2.Round(Location - Anchor).ToPoint(),
			Vector2.Round(Scale).ToPoint()
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
			Rotation,
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
		if (Texture.IsDisposed) { return; }
		if (manual) { Texture.Dispose(); }
	}

	/// <summary>Finalizer for Sprite</summary>
	~Sprite()
	{
		Dispose(false);
	}
}