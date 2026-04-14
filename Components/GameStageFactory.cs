namespace Azuxiren.MG.Components;

/// <summary>A custom factory that produces the set of stages for a given game</summary>
/// <typeparam name="Settings">The common shared settings for the game</typeparam>
public abstract class GameStageFactory<Settings>
{
	/// <summary>Custom initialization of Settings instance for the game</summary>
	/// <param name="game">The game instance</param>
	/// <returns>Initialized instance of game settings</returns>
	public abstract Settings InitializeSettings(in IMgRuntime game);
	/// <summary>Creates the start stage of the game</summary>
	/// <param name="game">The game instance</param>
	/// <param name="settings">The common shared settings for the game</param>
	/// <returns>Returns the loaded start stage for the game</returns>
	public abstract IGameStage<Settings> StartStage(in IMgRuntime game, Settings settings);

	/// <summary>Creates the loading stage of the game</summary>
	/// <param name="game">The game instance</param>
	/// <param name="settings">The common shared settings for the game</param>
	/// <returns>Returns the stage to show during loading stages of game</returns>
	public abstract IGameStage<Settings> LoadStage(in IMgRuntime game, Settings settings);

	/// <summary>Creates the stage of the game as requested by the code</summary>
	/// <param name="scene_code">The custom code/signal to request for stage creation</param>
	/// <param name="game">The game instance</param>
	/// <param name="settings">The common shared settings for the game</param>
	/// <returns>Returns the loaded stage for the game as requested</returns>
	public abstract IGameStage<Settings>? Create(in uint scene_code, in IMgRuntime game, in Settings settings);
}