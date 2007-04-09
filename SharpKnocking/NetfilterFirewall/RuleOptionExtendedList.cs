
using System;
using System.Collections;
using SharpKnocking.Common;

namespace SharpKnocking.IpTablesManager.RuleHandling
{

    /// <summary>
    /// List of RuleOptionExtended elements
    /// </summary>
	public class RuleOptionExtendedList: CollectionBase
	{
	    public event RuleOptionListEventHandler ItemAdded;
	    
	    public event RuleOptionListEventHandler ItemRemoved;
	    
	    public event EventHandler ItemsCleared;
	    
	    private IpTablesRule parentRule;
	    
	    public IpTablesRule ParentRule
	    {
	       get { return this.parentRule;}    
	    }
	    
		public RuleOptionExtendedList(IpTablesRule parentRule)
		  :base()
		{
		    this.parentRule = parentRule;
		}
		
		public new RuleOptionExtended this[int index]
		{
		    get
		    {
		        if(this.InnerList.Count>=index || index<0)
		            throw new IndexOutOfRangeException();
		        
		        return (RuleOptionExtended)this.InnerList[index];
		    }
		}
		
		public void Add(RuleOptionExtended option)
		{
		    if(option.ParentRule!=null)
		    {
		      option.ParentRule.Options.Remove(option);    
		    }
		    
		    this.List.Add(option);
		    
		    option.ParentRule = this.parentRule;
		}
		
		public bool Contains(RuleOptionExtended option)
		{
		    return this.List.Contains(option);
		}
		
		public void Remove(RuleOptionExtended option)
		{
		    this.List.Remove(option);
		    
		    option.ParentRule = null;
		}
		
		public int IndexOf(RuleOptionExtended option)
		{
		    return this.List.IndexOf(option);    
		}
		
		public void Insert(int index, RuleOptionExtended option)
		{
		    if(option.ParentRule!=null)
		        option.ParentRule.Options.Remove(option);
		    
		    this.List.Insert(index, option);
		    
		    option.ParentRule = this.parentRule;
		}
		
		public RuleOptionExtended[] ToArray()
		{
		    RuleOptionExtended[] arr = new RuleOptionExtended[this.List.Count];
		    
		    for(int i=0;i<arr.Length;i++)
		    {
		        arr[i] = (RuleOptionExtended)this.List[i];    
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
