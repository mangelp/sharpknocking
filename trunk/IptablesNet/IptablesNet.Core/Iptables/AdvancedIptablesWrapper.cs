// AdvancedIptablesWrapper.cs
//
//  Copyright (C) 2007 iSharpKnocking project
//  Created by mangelp<@>gmail[*]com
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
//
//

using System;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;

using IptablesNet.Core;
using IptablesNet.Core.Util;
using IptablesNet.Core.Commands;

using Developer.Common.Unix.Native;

namespace IptablesNet.Core.Iptables
{
	
	public class AdvancedIptablesWrapper: IIptablesAdapter
	{
		SystemProcess iptProc;
	
		public AdvancedIptablesWrapper()
			:base()
		{
			iptProc = SystemProcess.GetNewProcess("/bin/iptables", "", 12, 128);
		}

		public IIptablesTransaction CreateTransaction()
		{
			return new CommandTransaction(this);
		}

		public Void SetCurrentRuleSet(NetfilterTableSet ruleSet)
		{
			string tmpFile = UnixNative.CreateTempFileName();
			try{
				ruleSet.SaveToFile(tmpFile, true);
				SimpleIptablesWrapper.Restore(tmpFile);
			}catch(Exception ex){
				throw new InvalidOperationException("Can't restore from ruleset", ex);
			}finally{
				if(File.Exists(tmpFile))
					File.Delete(tmpFile);
			}
		}

		public NetfilterTableSet GetCurrentRuleSet()
		{
			string tmpFile = UnixNative.CreateTempFileName();
			NetfilterTableSet nrs = null;
			try{
				SimpleIptablesWrapper.Save(tmpFile);
				nrs = new NetfilterTableSet();
				nrs.LoadFromFile(tmpFile);
			}catch(Exception ex){
				throw new InvalidOperationException("Can't load current ruleset", ex);
			}finally{
				if(File.Exists(tmpFile))
					File.Delete(tmpFile);
			}
			return nrs;
		}

		public void Exec (IIptablesCommand cmd)
		{
			SimpleIptablesWrapper.Exec(cmd.Command);
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
			
			private AdvancedIptablesWrapper owner;
			
			/// <summary>
			/// Inits a new instance of the transaction
			/// </summary>
			public CommandTransaction(AdvancedIptablesWrapper owner)
			{
				this.owner = owner;
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
				string tmpFileName = UnixNative.CreateTempFileName();
				SimpleIptablesWrapper.Save(tmpFileName);
				bool hasToRestore = false;
				
				try
				{
					using (SystemProcess sp = SystemProcess.GetNewProcess ("iptables-save",String.Empty, 12, 128))
					{
						//TODO: 
						for(int i=0;i<this.commands.Count;i++)
						{
							sp.WriteToProcess(this.commands[i].ToString()+"\n");
							if(sp.HasErrors)
								throw new InvalidOperationException ("Error While executing commands: \n"+sp.GetErrors());
						}
						hasToRestore = true;
						sp.WriteToProcess("COMMIT\n");
						if(sp.HasErrors)
							throw new InvalidOperationException ("Error While executing commands: \n"+sp.GetErrors());
					}
					
					this.status = TransactionStatus.Commited;
				}
				catch(Exception)
				{
					this.status = TransactionStatus.Aborted;
					if(hasToRestore)
						SimpleIptablesWrapper.Restore (tmpFileName);
				}
				finally
				{
					File.Delete(tmpFileName);
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
}
