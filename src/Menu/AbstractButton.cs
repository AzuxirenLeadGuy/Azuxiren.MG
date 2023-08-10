using Microsoft.Xna.Framework;
namespace Azuxiren.MG.Menu
{
	/// <summary>An abstract implementation of a Button</summary>
	public abstract class AbstractButton : AbstractComponent
	{
		/// <summary> The message to display on Button</summary>
		public string ButtonText;
		/// <summary>
		/// The constructor for an AbstractButton object
		/// </summary>
		/// <param name="bds">The rectangle region it is bounded at</param>
		/// <param name="btext">The text to show</param>
		/// <param name="enabled">If this is true, state of component will be Enabled at start</param>
		/// <returns></returns>
		protected AbstractButton(Rectangle bds, string btext = "", bool enabled = true) : base(bds, enabled) => ButtonText = btext;
	}
}