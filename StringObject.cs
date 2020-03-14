using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Azuxiren.MG
{
	/// <summary>Maintains the drawing of text in the given Rectangle</summary>
	public struct StringObject
	{
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
		/// <summary>The LayerDepth of the text</summary>
		public float LayerDepth;
		/// <summary>The color of the text</summary>
		public Color TextColor;
		SpriteFont font;
		Rectangle bounds;
		string text;
		Vector2 Pos;
		float Scale;
		internal void FitText()
		{
			bounds.FitText(font,text,out Scale,out Pos);
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