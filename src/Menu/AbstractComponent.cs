using System;

using Microsoft.Xna.Framework;
namespace Azuxiren.MG.Menu
{
	/// <summary>A WPF-styled implementaion of a Component.It is advisable
	/// for Action to be defined on Release State of Component</summary>
	public abstract class AbstractComponent
	{
		/// <summary>Points to the menu this component belongs to</summary>
		protected AbstractMenu? _rootMenu;
		/// <summary>Defines the boundaries of the button</summary>
		public Rectangle Bounds;
		/// <summary>
		/// <para>The state of this component.</para>
		/// <para>A button can be in state of Disabled,UnSelected,Selected,Press and Release.</para>
		/// </summary>
		protected ComponentState _state;
		/// <summary>
		/// <para>The state of this component.</para>
		/// <para>A button can be in state of Disabled,UnSelected,Selected,Press and Release.</para>
		/// </summary>
		public virtual ComponentState State => _state;
		/// <summary>Returns true if the button is enabled</summary>
		public virtual bool Enabled
		{
			get => _state != ComponentState.Disabled;
			set => _state = value ? ComponentState.UnSelected : ComponentState.Disabled;
		}
		/// <summary>Returns true if the button is Selected by the user</summary>
		public abstract bool Selected { get; set; }
		/// <summary>Recommended to add Button on button press here</summary>
		public event EventHandler<ComponentArgs>? OnRelease;
		/// <summary>Add generic action on Button states here</summary>
		public event EventHandler<ComponentArgs>? StateChanged;
		/// <summary>The method to call in the derived class for any action on change of state</summary>
		/// <param name="gt">The instant in time</param>
		/// <param name="ps">Keeps track of State of component</param>
		protected void OnStateChanged(GameTime gt, ComponentState ps)
		{
			//Make a temporary copy to avoid race condition :
			//The subsciber just unsubscribes after invokation and before checking of null
			StateChanged?.Invoke(this, new ComponentArgs(gt, ps, _state));
			EventHandler<ComponentArgs>? y = OnRelease;
			if (y != null && _state == ComponentState.Release)
				y.Invoke(this, new ComponentArgs(gt, ps, _state));
		}
		/// <summary>Returns true if the some input is given/pressed</summary>
		public abstract bool InputPressed { get; set; }
		/// <summary>The general/default update mechanism. Not calling this will "freeze" the Component</summary>
		/// <param name="gt">GameTime instance on update</param>
		public virtual void Update(GameTime gt)
		{
			var ps = _state;
			if (Enabled)
			{
				switch (_state)
				{
					case ComponentState.UnSelected:
						if (Selected)
							_state = ComponentState.Selected;
						break;
					case ComponentState.Selected:
						if (!Selected) _state = ComponentState.UnSelected;
						else if (InputPressed) _state = ComponentState.Press;
						break;
					case ComponentState.Press:
						if (!Selected) _state = ComponentState.UnSelected;
						else if (!InputPressed) _state = ComponentState.Release;
						break;
					case ComponentState.Release:
						_state = ComponentState.UnSelected;
						break;
				}
			}
			if (_state != ps) OnStateChanged(gt, ps);
		}
		/// <summary>
		/// The constructor for an AbstractComponent
		/// </summary>
		/// <param name="bds">The bounds of the component</param>
		/// <param name="enableAtStart">If this is set to true, component will be enabled at start</param>
		protected AbstractComponent(Rectangle bds, bool enableAtStart = true)
		{
			Bounds = bds;
			Enabled = enableAtStart;
		}
	}
}