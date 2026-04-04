using Microsoft.Xna.Framework;

namespace Azuxiren.MG.Menu;

/// <summary>Represents a BaseSwitch component</summary>
public abstract class BaseSwitch : BaseComponent<BaseSwitch.BaseSwitchState>
{
	/// <summary>The state a switch can be in</summary>
	public enum BaseSwitchState
	{
		/// <summary>Switch is not enabled</summary>
		Disabled,

		/// <summary>Switch is released and off</summary>
		ReleasedOff,

		/// <summary>Switch is just pressed to be on</summary>
		JustPressedOn,

		/// <summary>Switch is in pressed state and on</summary>
		PressedOn,

		/// <summary>Switch is just released and on</summary>
		JustReleasedOn,

		/// <summary>Switch is in released state and on</summary>
		ReleasedOn,

		/// <summary>Switch is just pressed to be off</summary>
		JustPressedOff,

		/// <summary>Switch is in pressed state and off</summary>
		PressedOff,

		/// <summary>Switch is just released and off</summary>
		JustReleasedOff,
	}

	/// <summary>The input of the button being pressed</summary>
	public abstract bool Press { get; }

	/// <inheritdoc/>
	public override BaseSwitchState Update(GameTime gt)
	{
		if (Enabled == false) return BaseSwitchState.Disabled;
		BaseSwitchState state = State switch
		{
			BaseSwitchState.ReleasedOff => Press ? BaseSwitchState.JustPressedOn : BaseSwitchState.ReleasedOff,
			BaseSwitchState.JustPressedOn => Press ? BaseSwitchState.PressedOn : BaseSwitchState.JustReleasedOn,
			BaseSwitchState.PressedOn => Press ? BaseSwitchState.PressedOn : BaseSwitchState.JustReleasedOn,
			BaseSwitchState.JustReleasedOn => Press ? BaseSwitchState.JustPressedOff : BaseSwitchState.ReleasedOn,
			BaseSwitchState.ReleasedOn => Press ? BaseSwitchState.JustPressedOff : BaseSwitchState.ReleasedOn,
			BaseSwitchState.JustPressedOff => Press ? BaseSwitchState.PressedOff : BaseSwitchState.JustReleasedOff,
			BaseSwitchState.PressedOff => Press ? BaseSwitchState.PressedOff : BaseSwitchState.JustReleasedOff,
			BaseSwitchState.JustReleasedOff => Press ? BaseSwitchState.JustPressedOn : BaseSwitchState.ReleasedOff,
			_ => Press ? BaseSwitchState.JustPressedOn : BaseSwitchState.ReleasedOff,
		};
		State = state;
		return state;
	}

	/// <summary>Shows if the switch is off or on</summary>
	public bool? SwitchedOn => State switch
	{
		BaseSwitchState.Disabled => null,
		BaseSwitchState.JustPressedOn => true,
		BaseSwitchState.PressedOn => true,
		BaseSwitchState.JustReleasedOn => true,
		BaseSwitchState.ReleasedOn => true,
		_ => false,
	};

}