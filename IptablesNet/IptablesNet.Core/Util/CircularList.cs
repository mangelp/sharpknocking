// CircularList.cs
//
//  Copyright (C) 2007 iSharpKnocking project
//  Created by mangelp<@>gmail[*]com
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA 
//
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace IptablesNet.Core
{
	/// <summary>
	/// Generic class to model a circular list of elements with a certain capacity
	/// </summary>
	/// <remarks>
	/// Once the maximum capacity of the list is reached the elements added
	/// will overwrite the list starting for the first one.
	/// Each list instance is synchronized by default using a lock to prevent bad behaviour
	/// when using more than one thread to operate it.
	/// </remarks>
	public class CircularList<T>
		where T:class
	{
		private object lockObj = new Object();
		private List<T> innerList;
		//Last inserted item index
		private int start = -1;
		
		/// <value>
		/// Item in the list
		/// </value>
		/// <summary>Returns the item if it exists any item or null otherwise</summary>
		/// <remarks>The index of the item is calculated over the current number of 
		/// elements in the list so any number will be asigned an index in the list unless
		/// is empty.</remarks>
		public T this[int relativeIndex]
		{
			get {
				return this.GetItemAt(relativeIndex);
			}
		}
		
		/// <value>
		/// Capacity of the circular list.
		/// </value>
		public int Capacity
		{
			get {
				return this.innerList.Capacity;
			}
		}
		
		/// <summary>
		/// Creates a new circular list
		/// </summary>
		/// <param name="capac">Capacity of the list</param>
		public CircularList(int capac)
		{
			this.innerList = new List<T>(capac);
		}
		
		/// <value>
		/// Number of elements introduced in the list
		/// </value>
		/// <summary>Gets the number of elements introduced in the list</summary>
		public int Count
		{
			get {
				lock(lockObj)
				{
					return this.innerList.Count;
				}
			}
		}
		
		/// <summary>
		/// Adds a new item to the list.
		/// </summary>
		/// <param name="item">Item to add to the list</param>
		public void Add(T item)
		{
			lock(lockObj)
			{
				if(this.start == -1)
				{
					this.start = 0;
					this.innerList.Add (item);
				}
				if(this.innerList.Count == this.innerList.Capacity)
				{
					if((this.start+1) == this.innerList.Capacity)
						this.start = 0;
					else
						this.start ++;
					this.innerList[this.start] = item;
				}
				else if(this.innerList.Count < this.innerList.Capacity )
				{
					this.innerList.Add (item);
					this.start = this.innerList.Count -1;
				}
				else
				{
					throw new InvalidOperationException("Internal error. Can't figure where to add the item.");
				}
			}
		}
		
		/// <summary>
		/// Gets the last item inserted in the circular list.
		/// </summary>
		/// <returns>A item if the list has items or null otherwise</returns>
		public T GetLastItem()
		{
			lock(lockObj)
			{
				if(this.start>=0 && this.innerList.Count>0)
					return this.innerList[this.start];
			}			
			
			return null;
		}
		
		/// <summary>
		/// Gets an item from the list
		/// </summary>
		/// <param name="relativeIndex">Index from the older item inserted</param>
		/// <returns>An item if the index is a valid item in the list or null if 
		/// there is no items in the list.</returns>
		public T GetItemAt(int relativeIndex)
		{
			int pos = (this.start+1+relativeIndex)%this.innerList.Count;
			if(0<=pos && pos<this.innerList.Count)
				return this.innerList[pos];
			
			return null;
		}
		
		/// <summary>
		/// Gets the next index from a relative index.
		/// </summary>
		/// <param name="relativeIndex">Index where to start</param>
		/// <returns></returns>
		public int GetNextIndex(int relativeIndex)
		{
			return (this.start+2+relativeIndex)%this.innerList.Count;
		}
		
		/// <summary>
		/// Gets an array with all the items in the circular list.
		/// </summary>
		/// <returns></returns>
		public T[] ToArray()
		{
			List<T> res = new List<T>();
			lock(lockObj)
			{
				for(int i = this.start;i<this.innerList.Count ;i++)
					res.Add (this.innerList[i]);
			}	
			return res.ToArray ();
		}
				
		public void Clear()
		{
			this.innerList.Clear();		
		}
		
	}
}
