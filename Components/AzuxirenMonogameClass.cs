using System;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Azuxiren.MG.Components;

/// <summary>Common Game class wrapper for games</summary>
/// <typeparam name="Settings">Variable Setting Type shared between screens of the game</typeparam>
public class AzuxirenMonogameClass<Settings> : Game
{
	/// <summary>The custom unit that initializes the game stages</summary>
	protected readonly GameStageFactory<Settings> _factory;

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
	protected bool _isLoading;

	/// <summary>The game stages involved this game</summary>
	protected IGameStage<Settings> _loadScreen, _mainScreen;

	/// <summary>Stores the size of the render target</summary>
	protected Point _renderTargetSize;

	/// <summary>Stores the size of the render target</summary>
	protected Task<IGameStage<Settings>?> _taskLoader;

	/// <summary> Initializes the game object</summary>
	/// <param name="targetSize">The resolution of the game render target</param>
	/// <param name="factory">The unit that creates/initializes scenes dynamically</param>
	/// <param name="allow_window_resize">Allow the window to be resized</param>
	public AzuxirenMonogameClass(Point targetSize, GameStageFactory<Settings> factory, bool allow_window_resize=false)
	{
		GraphicsDM = new(this);
		_settings = default!;
		_isLoading = false;
		_factory = factory;
		_renderTargetSize = targetSize;
		_loadScreen = null!;
		_mainScreen = null!;
		_targetDrawer = default!;
		_taskLoader = Task.FromResult<IGameStage<Settings>?>(null);
		Window.AllowUserResizing = allow_window_resize;
		IsMouseVisible = true;
	}

	/// <summary>Loads the Content for both the screens </summary>
	protected override void LoadContent()
	{
		_targetDrawer = new(GraphicsDevice, _renderTargetSize.X, _renderTargetSize.Y);
		SetWindowed(_renderTargetSize.X, _renderTargetSize.Y);
		_settings = _factory.InitializeSettings(this);
		_mainScreen = _factory.StartStage(this, _settings);
		_loadScreen = _factory.LoadStage(this, _settings);
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
		_targetDrawer.UpdateResolution();
	}

	/// <summary>This will set The Screen as windowed with the given width/height</summary>
	/// <param name="w">The width of window</param>
	/// <param name="h">The height of window</param>
	public virtual void SetWindowed(int w, int h)
	{
		GraphicsDM.PreferredBackBufferWidth = w;
		GraphicsDM.PreferredBackBufferHeight = h;
		GraphicsDM.IsFullScreen = false;
		GraphicsDM.ApplyChanges();
		_targetDrawer.UpdateResolution();
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
	protected override async void Update(GameTime gt)
	{
		if (_isLoading)
		{
			_loadScreen.Update(gt, ref _settings);
			if (_taskLoader.IsCompleted)
			{
				_mainScreen = await _taskLoader ?? throw new InvalidOperationException(
					"The result of task factory is null"
				);
				_isLoading = false;
			}
		}
		else
		{
			var result = _mainScreen.Update(gt, ref _settings);
			switch (result.Type)
			{
				case GameUpdateResult.ResultType.Transition:
					_isLoading = true;
					_taskLoader = Task.Run(
						() => _factory.Create(result.StageCode, this, _settings)
					);
					break;
				case GameUpdateResult.ResultType.ExitRequest:
					Exit();
					break;
				case GameUpdateResult.ResultType.NoAction:
				default:
					break;
			}
		}
		base.Update(gt);
	}
}