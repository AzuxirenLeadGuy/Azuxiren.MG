using System.Collections;
using System.Collections.Generic;

namespace Azuxiren.MG.Hex;
/// <summary>
/// Represents a basic 2D Heaxagonal Grid with cubic/axial
/// coordinates centered at (0,0,[0]) and arranged
/// in a hexagonal layout.
/// </summary>
/// <typeparam name="T">
/// The type of object that occupies a position
/// within the Grid.
/// </typeparam>
public readonly struct HexGrid<T> : IReadOnlyDictionary<GridPoint, T>
{
	/// <summary>
	/// The distance from between center (0, 0, [0])
	/// to any of the edge point of the Grid that itself is shaped
	/// as a hexagon
	/// </summary>
	public readonly byte Radius;
	private readonly Dictionary<GridPoint, T> _data;
	/// <summary>The enumeration of the keys within the grid</summary>
	/// <returns>Enumeration of the keys within the grid</returns>
	public IEnumerable<GridPoint> Keys => ((IReadOnlyDictionary<GridPoint, T>)_data).Keys;
	/// <summary>The enumeration of the values within the grid</summary>
	/// <returns>Enumeration of the values within the grid</returns>
	public IEnumerable<T> Values => ((IReadOnlyDictionary<GridPoint, T>)_data).Values;
	/// <summary>The number of cells in the grid</summary>
	public int Count => _data.Count;
	/// <summary>
	/// Check if a key exists in the grid or not
	/// </summary>
	/// <param name="key">The key to seatch/check</param>
	/// <returns>True if the key exists; false otherwise</returns>
	public bool ContainsKey(GridPoint key) => _data.ContainsKey(key);
	/// <summary>The enumeration of the key/value pairs within the grid</summary>
	/// <returns>Enumeration of the key/value pairs within the grid</returns>
	public IEnumerator<KeyValuePair<GridPoint, T>> GetEnumerator() => ((IEnumerable<KeyValuePair<GridPoint, T>>)_data).GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_data).GetEnumerator();
	/// <summary>Default Constructor, fills all positions with the given value</summary>
	/// <param name="defaultvalue">The value to fill all cells with</param>
	/// <param name="r">Radius of the grid</param>
	public HexGrid(T defaultvalue, byte r = 1)
	{
		Radius = r;
		_data = new();
		foreach (var pt in new GridPoint(0, 0).GetSpiralRing(r))
			_data.Add(pt, defaultvalue);
	}
	/// <summary>Gets the element from the grid safely</summary>
	/// <param name="point">The point to get the value from</param>
	/// <param name="value">The output value</param>
	/// <returns>true if retrival was successful. Otherwise false</returns>
	public bool TryGetValue(GridPoint point, out T value)
	{
		if (_data.ContainsKey(point))
		{
			value = _data[point];
			return true;
		}
		value = default!;
		return false;
	}
	/// <summary>Sets the object at the given point</summary>
	/// <param name="point">The point to set at</param>
	/// <param name="value">The value to set</param>
	/// <returns>true if setting value was successful. Otherwise false</returns>
	public bool Set(GridPoint point, T value)
	{
		if (!_data.ContainsKey(point))
			return false;
		_data[point] = value;
		return true;
	}
	/// <summary>
	/// Wrapper to access the values of points within the structure.
	/// Will throw exceptions at invalid point.
	/// </summary>
	/// <value>The point to set at</value>
	public T this[GridPoint point]
	{
		get => _data[point];
		set => _data[point] = value;
	}
}