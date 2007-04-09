using System;

namespace SharpKnocking.IpTablesManager.RuleHandling
{
	
	
	public class ExtendedMatchCommandParameter: BaseCommandParameter
	{
	    private MatchExtensions type;
	    
	    public MatchExtensions Type
	    {
	        get{return this.type;}    
	    }
	    
	    public ExtendedMatchCommandParameter()
	    :base()
	    {
            
	    }
	}
}
