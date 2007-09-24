// SharpKnocking/SharpKnocking.Core/Calls/Knocks.cs
//
//  Copyright (C) 2007 Luis Román Gutiérrez y Miguel Ángel Pérez Valencia
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

using Developer.Common.Net;

namespace SharpKnocking.Core.Knocking
{
	/// <summary>
	/// Models all the data required for a knock.
	/// </summary>
	public struct Knock
	{
		/// <summary>
		/// Protocol type of the packet
		/// </summary>
		public ProtocolType Proto;
		
		/// <summary>
		/// Number of sequence for the packet.
		/// </summary>
		/// <remarks>
		/// Sequence numbers should not be consecutive, they must be 
		/// generated randomly.
		/// </remarks>
		public int NumSeq;
		
		/// <summary>
		/// Id of the user who is sending the knock
		/// </summary>
		/// <remarks>
		/// This id has nothing to do with the id of the user in the system.
		/// The same user can use different knock sequences with different
		/// user ids.
		/// </remarks>
		public short UsId;
		
		/// <summary>
		/// Gets if the UsId is valid.
		/// </summary>
		public bool HasUsId;
		
		/// <summary>
		/// Private key code of the user for the sequence to which this knock
		/// belongs.
		/// </summary>
		/// <remarks>
		/// This key code is required (along with the user id) to identify the 
		/// sequence to which this knock belongs.
		/// </remarks>
		public short UsPk;
		
		/// <summary>
		/// Gets if the UsPk is valid.
		/// </summary>
		public bool HasUsPk;
		
		/// <summary>
		/// Data associated with the knock
		/// </summary>
		/// <remarks>
		/// Each nock can have data or not and that data can be a valid data or 
		/// a random-generated set of bytes.
		/// The data can be encrypted using a public key from the server and/or
		/// signed by the user.
		/// </remarks>
		public byte[] Data;
		
		/// <summary>
		/// Gets if there is defined a valid random knock space.
		/// </summary>
		public bool HasRandomKnock;
		
		/// <summary>
		/// Random Knocks specification. Determines how to randomize certain
		/// parts of the knock sequence.
		/// </summary>
		/// <remarks>
		/// The existence of a valid value indicates that after the send/receive
		/// of this knock a random sequence follows. The end of the sequence is
		/// detected by the number of sequence. For the random sequence must be
		/// in a range starting from the sequence of the caller and ending before
		/// a certain value is reached.
		/// </remarks>
		public RandomKnock RandomKnock;
	}
	
	public struct RandomKnock
	{
		/// <summary>
		/// Maximum number of random knocks
		/// </summary>
		public byte Max;
		
		/// <summary>
		/// Minimum number of random knocks
		/// </summary>
		public byte Min;
		
		/// <summary>
		/// Minimum number of random bytes
		/// </summary>
		public short MinRandomBytes;
		
		/// <summary>
		/// Maximum number of random bytes
		/// </summary>
		public short MaxRandomBytes;
	}
}
