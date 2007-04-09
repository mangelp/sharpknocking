
using System;
using SharpKnocking.Common;
using System.Collections;

using SharpKnocking.NetfilterFirewall;

namespace SharpKnocking.NetfilterFirewall.ExtendedTarget
{
    public delegate void TargetExtensionParameterListEventHandler(object sender, TargetExtensionParameterEventArgs args);
	
	public class TargetExtensionParameterList: CollectionBase
	{
        public event TargetExtensionParameterListEventHandler ItemAdded;
	    
	    public event TargetExtensionParameterListEventHandler ItemRemoved;
	    
	    public event EventHandler ItemsCleared;
	    
	    private TargetExtensionHandler extension;
	    
	    public TargetExtensionHandler Extension
	    {
	       get { return this.extension;}    
	    }
	    
		public TargetExtensionParameterList(TargetExtensionHandler extension)
		  :base()
		{
		    this.extension = extension;
		}
		
		public new TargetExtensionParameter this[int index]
		{
		    get
		    {
		        return (TargetExtensionParameter)this.InnerList[index];
		    }
		}
		
		public void Add(TargetExtensionParameter param)
		{
		    if(param.Owner!=null && param.Owner.Contains(param.Name))
		    {
		        param.Owner.RemoveParameter(param.Name);
		    }
		    
		    this.List.Add(param);
		    
		    param.Owner = this.extension;
		}
		
		public bool Contains(TargetExtensionParameter param)
		{
		    return this.List.Contains(param);
		}
		
		public void Remove(TargetExtensionParameter option)
		{
		    this.List.Remove(option);
		    
		    option.Owner = null;
		}
		
		public int IndexOf(TargetExtensionParameter option)
		{
		    return this.List.IndexOf(option);    
		}
		
		public void Insert(int index, TargetExtensionParameter option)
		{
		    if(option.Owner!=null)
		        option.Owner.RemoveParameter(option.Name);
		    
		    this.List.Insert(index, option);
		    
		    option.Owner = this.extension;
		}
		
		public TargetExtensionParameter[] ToArray()
		{
		    TargetExtensionParameter[] arr = new TargetExtensionParameter[this.List.Count];
		    
		    for(int i=0;i<arr.Length;i++)
		    {
		        arr[i] = (TargetExtensionParameter)this.List[i];    
		    }
		    
		    return arr;
		}
		
		protected virtual void OnItemRemoved(TargetExtensionParameter item)
		{
		    if(this.ItemRemoved!=null)
		    {
		        this.ItemRemoved(this,
		                         new TargetExtensionParameterEventArgs(item));
		    }
		}
		
		protected virtual void OnItemAdded(TargetExtensionParameter item)
		{
		    if(this.ItemAdded!=null)
		    {
		        this.ItemAdded(this,
		                         new TargetExtensionParameterEventArgs(item));
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
			this.OnItemAdded((TargetExtensionParameter)value);
		}
		
		protected override void OnRemoveComplete (int index, object value)
		{
			this.OnItemRemoved((TargetExtensionParameter)value);
		}
		
		protected override void OnSetComplete (int index, object oldValue, object newValue)
		{
		    this.OnItemRemoved((TargetExtensionParameter)oldValue);
			this.OnItemAdded((TargetExtensionParameter)newValue);
		}
	}
}
