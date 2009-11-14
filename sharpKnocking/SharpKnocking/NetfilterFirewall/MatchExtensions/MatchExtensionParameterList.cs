
using System;
using System.Collections;

using SharpKnocking.NetfilterFirewall;

namespace SharpKnocking.NetfilterFirewall.ExtendedMatch
{
    public delegate void MatchExtensionParameterListEventHandler(object sender, MatchExtensionParameterEventArgs args);
    
	public class MatchExtensionParameterList: CollectionBase
	{
		
        public event MatchExtensionParameterListEventHandler ItemAdded;
	    
	    public event MatchExtensionParameterListEventHandler ItemRemoved;
	    
	    public event EventHandler ItemsCleared;
	    
	    private MatchExtensionHandler extension;
	    
	    public MatchExtensionHandler Extension
	    {
	       get { return this.extension;}    
	    }
	    
		public MatchExtensionParameterList(MatchExtensionHandler extension)
		  :base()
		{
		    this.extension = extension;
		}
		
		public new MatchExtensionParameter this[int index]
		{
		    get
		    {
		        return (MatchExtensionParameter)this.InnerList[index];
		    }
		}
		
		public void Add(MatchExtensionParameter param)
		{
		    if(param.Owner!=null && param.Owner.Contains(param.Name))
		    {
		        param.Owner.RemoveParameter(param.Name);
		    }
		    
		    this.List.Add(param);
		    
		    param.Owner = this.extension;
		}
		
		public bool Contains(MatchExtensionParameter param)
		{
		    return this.List.Contains(param);
		}
		
		public void Remove(MatchExtensionParameter option)
		{
		    this.List.Remove(option);
		    
		    option.Owner = null;
		}
		
		public int IndexOf(MatchExtensionParameter option)
		{
		    return this.List.IndexOf(option);    
		}
		
		public void Insert(int index, MatchExtensionParameter option)
		{
		    if(option.Owner!=null)
		        option.Owner.RemoveParameter(option.Name);
		    
		    this.List.Insert(index, option);
		    
		    option.Owner = this.extension;
		}
		
		public MatchExtensionParameter[] ToArray()
		{
		    MatchExtensionParameter[] arr = new MatchExtensionParameter[this.List.Count];
		    
		    for(int i=0;i<arr.Length;i++)
		    {
		        arr[i] = (MatchExtensionParameter)this.List[i];    
		    }
		    
		    return arr;
		}
		
		protected virtual void OnItemRemoved(MatchExtensionParameter item)
		{
		    if(this.ItemRemoved!=null)
		    {
		        this.ItemRemoved(this,
		                         new MatchExtensionParameterEventArgs(item));
		    }
		}
		
		protected virtual void OnItemAdded(MatchExtensionParameter item)
		{
		    if(this.ItemAdded!=null)
		    {
		        this.ItemAdded(this,
		                         new MatchExtensionParameterEventArgs(item));
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
			this.OnItemAdded((MatchExtensionParameter)value);
		}
		
		protected override void OnRemoveComplete (int index, object value)
		{
			this.OnItemRemoved((MatchExtensionParameter)value);
		}
		
		protected override void OnSetComplete (int index, object oldValue, object newValue)
		{
		    this.OnItemRemoved((MatchExtensionParameter)oldValue);
			this.OnItemAdded((MatchExtensionParameter)newValue);
		}
	}
}
