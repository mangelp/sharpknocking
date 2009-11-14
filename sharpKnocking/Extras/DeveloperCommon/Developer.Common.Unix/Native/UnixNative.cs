// UnixNative.cs
//
//  Copyright (C)  2007 iSharpKnocking project
//  Created by Miguel Angel Perez, mangelp{at}gmail{_DOT_}com
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA 

using System;
using System.IO;

using Developer.Common.Types;

using Mono.Unix;
using Mono.Unix.Native;
    
namespace Developer.Common.Unix.Native
{
	
	/// <summary>
    /// Native methods that only will work under *nix systems
    /// </summary>
	public static class UnixNative
	{        
		private static string baseLockPath="/tmp/lock";
		private static string baseTmpPath = "/tmp";
		private static object objLock = new object();
		
        /// <summary>
        /// Creates a new lock file.
        /// </summary>
        /// <returns>
        /// True if it was created or false if not
        /// </returns>
		public static bool CreateLockFile(string baseName)
        {
        	return CreateLockFile(baseName, Int32.MaxValue);
        }
		
     	/// <summary>
        /// Creates a new lock file.
        /// </summary>
        /// <returns>
        /// True if it was created or false if not
        /// </returns>
		public static bool CreateLockFile(string baseName, int pid)
        {
			baseName = baseLockPath + baseName + ".lock";
			bool result = false;
			lock(objLock) {
				if (!File.Exists(baseName)) {
            		FileStream fs = File.Create(baseName);
					if (fs != null) {
						if (pid != Int32.MaxValue) {
							byte[] data = Conversion.ToByteArray(pid);
							fs.Write(data, 0, data.Length);
						}
		                fs.Close();
						result = true;
					}
				}
			}
			
			return result;
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
            return UnixEnvironment.EffectiveUserId == 0;
        }
		
		public static string GetCurrentUser()
		{
			return UnixEnvironment.EffectiveUser.UserName;
		}
		
		public static bool IsCurrentUser(string userName)
		{
			return String.Equals(userName,
			                     UnixEnvironment.EffectiveUser.UserName);
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
		[Obsolete("Use new Mono.Unix.UnixSignal logic instead")]
        public static void HandleCtrlCSignal(SignalHandler usrHandler)
        {
			//TODO: Change this
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
		[Obsolete("Use new Mono.Unix.UnixSignal logic instead")]
        public static void HandleTermSignal(SignalHandler usrHandler)
        {
			//TODO: Change this
            SignalHandler handler = Mono.Unix.Native.Stdlib.signal(Signum.SIGTERM, usrHandler);
            
            if(handler == Stdlib.SIG_ERR)
                throw new Exception("Error handling SIGTERM: "+Stdlib.GetLastError());
        }
	}
}
