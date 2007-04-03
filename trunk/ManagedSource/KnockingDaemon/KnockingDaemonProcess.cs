
using System;
using System.Threading;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

using SharpKnocking.Common;
using SharpKnocking.Common.Remoting;
using SharpKnocking.NetfilterFirewall;
using SharpKnocking.KnockingDaemon.PacketFilter;
using SharpKnocking.KnockingDaemon.FirewallAccessor;

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
	public class KnockingDaemonProcess: IDaemonProcessUnit
	{	
	    private Thread monitorThread;
	
        private TcpChannel tcpChannel;
        
        private RemoteDaemon commObject;
        private ObjRef commObjectRef;

        private RemoteManager managerObject;
        private bool isInteractiveMode;
        private bool isStarted;
	        
        private bool running;
        
        public bool Running
        {
            get { return this.running;}
        }
        
        private bool stopped;
        
        public bool Stopped
        {
            get { return this.stopped;}
        }
        
        private bool doCapture = true;
        
        /// <summary>
        /// Gets/Sest if the capture process should be done
        /// </summary>
        public bool DoCapture
        {
            get { return this.doCapture ;}
            set { this.doCapture = value;}
        }
	
        private TcpdumpMonitor monitor;
        
        /// <summary>
        /// Gets/Sets a reference to the daemon that capture packets and generate
        /// objects with sequence information.
        /// </summary>
        public TcpdumpMonitor Monitor
        {
            get { return monitor;}
            set { this.monitor = value;}
        }
        
        private NetfilterAccessor accessor;
        
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
        }
        
        /// <summary>
        /// Runs the comunication daemon
        /// </summary>
        public void Run()
        {
            this.running = true;
            
            try
            {
                //We need to register our remoting object and try to reach the
                //one from the manager
                this.RegisterServerObject();
                this.RegisterManagerEnd();
                //This will start the monitor
                this.HotRestart();
            }
            catch(Exception ex)
            {
                Debug.Write("Exception catched in intercommunication processing.");
                throw ex;
            }
        }
        
        /// 
        public void Stop()
        {
            if(!this.running)
                return;
            
            if(!this.doCapture)
            {
                Debug.Write("Stopping capture processing.");
                this.monitor.Stop();
            }
            else
            {
                Debug.Write("Capture processing disabled. Can't stop.");
            }
            
            this.stopped = true;
        }
        
        public void Start()
        {
            if(!this.running)
                return;
            
            if(!this.doCapture)
            {
                Debug.Write("Resuming capture processing");
                this.monitor.Start();
            }
            else
            {
                Debug.Write("Capture processing disabled. Can't start.");
            }
            
            this.stopped = false;
        }
        
        /// <summary>
        /// Do a hot restart of the processing to reload config changes.
        /// </summary>
        public void HotRestart()
        {
            //Inconditionally we kill the monitor if it is running and if not
            //we start it if the flag doCapture isn't set to false
            
            if(this.monitor!= null && this.monitor.Running)
            {
                Debug.Write("Killing packet capture process");
                this.monitor.Kill();
                
                if(this.monitorThread.ThreadState == ThreadState.Running)
                {
                    this.monitorThread.Abort();
                }
            }
            
            if(this.doCapture)
            {
                Debug.Write("Initing packet capture process");
                this.monitor = new TcpdumpMonitor();
                this.monitorThread = new Thread(new ThreadStart(this.monitor.Run));
                this.monitorThread.Start();
            }
            else
            {
                Debug.Write("Packet capture disabled by command line option --nocapture");
            }
        }
        
        private void RegisterServerObject()
        {
            //Create the channel for the daemon part of communication
            this.tcpChannel = new TcpChannel(RemoteEndService.DaemonPortNumber);
            
            ChannelServices.RegisterChannel(this.tcpChannel);
            
            //Create the object for the comunication
            this.commObject = new RemoteDaemon();
            //Set as a remoting object to comunicate
            this.commObjectRef = RemotingServices.Marshal(this.commObject, RemoteEndService.DaemonServiceName);
            //Set a handler in the event to get notifications about incoming messages
            this.commObject.Received += new RemoteEndEventHandler(this.OnReceivedHandler);
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
            switch(args.Action)
            {
                case RemoteCommandActions.Bye:
                    this.UnregisterManagerEnd();
                    break;                    
                case RemoteCommandActions.Hello:
                    this.SendResponse(RemoteCommandActions.Hello , null);
                    break;
                case RemoteCommandActions.EndInteractiveMode:
                    this.isInteractiveMode = false;
                    break;
                case RemoteCommandActions.HotRestart:
                    this.HotRestart();
                    this.SendResponse(args.Action, null);
                    break;
                case RemoteCommandActions.Start:
                    this.Start();
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
               
            if(this.managerObject==null)
                throw new RemotingException(
                     "Can't connect to the manager end to answer request! ("+
                     action+")");
            
            Debug.VerboseWrite("InterCommDaemon: Sending response to "+action);
            this.managerObject.SendResponse(action, data);
        }
        
        private void SendRequest(RemoteCommandActions action, object data)
        {
            if(this.managerObject==null)
                this.RegisterManagerEnd();
            
            if(this.managerObject==null)
                throw new RemotingException(
                     "Can't connect to the manager end to answer request! ("+
                     action+")");
                
            Debug.VerboseWrite("InterCommDaemon: Sending response to "+action);
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
            try
            {
                if(this.managerObject==null)
                    this.managerObject = (RemoteManager)Activator.GetObject(typeof(RemoteManager),
                                                         "tcp://localhost:"+
                                                         RemoteEndService.ManagerPortNumber+
                                                         "/"+RemoteEndService.ManagerServiceName);
            }
            catch(RemotingException ex)
            {
                Debug.Write("Can't get remote object\nDetails:"+ex);
            }
            
            if(this.managerObject==null)
                Debug.VerboseWrite("Can't instantiate manager end");
            else
                this.SendRequest(RemoteCommandActions.Hello, null);
        }
        
        /// <summary>
        /// Ends the comunication with the manager
        /// </summary>
        private void UnregisterManagerEnd()
        {
            if(this.managerObject!=null)
            {
                this.SendRequest(RemoteCommandActions.Bye, null);
                this.managerObject = null;
            }
        }

	}
}
