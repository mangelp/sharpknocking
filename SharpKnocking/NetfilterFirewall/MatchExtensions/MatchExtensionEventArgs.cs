
using System;
using System.ComponentModel;

namespace SharpKnocking.NetfilterFirewall.ExtendedMatch
{
	
	public class MatchExtensionEventArgs
	{
	    private MatchExtensionHandler item;
	    
	    public MatchExtensionHandler Item
	    {
	        get { return this.item;}
	        set { this.item = value;}
	    }
	    
	    public MatchExtensionEventArgs(MatchExtensionHandler item)
	      :base()
		{
		    this.item = item;
		}
		
		public MatchExtensionEventArgs()
		  :base()
		{}
	}
}
