
using System;
using SharpKnocking.Common;

using SharpKnocking.IpTablesManager.RuleHandling;

namespace SharpKnocking.IpTablesManager.RuleHandling.ExtendedTarget
{
	
	
	public class TargetExtensionParameterEventHandler: EventHandler
	{
		
	    private TargetExtensionParameter item;
	    
	    public TargetExtensionParameter Item
	    {
	        get { return item;}
	        set { item = value;}
	    }
		
		public TargetExtensionParameterEventArgs()
		  :base()
		{
		}
		
		public TargetExtensionParameterEventArgs(TargetExtensionParameter item)
		  :base()
		{
		    this.item = item;
		}
	}
}
