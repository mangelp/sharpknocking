
using System;
using System.Collections;
using SharpKnocking.Common;

namespace SharpKnocking.NetfilterFirewall.Options
{
    public delegate void GenericOptionListEventHandler(object sender, GenericOptionListEventArgs args);
	    
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
		
		public new GenericOption this[int index]
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
		                         new GenericOptionListEventArgs(item));
		    }
		}
		
		protected virtual void OnItemAdded(GenericOption item)
		{
		    if(this.ItemAdded!=null)
		    {
		        this.ItemAdded(this,
		                         new GenericOptionListEventArgs(item));
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
