
using System;
using System.Collections;

namespace SharpKnocking.NetfilterFirewall
{
	public delegate void GenericParameterListEventHandler(object sender, GenericParameterListEventArgs args);
	
	public class GenericParameterList: CollectionBase
	{
		
        public event GenericParameterListEventHandler ItemAdded;
	    
	    public event GenericParameterListEventHandler ItemRemoved;
	    
	    public event EventHandler ItemsCleared;
	    
		public GenericParameterList()
		  :base()
		{

		}
		
		public new GenericParameter this[int index]
		{
		    get
		    {
		        return (GenericParameter)this.List[index];
		    }
		}
		
		public void Add(GenericParameter option)
		{
		    this.List.Add(option);
		}
		
		public bool Contains(GenericParameter option)
		{
		    return this.List.Contains(option);
		}
		
		public void Remove(GenericParameter option)
		{
		    this.List.Remove(option);
		}
		
		public int IndexOf(GenericParameter option)
		{
		    return this.List.IndexOf(option);    
		}
		
		public int IndexOf(string name)
		{
		    GenericParameter par;
		    
		    for(int i=0;i<this.Count;i++)
		    {
		        par = (GenericParameter)this.List[i];
		        
		        if(par.Name==name)
		            return i;
		    }
		    
		    return -1;
		}
		
		public bool ContainsName(string name)
		{
		    GenericParameter par;
		    
		    for(int i=0;i<this.Count;i++)
		    {
		        par = (GenericParameter)this.List[i];
		        
		        if(par.Name==name)
		            return true;
		    }
		    
		    return false;
		}
		
		public void Insert(int index, GenericParameter option)
		{
		    this.List.Insert(index, option);
		}
		
		public GenericParameter[] ToArray()
		{
		    GenericParameter[] arr = new GenericParameter[this.List.Count];
		    
		    for(int i=0;i<arr.Length;i++)
		    {
		        arr[i] = (GenericParameter)this.List[i];    
		    }
		    
		    return arr;
		}
		
		protected virtual void OnItemRemoved(GenericParameter item)
		{
		    if(this.ItemRemoved!=null)
		    {
		        this.ItemRemoved(this,
		                         new GenericParameterListEventArgs(item));
		    }
		}
		
		protected virtual void OnItemAdded(GenericParameter item)
		{
		    if(this.ItemAdded!=null)
		    {
		        this.ItemAdded(this,
		                         new GenericParameterListEventArgs(item));
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
			this.OnItemAdded((GenericParameter)value);
		}
		
		protected override void OnRemoveComplete (int index, object value)
		{
			this.OnItemRemoved((GenericParameter)value);
		}
		
		protected override void OnSetComplete (int index, object oldValue, object newValue)
		{
		    this.OnItemRemoved((GenericParameter)oldValue);
			this.OnItemAdded((GenericParameter)newValue);
		}

		


	}
}
