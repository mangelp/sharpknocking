
using System;

namespace SharpKnocking.Common.Remoting
{
	public class RemoteManager: RemoteEnd, IRemoteManager
	{
		
		public RemoteManager()
		  :base(RemoteEndService.ManagerServiceName)
		{
		}
	}
}
