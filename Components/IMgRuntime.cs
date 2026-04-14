using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Azuxiren.MG.Components;

/// <summary>Represents the game details that game stages can access</summary>
public interface IMgRuntime
{
	/// <summary>The game content</summary>
	abstract ContentManager Content { get; }
	/// <summary>The graphics device settings for the game</summary>
	abstract GraphicsDevice GraphicsDevice { get; }
	/// <summary>The color used to clear the RenderTarget before drawing</summary>
	abstract Color TargetClearColor { get; set; }
	/// <summary>The color used to clear the screen before drawing the RenderTarget on it</summary>
	abstract Color ScreenClearColor { get; set; }
	/// <summary>The color used to apply additional tint on the RenderTarget drawing</summary>
	abstract Color TargetTintColor { get; set; }
	/// <summary>Set mouse visibility in the game</summary>
	abstract bool IsMouseVisible { get; set; }
	/// <summary>The size of the RenderTarget</summary>
	abstract Point TargetSize { get; }
	/// <summary>The window Bounds</summary>
	abstract Rectangle WindowClientBounds { get; }
}