using System;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Azuxiren.MG
{
	/// <summary>
	/// Azuxiren Monogame Game Class: Standard Template for Monogame Game Class.<br/>
	/// 
	/// Consists of two IScreen objects : CurrentScreen and LoadingScreen.<br/>
	/// 
	/// CurrentScreen is the screen that is always 
	/// being updated and drawn except when loading a new screen <br/>
	/// 
	/// At that time, a new IScreen object is instaniated and calls 
	/// LoadContent() as a parallel task.<br/> 
	/// 
	/// Needless to say, this is when the LoadingScreen object is updated and drawn.<br/>
	/// 
	/// <br/><br/>----------<br/>
	/// 
	/// Use the base Constructor and initalize the CurrentScreen and LoadingScreen objects
	/// </summary>
	[Obsolete("This class is now outdated. It works, but you should probably use the new AzuxirenMonogame")]
	public abstract class AMGC<StartScreen, LoadScreen> : Game
		where StartScreen : IScreen, new()
		where LoadScreen : IScreen, new()
	{
		/// <summary>Manages the graphic setting fot the game</summary>
		public GraphicsDeviceManager GraphicsDM;
		internal bool IsLoading;
		/// <summary>The screens being managed internally</summary>
		protected internal IScreen _currentScreen, _loadingScreen;
		/// <summary>
		/// The Default Constructor
		/// </summary>
		public AMGC()
		{
			GraphicsDM = new GraphicsDeviceManager(this);
			IsLoading = false;
			_currentScreen = null!;
			_loadingScreen = null!;
		}
		/// <summary>
		/// The Provided Initializer for Monogame. This is where the LoadContent 
		/// method of each IScreen types are done.
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();
		}
		/// <summary>
		/// Loads the CurrentScreen and LoadingScreen
		/// </summary>
		protected override void LoadContent()
		{
			_currentScreen = new StartScreen();
			_loadingScreen = new LoadScreen();
			_currentScreen.LoadContent();
			_loadingScreen.LoadContent();
			base.LoadContent();
		}
		/// <summary>This will set the screen as FullScreen with the default Screen Size</summary>
		public virtual void SetFullScreen()
			=> SetFullScreen(GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height);
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
			if (IsLoading) _loadingScreen.Draw(gt);
			else _currentScreen.Draw(gt);
			base.Draw(gt);
		}
		/// <summary>
		/// The Update method implementation for CFMG
		/// </summary>
		/// <param name="gameTime">Denotes an instant in time</param>
		protected override void Update(GameTime gameTime)
		{
			if (IsLoading) _loadingScreen.Update(gameTime);
			else _currentScreen.Update(gameTime);
			base.Update(gameTime);
		}
		/// <summary>
		/// The LoadingScreen is drawn/updated while the parameter IScreen object is first Loaded 
		/// (LoadContent() is called) and then replaces the earlier CurrentScreen
		/// </summary>
		/// <typeparam name="T">The Screen to load</typeparam>
		public void ScreenLoad<T>() where T : IScreen, new()
		{
			IsLoading = true;
			Task.Run(() =>
				{
					var screen = new T();
					screen.LoadContent();
					_currentScreen = screen;
					IsLoading = false;
				}
			);
		}
		/// <summary>
		/// 
		/// The LoadingScreen is drawn/updated while the parameter IScreen object is first 
		/// generated from the delegate, then loaded, (LoadContent() is called) and 
		/// then replaces the earlier CurrentScreen
		/// </summary>
		/// <param name="generator">A delegate to generate the IScreen instance</param>
		public void ScreenLoad(Func<IScreen> generator)
		{
			IsLoading = true;
			Task.Run(() =>
				{
					_currentScreen = generator.Invoke();
					_currentScreen.LoadContent();
					IsLoading = false;
				}
			);
		}
	}
}