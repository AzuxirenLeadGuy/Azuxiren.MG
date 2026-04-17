using System.Diagnostics.CodeAnalysis;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Azuxiren.MG.Drawing;
/// <summary>Maintains the drawing of text in the given Rectangle</summary>
public class TextBox
{
	/// <summary>The paramter for alignment of a text in a TextBox</summary>
	public enum TextAlignment
	{
		/// <summary>Aligns the text to the left of textbox</summary>
		Left,
		/// <summary>Aligns the text to the center of TextBox</summary>
		Centered,
		/// <summary>Aligns the text to the right of TextBox</summary>
		Right
	}

	/// <summary> The Construtctor for Textbox</summary>
	/// <param name="bd">The rectangle where the text is displayed</param>
	/// <param name="txt">The text to display</param>
	/// <param name="fnt">The font used</param>
	/// <param name="color">The color of the text</param>
	/// /// <param name="align">The alignment of the text</param>
	public TextBox(
		Rectangle bd,
		string txt,
		SpriteFont fnt,
		Color color,
		TextAlignment align = TextAlignment.Centered
	)
	{
		_bounds = bd;
		_text = txt;
		_font = fnt;
		LayerDepth = 0;
		TextColor = color;
		_alignment = align;
		Angle = 0;
		Anchor = default;
		_pos = default;
		_scale = default;
		FitText();
	}

	/// <summary>Destination rectangle where the text is to be drawn</summary>
	public Rectangle Bounds
	{
		get => _bounds;
		set
		{
			_bounds = value;
			FitText();
		}
	}

	/// <summary>This is the text to display</summary>
	public string Text
	{
		get => _text;
		set
		{
			_text = value;
			FitText();
		}
	}

	/// <summary>The font used</summary>
	public SpriteFont Font
	{
		get => _font;
		set
		{
			_font = value;
			FitText();
		}
	}

	/// <summary>The Alignment of the text within the box</summary>
	public TextAlignment Alignment
	{
		get => _alignment;
		set
		{
			_alignment = value;
			FitText();
		}
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
	protected SpriteFont _font;

	/// <summary>The destination rectangle for the Textbox</summary>
	private Rectangle _bounds;

	/// <summary>The text written in the textbox</summary>
	private string _text;

	/// <summary>The vector representing the position of destination/rendering</summary>
	private Vector2 _pos;

	/// <summary>The uniform scale</summary>
	private float _scale;

	/// <summary>The text alignment</summary>
	private TextAlignment _alignment;
	private void FitText()
	{
		if (_bounds.Width == 0 || _bounds.Height == 0)
		{
			return;
		}

		Vector2 size = _font.MeasureString(_text);
		// Taking the smaller scaling value will result in the text always fitting in the boundaires.
		_scale = float.Min(_bounds.Width / size.X, _bounds.Height / size.Y);
		// Figure out the location to absolutely-center it in the boundaries rectangle.
		_pos = _bounds.Center.ToVector2();
		Anchor = size / 2;
		Vector2 fontlength = size * _scale;
		_pos.Y += (_bounds.Height - fontlength.Y) / 2;
		if (_alignment != TextAlignment.Left)
		{
			float diff = _bounds.Width - fontlength.X;
			_pos.X += _alignment == TextAlignment.Right ?
				diff : diff / 2;
		}
	}
	/// <summary>Draws the string</summary>
	/// <param name="batch">The spritebatch for the game</param>
	/// <param name="effects">The SpriteEffects to use</param>
	public void Draw(
		[NotNull] IBatchDrawer batch,
		SpriteEffects effects = SpriteEffects.None
	) => batch.DrawString(
		_font,
		_text,
		_pos,
		TextColor,
		Angle,
		Anchor,
		new(_scale),
		effects,
		LayerDepth
	);
}