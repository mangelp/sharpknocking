
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
		#region Attributes
		
		private RemotingCommunicator communicator;
		
		#endregion Attributes	
		
		#region Events
		
		public event EventHandler Hello;
		
		public event AccessRequestEventHandler AccessRequest;
		
		#endregion Events
	
		/// <summary>
        /// Default constructor.
        /// </summary>   
		public DaemonCommunication()
		{
            this.communicator = new RemotingCommunicator(typeof(RemoteManager), 
                                                         typeof(RemoteDaemon));
		}
	
		#region Properties
		
		/// <summary>
	    /// Checks if the connection with the daemon is valid
	    /// </summary>
	    public bool IsConnected
	    {
	       get { return this.communicator.Connected ;}
	    }
	    
	    #endregion Properties
		
		#region Public methods       
        
		
        /// <summary>
        /// Free resource
        /// </summary>
        public void Dispose()
        {

        }
		
		/// <summary>
        /// Starts the comunication with the daemon.
        /// </summary>
        public void Init()
        {
            Debug.VerboseWrite("DaemonCommunication::Init()");

            this.communicator.Init();
        }
		
		
		
		 /// <summary>
        /// Sends a command to the other end of the comunication.
        /// </summary>
        /// <returns>
        /// True if the command is sent and false if not.
        /// </returns>
        public bool SendCommand(RemoteCommandActions action)
        {                
            switch(action)
            {
                case RemoteCommandActions.Hello:
                case RemoteCommandActions.Start:
                    throw new InvalidOperationException("Action not allowed: "+action);
                case RemoteCommandActions.HotRestart:
                    this.communicator.SendRequest(action, null);
                    break;
                case RemoteCommandActions.Stop:
                    this.communicator.SendRequest(action, null);
                    this.communicator.UnregisterRemoteEnd();
                    break;
                case RemoteCommandActions.Status:
                case RemoteCommandActions.StatusExtended:
                    this.communicator.SendRequest(action, null);
                    break;
                default:
                    this.communicator.SendRequest(action, null);
                    break;
            }
            
            return true;
        }
        
        #endregion Public methods
        
        #region Private methods
			
		private void OnHelloSender()
		{
			if(Hello != null)
				Hello(this,EventArgs.Empty);
		}
		
		private void OnAccessRequestSender(string data)
		{
			// We take the xml data and then send the event.
			if(AccessRequest != null)
			{
				AccessRequest(this, new AccessRequestEventArgs(data));
			}			
			
		}
		
		// Request procesing. Only we spect events from the daemon with notifications
		// of access attemps
		private void OnRequestHandler(RemoteCommandActions action, object data)
		{
			switch(action)
			{
				case RemoteCommandActions.AccessRequest:
				
					// In this case, data is a string with the xml serialization
					// of a CallSequence object and the source ip address.							
					OnAccessRequestSender(data as String);					
					
					break;
				case RemoteCommandActions.Bye:
				    //The daemon has ended by unknown causes. End the comunication.
				    this.communicator.UnregisterRemoteEnd();
				    break;
			}
		}
		
	    // Response procesing. The responses that will be received are:
	    // - Hello/bye
	    // - Status/StatusExtended	
	    // - Start/Stop/Hotrestart
		private void OnResponseHandler(RemoteCommandActions action, object data)
		{
			switch(action)
			{
				case RemoteCommandActions.Hello:					
					OnHelloSender();
					break;
				case RemoteCommandActions.Bye:
					this.communicator.UnregisterRemoteEnd();
					break;
				case RemoteCommandActions.Status:
				case RemoteCommandActions.StatusExtended:
					break;
				case RemoteCommandActions.Start:
				case RemoteCommandActions.Stop:
				case RemoteCommandActions.HotRestart:
				    break;
				case RemoteCommandActions.EndInteractiveMode:
				    break;
			}
		}
		
		#endregion Private methods
	}
}
