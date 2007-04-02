// project created on 11/01/2007 at 17:42
using System;
using System.IO;
using System.Threading;

using SharpKnocking.Common;
using SharpKnocking.Common.Calls;
using SharpKnocking.KnockingDaemon.PacketFilter;
using SharpKnocking.KnockingDaemon.SequenceDetection;

namespace SharpKnocking.KnockingDaemon
{
   
	// Main class for the daemon process
	class MainClass
	{
        [MTAThread()]
		public static int Main(string[] args)
		{
            
		    NetfilterDaemon nDaemon = new NetfilterDaemon();
            Thread nThread;
            TcpdumpMonitor capDaemon = new TcpdumpMonitor();
            Thread capThread;
            
            bool noruledaemon = false;
            bool nocapturedaemon = false;
            
            //Moved below
		    
		    // Here we do parameter processing
		    for(int i=0; i<args.Length; i++)
		    {
		        Debug.VerboseWrite ("Parameters[ "+i+" ] = "+args[i]+"");
		        
		        if(args[i]=="--cfg")
		        {
		            nDaemon.Parameters.Add("cfg",args[++i]);
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
                else if(args[i]=="--noruledaemon")
                {
                    noruledaemon=true;
                }
                else if(args[i]=="--nocapturedaemon")
                {
                    nocapturedaemon=true;
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
                    nDaemon.Parameters.Add("ldcurrent",null);
                }
                else if(args[i]=="-h" || args[i]=="--help")
                {
                    PrintHelpMessage();
                    return 0;
                }
		    }
		    
		    
            if(!UnixNative.CreateLockFile() && !File.Exists(UnixNative.LockFile))
            {
                throw new Exception("Can't create lock file!");
            }
            else
            {
                Debug.Write("Lock file created!");
            }

                        
            try
            {
                Debug.Write("Instantiating Rule daemon");
                nThread = new Thread(new ThreadStart(nDaemon.Run));
                
                if(!noruledaemon)
                {
                    Debug.Write("Starting rule daemon thread");
                    nThread.Start();
                }
                else
                {
                    Debug.Write("Rule daemon not starting");
                }
                
                Debug.Write("Instantiating capture daemon");
        		capThread = new Thread(new ThreadStart(capDaemon.Run));
                
                if(!nocapturedaemon)
                {           
                    CallSequence [] sequences = CallsLoader.Load();
                    capDaemon.SetSequences(sequences);
                    
                    SequenceDetectorManager detectorManager =
                    	new SequenceDetectorManager(sequences);
                    
                    detectorManager.LinkPacketMonitor(capDaemon);
                    
                    Debug.Write("Starting capture daemon thread");
                    
                    capThread.Start();
                }
                else
                {
                    Debug.Write("Capture daemon not starting");
                }
                
                //This is the daemon in charge of inter process comunication. It
                //behaves as a remote control for all the functionality.
                InterCommDaemon icDaemon = new InterCommDaemon();
                icDaemon.CapDaemon = capDaemon;
                icDaemon.NetDaemon = nDaemon;
                
                //Run the communication daemon
                icDaemon.Run();
            }
            catch(Exception ex)
            {
                Debug.Write("Unexpected fail: "+ex.Message);
                Debug.Write("Details: \n"+ex);
                Debug.Write("Finishing daemon. Removing lock file...");
                UnixNative.RemoveLockFile();
                return 1;
            }
		    
		    return 0;
		}
        
        private static void PrintHelpMessage()
        {
            Console.Out.WriteLine("KnockingDaemon service for the SharpKnocking suite.");
            Console.Out.WriteLine("Use -h to print usage information.");
            
        }
	}
}