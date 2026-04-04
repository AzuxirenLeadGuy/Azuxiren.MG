namespace Azuxiren.MG.Components;

/// <summary>Represents the result of a game update function</summary>
public readonly struct GameUpdateResult
{
	/// <summary>The type of actions following an update</summary>
	internal enum ResultType
	{
		/// <summary>No action is to be taken</summary>
		NoAction,
		/// <summary>Game should be exited</summary>
		ExitRequest,
		/// <summary>Change to the next scene</summary>
		Transition,
		// TODO: Add enum for updating game resolutions?
	}
	/// <summary>The type of action to take</summary>
	internal readonly ResultType Type;
	/// <summary>In the case of transition, the next stage to transition</summary>
	internal readonly byte StageCode;
	private GameUpdateResult(ResultType type, byte stage_code = 0)
	{
		Type = type;
		StageCode = stage_code;
	}
	/// <summary>An instance of NoAction</summary>
	public static GameUpdateResult NoAction => new(ResultType.NoAction);
	/// <summary>An instance of ExitRequest</summary>
	public static GameUpdateResult ExitRequest => new(ResultType.ExitRequest);
	/// <summary>An instance of Transition</summary>
	public static GameUpdateResult Transition(byte code) => new(ResultType.Transition, code);
}