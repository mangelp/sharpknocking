
using System;
using System.Runtime.Serialization;

namespace SharpKnocking.Common.Remoting
{
    public delegate void RemoteEndEventHandler(object obj, RemoteEndEventArgs args);  
    
    /// <summary>
    /// Argument modeling for <c>RemoteEndEventHandler</c> event handler.
    /// </summary>
	public class RemoteEndEventArgs: EventArgs
	{
        private bool isRequestNotResponse;
        
        /// <summary>
        /// Returns true if this is a request.
        /// </summary>
        public bool IsRequest
        {
            get { return this.isRequestNotResponse;}
        }
        
        /// <summary>
        /// Returns true if this is a response.
        /// </summary>
        public bool IsResponse
        {
            get { return !this.isRequestNotResponse;}
        }
        
        private object data;
        
        /// <summary>
        /// Data sent
        /// </summary>
        public object Data
        {
            get { return this.data; }
        }
        
        private RemoteCommandActions action;
        
        /// <summary>
        /// Action for the request or action to wich this reponse belongs
        /// </summary>
        public RemoteCommandActions Action
        {
            get { return action; }
        }
        
        /// <summary>
        /// Parametrized Constructor. Initialices all fields in the argument
        /// </summary>
        /// <param name="data"> Data for the request or response</param>
        /// <param name="action"> Action for request or action that caused the response</param>
        /// <param name="isRequestNotResponse"> If true this argument is for a request but
        /// if it is false this argument is for a response</param>
        public RemoteEndEventArgs(object data, RemoteCommandActions action, bool isRequestNotResponse)
            :base()
        {
            this.isRequestNotResponse = isRequestNotResponse;
            this.action = action;
            this.data = data;
        }
                                  
        
	}
}
