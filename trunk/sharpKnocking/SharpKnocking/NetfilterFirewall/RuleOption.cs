
using System;
using System.Collections;
using SharpKnocking.Common;
using SharpKnocking.IpTablesManager.RuleHandling.Extensions;

namespace SharpKnocking.IpTablesManager.RuleHandling
{
	
	/// <summary>
	/// Option in the rule.
	/// </summary>
	public class RuleOption: GenericParameter
	{
	    
	    //Disable the set method of Name and Value properties
	    
	    public override string Name
	    {
	    	set { ; }
	    }
	    
	    public override string Value
	    {
	    	set { ; }
	    }
	    
	    private IpTablesRule parentRule;
	    
	    /// <summary>
	    /// Gets/Sets the parent rule owner of this option
	    /// </summary>
	    public IpTablesRule ParentRule
	    {
	        get { return this.parentRule;}
	        set { this.parentRule = value;}
	    }
	    
	    
	    private Options option;
	    
	    /// <summary>
	    /// Gets/Sets the option
	    /// </summary>
	    public virtual Options Option
	    {
	        get {return this.option;}
	        set
	        {
	            this.option = value;
	            this.Name = TypeUtil.GetDefaultAlias(value);
	        }
	    }
	    

	    /// <summary>
	    /// Gets/Sets if the parameter is a match extension parameter.
	    /// </summary>
	    public bool IsMatchExtension
	    {
	        get
	        {
	            return (this.option == Options.MatchExtension);
	        }
	    }

	    //cache for decoding names as options
	    private static Hashtable optNameCache;
	    
	    static RuleOption()
	    {
	        //We are going to keep in memory the list of option names
	        //as the keys of the hashtable and the enum constant value
	        //as the value. This will speed up the search speed.
	        
	        optNameCache = new Hashtable();
	        
	        Array arr = Enum.GetValues(typeof(Options));
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
	    
	    /// <summary>
	    /// Default constructor.
	    /// </summary>
		public RuleOption()
		{

		}
		
		protected void SetName(string name)
		{
		    base.Name = name;    
		}
		
		protected void SetValue(string value)
		{
		    base.Value = value;    
		}
		
		/// <summary>
		/// Gets if the parameter name is a option
		/// </summary>
		public static bool IsOption(string option)
		{
		    if(optNameCache.ContainsKey(option))
		    {
		        return true;    
		    }
		    
		    return false;
		}
		
	    /// <summary>
		/// Returns the Options enumeration constant that matches the
		/// option name. The name can be any valid alias for the option but
		/// it can't be the name of a constant of the enumeration.
		/// </summary>
		/// <returns>
		/// Options.None if the name is not a valid alias or it is the name of
		/// a constant in the enumeration.
		/// </returns>
		public static Options GetOptionType(string optName)
		{
		    if(optNameCache.ContainsKey(optName))
		    {
		        return (Options)optNameCache[optName];    
		    }
		    
		    return Options.None;
		}
		
		
		/// <summary>
		/// Returns an string with all the information in the option in the
		/// format used in iptables. This includes the match extension options
		/// </summary>
        public override string ToString ()
        {
            string result = TypeUtil.GetDefaultAlias(this.option);
            
            if(this.IsLongOption)
                result = "--"+result;
            else
                result = "-"+result;
            
            if(!Net20.StringIsNullOrEmpty(this.Value))
                result+=" "+this.Value;
            
            return result;
        }


	}
}
