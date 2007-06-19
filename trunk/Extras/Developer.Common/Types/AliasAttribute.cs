
using System;

namespace Developer.Common.Types
{
	
	/// <summary>
	/// Meta information about an alternate name for an enumeration member.
	/// </summary>
	/// <remarks>
	/// This atribute can check if a string match with any name available
	/// </remarks>
	public class AliasAttribute: Attribute
	{
	    private string[] aliases;
	    
	    public string[] Aliases
	    {
	        get {return this.aliases;}    
	    }
	    
		public AliasAttribute(params string[] aliases)
		  :base()
		{
		        this.aliases = aliases;
		}
		
		/// <summary>
		/// Checks if the name match any of the aliases
		/// </summary>
		public bool Match(string name, bool caseSensitive)
		{
		    if(name==null || name.Length==0)
		        return false;
		    
		    if(!caseSensitive)
		        name = name.ToLower();
		    
		    foreach(string alias in this.aliases)
		    {
		        if(caseSensitive && name.Equals(alias))
		        {
		            return true;
		        }
		        else if(!caseSensitive && name.Equals(alias.ToLower()))
                {
                    return true;                                  
                }
		    }
		    
		    return false;
		}
	}
}
