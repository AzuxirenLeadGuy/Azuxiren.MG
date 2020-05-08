using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Azuxiren.MG
{
	/// <summary>The paramter for alignment of a text in a TextBox</summary>
	public enum Alignment:byte
	{
		/// <summary>Aligns the text to the left of textbox</summary>
		Left, 
		/// <summary>Aligns the text to the center of TextBox</summary>
		Centered, 
		/// <summary>Aligns the text to the right of TextBox</summary>
		Right
	}
	/// <summary>Maintains the drawing of text in the given Rectangle</summary>
	public struct TextBox
	{
		/// <summary>
		/// The Construtctor for Textbox
		/// </summary>
		/// <param name="bd">The rectangle where the text is displayed</param>
		/// <param name="txt">The text to display</param>
		/// <param name="fnt">The font used</param>
		public TextBox(Rectangle bd, string txt, SpriteFont fnt)
		{
			bounds=bd;
			text=txt;
			font=fnt;
			LayerDepth=0;
			TextColor=Color.Black;
			Pos=default;
			Scale=default;
			alignment=Alignment.Left;
			FitText();
		}
		/// <summary>
		/// The Construtctor for Textbox
		/// </summary>
		/// <param name="bd">The rectangle where the text is displayed</param>
		/// <param name="txt">The text to display</param>
		/// <param name="fnt">The font used</param>
		/// <param name="c">The color of the text</param>
		/// <param name="align">The alignment of the text</param>
		public TextBox(Rectangle bd, string txt, SpriteFont fnt, Color c, Alignment align=Alignment.Left)
		{
			bounds=bd;
			text=txt;
			font=fnt;
			LayerDepth=0;
			TextColor=c;
			Pos=default;
			Scale=default;
			alignment=align;
			FitText();
		}
		/// <summary>
		/// This is the Destination rectangle where the text is to be drawn 
		/// </summary>
		/// <value></value>
		public Rectangle Bounds 
		{
			get=>bounds;
			set
			{
				bounds=value;
				FitText();
			}
		}
		/// <summary>
		/// This is the text to display
		/// </summary>
		/// <value></value>
		public string Text
		{
			get=>text;
			set
			{
				text=value;
				FitText();
			}
		}
		/// <summary>
		/// This is the font used
		/// </summary>
		/// <value></value>
		public SpriteFont Font
		{
			get=>font;
			set
			{
				font=value;
				FitText();
			}
		}
		/// <summary>
		/// The Alignment of the text within the box
		/// </summary>
		/// <value></value>
		public Alignment Alignment
		{
			get=>alignment;
			set
			{
				alignment=value;
				FitText();
			}
		}
		/// <summary>The LayerDepth of the text</summary>
		public float LayerDepth;
		/// <summary>The color of the text</summary>
		public Color TextColor;
		SpriteFont font;
		Rectangle bounds;
		string text;
		Vector2 Pos;
		float Scale;
		Alignment alignment;
		internal void FitText()
		{
			bounds.FitText(font,text,out Scale,out Pos);
			if(alignment!=Alignment.Left)
			{
				var len=(int)((font.MeasureString(text).X*Scale)+Pos.X);
				var diff=bounds.Right-len;
				if(alignment==Alignment.Right)Pos.X+=diff;
				else Pos.X+=diff/2;
			}
		}
		/// <summary>
		/// Draws the string (Color of text is white, by default)
		/// </summary>
		/// <param name="batch">The spritebatch for the game</param>
		public void Draw(SpriteBatch batch)
		{
			batch.DrawString(font,text,Pos,TextColor,0,Vector2.Zero,Scale,SpriteEffects.None,LayerDepth);
		}
		/// <summary>
		/// Draws the string
		/// </summary>
		/// <param name="batch">The spritebatch for the game</param>
		/// <param name="Rotation">The value of rotation</param>
		/// <param name="origin">The point to rotate the text about</param>
		public void Draw(SpriteBatch batch, float Rotation, Vector2 origin)
		{
			batch.DrawString(font,text,Pos,TextColor,Rotation,origin,Scale,SpriteEffects.None,LayerDepth);
		}
		/// <summary>
		/// Draws the string
		/// </summary>
		/// <param name="batch">The spritebatch for the game</param>
		/// <param name="Rotation">The value of rotation</param>
		/// <param name="origin">The point to rotate the text about</param>
		/// <param name="effects">The SpriteEffects to use</param>
		public void Draw(SpriteBatch batch,float Rotation, Vector2 origin, SpriteEffects effects)
		{
			batch.DrawString(font,text,Pos,TextColor,Rotation,origin,Scale,effects,LayerDepth);
		}
	}
}