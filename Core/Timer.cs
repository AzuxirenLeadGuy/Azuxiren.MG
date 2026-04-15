using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Xna.Framework;

namespace Azuxiren.MG.Core;
/// <summary>A stopwatch that uses GameTime instances to update itself</summary>
public struct Timer : IEquatable<Timer>
{
	/// <summary>The time(in milliseconds) that have passed</summary>
	public uint TimeMs { get; private set; }

	/// <summary>Add the timer with the given delta</summary>
	public void Update(GameTime delta) => TimeMs += (uint)delta.ElapsedGameTime.Milliseconds;

	/// <summary>Reset the timer to 0</summary>
	public void Reset() => TimeMs = 0;
	/// <inheritdoc/>
	public readonly bool Equals(Timer other) => TimeMs.Equals(other.TimeMs);
	/// <inheritdoc/>
	public override readonly bool Equals([NotNullWhen(true)] object? obj) =>
		obj is Timer timer && Equals(timer);
	/// <inheritdoc/>
	public override readonly int GetHashCode() => (int)TimeMs;
}