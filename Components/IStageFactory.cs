namespace Azuxiren.MG.Components;

/// <summary>A custom factory that produces the set of stages for a given game</summary>
/// <typeparam name="TSettings">The common shared settings for the game</typeparam>
public interface IStageFactory<TSettings>
{
	/// <summary>Custom initialization of Settings instance for the game</summary>
	/// <param name="game">The game instance</param>
	/// <returns>Initialized instance of game settings</returns>
	TSettings InitializeSettings(in IMgRuntime game);
	/// <summary>Creates the start stage of the game</summary>
	/// <param name="game">The game instance</param>
	/// <param name="settings">The common shared settings for the game</param>
	/// <returns>Returns the loaded start stage for the game</returns>
	IGameStage<TSettings> StartStage(in IMgRuntime game, TSettings settings);

	/// <summary>Creates the loading stage of the game</summary>
	/// <param name="game">The game instance</param>
	/// <param name="settings">The common shared settings for the game</param>
	/// <returns>Returns the stage to show during loading stages of game</returns>
	IGameStage<TSettings> LoadStage(in IMgRuntime game, TSettings settings);

	/// <summary>Creates the stage of the game as requested by the code</summary>
	/// <param name="sceneCode">The custom code/signal to request for stage creation</param>
	/// <param name="game">The game instance</param>
	/// <param name="settings">The common shared settings for the game</param>
	/// <returns>Returns the loaded stage for the game as requested</returns>
	IGameStage<TSettings>? Create(in uint sceneCode, in IMgRuntime game, in TSettings settings);
}