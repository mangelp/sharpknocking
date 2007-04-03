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
	public class NetfilterAccessor
	{	
	
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
	    
	    private bool running;
	    
	    /// <summary>
	    /// If true the accessor is working but if it is false the accesor 
	    /// failed.
	    /// </summary>
	    public bool Running
	    {
	       get{ return this.running;}
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
		public void Init()
		{
            try
            {
                if(this.Parameters.ContainsKey("cfg"))
                {
                    this.fManager.LoadRuleSetFrom((string)this.Parameters["cfg"]);    
                }
                else if(this.Parameters.ContainsKey("ldcurrent"))
                {
                    this.fManager.LoadCurrentRuleSet();
                }
                else
                {
                    this.fManager.RuleSet.SetAsSafe();
                    this.fManager.ApplyCurrentRuleSet();
                }

                if(!this.fManager.RuleSet.IsSafe)
    		    {
    		        Debug.Write("The iptables rule set haven't been"+
                              " loaded. How i'm going to work without"+
                              " it?\n Daemon thread exiting!");
                    this.running = false;
    		        return;
    		    }

                this.fManager.AddSharpKnockingChain();
                
                if(!this.Parameters.ContainsKey("ldcurrent"))
                {
                    this.fManager.ApplyCurrentRuleSet();
                }
                
                Debug.VerboseWrite("Applying ruleset:\n<Ruleset>");
                Debug.VerboseWrite(this.fManager.RuleSet.SaveToString());
                Debug.VerboseWrite("</Ruleset>");
            }
            catch(Exception ex)
            {
                Debug.Write("Error in rule daemon:\n"+ex);
            }
		}
		
	}
}
