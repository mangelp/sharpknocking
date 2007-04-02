
using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

using SharpKnocking.Common;
using SharpKnocking.Common.Remoting;

namespace SharpKnocking.Doorman.Remoting
{
	/// <summary>
	/// Models the operations required to send and receive messages to and from
	/// the daemon.
	/// </summary>
	public class DaemonCommunication: IDisposable
	{
	    /// <summary>
	    /// Current status for the comunication.
	    /// </summary>
	    public string Status
	    {
	       get { return this.status;}
	    }
	    
	    /// <summary>
	    /// Checks if the connection with the daemon is valid
	    /// </summary>
	    public bool IsConnected
	    {
	       get { return this.isConnected;}
	    }
	    
	    //Flag to see if we requested hello
		private bool helloReq;
		//Flag to see if we requested bye
		private bool byeReq;
		//Flag to see if we requested interactive mode.
		private bool interactiveModeReq;
		//Flag to see if we requested stop
		private bool stopReq;
		//Flag to see if we requested start
		private bool startReq;
		//Flag to see if we requested hotRestart
		private bool hotRestartReq;
		private bool statusReq;
		private RemoteManager myEnd;
		private ObjRef myEndRef;
		private RemoteDaemon remoteObj;
		private TcpChannel myChannel;
		
		private bool isConnected;
		private string status;
		
		/// <summary>
		/// Remote object used for comunication
		/// </summary>
		public RemoteDaemon Remote
		{
			get { return this.remoteObj;}
		}
         
        /// <summary>
        /// Default constructor.
        /// </summary>   
		public DaemonCommunication()
		{

		}
		
        /// <summary>
        /// End comunication.
        /// </summary>
        public void Dispose()
        {
            this.UnregisterDaemonEnd();
        
        	this.myEnd = null;
        	
        	if(this.myEndRef != null)
        	{
        	     RemotingServices.Unmarshal(this.myEndRef);
        	     this.myEndRef = null;
        	}
        	
        	if(this.myChannel!=null)
        	{
        	   ChannelServices.UnregisterChannel(this.myChannel);
        	   this.myChannel = null;
        	}
        }
		
		/// <summary>
		/// Finishes the comunication with the daemon
		/// </summary>
        public void UnregisterDaemonEnd()
        {	
        	if(this.remoteObj != null)
        	{
        		this.remoteObj.SendRequest(RemoteCommandActions.Bye, null);
        		this.remoteObj = null;
        	}
        }
        
        /// <summary>
        /// Starts the comunication with the daemon
        /// </summary>
        public void Init()
        {
            Debug.VerboseWrite("DaemonCommunication: Registering remoting object");

            this.myChannel = new TcpChannel(RemoteEndService.ManagerPortNumber);
            
            ChannelServices.RegisterChannel(this.myChannel);
            
            this.myEnd = new RemoteManager();
            //Set as a remoting object to comunicate
            this.myEndRef = RemotingServices.Marshal(myEnd, RemoteEndService.ManagerServiceName);
            
			//Set a event handler to get notifications of incoming messages.
            this.myEnd.Received += new RemoteEndEventHandler(this.IncomingMessage_event);
            
            this.RegisterDaemonEnd();
        }
        
        /// <summary>
        /// Sends a command to the other end of the comunication
        /// </summary>
        /// <returns>
        /// True if the command is sent and false if not.
        /// </returns>
        public bool SendCommand(RemoteCommandActions action)
        {
            if(this.remoteObj == null)
		    {
		        this.RegisterDaemonEnd();
		    }
		    
		    if(this.remoteObj == null)
		    {
		        Debug.Write("Can't connect to daemon");
		        return false;
		    }
                
            switch(action)
            {
                case RemoteCommandActions.Hello:
                case RemoteCommandActions.Bye:
                    throw new InvalidOperationException("Action not allowed: "+action);
                    break;
                case RemoteCommandActions.HotRestart:
                    this.hotRestartReq = true;
                    this.status = "Requesting daemon hot restart";
                    this.SendRequest(action, null);
                    break;
                case RemoteCommandActions.Start:
                    this.startReq = true;
                    this.status = "Requesting daemon start";
                    this.SendRequest(action, null);
                    break;
                case RemoteCommandActions.Stop:
                    this.startReq = true;
                    this.status = "Requesting daemon stop";
                    this.SendRequest(action, null);
                    break;
                case RemoteCommandActions.Status:
                case RemoteCommandActions.StatusExtended:
                    this.statusReq = true;
                    this.status = "Requesting daemon status";
                    this.SendRequest(action, null);
                    break;
                default:
                    this.SendRequest(action, null);
                    break;
            }
            
            return true;
        }
        
