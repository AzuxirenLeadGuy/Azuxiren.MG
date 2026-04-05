using Azuxiren.MG.Drawing;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Azuxiren.MG.Components;

/// <summary>
/// A drawing toolkit to render to a RenderTarget2D, 
/// which can be further drawn to any screen size
/// </summary>
public class RenderTargetDrawer : IBatchDrawer
{
	/// <summary>Creates an instance of a RenderTargetDrawer</summary>
	/// <param name="gd">The GraphicsDevice reference</param>
	/// <param name="width">The width of the internal RenderTarget</param>
	/// <param name="height">The height of the internal RenderTarget</param>
	internal RenderTargetDrawer(GraphicsDevice gd, int width, int height)
	{
		_graphicsDevice = gd;
		_target2D = new(_graphicsDevice, width, height);
		_spriteBatch = new(_graphicsDevice);
	}

	/// <summary>The clear color to use for the RenderTarget</summary>
	public Color Clear = Color.White;

	/// <summary>The clear color to use for the entire window, excluding RenderTarget</summary>
	public Color OutofBounds = Color.Black;

	/// <summary>The color to tint the final RenderTarget2D destination</summary>
	public Color TargetTint = Color.White;

	/// <summary>The instance of RenderTarget2D</summary>
	private readonly RenderTarget2D _target2D;

	/// <summary>The reference to the GraphicsDevice object</summary>
	private readonly GraphicsDevice _graphicsDevice;

	/// <summary>The internal spritebatch that will be utilized</summary>
	private readonly SpriteBatch _spriteBatch;

	/// <summary>The Destination Rectangle where the RenderTarget will be drawn to screen</summary>
	public Rectangle DestinationRect { get; private set; }

	/// <summary>The size of the internal RenderTarget Screen</summary>
	public Rectangle Bounds => _target2D.Bounds;

	/// <summary>
	/// Updates the resolution of the destination screen, 
	/// as detected in the GraphicsDevice reference
	/// </summary>
	internal void UpdateResolution()
	{
		var screen_size = _graphicsDevice.PresentationParameters.Bounds;
		DestinationRect = DrawingExtensions.SetCenterScaled(
			_target2D.Bounds.Size,
			screen_size
		);
	}

	/// <summary>Set the target for drawing</summary>
	internal void BeginTargetDraw()
	{
		_graphicsDevice.SetRenderTarget(_target2D);
		_graphicsDevice.Clear(Clear);
	}

	/// <summary>Passes the drawing spritebatch reference for custom drawing</summary>
	/// <param name="drawFunc">The custom drawing function</param>
	/// <param name="camera">The 2D camera to be used for drawing</param>
	/// <param name="sortMode">The sorting mode to use</param>
	/// <param name="blendState">The blend state to use</param>
	/// <param name="samplerState">The sampler state to use</param>
	/// <param name="stencilState">The stencil state to use</param>
	/// <param name="rasterState">The raster state to use</param>
	/// <param name="effect">The effect to use</param>
	public void DrawToTarget(
		System.Action<IBatchDrawer> drawFunc,
		Camera2D camera,
		SpriteSortMode sortMode = SpriteSortMode.Deferred,
		BlendState? blendState = null,
		SamplerState? samplerState = null,
		DepthStencilState? stencilState = null,
		RasterizerState? rasterState = null,
		Effect? effect = null
	)
	{
		var default_viewport = _graphicsDevice.Viewport;
		_graphicsDevice.Viewport = new(camera.Viewport);
		this.DrawToTarget(
			drawFunc,
			sortMode,
			blendState,
			samplerState,
			stencilState,
			rasterState,
			effect,
			camera.Transform
		);
		_graphicsDevice.Viewport = default_viewport;
	}


	/// <summary>Passes the drawing spritebatch reference for custom drawing</summary>
	/// <param name="drawFunc">The custom drawing function</param>
	/// <param name="sortMode">The sorting mode to use</param>
	/// <param name="blendState">The blend state to use</param>
	/// <param name="samplerState">The sampler state to use</param>
	/// <param name="stencilState">The stencil state to use</param>
	/// <param name="rasterState">The raster state to use</param>
	/// <param name="effect">The effect to use</param>
	/// <param name="transform">The transformation matrix to use</param>
	public void DrawToTarget(
		System.Action<IBatchDrawer> drawFunc,
		SpriteSortMode sortMode = SpriteSortMode.Deferred,
		BlendState? blendState = null,
		SamplerState? samplerState = null,
		DepthStencilState? stencilState = null,
		RasterizerState? rasterState = null,
		Effect? effect = null,
		Matrix? transform = null
	)
	{
		_spriteBatch.Begin(
			sortMode,
			blendState,
			samplerState,
			stencilState,
			rasterState,
			effect,
			transform
		);
		drawFunc(this);
		_spriteBatch.End();
	}

	/// <summary>
	/// Draw all the contents of the rendertarget to screen, 
	/// and clears the target content. Should be called 
	/// once per draw cycle of the game.
	/// </summary>
	internal void EndTargetDraw()
	{
		_graphicsDevice.SetRenderTarget(null);
		_graphicsDevice.Clear(OutofBounds);
		_spriteBatch.Begin();
		_spriteBatch.Draw(_target2D, DestinationRect, TargetTint);
		_spriteBatch.End();
	}

	/// <inheritdoc/>
	void IBatchDrawer.Draw(
		Texture2D texture,
		Vector2 position,
		Rectangle? sourceRectangle,
		Color? color,
		Vector2? scale,
		Vector2 origin,
		float rotation,
		SpriteEffects effects,
		float layerDepth
	) => _spriteBatch.Draw(
			texture,
			position,
			sourceRectangle,
			color ?? Color.White,
			rotation,
			origin,
			scale ?? Vector2.One,
			effects,
			layerDepth
		);

	/// <inheritdoc/>
	void IBatchDrawer.Draw(
		Texture2D texture,
		Rectangle destination,
		Rectangle? sourceRectangle,
		Color? color,
		Vector2 origin,
		float rotation,
		SpriteEffects effects,
		float layerDepth
	) => _spriteBatch.Draw(
		texture,
		destination,
		sourceRectangle,
		color ?? Color.White,
		rotation,
		origin,
		effects,
		layerDepth
	);

	/// <inheritdoc/>
	void IBatchDrawer.DrawString(
		SpriteFont spriteFont,
		string text,
		Vector2 position,
		Color color,
		float rotation,
		Vector2? origin,
		Vector2? scale,
		SpriteEffects effects,
		float layerDepth
	) => _spriteBatch.DrawString(
		spriteFont,
		text,
		position,
		color,
		rotation,
		origin ?? Vector2.Zero,
		scale ?? Vector2.One,
		effects,
		layerDepth
	);
}