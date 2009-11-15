// InInterfaceOption.cs
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

using NFSharp.Iptables;

namespace NFSharp.Iptables.Options
{
	
	/// <summary>
	/// input interface option
	/// </summary>
	public class InInterfaceOption: GenericOption
	{
	    private string iface;
	    
	    public string Interface
	    {
	        get { return this.iface;}
	        set 
            { 
                this.iface = value;
            }
	    }

		public InInterfaceOption()
		  :base(RuleOptions.InInterface)
		{
		}
		
		public override bool TryReadValues (string strVal, out string errStr)
		{
		    if(!String.IsNullOrEmpty(strVal))
		    {
		        this.iface = strVal;
		        errStr=String.Empty;
		        return true;
		    }
		    
		    errStr = "The value can't be null or empty";
		    return false;
		}

		protected override string GetValueAsString()
		{
			return this.iface;
		}

	}
}
