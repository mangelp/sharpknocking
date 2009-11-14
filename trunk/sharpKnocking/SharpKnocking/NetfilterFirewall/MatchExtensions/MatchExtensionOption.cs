
using System;
using System.Collections;

using SharpKnocking.Common;
using SharpKnocking.IpTablesManager.RuleHandling;

namespace SharpKnocking.IpTablesManager.RuleHandling.Extensions
{
	
	/// <summary>
	/// Base class for all the implementations of a match extension.
	/// </summary>
	/// <remarks>
	/// When extending this class the names of each class must
	/// follow this fully qualified scheme:<br/>
    /// SharpKnocking.IpTablesManager.RuleHandling.Extensions.[EnumName]Extension
    /// Where [EnumName] must be replaced by the name of the enum that
    /// represents the match extension type used if the extension is one included
    /// with iptables.<br/>
    /// If the extension is custom sorry but this is not supported and the
    /// behaviour is not tested in this case.
	/// </remarks>
	public class MatchExtensionOption
	{
	    
	     //cache for decoding names as options
	    private static Hashtable optNameCache;
	    
	    static MatchExtensionOption()
	    {
	        //We are going to keep in memory the list of option names
	        //as the keys of the hashtable and the enum constant value
	        //as the value. This will speed up the search speed.
	        
	        optNameCache = new Hashtable();
	        
	        Array arr = Enum.GetValues(typeof(MatchExtensions));
	        string[] aliases;
	        
	        foreach(object obj in arr)
	        {
	            aliases = TypeUtil.GetAliases(obj);
	            
	            for(int i=0;i<aliases.Length;i++)
	            {
	                optNameCache.Add(aliases[i], obj);
	            }
	        }
	    }
	    
	    
	    public MatchExtensionOption()
	    {}
	    
	    /// <summary>
	    /// Returns if the parameter name matches any extension name alias
	    /// </summary>
	    public static bool IsMatchExtension(string paramName)
	    {
	        if(TypeUtil.IsAliasName(typeof(MatchExtensions), paramName))
	            return true;
	        
	        return false;
	    }
	  
	}
}
