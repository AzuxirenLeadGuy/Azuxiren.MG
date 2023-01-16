using System;
using System.Collections.Generic;

namespace Azuxiren.MG.DS;

/// <summary>
/// A delegate for iteration of the values within
/// </summary>
/// <param name="item">The item being iterated</param>
/// <typeparam name="T">The type of the item</typeparam>
/// <returns>
/// Function should return false if the 
/// item is to be deleted, and true otherwise
/// </returns>
public delegate bool RefActionOrDelete<T>(ref T item);

/// <summary>
/// Interface for Simultaneous Iteration and
/// Deletion Data structure (SIDS)
/// Represents a data structire that supports
/// iteration with simultaneous deletion of
/// elements within the list
/// </summary>
/// <typeparam name="T">The type of items stored</typeparam>
public interface ISidds<T> : IEnumerable<T>
{
	/// <summary>
	/// Adds an element to the data structure
	/// </summary>
	/// <param name="item">The item to add</param>
	/// <returns>true if item is successfully added, false otherwise</returns>
	bool Add(T item);
	/// <summary>
	/// The number of items in the Data Structure
	/// </summary>
	uint Count { get; }
	/// <summary>
	/// Iterate through the items in the list and 
	/// performs a given function provided in the arguments.
	/// If the function returns false, the item 
	/// is removed from the collection. Otherwise, the item
	/// remains unchanged in the list. 
	/// </summary>
	/// <param name="function">
	/// The function to perform on every item.
	/// If the item is to be deleted, the function should 
	/// return false. Otherwise, it should return true.
	/// </param>
	void IterateAndDelete(Func<T, bool> function);
	/// <summary>
	/// Iterate through the references of items in the list and 
	/// performs a given function provided in the arguments.
	/// If the function returns false, the item 
	/// is removed from the collection. Otherwise, the item
	/// remains unchanged in the list. 
	/// </summary>
	/// <param name="function">
	/// The function to perform on every item. It works on
	/// the reference of the item, so all changes within
	/// the function are done in the item in list itself.
	/// If the item is to be deleted, the function should 
	/// return false. Otherwise, it should return true.
	/// </param>
	void IterateAndDelete(RefActionOrDelete<T> function);
}
