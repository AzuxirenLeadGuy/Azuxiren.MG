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
	public abstract class AMGC<StartScreen, LoadScreen> : Game
		where StartScreen : IScreen, new()
		where LoadScreen : IScreen, new()
	{
		/// <summary>Manages the graphic setting fot the game</summary>
		public GraphicsDeviceManager GraphicsDM;
		internal bool IsLoading;
		internal IScreen CurrentScreen, LoadingScreen;
		/// <summary>
		/// The Default Constructor
		/// </summary>
		public AMGC()
		{
			GraphicsDM = new GraphicsDeviceManager(this);
			IsLoading = false;
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
			GraphicsDM.PreferredBackBufferWidth = w;
			GraphicsDM.PreferredBackBufferHeight = h;
			GraphicsDM.IsFullScreen = true;
			GraphicsDM.ApplyChanges();
		}
		/// <summary>
		/// This will set The Screen as windowed with the given width/height
		/// </summary>
		/// <param name="w">The width of window</param>
		/// <param name="h">The height of window</param>
		public virtual void RevertFullScreen(int w, int h)
		{
			GraphicsDM.PreferredBackBufferWidth = w;
			GraphicsDM.PreferredBackBufferHeight = h;
			GraphicsDM.IsFullScreen = false;
			GraphicsDM.ApplyChanges();
		}
		/// <summary>
		/// The Draw method implementation for CFMG
		/// </summary>
		/// <param name="gt">Denotes an instant in time</param>
		protected override void Draw(GameTime gt)
		{
			if (IsLoading) LoadingScreen.Draw(gt);
			else CurrentScreen.Draw(gt);
			base.Draw(gt);
		}
		/// <summary>
		/// The Update method implementation for CFMG
		/// </summary>
		/// <param name="gameTime">Denotes an instant in time</param>
		protected override void Update(GameTime gameTime)
		{
			if (IsLoading) LoadingScreen.Update(gameTime);
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
			IsLoading = true;
			Task.Run(() => TaskPerform());
			void TaskPerform()
			{
				screen.LoadContent();
				CurrentScreen = screen;
				IsLoading = false;
			}
		}
		/// <summary>
		/// the LoadingScreen is now drawn/updated while the parameter IScreen object is first Loaded (LoadContent() is done) and then replaces the earlier CurrentScreen
		/// </summary>
		/// <typeparam name="T">The Screen to load</typeparam>
		public void ScreenLoad<T>() where T : IScreen, new()
		{
			IsLoading = true;
			Task.Run(() =>
				{
					var screen = new T();
					screen.LoadContent();
					CurrentScreen = screen;
					IsLoading = false;
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
		/// <param name="dest">The position on screen of drawing</param>
		/// <param name="sb">The SpriteBatch Object</param>
		/// <param name="c">Tint color (by default white)</param>
		public static void Draw(this Texture2D tex, Rectangle source, Rectangle dest, SpriteBatch sb, Color? c = null) => sb.Draw(tex, source, dest, c ?? Color.White);
		/// <summary>Shortcut to draw the image(Texture2D)</summary>
		/// <param name="tex">The image to draw</param>
		/// <param name="dest">The position on screen to draw</param>
		/// <param name="sb">The SpriteBatch Object</param>
		/// <param name="c">Tint color (by default White)</param>
		public static void Draw(this Texture2D tex, Rectangle dest, SpriteBatch sb, Color? c = null) => sb.Draw(tex, dest, c ?? Color.White);
		/// <summary> Shortcut to draw an image on a Vector2D</summary>
		/// <param name="tex">The image to draw</param>
		/// <param name="dest">The Position to draw</param>
		/// <param name="sb">The SpriteBatch Object</param>
		/// <param name="c">Tint color (by default White)</param>
		public static void Draw(this Texture2D tex, Vector2 dest, SpriteBatch sb, Color? c = null) => sb.Draw(tex, dest, c ?? Color.White);
		/// <summary>Writes a String on display at the provided Rectangle</summary>
		/// <param name="font">Spritefont object</param>
		/// <param name="sb">The SpriteBatch Object</param>
		/// <param name="dest">The Rectangle position to draw at</param>
		/// <param name="message">The content of the string</param>
		/// <param name="c">Tint color (by default White)</param>
		public static void Write(this SpriteFont font, SpriteBatch sb, Rectangle dest, string message, Color? c = null)
		{
			Vector2 size = font.MeasureString(message);
			float xScale = dest.Width / size.X;
			float yScale = dest.Height / size.Y;
			float scale = Math.Min(xScale, yScale);
			int strWidth = (int)Math.Round(size.X * scale);
			int strHeight = (int)Math.Round(size.Y * scale);
			Vector2 position = new Vector2
			{
				X = ((dest.Width - strWidth) / 2) + dest.X,
				Y = ((dest.Height - strHeight) / 2) + dest.Y
			};
			float rotation = 0.0f;
			Vector2 spriteOrigin = new Vector2(0, 0);
			float spriteLayer = 0.0f; // all the way in the front
			SpriteEffects spriteEffects = SpriteEffects.None;
			Color cc = c ?? Color.White;// Draw the string to the sprite batch!
			sb.DrawString(font, message, position, cc, rotation, spriteOrigin, scale, spriteEffects, spriteLayer);
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
		/// <param name="center">The Point to be taken reference of</param>
		public static Point SetCenter(ref Rectangle a, Point center)
		{
			a.X = center.X - (a.Width / 2);
			a.Y = center.Y - (a.Height / 2);
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
		/// <param name="dest">The rectangle to fit the string</param>
		/// <param name="font">The Spritefont object</param>
		/// <param name="message">The content of the string</param>
		/// <param name="scale">The scale to fit</param>
		/// <param name="position">The position of fitting</param>
		public static void FitText(this Rectangle dest, SpriteFont font, string message, out float scale, out Vector2 position)
		{
			Vector2 size = font.MeasureString(message);
			float xScale = dest.Width / size.X;
			float yScale = dest.Height / size.Y;
			// Taking the smaller scaling value will result in the text always fitting in the boundaires.
			scale = Math.Min(xScale, yScale);
			// Figure out the location to absolutely-center it in the boundaries rectangle.
			position = dest.Location.ToVector2();
		}
		/// <summary>
		/// Draws the StringObject Instance with the default settings
		/// </summary>
		/// <param name="batch">Spritebatch object</param>
		/// <param name="stringobj">The instance of StringObject</param>
		public static void Draw(this SpriteBatch batch, TextBox stringobj) => stringobj.Draw(batch);
		/// <summary>
		/// Draws the StringObject Instance
		/// </summary>
		/// <param name="batch">SpriteBatch object</param>
		/// <param name="stringObject">The instance of StringObject</param>
		/// <param name="rotation">The angle of rotation</param>
		/// <param name="origin">The origin about which rotation takes place</param>
		public static void Draw(this SpriteBatch batch, TextBox stringObject, float rotation, Vector2 origin) => stringObject.Draw(batch, rotation, origin);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="batch"></param>
		/// <param name="stringObject"></param>
		/// <param name="rotation"></param>
		/// <param name="origin"></param>
		/// <param name="effects"></param>
		public static void Draw(this SpriteBatch batch, TextBox stringObject, float rotation, Vector2 origin, SpriteEffects effects) => stringObject.Draw(batch, rotation, origin, effects);
		/// <summary>
		/// Updates an object for the given acceleration and friction
		/// </summary>
		/// <param name="obj2D">The IPhyObj2D object</param>
		/// <param name="acc">The acceleration acting on it</param>
		/// <param name="friction">The friction on the body</param>
		public static void Update(this IPhyObj2D obj2D, Vector2 acc, float friction = 0)
		{
			var (v, x) = obj2D.Current;
			var v_vec = new Vector2(v.X, v.Y);
			var x_vec = new Vector2(x.X, x.Y);
			v_vec += acc - (v * friction);
			x_vec += v;
			obj2D.Current = (v_vec, x_vec);
		}
		/// <summary>
		/// Updates an object for the given acceleration and friction
		/// </summary>
		/// <param name="current">The (Vector2,Vector2) Tuple object with elements Velocity and Displacement respectivly</param>
		/// <param name="acc">The acceleration acting on it</param>
		/// <param name="friction">The friction on the body</param>
		public static (Vector2 Velocity, Vector2 Position) Update(this (Vector2 Velocity, Vector2 Position) current, Vector2 acc, float friction = 0)
		{
			var (v, x) = current;
			var v_vec = new Vector2(v.X, v.Y);
			var x_vec = new Vector2(x.X, x.Y);
			v_vec += acc - (x * friction);
			x_vec += x;
			return (v_vec, x_vec);
		}
		/// <summary>
		/// Updates an object when no acceleration is acting upon it
		/// </summary>
		/// <param name="obj2D">The IPhyObj2D object</param>
		/// <param name="friction">The friction acting upon it</param>
		public static void Update(this IPhyObj2D obj2D, float friction = 0) => Update(obj2D, Vector2.Zero, friction);
		/// <summary>
		/// Updates an object when no acceleration is acting upon it
		/// </summary>
		/// <param name="current">The (Vector2,Vector2) Tuple object with elements Velocity and Displacement respectivly</param>
		/// <param name="friction">The friction acting upon it</param>
		public static (Vector2 Velocity, Vector2 Position) Update(this (Vector2 Velocity, Vector2 Position) current, float friction = 0) => Update(current, Vector2.Zero, friction);
		/// <summary>
		/// Updates an object when acted upon with the given acceleration and friction
		/// </summary>
		/// <param name="obj3D">The IPhyObj3D</param>
		/// <param name="acc">The acceleration acting upon it</param>
		/// <param name="friction">The friction on the object</param>
		public static void Update(this IPhyObj3D obj3D, Vector3 acc, float friction = 0)
		{
			var (v, x) = obj3D.Current;
			var v_vec = new Vector3(v.X, v.Y, v.Z);
			var x_vec = new Vector3(x.X, x.Y, x.Z);
			v_vec += acc - (v * friction);
			x_vec += v;
			obj3D.Current = (v_vec, x_vec);
		}
		/// <summary>
		/// Updates an object when acted upon with the given acceleration and friction
		/// </summary>
		/// <param name="current">The (Vector3,Vector3) Tuple object with elements Velocity and Displacement respectivly</param>
		/// <param name="acc">The acceleration acting upon it</param>
		/// <param name="friction">The friction on the object</param>
		public static (Vector3 Velocity, Vector3 Position) Update(this (Vector3 Velcoity, Vector3 Position) current, Vector3 acc, float friction = 0)
		{
			var (v, x) = current;
			var v_vec = new Vector3(v.X, v.Y, v.Z);
			var x_vec = new Vector3(x.X, x.Y, x.Z);
			v_vec += acc - (v * friction);
			x_vec += v;
			return (v_vec, x_vec);
		}
		/// <summary>
		/// Updates an object when no acceleration acts on it
		/// </summary>
		/// <param name="obj3D">The IPhyObj3D object</param>
		/// <param name="friction">The friction on the body</param>
		public static void Update(this IPhyObj3D obj3D, float friction = 0) => Update(obj3D, Vector3.Zero, friction);
		/// <summary>
		/// Updates an object when no acceleration acts on it
		/// </summary>
		/// <param name="current">The (Vector3,Vector3) Tuple object with elements Velocity and Displacement respectivly </param>
		/// <param name="friction">The friction on the body</param>
		public static (Vector3 Velocity, Vector3 Position) Update(this (Vector3 Velocity, Vector3 Position) current, float friction = 0) => Update(current, Vector3.Zero, friction);
	}
}