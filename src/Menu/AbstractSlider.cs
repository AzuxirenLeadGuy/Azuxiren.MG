using System;

using Microsoft.Xna.Framework;
namespace Azuxiren.MG.Menu;
/// <summary>
/// It is a Component offering a varying parameter.
/// Can be implemented as a Parameter input for a variable taking values from
/// Min to Max in a array of T(T=int/float/string/etc) with Value showing the
/// current index selected from the Array
/// </summary>
public abstract class AbstractSlider : AbstractComponent
{
	/// <summary>The state of input being pressed for the slider</summary>
	protected enum PressState : byte
	{
		/// <summary>No input is being pressed</summary>
		NoInputPressed,
		/// <summary>Increment input is being pressed</summary>
		IncPress,
		/// <summary>Decrement input is being pressed</summary>
		DecPress,
	}
	/// <summary>The current pressed state of the instance</summary>
	protected PressState _pressState;
	/// <summary>This is the variable that is sliding</summary>
	public abstract byte Value { get; protected set; }
	/// <summary>This is the value string that is currently selected</summary>
	public abstract string CurrentlySelected { get; protected set; }
	/// <summary>The title of the Slider</summary>
	public string Title;
	/// <summary>
	/// Base Constructor for AbstractSlider
	/// </summary>
	/// <param name="bounds">The Rectangle for drawing purpose</param>
	/// <param name="message">The text to show</param>
	/// <param name="enableAtStart">The state of enabled will be set if this value is true</param>
	/// <returns></returns>
	protected AbstractSlider(Rectangle bounds, string message = "", bool enableAtStart = true) : base(bounds, enableAtStart)
	{
		Title = message;
		ValueChanged = null!;
		_pressState = PressState.NoInputPressed;
	}
	/// <inheritdoc/>
	public override bool InputPressed
	{
		get => InputIncrement || InputDecrement;
		set => (InputIncrement, InputDecrement) = (value, value);
	}
	/// <summary>
	/// The event invoked on any change in Value
	/// </summary>
	public event EventHandler<SliderArgs>? ValueChanged;
	/// <summary>Used for checking input to increase value of Slider</summary>
	/// <returns>Returns true if increment is inputted, otherwise false</returns>
	public abstract bool InputIncrement { get; set; }
	/// <summary>Used for checking input to decrease value of Slider</summary>
	/// <returns>Returns true if decrement is inputted, otherwise false</returns>
	public abstract bool InputDecrement { get; set; }
	/// <summary>Increments the Value</summary>
	protected abstract void Increment();
	/// <summary>Decrements the value</summary>
	protected abstract void Decrement();
	/// <summary>The update mechanism for button. Not calling this will "freeze" the Slider</summary>
	/// <param name="gt">The gametime variable</param>
	public override void Update(GameTime gt)
	{
		var ps = _state;
		var pv = Value;
		if (Enabled)
		{
			base.Update(gt);
			if (InputPressed)
			{
				_pressState = InputIncrement ? PressState.IncPress : PressState.DecPress;
			}
			else if (_state == ComponentState.Release)
			{
				if (_pressState == PressState.IncPress)
					Increment();
				else // if(_pressState == PressState.DecrementPressed)
					Decrement();
				_pressState = PressState.NoInputPressed;
			}
			if (pv != Value)
			{
				ValueChanged?.Invoke(this, new(CurrentlySelected, Value));
			}
		}
	}
}