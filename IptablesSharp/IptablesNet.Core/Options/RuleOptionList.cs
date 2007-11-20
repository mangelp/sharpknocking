// RuleOptionList.cs
//
//  Copyright (C) 2006 SharpKnocking project
//  Created by Miguel Angel Pérez, mangelp@gmail.com
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
using System;
using System.Collections;

namespace IptablesNet.Core.Options
{
    public delegate void GenericOptionListEventHandler(object sender, ListChangedEventArgs<GenericOption> args);
	    
    /// <summary>
    /// List of GenericOption elements
    /// </summary>
	public class RuleOptionList: System.Collections.CollectionBase
	{
	    
	    public event GenericOptionListEventHandler ItemAdded;
	    
	    public event GenericOptionListEventHandler ItemRemoved;
	    
	    public event EventHandler ItemsCleared;
	    
	    private NetfilterRule parentRule;
	    
  	    public NetfilterRule ParentRule
	    {
	       get { return this.parentRule;}    
	    }
	    
		public RuleOptionList(NetfilterRule parentRule)
		  :base()
		{
		    this.parentRule = parentRule;
		}
		
		public GenericOption this[int index]
		{
		    get
		    {       
		        return (GenericOption)this.InnerList[index];
		    }
		}
		
		public void Add(GenericOption option)
		{
		    if(option.ParentRule!=null)
		    {
		      option.ParentRule.Options.Remove(option);    
		    }
		    
		    this.List.Add(option);
		    
		    option.ParentRule = this.parentRule;
		}
		
		public bool Contains(GenericOption option)
		{
		    return this.List.Contains(option);
		}
		
		public void Remove(GenericOption option)
		{
		    this.List.Remove(option);
		    
		    option.ParentRule = null;
		}
		
		public int IndexOf(GenericOption option)
		{
		    return this.List.IndexOf(option);    
		}
		
		public void Insert(int index, GenericOption option)
		{
		    if(option.ParentRule!=null)
		        option.ParentRule.Options.Remove(option);
		    
		    this.List.Insert(index, option);
		    
		    option.ParentRule = this.parentRule;
		}
		
		public GenericOption[] ToArray()
		{
		    GenericOption[] arr = new GenericOption[this.List.Count];
		    
		    for(int i=0;i<arr.Length;i++)
		    {
		        arr[i] = (GenericOption)this.List[i];    
		    }
		    
		    return arr;
		}
		
		protected virtual void OnItemRemoved(GenericOption item)
		{
		    if(this.ItemRemoved!=null)
		    {
		        this.ItemRemoved(this,
		                         new ListChangedEventArgs<GenericOption>(item));
		    }
		}
		
		protected virtual void OnItemAdded(GenericOption item)
		{
		    if(this.ItemAdded!=null)
		    {
		        this.ItemAdded(this,
		                         new ListChangedEventArgs<GenericOption>(item));
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
			this.OnItemAdded((GenericOption)value);
		}
		
		protected override void OnRemoveComplete (int index, object value)
		{
			this.OnItemRemoved((GenericOption)value);
		}
		
		protected override void OnSetComplete (int index, object oldValue, object newValue)
		{
		    this.OnItemRemoved((GenericOption)oldValue);
			this.OnItemAdded((GenericOption)newValue);
		}


	}
}
