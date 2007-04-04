
using System;
using System.Runtime.Remoting;

namespace SharpKnocking.Common.Remoting
{
	
	/// <summary>
	/// Arguments for <c>RemoteCommandEventHandler</c> event handler.
	/// </summary>
	public class RemoteCommandEventArgs: EventArgs
	{
	    /// <summary>
	    /// Action performed
	    /// </summary>
        public RemoteCommandActions Action;
        
        /// <summary>
        /// Data related to the action. It can be null.
        /// </summary>
        public object Data;
        
        /// <summary>
        /// 
        public object Response;
		
		public RemoteCommandEventArgs()
            :base()
		{
            this.Data = null;
            this.Response = null;
            this.Action = RemoteCommandActions.Hello;
		}
        
	}
}
