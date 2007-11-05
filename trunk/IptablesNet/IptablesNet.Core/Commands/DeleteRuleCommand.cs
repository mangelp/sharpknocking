// DeleteRuleCommand.cs
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
	/// Models the delete rule command
	/// </summary>
	public class DeleteRuleCommand: GenericCommand
	{
		//Default value of -1
	    private int ruleNum = -1;
	    
		/// <summary>
		/// Rule number to delete
		/// </summary>
		/// <remarks>
		/// Set this property to a value less than 1 to disable it.
		/// </remarks>
	    public int RuleNum
	    {
	        get { return this.ruleNum;}
	        set
	        {
	            if(value<1)
	                throw new ArgumentException("The number must be greater or equal to 1!");
	            
	            this.ruleNum = value;
	        }
	    }
	    
		/// <summary>
		/// Gets if the rule must be specified. It happens when the rule number is
		/// less than 1
		/// </summary>
	    public override bool MustSpecifyRule
	    {
	        get
	        {
	            if(this.ruleNum>=1)
	                return false;
	            else
	                return true;
	        }
	    }
		
		/// <summary>
		/// Default constructor
		/// </summary>
		public DeleteRuleCommand()
		  :base(RuleCommands.DeleteRule)
		{
		    ruleNum = -1;
		}
		
		/// <summary>
		/// Returns the value of the command parameter as a string. 
		/// </summary>
		protected override string GetValuesAsString()
		{
			if(this.ruleNum>=1)
				return ruleNum.ToString();
			else
				return String.Empty;
		}
	}
}
