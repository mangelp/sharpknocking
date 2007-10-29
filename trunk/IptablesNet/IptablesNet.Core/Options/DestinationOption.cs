// DestinationOption.cs
//
//  Copyright (C) 2006 SharpKnocking project
//  Created by Miguel Angel Pérez, mangelp@gmail.com
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
using System;

using IptablesNet.Core;

using Developer.Common.Net;

namespace IptablesNet.Core.Options
{
	
	/// <summary>
	/// Destination address option
	/// </summary>
	public class DestinationOption: GenericOption
	{
	    private SkIpAddress address;
	    
	    /// <summary>
	    /// Destination address range to match
	    /// </summary>
	    public SkIpAddress Address
	    {
	        get { return this.address;}
	        set 
            { 
                this.address = value;
            }
	    }
		
		public DestinationOption()
		  :base(RuleOptions.Destination)
		{
		      
		}
		
		public override bool TryReadValues (string strVal, out string errorStr)
		{
		    SkIpAddress range;
		    errorStr = String.Empty;
		    
		    if(SkIpAddress.TryParse(strVal, out range))
		    {
		        this.address = range;
		        return true;
		    }

	        errorStr = "Can't convert from string '"+strVal+"' to "+
	                   "object IpAddressRange";
	        return false;
		}
		
		protected override string GetValuesAsString()
		{
			return this.address.ToString();
		}

	}
}
