using Microsoft.Xna.Framework;

namespace Azuxiren.MG;

/// <summary>
/// Common Game class wrapper for games
/// </summary>
/// <typeparam name="Parameters">Constant Parameter Type shared for the game</typeparam>
/// <typeparam name="Settings">Variable Setting Type shared between screens of the game</typeparam>
public abstract class AzuxirenMonogameClass<Parameters, Settings> : Game
where Parameters : class
{
	/// <summary>The constant Parameters of the game</summary>
	public readonly Parameters GameParameters;
	/// <summary>
	/// The shared setting of the game. This object
	/// is shared between the multipls GameStages
	/// in the lifetime of this object
	/// </summary>
	protected Settings _settings;
	/// <summary>The graphics device manager for this game</summary>
	public readonly GraphicsDeviceManager GraphicsDM;
	/// <summary>Indicates whether a screen is being loaded or not</summary>
	protected volatile bool _isLoading;
	/// <summary>The game stages involved this game</summary>
	protected IGameStage<Parameters, Settings> _loadScreen, _mainScreen;
	/// <summary>
	/// Initializes the game object
	/// </summary>
	/// <param name="parameters">The constant parameters for this game</param>
	/// <param name="startScreen">The screen to begin the game with</param>
	/// <param name="loadScreen">The screen for loading, to be shown during transitions</param>
	protected AzuxirenMonogameClass(
		Parameters parameters,
		IGameStage<Parameters, Settings> startScreen,
		IGameStage<Parameters, Settings> loadScreen)
	{
		GameParameters = parameters;
		_settings = default!;
		GraphicsDM = new(this);
		_isLoading = false;
		_loadScreen = loadScreen;
		_mainScreen = startScreen;
	}
	/// <summary>Prepares the initial loaded setting based on the game</summary>
	/// <returns>The Loaded Setting object</returns>
	protected abstract Settings LoadInitialSetting();
	/// <summary>
	/// Loads the Content for both the screens
	/// </summary>
	protected override void LoadContent()
	{
		_settings = LoadInitialSetting();
		_mainScreen.LoadContent(GameParameters, ref _settings);
		_loadScreen.LoadContent(GameParameters, ref _settings);
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
		if (_isLoading) _loadScreen.Draw(gt, GameParameters, _settings);
		else _mainScreen.Draw(gt, GameParameters, _settings);
		base.Draw(gt);
	}
	/// <inheritdoc/>
	protected override void Update(GameTime gameTime)
	{
		if (_isLoading)
		{
			_ = _loadScreen.Update(
			gameTime,
			GameParameters,
			ref _settings,
			out var _);
		}
		else if (_mainScreen.Update(gameTime, GameParameters, ref _settings, out var nextStage))
		{
			if (nextStage == null) { Exit(); }
			else
			{
				_isLoading = true;
				_mainScreen = nextStage;
				_ = System.Threading.Tasks.Task.Run(
					() =>
					{
						_mainScreen.LoadContent(GameParameters, ref _settings);
						_isLoading = false;
					}
				);
			}
		}
		base.Update(gameTime);
	}
}