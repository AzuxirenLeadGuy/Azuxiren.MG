using Azuxiren.MG.Drawing;

namespace Azuxiren.MG.Menu;


/// <summary>Represents a base class for a component within a menu</summary>
public interface IComponent<TStateType>
{
	/// <summary>
	/// A basic input to this component, showing if 
	/// the component is enabled
	/// </summary>
	bool Enabled { get; }

	/// <summary>The state of the component stored as data</summary>
	TStateType State { get; protected set; }

	/// <summary>Draw this component</summary>
	/// <param name="drawer">The drawing context object</param>
	void Draw(IBatchDrawer drawer);
}