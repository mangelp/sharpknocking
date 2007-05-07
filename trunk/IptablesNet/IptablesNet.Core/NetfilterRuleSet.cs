
using System;
using System.Collections;
using System.IO;
using System.Text;
using SharpKnocking.Common;

using IptablesNet.Core.Commands;
using IptablesNet.Core.Options;

namespace IptablesNet.Core
{
	
    /// <summary>
    /// Models the methods to load iptables configuration data in the format
    /// of the output of the command iptables-save.
    /// It stores the data as a hierarchical tree where the tables are the
    /// roots and the rules are the leafs.
    /// </summary>
	public class NetfilterRuleSet
	{
	    private bool isSafe;
	    
	    /// <summary>
	    /// Gets if there have been loaded some configuration data and if it
	    /// is valid. If this flag is false there is no rules in this storage.
        /// </summary>
	    public bool IsSafe
	    {
	        get {return this.isSafe;}    
	    }
	    
	    private ArrayList tables;
	    
	    /// <summary>
        /// Array of the iptables tables found in the config. 
        /// </summary>
	    public NetfilterTable[] Tables
	    {
	        get {return (NetfilterTable[])this.tables.ToArray();}    
	    }
	    
        /// <summary>
        /// Default constructor
        /// </summary>
		public NetfilterRuleSet()
		{
		    this.tables = new ArrayList();
            this.InitEmptySet();
		}
		
		/// <summary>
		/// Sets the rule set from the file. The previous one is completely
		/// removed and the file must be in the same format as iptables-save
		/// outputs.
        /// </summary>
		public void LoadFromFile(string fileName)
		{
		    Debug.Write("Loading iptables rules from file: "+fileName+"");
		    this.isSafe = false;
		    
		    if(!File.Exists(fileName))
		        throw new ArgumentException("The file name is invalid: "
		                                              +fileName+"","fileName");
		    
		    StreamReader sReader = File.OpenText(fileName);
		    
		    if(sReader==null)
		        return;
		    
		    string text = sReader.ReadToEnd();
		    
		    this.LoadFromString(text);
		}
		

        /// <summary>
        /// Inits the rule set with the default table named filter and with
        /// the default chains INPUT, OUTPUT and FORWARD.
        /// </summary>
        private void InitEmptySet()
        {
            this.tables.Clear();
            
            NetfilterTable table = new NetfilterTable();
            table.Type = PacketTables.Filter;
            
            this.tables.Add(table);
            
            NetfilterChain chain = new NetfilterChain(table);
            chain.Chain = BuiltInChains.Input;
            table.AddChain(chain);
            
            chain = new NetfilterChain(table);
            chain.Chain = BuiltInChains.Forward;
            table.AddChain(chain);
            
            chain = new NetfilterChain(table);
            chain.Chain = BuiltInChains.Output;
            table.AddChain(chain);
        }
        
        /// <summary>
        /// Sets the rule set as safe without checking. 
        /// </summary>
        public void SetAsSafe()
        {
            this.isSafe = true;
        }
		
