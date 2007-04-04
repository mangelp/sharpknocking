
using System;
using System.IO;
using Mono.Unix.Native;

    
namespace SharpKnocking.Common
{
	
	/// <summary>
    /// Native methods that only will work under *nix systems
    /// </summary>
	public static class UnixNative
	{
	    /// <summary>
	    /// Lock file name;
	    /// </summary>
        public static readonly string LockFile = "/tmp/SharpKnocking.lock";
        
        /// <summary>
        /// Creates a new lock file.
        /// </summary>
        /// <returns>
        /// True if it was created or false if not
        /// </returns>
		public static bool CreateLockFile()
        {
            FileStream fs = File.Create(LockFile);
            
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
        public static bool ExistsLockFile()
        {
            return File.Exists(LockFile);
        }
        
        /// <summary>
        /// Removes the lock file
        /// </summary>
        /// <returns>
        /// True if there was a lock file and it is removed and false if the file
        /// doesn't exist or if it can be removed.
        /// </returns>
        public static bool RemoveLockFile()
        {
            if(!File.Exists(LockFile))
                return false;
            
            try
            {
                File.Delete(LockFile);
            }
            catch(Exception ex)
            {
                Debug.Write ("Can't remove lock file. Reason: "+ex.Message +"\n ** Details **\n"+ex);
                return false;
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
            
            string name = "/tmp/SharpKnocking."+r.Next()+"tmp";
            
            while(File.Exists(name))
            {
                name = "/tmp/SharpKnocking."+r.Next()+"tmp";
            }
            
            return name;
        }
	}
}
