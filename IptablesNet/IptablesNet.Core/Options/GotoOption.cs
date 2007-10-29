// GotoOption.cs
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

namespace IptablesNet.Core.Options
{
	
	/// <summary>
	/// Goto option
	/// </summary>
	public class GotoOption: GenericOption
	{
	    private string chainName;
	    
	    public string ChainName
	    {
	        get{ return this.chainName;}
	        set
	        {
	            if(String.IsNullOrEmpty(value))
	                throw new ArgumentException("The value can't be null or empty");
	            
	            this.chainName = value;
	        }
	    }
	    
		public GotoOption()
		  :base(RuleOptions.Goto)
		{
		}
		
		public override bool TryReadValues(string strVal, out string errStr)
		{
		    if(String.IsNullOrEmpty(strVal))
		    {
		        errStr = "The input string is null or empty";
		        return false;
		    }
		    
		    this.chainName = strVal;
		    errStr=String.Empty;
		    return true;
		}
		
		protected override string GetValuesAsString()
		{
			return this.chainName;
		}

	}
}
