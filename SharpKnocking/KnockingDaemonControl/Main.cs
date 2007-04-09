// project created on 17/03/2007 at 20:45
using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

using SharpKnocking.Common;
using SharpKnocking.Common.Remoting;

namespace KnockingDaemonControl
{
	class MainClass
	{
		public static void Main(string[] args)
		{
            
//            TcpChannel channel = new TcpChannel();
//            ChannelServices.RegisterChannel(channel);
//            
//            IRemoteCommand cmd = 
//                (IRemoteCommand)Activator.GetObject(typeof(IRemoteCommand), 
//                                "tcp://localhost:29566/KnockingDaemonControl");
//            
//            Console.WriteLine("KnockingDaemonControl: Asking for daemon status ...");
//            
//            string status = cmd.GetStatusDetail();
//            
//            Console.WriteLine("KnockingDaemonControl: Daemon status response follow:\n"+status);
            UnixNative.ExecUserIsRoot ();
		}
	}
}