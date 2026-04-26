namespace Azuxiren.MG.Components;

/// <summary>Represents the result of a game update function</summary>
public readonly record struct GameUpdate
{
	/// <summary>Represents a function for transition to next stage</summary>
	/// <param name="runtime">The game runtime</param>
	/// <param name="settings">The shared settings, if any</param>
	/// <returns>The next game stage to transition into</returns>
	public delegate IGameStage<TSettings> TransitionFun<TSettings>(IMgRuntime runtime, TSettings settings);
	/// <summary>The type of actions following an update</summary>

	internal enum ResultType
	{
		/// <summary>No action is to be taken</summary>
		NoAction,
		/// <summary>Change to the next scene</summary>
		Transition,
		/// <summary>Game should be exited</summary>
		ExitRequest,
		/// <summary>Update Window resolution</summary>
		SetWindowed,
		/// <summary>Set window as a Borderless Window with given resolution</summary>
		SetBorderlessWindowed,
		/// <summary>Set window as a fullscreen window with the given resolution</summary>
		SetFullScreen,
		/// <summary>Set window as a Borderless Fullscreen Window</summary>
		SetBorderlessFullscreen,
		/// <summary>Set new RenderTarget size</summary>
		SetTargetSize,
	}
	/// <summary>The type of action to take</summary>
	internal readonly ResultType Type;
	/// <summary>The resolution parameters</summary>
	internal readonly ushort Width, Height;
	/// <summary>In the case of transition, the function that gets the next scene</summary>
	internal readonly object? TransitionFunc = default;

	private GameUpdate(
		ResultType type,
		object? func = null,
		ushort width = 0,
		ushort height = 0
	)
	{
		Type = type;
		TransitionFunc = func;
		Width = width;
		Height = height;
	}
	/// <summary>An instance of NoAction</summary>
	public static GameUpdate NoAction => new(ResultType.NoAction);
	/// <summary>An instance of ExitRequest</summary>
	public static GameUpdate ExitRequest => new(ResultType.ExitRequest);
	/// <summary>An instance of Transition</summary>
	public static GameUpdate Transition<TSettings>(TransitionFun<TSettings> func) => new(ResultType.Transition, func);
	/// <summary>Requests to change the game window to borderless fullscreen</summary>
	public static GameUpdate SetBorderlessFullscreen() => new(ResultType.SetBorderlessFullscreen);
	/// <summary>Requests to change the game window to windowed with given properties</summary>
	public static GameUpdate SetWindowed(ushort width, ushort height, bool borderless = false) =>
		 new(
			borderless ? ResultType.SetWindowed : ResultType.SetBorderlessWindowed,
			width: width,
			height: height
		);

	/// <summary>Requests to change the game window to fullscreen with given dimensions</summary>
	public static GameUpdate SetFullScreen(ushort width, ushort height) =>
		new(ResultType.SetFullScreen, width: width, height: height);

	/// <summary>Requests to change the game window to fullscreen with given dimensions</summary>
	public static GameUpdate SetRenderTarget(ushort width, ushort height) =>
		new(ResultType.SetTargetSize, width: width, height: height);

}