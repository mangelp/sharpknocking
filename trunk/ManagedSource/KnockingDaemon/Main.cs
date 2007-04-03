// project created on 11/01/2007 at 17:42
using System;
using System.IO;
using System.Threading;

using SharpKnocking.Common;
using SharpKnocking.Common.Calls;
using SharpKnocking.NetfilterFirewall;
using SharpKnocking.KnockingDaemon.PacketFilter;
using SharpKnocking.KnockingDaemon.SequenceDetection;
using SharpKnocking.KnockingDaemon.FirewallAccessor;

namespace SharpKnocking.KnockingDaemon
{
   
	// Main class for the daemon process
	class MainClass
	{
	    /// <summary>
	    /// Application entry point
	    /// </summary>
	    /// <remarks>
	    /// Exit codes:</br>
	    /// 0: Normal exit.</br>
	    /// 1: Another instance running.</br>
	    /// 2: Can't create lock file.</br>
	    /// 3: Unmanaged exception thrown.</br>
	    /// 4: Tried to run without root permissions.</br>
	    /// 5: WTF!</br>
	    /// </remarks>
        [MTAThread()]
		public static int Main(string[] args)
		{
            //This is the daemon in charge of inter process comunication. It
            //behaves as a remote control for all the functionality.
            KnockingDaemonProcess daemon = new KnockingDaemonProcess();
		    
		    // Here we do parameter processing
		    for(int i=0; i<args.Length; i++)
		    {
		        Debug.VerboseWrite ("Parameters[ "+i+" ] = "+args[i]+"");
		        
		        if(args[i]=="--cfg")
		        {
		            daemon.Accessor.Parameters.Add("cfg",args[++i]);
		        }
		        else if(args[i]=="--dbg")
		        {
		            // Enable debugging
		            Debug.DebugEnabled = true;
		        }
		        else if(args[i]=="-v")
		        {
		            // Print more messages
		            Debug.MoreVerbose = true;
                    Debug.VerbLevel = VerbosityLevels.Normal;
		        }
                else if(args[i]=="--nofwmodify")
                {
                    daemon.Accessor.DryRun =true;
                }
                else if(args[i]=="--nocapture")
                {
                    daemon.DoCapture = false;
                }
                else if(args[i]=="-vv")
                {
                    Debug.MoreVerbose = true;
                    Debug.VerbLevel = VerbosityLevels.High;
                }
                else if(args[i]=="-vvv")
                {
                    Debug.MoreVerbose = true;
                    Debug.VerbLevel = VerbosityLevels.Insane;
                }
                else if(args[i]=="--ldcurrent")
                {
                    daemon.Accessor.Parameters.Add("ldcurrent",null);
                }
                else if(args[i]=="-h" || args[i]=="--help")
                {
                    PrintHelpMessage();
                    return 0;
                }
		    }
		    
		    if(!UnixNative.ExecUserIsRoot())
		    {
		        //If we aren't rut exit mercyfully
		        Console.Out.WriteLine("You need root permissions to run this daemon");
		        return 4;
		    }
		    else if(UnixNative.ExistsLockFile())
		    {
		        //The file already exists. Daemon created.
		        Console.Out.WriteLine("Another instance of the daemon is running. If not\n"+
		                      "remove the lock file: "+UnixNative.LockFile);
		        return 1;
		    }
            else if(!UnixNative.CreateLockFile() || !File.Exists(UnixNative.LockFile))
            {
                Console.Out.WriteLine("Can't create lock file. Check the permissions to write into\n"+
                                "the file: "+UnixNative.LockFile );
                return 2;
            }
            else
            {
                Debug.Write("Lock file created!");
            }

                        
            try
            {
                //Run the communication daemon
                Debug.Write("Instantiating communication daemon");
                daemon.Run();
            }
            catch(Exception ex)
            {
                Debug.VerboseWrite("Unexpected fail: "+ex.Message, VerbosityLevels.Insane );
                Debug.Write("Details: \n"+ex);
                Debug.Write("Finishing daemon. Removing lock file...");
                UnixNative.RemoveLockFile();
                return 3;
            }
		    
		    return 0;
		}
        
        private static void PrintHelpMessage()
        {
            Console.Out.WriteLine ("KnockingDaemon service for the SharpKnocking suite.");
            Console.Out.WriteLine ("Released under LGPL terms.");
            Console.Out.WriteLine ("(c)2007 Luís Roman Gutierrez y Miguel Ángel Pérez Valencia"); 
            Console.Out.WriteLine ("Commands: --nofwmodify, --nocapture, --dbg, -v, -vv, -vvv, -h, --help, --cfg, --dry");
            Console.Out.WriteLine ("     --dbg, -v, -vv, -vvv: The first activates debuggin and the rest the level of");
            Console.Out.WriteLine ("       detail.");
            Console.Out.WriteLine ("     --nocapture: Don't start the capture thread. This makes daemon unusable");
            Console.Out.WriteLine ("     --nofwmodify: Don't modify current rule set. This makes daemon unusable");
            Console.Out.WriteLine ("     --cfg: The next argument must be a valid iptables configuration file that");
            Console.Out.WriteLine ("       will be loaded.");
            Console.Out.WriteLine ("     --dry: The current ruleset of the firewall will remain untouched");
            
        }
	}
}