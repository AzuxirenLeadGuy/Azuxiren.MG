using Microsoft.Xna.Framework;

namespace Azuxiren.MG;

/// <summary>
/// Represents a simple component within the game
/// </summary>
/// <typeparam name="Parameters">Constant Parameter Type shared for the game</typeparam>
/// <typeparam name="Settings">Variable Setting Type shared between screens of the game</typeparam>
public interface IGameLoadableComponent<Parameters, Settings>
{
	/// <summary>
	/// Loads the required content. This is a required 
	/// phase for transitioning between game stages.
	/// </summary>
	/// <param name="parameters">The object that contain all constant paramters for the game</param>
	/// <param name="settings">The shared settings for the game</param>
	public void LoadContent(in Parameters parameters, ref Settings settings);
}

/// <summary>
/// Represents a drawable component within the game
/// </summary>
/// <typeparam name="Parameters">Constant Parameter Type shared for the game</typeparam>
/// <typeparam name="Settings">Variable Setting Type shared between screens of the game</typeparam>
public interface IDrawableComponent<Parameters, Settings>
{
	/// <summary>
	/// The logic for drawing the components within the game. 
	/// It is not supposed to alter any existing setting
	/// </summary>
	/// <param name="gt">The GameTime object for this frame of the game</param>
	/// <param name="parameters">The object that contain all constant paramters for the game</param>
	/// <param name="settings">The shared settings for the game</param>
	public void Draw(GameTime gt, in Parameters parameters, in Settings settings);
}


/// <summary>
/// Represents a stage within the game
/// </summary>
/// <typeparam name="Parameters">Constant Parameter Type shared for the game</typeparam>
/// <typeparam name="Settings">Variable Setting Type shared between screens of the game</typeparam>
public interface IGameStage<Parameters, Settings> : IGameLoadableComponent<Parameters, Settings>, IDrawableComponent<Parameters, Settings>
{
	/// <summary>
	/// The logic for updating the components within the game. 
	/// It may alter any existing setting. <br/>
	/// Transitions to other GameStages must be handled here as well. <br/>
    /// If return value is false, no transition is performed<br/>
    /// If return value is true, and nextStage is null, then the game is exited/closed.<br/>
    /// If return value is true, and nextStage is not null, then the game performs transition
    /// (before loading in a parallel thread)
	/// </summary>
	/// <param name="gt">The GameTime object for this frame of the game</param>
	/// <param name="parameters">The object that contain all constant paramters for the game</param>
	/// <param name="settings">The shared settings for the game</param>
	/// <param name="nextStage">The nextStage for the game, if any. Otherwise null is returned</param>
	/// <returns>true the screen needs to be updated, or game needs to be exited/closed. Otherwise false</returns>
	public bool Update(GameTime gt, in Parameters parameters, ref Settings settings, out IGameStage<Parameters, Settings>? nextStage);
}
