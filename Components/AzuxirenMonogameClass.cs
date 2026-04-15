using System;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Azuxiren.MG.Components;

/// <summary>Common Game class wrapper for games</summary>
/// <typeparam name="Settings">Variable Setting Type shared between screens of the game</typeparam>
public class AzuxirenMonogameClass<Settings> : Game, IMgRuntime
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

	/// <summary>The color used to clear the final window during the render phase</summary>
	public Color ScreenClearColor { get; set; } = Color.Black;

	/// <summary>The color used to clear the RenderTarget during the intermediate drawing phase</summary>
	public Color TargetClearColor { get; set; } = Color.White;

	/// <summary>The color to tint/multiply the target to the screen</summary>
	public Color TargetTintColor { get; set; } = Color.White;

	/// <summary>The size of the RenderTarget</summary>
	public Point TargetSize => _targetDrawer.Bounds.Size;
	/// <inheritdoc/>
	public Rectangle WindowClientBounds => Window.ClientBounds;

	/// <summary> Initializes the game object</summary>
	/// <param name="targetSize">The resolution of the game render target</param>
	/// <param name="factory">The unit that creates/initializes scenes dynamically</param>
	public AzuxirenMonogameClass(
		Point targetSize,
		GameStageFactory<Settings> factory
	)
	{
		GraphicsDM = new(this);
		_renderTargetSize = targetSize;
		_factory = factory;
		Window.AllowUserResizing = false;
		_settings = default!;
		_isLoading = false;
		_loadScreen = null!;
		_mainScreen = null!;
		_targetDrawer = default!;
		_taskLoader = Task.FromResult<IGameStage<Settings>?>(null);
		IsMouseVisible = true;
		Content.RootDirectory = "Content";
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
	protected virtual void SetFullScreen()
		=> SetFullScreen(
			GraphicsDevice.DisplayMode.Width,
			GraphicsDevice.DisplayMode.Height
		);

	/// <summary>This will Set the Screen as FullScreen with the given Width/Height</summary>
	/// <param name="w">The Width to occupy</param>
	/// <param name="h">The Height to cover</param>
	protected virtual void SetFullScreen(int w, int h)
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
	protected virtual void SetWindowed(int w, int h)
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
		_targetDrawer.BeginTargetDraw(TargetClearColor);
		if (_isLoading)
		{
			_loadScreen.Draw(gt, _targetDrawer, _settings);
		}
		else
		{
			_mainScreen.Draw(gt, _targetDrawer, _settings);
		}
		_targetDrawer.EndTargetDraw(ScreenClearColor, TargetTintColor);
		base.Draw(gt);
	}

	/// <summary>Updates the game for one frame</summary>
	/// <param name="gt">Denotes an instant in time</param>
	protected override async void Update(GameTime gt)
	{
		if (_isLoading)
		{
			_ = _loadScreen.Update(gt, this, ref _settings);
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
			GameUpdateResult result = _mainScreen.Update(gt, this, ref _settings);
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
				case GameUpdateResult.ResultType.SetTargetSize:
					(ushort width, ushort height) = GameUpdateResult.UnpackData(result.StageCode);
					_targetDrawer = new(GraphicsDevice, width, height);
					_mainScreen.Resize(this, new(width, height), ref _settings);
					_loadScreen.Resize(this, new(width, height), ref _settings);
					break;
				case GameUpdateResult.ResultType.SetWindowed:
				case GameUpdateResult.ResultType.SetBorderlessWindowed:
				case GameUpdateResult.ResultType.SetFullScreen:
				case GameUpdateResult.ResultType.SetBorderlessFullscreen:
				case GameUpdateResult.ResultType.NoAction:
				default:
					break;
			}
		}
		base.Update(gt);
	}
}