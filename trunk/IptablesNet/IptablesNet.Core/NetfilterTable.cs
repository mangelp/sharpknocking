
using System;
using System.Text;
using System.Collections;

using Developer.Common.Types;

namespace IptablesNet.Core
{
	/// <summary>
	/// IpTables table.
    /// </summary>
	public class NetfilterTable
	{
	    
	    private PacketTables type;
	    
	    /// <summary>
        /// Table type 
        /// </summary>
	    public PacketTables Type
	    {
	        get { return this.type; }
	        set { this.type = value; }
	    }
	    
	    private ArrayList chains;
	    
	    public NetfilterChain[] Chains
	    {
	        get
	        {
	            NetfilterChain[] result = new NetfilterChain[this.chains.Count];
	            
	            for(int i=0;i<result.Length;i++)
	            {
	                result[i] = (NetfilterChain)this.chains[i];    
	            }
	            
	            return result;
	            
	        }
	    }
	    
        /// <summary>
        /// Default constructor. Initializes the type to filter. 
        /// </summary>
		public NetfilterTable()
		{
		    //Default table
		    this.type = PacketTables.Filter;
		    this.chains = new ArrayList();
		}
		
	    /// <summary>
        /// Sets the table type from a string. 
        /// </summary>
		public bool SetTable(string tableName)
		{
		    try
		    {
		        this.type = (PacketTables) Enum.Parse(typeof(PacketTables), tableName, false);
		    }
		    catch(Exception ex)
		    {
		        Console.Out.WriteLine("Bad table name: "+ex.Message+"\nDetails:\n"+ex);
		        return false;
		    }
		    
		    return true;
		}
		
	    /// <summary>
	    /// Adds a new chain to the table. The chain must be a user-defined
	    /// chain or one of the builtin chains for the table.
        /// </summary>
		public void AddChain(NetfilterChain chain)
		{
		    if(this.chains.Contains(chain))
		        throw new DuplicateElementException("The chain "
		                    +chain+" have been already added");
		    
		    this.chains.Add(chain);
		}
		
		/// <summary>
        /// Removes a chain from the table. 
        /// </summary>
		public void RemoveChain(int pos)
		{
		    if(this.chains.Count>pos && pos>=0)
		        this.chains.RemoveAt(pos);
		    else
		        throw new IndexOutOfRangeException();
		}
        
        public int IndexOfChain(string name)
        {
		    NetfilterChain next;
		    
		    for(int i=0;i<this.chains.Count;i++)
		    {
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
		    NetfilterChain next;
		    
		    for(int i=0;i<this.chains.Count;i++)
		    {
		        next = (NetfilterChain)this.chains[i];
		        
		        if(String.Equals(next.CurrentName, name, StringComparison.InvariantCultureIgnoreCase))
		            return next;
		    }
		    
		    return null;
		}
		
		/// <summary>
		/// Returns a string that represents the table and all the chains and
		/// all the rules in every chain.
        /// </summary>
		public override string ToString()
		{
		    StringBuilder sb =
		        new StringBuilder("*"+this.type.ToString().ToLower());
            
		    for(int i=0;i<this.chains.Count;i++)
		    {
		        sb.Append("\n"+chains[i].ToString());
		    }
            
            NetfilterChain chain = null;
            
            for(int i=0;i<this.chains.Count;i++)
            {
                chain = (NetfilterChain)chains[i];
                if(chain.Rules.Count>0)
                    sb.Append("\n"+chain.GetRulesAsString());
            }
		    
		    return sb.ToString();
		}
		
		/// <summary>
		/// Gets if the string is a valid table. If it is the enum that matches
		/// the table is set in the output parameter.
        /// </summary>
		public static bool IsTable(string line, out PacketTables table)
		{
		    line = line.Trim();
		    
		    if(line.StartsWith("*"))
		    {
		        line = line.Substring(1).Trim();
		        object obj = TypeUtil.GetEnumValue(typeof(PacketTables), line);
		        
		        if(obj!=null)
		        {
		            table = (PacketTables)obj;
		            return true;
		        }
		    }
		    
		    //Set this by default.
		    table = PacketTables.Filter;
		    return false;
		}
		
		/// <summary>
        /// Parses a string and builds a instance of a NetfilterTable object
        /// </summary>
		public static NetfilterTable Parse(string line)
		{
		    NetfilterTable tableObj = new NetfilterTable();
		    
		    if(!IsTable(line, out tableObj.type))
		        return null;
		    
		    return tableObj;
		}
		
	}
}
