
using System;

namespace SharpKnocking.Common.Remoting 
{
	public class RemoteDaemon: RemoteEnd, IRemoteDaemon
	{
		
		public RemoteDaemon()
		  :base(RemoteEndService.DaemonServiceName)
		{
		}
	}
}
