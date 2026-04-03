using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Azuxiren.MG.Drawing;
/// <summary>Maintains the drawing of text in the given Rectangle</summary>
public struct TextBox
{
	/// <summary>The paramter for alignment of a text in a TextBox</summary>
	public enum TextAlignment : byte
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
		ResetRotationAnchor();
	}
	/// <summary>
	/// This is the Destination rectangle where the text is to be drawn
	/// </summary>
	/// <value></value>
	public Rectangle Bounds
	{
		readonly get => _bounds;
		set
		{
			_bounds = value;
			FitText();
		}
	}
	/// <summary>
	/// This is the text to display
	/// </summary>
	/// <value></value>
	public string Text
	{
		readonly get => _text;
		set
		{
			_text = value;
			FitText();
		}
	}
	/// <summary>
	/// This is the font used
	/// </summary>
	/// <value></value>
	public SpriteFont Font
	{
		readonly get => _font;
		set
		{
			_font = value;
			FitText();
		}
	}
	/// <summary>
	/// The Alignment of the text within the box
	/// </summary>
	/// <value></value>
	public TextAlignment Alignment
	{
		readonly get => _alignment;
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
	private SpriteFont _font;
	private Rectangle _bounds;
	private string _text;
	private Vector2 _pos;
	private float _scale;
	private TextAlignment _alignment;
	/// <summary>Sets Anchor of rotation to the center of the text</summary>
	public void ResetRotationAnchor() => Anchor = _font.MeasureString(_text) * _scale / 2;
	internal void FitText()
	{
		Vector2 size = _font.MeasureString(_text);
		// Taking the smaller scaling value will result in the text always fitting in the boundaires.
		_scale = float.Min(_bounds.Width / size.X, _bounds.Height / size.Y);
		// Figure out the location to absolutely-center it in the boundaries rectangle.
		_pos = _bounds.Location.ToVector2();
		Vector2 fontlength = size * _scale;
		_pos.Y += (_bounds.Height - fontlength.Y) / 2;
		if (_alignment != TextAlignment.Left)
		{
			var diff = _bounds.Width - fontlength.X;
			if (_alignment == TextAlignment.Right) _pos.X += diff;
			else _pos.X += diff / 2;
		}
	}
	/// <summary>Draws the string</summary>
	/// <param name="batch">The spritebatch for the game</param>
	/// <param name="effects">The SpriteEffects to use</param>
	public readonly void Draw(
		SpriteBatch batch,
		SpriteEffects effects = SpriteEffects.None
	) => batch.DrawString(
		_font,
		_text,
		_pos,
		TextColor,
		Angle,
		Anchor,
		_scale,
		effects,
		LayerDepth
	);
}