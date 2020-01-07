using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Azuxiren.MG
{
	/// <summary>
	/// The Common Interface to use for all Screens
	/// </summary>
	public interface IScreen
	{
		/// <summary>
		/// Loads all the components of the screen
		/// </summary>
		void LoadContent();
		/// <summary>
		/// Updates a frame of the screen
		/// </summary>
		/// <param name="gt">Denotes an instant of time in Monogame</param>
		void Update(GameTime gt);
		/// <summary>
		/// Draws a frame of a game
		/// </summary>
		/// <param name="gt">Denotes an instant of time in Monogame</param>
		void Draw(GameTime gt);
	}
	/// <summary>
	/// Azuxiren Monogame Game Class: Standard Template for Monogame Game Class.
	/// 
	/// Consists of two IScreen objects : CurrentScreen and LoadingScreen.
	/// 
	/// CurrentScreen is the screen that is alwaus being updated and drawn except when loading a new screen
	/// 
	/// At that time, a new IScreen object is instaniated and called  LoadContent() as a parallel task. 
	/// 
	/// Needless to say, this is when the LoadingScreen object is updated and drawn
	/// 
	/// ----------
	/// 
	/// Please use base.LoadContent() methods on overriding of LoadContent() method
	/// 
	/// Use the base Constructor and initalize the CurrentScreen and LoadingScreen objects
	/// </summary>
	public abstract class AMGC<StartScreen,LoadScreen> : Game
        where StartScreen:IScreen,new()
        where LoadScreen:IScreen,new()
	{
		/// <summary>Manages the graphic setting fot the game</summary>
		public GraphicsDeviceManager graphics;
		internal bool isLoading;
        internal IScreen CurrentScreen,LoadingScreen;
        /// <summary>
        /// The Default Constructor
        /// </summary>
		public AMGC()
        {
            graphics = new GraphicsDeviceManager(this);
			isLoading = false;
		}
        /// <summary>
        /// The Provided Initializer for Monogame. This is where the LoadContent method of each IScreen types are done.
        /// </summary>
        protected override void Initialize()
        {
            CurrentScreen = new StartScreen();
            LoadingScreen = new LoadScreen();
            base.Initialize();
        }
        /// <summary>
        /// Loads the CurrentScreen and LoadingScreen
        /// </summary>
        protected override void LoadContent()
		{
			CurrentScreen.LoadContent();
			LoadingScreen.LoadContent();
			base.LoadContent();
		}
		/// <summary>This will set the screen as FullScreen with the default Screen Size</summary>
		public virtual void SetFullScreen() => SetFullScreen(GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height);
		/// <summary>
		/// This will Set the Screen as FullScreen with the given Width/Height
		/// </summary>
		/// <param name="w">The Width to occupy</param>
		/// <param name="h">The Height to cover</param>
		public virtual void SetFullScreen(int w, int h)
		{
			graphics.PreferredBackBufferWidth = w;
			graphics.PreferredBackBufferHeight = h;
			graphics.IsFullScreen = true;
			graphics.ApplyChanges();
		}
		/// <summary>
		/// This will set The Screen as windowed with the given width/height
		/// </summary>
		/// <param name="w">The width of window</param>
		/// <param name="h">The height of window</param>
		public virtual void RevertFullScreen(int w, int h)
		{
			graphics.PreferredBackBufferWidth = w;
			graphics.PreferredBackBufferHeight = h;
			graphics.IsFullScreen = false;
			graphics.ApplyChanges();
		}
		/// <summary>
		/// The Draw method implementation for CFMG
		/// </summary>
		/// <param name="gt">Denotes an instant in time</param>
		protected override void Draw(GameTime gt)
		{
			if (isLoading) LoadingScreen.Draw(gt);
			else CurrentScreen.Draw(gt);
			base.Draw(gt);
		}
		/// <summary>
		/// The Update method implementation for CFMG
		/// </summary>
		/// <param name="gameTime">Denotes an instant in time</param>
		protected override void Update(GameTime gameTime)
		{
			if (isLoading) LoadingScreen.Update(gameTime);
			else CurrentScreen.Update(gameTime);
			base.Update(gameTime);
		}
		///<summary>
		/// 	THIS METHOD IS NOW OBSOLETE!
		///
		///   	the LoadingScreen is now drawn/updated while the parameter IScreen object is first Loaded (LoadContent() is done) and then replaces the earlier CurrentScreen</summary>
		/// <param name="screen">The IScreen object to replace the CurrentScreen</param>
		[Obsolete("This methods is now obsolete. Please use ScreenLoad<IScreen>() method for better performance")]
		public void ScreenLoad(IScreen screen)
		{
			isLoading = true;
			Task.Run(() => TaskPerform());
			void TaskPerform()
			{
				screen.LoadContent();
				CurrentScreen = screen;
				isLoading = false;
			}
		}
		/// <summary>
		/// the LoadingScreen is now drawn/updated while the parameter IScreen object is first Loaded (LoadContent() is done) and then replaces the earlier CurrentScreen
		/// </summary>
		/// <typeparam name="T">The Screen to load</typeparam>
		public void ScreenLoad<T>() where T : IScreen, new()
		{
			isLoading = true;
			Task.Run(() =>
				{
					var screen = new T();
					screen.LoadContent();
					CurrentScreen = screen;
					isLoading = false;
				}
			);
		}
	}
	/// <summary>
	/// Global class for all-weather running
	/// </summary>
	public static partial class Global
	{
		/// <summary>Shortcut to draw part of a image(Texture2D)</summary>
		/// <param name="tex">The Texture2D to draw</param>
		/// <param name="source">The portion of image to draw</param>
		/// <param name="Dest">The position on screen of drawing</param>
		/// <param name="sb">The SpriteBatch Object</param>
		/// <param name="c">Tint color (by default white)</param>
		public static void Draw(this Texture2D tex, Rectangle source, Rectangle Dest, SpriteBatch sb, Color? c = null) => sb.Draw(tex, source, Dest, c ?? Color.White);
		/// <summary>Shortcut to draw the image(Texture2D)</summary>
		/// <param name="tex">The image to draw</param>
		/// <param name="Dest">The position on screen to draw</param>
		/// <param name="sb">The SpriteBatch Object</param>
		/// <param name="c">Tint color (by default White)</param>
		public static void Draw(this Texture2D tex, Rectangle Dest, SpriteBatch sb, Color? c = null) => sb.Draw(tex, Dest, c ?? Color.White);
		/// <summary> Shortcut to draw an image on a Vector2D</summary>
		/// <param name="tex">The image to draw</param>
		/// <param name="Dest">The Position to draw</param>
		/// <param name="sb">The SpriteBatch Object</param>
		/// <param name="c">Tint color (by default White)</param>
		public static void Draw(this Texture2D tex, Vector2 Dest, SpriteBatch sb, Color? c = null) => sb.Draw(tex, Dest, c ?? Color.White);
		/// <summary>Writes a String on display at the provided Rectangle</summary>
		/// <param name="font">Spritefont object</param>
		/// <param name="sb">The SpriteBatch Object</param>
		/// <param name="Dest">The Rectangle position to draw at</param>
		/// <param name="Message">The content of the string</param>
		/// <param name="c">Tint color (by default White)</param>
		public static void Write(this SpriteFont font, SpriteBatch sb, Rectangle Dest, string Message, Color? c = null)
		{
			Vector2 size = font.MeasureString(Message);
			float xScale = Dest.Width / size.X;
			float yScale = Dest.Height / size.Y;
			float scale = Math.Min(xScale, yScale);
			int strWidth = (int)Math.Round(size.X * scale);
			int strHeight = (int)Math.Round(size.Y * scale);
			Vector2 position = new Vector2
			{
				X = ((Dest.Width - strWidth) / 2) + Dest.X,
				Y = ((Dest.Height - strHeight) / 2) + Dest.Y
			};
			float rotation = 0.0f;
			Vector2 spriteOrigin = new Vector2(0, 0);
			float spriteLayer = 0.0f; // all the way in the front
			SpriteEffects spriteEffects = SpriteEffects.None;
			Color cc = c ?? Color.White;// Draw the string to the sprite batch!
			sb.DrawString(font, Message, position, cc, rotation, spriteOrigin, scale, spriteEffects, spriteLayer);
		}
		/// <summary>
		/// Sets rectangle a such that a and b share the same center
		/// </summary>
		/// <param name="a">The rectangle to shift</param>
		/// <param name="b">The rectangle whose center is to be taken reference of</param>
		public static void SetCenter(ref Rectangle a, Rectangle b) => SetCenter(ref a, b.Center);
		/// <summary>
		/// Sets rectangle a such that a and b share the same center
		/// </summary>
		/// <param name="a">The rectangle to shift</param>
		/// <param name="Center">The Point to be taken reference of</param>
		public static Point SetCenter(ref Rectangle a, Point Center)
		{
			a.X = Center.X - (a.Width / 2);
			a.Y = Center.Y - (a.Height / 2);
			return a.Location;
		}
		/// <summary>
		/// Sets rectangle a such that a is fitted inside b and both same the same center
		/// </summary>
		/// <param name="a">Rectangle to scale and shift</param>
		/// <param name="b">Rectangle taken as reference</param>
		/// <returns>The final location of the rectangle a</returns>
		public static Point SetCenterScaled(ref Rectangle a, Rectangle b)
		{
			float xScale = b.Width / (float)a.Width;
			float yScale = b.Height / (float)a.Height;
			xScale = xScale < yScale ? xScale : yScale;
			a.Width = (int)(xScale * a.Width);
			a.Height = (int)(xScale * a.Height);
			SetCenter(ref a, b);
			return a.Location;
		}
		/// <summary>Initalizes the Vector2 and Float value of poition and scale to fit the text in the rectangle</summary>
		/// <param name="Dest">The rectangle to fit the string</param>
		/// <param name="font">The Spritefont object</param>
		/// <param name="Message">The content of the string</param>
		/// <param name="scale">The scale to fit</param>
		/// <param name="position">The position of fitting</param>
		public static void FitText(this Rectangle Dest, SpriteFont font, string Message, out float scale, out Vector2 position)
		{
			Vector2 size = font.MeasureString(Message);
			float xScale = Dest.Width / size.X;
			float yScale = Dest.Height / size.Y;
			// Taking the smaller scaling value will result in the text always fitting in the boundaires.
			scale = Math.Min(xScale, yScale);
			// Figure out the location to absolutely-center it in the boundaries rectangle.
			int strWidth = (int)Math.Round(size.X * scale);
			int strHeight = (int)Math.Round(size.Y * scale);
            Rectangle xx = new Rectangle(0, 0, strWidth, strHeight);
            SetCenter(ref xx, Dest.Center);
            position = xx.Location.ToVector2();
		}
	}
}