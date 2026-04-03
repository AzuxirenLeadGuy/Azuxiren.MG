using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Azuxiren.MG.Components;

/// <summary>Common Game class wrapper for games</summary>
/// <typeparam name="Settings">Variable Setting Type shared between screens of the game</typeparam>
public class AzuxirenMonogameClass<Settings> : Game
{
	/// <summary>
	/// The shared setting of the game. This object is shared between the 
	/// multipls GameStages in the lifetime of this object
	/// </summary>
	protected Settings _settings;

	/// <summary>drawing utilities for the game</summary>
	protected RenderTargetDrawer _targetDrawer;

	/// <summary>The graphics device manager for this game</summary>
	public readonly GraphicsDeviceManager GraphicsDM;

	/// <summary>Indicates whether a screen is being loaded or not</summary>
	protected volatile bool _isLoading;

	/// <summary>The game stages involved this game</summary>
	protected IGameStage<Settings> _loadScreen, _mainScreen;

	/// <summary>A function pointer to load the internal settings after the game is loaded</summary>
	protected readonly Func<AzuxirenMonogameClass<Settings>, Settings> _delayedLoader;

	/// <summary> Initializes the game object</summary>
	/// <param name="start">The screen to begin the game with</param>
	/// <param name="load">The screen for loading, to be shown during transitions</param>
	/// <param name="loader">The function for loading the Setting instance once the game is initialized</param>
	protected AzuxirenMonogameClass(
		IGameStage<Settings> start,
		IGameStage<Settings> load,
		Func<AzuxirenMonogameClass<Settings>, Settings> loader
	)
	{
		GraphicsDM = new(this);
		_settings = default!;
		_targetDrawer = default!;
		_isLoading = false;
		_loadScreen = load;
		_mainScreen = start;
		_delayedLoader = loader;
	}

	/// <summary>Loads the Content for both the screens </summary>
	protected override void LoadContent()
	{
		_targetDrawer = new(this.GraphicsDevice, 640, 480);
		_settings = _delayedLoader(this);
		_mainScreen.LoadContent(GraphicsDevice, this.Content, ref _settings);
		_loadScreen.LoadContent(GraphicsDevice, this.Content, ref _settings);
		base.LoadContent();
	}
	/// <summary>This will set the screen as FullScreen with the default Screen Size</summary>
	public virtual void SetFullScreen()
		=> SetFullScreen(
			GraphicsDevice.DisplayMode.Width,
			GraphicsDevice.DisplayMode.Height
		);

	/// <summary>This will Set the Screen as FullScreen with the given Width/Height</summary>
	/// <param name="w">The Width to occupy</param>
	/// <param name="h">The Height to cover</param>
	public virtual void SetFullScreen(int w, int h)
	{
		GraphicsDM.PreferredBackBufferWidth = w;
		GraphicsDM.PreferredBackBufferHeight = h;
		GraphicsDM.IsFullScreen = true;
		GraphicsDM.ApplyChanges();
	}

	/// <summary>This will set The Screen as windowed with the given width/height</summary>
	/// <param name="w">The width of window</param>
	/// <param name="h">The height of window</param>
	public virtual void RevertFullScreen(int w, int h)
	{
		GraphicsDM.PreferredBackBufferWidth = w;
		GraphicsDM.PreferredBackBufferHeight = h;
		GraphicsDM.IsFullScreen = false;
		GraphicsDM.ApplyChanges();
	}

	/// <summary>The Draw method implementation for CFMG</summary>
	/// <param name="gt">Denotes an instant in time</param>
	protected override void Draw(GameTime gt)
	{
		_targetDrawer.BeginTargetDraw();
		if (_isLoading) _loadScreen.Draw(gt, _targetDrawer, _settings);
		else _mainScreen.Draw(gt, _targetDrawer, _settings);
		_targetDrawer.EndTargetDraw();
		base.Draw(gt);
	}

	/// <summary>Updates the game for one frame</summary>
	/// <param name="gt">Denotes an instant in time</param>
	protected override void Update(GameTime gt)
	{
		if (_isLoading) { _loadScreen.Update(gt, ref _settings); }
		else
		{
			GameUpdateResult<Settings> result = _mainScreen.Update(gt, ref _settings);
			switch (result.Type)
			{
				case GameUpdateResult<Settings>.ResultType.Transition:
					if (result.NextStage == null)
						throw new Exception("Recieved null reference for transition IGameStage");
					_isLoading = true;
					_mainScreen = result.NextStage;
					_ = System.Threading.Tasks.Task.Run(
						() =>
						{
							_mainScreen.LoadContent(this.GraphicsDevice, this.Content, ref _settings);
							_isLoading = false;
						}
					);
					break;
				case GameUpdateResult<Settings>.ResultType.ExitRequest:
					Exit();
					break;
				case GameUpdateResult<Settings>.ResultType.NoAction:
				default:
					break;
			}
		}
		base.Update(gt);
	}
}