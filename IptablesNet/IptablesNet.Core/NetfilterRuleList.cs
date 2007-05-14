
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
