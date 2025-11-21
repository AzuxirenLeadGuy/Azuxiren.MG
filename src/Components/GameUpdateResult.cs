using System;
namespace Azuxiren.MG.Components;

/// <summary>Represents the result of a game update function</summary>
public readonly struct GameUpdateResult<Args>
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
	}
	/// <summary>The type of action to take</summary>
	internal readonly ResultType Type;
	/// <summary>In the case of transition, the next stage to transition</summary>
	internal readonly IGameStage<Args>? NextStage;
	private GameUpdateResult(ResultType type, IGameStage<Args>? next = null)
	{
		Type = type;
		NextStage = next;
	}
	/// <summary>An instance of NoAction</summary>
	public static GameUpdateResult<Args> NoAction => new(ResultType.NoAction);
	/// <summary>An instance of ExitRequest</summary>
	public static GameUpdateResult<Args> ExitRequest => new(ResultType.ExitRequest);
	/// <summary>An instance of Transition</summary>
	public static GameUpdateResult<Args> Transition(IGameStage<Args> stage)
		=> stage != null ?
			new(ResultType.Transition, stage) :
			throw new ArgumentNullException(nameof(stage));
}
