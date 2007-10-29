
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using IptablesNet.Core;
using IptablesNet.Core.Commands;

using Developer.Common.Unix.Native;
using Developer.Common.SystemCommands;
using Developer.Common.Unix.SystemCommands;

namespace IptablesNet.Core.Iptables
{
    /// <summary>
    /// This class encapsulates all the logic needed to manage rules throught
    /// the real command line iptables utility.
    /// </summary>
	public class SimpleIptablesWrapper: IIptablesAdapter
	{
		/* ------------------------------------------------------------ 
         * Static stuff 
		 * ------------------------------------------------------------*/
		
        private static string ipTablesCommand;
        
        /// <summary>
        /// Gets/Sets the path to the iptables binary. The iptables-save and
        /// iptables-restore commands are supossed to be in the same directory
        /// as iptables command.
        /// </summary>
        public static string IpTablesCommand
        {
            get { return ipTablesCommand;}
            set { ipTablesCommand = value;}
        }
        
        /// <summary>
        /// Static constructor. Inits the iptables binary path to a common
        /// default value.
        /// </summary>
        static SimpleIptablesWrapper()
        {
			WhichSysCmd which = new WhichSysCmd();
			which.Args="iptables";
			CommandResult cres = which.Exec();
			List<string> result = (List<string>)cres.UserData;
			if(result.Count>0)
				ipTablesCommand = result[0];
        }
        
        /// <summary>
        /// Stores the current rule set to a file using the same format as
        /// iptables-save output.
        /// </summary>
        public static int Save(string fileName)
        {
            string cmd = String.Format("{0}-save > {1}",ipTablesCommand, fileName);
            int ret = Mono.Unix.Native.Syscall.system(cmd);
            
            if(ret!=0)
            {
                throw new InvalidOperationException("The command '"+cmd+"' returned "+ret+". Check it out.");
            }
            
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
            
            int ret = Mono.Unix.Native.Syscall.system(cmd);
            
            if(ret!=0)
            {
                throw new InvalidOperationException("Can't exec command '"+cmd+"' result: "+ret);
            }
            
            return ret;
        }
		
		/* ------------------------------------------------------------ 
         * Concrete stuff from the instance 
		 * ------------------------------------------------------------*/
		
		public SimpleIptablesWrapper ()
		{}

        public virtual IIptablesTransaction CreateTransaction()
        {
			CommandTransaction cmdt = new CommandTransaction(this);
			return cmdt;
        }

        public Void SetCurrentRuleSet(NetfilterTableSet ruleSet)
        {
			string fileName = UnixNative.CreateTempFileName();
			ruleSet.SaveToFile(fileName, true);
			SimpleIptablesWrapper.Restore(fileName);
			File.Delete(fileName);
        }

        public NetfilterTableSet GetCurrentRuleSet()
        {
			NetfilterTableSet rs = new NetfilterTableSet();
			string fileName = UnixNative.CreateTempFileName();
			SimpleIptablesWrapper.Save(fileName);
			rs.LoadFromFile(fileName);
			File.Delete(fileName);
			return rs;
        }

        public Void Exec(IIptablesCommand cmd)
        {
			SimpleIptablesWrapper.Exec(cmd.Command);
        }
		
	}
	
	internal class CommandTransaction: IIptablesTransaction
	{
		private TransactionStatus status;
		
		/// <summary>
		/// Get the status of the transaction
		/// </summary>
		public TransactionStatus Status 
		{
			get{ return this.status;}
		}
		
		private List<IIptablesCommand> commands;
		
		/// <summary>
		/// Gets the list of commands in the transaction.
		/// </summary>
		public IIptablesCommand[] Commands 
		{
			get{
				return this.commands.ToArray();
			}
		}
		
		private SimpleIptablesWrapper cmd;
					
		public CommandTransaction(SimpleIptablesWrapper cmd)
		{
			this.cmd = cmd;
			this.commands = new List<IIptablesCommand>();
			this.status = TransactionStatus.Active;
		}
		
		/// <summary>
		/// Adds a new command into the transaction
		/// </summary>
		public void Add(IIptablesCommand cmd)
		{
			this.commands.Add(cmd);
		}
		
		/// <summary>
		/// Applies the full set of commands
		/// </summary>
		public void Commit()
		{
			//We need a backup, to restore the rule set, that will
			//be erased at the end.
			string bkFile = UnixNative.CreateTempFileName();
			SimpleIptablesWrapper.Save(bkFile);
			try
			{
				//Exec the commands one by one, can be slow but its
				//the simpler way of doing this.
				for(int i=0;i<this.commands.Count;i++)
					this.cmd.Exec(this.commands[i]);
				this.status = TransactionStatus.Commited;
			}
			catch(Exception)
			{
				this.status = TransactionStatus.Aborted;
				SimpleIptablesWrapper.Restore(bkFile);
			}
			finally
			{
				File.Delete(bkFile);
			}
		}
		
		/// <summary>
		/// Aborts the transaction. Undoes all changes.
		/// </summary>
		public void Abort()
		{
			this.status = TransactionStatus.Aborted;
			this.commands.Clear();
		}
	}
}
