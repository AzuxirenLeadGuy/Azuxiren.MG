using System;
using System.Collections;
using System.Collections.Generic;

namespace Azuxiren.MG.DS;

/// <summary>
/// Linked-list Enumeration with Random-Access Deletion Structure (LERDS) is
/// a data structure that can stores and iterate item in
/// any order, and support random access deletion. Deletion is supported
/// while iteration in a custom iterator
/// </summary>
/// <typeparam name="T">Type of item being stored</typeparam>
public class Lerds<T> : ISidds<T>
where T : IEquatable<T>
{
	/// <summary>
	/// Represents a single linked list node
	/// </summary>
	protected sealed class LLNode
	{
		/// <summary>The item contained by the node</summary>
		public T Item;
		/// <summary>The next node after this node</summary>
		public LLNode? Next;
		/// <summary>The previous node after this node</summary>
		public LLNode? Prev;
		/// <summary>Constructor for the node</summary>
		public LLNode(T item)
		{
			Item = item;
			Next = null;
			Prev = null;
		}
	}
	/// <summary>The internal list represented by linked list nodes</summary>
	protected LLNode? _head;
	/// <summary>Represents the number of items in the list</summary>
	protected uint _count;
	/// <summary>The limit of number of items in the list</summary>
	protected uint _cap;
	/// <inheritdoc/>
	public uint Count => _count;
	/// <summary>Constructs an instance</summary>
	/// <param name="cap">
	/// The Number of items in the collection will
	/// be capped to this number. More precisely, if
	/// cap = x, then the list is not allowed to insert
	/// an element if there are (x-1) in the list. <br/>
	/// A value of 0 means there is no capacity limit
	/// </param>
	public Lerds(uint cap = 0)
	{
		_count = 0;
		_head = null;
		_cap = cap;
	}
	/// <inheritdoc/>
	public IEnumerator<T> GetEnumerator()
	{
		LLNode? x = _head;
		while (x != null)
		{
			yield return x.Item;
			x = x.Next;
		}
	}
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	/// <inheritdoc/>
	public bool Add(T item)
	{
		if (_count + 1 == _cap) return false;
		if (item == null) return false;
		LLNode node = new(item) { Next = _head, Prev = null };
		node.Next = _head;
		if (_head != null)
			_head.Prev = node;
		_head = node;
		_count++;
		return true;
	}
	/// <summary>
	/// Deletes a given node from the list
	/// </summary>
	/// <param name="item">The node to delete</param>
	/// <returns>true if the deletion is successful, otherwise false</returns>
	protected bool Delete(LLNode item)
	{
		if (item == null) return false;
		LLNode? prev = item.Prev, next = item.Next;
		if (prev == null) // Only possible for _head
		{
			if (next == null)
			{
				_head = null;
			}
			else
			{
				next.Prev = null;
				_head = next;
			}
		}
		else if (next == null) // Only possible for the last element in the list
		{
			prev.Next = item.Prev = null;
		}
		else // Any element in the middle
		{
			prev.Next = next;
			next.Prev = prev;
		}
		item.Next = item.Prev = null;
		_count--;
		return true;
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
		LLNode? x = _head, y;
		while (x != null)
		{
			y = x.Next;
			if (!function(ref x.Item))
				Delete(x);
			x = y;
		}
	}
	/// <inheritdoc/>
	public void Clear() => _head = null;
}