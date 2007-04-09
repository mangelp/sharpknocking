
using System;

namespace SharpKnocking.Common.Remoting
{
	public interface IRemoteCommand
	{
        /// <summary>
        /// Returns an enumeration with the status of the server.
        /// </summary>
        /// <remarks>
        /// This call is an alias to ExecCommand(1) passing RemoteCommandActions.Status
        /// as the parameter.
        /// </remarks>
        RemoteServerStatus GetStatus();
        
        /// <summary>
        /// Returns an string with a description of the current status of the
        /// server.
        /// </summary>
        /// <remarks>
        /// This call is an alias to ExecCommand(1) passing RemoteCommandActions.StatusExtended
        /// as the parameter.
        /// </remarks>
        string GetStatusDetail();
        
        /// <summary>
        /// Executes a simple action command in the server.
        /// </summary>
        /// <param name="action"> Action to perform</param>
        /// <returns>
        /// Null if the command was not successfull or an object with the response
        /// if not. The response can be a serializable object or a value.
        /// </returns>        
        object ExecCommand(RemoteCommandActions action);
        
        /// <summary>
        /// Executes a action command in the server.
        /// </summary>
        /// <param name="cmdData">Data about the command to execute</param>
        /// <param name="action">Action to perform</param>
        /// <returns>
        /// Null if the command was not successfull or an object with the response
        /// if not. The response can be a serializable object or a value.
        /// </returns>        
        object ExecCommand(RemoteCommandActions action, object cmdData);
        
        /// <summary>
        /// This event occurs when a command is issued remotely and must be
        /// listen by server-side code.
        /// </summary>
        event RemoteCommandEventHandler CommandExecuted;
        
        ServerResponseDelegate GetServerResponse{get;}
    }
}
