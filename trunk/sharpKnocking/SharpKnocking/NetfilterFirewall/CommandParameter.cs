
using System;
using System.Net;

namespace SharpKnocking.IpTablesManager.RuleHandling
{
	
	/// <summary>
	/// Models a parameter in a iptables command
	/// </summary>
	public class CommandParameter: BaseCommandParameter
	{

	    
	    public CommandParameter()
	    	:base()
		{
		    
		}
		
		public static bool IsCommandParameter(string param)
		{
		    try
		    {		        
    		    if(param[0]=='-')
    		    {
    		        Enum.Parse(typeof(CommandParameters), param.Substring(1), true);        
    		    }
    		    else
    		    {
    		        Enum.Parse(typeof(CommandParameters), param, true);    
    		    }
		    }
		    catch(Exception)
		    {
                return false;		            
		    }
		    
		    return true;
		}
		
	}
}
