
using System;
using System.Threading;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

using Mono.Unix.Native;

using SharpKnocking.Common;
using SharpKnocking.Common.Calls;
using SharpKnocking.Common.Remoting;
using SharpKnocking.NetfilterFirewall;
using SharpKnocking.KnockingDaemon.PacketFilter;
using SharpKnocking.KnockingDaemon.FirewallAccessor;
using SharpKnocking.KnockingDaemon.SequenceDetection;

namespace SharpKnocking.KnockingDaemon
{
	
    /// <summary>
    /// Daemon class that manages the communication of the knockingDaemon
    /// with other applications that requires information about the current status
    /// or that want to change the daemon implementation.
    /// </summary>
    /// <remarks>
    /// This class is not really a daemon itself. It runs in the main thread started
    /// with the daemon and coordinates the other "daemon" class that run
    /// in a separate thread.
    /// </remarks>
	public class KnockingDaemonProcess: IDisposable
	{	
	    private Hashtable pendingCalls;
        private Hashtable acceptedKnockings;
	    private RemotingCommunicator communicator;
        private NetfilterAccessor accessor;
        private CallSequence[] calls;
        private bool doCapture = true;
        private bool isInteractiveMode;
        private static TcpdumpMonitor monitor;
        private Thread monitorThread;
        private bool running;
        private bool die;
        private SequenceDetectorManager seqManager;
        
        public bool Die
		{
		    get { return this.die;}
		    set { this.die = value;}
		}
        
        /// <summary>
        /// Gets/Sest if the capture process should be done
        /// </summary>
        public bool DoCapture
        {
            get { return this.doCapture ;}
            set { this.doCapture = value;}
        }
        
        public bool Running
        {
            get { return this.running;}
        }
        
        /// <summary>
        /// Gets/Sets a reference to the daemon that capture packets and generate
        /// objects with sequence information.
        /// </summary>
        public TcpdumpMonitor Monitor
        {
            get { return monitor;}
            set { monitor = value;}
        }
        
        /// <summary>
        /// Gets/sets a reference to the daemon that manages netfilter rules,
        /// receives sequence information and issues the rules that grant access
        /// to remote users.
        /// </summary>
        public NetfilterAccessor Accessor
        {
            get { return this.accessor ;}
            set { this.accessor = value;}
        }
		
		/// <summary>
		/// Default constructor.
		/// </summary>
		public KnockingDaemonProcess()
		{
		    this.accessor = new NetfilterAccessor ();
		    
		    if(!UnixNative.HandleTermSignal(new SignalHandler(this.HandleTermSignal)))
		    {
		        Console.Out.WriteLine ("KnockingDaemonProcess:: Term signal unhandled"); 
		    }
		    else
		    {
		        Console.Out.WriteLine ("KnockingDaemonProcess:: Term signal handled");
		    }
		    
            if(!UnixNative.HandleCtrlCSignal (new SignalHandler(this.HandleTermSignal)))
		    {
		        Console.Out.WriteLine ("KnockingDaemonProcess:: Ctrl-C signal unhandled"); 
		    }
		    else
		    {
		        Console.Out.WriteLine ("KnockingDaemonProcess:: Ctrl-C signal handled");
		    }
		    
		    this.doCapture = true;
		    
		    this.communicator = new RemotingCommunicator(typeof(RemoteDaemon), 
		                                                 typeof(RemoteManager));
		                                                 
		    this.communicator.RequestReceived += 
		          new RemotingCommunicatorEventHandler(this.OnRequestHandler);
		    this.communicator.ResponseReceived += 
		          new RemotingCommunicatorEventHandler(this.OnResponseHandler);
		          
		    this.communicator.LocalName = RemoteEndService.DaemonServiceName;
		    this.communicator.LocalPort = RemoteEndService.DaemonPortNumber;
		    this.communicator.RemoteName = RemoteEndService.ManagerServiceName;
		    this.communicator.RemotePort = RemoteEndService.ManagerPortNumber;
            
            Console.Out.WriteLine("KnockingDaemonProcess:: DAEMON: "+
                               RemoteEndService.DaemonServiceName+":"+RemoteEndService.DaemonPortNumber+
                               " MANAGER: "+
                               RemoteEndService.ManagerServiceName+":"+RemoteEndService.ManagerPortNumber);
		          
		    this.pendingCalls  = new Hashtable(20); 
            this.acceptedKnockings = new Hashtable(40); 
		}
		
