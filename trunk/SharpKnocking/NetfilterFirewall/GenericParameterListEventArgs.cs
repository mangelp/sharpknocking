
using System;

namespace SharpKnocking.NetfilterFirewall
{
	public class GenericParameterListEventArgs: EventArgs
	{
	    private GenericParameter item;
	    
	    public GenericParameter Item
	    {
	       get { return this.item;}       
	       set { this.item = value;}
	    }
	    
		public GenericParameterListEventArgs(GenericParameter item)
		{
		    this.item = item;
		}
	}
}
