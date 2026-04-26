using System;

using Azuxiren.MG.Drawing;

using Microsoft.Xna.Framework;

namespace Azuxiren.MG.Components;

/// <summary>
/// Represents a stage within the game
/// </summary>
/// <typeparam name="TSettings">Variable Setting Type shared between screens of the game</typeparam>
public interface IGameStage<TSettings> : IDisposable
{
	/// <summary>
	/// Resizes the game content for the given size of the target. 
	/// Called automatically by the game engine when the RenderTarget
	/// size changes.
	/// </summary>
	/// <param name="game">The current game instance</param>
	/// <param name="settings">The shared settings for the game</param>
	void Resize(in IMgRuntime game, ref TSettings settings);

	/// <summary>
	/// The logic for drawing the components within the game.
	/// It is not supposed to alter any existing setting
	/// </summary>
	/// <param name="gt">The GameTime object for this frame of the game</param>
	/// <param name="drawer">The instance for drawing actions</param>
	/// <param name="settings">The shared settings for the game</param>
	void Draw(GameTime gt, in IDrawHandler drawer, in TSettings settings);

	/// <summary>
	/// The logic for updating the components within the game.
	/// It may alter any existing setting. <br/>
	/// Transitions to other GameStages must be handled here as well. <br/>
	/// </summary>
	/// <param name="gt">The GameTime object for this frame of the game</param>
	/// <param name="game">The current game instance</param>
	/// <param name="settings">The shared settings for the game</param>
	/// <returns>Indication that screen needs to be updated, or game needs to be exited/closed.</returns>
	GameUpdate Update(GameTime gt, in IMgRuntime game, ref TSettings settings);
}