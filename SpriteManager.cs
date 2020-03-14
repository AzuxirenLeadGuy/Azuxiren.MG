using System;
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
		/// OBSOLETE: Works, but you are better off picking any other Draw() method
		/// 
		/// The draw method of the SpriteSheet
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="sb"></param>
		[Obsolete]
        public void Draw(GameTime gameTime,SpriteBatch sb) => Draw(sb);
        ///<summary>The Draw function of the SpriteSheet, drawing wherever the Dest rectangle of this object is</summary>
        public void Draw(SpriteBatch sb) { sb.Draw(Sheet ,Dest, Source, Color.White); }
        ///<summary>The Draw function of the SpriteSheet, drawing at the rectangle in the argument</summary>
        public void Draw(SpriteBatch sb, Rectangle dest)
        { sb.Draw(Sheet, dest, Source, Color.White); }
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
		internal byte Num,Den,Cur;
		/// <summary>
		/// Sets the speed of unrolling the sprite as a fraction of the current game's FPS.
		/// 
		/// For example, if the game is 60 fps, and the function is invoked with num=1,den=2 (1/2), then the speed of unrolling this spritesheet will be (1/2) of 60fps, ie 30fps
		/// </summary>
		/// <param name="num">Numenator of the fraction</param>
		/// <param name="den">Denominator of fraction</param>
		public void SetSpeed(byte num,byte den)=>(Num,Den,Cur)=(num,den,0);
		/// <summary>
		///OBSOLETE: Works, but you are better off using the Draw() method (the one without arguments)
	 	/// 	
		/// Updates the sprite with gametime
		/// </summary>
		/// <param name="gt"></param>
		[Obsolete]
        public void Update(GameTime gt) => Update();
		/// <summary>
		/// Updates the frame for the sprite
		/// </summary>
        public void Update()
        {
			Cur+=Num;
			if(Cur>=Den)
			{
				CurrentFrame++;
				CurrentFrame %= TotalFrame;
				Cur=(byte)(Cur%Den);
			}
        }
		/// <summary>
		/// Draws the Sprite using the given spritebatch 
		/// </summary>
		/// <param name="sb"></param>
        public void Draw(SpriteBatch sb) => sb.Draw(FrameImages[CurrentFrame], Dest, Color.White);
    }
}
