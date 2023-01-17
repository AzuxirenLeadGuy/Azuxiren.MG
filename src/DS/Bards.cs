using System;
using System.Collections;
using System.Collections.Generic;

namespace Azuxiren.MG.DS;

/// <summary>
/// Buffer Array with Random-Access Deletion Structure (BARDS) is a
/// data structure that can stores and iterate item in
/// any order, and support random access deletion. Deletion is supported
/// while iteration in a custom iterator
/// </summary>
/// <typeparam name="T">Type of item being stored</typeparam>
public class Bards<T> : ISidds<T>
where T : IEquatable<T>
{
	/// <summary>The buffer where data is stored</summary>
	protected readonly T[] _data;
	/// <summary>The length utilized so far</summary>
	protected uint _currentLength;
	/// <summary>The size of the buffer of data</summary>
	public readonly uint Maxsize;
	/// <summary>Contructor to initialize the data structure</summary>
	/// <param name="size">The size of the fixed bugger</param>
	public Bards(uint size)
	{
		Maxsize = size;
		_data = new T[size];
		_currentLength = 0;
	}
	/// <inheritdoc/>
	public uint Count => _currentLength;
	/// <summary>Generic enumerator for enumerating the elements added in the data structure/// </summary>
	/// <returns>The enumeration of elements in the collection</returns>
	public IEnumerator<T> GetEnumerator()
	{
		for (int i = 0; i < _currentLength; i++)
		{
			yield return _data[i];
		}
	}
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	/// <inheritdoc/>
	public bool Add(T item)
	{
		if (item == null || _currentLength >= Maxsize) return false;
		_data[_currentLength++] = item;
		return true;
	}
	/// <summary>Removes the instance from the "active" list</summary>
	/// <param name="index">The index of the item to remove</param>
	/// <returns>true if removal was successful, otherwise false</returns>
	protected bool Remove(uint index)
	{
		if (index < _currentLength && _currentLength > 0)
		{
			--_currentLength;
			_data[index] = _data[_currentLength];
			return true;
		}
		else return false;
	}
	/// <summary>Removes the item from the list (does not deallocate)</summary>
	/// <param name="item">the item to search and remove</param>
	/// <returns>true if the item is found and removed successfully. Otherwise false</returns>
	public bool Remove(T item)
	{
		for (uint i = 0; i < _currentLength; i++)
		{
			if (_data[i].Equals(item))
			{
				return Remove(i);
			}
		}
		return false;
	}
	/// <inheritdoc/>
	public void CustomIterate(Func<T, bool> function)
	{
		bool Action(ref T item) => function(item);
		CustomIterate(Action);
	}
	/// <inheritdoc/>
	public void CustomIterate(RefActionOrDelete<T> function)
	{
		uint i = 0;
		while (i < _currentLength)
		{
			if (function(ref _data[i])) i++;
			else Remove(i);
		}
	}
	/// <inheritdoc/>
	public void Clear() => _currentLength = 0;
}