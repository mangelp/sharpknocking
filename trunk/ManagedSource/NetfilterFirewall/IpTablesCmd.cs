
using System;

using Mono.Unix.Native;

using SharpKnocking.Common;

namespace SharpKnocking.NetfilterFirewall
{
    /// <summary>
    /// This class encapsulates all the logic needed to manage rules throught
    /// the real command line iptables utility.
    /// </summary>
	public static class IpTablesCmd
	{
        private static string ipTablesCommand;
        
        /// <summary>
        /// Gets/Sets the path to the iptables binary. The iptables-save and
        /// iptables-restore commands are supossed to be in the same directory
        /// as iptables command.
        /// </summary>
        public static string IpTablesCommand
        {
            get { return IpTablesCmd.ipTablesCommand;}
            set { IpTablesCmd.ipTablesCommand = value;}
        }
        
        /// <summary>
        /// Static constructor. Inits the iptables binary path to a common
        /// default value.
        /// </summary>
        static IpTablesCmd()
        {
            ipTablesCommand= WhichWrapper.Search("iptables");
        }
        
        /// <summary>
        /// Stores the current rule set to a file using the same format as
        /// iptables-save output.
        /// </summary>
        public static int Save(string fileName)
        {   
            string cmd = ""+ipTablesCommand+"-save > "+fileName;
            int ret = Mono.Unix.Native.Syscall.system(cmd);
            
            if(ret!=0)
                throw new InvalidOperationException("The command '"+cmd+"' returned "+ret);
            
            Debug.VerboseWrite("Save to file '"+fileName+"': success");
            
            return ret;
        }
        
        /// <summary>
        /// Loads the current rule set from a file that have the same format as 
        /// iptables-save output.
        /// </summary>
        public static int Restore(string fileName)
        {
            string cmd = ""+ipTablesCommand+"-restore "+fileName;
            int ret = Mono.Unix.Native.Syscall.system(cmd);
            
            if(ret!=0)
                throw new InvalidOperationException("The command '"+cmd+"' returned "+ret);
            
            Debug.VerboseWrite("Restore from file '"+fileName+"': success");
            
            return ret;
        }
        
        /// <summary>
        /// Executes the iptables command using the arguments passed.
        /// </summary>
        public static int Exec(params string[] args)
        {
            string cmd = ""+ipTablesCommand;
            
            for(int i=0;i<args.Length; i++)
            {
                cmd+=" "+args[i];
            }
            
            Debug.VerboseWrite("IpTablesCmd: Executing action: "+cmd, 
                               VerbosityLevels.Insane);
            
            int ret = Mono.Unix.Native.Syscall.system(cmd);
            
            if(ret!=0)
            {
                throw new InvalidOperationException("Can't exec command '"+cmd+"' result: "+ret);
            }
            
            Debug.VerboseWrite("IpTablesCmd: Operation success: "+cmd);
            
            return ret;
        }
        
        /// <summary>
        /// Executes the iptables command using the argument passed.
        /// </summary>
        public static int Exec(string cmdArgs)
        {
            cmdArgs = ""+ipTablesCommand+" "+cmdArgs;
            int ret = Mono.Unix.Native.Syscall.system(cmdArgs);
            
            if(ret!=0)
            {
                throw new InvalidOperationException("Can't exec command '"+
                                                    cmdArgs+"' result: "+ret);
            }
            
            Debug.VerboseWrite("IpTablesCmd: Operation success: "+cmdArgs);
            
            return ret;
        }
	}
}
