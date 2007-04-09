
using System;

namespace SharpKnocking.NetfilterFirewall.Options
{
	public class GenericOptionListEventArgs: EventArgs
	{
	    private GenericOption item;
	    
	    public GenericOption Item
	    {
	        get { return this.item;}
	        set { this.item = value;}
	    }
	    
		public GenericOptionListEventArgs(GenericOption item)
		{
		    this.item = item;
		}
	}
}
