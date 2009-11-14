
using System;
using System.Collections;
using System.Collections.Specialized;


using SharpKnocking.Common;
using SharpKnocking.IpTablesManager.RuleHandling;

namespace SharpKnocking.IpTablesManager.RuleHandling
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
	public class RuleCommand: GenericParameter
	{
	    private IpTablesChain chain;
	    
	    public IpTablesChain Chain
	    {
	        get { return this.chain;}
	        set
	        {
	            this.chain = value;
	        }
	    }
	    
	    private string oldChainName;
	    
	    /// <summary>
	    /// Gets/sets the old name of the chain to rename
	    /// </summary>
	    public string OldChainName
	    {
	        get { return this.oldChainName;}
	        set { this.oldChainName = value;}
	    }
	    
	    private string newChainName;
	    
	    /// <summary>
	    /// Gets/sets the new name for the chain
	    /// </summary>
	    public string NewChainName
	    {
	        get { return this.newChainName;}
	        set {this.newChainName = value;}
	    }
	    
	    private int ruleNum;
	    
	    /// <summary>
	    /// Gets/sets the number of the rule affected
	    /// </summary>
	    public int RuleNum
	    {
	        get { return this.ruleNum; }
	        set {this.ruleNum = value;}
	    }
	    
	    private RuleTargets target;
	    
	    /// <summary>
	    /// Gets/sets the target affected. Used only with the --policy
	    /// command.
	    /// </summary>
	    public RuleTargets Target
	    {
	        get { return this.target;}
	        set { this.target = value;}
	    }
	    
	    private Commands command;
	    
	    /// <summary>
	    /// Command type.
	    /// </summary>
	    public Commands Command
	    {
	        get { return this.command;}
	        set
	        {
	            this.command = value;
	            this.Name = TypeUtil.GetDefaultAlias(this.command);
	        }
	    }
	    
	    //Cache for the names and the enum type for each name
	    private static Hashtable optNameCache;
	    
	    /// <summary>
	    /// Static initialization of cached values
	    /// </summary>
	    static RuleCommand()
	    {
	        //We are going to keep in memory the list of option names
	        //as the keys of the hashtable and the enum constant value
	        //as the value. This will speed up the search time and will cost
	        //little memory
	        optNameCache = new Hashtable();
	        
	        Array arr = Enum.GetValues(typeof(Commands));
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
		public RuleCommand()
		{
		    this.ruleNum = -1;
		    this.target = RuleTargets.CustomTarget;
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
		    if(optNameCache.ContainsKey(optName))
		    {
		        return true;    
		    }
		    
		    return false;
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
		public static Commands GetCommandType(string cmd)
		{
		    if(optNameCache.ContainsKey(cmd))
		    {
		        return (Commands)optNameCache[cmd];    
		    }
		    
		    return Commands.None;
		}
		
		public override string ToString()
		{
		    string result = TypeUtil.GetDefaultAlias(this.command);
		    
		    if(this.IsLongOption)
		        result = "--"+result;
		    else
		        result = "-"+result;
		    
		    if(!Net20.StringIsNullOrEmpty(this.oldChainName))
		        result+=" "+this.oldChainName;
		    else
		        result+=" "+this.chain.CurrentName;
		        
		    if(this.ruleNum!=-1)
		        result+=" "+this.ruleNum;
		    else if(this.target != RuleTargets.CustomTarget)
		        result+=" "+this.target.ToString().ToUpper();
		    else if(!Net20.StringIsNullOrEmpty(this.newChainName))
		        result+=" "+this.newChainName;
		    
		    return result;
		}
		
	}
}
