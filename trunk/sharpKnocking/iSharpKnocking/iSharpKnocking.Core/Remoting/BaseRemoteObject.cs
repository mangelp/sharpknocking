
using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;

using SharpKnocking.Common;

namespace SharpKnocking.Common.Remoting
{
	
	/// <summary>
    /// Remote object base implementation.
    /// </summary>
    /// <remarks>
    /// Implements a remoting object that doesn't expire.
    /// </remarks>
	public abstract class BaseRemoteObject: MarshalByRefObject, IRemoteCommand
	{

        public event RemoteCommandEventHandler CommandExecuted;
        
        private ServerResponseDelegate getServerResponse;
        
        public virtual ServerResponseDelegate GetServerResponse
        {
            get { return this.getServerResponse;}
            set { this.getServerResponse = value;}
        }
        
        public BaseRemoteObject()
            :base()
        {}
        
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
        
        protected virtual void OnCommandExecuted(RemoteCommandEventArgs args)
        {
            if(this.CommandExecuted!=null)
            {
                this.CommandExecuted(this, args);
            }
        }

        
        public RemoteServerStatus GetStatus()
        {
            object response = this.ExecCommand(
                                       RemoteCommandActions.Status, null);
            
            if(response!=null && response is RemoteServerStatus)
                return (RemoteServerStatus)response;
            else
                return RemoteServerStatus.Unknown;
        }
        
        public string GetStatusDetail()
        {
            object response = this.ExecCommand(
                                      RemoteCommandActions.StatusExtended, null);
            
            if(response!=null && response is string)
                return (string)response;
            else
                return String.Empty;
        }
        
        public object ExecCommand(RemoteCommandActions action)
        {
            return this.ExecCommand(action, null);
        }
        
        public virtual object ExecCommand (
                                     RemoteCommandActions action, object cmdData)
        {
        	RemoteCommandEventArgs args = new RemoteCommandEventArgs();
            args.Action = RemoteCommandActions.StatusExtended;
            args.Data = cmdData;
            
            this.OnCommandExecuted(args);
            
            return this.getServerResponse(args); 
        }

	}
}
