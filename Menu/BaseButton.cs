using Microsoft.Xna.Framework;

namespace Azuxiren.MG.Menu;

/// <summary>Represents an absract button</summary>
public abstract class BaseButton : BaseComponent<BaseButton.BaseButtonState>
{
	/// <summary>Represents the state a Base button can be in</summary>
	public enum BaseButtonState
	{
		/// <summary>Button is not enabled</summary>
		Disabled,

		/// <summary>Button is not being pressed</summary>
		Released,

		/// <summary>Button has been pushed in this frame</summary>
		JustPressed,

		/// <summary>Button is being pressed</summary>
		Pressed,

		/// <summary>Button is released from pressed state in this frame</summary>
		JustReleased,
	}
	/// <summary>The input to the button, if it is pressed</summary>
	/// <value></value>
	public abstract bool Press { get; }

	/// <inheritdoc />
	public override BaseButtonState Update(GameTime gt)
	{
		if (Enabled == false)
		{
			return State = BaseButtonState.Disabled;
		}
		var state = State switch
		{
			BaseButtonState.Released => Press ? BaseButtonState.JustPressed : BaseButtonState.Released,
			BaseButtonState.JustPressed => Press ? BaseButtonState.Pressed : BaseButtonState.JustReleased,
			BaseButtonState.Pressed => Press ? BaseButtonState.Pressed : BaseButtonState.JustReleased,
			BaseButtonState.JustReleased => Press ? BaseButtonState.JustPressed : BaseButtonState.Released,
			_ => Press ? BaseButtonState.JustPressed : BaseButtonState.Released,
		};
		State = state;
		return state;
	}
}