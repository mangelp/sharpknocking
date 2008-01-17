// ReplaceRuleCommand.cs
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

namespace IptablesNet.Core.Commands
{
	/// <summary>
	/// Models the replace rule command
	/// </summary>
	public class ReplaceRuleCommand: GenericCommand
	{
	    private int ruleNum;
	    
		/// <summary>
		/// Gets/Sets the rule number to replace
		/// </summary>
	    public int RuleNum
	    {
	        get
	        {
	            return this.ruleNum;
	        }
	        set
	        {
	            if(value<1)
	                throw new ArgumentException("The rules number must be greater"+
	                                                             " or equal than 1");
	            
	            this.ruleNum = value;
	        }
	    }
	    
		/// <summary>
		/// Constructor
		/// </summary>
		public ReplaceRuleCommand()
		  :base(RuleCommands.ReplaceRule)
		{
		    this.ruleNum = 1;
		}
		
		/// <summary>
		/// Gets an string that represents the command
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/>
		/// </returns>
		protected override string GetValueAsString ()
		{
			return this.ruleNum.ToString();
		}

	}
}
