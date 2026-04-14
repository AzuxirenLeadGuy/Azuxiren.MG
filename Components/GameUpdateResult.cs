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
	/// <summary>In the case of transition, the next stage to transition</summary>
	internal readonly uint StageCode;
	internal static uint PackData(ushort width, ushort height)
	{
		uint data = width;
		data = (data << 16) | height;
		return data;
	}
	internal static (ushort width, ushort height) UnpackData(uint data)
	{
		ushort width = (ushort)(data >> 16);
		ushort height = (ushort)(data & 0xffff);
		return (width, height);
	}

	private GameUpdateResult(ResultType type, uint stage_code = 0)
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
	/// <summary>Requests to change the game window to borderless fullscreen</summary>
	public static GameUpdateResult SetBorderlessFullscreen() => new(ResultType.SetBorderlessFullscreen);
	/// <summary>Requests to change the game window to windowed with given properties</summary>
	public static GameUpdateResult SetWindowed(ushort width, ushort height, bool borderless = false) =>
		 new(
			borderless ? ResultType.SetWindowed : ResultType.SetBorderlessWindowed,
			PackData(width, height)
		);

	/// <summary>Requests to change the game window to fullscreen with given dimensions</summary>
	public static GameUpdateResult SetFullScreen(ushort width, ushort height) =>
		new(ResultType.SetFullScreen, PackData(width, height));

	/// <summary>Requests to change the game window to fullscreen with given dimensions</summary>
	public static GameUpdateResult SetRenderTarget(ushort width, ushort height) =>
		new(ResultType.SetTargetSize, PackData(width, height));

}