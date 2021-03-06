using System;
using Microsoft.Xna.Framework;
namespace Azuxiren.MG.Menu
{
	/// <summary>A WPF-styled implementaion of a Component.It is advisable
	/// for Action to be defined on Release State of Component</summary>
	public abstract class AbstractComponent : IScreen
	{
		/// <summary>Points to the menu this component belongs to</summary>
		protected Menu RootMenu;
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
		/// <summary>Recommended to add Button on button press here</summary>
		public event EventHandler<ComponentArgs> OnRelease;
		/// <summary>Add generic action on Button states here</summary>
		public event EventHandler<ComponentArgs> StateChanged;
		/// <summary>The method to call in the derived class for any action on change of state</summary>
		/// <param name="gt">The instant in time</param>
		/// <param name="ps">Keeps track of State of component</param>
		protected void OnStateChanged(GameTime gt, ComponentState ps)
		{
			//Make a temporary copy to avoid race condition :
			//The subsciber just unsubscribes after invokation and before checking of null
			EventHandler<ComponentArgs> x = StateChanged;
			if (x != null)
			{ 
				x.Invoke(this, new ComponentArgs(gt, ps, state));
				var y=OnRelease;
				if(ps==ComponentState.Release && y!=null)y.Invoke(this,new ComponentArgs(gt, ps, state));
			}
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
}