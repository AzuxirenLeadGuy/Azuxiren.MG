using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Azuxiren.MG
{
	///<summary>SpriteSheet Manager. 
	///
	///Only use Draw and Update methods once initialized</summary>
	public struct SpriteSheet
	{
		///<summary>SpriteSheet Image</summary>
		internal Texture2D Sheet;
		///<summary>Source Rectangle Maps the image on the source Spritesheet</summary>
		internal Rectangle Source;
		///<summary>Destination Rectangle is on the Screen</summary>
		public Rectangle Dest;
		///<summary>Keeps track of Frames on the SpriteSheet</summary>
		internal int[] FrameX, FrameY, Next;
		///<summary>The Count of Frames</summary>
		int Frames;
		///<summary>The Current Frame</summary>
		int CurrentFrame;
		///<summary>The Constructor that *MUST* be used</summary>
		///<param name="sh">Sheet Image Texture</param>
		///<param name="fw">Frame-width</param>
		///<param name="fh">Frame-height</param>
		///<param name="lx">Last Frame-X count (Starts from 0)</param>
		///<param name="ly">Last Frame-Y count (Starts from 0)</param>
		public SpriteSheet(Texture2D sh, int fw, int fh, int lx, int ly)
		{
			Sheet = sh;
			int FramesPerLine = sh.Width / fw;
			Source = new Rectangle(0, 0, fw, fh);
			Dest = new Rectangle(0, 0, fw, fh);
			Frames = (FramesPerLine * ly) + lx + 1;
			FrameX = new int[Frames];
			FrameY = new int[Frames];
			Next = new int[Frames];
			CurrentFrame = 0;
			int i, j;
			for (i = 0; i < ly; i++)
			{
				for (j = 0; j < FramesPerLine; j++)
				{
					FrameX[CurrentFrame] = j * fw;
					FrameY[CurrentFrame] = i * fh;
					Next[CurrentFrame] = CurrentFrame + 1;
					CurrentFrame++;
				}
			}
			for (j = 0; j <= lx; j++)
			{
				FrameX[CurrentFrame] = j * fw;
				FrameY[CurrentFrame] = ly * fh;
				Next[CurrentFrame] = CurrentFrame + 1;
				CurrentFrame++;
			}
			Next[Frames - 1] = 0;
			CurrentFrame = 0;
		}
		///<summary>Sets the Current Animation frame at f</summary>
		/// <param name="f">The frame value to set</param>
		public void SetFrame(int f) => CurrentFrame = f;
		/// <summary>
		/// Draws the SpriteSheet
		/// </summary>
		/// <param name="sb">SpriteBatch object to use</param>
		public void Draw(SpriteBatch sb) { sb.Draw(Sheet, Dest, Source, Color.White); }
		/// <summary>
		/// Draws the SpriteSheet
		/// </summary>
		/// <param name="sb">SpriteBatch object to use</param>
		/// <param name="dest"></param>
		public void Draw(SpriteBatch sb, Rectangle dest) { sb.Draw(Sheet, dest, Source, Color.White); }
		/// <summary>
		/// Draws the SpriteSheet
		/// </summary>
		/// <param name="sb">SpriteBatch object to use</param>
		/// <param name="dest">The Rectangle to draw the sheet frame at. (You are better off using the Dest variable instide the Spritesheet class</param>
		/// <param name="Tint">The Color to tint the drawing with</param>
		public void Draw(SpriteBatch sb, Rectangle dest, Color Tint) { sb.Draw(Sheet, dest, Source, Tint); }
		/// <summary>
		/// Draws the SpriteSheet
		/// </summary>
		/// <param name="sb">SpriteBatch object to use</param>
		/// <param name="Tint">The Color to tint the drawing with</param>
		public void Draw(SpriteBatch sb, Color Tint) { sb.Draw(Sheet, Dest, Source, Tint); }
		///<summary>The Update Function of SpriteSheet
		///
		///Not Calling Update "Pauses" the Animation.</summary>
		public void Update()
		{
			Source.X = FrameX[CurrentFrame];
			Source.Y = FrameY[CurrentFrame];
			CurrentFrame = Next[CurrentFrame];
		}
		/// <summary>
		/// Copy the properties of another spritesheet, sharing the same reference of Texture2D Spritesheet image
		/// </summary>
		/// <param name="source"></param>
		public void Copy(SpriteSheet source)
		{
			Sheet = source.Sheet;
			Next = source.Next;
			FrameX = source.FrameX;
			FrameY = source.FrameY;
			Frames = source.Frames;
			Source = source.Source;
			Dest = source.Dest;
		}
	}
	/// <summary>
	/// LargeSprite consist of series of Texture2D. This is Not a SpriteSheet. 
	/// 
	/// If the animation consists of very large frames, this is the struct you need, provieded you are able to give
	/// an array of images/Texture2D as input
	/// </summary>
	public struct LargeSprite
	{
		/// <summary>
		/// The constructor for LargeSprite
		/// </summary>
		/// <param name="Frames">The frames in this Sprite</param>
		/// <param name="dest">The Rectangle where all the spirtes are being displayed</param>
		/// <param name="speednum">The numenator of the ratio of speed of unrolling the sprite with respect to the framerate of the game. Don't touch if you don't understand</param>
		/// <param name="speedden">The denominator of the ratio of speed of unrolling the sprite with respect to the framerate of the game. Don't touch if you don't understand</param>
		public LargeSprite(IEnumerable<Texture2D> Frames, Rectangle dest, byte speednum = 1, byte speedden = 1)
		{
			FrameImages = Enumerable.ToArray(Frames);
			Dest = dest;
			Num = speednum;
			Den = speedden;
			Cur = 0;
			CurrentFrame = 0;
		}
		/// <summary>
		/// The sprite Image collection
		/// </summary>
		public Texture2D[] FrameImages;
		/// <summary>
		/// This is where the sprite is drawn
		/// </summary>
		public Rectangle Dest;
		/// <summary>
		/// Denotes the currentFrame value.
		/// </summary>
		public int CurrentFrame;
		/// <summary>
		/// The total count of frames in the object
		/// </summary>
		public int TotalFrame => FrameImages.Length;
		internal byte Num, Den, Cur;
		/// <summary>
		/// Sets the speed of unrolling the sprite as a fraction of the current game's FPS.
		/// 
		/// For example, if the game is 60 fps, and the function is invoked with num=1,den=2 (1/2), then the speed of unrolling this spritesheet will be (1/2) of 60fps, ie 30fps
		/// </summary>
		/// <param name="num">Numenator of the fraction</param>
		/// <param name="den">Denominator of fraction</param>
		public void SetSpeed(byte num, byte den) => (Num, Den, Cur) = (num, den, 0);
		/// <summary>
		/// Updates the frame for the sprite
		/// </summary>
		public void Update()
		{
			Cur += Num;
			if (Cur >= Den)
			{
				CurrentFrame++;
				CurrentFrame %= TotalFrame;
				Cur = (byte)(Cur % Den);
			}
		}
		/// <summary>
		/// Draws the Sprite using the given spritebatch 
		/// </summary>
		/// <param name="sb">The given SpriteBatch</param>
		public void Draw(SpriteBatch sb) => Draw(sb, Color.White, 0, Vector2.Zero);
		/// <summary>
		/// Draws the spirte using the given spritebatch
		/// </summary>
		/// <param name="sb">The given SpriteBatch</param>
		/// <param name="Tint">The tint color to add</param>
		public void Draw(SpriteBatch sb, Color Tint) => Draw(sb, Tint, 0, Vector2.Zero);
		/// <summary>
		/// Draws the sprite using the given spritebatcj
		/// </summary>
		/// <param name="spriteBatch">The given SpriteBatch</param>
		/// <param name="Tint">The tint color to add</param>
		/// <param name="angle">The angle to rotate</param>
		public void Draw(SpriteBatch spriteBatch, Color Tint, float angle) => Draw(spriteBatch, Tint, angle, FrameImages[CurrentFrame].Bounds.Center.ToVector2());
		/// <summary>
		/// Draws the sprite using the given spritebatcj
		/// </summary>
		/// <param name="spriteBatch">The given SpriteBatch</param>
		/// <param name="Tint">The tint color to add</param>
		/// <param name="angle">The angle to rotate</param>
		/// <param name="origin">The origin of rotation</param>
		/// <param name="effects">Added effects</param>
		/// <param name="depth">The depth in the layer for this sprite</param>
		public void Draw(SpriteBatch spriteBatch, Color Tint, float angle, Vector2 origin, SpriteEffects effects = SpriteEffects.None, float depth = 0) => spriteBatch.Draw(FrameImages[CurrentFrame], Dest, null, Tint, angle, origin, effects, depth);
	}
	public struct MultiComponentSprite
	{

	}
}