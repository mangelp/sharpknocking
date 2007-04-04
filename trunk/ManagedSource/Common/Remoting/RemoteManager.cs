
using System;

namespace SharpKnocking.Common.Remoting
{
	public class RemoteManager: RemoteEnd, IRemoteManager
	{
		private bool valid;
		
		public bool Valid
		{
		    get { return this.valid; }
		}
		
		public RemoteManager(bool valid)
		  :base(RemoteEndService.ManagerServiceName)
		{
		    this.valid = valid;
		}
	}
}
