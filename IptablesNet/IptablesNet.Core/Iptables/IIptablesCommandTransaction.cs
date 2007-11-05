// IIptablesCommandTransaction.cs
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
