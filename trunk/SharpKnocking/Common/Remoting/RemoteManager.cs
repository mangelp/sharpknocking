
using System;

namespace SharpKnocking.Common.Remoting
{
	public class RemoteManager: RemoteEnd
	{
		public RemoteManager()
		  :base(RemoteEndService.ManagerServiceName)
		{
		}
	}
}
