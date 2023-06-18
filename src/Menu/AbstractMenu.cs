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
		public GameTime Gametime;
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
		public ComponentArgs(GameTime g, ComponentState ps, ComponentState cs) { Gametime = g; Previous = ps; Current = cs; }
	}
	/// <summary>Represents the changed value in an AbstractSlider</summary>
	public class SliderValueArgs : EventArgs
	{
		/// <summary>The value before the change</summary>
		public byte Prev;
		/// <summary>The value after the change</summary>
		public byte Curr;
		/// <summary>
		/// The constructor for SliderValueArgs class
		/// </summary>
		/// <param name="pre">The value before change</param>
		/// <param name="cur">The value after change</param>
		public SliderValueArgs(byte pre, byte cur)
		{
			Prev = pre;
			Curr = cur;
		}
	}
	/// <summary>
	/// Interface for a basic menu which is a collection of AbstractComponents
	/// </summary>
	public abstract class AbstractMenu
	{
		/// <summary>The collection of components</summary>
		public abstract IEnumerable<AbstractComponent> Components { get; }
		/// <summary>The element that is Currently selected in the menu, i.e the component which has Selected property true, others have it false</summary>
		public virtual AbstractComponent? CurrentlySelected
		{
			get => _currentlySelected;
			set
			{
				if (value == null) return;
				if (_currentlySelected != null)
					_currentlySelected.Selected = false;
				_currentlySelected = value;
				_currentlySelected.Selected = true;
			}
		}
		/// <summary>The component that is currently selected in the menu</summary>
		protected AbstractComponent? _currentlySelected;
		/// <summary>
		/// Draws the menu
		/// </summary>
		/// <param name="gt">GameTime instance</param>
		public virtual void Draw(GameTime gt) { foreach (var comp in Components) comp.Draw(gt); }
		/// <summary>
		/// Updates the menu
		/// </summary>
		/// <param name="gt">GameTime instance</param>
		public virtual void Update(GameTime gt) { foreach (var comp in Components) comp.Update(gt); }
	}
}