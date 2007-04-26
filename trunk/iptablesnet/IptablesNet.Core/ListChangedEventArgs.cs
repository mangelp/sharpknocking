
using System;

namespace IptablesNet.Core
{
	public class ListChangedEventArgs<T>: EventArgs
	{
	    private T item;
	    
	    public T Item
	    {
	       get { return this.item;}       
	       set { this.item = value;}
	    }
	    
		public ListChangedEventArgs(T item)
		{
		    this.item = item;
		}
	}
}
