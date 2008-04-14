// NetfilterTable.cs
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
using System.Collections.Generic;

using Developer.Common.Types;

namespace IptablesNet.Core
{
	/// <summary>
	/// Netfilter packet table
    /// </summary>
	public class NetfilterTable
	{
	    private PacketTableType type;
	    
	    /// <summary>
        /// Table type 
        /// </summary>
	    public PacketTableType Type
	    {
	        get { return this.type; }
	    }
	    
	    private List<NetfilterChain> chains;
	    
		/// <summary>
		/// Chains defined in the table.
		/// </summary>
		/// <remarks>
		/// This list is populated with the default chains for the table upon
		/// constructor call
		/// </remarks>
	    public NetfilterChain[] Chains
	    {
	        get
	        {
				return this.chains.ToArray();
	        }
	    }
	    
        /// <summary>
        /// Default constructor. Initializes the type to filter table and
		/// default chains. 
        /// </summary>
		public NetfilterTable()
			:this(PacketTableType.Filter)
		{
		}
		
		/// <summary>
		/// Parametrized constructor. Initializes the table type and default
		/// chains.
		/// </summary>
		public NetfilterTable(PacketTableType type)
		{
			this.type = type;
			this.chains = new List<NetfilterChain>(6);
			this.AddBuiltInChainsToList();
		}

		/// <summary>
		/// Adds the built-in chains to the list
		/// </summary>
		protected virtual void AddBuiltInChainsToList()
		{
			switch(type) {
				case PacketTableType.Filter:
					this.AddDefaultChain(BuiltInChains.Input);
					this.AddDefaultChain(BuiltInChains.Forward);
					this.AddDefaultChain(BuiltInChains.Output);
					break;
				case PacketTableType.Mangler:
					this.AddDefaultChain(BuiltInChains.Prerouting);
					this.AddDefaultChain(BuiltInChains.Output);
					this.AddDefaultChain(BuiltInChains.Input);
					this.AddDefaultChain(BuiltInChains.Forward);
					this.AddDefaultChain(BuiltInChains.Postrouting);
					break;
				case PacketTableType.Nat:
					this.AddDefaultChain(BuiltInChains.Prerouting);
					this.AddDefaultChain(BuiltInChains.Output);
					this.AddDefaultChain(BuiltInChains.Postrouting);
					break;
				case PacketTableType.Raw:
					this.AddDefaultChain(BuiltInChains.Prerouting);
					this.AddDefaultChain(BuiltInChains.Output);
					break;
			}			
		}
		
		/// <summary>
		/// Adds a user-defined chain to the table
		/// </summary>
		public void AddChain(string name, RuleTargets defTarget)
		{
			NetfilterChain chain = new NetfilterChain(this, name);
			this.AddChain(chain);
		}
		
	    /// <summary>
	    /// Adds a new user-defined chain to the table.
        /// </summary>
		/// <remarks>
		/// Any try to add a built-in chain will get an ArgumentException.
		/// <br>
		/// There is also problems if the user-defined name matches the name of
		/// any built-in chain or if the parent table of the chain is not the
		/// current table. In any case an ArgumentException is thrown.
		/// in result.
		/// </remarks>
		public void AddChain(NetfilterChain chain)
		{
			PacketTableType tblType = PacketTableType.Filter;
			
		    if(chain.IsBuiltIn) {
				throw new ArgumentException(
				    "Can't add a built-in chain. Built-in chains are already added", "chain.IsBuiltIn");	
			}
		    else if(NetfilterTable.TryGetTableType(chain.Name, out tblType)) {
				throw new ArgumentException(
				    "The chain has a name that matches the name of a built-in chain", "chain.Name");
			}
			else if(chain.ParentTable!=this) {
				throw new ArgumentException(
				    "The chain doesn't belong to this table", "chain.ParentTable");
			}
			else if(this.chains.Contains(chain)) {
				throw new ArgumentException("The chain is already in the table", "chain");
			}
			
		    this.chains.Add(chain);
		}
		
		/// <summary>
		/// Adds a default chain of the type specified with a default policy of
		/// accepting packets
		/// </summary>
		protected void AddDefaultChain(BuiltInChains biChain)
		{
			NetfilterChain chain = new NetfilterChain(this, biChain, RuleTargets.Accept);
			this.chains.Add(chain);
		}
		
