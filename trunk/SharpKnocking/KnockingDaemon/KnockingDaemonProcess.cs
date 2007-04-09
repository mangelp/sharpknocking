
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
        private NetfilterAccessor accessor;
        private CallSequence[] calls;
        private RemoteDaemon commObject;
        private ObjRef commObjectRef;
        private bool doCapture = true;
        private bool isInteractiveMode;
        private bool isStarted;
        private RemoteManager managerObject;
        private TcpdumpMonitor monitor;
        private Thread monitorThread;
        private bool running;
        private SequenceDetectorManager seqManager;
        private TcpChannel tcpChannel;
        
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
            get { return true;}
        }
        
        /// <summary>
        /// Gets/Sets a reference to the daemon that capture packets and generate
        /// objects with sequence information.
        /// </summary>
        public TcpdumpMonitor Monitor
        {
            get { return monitor;}
            set { this.monitor = value;}
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
		}
		
		/// <summary>
		/// Clear everything.
		/// </summary>
        public void Dispose()
        {
            this.UnregisterManagerEnd();
        
            this.commObject = null;
            
            //Delete comunication object
            if(this.commObjectRef != null)
            {
                RemotingServices.Unmarshal(this.commObjectRef);
                this.commObjectRef = null;
            }
                
            //Unregister channel
            if(this.tcpChannel != null)
            {
                ChannelServices.UnregisterChannel(this.tcpChannel);
                this.tcpChannel = null;
            }
            
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
                
             if(this.monitor!=null)
             {
                 this.monitor.Dispose();
                 this.monitor = null;
             }
        }
        
        /// <summary>
        /// Runs the comunication daemon
        /// </summary>
        public void Run()
        {
            this.running = true;
            
            try
            {
                //Init rules accessor
                this.accessor.Init();
                //We need to register our remoting object and try to reach the
                //one from the manager
                this.RegisterServerObject();
                this.RegisterManagerEnd();
                //This will start the monitor thread and load the sequences
                this.HotRestart();
                
                //Keep the process up
                while(this.running)
                {
                	// FIX: Hey, we don't want to eat all CPU cicles.
                	Thread.Sleep(100);
                	
                }
            }
            catch(Exception ex)
            {
                Debug.Write("Exception catched in KnockingDaemon processing.");
                Debug.VerboseWrite("Details: "+ex);
                this.accessor.End();
                throw ex;
            }
            
            this.running = false;
        }
        
        /// <summary> 
        /// Stops the procesing.
        /// </summary>
        public void Stop()
        {
            this.running = false;
            Console.Out.WriteLine("Stopping requested. Daemon will die now");
            this.InternalStopMonitor();
            this.Dispose();
        }
        
        /// <summary>
        /// Do a hot restart of the processing to reload config changes.
        /// </summary>
        public void HotRestart()
        {
            this.running = false;
            //Inconditionally we kill the monitor if it is running and if not
            //we start it if the flag doCapture isn't set to false
            this.InternalStopMonitor();
            
            if(this.doCapture)
            {
                Debug.Write("Initing packet capture process");
                
                try
                {
                	PortInverseResolver.LoadTranslations();
                
                    //Load the calls
                    this.calls = CallsLoader.Load();
                    //Create a new monitor for these calls
                    this.monitor = new TcpdumpMonitor(this.calls);
                    //Create a new sequence manager that gets the notifications about
                    //packets from the monitor and uses the current calls array.
                    this.seqManager = new SequenceDetectorManager(this.calls, this.monitor);
                    if(this.monitorThread!=null)
                        this.monitorThread = null;
                    //Start a new thread
                    this.monitorThread = new Thread(new ThreadStart(this.monitor.Run));
                    Debug.VerboseWrite ("Starting new thread", VerbosityLevels.High);
                    this.monitorThread.Start();
                }
                catch(ThreadAbortException ex)
                {
                    Debug.VerboseWrite("KnockingDaemon::HotRestart() exception: \n"+ex);
                }
            }
            else
            {
                Debug.Write("Packet capture disabled by command line option --nocapture");
                this.seqManager = null;
                this.monitorThread = null;
                this.monitor = null;
            }
            
            this.running = true;
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
            if(this.doCapture)
            {
                Debug.Write("Stopping capture processing.");
                
                if(this.seqManager!=null)
                {
                    this.seqManager.Dispose();
                    this.seqManager = null;
                }
                
                this.calls = new CallSequence[0];
                
                if(this.monitor!= null && this.monitor.Running)
                {
                    Debug.Write("Killing packet capture process");
                    this.monitor.Stop();
                    
//                    if(this.monitorThread.ThreadState == ThreadState.Running)
//                    {
//                        this.monitorThread.Abort();
//                    }
                }
                else
                {
                    this.monitor = null;
                }
            }
            else
            {
                Debug.Write("Capture processing disabled. Can't stop.");
            }
        }
        
        private void RegisterServerObject()
        {
            //Create the channel for the daemon part of communication
            this.tcpChannel = new TcpChannel(RemoteEndService.DaemonPortNumber);
            
            ChannelServices.RegisterChannel(this.tcpChannel);
            
            //Create the object for the comunication
            this.commObject = new RemoteDaemon(true);
            //Set as a remoting object to comunicate
            this.commObjectRef = RemotingServices.Marshal(this.commObject, RemoteEndService.DaemonServiceName);
            //Set a handler in the event to get notifications about incoming messages
            this.commObject.Received += new RemoteEndEventHandler(this.OnReceivedHandler);
        }
        
