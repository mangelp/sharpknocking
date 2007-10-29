// NetfilterChain.cs
//
//  Copyright (C)  2007 iSharpKnocking project
//  Created by Miguel Angel Perez, mangelp@gmail.com
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA 

using System;
using System.Text;
using System.Collections;

using Developer.Common.Types;

namespace IptablesNet.Core
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
	        get { return (this.chainType != BuiltInChains.UserDefined); }
	    }
	    
	    private BuiltInChains chainType;
	    
	    /// <summary>
	    /// Get/set the chain type.
	    /// </summary>
	    public BuiltInChains ChainType
	    {
	        get {return this.chainType;}
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
	                return this.chainType.ToString().ToUpper();
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
	    }
	    
	    /// <summary>
	    /// Constructor. Use this for user-defined chains.
	    /// </summary>
		public NetfilterChain(NetfilterTable parentTable, string chainName)
			:this(parentTable, BuiltInChains.UserDefined, RuleTargets.Accept)
		{
			if(TypeUtil.IsEnumValue(typeof(BuiltInChains), chainName))
				throw new ArgumentException("Invalid name "+chainName+" for chain. It matches an built-in chain name");
			
		    this.name = chainName;
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="parentTable">
		/// Table where this chain belongs to
		/// </param>
		/// <param name="builtIn">
		/// Built-in chain type.
		/// </param>
		/// <param name="defTarget">
		/// Default target for the chain
		/// </param>
		public NetfilterChain(NetfilterTable parentTable, Core.BuiltInChains builtIn, RuleTargets defTarget)
		{
			this.parentTable = parentTable;
			this.defaultTarget = defTarget;
			this.chainType = builtIn;
			this.rules = new NetfilterRuleList(this);
		}
		
		/*** methods and functions ***/

		/// <summary>
		/// Gets a string that represents the chain and his counters
		/// </summary>
		/// <remarks>
		/// This string is in the format required by iptables-restore and the
		/// counters are ever set to 0.
		/// </remarks>
		public override string ToString()
		{
			return GetChainDefinition();
		}
		
		/// <summary>
		/// Returns a string that defines the chain in the format required by
		/// iptables-restore.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/>
		/// </returns>
		public string GetChainDefinition()
		{
		    string chainSpec = ":";
		    
		    //If is builtIn it must have a default target, but it is user
		    //defined there is no target and is specified as -
		    if(this.IsBuiltIn)
		    {
		        chainSpec += this.chainType.ToString().ToUpper()+ " "
		        + this.defaultTarget.ToString().ToUpper();
		    }
		    else
		    {
		        chainSpec += this.name+ " -";
		    }
		    
			//TODO: Preserve counters
		    chainSpec += " [0:0]";
		    
		    return chainSpec;		
		}
        
		/// <summary>
		/// Gets a string that can be parsed back to a rule list
		/// </summary>
        public string GetContentsAsString()
        {
            if(this.rules.Count==0)
		        return String.Empty;
		    
		    StringBuilder sb = new StringBuilder(this.rules[0].ToString());
		    this.AppendContentsTo(sb);
		    return sb.ToString();
        }
		
		/// <summary>
		/// Removes all the rules in the chain.
		/// </summary>
		public void Clear()
		{
			foreach(NetfilterRule rule in this.rules)
				rule.ParentChain = null;
			rules.Clear();
		}
		
		/// <summary>
		/// Appends every rule to the string builder using the overload of the
		/// ToString methods of the rules.
		/// </summary>
		public void AppendContentsTo(StringBuilder sb)
		{
            if(this.rules.Count==0)
		        return;
			
			sb.Append(this.rules[0].ToString());
		    
		    for(int i=1;i<this.rules.Count;i++)
		    {
		        sb.Append("\n"+rules[i].ToString());
		    }
		}
		
		/*** static stuff ***/
		
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
			object obj = null;
			
			if(!TypeUtil.TryGetEnumValue(typeof(BuiltInChains),
			                                   name.Trim(), out obj))
			{
				chain = BuiltInChains.Input;
				return false;
			}
			
		    chain = (BuiltInChains)obj;
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
		    
		    string[] parts = StringUtil.Split(line, true,':',' ');
		    
//		    if(Debug.DebugEnabled)
//		    {
//		        for(int i=0;i<parts.Length;i++)
//		        {
//		            Debug.Write("Split["+i+"]="+parts[i]);    
//		        }
//		    }
			
			BuiltInChains blt;
			string name;
			RuleTargets defaultTarget;
		    
		    //First check if the table is builtin to get default target or not
		    if(IsBuiltinChain(parts[0], out blt))
		    {
		        defaultTarget = (RuleTargets)TypeUtil.GetEnumValue(typeof(RuleTargets),
				                                                   parts[1]);
				return new NetfilterChain(table, blt, defaultTarget); 
		    } else {
		        //Is not predefined. Grab the name
		        name = parts[0];
				return new NetfilterChain(table, name); 
		    }
		}
	}
}
