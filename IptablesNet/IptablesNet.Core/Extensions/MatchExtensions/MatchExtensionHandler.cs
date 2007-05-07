
using System;

using IptablesNet.Core.Util;
using IptablesNet.Core.Extensions;


namespace IptablesNet.Core.Extensions.ExtendedMatch
{
	/// <summary>
	/// Base class for all the implementations of a match extension.
	/// </summary>
	/// <remarks>
	/// When extending this class the names of each class must
	/// follow this fully qualified scheme:<br/>
    /// IptablesNet.Core.ExtendedMatch.[EnumName]MatchExtension
    /// Where [EnumName] must be replaced by the name of the enum that
    /// represents the match extension type used if the extension is one included
    /// with iptables.<br/>
    /// Custom extensions are not supported by now so you can only implement
    /// those defined in the enumeration
    /// <see cref="T:SharpKnocking.IpTablesManager.RuleHandler.MatchExtensions"/>
    /// <br/><br/>
	/// </remarks>
	public abstract class MatchExtensionHandler: ExtensionHandler<MatchExtensionParameter>
	{
	    /// <summary>
	    /// Inits the instance with the values specified.
	    /// </summary>
	    /// <param name="enumType"> Type of the enumeration used for the options
	    /// that this extension supports</param>
	    /// <param name="name"> Name for the extension. This name has internal
	    /// use only. For client code use the MatchExtensions enumeration.
	    /// </param>
	    /// <remarks>
	    /// This constructor is for the child classes. Every one must define
	    /// the parameters.
	    /// </remarks>
	    protected MatchExtensionHandler(Type enumType, object handlerType)
			:base(handlerType, enumType)
	    {
	    }
	    
	    // ------------------------------------------------------------- //
	    // Static method and properties                                  //
	    // ------------------------------------------------------------- //
	    
	    /// <summary>
	    /// Returns if the parameter name matches any extension name alias
	    /// </summary>
	    public static bool IsMatchExtension(string paramName)
	    {
	        if(optNameCache.Exists(paramName))
	            return true;
	        
	        return false;
	    }

	    
	    //cache for decoding names as MatchExtensions enum constants.
	    private static NameCache optNameCache;
	    
	    static MatchExtensionHandler()
	    {
	        //We are going to keep in memory the list of option names
	        //as the keys of the hashtable and the enum constant value
	        //as the value. This will speed up the search speed.
	        
	        optNameCache = new NameCache();
	        
	        optNameCache.FillFromEnum(typeof(MatchExtensions));
	    }
	  
	}

}
