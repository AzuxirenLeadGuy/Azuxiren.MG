using System;

using Microsoft.Xna.Framework;

namespace Azuxiren.MG.Menu;
/// <summary>Contains the event infromation for a slider value</summary>
public class SliderArgs : EventArgs
{
    /// <summary>The currently selected option string value</summary>
    public string SelectedOption;
    /// <summary>Currently selected index in the slider</summary>
    public int SelectedIndex;
    /// <summary>
    /// Creates a new SliderArgs object
    /// </summary>
    /// <param name="opt">Selected Option</param>
    /// <param name="sel">Selected Index</param>
    /// <returns></returns>
	public SliderArgs(string opt, int sel)
	{
        SelectedOption = opt;
        SelectedIndex = sel;
	}
}