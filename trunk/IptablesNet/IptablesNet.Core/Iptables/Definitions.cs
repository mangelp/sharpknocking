// /home/mangelp/Projects/sharpknocking/IptablesNet/IptablesNet.Core/Iptables/Definitions.cs created with MonoDevelop at 15:53 23/05/2007 by mangelp 
//
//This project is released under the terms of the LGPL V2. See the file lgpl.txt for details.
//(c) 2007 SharpKnocking projects and authors (see AUTHORS).

using System;

namespace IptablesNet.Core
{
	public enum TransactionStatus
	{
		/// <summary>
		/// The transaction was commited successfully
		/// </summary>
		Commited,
		/// <summary>
		/// The transaction was aborted.
		/// </summary>
		Aborted,
		/// <summary>
		/// The transaction is still active, was not aborted nor commited.
		/// </summary>
		Active
	}
}
