
using System;

namespace SharpKnocking.Common.Remoting
{
	public delegate void RemotingCommunicatorEventHandler ( object sender, 
	                           RemotingCommunicatorEventArgs args);
	
	public class RemotingCommunicatorEventArgs: EventArgs
	{
        private RemoteCommandActions action;
        private object data;
        
        public RemoteCommandActions Action
        { 
            get { return this.action ;}
        }
        
        public object Data
        { 
            get { return this.data ;}
        }
        
        public RemotingCommunicatorEventArgs(RemoteCommandActions action, 
                    object data)
        {
            this.action = action;
            this.data = data;
        }
	}
}
