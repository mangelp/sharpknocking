
using System;
using System.Net;

using SharpKnocking.Common;
using SharpKnocking.Common.Net;
using SharpKnocking.NetfilterFirewall;
using SharpKnocking.NetfilterFirewall.Commands;
using SharpKnocking.NetfilterFirewall.ExtendedTarget;
using SharpKnocking.NetfilterFirewall.ExtendedMatch;
using SharpKnocking.NetfilterFirewall.Options;
    

namespace SharpKnocking.NetfilterFirewall
{
    /// <summary>
    /// Models an entry-point to access the current iptables rule set.
    /// Implements the needed operations for our purposses for the 
    /// sharp knocking project.
    /// </summary>
	public class FirewallManager
	{
        private static FirewallManager instance = new FirewallManager();
        
        /// <summary>
        /// Gets the active instance of the firewall manager.
        /// </summary>
        public static FirewallManager GetInstance 
        {
            get { return instance;}
        }
        
        private string tempFileName;
        
        /// <summary>
        /// Temporary file name to store rules when using the current loaded
        /// set of rules. This file may be overwritten continuously.
        /// </summary>
        public string TempFileName
        {
            get { return this.tempFileName;}
            set { this.tempFileName = value;}
        }
        
        private string chainName;
        
        /// <summary>
        /// Name for the chain where all the rules are stored.
        /// </summary>
        public string ChainName
        {
            get { return this.chainName;}
            set { this.chainName = value;}
        }
        
        private NetfilterRuleSet ruleSet;
        
        /// <summary>
        /// Current rule set loaded.
        /// </summary>
        public NetfilterRuleSet RuleSet
        {
            get { return this.ruleSet;}
        }
        
        private bool dryRun;
        
        /// <summary>
        /// If set to true all the actions over the iptables command results as
        /// successful but they are not really done. If false the actions are
        /// done.
        /// </summary>
        /// <remarks>
        /// This will cause that the rule set will be changed but the rule set
        /// in netfilter will remain as is. Take this in account when you set
        /// this flag to true.
        /// </remarks>
        public bool DryRun
        {
            get { return this.dryRun;}
            set { this.dryRun = value;}
        }
	    
        /// <summary>
        /// Default constructor
        /// </summary>
		private FirewallManager()
		{
            this.ruleSet = new NetfilterRuleSet();
            this.tempFileName = UnixNative.CreateTempFileName();
            Debug.VerboseWrite("FirewallManager: Will use "+this.tempFileName+
                    " for scratch");
            this.chainName = "SharpKnocking-INPUT";
		}
        
        /// <summary>
        /// Loads the current set of rules from iptables-save command
        /// </summary>
        public void LoadCurrentRuleSet()
        {
            Debug.VerboseWrite("Storing current netfilter set in: "+tempFileName+".rr.ruleset");
            IpTablesCmd.Save(tempFileName+".ruleset");
            
            Debug.VerboseWrite("Loading current set from: "+tempFileName+".rr.ruleset");
            this.ruleSet.LoadFromFile(tempFileName+".rr.ruleset");
        }
        
        /// <summary>
        /// Loads the current set of rules from a file
        /// </summary>
        public void LoadRuleSetFrom(string fileName)
        {
            Debug.VerboseWrite("Loading rule set from: "+fileName+"");
            this.ruleSet.LoadFromFile(fileName);
        }

        /// <summary>
        /// Applies the current set of rules using iptables-restore.
        /// </summary>
        public void ApplyCurrentRuleSet()
        {
            Debug.VerboseWrite("Applying current rule set");
            this.ruleSet.SaveToFile(tempFileName+".ruleset",true);
            
            if(this.dryRun )
                return;
                
            IpTablesCmd.Restore(tempFileName+".ruleset");
        }
        
        /// <summary>
        /// Applies the rule to the current set of rules.
        /// </summary>
        public void ApplyRule(NetfilterRule rule)
        {   
            Debug.VerboseWrite("FirewallManager::ApplyRule: Executing rule in real firewall: "+rule);
            
            if(this.dryRun)
                return;
                
            IpTablesCmd.Exec(new string[] {rule+""});
        }
        
        /// <summary>
        /// Removes the sharpknocking chain with all the rules from the firewall.
        /// This lets the rule set as it previously was.
        /// </summary>
        public void RemoveSharpKnockingChain()
        {
            NetfilterRule rule = new NetfilterRule();
            DeleteRuleCommand delCmd = new DeleteRuleCommand();
            delCmd.ChainName = "INPUT";
            delCmd.RuleNum = 1;
            
            rule.Command = delCmd;
            FirewallManager.instance.ApplyRule(rule);
            this.ruleSet.ExecRule(rule);
            
            rule = new NetfilterRule();
            FlushChainCommand fCmd = new FlushChainCommand();
            fCmd.ChainName = this.chainName;
            
            rule.Command = fCmd;
            FirewallManager.instance.ApplyRule(rule);
            this.ruleSet.ExecRule(rule);
            
            rule = new NetfilterRule();
            DeleteChainCommand dCmd = new DeleteChainCommand();
            dCmd.ChainName = this.chainName;
            
            rule.Command = dCmd;
            FirewallManager.instance.ApplyRule(rule);
            this.ruleSet.ExecRule(rule);
        }
        
