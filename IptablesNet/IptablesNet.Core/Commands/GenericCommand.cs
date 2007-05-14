
using System;
using System.Text;
using System.Collections;

using SharpKnocking.Common;

using IptablesNet.Core;

namespace IptablesNet.Core.Commands
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
	public  abstract class GenericCommand: AbstractParameter
	{
		// -- Instance fields and properties -- //
		
		/// <summary>
		/// Gets if the rule has to be specified.
		/// </summary>
	    public virtual bool MustSpecifyRule
	    {
	       get { return true;}    
	    }
		
		private NetfilterRule rule;
		
		/// <summary>
		/// Gets/Sets the rule related to this command.
		/// </summary
		/// <remarks>
		/// The most of the times the rule is a parameter with the command. If
		/// this property returns null is because the command doesn't require
		/// a rule or because it haven't been set.
		/// </remarks>
		public NetfilterRule Rule
		{
			get { return this.rule;}
			set { this.rule = value;}
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
	    
	    private RuleCommands commandType;
	    
	    /// <summary>
	    /// Command implemented
	    /// </summary>
	    public RuleCommands CommandType
	    {
	        get { return this.commandType;}
	    }
		
		public override bool IsLongFormat
		{
			get 
			{
				string def = TypeUtil.GetDefaultAlias (this.commandType);
				if(def!=null && def.Length > 1)
					return true;
				
				return false;
			}
		}

		
		// -- Static Fields, properties and methods -- //
		
	    
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
		/// Gets if the line can be a rule candidate. 
		/// </summary>
		/// <remarks>
		/// This only checks if the line starts with a -. So this check is poor
		/// and only useful when reading lines in iptables config format (the
        /// same format as iptables-save output)
        /// </summary>
		public static bool CanBeACommand(string line)
		{
		    line = line.Trim();
		    
		    if(line.StartsWith("-"))
		    {
		        return true;
		    }
		    
		    return false;
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
		    
            //Debug.VerboseWrite("IsCommand("+optName+")"+optNameCache.Count+"? "+result, VerbosityLevels.High);
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
		
		// -- Public Constructors and methods -- //
	    
	    /// <summary>
	    /// Default instance constructor
	    /// </summary>
		public GenericCommand(RuleCommands commandType)
		{
		    this.commandType = commandType;
		}
		
		public override string GetDefaultAlias()
		{
			return TypeUtil.GetDefaultAlias(this.commandType);
		}
		
		public override bool IsAlias(string name)
		{
			return TypeUtil.IsAliasName(this.commandType, name);
		}
		
		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
		    string result = TypeUtil.GetDefaultAlias(this.commandType);
            
            if(this.IsLongFormat)
                builder.Append("--"+result);
            else
                builder.Append("-"+result);
			
			if(!String.IsNullOrEmpty(this.chainName))
				builder.Append(" "+this.chainName);
			
			string val=this.GetValuesAsString();
			
			if(!String.IsNullOrEmpty(val))
				builder.Append(" "+val);
			
			if(this.rule!=null && this.MustSpecifyRule)
				builder.Append(" "+this.rule.ToString());
            
            return builder.ToString();
		}
		
	}
}