		/// <summary>
        /// Removes a chain from the table. 
        /// </summary>
		public void RemoveChain(int pos)
		{
		    if(pos>this.chains.Count || pos<0)
		        throw new IndexOutOfRangeException("Index "+pos+" is out of the chain range");
			else if(this.chains[pos].IsBuiltIn)
				throw new InvalidOperationException("Can't remove a built-in chain");
			this.chains.RemoveAt(pos);
		}
        
		/// <summary>
		/// Gets the position of a chain in the internal array.
		/// </summary>
		/// <returns>
		/// An integer greater than -1 if the chain was found or -1 if not. 
		/// </returns>
        public int IndexOfChain(string name)
        {
		    NetfilterChain next;
		    
		    for(int i=0;i<this.chains.Count;i++) {
		        next = (NetfilterChain)this.chains[i];
		        if(String.Equals(next.CurrentName, name, StringComparison.InvariantCultureIgnoreCase))
		            return i;
		    }
		    
		    return -1;
        }
		
		/// <summary>
		/// Gets the chain named if it is in the internal array
		/// </summary>
		public NetfilterChain FindChain(string name)
		{
		    int pos = this.IndexOfChain(name);
			
			if(pos==-1)
				return null;
			
			return this.chains[pos];
		}
		
		/// <summary>
		/// Removes all the chains in this table
		/// </summary>
		public void Clear()
		{
			foreach(NetfilterChain chain in this.chains)
				chain.Clear();
			this.chains.Clear();
		}
		
		/// <summary>
		/// Sets the counters for a default chain
		/// </summary>
		/// <param name="type">
		/// A <see cref="BuiltInChains"/> with de chain to set the counters
		/// </param>
		/// <param name="incoming">
		/// A <see cref="System.Int32"/> with the incoming packet counter
		/// </param>
		/// <param name="outcoming">
		/// A <see cref="System.Int32"/> with the outcoming packet counter
		/// </param>
		public void SetChainCounters(BuiltInChains type, int incoming, int outcoming)
		{
			if(type == BuiltInChains.UserDefined)
				throw new ArgumentException("type","Must be a built-in chain");
			
			NetfilterChain ch = this.FindChain(type.ToString().ToLower());
			
			if(ch != null)
				ch.SetCounters(incoming, outcoming);
			else
				throw new NetfilterException("Can't find the default chain. Check the table type");
		}
		
		/// <summary>
		/// Adds the contents of the current table (chains and rules) as strings
		/// to a string builder.
		/// </summary>
		public void AppendContentsTo(StringBuilder sb, bool iptablesFormat)
		{
			//Start with the table name in lowercase
		    sb.Append("*"+this.type.ToString().ToLower());
            //Then add the strings for each chain that will contain all the
			//rules in these chains
		    for(int i=0;i<this.chains.Count;i++) {
		        sb.Append("\n"+this.chains[i].GetChainDefinition());
		    }
            
            NetfilterChain chain = null;
            
            for(int i=0;i<this.chains.Count;i++) {
                chain = (NetfilterChain)chains[i];
                if(chain.Rules.Count>0) {
					sb.Append('\n');
                    chain.AppendContentsTo(sb, iptablesFormat);
				}
            }
		}
		
		/// <summary>
		/// Returns a string that represents the table and all the chains and
		/// all the rules in every chain.
        /// </summary>
		public string GetContentsAsString(bool iptablesFormat)
		{
		    StringBuilder sb = new StringBuilder();
			this.AppendContentsTo(sb, iptablesFormat);
		    return sb.ToString();
		}
		
		/// <summary>
		/// Gets if the string is a valid table. If it is the enum that matches
		/// the table is set in the output parameter.
        /// </summary>
		public static bool TryGetTableType(string line, out PacketTableType table)
		{
		    line = line.Trim();
			object obj = null;
		    
		    if(line.StartsWith("*"))
		        line = line.Substring(1).Trim();
			
			if(!TypeUtil.TryGetEnumValue(typeof(PacketTableType), line, out obj)) {
			    table = PacketTableType.Filter;
			    return false;
		    }
			
			table = (PacketTableType)obj;
		    return true;
		}
		
		/// <summary>
        /// Parses a string and builds a instance of a NetfilterTable object
        /// </summary>
		public static NetfilterTable Parse(string line)
		{
			PacketTableType tp = PacketTableType.Filter;
		    if(!TryGetTableType(line, out tp))
		        return null;
		    
		    return new NetfilterTable(tp);
		}
	}
}
