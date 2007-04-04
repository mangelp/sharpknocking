
using System;

namespace SharpKnocking.Common.Remoting 
{
	public class RemoteDaemon: RemoteEnd, IRemoteDaemon
	{
		private bool valid;
		
		public bool Valid
		{
		    get { return this.valid; }
		}
		
		
		public RemoteDaemon(bool valid)
		  :base(RemoteEndService.DaemonServiceName)
		{
		    this.valid = valid;
		}
	}
}
