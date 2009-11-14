
using System;
using SharpKnocking.Common;

using SharpKnocking.NetfilterFirewall;

namespace SharpKnocking.NetfilterFirewall.ExtendedTarget
{
	
	
	public class TargetExtensionEventArgs
	{
		
	    private TargetExtensionHandler item;
	    
	    public TargetExtensionHandler Item
	    {
	        get { return this.item;}
	        set { this.item = value;}
	    }
	    
	    public TargetExtensionEventArgs(TargetExtensionHandler item)
	      :base()
		{
		    this.item = item;
		}
		
		public TargetExtensionEventArgs()
		  :base()
		{}
	}
}
