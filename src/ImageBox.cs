using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Azuxiren.MG
{
	/// <summary>
	/// A simple struct for wrapping images and
	/// their destination rectangle together
	/// </summary>
	public struct ImageBox
	{
		/// <summary>The Rectangle to draw the image at</summary>
		public Rectangle Dest;
		/// <summary>The Image to draw</summary>
		public Texture2D Image;
		/// <summary>
		/// Draws the image
		/// </summary>
		/// <param name="batch">The spritebatch used for drawing</param>
		public void Draw(SpriteBatch batch) => batch.Draw(Image, Dest, Color.White);
		/// <summary>
		/// Draws the image
		/// </summary>
		/// <param name="batch">The spritebatch used for drawing</param>
		/// <param name="tint">The color to tint the image with</param>
		public void Draw(SpriteBatch batch, Color tint) => batch.Draw(Image, Dest, tint);
	}
}