        /// <summary>
        /// Adds all the necesary rules to intercept every incoming packet
        /// and redirect to our user-defined chain were we are going to put
        /// our rules.
        /// </summary>
        /// <remarks>
        /// This adds a rule to let all loopback packets coming, a rule to let
        /// incoming established or related conections and a rule to drop everything
        /// else.
        /// </remarks>
        public void AddSharpKnockingChain()
        {            
            Debug.VerboseWrite("Adding SharpKnocking chain");
            NetfilterRule rule = null;
            JumpOption jopt = null;
            
            if(this.ruleSet.FindChain(this.chainName)!=null)
            {
                Debug.Write("The required chain is in place. Flushing and reusing it");
                
                rule = new NetfilterRule();
                FlushChainCommand cmd = new FlushChainCommand();
                cmd.ChainName = this.chainName;
                //Set in the rule
                rule.Command = cmd;
            }
            else
            {
                Debug.VerboseWrite("Intercepting INPUT chain packets with chain "+
                                   this.chainName);
                
                //Create rule for adding a new chain
                rule = new NetfilterRule();
                
                //Create new chain command
                NewChainCommand cmd = new NewChainCommand();
                cmd.ChainName = this.chainName;
                //Set in the rule
                rule.Command = cmd;
                
                //Execute
                FirewallManager.instance.ApplyRule(rule);
                //Execute in default table named filter
                this.ruleSet.ExecRule(rule);
                   
                //Create insert rule to redirect INPUT packets to our chain
                rule = new NetfilterRule();
                    
                //Create insert command
                InsertRuleCommand iCmd = new InsertRuleCommand();
                iCmd = new InsertRuleCommand();
                iCmd.RuleNum = 1;
                iCmd.ChainName = "INPUT";
                
                rule.Command = iCmd;
                
                //Create jump option to redirect to our chain
                jopt = new JumpOption();
                jopt.Target = RuleTargets.CustomTarget;
                jopt.CustomTarget = CustomRuleTargets.UserDefinedChain;
                jopt.CustomTargetName = this.chainName;
                //Add to rule
                rule.Options.Add(jopt);
                
            }

           //Execute
           FirewallManager.instance.ApplyRule(rule);
           //Execute in default table named filter
           this.ruleSet.ExecRule(rule);
            
            //Create rule
            rule = new NetfilterRule();
            
            //Create append rule command
            AppendRuleCommand acmd = new AppendRuleCommand();
            acmd.ChainName = this.chainName;
            //Set in the rule
            rule.Command = acmd;
            
            //Create option to accept loopback traffic
            InInterfaceOption inOpt = new InInterfaceOption();
            inOpt.Interface = "lo";
            //Add to rule
            rule.Options.Add(inOpt);
            
            //Create jump option with accept target
            jopt = new JumpOption();
            jopt.Target = RuleTargets.Accept;
            //Add to rule
            rule.Options.Add(jopt);
            
            //Execute
            FirewallManager.instance.ApplyRule(rule);
            //Execute in default table named filter
            this.ruleSet.ExecRule(rule);
            
            //Create rule to accept new or related connections
            rule = new NetfilterRule();
            
            //Create append rule command
            acmd = new AppendRuleCommand();
            acmd.ChainName = this.chainName;
            //set in the rule
            rule.Command = acmd;
            
            //Load state extension with -m option
            MatchExtensionOption meop = new MatchExtensionOption();
            meop.Extension = MatchExtensions.State;
            //Add to rule
            rule.Options.Add(meop);
            //The previous option causes the extension to be instantiated and
            //added. We add the parameter directly.
            rule.LoadedExtensions[meop.Extension].AddParameter("state","RELATED,ESTABLISHED");
            
            //Create jump option to ACCEPT
            jopt = new JumpOption();
            jopt.Target = RuleTargets.Accept;
            //Add to rule
            rule.Options.Add(jopt);
            
            //Execute
            FirewallManager.instance.ApplyRule(rule);
            //Execute in default table named filter
            this.ruleSet.ExecRule(rule);
             
            //Create rule to drop anything else
            rule = new NetfilterRule();
             
            //Create append rule command
            acmd = new AppendRuleCommand();
            acmd.ChainName = this.chainName;
            //Set in the rule
            rule.Command = acmd;
             
            //Create jump option with drop target for all non-matching packets
            //if there is no previous chain.
            jopt = new JumpOption();
            jopt.Target = RuleTargets.Drop;
            //Add to rule
            rule.Options.Add(jopt);
             
            //Execute
            FirewallManager.instance.ApplyRule(rule);
            //Execute in default table named filter
            this.ruleSet.ExecRule(rule);
        }

