
using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

using SharpKnocking.Common;

namespace SharpKnocking.Common.Remoting 
{
	public class RemotingCommunicator: IDisposable
	{
		private RemoteEnd localEnd;
		private RemoteEnd remoteEnd;
		private ObjRef localEndRef;
		private TcpChannel channel;
		
		private bool connected;
		private bool disposed;
		
		private Type localType;
		private Type remoteType;
		
		private int localPort;
		private int remotePort;
		
		private string localName;
		private string remoteName;
		
		private string remoteUri;
        private string localUri;
		
		public event RemotingCommunicatorEventHandler RequestReceived;
		public event RemotingCommunicatorEventHandler ResponseReceived;
		
		
		/// <summary>
	    /// Checks if the connection with the daemon is valid
	    /// </summary>
	    public bool Connected
	    {
	       get { return this.connected;}
	    }
	    
	    public bool Disposed
	    {
	       get { return this.disposed; }
	    }
	    
        /// <summary>
        /// Uri of the remote end to connect
        /// </summary>
	    public string RemoteUri
	    {
	       get { return this.remoteUri;}
	    }
	    
        public string LocalUri
        {
            get { return this.localUri;}
        }
        
	    public int LocalPort
	    {
	       get {return this.localPort ;}
	       set 
	       {
	           this.localPort = value;
               this.localUri = "tcp://localhost:"+this.localPort+"/"+this.localName;
	       }
	    }
	    
	    public string LocalName
	    {
	       get { return this.localName;}
	       set 
	       {
	           this.localName = value;
               this.localUri = "tcp://localhost:"+this.localPort+"/"+this.localName;
	       }
	    }
	    
	    public int RemotePort
	    {
	       get {return this.remotePort ;}
	       set 
	       {
	           this.remotePort = value;
	           this.remoteUri = "tcp://localhost:"+this.remotePort+"/"+this.RemoteName;
	       }
	    }
	    
	    public string RemoteName
	    {
	       get { return this.remoteName;}
	       set 
	       {
	           this.remoteName = value;
	           this.remoteUri = "tcp://localhost:"+this.remotePort+"/"+this.RemoteName;
	       }
	    }
	    
		/// <summary>
        /// Default constructor.
        /// </summary>   
		public RemotingCommunicator(Type localType, Type remoteType)
		{
            this.localType = localType;
            this.remoteType = remoteType;
		}
	    
        /// <summary>
        /// Free up things.
        /// </summary>
        public void Dispose()
        {
            this.UnregisterRemoteEnd();
        
        	this.localEnd = null;
        	
        	if(this.localEndRef != null)
        	{
        	     RemotingServices.Unmarshal(this.localEndRef);
        	     this.localEndRef = null;
        	}
        	
        	if(this.channel!=null)
        	{
        	   ChannelServices.UnregisterChannel(this.channel);
        	   this.channel = null;
        	}
        }
		
		/// <summary>
        /// Starts the comunication with the daemon.
        /// </summary>
        public void Init()
        {
            if(this.localPort == 0 || this.remotePort == 0 || 
                Net20.StringIsNullOrEmpty(this.localName ) ||
                Net20.StringIsNullOrEmpty (this.remoteName ))
                    throw new ArgumentException ("Must initialize all the properties", 
                                "LocalPort, LocalName, RemotePort, RemoteName");
                                
            Debug.VerboseWrite("RemoteEndCommunicator: Registering remoting object with uri " +this.localUri);
			
            this.channel = new TcpChannel(this.localPort);
            
            ChannelServices.RegisterChannel(this.channel);
            
            this.localEnd = (RemoteEnd)Activator.CreateInstance (this.localType);
            this.localEnd.Valid = true;
            //Set as a remoting object to comunicate
            this.localEndRef = 
            	RemotingServices.Marshal(this.localEnd , this.localName);
            
			//Set a event handler to get notifications of incoming messages.
            this.localEnd.Received += new RemoteEndEventHandler(this.OnIncomingMessage);
        }
			
	    // Handler for incoming messages (requests and responses).
		private void OnIncomingMessage(object sender, RemoteEndEventArgs args)
		{
			if(args.IsRequest)
			{
				this.OnRequestReceived(args.Action, args.Data);
			}
			else 
			{
				this.OnResponseReceived(args.Action, args.Data);
			}
		}
		
		// Request procesing. Only we spect events from the daemon with notifications
		// of access attemps
		private void OnRequestReceived(RemoteCommandActions action, object data)
		{
		    Debug.VerboseWrite("RemotingCommunicator:: Incomming request: Action: "+
		              action+", Data: '"+data+"'");
		    
		    if(this.RequestReceived !=null)
		    {
		        RemotingCommunicatorEventArgs args = 
		            new RemotingCommunicatorEventArgs(action, data);
		            
		        this.RequestReceived (this, args);
		    }		    
		}
		
