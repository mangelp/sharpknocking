// Definitions.cs
//
//  Copyright (C)  2007 iSharpKnocking project
//  Created by Miguel Angel Perez Valencia, mangelp@gmail.com
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

namespace Developer.Common.Debuging
{
	/// <summary>
	/// Level of detail for the debugging output
	/// </summary>
	/// <remarks>
	/// This mainly affects what categgories are shown in each level.
	/// </remarks>
	public enum DebugLevel
    {
		/// <summary>
		/// Show nothing
		/// </summary>
		None=0,
		/// <summary>
		/// Show only very critical messages 
		/// </summary>
		Low=1,
		/// <summary>
		/// Show critical messages, critical warnings and informative messages
		/// </summary>
        Medium=2,
		/// <summary>
		/// Show critical messages, critical warnings, informative messages and
		/// high-level debug information.
		/// </summary>
        High=3,
		/// <summary>
		/// Show everything that happens inside the app
		/// </summary>
        Insane=4
    }
	
	/// <summary>
	/// Logging categories to mark the debug messages.
	/// </summary>
	/// <remarks>
	/// The categories determine what are the visibility of the messages
	/// </remarks>
	public enum LogCategory: short
	{
		/// <summary>
		/// Warning level. Shown in medium level and up.
		/// </summary>
		Warning=0,
		/// <summary>
		/// Critical warning level. Shown in low level and up.
		/// </summary>
		CriticalWarning=1,
		/// <summary>
		/// Error level. Shown in medium level and up.
		/// </summary>
		Error=2,
		/// <summary>
		/// Critical error level. Shown in low level and up.
		/// </summary>
		CriticalError=3,
		/// <summary>
		/// Information level. Shown in the medium level and up.
		/// </summary>
		Information=4,
		/// <summary>
		/// Debuggin information level. Shown in the high level and up.
		/// </summary>
		Debug=5,
		/// <summary>
		/// Trace level. Shown in the highest level.
		/// </summary>
		Trace=6
	}
	
	/// <summary>
	/// Target to send the logging information
	/// </summary>
	public enum OutputTarget: short
	{
		/// <summary>
		/// Log to file
		/// </summary>
	    File=0,
		/// <summary>
		/// Log to console
		/// </summary>
	    Console=1,
		/// <summary>
		/// Send messages to an stream
		/// </summary>
		Stream=2,
		/// <summary>
		/// Send messages to a string property of a control
		/// </summary>
		StringProperty,
	}
}
