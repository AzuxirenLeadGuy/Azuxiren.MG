using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Azuxiren.MG.Drawing;

/// <summary>The minimal functions for a Spritebatch implementation</summary>
public interface IBatchDrawer
{
	/// <summary> Submit a sprite for drawing in the current batch.</summary>
	/// <param name="texture">A texture.</param>
	/// <param name="position">The drawing location on screen.</param>
	/// <param name="sourceRectangle">An optional region on the texture which will be rendered. If null - draws full texture.</param>
	/// <param name="color">A color mask.</param>
	/// <param name="rotation">A rotation of this sprite.</param>
	/// <param name="origin">Center of the rotation. 0,0 by default.</param>
	/// <param name="scale">A scaling of this sprite.</param>
	/// <param name="effects">Modificators for drawing. Can be combined.</param>
	/// <param name="layerDepth">A depth of the layer of this sprite.</param>
	void Draw(
		Texture2D texture,
		Vector2 position,
		Rectangle? sourceRectangle = null,
		Color? color = null,
		Vector2? scale = null,
		Vector2 origin = default,
		float rotation = 0,
		SpriteEffects effects = SpriteEffects.None,
		float layerDepth = 0
	);
	/// <summary> Submit a sprite for drawing in the current batch.</summary>
	/// <param name="texture">A texture.</param>
	/// <param name="destination">The drawing location on screen.</param>
	/// <param name="sourceRectangle">An optional region on the texture which will be rendered. If null - draws full texture.</param>
	/// <param name="color">A color mask.</param>
	/// <param name="rotation">A rotation of this sprite.</param>
	/// <param name="origin">Center of the rotation. 0,0 by default.</param>
	/// <param name="effects">Modificators for drawing. Can be combined.</param>
	/// <param name="layerDepth">A depth of the layer of this sprite.</param>
	void Draw(
		Texture2D texture,
		Rectangle destination,
		Rectangle? sourceRectangle = null,
		Color? color = null,
		Vector2 origin = default,
		float rotation = 0,
		SpriteEffects effects = SpriteEffects.None,
		float layerDepth = 0
	);

	/// <summary>
	/// Submit a text string of sprites for drawing in the current batch.
	/// </summary>
	/// <param name="spriteFont">A font.</param>
	/// <param name="text">The text which will be drawn.</param>
	/// <param name="position">The drawing location on screen.</param>
	/// <param name="color">A color mask.</param>
	/// <param name="rotation">A rotation of this string.</param>
	/// <param name="origin">Center of the rotation. 0,0 by default.</param>
	/// <param name="scale">A scaling of this string.</param>
	/// <param name="effects">Modificators for drawing. Can be combined.</param>
	/// <param name="layerDepth">A depth of the layer of this string.</param>
	public void DrawString(
		SpriteFont spriteFont,
		string text,
		Vector2 position,
		Color color,
		float rotation = 0,
		Vector2? origin = null,
		Vector2? scale = null,
		SpriteEffects effects = SpriteEffects.None,
		float layerDepth = 0
	);
	/// <summary>
	/// Submit a text string of sprites for drawing in the current batch.
	/// </summary>
	/// <param name="spriteFont">A font.</param>
	/// <param name="text">The text which will be drawn.</param>
	/// <param name="position">The drawing location on screen.</param>
	/// <param name="color">A color mask.</param>
	/// <param name="rotation">A rotation of this string.</param>
	/// <param name="origin">Center of the rotation. 0,0 by default.</param>
	/// <param name="scale">A scaling of this string.</param>
	/// <param name="effects">Modificators for drawing. Can be combined.</param>
	/// <param name="layerDepth">A depth of the layer of this string.</param>
	public void DrawString(
		SpriteFont spriteFont,
		string text,
		Vector2 position,
		Color color,
		float rotation = 0,
		Vector2? origin = null,
		float scale = 1,
		SpriteEffects effects = SpriteEffects.None,
		float layerDepth = 0
	) => DrawString(
		spriteFont,
		text,
		position,
		color,
		rotation,
		origin,
		new Vector2(scale),
		effects,
		layerDepth
	);
}