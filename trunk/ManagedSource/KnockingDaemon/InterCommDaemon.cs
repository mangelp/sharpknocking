
using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

using SharpKnocking.Common;
using SharpKnocking.Common.Remoting;
using SharpKnocking.NetfilterFirewall;
using SharpKnocking.KnockingDaemon.PacketFilter;

namespace SharpKnocking.KnockingDaemon
{
	
    /// <summary>
    /// Daemon class that manages the communication of the knockingDaemon
    /// with other applications that requires information about the current status
    /// or that want to change the daemon implementation.
    /// </summary>
    /// <remarks>
    /// This class is not really a daemon itself. It runs in the main thread started
    /// with the daemon and coordinates the other two "daemon" classes that run
    /// in separate threads (one for each one). So our Knocking daemon is really
    /// a process with 3 daemons executing each one in his own thread.
    /// </remarks>
	public class InterCommDaemon: IDisposable
	{
        private TcpdumpMonitor capDaemon;
        
        /// <summary>
        /// Gets/Sets a reference to the daemon that capture packets and generate
        /// objects with sequence information.
        /// </summary>
        public TcpdumpMonitor CapDaemon
        {
            get { return capDaemon;}
            set { this.capDaemon = value;}
        }
        
        private NetfilterDaemon netDaemon;
        
        /// <summary>
        /// Gets/sets a reference to the daemon that manages netfilter rules,
        /// receives sequence information and issues the rules that grant access
        /// to remote users.
        /// </summary>
        public NetfilterDaemon NetDaemon
        {
            get { return netDaemon;}
            set { netDaemon = value;}
        }
        
        private TcpChannel tcpChannel;
        
        private RemoteDaemon commObject;
        private ObjRef commObjectRef;

        private RemoteManager managerObject;
        private bool isInteractiveMode;
        private bool isStarted;
		
		public InterCommDaemon()
		{
		}
        
        /// <summary>
        /// Runs the comunication daemon
        /// </summary>
        public void Run()
        {
            //We need to register our remoting object and try to reach the
            //one from the manager
            this.RegisterServerObject();
            this.RegisterManagerEnd();
            this.isStarted = true;
        }
        
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
                    //TODO: Do hot restart :B
                    this.SendResponse(args.Action, null);
                    break;
                case RemoteCommandActions.Start:
                    //TODO: Start the daemon parts
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
                    //TODO: Stop the other two daemon parts
                    this.isStarted = false;
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
