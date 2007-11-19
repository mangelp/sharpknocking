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

namespace Developer.Common.Debug
{
	public enum LogLevel
    {
		Low=1,
        Normal=2,
        High=3,
        Insane=4
    }
	
	public enum LogCategory: short
	{
	    Warning=0, //Warning messages
	    Error=1, //Application crashes information
	    Information=2, //Keep the user informed about things
	    Debug=3 //Debug things. Prints everything to output target. 
	}
	
	public enum OutputTarget: short
	{
	    File=0, //Log to file
	    Console=1 //Log to console
	}
}
