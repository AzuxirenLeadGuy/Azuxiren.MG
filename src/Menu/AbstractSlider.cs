using System;

using Microsoft.Xna.Framework;
namespace Azuxiren.MG.Menu
{
	/// <summary>
	/// It is a Component offering a varying parameter.
	/// Can be implemented as a Parameter input for a variable taking values from 
	/// Min to Max in a array of T(T=int/float/string/etc) with Value showing the 
	/// current index selected from the Array
	/// </summary>
	public abstract class AbstractSlider : AbstractComponent
	{
		/// <summary>This is the variable that is sliding</summary>
		public abstract byte Value { get; protected set; }
		/// <summary>This is the value string that is currently selected</summary>
		public abstract string CurrentlySelected{get; protected set;}
		/// <summary>The title of the Slider</summary>
		public string Title;
		/// <summary>
		/// Base Constructor for AbstractSlider
		/// </summary>
		/// <param name="bounds">The Rectangle for drawing purpose</param>
		/// <param name="message">The text to show</param>
		/// <param name="enableAtStart">The state of enabled will be set if this value is true</param>
		/// <returns></returns>
		public AbstractSlider(Rectangle bounds, string message = "", bool enableAtStart = true) : base(bounds, enableAtStart)
		{
			Title = message;
			ValueChanged = null!;
		}

		/// <summary>
		/// The event invoked on any change in Value
		/// </summary>
		public event EventHandler<SliderArgs>? ValueChanged;
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
			if (Enabled)
			{
				switch (_state)
				{
					case ComponentState.UnSelected:
						if (Selected) _state = ComponentState.Selected;
						break;
					case ComponentState.Selected:
						if (!Selected) _state = ComponentState.UnSelected;
						else if (InputIncrement || InputDecrement) _state = ComponentState.Press;
						break;
					case ComponentState.Press:
						if (!Selected) _state = ComponentState.UnSelected;
						else if (InputIncrement) Increment();
						else if (InputDecrement) Decrement();
						else _state = ComponentState.Release;
						break;
					case ComponentState.Release:
						_state = ComponentState.UnSelected;
						break;
				}
				if (ps != _state) 
				{
					OnStateChanged(gt, ps);
					ValueChanged?.Invoke(this, new(CurrentlySelected, Value));
				}
			}
		}
	}
}