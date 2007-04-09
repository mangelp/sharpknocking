// created on 17/03/2007 at 20:13
using System;

namespace SharpKnocking.Common.Remoting
{

    /// <summary>
    /// Actions with a remote object
    /// </summary>
    public enum RemoteCommandActions
    {
        //Accept access
        Accept,
        //Incoming sequence
        AccessRequest,
        //Disconnect for the other end
        Bye,
        //Deny access
        Deny,
        // Kill the process
        Die,
        //Notify something
        Event,
        //Stop interactive mode
        EndInteractiveMode,
        //Connect for the other end
        Hello,
        //Hot restart ;)
        HotRestart,
        //Make the end start to work
        Start,
        //Start interactive mode
        StartInteractiveMode,
        //Get the end status
        Status,
        //Get the end status in a extended form
        StatusExtended,
        //Stop the end
        Stop
    }
    
    /// <summary>
    /// Estatus for the remote server.
    /// </summary>
    public enum RemoteServerStatus
    {
        /// <summary>
        /// Server started.
        /// </summary>
        Started,
        /// <summary>
        /// Server running in interactive mode.
        /// </summary>
        StartedInteractiveMode,
        /// <summary>
        /// Server stopped.
        /// </summary>
        Stopped,
        /// <summary>
        /// Don't know
        /// </summary>
        Unknown
    } 
}