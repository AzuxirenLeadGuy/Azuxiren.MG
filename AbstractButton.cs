using Microsoft.Xna.Framework;
namespace Azuxiren.MG.Menu
{
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
						if (Selected) 
						state = ComponentState.Selected;
						break;
					case ComponentState.Selected:
						if (!Selected) state = ComponentState.UnSelected;
						else if (InputPressed) state = ComponentState.Press;
						break;
					case ComponentState.Press:
						if (!Selected) state = ComponentState.UnSelected;
						else if (!InputPressed) state = ComponentState.Release;
						break;
					case ComponentState.Release:
						state = ComponentState.UnSelected;
						break;
				}
			}
			if (state != ps) OnStateChanged(gt, ps);
		}
	}
}