		/// <summary>
		/// Sets the rule set from the string. The previous one is completely
		/// removed and the string must be in the same format as iptables-save
		/// output.
        /// </summary>
		public void LoadFromString(string current)
		{
		    //Clear existing stuff
		    this.tables.Clear();
		    //Set the flag to mark as unsafe set of rules
		    this.isSafe = false;
		    
		    string[] lines = current.Split('\n','\r');
		    
		    string line;
		    
		    NetfilterTable lastTable=null;
		    NetfilterChain lastChain=null;
		    NetfilterRule rule=null;
		    PacketTables tableType;
		    
		    //Now process each line until COMMIT
		    for(int i=0;i<lines.Length;i++)
		    {
		        Debug.Write("Processing Line: "+lines[i]);
		        line = lines[i].Trim();
		        
		        if(line==null || line.Length==0 || line.StartsWith("#"))
		        {
		            continue;
		        }
		        else if(line.Equals("COMMIT"))
		        {
		            break;
		        }
		        else if(NetfilterTable.IsTable(line,out tableType))
		        {
		            lastTable = new NetfilterTable();
		            
		            lastTable.Type = tableType;
		            
		            if(lastTable == null)
		            {
		                Debug.Write("Invalid table specification: "
		                               +line+". Parsing broken");
		                return;
		            }
		            
		            this.tables.Add(lastTable);
		            
		            Debug.Write("Found table: "+lastTable);
		        }
		        else if(NetfilterChain.IsChain(line))
		        {
		            lastChain = NetfilterChain.Parse(line, lastTable);
		            
		            Debug.Write("Found chain: "+lastChain+". Adding to table "+lastTable.Type);
		            
		            lastTable.AddChain(lastChain);
		        }
		        else if(NetfilterRule.IsRule(line))
		        {
		            
		            if(Net20.StringIsNullOrEmpty(line))
		            {
		                Console.Out.WriteLine("Invalid chain in line: "+line+
		                                      ". Parsing broken");
		                return;
		            }

		            //No chain found. Can't continue.
		            if(lastTable==null)
		            {
		                Debug.Write("There is no chain table. "+
		                                      "Parsing broken.");
		                return;
		            }
		            
		            //Use the rule parser to build a NetfilterRule instance
		            //from the line.
		            rule = RuleParser.GetRule(line, lastTable);
		            
		            if(rule!=null)
		            {
                        Debug.VerboseWrite("Adding to chain '"+
                                           lastChain.CurrentName+"' rule '"+
                                           rule+"'", VerbosityLevels.Insane);
		                lastChain.Rules.Add(rule);
		            }
		            else
		            {
		                Debug.Write("Can't get a rule from "+line+
		                                      ". Parsing broken");
		                return;    
		            }
		            
		            Debug.Write("Found rule: "+rule);
		                
		        }
		        else
		        {
		            Debug.Write("Doen't know how to handle the line: "+line);    
		        }
		    }
		    
		    //Mark the config as good one.
		    this.isSafe = true;
		}
		
		
		/// <summary>
		/// Finds a chain with the name if it exists. It first checks the
		/// builtin name and then the user-defined one.
		/// </summary>
        /// <returns>
        /// It returns the first chain found or null if no chain can be found.
        /// </returns>
		public NetfilterChain FindChain(string name)
		{
		    NetfilterChain result = null;
		    
		    foreach(NetfilterTable table in this.tables)
		    {
		        Debug.VerboseWrite("NetfilterRuleSet.FindChain: Searching "+table.Type,
                            VerbosityLevels.Insane);
		            
		        result = table.FindChain(name);
		        
		        if(result!=null)
		            return result;
		    }
		    
		    return null;
		}
		
        /// <summary>
        /// Returns the current rule set separating each item with the unix
        /// new line character.
        /// </summary>
		public override string ToString()
		{
            if(this.tables.Count==0)
                return String.Empty;
            else if(this.tables.Count==1)
                return this.tables[0].ToString();
            
		    StringBuilder sb = new StringBuilder(tables[0].ToString());
            
		    for(int i=1;i<this.tables.Count;i++)
		    {
		        sb.Append("\n"+tables[i].ToString());
		    }
            
		    return sb.ToString();
		}
        
        /// <summary>
        /// Returns an instance of the default table named filter.
        /// </summary>
        public NetfilterTable GetDefaultTable()
        {
            for(int i=0;i<this.tables.Count;i++)
            {
                if(((NetfilterTable)this.tables[i]).Type == PacketTables.Filter)
                    return (NetfilterTable)this.tables[i];
            }
            
            return null;
        }
        