        /// <summary>
        /// Tries to return a new remote end object from the daemon to perform
        /// the calls for comunication
        /// </summary>
        /// <remarks>
        /// This method is called every time that a request is sent and a 
        /// </remarks>
        private bool RegisterDaemonEnd()
        {           
            Debug.VerboseWrite("DaemonCommunication: Trying to register remote end");

        	//Gets the object from the daemon part
        	if(this.remoteObj == null)
        	{
        	    try
        	    {
        	        string uri = "tcp://localhost:"+
        	                     RemoteEndService.DaemonPortNumber+
        	                     "/"+RemoteEndService.DaemonServiceName;
        	                     
        	        Debug.VerboseWrite("DaemonCommunication: Uri="+uri);
        	                     
	                this.remoteObj = (RemoteDaemon) Activator.GetObject(
	                                               typeof(RemoteDaemon), 
	                                               uri);
	                                               
                    //Set up the communication channel
	               if(this.remoteObj != null)
	               {
	                   Debug.VerboseWrite("Remote object got "+this.remoteObj.Name );
	            	  //Say hello to the daemon so he can create the required object
	            	  //to stay in touch.
	            	  this.helloReq = true;
	            	  Debug.VerboseWrite("Sending request to remote object");
	                  remoteObj.SendRequest(RemoteCommandActions.Hello, null);
	                  Debug.VerboseWrite("Remote object request sent");
	                  return true;
	               }
                }
                catch(RemotingException ex)
                {
                    
                    if(!UnixNative.ExistsLockFile())
                        this.status = "Remote daemon not running";
                    else
                        this.status = "Can't connect with remote daemon. Connection refused";
                        
                    Debug.Write("DaemonCommunication: "+this.status+".\nDetails:"+ex);
                }
        	}
        	
        	return false;
        }
			
	    //Handler for incoming messages (requests and responses).
		private void IncomingMessage_event(object sender, RemoteEndEventArgs args)
		{
			if(args.IsRequest)
			{
				this.ProcessRequest (args.Action, args.Data);
			}
			else 
			{
				this.ProcessResponse (args.Action , args.Data );
			}
		}
		
		//Request procesing. Only we spect events from the daemon with notifications
		//of access attemps
		private void ProcessRequest(RemoteCommandActions action, object data)
		{
			switch(action)
			{
				case RemoteCommandActions.Event:
					//This happens only when the daemon is not in interactive mode.
					//It sends continuously the sequence
					//TODO:
					break;
				case RemoteCommandActions.Bye:
				    //The daemon has ended by unknown causes. End the comunication.
				    this.UnregisterDaemonEnd();
				    break;
			}
		}
		
	    //Response procesing. The responses that will be received are:
	    //- Hello/bye
	    //- Status/StatusExtended	
	    //- Start/Stop/Hotrestart
		private void ProcessResponse(RemoteCommandActions action, object data)
		{
			switch(action)
			{
				case RemoteCommandActions.Hello:
					//Daemon up and running
					if(this.helloReq)
					{
					   this.helloReq = false;
					   this.isConnected = true;
					}
					break;
				case RemoteCommandActions.Bye:
					//End of processing with daemon
					if(this.byeReq)
					{
					    this.byeReq = false;
					    this.isConnected = false;
					    this.UnregisterDaemonEnd ();
					}
					break;
				case RemoteCommandActions.Status:
				case RemoteCommandActions.StatusExtended:
					//Status
					break;
				case RemoteCommandActions.Start:
				case RemoteCommandActions.Stop:
				case RemoteCommandActions.HotRestart:
				    if(data is bool && ((bool)data))
				    {
				        this.status = "Finished "+action+" operation successfully";
				        this.startReq = false;
				        this.stopReq = false;
				        this.hotRestartReq = false;
				    }
				    break;
				case RemoteCommandActions.EndInteractiveMode:
				    if(this.interactiveModeReq)
				    {
				        this.interactiveModeReq = false;
				    }
				    break;
			}
		}
		
		private void SendRequest(RemoteCommandActions action, object data)
		{
		    if(this.remoteObj == null)
		    {
		        this.RegisterDaemonEnd();
		    }
		    
		    if(this.remoteObj == null)
		    {
		        Debug.Write("Can't connect to daemon");
		        return;
		    }
		    
		    Debug.VerboseWrite("Sending Request "+action);
		    this.remoteObj.SendRequest( action, data);
		}
		
		private void SendResponse(RemoteCommandActions action, object data)
		{
		    if(this.remoteObj == null)
		    {
		        this.RegisterDaemonEnd();
		    }
		    
		    if(this.remoteObj == null)
		      throw new Exception ("Can't get daemon object to send response!");
		    
		    Debug.VerboseWrite("Sending Response "+action);
		    this.remoteObj.SendRequest( action, data);
		}
	}
}