//        private void OnDisposedHandler(object sender, EventArgs args)
//        {
//            if(this.running)
//            {
//                Debug.VerboseWrite("The packet pacture have ended by an unexpected error. Stopping daemon.");
//                this.Stop();
//            }
//        }
        
        //Handles a sequence detected
        private void OnSequenceDetectedHandler(object sender, 
                        SequenceDetectorEventArgs args)
        {
            if(this.isInteractiveMode)
            {
                this.SendRequest (RemoteCommandActions.AccessRequest, 
                        args.IP + "<>" + args.SerializedSequence );  
            }
            else
            {
                this.accessor.AddAccessToIp(args.IP, args.Port);
            }
        }
        
        private void OnReceivedHandler(object sender, RemoteEndEventArgs args)
        {
            if(args.IsRequest)
                this.HandleRequest(args);
            else
                this.HandleResponse(args);
        }
        
        private void HandleRequest(RemoteEndEventArgs args)
        {
            Debug.VerboseWrite("Received request!\nRequest: "+args.Action+" Data: '"+args.Data+"'"); 
            switch(args.Action)
            {
                case RemoteCommandActions.Bye:
                    this.UnregisterManagerEnd();
                    break;                    
                case RemoteCommandActions.Die:
                    this.Stop();
                    break;
                case RemoteCommandActions.Hello:
                    Debug.VerboseWrite("Hello back to manager", VerbosityLevels.High);
                    this.SendResponse(RemoteCommandActions.Hello , null);
                    break;
                case RemoteCommandActions.EndInteractiveMode:
                    this.isInteractiveMode = false;
                    break;
                case RemoteCommandActions.HotRestart:
                    Debug.VerboseWrite("KDP: <HotRestart>");
                    this.HotRestart();
                    Debug.VerboseWrite("KDP: </HotRestart>");
                    this.SendResponse(args.Action, true);
                    break;
                case RemoteCommandActions.StartInteractiveMode:
                    this.isInteractiveMode = true;
                    break;
                case RemoteCommandActions.Status:
                case RemoteCommandActions.StatusExtended:
                    if(this.isInteractiveMode)
                        this.SendResponse(RemoteCommandActions.Status, RemoteServerStatus.StartedInteractiveMode);
                    else if(this.isStarted)
                        this.SendResponse(RemoteCommandActions.Status, RemoteServerStatus.Started);
                    else
                        this.SendResponse(RemoteCommandActions.Status, RemoteServerStatus.Stopped);
                    break;
                case RemoteCommandActions.Stop:
                    this.Stop();
                    break;
            }
        }
            
        private void SendResponse(RemoteCommandActions action, object data)
        {
            if(this.managerObject==null)
                this.RegisterManagerEnd();
               
//            if(this.managerObject==null)
//                throw new RemotingException(
//                     "Can't connect to the manager end to answer request! ("+
//                     action+")");
            
            //Return instead of givving an exception
            if(this.managerObject == null)
                return;
            
            Debug.VerboseWrite("KnockingDaemonProcess: Sending response to "+action);
            
            this.managerObject.SendResponse(action, data);
        }
        
        private void SendRequest(RemoteCommandActions action, object data)
        {
            if(this.managerObject==null)
                this.RegisterManagerEnd();
            
//            if(this.managerObject==null)
//                throw new RemotingException(
//                     "Can't connect to the manager end to answer request! ("+
//                     action+")");

            //If there isn't manager go back without sending answer.
            if(this.managerObject == null)
                return;
                
            Debug.VerboseWrite("KnockingDaemonProcess: Sending request to "+action);
            this.managerObject.SendRequest(action, data);
        }

        private void HandleResponse(RemoteEndEventArgs args)
        {
            switch(args.Action)
            {
                case RemoteCommandActions.Accept:
                    //TODO: Accepted call
                    break;
                case RemoteCommandActions.Deny:
                    //TODO: Denied call
                    break;
            }
        }
        
        /// <summary>
        /// Starts the comunication with the manager
        /// </summary>
        private void RegisterManagerEnd()
        {
            Debug.VerboseWrite("Registering manager end: ", VerbosityLevels.High);
            
            if(this.managerObject == null)
            {
                try
                {
        	        string uri = "tcp://localhost:"+
        	                     RemoteEndService.ManagerPortNumber+
        	                     "/"+RemoteEndService.ManagerServiceName;
                
                    this.managerObject = (RemoteManager)Activator.GetObject(
                                                            typeof(IRemoteManager),
                                                            uri);
                                                             
                    if(this.managerObject!=null)
                    {
                        Debug.VerboseWrite("Manager found!\nSending hello to manager ...", 
                                VerbosityLevels.High);
                        this.SendRequest(RemoteCommandActions.Hello, null);
                    }
                    else
                    {
                        Debug.VerboseWrite("Manager end not found!", VerbosityLevels.High);
                    }
                }
                catch(RemotingException ex)
                {
                    Debug.VerboseWrite("Can't get daemon remote object\nDetails:"+ex);
                }
            }
        }
        
        /// <summary>
        /// Ends the comunication with the manager
        /// </summary>
        private void UnregisterManagerEnd()
        {
            if(this.managerObject!=null)
            {
                this.managerObject = null;
            }
        }
        
	}
}