        /// <summary>
        /// Simulates the execution of the rule.
        /// </summary>
        /// <remarks>
        /// The actions are limited to those that changes chains or rules.
        /// </remarks>
        public void ExecRule(NetfilterRule rule)
        {
            Debug.VerboseWrite("NetfilterRuleSet.ExecRule: "+rule, VerbosityLevels.Insane);
            Debug.VerboseWrite("NetFilterRuleSet.ExecRule: "+rule.Command.CommandType);
            NetfilterTable table = this.GetDefaultTable();
            NetfilterChain chain = null;
            int pos = 0;
            
            switch(rule.Command.CommandType)
            {
                case RuleCommands.NewChain:
                    //Creates the chain.
                    chain = new NetfilterChain(table);
                    chain.Chain = BuiltInChains.None;
                    chain.Name = rule.Command.ChainName;
                    table.AddChain(chain);
                    break;
                case RuleCommands.FlushChain:
                    //Clears all the rules from the chain
                    chain = table.FindChain(rule.Command.ChainName);
                    if(chain==null)
                        throw new InvalidOperationException("The chain "+
                                    rule.Command.ChainName+
                                    " doesn't exist in table "+table.Type);
                    chain.Rules.Clear();
                    break;
                case RuleCommands.DeleteChain:
                    //Removes a chain if it is empty and is not a built-in one
                    pos = table.IndexOfChain(rule.Command.ChainName);
                    if(pos>-1)
                        chain = table.Chains[pos];
                
                    if(chain==null)
                        throw new InvalidOperationException("The chain "+
                                    rule.Command.ChainName+
                                    " doesn't exist in table "+table.Type);
                    else if(chain.IsBuiltIn)
                        throw new InvalidOperationException("Can't delete "+
                                    " built-in chain "+rule.Command.ChainName);
                    else if(chain.Rules.Count>0)
                        throw new InvalidOperationException("Can't delete "+
                                    " chain "+rule.Command.ChainName+
                                    ". Its not empty");
                
                    table.RemoveChain(pos);
                    break;
                case RuleCommands.AppendRule:
                    //Adds a rule to a chain
                    chain = table.FindChain(rule.Command.ChainName);
                
                    if(chain==null)
                        throw new InvalidOperationException("The chain "+
                                    rule.Command.ChainName+
                                    " doesn't exist in table "+table.Type);
                    
                    chain.Rules.Add(rule);
                    break;
                case RuleCommands.DeleteRule:
                    //Removes a rule from a chain
                    chain = table.FindChain(rule.Command.ChainName);
                
                    if(chain==null)
                        throw new InvalidOperationException("The chain "+
                                    rule.Command.ChainName+
                                    " doesn't exist in table "+table.Type);
                
                    DeleteRuleCommand delCmd = (DeleteRuleCommand)rule.Command;
                    chain.Rules.RemoveAt(delCmd.RuleNum-1);
                    break;
                case RuleCommands.InsertRule:
                    Debug.VerboseWrite("NetfilterRuleSet.ExecRule: Inserting rule");
                    //Inserts a rule into a chain
                    chain = table.FindChain(rule.Command.ChainName);
                
                    if(chain==null)
                        throw new InvalidOperationException("The chain "+
                                    rule.Command.ChainName+
                                    " doesn't exist in table "+table.Type);
                
                    InsertRuleCommand insCmd = (InsertRuleCommand)rule.Command;
                    Debug.VerboseWrite("NetfilterRuleSet.ExecRule: Inserting rule in list at "+insCmd.RuleNum);
                    chain.Rules.Insert(insCmd.RuleNum-1, rule);
                    break;
                default:
                    //Every other thing is not implemented and must not be used.
                    throw new NotImplementedException("Not implemented. Sorry!");
            }
               
        }
		
        /// <summary>
        /// Generates a string with all the rule set in the format required by
        /// iptables-restore.
        /// </summary>
        public string SaveToString()
        {
            string str = 
                "#Generated by "+this.GetType().FullName+" on "+
                DateTime.Now.ToShortDateString()+" "+
                DateTime.Now.ToShortTimeString()+"\n"+ this.ToString()+"\n"+
                "COMMIT\n"+"#Completed on "+
                DateTime.Now.ToShortDateString()+" "+
                DateTime.Now.ToShortTimeString()+"";
            
            return str;
        }
        
        /// <summary>
        /// Stores the current rule set in the format required by iptables-restore.
        /// </summary>
		public void SaveToFile(string fileName, bool overwrite)
		{
		    bool exists = File.Exists(fileName);
		    
            if ((exists && overwrite) || !exists) 
            {
                try
                {
                    string currRuleSet = this.SaveToString();
                    Debug.VerboseWrite("Saving set:\n"+currRuleSet, VerbosityLevels.Insane);
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(fileName)) 
                    {
                        sw.Write(currRuleSet);
                    }
                    
                    return;
                }
                catch(Exception ex)
                {
                    Console.Out.WriteLine("NetfilterConfig.SaveToFile(): Can't save text to file"+
                        ". Check you have the required permissions to write file "+fileName+
                        " and that the disk is not full");
                    
                    Console.Out.WriteLine("Fault details:\n"+ex);
                }
            }
            
            return;
		}
		
		/// <summary>
		/// Empties the tables releasing objects to the garbage collector
		/// </summary>
		public void Clear()
		{
		    this.tables.Clear();
		}
	}
}