		/// <summary>
		/// Clear everything.
		/// </summary>
        public void Dispose()
        {
            //Set sequence manager
	        if(this.seqManager !=null)
            {
                this.seqManager.SequenceDetected -= 
                        new SequenceDetectorEventHandler(this.OnSequenceDetectedHandler);
	            this.seqManager.Dispose();
	            this.seqManager = null;
	        }
	        
	        //Clear calls
	        this.calls = null;
	        
	        //Clear accessor
	        if(this.accessor !=null)
	        {
    	        this.accessor.End();
    	        this.accessor = null;
	        }
	        
	        //If still exists the lock file erase it
	        if(UnixNative.ExistsLockFile())
                UnixNative.RemoveLockFile();
                
             if(monitor!=null)
             {
                 monitor.Dispose();
                 monitor = null;
             }
        }
        
        /// <summary>
        /// Runs the comunication daemon
        /// </summary>
        public static void Run(KnockingDaemonProcess daemon)
        {
            daemon.running = true;
            
            try
            {
                //Init rules accessor
                daemon.accessor.Init();

                //Init communication
                daemon.communicator.Init();
                
                //Init packet monitor
                daemon.InternalStartMonitor();
                
                //Keep the process up
                while(!daemon.die)
                {
                	// FIX: Hey, we don't want to eat all CPU cicles.
                	Thread.Sleep(100);
                }
            }
            catch(Exception ex)
            {
                Debug.VerboseWrite("KnockingDaemonProcess::Run(): Exception catched");
                Debug.VerboseWrite("Details: "+ex);
                daemon.die = true;
            }
            
            daemon.running = false;
            
            daemon.InternalStopMonitor();
            
            Debug.VerboseWrite("KnockingDaemonProcess::Run(): Exiting");
        }
        
        /// <summary> 
        /// Stops the procesing.
        /// </summary>
        public void Stop()
        {
           //Net20.PrintThreadInformation("STOP");
           Debug.VerboseWrite("KnockingDaemonProcess::Stop():"+
                           "Stop requested.");
           this.die = true;
           
           if(this.monitorThread!=null)
           {
               Debug.VerboseWrite ("KnockingDaemonProcess::Stop:"+
                       " Thread status: "+this.monitorThread.ThreadState);
           }
           
           Console.Out.WriteLine("KnockingDaemonProcess::Stop():"+
                           "Stopping requested. Wait while daemon dies ...");
                           
           if(this.accessor !=null)
           {
               this.accessor.End ();
               this.accessor = null;
           }

           this.InternalStopMonitor();
        }
        
        /// <summary>
        /// Do a hot restart of the processing to reload config changes.
        /// </summary>
        public void HotRestart()
        {
            //Net20.PrintThreadInformation("HOTRESTART");
            Debug.VerboseWrite("KnockingDaemonProcess::HotRestart(): Invoked ...");
            this.InternalStopMonitor();
            
            this.InternalStartMonitor();
        }
        
        private void HandleTermSignal(int signal)
        {
            //Term signal marks the end of the processing
            Console.Out.WriteLine("TERM/CTRL-C signal cautch. Trying to exit graccessfully.("+signal+")");
            this.Stop();    
        }
        
        /// <summary>
        /// Stops the monitor for packets, kills the sequence manager and clears
        /// the sequences used.
        /// </summary>
        private void InternalStopMonitor()
        {
            if(!this.doCapture)
            {
                Debug.VerboseWrite ("KnockingDaemonProcess::InternalStopMonitor:"+
                        " Packet capture disabled by command option. Exiting");
                return;
            }
            
            //Net20.PrintThreadInformation("INTERNAL_STOP_MONITOR");
            Debug.VerboseWrite("KnockingDaemonProcess::InternalStopMonitor: "+
                    "Stopping capture processing.");
                    
            if(monitor!= null)
            {
                monitor.Stop();
                while(!monitor.Die);
                monitor = null;
            }
           
            if(this.seqManager!=null)
            {
                this.seqManager.Dispose();
                this.seqManager = null;
            }
           
            this.calls = new CallSequence[0];
        }
        
        private void InternalStartMonitor()
        {
            if(!this.doCapture)
            {
                Debug.VerboseWrite ("KnockingDaemonProcess:: "+
                        "Packet capture disabled by command option. Exiting");
                return;
            }
       
            //Net20.PrintThreadInformation("INTERNAL_START_MONITOR");
            Debug.Write("KnockingDaemonProcess:: Initing packet capture process");
           
            try
            {
           	    PortInverseResolver.LoadTranslations();
           
                //Load the calls
                this.calls = CallsLoader.Load();
                //Create a new monitor for these calls
                monitor = new TcpdumpMonitor(this.calls);
                //Create a new sequence manager that gets the notifications about
                //packets from the monitor and uses the current calls array.
                this.seqManager = new SequenceDetectorManager(this.calls, monitor);
                this.seqManager.SequenceDetected +=
                	new SequenceDetectorEventHandler(OnSequenceDetectedHandler);
                
                if(this.monitorThread!=null)
                {
                    this.monitorThread = null;
                }
                    
                //Start a new thread
                this.monitorThread = new Thread(new ParameterizedThreadStart(TcpdumpMonitor.Run));
                this.monitorThread.Name = "TcpdumpMonitorThread";
                Debug.VerboseWrite ("KnockingDaemonProcess::Starting new thread", VerbosityLevels.High);
                this.monitorThread.Start(monitor);
            }
            catch(ThreadAbortException ex)
            {
                Debug.VerboseWrite("KnockingDaemonProcess::InternalStartMonitor():"+
                        " Got exception: \n"+ex.Message+"\nDetails:"+ex);
            }
        }
        
