
using System;

namespace SharpKnocking.Common.Remoting 
{
	public class RemoteDaemon: RemoteEnd
	{
		public RemoteDaemon()
		  :base(RemoteEndService.DaemonServiceName)
		{
		}
	}
}
