using Microsoft.Xna.Framework;

namespace Azuxiren.MG;

/// <summary>
/// Represents a stage within the game
/// </summary>
/// <typeparam name="Parameters">Constant Parameter Type shared for the game</typeparam>
/// <typeparam name="Settings">Variable Setting Type shared between screens of the game</typeparam>
public interface IGameStage<Parameters, Settings>
{
	/// <summary>
	/// Loads the required content. This is a required 
	/// phase for transitioning between game stages.
	/// </summary>
	/// <param name="parameters">The object that contain all constant paramters for the game</param>
	/// <param name="settings">The shared settings for the game</param>
	public void LoadContent(in Parameters parameters, ref Settings settings);
	/// <summary>
	/// The logic for updating the components within the game. 
	/// It may alter any existing setting. <br/>
	/// Transitions to other GameStages must be handled here as well. <br/>
    /// If return value is false, no transition is performed<br/>
    /// If return value is true, and nextStage is null, then the game is exited/closed.<br/>
    /// If return value is true, and nextStage is not null, then the game performs transition
    /// (before loading in a parallel thread)
	/// </summary>
	/// <param name="gt">The GameTime object for this frame of the game</param>
	/// <param name="parameters">The object that contain all constant paramters for the game</param>
	/// <param name="settings">The shared settings for the game</param>
	/// <param name="nextStage">The nextStage for the game, if any. Otherwise null is returned</param>
	/// <returns>true the screen needs to be updated, or game needs to be exited/closed. Otherwise false</returns>
	public bool Update(GameTime gt, in Parameters parameters, ref Settings settings, out IGameStage<Parameters, Settings>? nextStage);
	/// <summary>
	/// The logic for drawing the components within the game. 
	/// It is not supposed to alter any existing setting
	/// </summary>
	/// <param name="gt">The GameTime object for this frame of the game</param>
	/// <param name="parameters">The object that contain all constant paramters for the game</param>
	/// <param name="settings">The shared settings for the game</param>
	public void Draw(GameTime gt, in Parameters parameters, in Settings settings);
}

/// <summary>
/// Common Game class wrapper for games 
/// </summary>
/// <typeparam name="Parameters">Constant Parameter Type shared for the game</typeparam>
/// <typeparam name="Settings">Variable Setting Type shared between screens of the game</typeparam>
public class AzuxirenMonogameClass<Parameters, Settings> : Game
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
	/// <param name="initalSettings">The initial settings of the game</param>
	/// <param name="startScreen">The screen to begin the game with</param>
	/// <param name="loadScreen">The screen for loading, to be shown during transitions</param>
	public AzuxirenMonogameClass(
		Parameters parameters,
		Settings initalSettings,
		IGameStage<Parameters, Settings> startScreen,
		IGameStage<Parameters, Settings> loadScreen)
	{
		GameParameters = parameters;
		_settings = initalSettings;
		GraphicsDM = new(this);
		_isLoading = false;
		_loadScreen = loadScreen;
		_mainScreen = startScreen;
	}
	/// <summary>
	/// Loads the Content for both the screens
	/// </summary>
	protected override void LoadContent()
	{
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
        if(_isLoading) _ = _loadScreen.Update(
            gameTime, 
            GameParameters, 
            ref _settings, 
            out var _);
        else if(_mainScreen.Update(gameTime, GameParameters, ref _settings, out var nextStage))
        {
            if(nextStage == null)
                Exit();
            else
            {
                _isLoading = true;
                _mainScreen = nextStage;
                _ = System.Threading.Tasks.Task.Run(
                    ()=>{
                        _mainScreen.LoadContent(GameParameters, ref _settings);
                        _isLoading = false;    
                    }
                );
            }
        }
		base.Update(gameTime);
	}
}