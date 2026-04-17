using System;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Azuxiren.MG.Components;

/// <summary>Common Game class wrapper for games</summary>
/// <typeparam name="TSettings">Variable Setting Type shared between screens of the game</typeparam>
public class AzuxirenMonogameClass<TSettings> : Game, IMgRuntime
{
	/// <summary>drawing utilities for the game</summary>
	private RenderTargetDrawer _targetDrawer;

	/// <summary>The custom unit that initializes the game stages</summary>
	protected readonly IStageFactory<TSettings> _factory;

	/// <summary>
	/// The shared setting of the game. This object is shared between the 
	/// multipls GameStages in the lifetime of this object
	/// </summary>
	protected TSettings _settings;

	/// <summary>The graphics device manager for this game</summary>
	public readonly GraphicsDeviceManager GraphicsDM;

	/// <summary>Indicates whether a screen is being loaded or not</summary>
	protected bool _isLoading;

	/// <summary>The game stages involved this game</summary>
	protected IGameStage<TSettings> _loadScreen, _mainScreen;

	/// <summary>Stores the size of the render target</summary>
	protected Point _renderTargetSize;

	/// <summary>Stores the size of the render target</summary>
	protected Task<IGameStage<TSettings>?> _taskLoader;

	/// <summary>The color used to clear the final window during the render phase</summary>
	public Color ScreenClearColor { get; set; }

	/// <summary>The color used to clear the RenderTarget during the intermediate drawing phase</summary>
	public Color TargetClearColor { get; set; }

	/// <summary>The color to tint/multiply the target to the screen</summary>
	public Color TargetTintColor { get; set; }

	/// <summary>The size of the RenderTarget</summary>
	public Point TargetSize => _targetDrawer.Bounds.Size;
	/// <inheritdoc/>
	public Rectangle WindowClientBounds => Window.ClientBounds;

	/// <summary> Initializes the game object</summary>
	/// <param name="targetSize">The resolution of the game render target</param>
	/// <param name="factory">The unit that creates/initializes scenes dynamically</param>
	public AzuxirenMonogameClass(
		Point targetSize,
		IStageFactory<TSettings> factory
	)
	{
		if (targetSize.X <= 0 || targetSize.Y <= 0)
		{
			throw new ArgumentException(
				"Invalid target size. The targetSize must have postive area",
				nameof(targetSize)
			);
		}
		_renderTargetSize = targetSize;
		GraphicsDM = new(this);
		_factory = factory;
		Window.AllowUserResizing = false;
		_settings = default!;
		_isLoading = false;
		_loadScreen = null!;
		_mainScreen = null!;
		_targetDrawer = default!;
		_taskLoader = Task.FromResult<IGameStage<TSettings>?>(null);
		IsMouseVisible = true;
		Content.RootDirectory = "Content";
		ScreenClearColor = Color.White;
		TargetTintColor = Color.White;
		TargetClearColor = Color.White;
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
	/// <param name="width">The Width to occupy</param>
	/// <param name="height">The Height to cover</param>
	protected virtual void SetFullScreen(int width, int height)
	{
		GraphicsDM.PreferredBackBufferWidth = width;
		GraphicsDM.PreferredBackBufferHeight = height;
		GraphicsDM.IsFullScreen = true;
		GraphicsDM.ApplyChanges();
		_targetDrawer.UpdateResolution();
	}

	/// <summary>This will set The Screen as windowed with the given width/height</summary>
	/// <param name="width">The width of window</param>
	/// <param name="height">The height of window</param>
	protected virtual void SetWindowed(int width, int height)
	{
		GraphicsDM.PreferredBackBufferWidth = width;
		GraphicsDM.PreferredBackBufferHeight = height;
		GraphicsDM.IsFullScreen = false;
		GraphicsDM.ApplyChanges();
		_targetDrawer.UpdateResolution();
	}

	/// <summary>The Draw method implementation for CFMG</summary>
	/// <param name="gameTime">Denotes an instant in time</param>
	protected override void Draw(GameTime gameTime)
	{
		_targetDrawer.BeginTargetDraw(TargetClearColor);
		IGameStage<TSettings> screen = _isLoading ? _loadScreen : _mainScreen;
		screen.Draw(gameTime, _targetDrawer, _settings);
		_targetDrawer.EndTargetDraw(ScreenClearColor, TargetTintColor);
		base.Draw(gameTime);
	}

	/// <summary>Updates the game for one frame</summary>
	/// <param name="gameTime">Denotes an instant in time</param>
	protected override async void Update(GameTime gameTime)
	{
		if (_isLoading)
		{
			_ = _loadScreen.Update(gameTime, this, ref _settings);
			if (_taskLoader.IsCompleted)
			{
				_mainScreen = await _taskLoader.ConfigureAwait(false) ?? throw new InvalidOperationException(
					"The result of task factory is null"
				);
				_isLoading = false;
			}
		}
		else
		{
			GameUpdateResult result = _mainScreen.Update(gameTime, this, ref _settings);
			switch (result.Type)
			{
				case GameUpdateResult.ResultType.Transition:
					_isLoading = true;
					_taskLoader = Task.Run(
						() =>
						{
							_mainScreen.Dispose();
							return _factory.Create(result.StageCode, this, _settings);
						}
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
				case GameUpdateResult.ResultType.SetWindowed: // TODO
				case GameUpdateResult.ResultType.SetBorderlessWindowed:// TODO
				case GameUpdateResult.ResultType.SetFullScreen:// TODO
				case GameUpdateResult.ResultType.SetBorderlessFullscreen:// TODO
				case GameUpdateResult.ResultType.NoAction:// TODO
				default:
					break;
			}
		}
		base.Update(gameTime);
	}
	/// <inheritdoc/>
	protected override void Dispose(bool disposing)
	{
		if (_renderTargetSize.X > 0 || _renderTargetSize.Y > 0)
		{
			if (disposing)
			{
				_targetDrawer?.Dispose();
				GraphicsDM.Dispose();
				_loadScreen?.Dispose();
				_mainScreen?.Dispose();
			} // No Unmanaged resources here
			_renderTargetSize = Point.Zero;
		}
		base.Dispose(disposing);
	}
}