		// Request procesing. Only we spect events from the daemon with notifications
		// of access attemps
		private void OnResponseReceived(RemoteCommandActions action, object data)
		{
		    Debug.VerboseWrite("RemotingCommunicator:: Incomming response: Action: "+
		              action+", Data: '"+data+"'");
		
		    if(this.ResponseReceived !=null)
		    {
		        RemotingCommunicatorEventArgs args = 
		            new RemotingCommunicatorEventArgs(action, data);
		            
		        this.ResponseReceived (this, args);
		    }		    
		}
		
		/// <summary>
		/// Finishes the comunication with the daemon.
		/// </summary>
        public void UnregisterRemoteEnd()
        {	
        	if(this.remoteEnd != null)
        	{
        		this.remoteEnd = null;
        		this.connected  = false;
        	}
        }
		
		/// <summary>
        /// Tries to return a new remote end object from the daemon to perform
        /// the calls for comunication.
        /// </summary>
        /// <remarks>
        /// This method is called every time that a request is sent and aswered. 
        /// </remarks>
        private bool RegisterRemoteEnd()
        {           
            Debug.VerboseWrite("RemotingCommunicator:: Trying to register remote end");

        	//Gets the object from the daemon part
        	if(this.remoteEnd == null)
        	{
        	    this.connected  = false;
        	    
        	    try
        	    {
        	        Debug.VerboseWrite ("Trying to connect to "+this.remoteType+
                                        " in "+this.remoteName+":"+this.remotePort);
	                this.remoteEnd = (RemoteEnd) Activator.GetObject(
	                                               this.remoteType, 
	                                               this.remoteUri);
	                                               
                   	// Set up the communication channel
	               	if(this.remoteEnd != null)
	               	{
	             		Debug.VerboseWrite("RemotingCommunicator:: Remote object got "+
	             		        this.remoteEnd.Name );
	             		this.connected = true;
	               	}
	               	else if(!this.remoteEnd.Valid)
	               	{
	               	    //The object must be marked as valid or we shouldn't 
	               	    //want it
	               	    this.remoteEnd = null;
	               	}
                }
                catch(RemotingException ex)
                {
                    Debug.VerboseWrite(
                    	"RemotingCommunicator:: Problem while trying to get the remote end",
                    	VerbosityLevels.Insane);
                    Debug.VerboseWrite(
                    	"RemotingCommunicator:: Details: \n"+ex,
                    	VerbosityLevels.Insane);
                }
        	}
        	
        	return this.connected;
        }
		
		public void SendRequest(RemoteCommandActions action, object data)
		{
		    if(this.remoteEnd == null)
		    {
		        this.RegisterRemoteEnd ();
		    }
		    
		    if(this.remoteEnd == null)
		    {
		        Debug.VerboseWrite("RemotingCommunicator:: Can't connect to remote end"
		                  +". Request aborted.");
		        return;
		    }
		    
		    try
		    {
    		    Debug.VerboseWrite("RemotingCommunicator:: Sending Request "+action);
    		    this.remoteEnd.SendRequest( action, data);
		    }
		    catch(Exception ex)
		    {
		        Debug.VerboseWrite("RemotingCommunicator:: Can't send request. "+
		                  "Unregistering end.\nDetails:\n"+ex);
		        this.UnregisterRemoteEnd ();
		    }
		}
		
		public void SendResponse(RemoteCommandActions action, object data)
		{
		    if(this.remoteEnd == null)
		    {
		        Debug.VerboseWrite("RemotingCommunicator:: WARNING:"+
		                  " Tried to send a response but there was no remote"+
		                  " end registered. Registering now.");
		        this.RegisterRemoteEnd ();
		    }
		    
		    if(this.remoteEnd == null)
		    {
		        Debug.VerboseWrite("RemotingCommunicator:: Can't connect to remote end"+
		                  ". Response aborted.");
		        return;
		    }
		    
		    try
		    {
    		    Debug.VerboseWrite("RemotingCommunicator:: Sending Response "+action);
    		    this.remoteEnd.SendResponse ( action, data);
		    }
		    catch(Exception ex)
		    {
		        Debug.VerboseWrite("RemotingCommunicator:: Can't send response. "+
		                  "Unregistering end.\nDetails:\n"+ex);
		        this.UnregisterRemoteEnd ();
		    }
		}

	}
}
