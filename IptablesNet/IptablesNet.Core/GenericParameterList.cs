
using System;
using System.Collections;

namespace IptablesNet.Core
{
	
	public class GenericParameterList<T>: CollectionBase where T:SimpleParameter
	{
		public delegate void ListChangedEventHandler(object sender, ListChangedEventArgs<T> args);
		
        public event ListChangedEventHandler ItemAdded;
	    
	    public event ListChangedEventHandler ItemRemoved;
	    
	    public event EventHandler ItemsCleared;
	    
		public GenericParameterList()
		  :base()
		{

		}
		
		public T this[int index]
		{
		    get
		    {
		        return (T)this.List[index];
		    }
		}
		
		public void Add(T option)
		{
		    this.List.Add(option);
		}
		
		public bool Contains(T option)
		{
		    return this.List.Contains(option);
		}
		
		public void Remove(T option)
		{
		    this.List.Remove(option);
		}
		
		public int IndexOf(T option)
		{
		    return this.List.IndexOf(option);    
		}
		
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
