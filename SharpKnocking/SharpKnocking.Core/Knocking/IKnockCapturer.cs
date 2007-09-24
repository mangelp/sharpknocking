// IKnockCapturer.cs
//
//  Copyright (C) SharpKnocking Project 2007
//  Author: mangelp[on]gmail*com
//  For a list of contributors see AUTHORS
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

namespace SharpKnocking.Core.Knocking
{
	
	
	public interface IKnockCapturer
	{
			/// <summary>
		/// Name of the capturer.
		/// </summary>
		string Name{get;}
		
		/// <summary>
		/// Short description about the capturer
		/// </summary>
		string Description{get;}
		
		/// <summary>
		/// Number of packets to hold before rising the event to notify others
		/// about the capture.
		/// </summary>
		/// <remarks>
		/// </remarks>
		int CaptureBufferSize{get;}
		
		/// <summary>
		/// Checks if the capture process is running or not.
		/// </summary>
		/// <remarks>
		/// This flag doen't guarantee that the capture process is completely
		/// stopped and the resources where freed. It only guaranatees that 
		/// the capturer will not capture nothing when its value is false and
		/// that the capturer will produce captures if its value is true (only
		/// if there is something to capture in the interface).
		/// </remarks>
		bool CaptureStarted{get;}
		
		/// <summary>
		/// Gets if the buffer for the capture is auto-sized when no more packets
		/// can get into it.
		/// </summary>
		/// <remarks>
		/// </remarks>
		bool IncreaseBufferOnOverflow{get;set;}
		
		/// <summary>
		/// Sets the reuse of the buffer
		/// </summary>
		/// <remarks>
		/// When this flag is true the buffer will be recycled when it is not
		/// emptied fast enought. There is, the newly incoming packets will
		/// replace existing ones.
		/// </remarks>
		bool ReuseBuffer{get;}
		
		/// <summary>
		/// Starts a new asynchronous capture process.
		/// </summary>
		/// <remarks>
		/// Every time that a packet is captured an event named PacketCaptured
		/// notifies about it.
		/// </remarks>
		void StartCapture();
		
		/// <summary>
		/// Finishes the asynchronous capture process.
		/// </summary>
		/// <remarks>
		/// </remarks>
		void EndCapture();
		
		/// <summary>
		/// Notifies about a new captured packet.
		/// </summary>
		/// <remarks>
		/// The argument carries an array with the elements captured. Those 
		/// elements are removed from the buffer.
		/// </remarks>
		event EventHandler<PacketCapturedEventArgs> Captured;
	}
}
