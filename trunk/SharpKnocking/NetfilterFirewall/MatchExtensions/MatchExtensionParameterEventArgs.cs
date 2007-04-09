
using System;
using System.Collections;

using SharpKnocking.Common;

namespace SharpKnocking.NetfilterFirewall.ExtendedMatch
{
	
	
	public class MatchExtensionParameterEventArgs: EventArgs
	{
	    private MatchExtensionParameter item;
	    
	    public MatchExtensionParameter Item
	    {
	        get { return item;}
	        set { item = value;}
	    }
		
		public MatchExtensionParameterEventArgs()
		  :base()
		{
		}
		
		public MatchExtensionParameterEventArgs(MatchExtensionParameter item)
		  :base()
		{
		    this.item = item;
		}
	}
}
