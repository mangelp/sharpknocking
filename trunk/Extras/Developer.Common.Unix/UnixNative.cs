
using System;
using System.IO;
using Mono.Unix.Native;

    
namespace Developer.Common.Native.Unix
{
	
	/// <summary>
    /// Native methods that only will work under *nix systems
    /// </summary>
	public static class UnixNative
	{        
		private static string baseLockPath="/tmp";
		private static string baseTmpPath = "/tmp";
		
        /// <summary>
        /// Creates a new lock file.
        /// </summary>
        /// <returns>
        /// True if it was created or false if not
        /// </returns>
		public static bool CreateLockFile(string baseName)
        {
            FileStream fs = File.Create(baseLockPath+baseName+".lock");
            
            if(fs!=null)
            {
                fs.Close();
                return true;
            }
            else
                return false;
        }
        
        /// <summary>
        /// Checks if the lock file exists
        /// </summary>
        public static bool ExistsLockFile(string baseName)
        {
            return File.Exists(baseLockPath+baseName+".lock");
        }
        
        /// <summary>
        /// Removes the lock file
        /// </summary>
        /// <returns>
        /// True if there was a lock file and it is removed and false if the file
        /// doesn't exist or if it can be removed.
        /// </returns>
        public static bool RemoveLockFile(string baseName)
        {
			string fName = baseLockPath+baseName+".lock";
            if(!File.Exists(fName))
                return false;
            
            try
            {
                File.Delete(fName);
            }
            catch(Exception ex)
            {
                throw new Exception("Can't remove lock file "+fName+". Reason: "+ex.Message,ex);
            }
            
            return true;
        }
        
        /// <summary>
        /// Detect if current user has root privileges
        /// </summary>
        public static bool ExecUserIsRoot()
        {
            return Mono.Unix.UnixEnvironment.EffectiveUserId == 0;
        }
        
        /// <summary>
        /// Creates a temporary file name
        /// </summary>
        /// <returns>
        /// A temporary name for a file that doesn't exist
        /// </returns>
        public static string CreateTempFileName()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            
            string name = String.Empty;
			
			do
			{
				name = UnixNative.baseTmpPath+"/a"+r.Next()+"b"+r.Next()+"c"+r.Next()+".tmp";
			}
            while(File.Exists(name));
            
            return name;
        }
        
		/// <summary>
		/// Set a handler for Ctrl-C signal (SIGINT).
		/// </summary>
		/// <remarks>
		/// If it can't handle the signal launches an exception
		/// </remarks>
        public static void HandleCtrlCSignal(SignalHandler usrHandler)
        {
            SignalHandler handler = Mono.Unix.Native.Stdlib.signal(Signum.SIGINT , usrHandler);
            
            if(handler == Stdlib.SIG_ERR)
                throw new Exception("Error handling SIGINT: "+Stdlib.GetLastError());
        }
        
		/// <summary>
		/// Set a handler for SIGTERM signal.
		/// </summary>
		/// <remarks>
		/// If it can't handle the signal launches an exception
		/// </remarks>
        public static void HandleTermSignal(SignalHandler usrHandler)
        {
            SignalHandler handler = Mono.Unix.Native.Stdlib.signal(Signum.SIGTERM, usrHandler);
            
            if(handler == Stdlib.SIG_ERR)
                throw new Exception("Error handling SIGTERM: "+Stdlib.GetLastError());
        }
	}
}