        /// <summary>
        /// Adds a new rule to the chain to grant access for the ipAddr address.
        /// </summary>
        /// <returns>
        /// The rule added.
        /// </returns>
        public NetfilterRule GrantAccess(string ipAddr, int port, ProtocolType pType)
        {
            if(Net20.StringIsNullOrEmpty(ipAddr))
                throw new ArgumentException("The address can't be null or empty", "ipAddr");
                
            Debug.VerboseWrite("NetfilterRule:: Granting access for "+
                    ipAddr+" to port "+port+" for protocol "+pType);
            
            if( pType != ProtocolType.Tcp && pType != ProtocolType.Udp )
                throw new ArgumentException ("The only allowed protocols are tcp and udp",
                                             "pType");
                                             
            Debug.VerboseWrite ("FirewallManager::GrantAccess: Creating new rule");
            
            //Create rule
            NetfilterRule rule = new NetfilterRule();
            //Create command
            InsertRuleCommand cmd = new InsertRuleCommand();
            //Just insert after the rule that keeps outgoing connections working
            cmd.RuleNum = 3;
            cmd.ChainName = this.chainName;
            //Set command in rule
            rule.Command = cmd;
            //Create option
            SourceOption sop = new SourceOption();
            sop.Address = IpAddressRange.Parse(ipAddr);
            //Add option
            rule.Options.Add(sop);
            
            //Protocol type option
            ProtocolOption pOpt = new ProtocolOption();
            pOpt.Protocol = pType;
            
            Debug.VerboseWrite ("FirewallManager::GrantAccess: Adding protocol "+pOpt);
            //Add to rule
            rule.Options.Add (pOpt);
            
            //The above option loads an implicit extension: tcp
            Debug.VerboseWrite ("FirewallManager::GrantAccess: Adding extended option");
            
            //Add a extended option named --dport with the port as parameter
            if(pType == ProtocolType.Tcp)
                rule.LoadedExtensions[MatchExtensions.Tcp].AddParameter("dport",port.ToString());
            else if(pType == ProtocolType.Udp)
                rule.LoadedExtensions[MatchExtensions.Udp].AddParameter("dport",port.ToString());
                
            Debug.VerboseWrite ("FirewallManager::GrantAccess: Adding jump option");
            
            //Create option
            JumpOption jop = new JumpOption();
            
//            NetfilterTable nTable = this.ruleSet.GetDefaultTable();
//            
//            //If there are more rules we return to them to continue traversing existing rules
//            //TODO: Encapsulate this as a new operation mode -> mangelp
//            if(nTable.Chains.Length>1 && nTable.Chains[0].CurrentName == this.chainName)
//            {
//                //Back to input chain but to the next rule.
//                jop.Target = RuleTargets.Return;
//            }
//            else
//            {
                jop.Target = RuleTargets.Accept;
//            }
            
            //Add option
            rule.Options.Add(jop);

            
            //Execute
            FirewallManager.instance.ApplyRule(rule);
            //Execute in default table named filter
            this.ruleSet.ExecRule(rule);
            
            Debug.VerboseWrite ("FirewallManager::GrantAccess: Done!");
            return rule;
        }
        
        /// <summary>
        /// Creates a backup file with the current rule set in netfilter
        /// </summary>
        /// <returns>
        /// The file name with the backup
        /// </returns>
        public string BackupCurrentSet()
        {
            string fileName = UnixNative.CreateTempFileName();
            Debug.Write("FirewallManager:: Storing backup copy of set: "+fileName+"");
            IpTablesCmd.Save(fileName);
            return fileName;
        }
        
        /// <summary>
        /// Restores a rule set from a file
        /// </summary>
        public void RestoreRuleSetBackup(string file, bool delete)
        {
            if(!System.IO.File.Exists(file))
            {
//                throw new System.IO.FileNotFoundException("The file doesn't exists: "+file);
                Debug.Write ("Backup file not found: "+file);
                return;
            }
                
            Debug.Write("Restoring backup copy from: "+file);
            
            IpTablesCmd.Restore(file);
            
            if(delete)
                System.IO.File.Delete (file);
        }
        
        public void Clear()
        {
            Debug.VerboseWrite ("FirewallManager:: Clearing all rules", 
                    VerbosityLevels.Insane);
                    
            //Clear the rule set
            this.ruleSet.Clear();
            
            Debug.VerboseWrite ("FirewallManager:: Creating new object", 
                    VerbosityLevels.Insane);
            //Clear ourselves from the static reference so everything gets removed.
            FirewallManager.instance = new FirewallManager ();
        }
	}
}
