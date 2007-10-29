// /home/mangelp/Projects/sharpknocking/IptablesNet/IptablesNet.Core/Iptables/IFirewallCommandSession.cs created with MonoDevelop at 14:34 23/05/2007 by mangelp 
//
//This project is released under the terms of the LGPL V2. See the file lgpl.txt for details.
//(c) 2007 SharpKnocking projects and authors (see AUTHORS).

using System;
using System.Collections;
using System.Collections.Generic;

using IptablesNet.Core;
using IptablesNet.Core.Commands;

namespace IptablesNet.Core.Iptables
{
	/// <summary>
	/// Operations for generic transaction commands.
	/// </summary>
	public interface IIptablesTransaction
	{
		/// <summary>
		/// Get the status of the transaction
		/// </summary>
		TransactionStatus Status {get;}
		
		/// <summary>
		/// Gets an array of commands in the transaction.
		/// </summary>
		IIptablesCommand[] Commands {get;}
		
		/// <summary>
		/// Adds a new command into the transaction
		/// </summary>
		void Add(IIptablesCommand cmd);
		
		/// <summary>
		/// Applies the full set of commands
		/// </summary>
		void Commit();
		
		/// <summary>
		/// Aborts the transaction. Undoes all changes.
		/// </summary>
		void Abort();
	}
}
