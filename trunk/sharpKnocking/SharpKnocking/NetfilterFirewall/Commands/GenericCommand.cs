
using System;
using System.Collections;

using SharpKnocking.Common;
using SharpKnocking.NetfilterFirewall;

namespace SharpKnocking.NetfilterFirewall.Commands
{
	
	/// <summary>
	/// Models all the parameters for the command part of the rule.
	/// After the command can follow a rule specification but the rule
	/// specification is not part of this object.
	/// </summary>
	/// <remarks>
	/// The client must initialize all the parameters required for each
	/// concrete command. No further tests are performed over the correctness
	/// of the values.
	/// </remarks>
	public  abstract class GenericCommand
	{
	    public virtual bool MustSpecifyRule
	    {
	       get { return true;}    
	    }
	    
	    private bool isLongOption;
	    
	    /// <summary>
	    /// Returns true if the option is in long format or false if not
	    /// </summary>
	    public bool IsLongOption
	    {
	        get { return this.isLongOption;}
	        set { this.isLongOption = value;}
	    }
	    
	    /// <summary>
	    /// Returns the values (parameters for the option) as a single string
	    /// </summary>
	    public virtual string Value
	    {
	        get
	        {
	            if(this.chainName==null)
	                return String.Empty;
	            else
	                return this.chainName;
	        }
	    }
	    
	    private string chainName;
	    
	    /// <summary>
	    /// Name for the chain
	    /// </summary>
	    /// <remarks>
	    /// If the concrete command doesn't have a chain name as parameter
	    /// this property must be null or empty.
	    /// </remarks>
	    public string ChainName
	    {
	        get
	        {
	            return this.chainName;
	        }
	        set
	        {
	            this.chainName = value;
	        }
	    }
	    
	    private NetfilterRule parentRule;
	    
	    /// <summary>
	    /// Gets/Sets the parent rule owner of this option
	    /// </summary>
	    public NetfilterRule ParentRule
	    {
	        get { return this.parentRule;}
	        set { this.parentRule = value;}
	    }
	    
	    private RuleCommands command;
	    
	    /// <summary>
	    /// Command implemented
	    /// </summary>
	    public RuleCommands Command
	    {
	        get { return this.command;}
	    }
	    
	    
	    //Cache for the names and the enum type for each name
	    private static Hashtable optNameCache;
	    
	    /// <summary>
	    /// Static initialization of cached values
	    /// </summary>
	    static GenericCommand()
	    {
	        //We are going to keep in memory the list of option names
	        //as the keys of the hashtable and the enum constant value
	        //as the value. This will speed up the search time and will cost
	        //little memory
	        optNameCache = new Hashtable();
	        
	        Array arr = Enum.GetValues(typeof(RuleCommands));
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
	    /// Default instance constructor
	    /// </summary>
		public GenericCommand(RuleCommands command)
		{
		    this.command = command;
		}
		
		/// <summary>
		/// Gets if the option name (parameter name without '-') matches one
		/// of the names (including aliases) of the commands.
		/// </summary>
		/// <remarks>
		/// We aren't going to implement the way iptables processes parameters
		/// in the command line (it can determine the name of the parameter if
	    /// there are enough initial letters).
	    /// The name must be specified as the short or long formats only (this
        /// includes aliases).
		/// </remarks>
		public static bool IsCommand(string optName)
		{
            bool result = false;
            
		    if(optNameCache.ContainsKey(optName))
		    {
		        result = true;    
		    }
		    
            Debug.VerboseWrite("IsCommand("+optName+")"+optNameCache.Count+"? "+result, VerbosityLevels.High);
		    return result;
		}
		
		/// <summary>
		/// Returns the Commands enumeration constant that matches the
		/// command name. The name can be any valid alias for the command but
		/// it can't be the name of a constant of the enumeration.
		/// </summary>
		/// <returns>
		/// Commands.None if the name is not a valid alias or the name is one
		/// of the enumeration constants.
		/// </returns>
		public static RuleCommands GetCommandType(string cmd)
		{
		    if(optNameCache.ContainsKey(cmd))
		    {
		        return (RuleCommands)optNameCache[cmd];    
		    }
		    
		    return RuleCommands.None;
		}
		
		
		public override string ToString()
		{
		    string result = TypeUtil.GetDefaultAlias(this.command);
            
            if(this.IsLongOption)
                result = "--"+result;
            else
                result = "-"+result;
            
            if(!Net20.StringIsNullOrEmpty(this.Value))
                result+=" "+this.Value;
            
            return result;
		}
		
		/// <summary>
		/// Validates the current set of values (command parameters) and throws
		/// an exception if someone is incorrect
		/// </summary>
		public virtual void ValidateValues()
		{}
		
	}
}
