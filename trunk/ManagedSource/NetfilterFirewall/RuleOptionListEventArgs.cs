
using System;

namespace SharpKnocking.IpTablesManager.RuleHandling
{
	public class RuleOptionListEventArgs: EventArgs
	{
	    private RuleOption item;
	    
	    public RuleOption Item
	    {
	        get { return this.item;}
	        set { this.item = value;}
	    }
	    
		public RuleOptionListEventArgs(RuleOption item)
		{
		    this.item = item;
		}
	}
}
