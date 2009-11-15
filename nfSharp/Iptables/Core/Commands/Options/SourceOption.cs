// SourceOption.cs
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
using System.Net;

using NFSharp.Iptables;

using CommonUtilities.Net;

namespace NFSharp.Iptables.Core.Commands.Options
{
	
	/// <summary>
	/// Destination address option
	/// </summary>
	public class SourceOption: GenericOption
	{
	    private IPAddress address;
	    
	    /// <summary>
	    /// Source address range to match
	    /// </summary>
	    public IPAddress Address
	    {
	        get { return this.address;}
	        set 
            { 
                this.address = value;
            }
	    }
		
		public SourceOption()
		  :base(RuleOptions.Source)
		{
		}

		/// <summary>
		/// Read values for this option from an input string
		/// </summary>
		/// <param name="strVal">
		/// A <see cref="System.String"/> with the values to extract
		/// </param>
		/// <param name="errorStr">
		/// A <see cref="System.String"/> with the error description
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/> true if a value was read or false if not
		/// </returns>
	    public override bool TryReadValues (string strVal, out string errorStr)
		{
		    IPAddress range;
		    errorStr = String.Empty;
		    
		    if(IPAddress.TryParse(strVal, out range))
		    {
		        this.address = range;
		        return true;
		    }

	        errorStr = "Can't convert from string '"+strVal+"' to "+
	                   "object IPAddressRange";
	        return false;
		}
		
		protected override string GetValueAsString()
		{
			return this.address.ToString ();
		}
	}
}
