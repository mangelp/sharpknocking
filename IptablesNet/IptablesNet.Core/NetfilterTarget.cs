// NetfilterTarget.cs
//
//  Copyright (C) 2007 iSharpKnocking project
//  Created by Miguel Angel Perez (mangelp{@}gmail{d0t}com)
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

using Developer.Common.Types;

namespace IptablesNet.Core
{
	
	/// <summary>
	/// Models a target for rules or parameters
	/// </summary>
	/// <remarks>
	/// The kind of things that can be allowed in a target depends in the
	/// place where the target is used.<br/>
	/// - Built in chain.<br/>
	/// - User defined chain.<br/>
	/// - Target extension <br/>
	/// </remarks>
	public class NetfilterTarget
	{
	    private string targetName;
	    
	    public string TargetName
	    {
	        get { return this.targetName;}
	        set { this.targetName = value;}
	    }
	    
		public NetfilterTarget(string targetName)
		{
		}
		
		public static bool IsBuiltInTarget(string name)
		{
		    object obj;
		    
		    if(AliasUtil.IsAliasName(typeof(RuleTargets), name, out obj))
		        return true;
		    
		    return false;
		}
		
		public static bool IsBuiltInChain(string name)
		{
		    object obj;
		    
		    if(AliasUtil.IsAliasName(typeof(BuiltInChains), name, out obj))
		        return true;
		    
		    return false;
		}
	}
}
