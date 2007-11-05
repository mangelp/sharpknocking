// NetfilterRuleList.cs
//
//  Copyright (C) 2007 iSharpKnocking project
//  Created by Miguel Angel Perez (mangelp{@}gmail{d0t}com)
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
//
//

using System;
using System.Collections;
using IptablesNet.Core;

namespace IptablesNet.Core
{
    
    /// <summary>
    /// List of rules for netfilter
    /// </summary>
	public class NetfilterRuleList: CollectionBase 
	{
	    NetfilterChain parentChain;
	    
	    /// <summary>
	    /// Parent chain that holds this list of rules.
	    /// <summary>
	    public NetfilterChain ParentChain
	    {
	       get {return this.parentChain;}    
	    }
		
        /// <summary>
        /// Constructor. Requires the parent chain reference.
        /// </summary>
		public NetfilterRuleList(NetfilterChain parentChain)
		  :base()
		{
            this.parentChain = parentChain;
		}
		
        /// <summary>
        /// Gets/Replaces a rule in the list.
        /// </summary>
	    public NetfilterRule this[int index]
		{
		    get
		    {
		        return (NetfilterRule)this.InnerList[index];
		    }
		    set
		    {   
		        NetfilterRule rule = (NetfilterRule)this.InnerList[index];
                
		        this.List[index] = value;
		        rule.ParentChain = null;
		    }
		}
		
        /// <summary>
        /// Adds a rule to the list.
        /// </summary>
        /// <remarks>
        /// The parent chain for the rule is updated with the parent chain
        /// for this list.
        /// </remarks>
		public void Add(NetfilterRule rule)
		{
		    if(rule.ParentChain!=null)
		    {
		        rule.ParentChain.Rules.Remove(rule);
		    }
            
		    this.List.Add(rule);
		    
		    rule.ParentChain = this.parentChain;
		}
		
        /// <summary>
        /// Gets if the list contains a rule
        /// </summary>
		public bool Contains(NetfilterRule rule)
		{
		    return this.List.Contains(rule);
		}
		
		public void Remove(NetfilterRule rule)
		{
		    this.List.Remove(rule);
		    rule.ParentChain = null;
		}
		
		public int IndexOf(NetfilterRule rule)
		{
		    return this.List.IndexOf(rule);    
		}
		
        /// <summary>
        /// Inserts a rule in the list. 
        /// </summary>
		public void Insert(int index, NetfilterRule rule)
		{   
		    if(rule.ParentChain!=null)
		        rule.ParentChain.Rules.Remove(rule);
		    
		    this.List.Insert(index, rule);
		    
		    rule.ParentChain = this.parentChain;
		}

	}
}
