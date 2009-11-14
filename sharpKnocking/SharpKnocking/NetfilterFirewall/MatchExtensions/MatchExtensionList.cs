
using System;
using System.Collections;

using SharpKnocking.Common;
using SharpKnocking.NetfilterFirewall;

namespace SharpKnocking.NetfilterFirewall.ExtendedMatch
{
    public delegate void MatchExtensionListEventHandler(object sender, MatchExtensionEventArgs args);
	
	public class MatchExtensionList: CollectionBase
	{
	    
        public event MatchExtensionListEventHandler ItemAdded;
	    
	    public event MatchExtensionListEventHandler ItemRemoved;
	    
	    public event EventHandler ItemsCleared;
	    
	    private NetfilterRule parentRule;
	    
	    public NetfilterRule ParentRule
	    {
	       get { return this.parentRule;}    
	    }
	    
	    private ReadOnlyMatchExtensionListAdapter adapter;
	    
	    public ReadOnlyMatchExtensionListAdapter Adapter
	    {
	       get { return this.adapter;}
	    }
	    
		public MatchExtensionList(NetfilterRule parentRule)
		  :base()
		{
		    this.parentRule = parentRule;
		    this.adapter = new ReadOnlyMatchExtensionListAdapter(this);
		}
		
		public new MatchExtensionHandler this[int index]
		{
		    get
		    {
		        return (MatchExtensionHandler)this.List[index];
		    }
		}
		
		public MatchExtensionHandler this[string index]
		{
		    get
		    {
		        string name;
		        index = index.ToLower();
		        
		        for(int i=0;i<this.Count;i++)
		        {
		            name = ((MatchExtensionHandler)this.List[i]).ExtensionName;
		            
		            if(name.ToLower().Equals(index))
		            {
		                return (MatchExtensionHandler)this.List[i];    
		            }
		        }
		        
		        Debug.VerboseWrite("MatchExtensionList:: Not found "+index+" ("+this.List.Count+")");
		        return null;
		    }
		}
		
		public void Add(MatchExtensionHandler option)
		{
		    Debug.VerboseWrite ("MatchExtensionList::Add(): "+option);
		    this.List.Add(option);
		}
		
		public bool Contains(MatchExtensionHandler option)
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
		
		public void Remove(MatchExtensionHandler option)
		{
		    this.List.Remove(option);
		}
		
		public int IndexOf(MatchExtensionHandler option)
		{
		    return this.List.IndexOf(option);    
		}
		
		public void Insert(int index, MatchExtensionHandler option)
		{
		    this.List.Insert(index, option);
		}
		
		public MatchExtensionHandler[] ToArray()
		{
		    MatchExtensionHandler[] arr = new MatchExtensionHandler[this.List.Count];
		    
		    for(int i=0;i<arr.Length;i++)
		    {
		        arr[i] = (MatchExtensionHandler)this.List[i];    
		    }
		    
		    return arr;
		}
		
		protected virtual void OnItemRemoved(MatchExtensionHandler item)
		{
		    if(this.ItemRemoved!=null)
		    {
		        this.ItemRemoved(this,
		                         new MatchExtensionEventArgs(item));
		    }
		}
		
		protected virtual void OnItemAdded(MatchExtensionHandler item)
		{
		    if(this.ItemAdded!=null)
		    {
		        this.ItemAdded(this,
		                         new MatchExtensionEventArgs(item));
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
			this.OnItemAdded((MatchExtensionHandler)value);
		}
		
		protected override void OnRemoveComplete (int index, object value)
		{
			this.OnItemRemoved((MatchExtensionHandler)value);
		}
		
		protected override void OnSetComplete (int index, object oldValue, object newValue)
		{
		    this.OnItemRemoved((MatchExtensionHandler)oldValue);
			this.OnItemAdded((MatchExtensionHandler)newValue);
		}
	}
}
