using System.Diagnostics.CodeAnalysis;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Azuxiren.MG.Drawing;
/// <summary>Maintains the drawing of text in the given Rectangle</summary>
public class TextBox
{

	/// <summary> The Construtctor for Textbox</summary>
	/// <param name="bounds">The rectangle where the text is displayed</param>
	/// <param name="txt">The text to display</param>
	/// <param name="fnt">The font used</param>
	/// <param name="color">The color of the text</param>
	/// /// <param name="align">The alignment of the text</param>
	public TextBox(
		Rectangle bounds,
		string txt,
		SpriteFont fnt,
		Color color,
		AlignmentStyle align = AlignmentStyle.CenterXCenterY
	)
	{
		Text = txt;
		Font = fnt;
		LayerDepth = 0;
		TextColor = color;
		Angle = 0;
		Anchor = default;
		Position = default;
		Scale = default;
		FitText(bounds, align);
	}

	/// <summary>The LayerDepth of the text</summary>
	public float LayerDepth;

	/// <summary>The color of the text</summary>
	public Color TextColor;

	/// <summary>The point of rotation</summary>
	public Vector2 Anchor;

	/// <summary>The angle of rotation around the anchor</summary>
	public float Angle;

	/// <summary>The font for the Textbox</summary>
	public readonly SpriteFont Font;

	/// <summary>The text written in the textbox</summary>
	public string Text;

	/// <summary>The vector representing the position of destination/rendering</summary>
	public Vector2 Position;

	/// <summary>The uniform scale</summary>
	public Vector2 Scale;

	/// <summary>The size taken by this textbox with scale=1</summary>
	public Vector2 StringBaseSize => Font.MeasureString(Text);

	/// <summary>The (top-left) position of the destination rectangle</summary>
	public Vector2 DestLocation => Position - (Scale * Anchor);

	/// <summary>The size of the destination rectangle</summary>
	public Vector2 DestSize => Vector2.Multiply(Scale, StringBaseSize);

	/// <summary>Fits the given text in the rectangular area</summary>
	/// <param name="bounds">The boundary to fit the text at</param>
	/// <param name="style">The horizontal alignment of the text</param>
	public void FitText(
		Rectangle bounds,
		AlignmentStyle style = AlignmentStyle.CenterXCenterY
	)
	{
		if (bounds.Width == 0 || bounds.Height == 0)
		{
			return;
		}
		Angle = 0;
		Vector2 base_size = StringBaseSize;
		DrawingExtensions.SetPositionAndScale(
			base_size,
			bounds.Location.ToVector2(),
			bounds.Size.ToVector2(),
			out Position,
			out Scale,
			out Anchor,
			style
		);
	}
	/// <summary>Draws the string</summary>
	/// <param name="batch">The spritebatch for the game</param>
	/// <param name="effects">The SpriteEffects to use</param>
	public void Draw(
		[NotNull] IBatchDrawer batch,
		SpriteEffects effects = SpriteEffects.None
	) => batch.DrawString(
		Font,
		Text,
		Position,
		TextColor,
		Angle,
		Anchor,
		Scale,
		effects,
		LayerDepth
	);
}