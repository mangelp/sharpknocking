
using System;
using System.Runtime.Remoting;

namespace SharpKnocking.Common.Remoting
{
    /// <summary>
    /// Models a object to comunicate between two points.
    /// </summary>
	public class RemoteEnd: MarshalByRefObject
	{    
        private bool valid;
        
        public bool Valid
        {
            get { return this.valid;}
            set { this.valid = value;}
        }

        private string name;
        
        /// <summary>
        /// Return name.
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { this.name = value;}
        }
        
        /// <summary>
        /// Event that occurs when a message is received (request or response)
        /// </summary>
        public event RemoteEndEventHandler Received;
        
        public RemoteEnd(string name)
        {
            this.name = name;
        }
        
        public override object GetLifetimeService ()
        {
            //Return null to prevent this object from expiring.
        	return null;
        }

        public override object InitializeLifetimeService ()
        {
            //Return null to prevent this object from expiring.
        	return null;
        }
        
        /// <summary>
        /// Sends a request for the object that implements this interface.
        /// </summary>
        public void SendRequest(RemoteCommandActions action, object data)
        {
            RemoteEndEventArgs args = new RemoteEndEventArgs(data, action,true);
            this.OnReceived(args);
        }
        
        /// <summary>
        /// Sends a request for the object that implements this interface.
        /// </summary>
        public void SendResponse(RemoteCommandActions action, object data)
        {
            RemoteEndEventArgs args = new RemoteEndEventArgs(data, action, false);
            this.OnReceived(args);
        }
        
        /// <summary>
        /// Notify about message received. Real object owner should handle notifications
        /// to see messages that others send to him.
        /// </summary>
        protected virtual void OnReceived(RemoteEndEventArgs args)
        {
            if(this.Received!=null)
                this.Received(this, args);
        }
	}
}
