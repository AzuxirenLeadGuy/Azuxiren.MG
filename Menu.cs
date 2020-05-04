using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
namespace Azuxiren.MG.Menu
{
	/// <summary>All possibel states of a button</summary>
	public enum ComponentState : byte
	{
		/// <summary>The component is unselected</summary>
		UnSelected = 0,
		/// <summary>The component is now on the focus of the end-user</summary>
		Selected = 1,
		/// <summary> The component is input button is down</summary>
		Press = 2,
		/// <summary>The component input button has just been released. A good time to implement function of the component</summary>
		Release = 3,
		/// <summary>The component is inaccessible</summary>
		Disabled = 4
	}
	/// <summary>The EventArgs implementation for components</summary>
	public class ComponentArgs : EventArgs
	{
		/// <summary>The states invloved in transition of Button</summary>
		public GameTime gt;
		/// <summary>The state of Component before Invokation</summary>
		public ComponentState Previous;
		/// <summary>The state of Component during Invokation</summary>
		public ComponentState Current;
		/// <summary>
		/// Defines all the values of ButtonArgs
		/// </summary>
		/// <param name="g">GameTime object</param>
		/// <param name="ps">Previous State</param>
		/// <param name="cs">Current State</param>
		public ComponentArgs(GameTime g, ComponentState ps, ComponentState cs) { gt = g; Previous = ps; Current = cs; }
	}
	/// <summary>Represents the changed value in an AbstractSlider</summary>
	/// <typeparam name="T">The data type of the value of AbstractSlider</typeparam>
	public class SliderValueArgs<T> : EventArgs
	{
		/// <summary>The value before the change</summary>
		public T Prev;
		/// <summary>The value after the change</summary>
		public T Curr;
		/// <summary>
		/// The constructor for SliderValueArgs class
		/// </summary>
		/// <param name="pre">The value before change</param>
		/// <param name="cur">The value after change</param>
		public SliderValueArgs(T pre, T cur)
		{
			Prev=pre;
			Curr=cur;
		}
	}
	/// <summary>A WPF-styled implementaion of a Component.It is advisable
	/// for Action to be defined on Release State of Component</summary>
	public abstract class AbstractComponent : IScreen
	{
		/// <summary>Defines the boundaries of the button</summary>
		public Rectangle bounds;
		/// <summary>
		/// The state of this component.
		/// 
		/// A button can be in state of Disabled,UnSelected,Selected,Press and Release.
		/// </summary>
		protected ComponentState state;
		/// <summary>
		/// The state of this component.
		/// 
		/// A button can be in state of Disabled,UnSelected,Selected,Press and Release.
		/// </summary>
		public ComponentState State => state;
		/// <summary>Returns true if the button is enabled</summary>
		public virtual bool Enabled
		{
			get => state != ComponentState.Disabled;
			set => state = value ? ComponentState.UnSelected : ComponentState.Disabled;
		}
		/// <summary>Returns true if the button is Selected by the user</summary>
		public abstract bool Selected { get; set; }
		/// <summary>Add implementation of Button Action here</summary>
		public event EventHandler<ComponentArgs> StateChanged;
		/// <summary>The method to call in the derived class for actionOnSelect Invokation</summary>
		/// <param name="gt">The instant in time</param>
		/// <param name="ps">Keeps track of State of component</param>
		protected void OnStateChanged(GameTime gt, ComponentState ps)
		{
			//Make a temporary copy to avoid race condition :
			//The subsciber just unsubscribes after invokation and before checking of null
			EventHandler<ComponentArgs> x = StateChanged;
			if (x != null) x.Invoke(this, new ComponentArgs(gt, ps, state));
		}
		/// <summary>The Draw function of this Component.</summary>
		/// <param name="gt">The GameTime variable</param>
		public abstract void Draw(GameTime gt);
		/// <summary>Load contents for the Component(If required)</summary>
		public abstract void LoadContent();
		/// <summary>The update mechanism for button. Not calling this will "freeze" the Component</summary>
		/// <param name="gt"></param>
		public abstract void Update(GameTime gt);
		/// <summary>
		/// The constructor for an AbstractComponent
		/// </summary>
		/// <param name="bds">The bounds of the component</param>
		/// <param name="EnableAtStart">If this is set to true, component will be enabled at start</param>
		public AbstractComponent(Rectangle bds, bool EnableAtStart = true)
		{
			bounds = bds;
			Enabled = EnableAtStart;
		}
	}
	/// <summary>An abstract implementation of a Button</summary>
	public abstract class AbstractButton : AbstractComponent
	{
		/// <summary> The message to display on Button</summary>
		public string Title;
		/// <summary>
		/// The constructor for an AbstractButton object
		/// </summary>
		/// <param name="bounds">The rectangle region it is bounded at</param>
		/// <param name="Message">The text to show</param>
		/// <param name="EnableAtStart">If this is true, state of component will be Enabled at start</param>
		/// <returns></returns>
		public AbstractButton(Rectangle bounds, string Message = "", bool EnableAtStart = true) : base(bounds, EnableAtStart) { Title = Message; }
		/// <summary>Returns true if the button is inputted for pressing</summary>
		public abstract bool InputPressed { get; set; }
		/// <summary>The update mechanism for button. Not calling this will "freeze" the button</summary>
		/// <param name="gt"></param>
		public override void Update(GameTime gt)
		{
			var ps = state;
			if (Enabled)
			{
				switch (state)
				{
					case ComponentState.UnSelected:
						if (Selected) state = ComponentState.Selected;
						break;
					case ComponentState.Selected:
						if (!Selected) state = ComponentState.UnSelected;
						else if (InputPressed) state = ComponentState.Press;
						break;
					case ComponentState.Press:
						if (!Selected) state = ComponentState.UnSelected;
						else if (!InputPressed) state = ComponentState.Release;
						InputPressed = false;
						break;
					case ComponentState.Release:
						state = ComponentState.UnSelected;
						break;
				}
			}
			if (state != ps) OnStateChanged(gt, ps);
		}
	}
	/// <summary>
	/// It is a Component offering a varying parameter.
	/// 
	/// Can be implemented as multiple option (T=String, implemented by using multiple Options)
	/// 
	/// Can be implemented as a Parameter input for a variable taking values from Min to Max (T=int/float/etc)
	/// </summary>
	public abstract class AbstractSlider<T> : AbstractComponent
	{
		/// <summary>This is the variable that is sliding</summary>
		protected abstract T Value{get;set;}
		/// <summary>The title of the Slider</summary>
		public string Title;
		/// <summary>
		/// The Limit of CoolDown Timer
		/// </summary>
		protected int coolDownTime;
		/// <summary>
		/// The running timer. Do not touch unless you know what you're doing
		/// </summary>
		protected int Timer;
		/// <summary>
		/// Base Constructor for AbstractSlider
		/// </summary>
		/// <param name="bounds">The Rectangle for drawing purpose</param>
		/// <param name="Message">The text to show</param>
		/// <param name="EnableAtStart">The state of enabled will be set if this value is true</param>
		/// <param name="cdms">The coolDownTime in milliseconds</param>
		/// <returns></returns>
		public AbstractSlider(Rectangle bounds, string Message = "", bool EnableAtStart = true, int cdms = 250) : base(bounds, EnableAtStart)
		{
			Title = Message;
			coolDownTime = cdms;
		}
		/// <summary>
		/// The event invoked on any change in Value
		/// </summary>
		public EventHandler<SliderValueArgs<T>> OnValueChange;
		/// <summary>The event invoked on Incrementing the slider</summary>
		public event EventHandler<ComponentArgs> OnIncrement;
		/// <summary>The event invoked on Decrementing the slider</summary>
		public event EventHandler<ComponentArgs> OnDecrement;
		/// <summary>Used for checking input to increase value of Slider</summary>
		/// <returns>Returns true if increment is inputted, otherwise false</returns>
		public abstract bool InputIncrement { get; set; }
		/// <summary>Used for checking input to decrease value of Slider</summary>
		/// <returns>Returns true if decrement is inputted, otherwise false</returns>
		public abstract bool InputDecrement { get; set; }
		/// <summary>The update mechanism for button. Not calling this will "freeze" the Slider</summary>
		/// <param name="gt">The gametime variable</param>
		public override void Update(GameTime gt)
		{
			var ps = state;
			if (Enabled)
			{
				switch (state)
				{
					case ComponentState.UnSelected:
						if (Selected) state = ComponentState.Selected;
						break;
					case ComponentState.Selected:
						if (!Selected) state = ComponentState.UnSelected;
						else if (InputIncrement || InputDecrement) { state = ComponentState.Press; goto case ComponentState.Press; }
						break;
					case ComponentState.Press:
						if (InputIncrement) OnIncrement.Invoke(this, new ComponentArgs(gt, ps, state));
						else if (InputDecrement) OnDecrement.Invoke(this, new ComponentArgs(gt, ps, state));
						state = ComponentState.Release;
						break;
					case ComponentState.Release:
						state = ComponentState.Selected;
						InputDecrement = false;
						InputIncrement = false;
						break;
				}
				if (ps != state)
				{
					Timer=0;
					OnStateChanged(gt, ps);
				}
			}
			else
			{
				Timer+=gt.ElapsedGameTime.Milliseconds;
				if(Timer>=coolDownTime)Enabled=true;
			}
		}
	}
	/// <summary>
	/// Interface for a basic menu which is a collection of AbstractComponents
	/// </summary>
	public abstract class Menu
	{
		/// <summary>The collection of components</summary>
		public abstract IEnumerable<AbstractComponent> Components{get;}
		/// <summary>The element that is Currently selected in the menu, i.e the component which has Selected property true, others have it false</summary>
		public virtual AbstractComponent CurrentlySelected
		{
			get=>currentlySelected;
			set
			{
				currentlySelected.Selected=false;
				currentlySelected=value;
				currentlySelected.Selected=true;
			}
		}
		/// <summary>The component that is currently selected in the menu</summary>
		protected AbstractComponent currentlySelected;
		/// <summary>
		/// Draws the menu
		/// </summary>
		/// <param name="gt">GameTime instance</param>
		public virtual void Draw(GameTime gt){foreach(var comp in Components)comp.Draw(gt);}
		/// <summary>
		/// Updates the menu
		/// </summary>
		/// <param name="gt">GameTime instance</param>
		public virtual void Update(GameTime gt){foreach(var comp in Components)comp.Update(gt);}
	}
}