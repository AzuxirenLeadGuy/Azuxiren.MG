using Azuxiren.MG.Drawing;

using Microsoft.Xna.Framework;

namespace Azuxiren.MG.Menu;


/// <summary>Represents a base class for a component within a menu</summary>
public abstract class BaseComponent<StateType>
{
	/// <summary>The boundary of the component</summary>
	public Rectangle Bounds { get; protected set; }

	/// <summary>
	/// A basic input to this component, showing if 
	/// the component is enabled
	/// </summary>
	public abstract bool Enabled { get; set; }

	/// <summary>The state of the component stored as data</summary>
	public abstract StateType State { get; protected set; }

	/// <summary>
	/// Update the component state based on 
	/// input for the component
	/// </summary>
	/// <param name="gt">Time delta from the previous frame</param>
	/// <returns>Returns the currently evaluated state of the component</returns>
	public abstract StateType Update(GameTime gt);

	/// <summary>Draw this component</summary>
	/// <param name="drawer">The drawing context object</param>
	public abstract void Draw(IBatchDrawer drawer);
}