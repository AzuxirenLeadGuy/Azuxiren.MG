using System;
using Microsoft.Xna.Framework;
namespace Azuxiren.MG.Menu
{
	/// <summary>
	/// It is a Component offering a varying parameter.
	/// Can be implemented as a Parameter input for a variable taking values from 
	/// Min to Max in a array of T(T=int/float/string/etc) with Value showing the 
	/// current index selected from the Array
	/// </summary>
	public abstract class AbstractSlider : AbstractComponent
	{
		/// <summary>This is the variable that is sliding</summary>
		protected virtual byte Value
		{
			get=>val;
			set
			{
				if(val!=value)
				{
					var x=OnValueChange;
					if(x!=null)x.Invoke(this,new SliderValueArgs(val,value));
					val=value;
				}
			}
		}
		/// <summary>The actual varible storing the current Value</summary>
		protected byte val;
		/// <summary>The title of the Slider</summary>
		public string Title;
		/// <summary>
		/// Base Constructor for AbstractSlider
		/// </summary>
		/// <param name="bounds">The Rectangle for drawing purpose</param>
		/// <param name="Message">The text to show</param>
		/// <param name="EnableAtStart">The state of enabled will be set if this value is true</param>
		/// <returns></returns>
		public AbstractSlider(Rectangle bounds, string Message = "", bool EnableAtStart = true) : base(bounds, EnableAtStart)=>Title = Message;
		/// <summary>
		/// The event invoked on any change in Value
		/// </summary>
		public event EventHandler<SliderValueArgs> OnValueChange;
		/// <returns>Returns true if increment is inputted, otherwise false</returns>
		public abstract bool InputIncrement { get; set; }
		/// <summary>Used for checking input to decrease value of Slider</summary>
		/// <returns>Returns true if decrement is inputted, otherwise false</returns>
		public abstract bool InputDecrement { get; set; }
		/// <summary>Increments the Value</summary>
		protected abstract void Increment();
		/// <summary>Decrements the value</summary>
		protected abstract void Decrement();
		/// <summary>The update mechanism for button. Not calling this will "freeze" the Slider</summary>
		/// <param name="gt">The gametime variable</param>
		public override void Update(GameTime gt)
		{
			var ps = state;
			if (Enabled)
			{
				switch (state)
				{
					case ComponentState.UnSelected:
						if (Selected) state = ComponentState.Selected;
						break;
					case ComponentState.Selected:
						if (!Selected) state = ComponentState.UnSelected;
						else if (InputIncrement || InputDecrement)state = ComponentState.Press;
						break;
					case ComponentState.Press:
						if(!Selected)state=ComponentState.UnSelected;
						else if(InputIncrement)Increment();
						else if(InputDecrement)Decrement();
						else state=ComponentState.Release;
						break;
					case ComponentState.Release:
						state = ComponentState.UnSelected;
						break;
				}
				if (ps != state)OnStateChanged(gt, ps);
			}
		}
	}
	/// <summary>
	/// Inherits from Abstract Slider, but with the implementation of Increment() and Decrement(). A timer/Cooldown is setup in the Hold State
	/// </summary>
	public abstract class AbstractSilderHold : AbstractSlider
	{
		/// <summary>
		/// Constructpr for base values
		/// </summary>
		/// <param name="bounds">The Rectangle denoting the location of this component to draw</param>
		/// <param name="opt">The array of options (as string) to display</param>
		/// <param name="MaxTimerMs">The time in milliseconds for cooldown of the button once taken input</param>
		/// <param name="Cycle">If true, the index </param>
		/// <param name="Message">The message to display in the component</param>
		/// <param name="EnableAtStart">Signifies that this element is enabled from the start</param>
		/// <returns></returns>
		protected AbstractSilderHold(Rectangle bounds, string[] opt, uint MaxTimerMs=200, bool Cycle=false, string Message = "", bool EnableAtStart = true) : base(bounds, Message, EnableAtStart)
		{
			MaxTimer=(int)MaxTimerMs;
			CycleMaxMin=Cycle;
			Options=opt;
			if(Options==null || Options.Length<1)throw new ArgumentException();
			MaxIndex=(byte)(Options.Length - 1);
		}
		internal int Timer;
		internal readonly int MaxTimer;
		/// <summary>
		/// If true, Decrementing at 0 will cycle it to MaxIndex, and similarly Incrementing on MaxIndex will cycle it to 0.
		/// 
		/// If false, Decrementing at 0 will not change the Value, and nor will Incrementing on MaxIndex.
		/// </summary>
		public readonly bool CycleMaxMin;
		/// <summary>
		/// The various options the slider will shift across
		/// </summary>
		public readonly string[] Options;
		internal readonly byte MaxIndex;
		/// <summary>The update mechanism for button. Not calling this will "freeze" the Slider. 
		/// Now Includes the Hold state where Events OnIncrement and OnDecrement is called on cooldowns.
		/// </summary>
		/// <param name="gt">The gametime variable</param>
		public override void Update(GameTime gt)
		{
			if(Timer<MaxTimer)Timer+=gt.ElapsedGameTime.Milliseconds;
			base.Update(gt);
		}
		/// <summary>
		/// This event is triggered during Increment and Cooldown of button press. 
		/// </summary>
		public event EventHandler<SliderValueArgs> OnIncrement;
		/// <summary>
		/// This event is triggered during Decrement and Cooldown of button press. 
		/// </summary>
		public event EventHandler<SliderValueArgs> OnDecrement;
		/// <summary>
		/// The implemented function to Increment the value. It implements a timer so that 
		/// OnIncrement is fired after the component has cooled down
		/// </summary>
		protected override void Increment()
		{
			if(Timer>=MaxTimer)
			{
				var pv=val;
				if(val<MaxIndex)Value++;
				else if(CycleMaxMin)Value=0;
				Timer=0;
				OnIncrement(this, new SliderValueArgs(pv,Value));
			}
		}
		/// <summary>
		/// The implemented function to Decrement the value. It implements a timer so that 
		/// OnDecrement is fired after the component has cooled down
		/// </summary>
		protected override void Decrement()
		{
			if(Timer>=MaxTimer)
			{
				var pv=val;
				if(val>0)Value--;
				else if(CycleMaxMin)Value=MaxIndex;
				Value--;
				Timer=0;
				OnDecrement(this, new SliderValueArgs(pv,Value));
			}
		}
	}
}