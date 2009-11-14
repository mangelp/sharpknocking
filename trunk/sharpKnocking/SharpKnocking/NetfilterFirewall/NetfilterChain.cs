
using System;
using System.Text;
using System.Collections;

using SharpKnocking.Common;

namespace SharpKnocking.NetfilterFirewall
{
    
	/// <summary>
	/// IpTables Chain (builtin or user-defined) that contains all the rules
	/// that apply to the packets traversing this chain.
    /// </summary>
	public class NetfilterChain
	{
	    private RuleTargets defaultTarget;
	    
	    /// <summary>
	    /// Get/set the default target for the chain.
	    /// </summary>
	    /// <remarks>
	    /// If it is predefined the value is other than None. If not is set
	    /// to None when the rule is loaded and when it is saved this field is
	    /// ignored.
	    /// </remarks>
	    public RuleTargets DefaultTarget
	    {
	        get {return this.defaultTarget;}
	        set {this.defaultTarget = value;}
	    }
	    
	    /// <summary>
	    /// Get if the chain is builtin or not (user defined).
	    /// the field Name
	    /// </summary>
	    /// <remarks>
	    /// If the chain is builtin the enum field Chain has the chain type,
	    /// but if it is false the chain is user defined and the name is in
	    /// the field Name 
	    /// </remarks>
	    public bool IsBuiltIn
	    {
	        get { return (this.chain != BuiltInChains.None); }
	    }
	    
	    private BuiltInChains chain = BuiltInChains.None;
	    
	    /// <summary>
	    /// Get/set the chain type.
	    /// </summary>
	    public BuiltInChains Chain
	    {
	        get {return this.chain;}
	        set {this.chain = value;}
	    }

	    private string name;
	    
	    /// <summary>
	    /// Get/set the user defined chain name.
	    /// </summary>
	    public string Name
	    {
	        get {return this.name;}
	        set {this.name = value;}
	    }
	    
	    /// <summary>
	    /// Gets the current name for the chain
	    /// </summary>
	    public string CurrentName
	    {
	        get
	        {
	            if(this.IsBuiltIn)
	                return this.chain.ToString().ToUpper();
	            else
	                return this.name;
	        }
	    }
	    
	    private NetfilterRuleList rules;
	    
	    /// <summary>
	    /// Get/set an array of rules with the current rule set in the chain.
	    /// </summary>
	    public NetfilterRuleList Rules
	    {
	        get
	        {
	            return this.rules;    
	        }
	    }
	    
	    private NetfilterTable parentTable;
	    
	    /// <summary>
	    /// Get the table where this chain is.
	    /// </summary>
	    public NetfilterTable ParentTable
	    {
	        get { return this.parentTable; }
	        set { this.parentTable = value;}
	    }
	    
	    /// <summary>
	    /// Constructor. Initializes the parent table. The chain is set to
	    /// input.
	    /// </summary>
		public NetfilterChain(NetfilterTable parentTable)
		{
		    this.parentTable = parentTable;
		    this.chain = BuiltInChains.Input;
		    //By default accept everything
		    this.defaultTarget = RuleTargets.Accept;
		    this.rules = new NetfilterRuleList(this);
		}
		
		/// <summary>
		/// Gets if the string could be a chain as specified in standard
		/// iptables configuration file (see iptables-save output format).
	    /// </summary>
		public static bool IsChain(string line)
		{
		    line = line.Trim();
		    
		    if(line.StartsWith(":"))
		        return true;
		    
		    return false;
		}
		
		/// <summary>
		/// Gets if the name matches a builtin chain and sets that in the output
		/// parameter.
	    /// </summary>
		public static bool IsBuiltinChain(string name, out BuiltInChains chain)
		{
		    try
		    {
		        chain = (BuiltInChains)TypeUtil.GetEnumValue(typeof(BuiltInChains),
		                                                       name.Trim());
		    }
		    catch(Exception)
		    {
		        chain = BuiltInChains.None;    
		    }
		    
		    if(chain == BuiltInChains.None)
		        return false;
		    else
		        return true;
		}
		
		/// <summary>
		/// Parses a string and builds a object that represent the chain found
		/// in the line.
		/// </summary>
		public static NetfilterChain Parse(string line, NetfilterTable table)
		{
		    if(!IsChain(line))
		        return null;
		    
		    NetfilterChain result = new NetfilterChain(table);
		    
		    string[] parts = Net20.StringSplit(line, true,':',' ');
		    
//		    if(Debug.DebugEnabled)
//		    {
//		        for(int i=0;i<parts.Length;i++)
//		        {
//		            Debug.Write("Split["+i+"]="+parts[i]);    
//		        }
//		    }
		    
		    //First check if the table is builtin to get default target or not
		    if(IsBuiltinChain(parts[0], out result.chain))
		    {
		        Debug.Write("Found builtin chain");
		        result.defaultTarget =
		            (RuleTargets)TypeUtil.GetEnumValue(typeof(RuleTargets),
		                                                          parts[1]);
		    }
		    else
		    {
		        Debug.Write("Found user defined chain");
		        //Is not predefined. Grab the name
		        result.name = parts[0];
		    }
		    
		    //TODO: How valuable are the counters? IE [2213:34243235] ->Mangelp
		    //we omit them for now.		    
		    return result;
		}
		
		/// <summary>
		/// Gets a string that represents the chain and all the rules assigned
		/// to it.
		/// </summary>
		/// <remarks>
		/// This string is in the format required by iptables-restore and the
		/// counters are ever set to 0.
		/// </remarks>
		public override string ToString()
		{
		    string chainSpec = ":";
		    
		    //If is builtIn it must have a default target, but it is user
		    //defined there is no target and is specified as -
		    if(this.IsBuiltIn)
		    {
		        chainSpec += this.chain.ToString().ToUpper()+ " "
		        + this.defaultTarget.ToString().ToUpper();
		    }
		    else
		    {
		        chainSpec += this.name+ " -";
		    }
		    
		    chainSpec += " [0:0]";
		    
		    return chainSpec;
		}
        
        public string GetRulesAsString()
        {
            if(this.rules.Count==0)
		        return String.Empty;
		    
		    StringBuilder sb = new StringBuilder(this.rules[0].ToString());
		    
		    for(int i=1;i<this.rules.Count;i++)
		    {
		        sb.Append("\n"+rules[i].ToString());
		    }
		    
		    return sb.ToString();
        }
	}
}
