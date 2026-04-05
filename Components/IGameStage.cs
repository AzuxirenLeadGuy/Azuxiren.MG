using Azuxiren.MG.Drawing;

using Microsoft.Xna.Framework;

namespace Azuxiren.MG.Components;

/// <summary>
/// Represents a stage within the game
/// </summary>
/// <typeparam name="Settings">Variable Setting Type shared between screens of the game</typeparam>
public interface IGameStage<Settings>
{
	/// <summary>
	/// The logic for drawing the components within the game.
	/// It is not supposed to alter any existing setting
	/// </summary>
	/// <param name="gt">The GameTime object for this frame of the game</param>
	/// <param name="drawer">The instance for drawing actions</param>
	/// <param name="settings">The shared settings for the game</param>
	void Draw(GameTime gt, in RenderTargetDrawer drawer, in Settings settings);

	/// <summary>
	/// The logic for updating the components within the game.
	/// It may alter any existing setting. <br/>
	/// Transitions to other GameStages must be handled here as well. <br/>
	/// </summary>
	/// <param name="gt">The GameTime object for this frame of the game</param>
	/// <param name="settings">The shared settings for the game</param>
	/// <returns>Indication that screen needs to be updated, or game needs to be exited/closed.</returns>
	GameUpdateResult Update(GameTime gt, ref Settings settings);
}