using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Azuxiren.MG.Drawing;

/// <summary>Represents a Texture2D and additional data for drawing</summary>
public struct Sprite
{
	/// <summary>The texture for the sprite</summary>
	public readonly Texture2D Texture;
	/// <summary>The center for the rotation</summary>
	public Vector2 Anchor;
	/// <summary>The rotation angle in radians</summary>
	public float Rotation;
	/// <summary>The tint color</summary>
	public Color Tint;
	/// <summary>The zoom/scale for the drawing</summary>
	public Vector2 Scale;
	/// <summary>The drawing destination on screen</summary>
	public Vector2 Location;
	/// <summary>Creates a Sprite object</summary>
	/// <param name="tex">The texture to initialize the sprite with</param>
	/// <param name="anchoredCenter">if true, the anchor point is set at the center of the texture</param>
	public Sprite(Texture2D tex, bool anchoredCenter = true)
	{
		if (tex == null || tex.Bounds.Size == Point.Zero)
		{
			throw new ArgumentException(
				"The texture must not be null or empty",
				nameof(tex)
			);
		}
		Texture = tex;
		Anchor = anchoredCenter ? tex.Bounds.Size.ToVector2() / 2 : Vector2.Zero;
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
		var cur_size = Texture.Bounds.Size;
		var target_size = dest.Size;
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
	public readonly Rectangle GetDest() 
		=> new(
			Vector2.Round(Location - Anchor).ToPoint(),
			Vector2.Round(Scale).ToPoint()
		);

	/// <summary>Draws the sprite with the Spritebatch instance</summary>
	/// <param name="batch">The spritebatch instance to draw with</param>
	/// <param name="effect">The effects to use</param>
	/// <param name="depth">The depth to draw on</param>
	public readonly void Draw(in SpriteBatch batch, SpriteEffects effect = SpriteEffects.None, float depth = 0)
	{
		batch.Draw(
			Texture,
			Location,
			null,
			Tint,
			Rotation,
			Anchor,
			Scale,
			effect,
			depth
		);
	}

	/// <summary>Draws the sprite with the BatchDrawer instance</summary>
	/// <param name="batch">The spritebatch instance to draw with</param>
	/// <param name="effect">The effects to use</param>
	/// <param name="depth">The depth to draw on</param>
	public readonly void Draw(in IBatchDrawer batch, SpriteEffects effect = SpriteEffects.None, float depth = 0)
	{
		batch.Draw(
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
	}

}
