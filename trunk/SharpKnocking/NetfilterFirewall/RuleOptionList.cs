
using System;
using System.Collections;
using SharpKnocking.Common;

namespace SharpKnocking.IpTablesManager.RuleHandling
{
    public delegate void RuleOptionListEventHandler(object sender, RuleOptionListEventArgs args);
	    
    /// <summary>
    /// List of RuleOption elements
    /// </summary>
	public class RuleOptionList: System.Collections.CollectionBase
	{
	    
	    public event RuleOptionListEventHandler ItemAdded;
	    
	    public event RuleOptionListEventHandler ItemRemoved;
	    
	    public event EventHandler ItemsCleared;
	    
	    private IpTablesRule parentRule;
	    
  	    public IpTablesRule ParentRule
	    {
	       get { return this.parentRule;}    
	    }
	    
		public RuleOptionList(IpTablesRule parentRule)
		  :base()
		{
		    this.parentRule = parentRule;
		}
		
		public new RuleOption this[int index]
		{
		    get
		    {
		        if(this.InnerList.Count>=index || index<0)
		            throw new IndexOutOfRangeException();
		        
		        return (RuleOption)this.InnerList[index];
		    }
		}
		
		public void Add(RuleOption option)
		{
		    if(option.ParentRule!=null)
		    {
		      option.ParentRule.Options.Remove(option);    
		    }
		    
		    this.List.Add(option);
		    
		    option.ParentRule = this.parentRule;
		}
		
		public bool Contains(RuleOption option)
		{
		    return this.List.Contains(option);
		}
		
		public void Remove(RuleOption option)
		{
		    this.List.Remove(option);
		    
		    option.ParentRule = null;
		}
		
		public int IndexOf(RuleOption option)
		{
		    return this.List.IndexOf(option);    
		}
		
		public void Insert(int index, RuleOption option)
		{
		    if(option.ParentRule!=null)
		        option.ParentRule.Options.Remove(option);
		    
		    this.List.Insert(index, option);
		    
		    option.ParentRule = this.parentRule;
		}
		
		public RuleOption[] ToArray()
		{
		    RuleOption[] arr = new RuleOption[this.List.Count];
		    
		    for(int i=0;i<arr.Length;i++)
		    {
		        arr[i] = (RuleOption)this.List[i];    
		    }
		    
		    return arr;
		}
		
		protected virtual void OnItemRemoved(RuleOption item)
		{
		    if(this.ItemRemoved!=null)
		    {
		        this.ItemRemoved(this,
		                         new RuleOptionListEventArgs(item));
		    }
		}
		
		protected virtual void OnItemAdded(RuleOption item)
		{
		    if(this.ItemAdded!=null)
		    {
		        this.ItemAdded(this,
		                         new RuleOptionListEventArgs(item));
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
			this.OnItemAdded((RuleOption)value);
		}
		
		protected override void OnRemoveComplete (int index, object value)
		{
			this.OnItemRemoved((RuleOption)value);
		}
		
		protected override void OnSetComplete (int index, object oldValue, object newValue)
		{
		    this.OnItemRemoved((RuleOption)oldValue);
			this.OnItemAdded((RuleOption)newValue);
		}


	}
}
