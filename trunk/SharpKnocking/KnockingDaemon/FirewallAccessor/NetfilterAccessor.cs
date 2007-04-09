using System;
using System.Threading;
using System.Collections;

using SharpKnocking.Common;
using SharpKnocking.Common.Calls;
using SharpKnocking.NetfilterFirewall;
using SharpKnocking.KnockingDaemon.SequenceDetection;

namespace SharpKnocking.KnockingDaemon.FirewallAccessor
{
	
	/// <summary>
	/// Daemon code that uses all the libraries created.
	/// Loads the ruleset from netfilter throught iptables, listens to incoming
	/// knockings and alters the ruleset to configure properly the firewall.
	/// </summary>
	public class NetfilterAccessor: IDisposable
	{	
	    private string backupRulesFile;
	    
	    private FirewallManager fManager;
	    
	    /// <summary>
	    /// Gets/Sets if the real rule set in the firewall will be modified by
	    /// the actions over the rules
	    /// </summary>
	    public bool DryRun
	    {
	       get { return this.fManager.DryRun;}
	       set { this.fManager.DryRun = value;}
	    }
	
	    public Hashtable parameters;
	    
	    /// <summary>
	    /// Parameters for the accessor.
	    /// </summary>
	    public Hashtable Parameters
	    {
	       get { return this.parameters ; }
	    }
	    
		/// <summary>
		/// Empty constructor. Inits some collections.
		/// </summary>
		public NetfilterAccessor()
		{
		    this.parameters = new Hashtable();
		    this.fManager = FirewallManager.GetInstance;
		}
		
		/// <summary>
		/// Loads the current netfilter rule set.
		/// </summary>
		/// <remarks>
		/// If there is an unexpected exception the old rule set is reloaded.
		/// </remarks>
		public void Init()
		{
            try
            {
                Debug.VerboseWrite("NetfilterAccessor::Init("+this.DryRun+")", VerbosityLevels.High);
                this.backupRulesFile = this.fManager.BackupCurrentSet();
                
                
                if(this.Parameters.ContainsKey("cfg"))
                {
                    //Load a rule set. Usefull for debugging
                    this.fManager.LoadRuleSetFrom((string)this.Parameters["cfg"]);    
                }
                else if(this.Parameters.ContainsKey("ldcurrent"))
                {
                    //Load the current rule set in netfilter tables
                    this.fManager.LoadCurrentRuleSet();
                }
                else
                {
                    //We did a backup of the rule set so we can safely
                    //remove all rules applying our own rule set.
                    this.fManager.RuleSet.SetAsSafe();
                    Debug.VerboseWrite("NetfilterAccessor::Init(emptyrules)", VerbosityLevels.High);
                    this.fManager.ApplyCurrentRuleSet();
                }

                if(!this.fManager.RuleSet.IsSafe)
    		    {
    		        Debug.Write("The iptables rule set haven't been"+
                              " loaded. How i'm going to work without"+
                              " it?!");
    		        throw new InvalidOperationException("Can't load the required data");
    		    }

                Debug.VerboseWrite("NetfilterAccessor::Init(addknockingchain)", VerbosityLevels.High);
                this.fManager.AddSharpKnockingChain();
                
                if(!this.Parameters.ContainsKey("ldcurrent"))
                {
                    this.fManager.ApplyCurrentRuleSet();
                }
                
                Debug.VerboseWrite("NetfilterAccessor::Init(rulesetused)", VerbosityLevels.High);
                Debug.VerboseWrite("**\n"+this.fManager.RuleSet.SaveToString()+"\n**");
            }
            catch(Exception ex)
            {
                Debug.Write("Error in rule daemon:\n"+ex);
                Debug.Write("Restoring previous rule set...");
                this.fManager.RestoreRuleSetBackup(this.backupRulesFile);
                this.backupRulesFile = String.Empty;
            }
		}
		
		/// <summary>
		/// Ends the activity for the accessor restoring the old rule set if
		/// there is a valid file and if its not running dry.
		/// </summary>
		public void End()
		{
		    if(!this.DryRun && !Net20.StringIsNullOrEmpty(this.backupRulesFile))
		    {
		        Debug.Write("Restoring previous rule set ...");
		        this.fManager.RestoreRuleSetBackup(this.backupRulesFile);
		        this.backupRulesFile = String.Empty ;
		    }
		    
		    //This clears the manager rule set 
		    this.fManager.Dispose();
		}
		
		/// <summary>
		/// Add a rule that gives acces for a ip
		/// </summary>
		public void AddAccessToIp(string ip, int port)
		{
		    this.fManager.GrantAccess(ip, port);
		}
		
		/// <summary>
		/// </summary>
		public void Dispose()
		{

		}
	}
}
