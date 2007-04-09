
using System;

namespace SharpKnocking.Common.Remoting
{
	/// <summary>
    /// Static definitions
    /// </summary>
	public static class RemoteEndService
	{
        private static int daemonPortNumber;
        
        /// <summary>
        /// Daemon port number
        /// </summary>
		public static int DaemonPortNumber
        {
            get { return daemonPortNumber; }
        }
        
        private static int managerPortNumber;
        
        /// <summary>
        /// Manager port number
        /// </summary>
        public static int ManagerPortNumber
        {
            get {return managerPortNumber;}
        }
        
        private static string daemonServiceName;
        
        /// <summary>
        /// Daemon service name
        /// </summary>
        public static string DaemonServiceName 
        {
            get {return daemonServiceName;}
        }
        
        private static string managerServiceName;
        
        /// <summary>
        /// Manager service name
        /// </summary>
        public static string ManagerServiceName 
        {
            get {return managerServiceName;}
        }        
        
		static RemoteEndService()
		{
            RemoteEndService.daemonPortNumber = 29566;
            RemoteEndService.managerPortNumber = 29567;
            RemoteEndService.daemonServiceName = "Daemon";
            RemoteEndService.managerServiceName = "Manager";
		}
	}
}
