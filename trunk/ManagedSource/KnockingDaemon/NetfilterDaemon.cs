using System;
using System.Threading;
using System.Collections;

using SharpKnocking.Common;
using SharpKnocking.Common.Calls;
using SharpKnocking.NetfilterFirewall;
using SharpKnocking.KnockingDaemon.SequenceDetection;

namespace SharpKnocking.KnockingDaemon
{
	
	/// <summary>
	/// Daemon code that uses all the libraries created.
	/// Loads the ruleset from netfilter throught iptables, listens to incoming
	/// knockings and alters the ruleset to configure properly the firewall.
	/// </summary>
	public class NetfilterDaemon
	{
        private bool running;
        
        public bool Running
        {
            get { return this.running;}
        }
        
	    private int exitCode = 0;
	    
	    public int ExitCode
	    {
	        get {return this.exitCode;}    
	    }
	    
	    public Hashtable Parameters;
	    
	    private bool stop;
	    
		/// <summary>
		/// Empty constructor. Inits some collections.
		/// </summary>
		public NetfilterDaemon()
		{
		    this.Parameters = new Hashtable();
		}
        
        public void Stop()
        {
            this.stop = true;
        }
		
		/// <summary>
		/// Starts the daemon work. Loads the current netfilter rule set, knocking
        /// configuration and starts the packet capture and analising.
		/// </summary>
		public void Run()
		{
            this.running = true;
            
	    	//Test it
            
            try
            {
            
                if(this.Parameters.ContainsKey("cfg"))
                {
                    FirewallManager.GetInstance.LoadRuleSetFrom((string)this.Parameters["cfg"]);    
                }
                else if(this.Parameters.ContainsKey("ldcurrent"))
                {
                    FirewallManager.GetInstance.LoadCurrentRuleSet();
                }
                else
                {
                    FirewallManager.GetInstance.RuleSet.SetAsSafe();
                    FirewallManager.GetInstance.ApplyCurrentRuleSet();
                }
                

                if(!FirewallManager.GetInstance.RuleSet.IsSafe)
    		    {
    		        Debug.Write("The iptables rule set haven't been"+
                              " loaded. How i'm going to work without"+
                              " it?\n Daemon thread exiting!");
    		        this.exitCode = 1;
                    this.running = false;
    		        return;
    		    }

                FirewallManager.GetInstance.AddSharpKnockingChain();
    		    
    		    System.Random randGen = new System.Random();
    		    int wait=0;
                
                if(!this.Parameters.ContainsKey("ldcurrent"))
                {
                    FirewallManager.GetInstance.ApplyCurrentRuleSet();
                }
                
                Debug.VerboseWrite("Applying ruleset:\n<Ruleset>");
                Debug.VerboseWrite(FirewallManager.GetInstance.RuleSet.SaveToString());
                Debug.VerboseWrite("</Ruleset>");
    		    
    			//Wait here for events until it is killed
    			while(!this.stop)
    			{
                    wait = randGen.Next(15, 255);
                    
                    Debug.Write("Netfilter daemon getting bored "+(wait*100)+" ms");
                    
                    //Debug.Write("If i would like i could give access to 150.214.142."+wait);
                    //Don't insert this
                    //FirewallManager.GetInstance.GrantAccess("150.214.142."+wait);
    			    
    				Thread.Sleep(wait*1000);
    			}
            }
            catch(Exception ex)
            {
                Debug.Write("Error in rule daemon:\n"+ex);
            }
            finally
            {
                this.running = false;
            }
            
            this.running = false;
		}
		
	}
}