        //Handles a sequence detected
        private void OnSequenceDetectedHandler(object sender, 
                        SequenceDetectorEventArgs args)
        {
        
        	Debug.VerboseWrite("KnockingDaemonProcess::Sequence detected");
        	
            if(this.isInteractiveMode)
            {
                Debug.VerboseWrite ("KnockingDaemonProcess::OnSequenceDetectedHandler:"
                        +"Sequence requested for accept!");
                this.pendingCalls.Add (args.IP+":"+args.Port, null);
            	Debug.VerboseWrite("KnockingDaemonProcess::Asking Doorman for clearance to continue");
                this.communicator.SendRequest (RemoteCommandActions.AccessRequest, 
                        args.IP + "<>" + args.SerializedSequence);  
            }
            else
            {
                this.accessor.AddAccessToIp(args.IP, args.Port);
            }
        }
        
        private void OnRequestHandler(object sender, RemotingCommunicatorEventArgs args)
        {
            switch(args.Action)
            {
                case RemoteCommandActions.Bye:
                    this.communicator.UnregisterRemoteEnd ();
                    this.isInteractiveMode = false;
                    break;                    
                case RemoteCommandActions.Die:
                    this.Stop();
                    break;
                case RemoteCommandActions.Hello:
                    Debug.VerboseWrite("Hello back to manager", VerbosityLevels.High);
                    this.communicator.SendResponse(RemoteCommandActions.Hello , null);
                    break;
                case RemoteCommandActions.EndInteractiveMode:
                    this.isInteractiveMode = false;
                    break;
                case RemoteCommandActions.HotRestart:
                    this.HotRestart();
                    //FIX: Don't answer hot restart command
                    //this.communicator.SendResponse(args.Action, true);
                    break;
                case RemoteCommandActions.StartInteractiveMode:
                    this.isInteractiveMode = true;
                    Debug.VerboseWrite("KnockingDaemonProcess::Activating interactive mode");
                    break;
                case RemoteCommandActions.Status:
                case RemoteCommandActions.StatusExtended:
                    if(this.running && this.isInteractiveMode)
                        this.communicator.SendResponse(RemoteCommandActions.Status, RemoteServerStatus.StartedInteractiveMode);
                    else if(this.running)
                        this.communicator.SendResponse(RemoteCommandActions.Status, RemoteServerStatus.Started);
                    else
                        this.communicator.SendResponse(RemoteCommandActions.Status, RemoteServerStatus.Stopped);
                    break;
                case RemoteCommandActions.Stop:
                    this.Stop();
                    break;
            }
        }

        private void OnResponseHandler(object sender, RemotingCommunicatorEventArgs args)
        {
            switch(args.Action)
            {
                case RemoteCommandActions.Accept:
                case RemoteCommandActions.Deny:
                
                	// If knocking daemon isn't in interactive mode,
                	// we shouldn't have recived this.
                    if(!this.isInteractiveMode)
                        return;
                    
                    string xml = (string)args.Data;
                    int pos = xml.IndexOf ("<>");
                    string ip;
                    CallSequence seq;
                    
                    if(pos >= 0)
                    {
                    	// TODO: Management of the data should be encapsulated
                    	// FIX: Create class SequenceHitData, and make it serializable.
                        ip = xml.Substring(0, pos);
                        seq = CallSequence.LoadFromString (xml.Substring(pos+2));
                        if(!this.pendingCalls.Contains(ip+":"+seq.TargetPort))
                        {
                            Debug.VerboseWrite("KnockingDaemonProcess::OnResponseHandler:"+
                                    "Received action "+args.Action+" for call in interactive"+
                                    " mode that isn't in the pending list\nCall data:\n"+
                                    args.Data);
                        }
                        else
                        {
                            if( args.Action == RemoteCommandActions.Accept)
                                this.accessor.AddAccessToIp (ip, seq.TargetPort);
                                
                            this.pendingCalls.Remove(ip+":"+seq.TargetPort);
                        }
                    }
                    else
                    {
                        Debug.Write ("KnockingDaemonProcess::OnResponseHandler: "+
                                "Malformed sequence received in response");
                    }
                    break;
            }
        }
                
	}
}
