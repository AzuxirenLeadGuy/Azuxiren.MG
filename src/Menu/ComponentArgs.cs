using System;

using Microsoft.Xna.Framework;
namespace Azuxiren.MG.Menu
{
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
}