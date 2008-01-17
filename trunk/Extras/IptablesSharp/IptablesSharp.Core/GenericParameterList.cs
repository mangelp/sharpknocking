// GenericParameterList.cs
//
//  Copyright (C) 2007 iSharpKnocking project
//  Created by Miguel Angel Perez (mangelp{@}gmail{d0t}com)
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

namespace IptablesNet.Core
{
	/// <summary>
	/// Models a list of parameters where each one must be a subtype of SimpleParameter
	/// </summary>
	public class GenericParameterList<T>: CollectionBase where T:SimpleParameter
	{
		/// <summary>
		/// Delegate to notify changes in the list
		/// </summary>
		public delegate void ListChangedEventHandler(object sender, ListChangedEventArgs<T> args);
		
		/// <summary>
		/// Notifies that a new element have been added to the list
		/// </summary>
        public event ListChangedEventHandler ItemAdded;
	    
		/// <summary>
		/// Notifies that an existing element have been removed from the list
		/// </summary>
	    public event ListChangedEventHandler ItemRemoved;
	    
		/// <summary>
		/// Notifies that all the elements in the list have been erased
		/// </summary>
	    public event EventHandler ItemsCleared;
	    
		/// <summary>
		/// Constructor
		/// </summary>
		public GenericParameterList()
		  :base()
		{}
		
		/// <summary>
		/// Indexer for the class to return elements by its index
		/// </summary>
		public T this[int index]
		{
		    get {
		        return (T)this.List[index];
		    }
		}
		
		/// <summary>
		/// Adds a new element to the list
		/// </summary>
		/// <param name="option">
		/// A <see cref="T"/> element to add
		/// </param>
		public void Add(T option)
		{
		    this.List.Add(option);
		}
		
		/// <summary>
		/// Gets if an element exists within the list
		/// </summary>
		/// <param name="option">
		/// A <see cref="T"/> to look for in the list
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/> with a value of true if the element
		/// was in the list or false if not.
		/// </returns>
		public bool Contains(T option)
		{
		    return this.List.Contains(option);
		}
		
		/// <summary>
		/// Removes an element of the list
		/// </summary>
		/// <param name="option">
		/// A <see cref="T"/> element to remove
		/// </param>
		public void Remove(T option)
		{
		    this.List.Remove(option);
		}
		
		/// <summary>
		/// Gets the zero based position of an element in the list
		/// </summary>
		/// <param name="option">
		/// A <see cref="T"/> to find in the list
		/// </param>
		/// <returns>
		/// A <see cref="System.Int32"/> with the position of the element in
		/// the list or -1 if the element is not found.
		/// </returns>
		public int IndexOf(T option)
		{
		    return this.List.IndexOf(option);    
		}
		
		/// <summary>
		/// Gets the zero based position of an element in the list
		/// </summary>
		/// <param name="name">
		/// A <see cref="System.String"/> with the name of the option to find
		/// </param>
		/// <returns>
		/// A <see cref="System.Int32"/> with the position of the element in
		/// the list or -1 if the element is not found.
		/// </returns>
		public int IndexOf(string name)
		{
		    T par;
		    
		    for(int i=0;i<this.Count;i++)
		    {
		        par = (T)this.List[i];
		        
		        if(par.Name.Equals(name, StringComparison.InvariantCulture) || par.IsAlias(name))
		            return i;
		    }
		    
		    return -1;
		}
		
		public bool ContainsName(string name)
		{
		    if(this.IndexOf (name)==0)
				return true;
			return false;
		}
		
		public void Insert(int index, T option)
		{
		    this.List.Insert(index, option);
		}
		
		public T[] ToArray()
		{
		    T[] arr = new T[this.List.Count];
		    
		    for(int i=0;i<arr.Length;i++)
		    {
		        arr[i] = (T)this.List[i];    
		    }
		    
		    return arr;
		}
		
		protected virtual void OnItemRemoved(T item)
		{
		    if(this.ItemRemoved!=null)
		    {
		        this.ItemRemoved(this,
		                         new ListChangedEventArgs<T>(item));
		    }
		}
		
		protected virtual void OnItemAdded(T item)
		{
		    if(this.ItemAdded!=null)
		    {
		        this.ItemAdded(this,
		                         new ListChangedEventArgs<T>(item));
		    }  
		}
		
		protected virtual void OnItemsCleared()
		{
		    if(this.ItemsCleared!=null)
		    {
		        this.ItemsCleared(this, EventArgs.Empty);    
		    }
		}
		
		protected override void OnClearComplete ()
		{
			this.OnItemsCleared();
		}

		
		protected override void OnInsertComplete (int index, object value)
		{
			this.OnItemAdded((T)value);
		}
		
		protected override void OnRemoveComplete (int index, object value)
		{
			this.OnItemRemoved((T)value);
		}
		
		protected override void OnSetComplete (int index, object oldValue, object newValue)
		{
		    this.OnItemRemoved((T)oldValue);
			this.OnItemAdded((T)newValue);
		}

		


	}
}
