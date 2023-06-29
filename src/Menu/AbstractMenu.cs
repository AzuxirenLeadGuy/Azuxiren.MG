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
		/// Updates the menu
		/// </summary>
		/// <param name="gt">GameTime instance</param>
		public virtual void Update(GameTime gt) { foreach (var comp in Components) comp.Update(gt); }
	}
}