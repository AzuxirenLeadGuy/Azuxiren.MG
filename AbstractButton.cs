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
		/// <param name="message">The text to show</param>
		/// <param name="enableAtStart">If this is true, state of component will be Enabled at start</param>
		/// <returns></returns>
		public AbstractButton(Rectangle bounds, string message = "", bool enableAtStart = true) : base(bounds, enableAtStart) { Title = message; }
		/// <summary>Returns true if the button is inputted for pressing</summary>
		public abstract bool InputPressed { get; set; }
		/// <summary>The update mechanism for button. Not calling this will "freeze" the button</summary>
		/// <param name="gt"></param>
		public override void Update(GameTime gt)
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
	}
}