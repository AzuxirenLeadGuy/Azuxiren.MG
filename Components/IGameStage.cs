using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Azuxiren.MG.Components;

/// <summary>
/// Represents a stage within the game
/// </summary>
/// <typeparam name="Settings">Variable Setting Type shared between screens of the game</typeparam>
public interface IGameStage<Settings>
{
	/// <summary>
	/// Loads the required content. This is a required
	/// phase for transitioning between game stages.
	/// </summary>
	/// <param name="device">The graphics setting of the game for initialization</param>
	/// <param name="content">Content manager to load assets for game</param>
	/// <param name="settings">The shared settings for the game</param>
	void LoadContent(in GraphicsDevice device, in ContentManager content, ref Settings settings);

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
	GameUpdateResult<Settings> Update(GameTime gt, ref Settings settings);
}