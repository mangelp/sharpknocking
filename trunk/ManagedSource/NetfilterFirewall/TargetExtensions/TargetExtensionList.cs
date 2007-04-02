
using System;
using System.Collections;

using SharpKnocking.Common;
using SharpKnocking.NetfilterFirewall;

namespace SharpKnocking.NetfilterFirewall.ExtendedTarget
{
	public delegate void TargetExtensionListEventHandler(object sender, TargetExtensionEventArgs args);

	
	public class TargetExtensionList: CollectionBase
	{
		
        public event TargetExtensionListEventHandler ItemAdded;
	    
	    public event TargetExtensionListEventHandler ItemRemoved;
	    
	    public event EventHandler ItemsCleared;
	    
	    private NetfilterRule parentRule;
	    
	    public NetfilterRule ParentRule
	    {
	       get { return this.parentRule;}    
	    }
	    
	    private ReadOnlyTargetExtensionListAdapter adapter;
	    
	    public ReadOnlyTargetExtensionListAdapter Adapter
	    {
	       get { return this.adapter;}
	    }
	    
		public TargetExtensionList(NetfilterRule parentRule)
		  :base()
		{
		    this.parentRule = parentRule;
		    this.adapter = new ReadOnlyTargetExtensionListAdapter(this);
		}
		
		public new TargetExtensionHandler this[int index]
		{
		    get
		    {
		        return (TargetExtensionHandler)this.List[index];
		    }
		}
		
		public TargetExtensionHandler this[string index]
		{
		    get
		    {
		        string name;
		        
		        for(int i=0;i<this.Count;i++)
		        {
		            name = ((TargetExtensionHandler)this.List[i]).ExtensionName;
		            
		            if(name.ToLower().Equals(index.ToLower()))
		            {
		                return (TargetExtensionHandler)this.List[i];    
		            }
		        }
		        
		        return null;
		    }
		}
		
		public void Add(TargetExtensionHandler option)
		{
		    this.List.Add(option);
		}
		
		public bool Contains(TargetExtensionHandler option)
		{
		    return this.List.Contains(option);
		}
		
		public void Remove(Type handlerType)
		{
		    for(int i=0;i<this.Count;i++)
		    {
		        if(this.List[i].GetType()==handlerType)
		        {
		            this.RemoveAt(i);
		            return;
		        }
		    }
		}
		
		public void Remove(TargetExtensionHandler option)
		{
		    this.List.Remove(option);
		}
		
		public int IndexOf(TargetExtensionHandler option)
		{
		    return this.List.IndexOf(option);    
		}
		
		public void Insert(int index, TargetExtensionHandler option)
		{
		    this.List.Insert(index, option);
		}
		
		public TargetExtensionHandler[] ToArray()
		{
		    TargetExtensionHandler[] arr = new TargetExtensionHandler[this.List.Count];
		    
		    for(int i=0;i<arr.Length;i++)
		    {
		        arr[i] = (TargetExtensionHandler)this.List[i];    
		    }
		    
		    return arr;
		}
		
		protected virtual void OnItemRemoved(TargetExtensionHandler item)
		{
		    if(this.ItemRemoved!=null)
		    {
		        this.ItemRemoved(this,
		                         new TargetExtensionEventArgs(item));
		    }
		}
		
		protected virtual void OnItemAdded(TargetExtensionHandler item)
		{
		    if(this.ItemAdded!=null)
		    {
		        this.ItemAdded(this,
		                         new TargetExtensionEventArgs(item));
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
			this.OnItemAdded((TargetExtensionHandler)value);
		}
		
		protected override void OnRemoveComplete (int index, object value)
		{
			this.OnItemRemoved((TargetExtensionHandler)value);
		}
		
		protected override void OnSetComplete (int index, object oldValue, object newValue)
		{
		    this.OnItemRemoved((TargetExtensionHandler)oldValue);
			this.OnItemAdded((TargetExtensionHandler)newValue);
		}
	}
}
