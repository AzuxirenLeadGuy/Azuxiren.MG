using Microsoft.Xna.Framework;

namespace Azuxiren.MG.Core;
/// <summary>Represents a base class for a component within a menu</summary>
public struct Timer
{
	/// <summary>The time(in milliseconds) that have passed</summary>
	public uint TimeMs { get; private set; }

	/// <summary>Add the timer with the given delta</summary>
	public void Update(GameTime delta) => TimeMs += (uint)delta.ElapsedGameTime.Milliseconds;

	/// <summary>Reset the timer to 0</summary>
	public void Reset() => TimeMs = 0;
}