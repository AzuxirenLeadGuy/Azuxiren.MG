using System;

using Azuxiren.MG.Components;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Azuxiren.MG.Drawing;

/// <summary>The minimal functions of drawing that a game should handle</summary>
public interface IDrawHandler
{   /// <summary>Passes the drawing spritebatch reference for custom drawing</summary>
	/// <param name="drawFunc">The custom drawing function</param>
	/// <param name="camera">The 2D camera to be used for drawing</param>
	/// <param name="sortMode">The sorting mode to use</param>
	/// <param name="blendState">The blend state to use</param>
	/// <param name="samplerState">The sampler state to use</param>
	/// <param name="stencilState">The stencil state to use</param>
	/// <param name="rasterState">The raster state to use</param>
	/// <param name="effect">The effect to use</param>
	void DrawToTarget(
		Action<IBatchDrawer> drawFunc,
		Camera2D camera,
		SpriteSortMode sortMode = SpriteSortMode.Deferred,
		BlendState? blendState = null,
		SamplerState? samplerState = null,
		DepthStencilState? stencilState = null,
		RasterizerState? rasterState = null,
		Effect? effect = null);
	/// <summary>Passes the drawing spritebatch reference for custom drawing</summary>
	/// <param name="drawFunc">The custom drawing function</param>
	/// <param name="sortMode">The sorting mode to use</param>
	/// <param name="blendState">The blend state to use</param>
	/// <param name="samplerState">The sampler state to use</param>
	/// <param name="stencilState">The stencil state to use</param>
	/// <param name="rasterState">The raster state to use</param>
	/// <param name="effect">The effect to use</param>
	/// <param name="transform">The transformation matrix to use</param>
	void DrawToTarget(
		Action<IBatchDrawer> drawFunc,
		SpriteSortMode sortMode = SpriteSortMode.Deferred,
		BlendState? blendState = null,
		SamplerState? samplerState = null,
		DepthStencilState? stencilState = null,
		RasterizerState? rasterState = null,
		Effect? effect = null,
		Matrix? transform = null);
}