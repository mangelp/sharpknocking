
using System;

using System.Runtime.Remoting;

namespace SharpKnocking.Common.Remoting
{
    /// <summary>
    /// Operations for a object that allows to receive messages from a remote
    /// process.
    /// </summary>
	public interface IRemoteEnd
	{
        /// <summary>
        /// Sends a request for the object that implements this interface.
        /// </summary>
        void SendRequest(RemoteCommandActions action, object data);
        
        /// <summary>
        /// Sends a request for the object that implements this interface.
        /// </summary>
        void SendResponse(RemoteCommandActions action, object data);
        
        /// <summary>
        /// Event that occurs when a message is received (request or response)
        /// </summary>
        /// <remarks>
        /// In one side the remote process uses the <c>SendRequest</c> methods
        /// to send messages but in the side of the object's owner it should handle
        /// this event to get notifications of the messages sent to him.
        /// </remarks>
        event RemoteEndEventHandler Received;
	}
}
