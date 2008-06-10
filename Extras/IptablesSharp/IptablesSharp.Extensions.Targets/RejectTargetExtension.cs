// RejectTargetExtension.cs
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

using Developer.Common.Types;

using IptablesSharp.Core.Extensions;
using IptablesSharp.Core.Extensions.ExtendedTarget;

namespace IptablesSharp.Extensions.Targets
{
    /// <summary>
    /// Models the Reject target extension
    /// </summary>
	public class RejectTargetExtension: TargetExtensionHandler
	{
		
		public RejectTargetExtension()
		  :base(typeof(RejectTargetOptions), TargetExtensions.Reject)
		{
		}
        
        public override TargetExtensionParameter CreateParameter (string paramType)
        {
            object obj=null;
            
            if(!AliasUtil.IsAliasName (typeof (RejectTargetOptions), paramType, out obj))
                return null;
            
            RejectTargetOptions option = (RejectTargetOptions)obj;
            
            switch(option)
            {
                case RejectTargetOptions.RejectWith:
                    return new RejectParam(this);
                default:
                    throw new ArgumentException ("Not supported option: "+option,"name");
            }
        }
		
		public class RejectParam: TargetExtensionParameter
		{
		    private RejectIcmpTypes rejectWith;
		    
		    /// <summary>
		    /// Icmp message type to return when a packet is rejected
		    /// </summary>
		    public RejectIcmpTypes RejectWith
		    {
		        get { return this.rejectWith;}
		        set { this.rejectWith = value;}
		    }
		    
		    public new RejectTargetOptions Option
		    {
		        get { return (RejectTargetOptions)base.Option;}
		    }
		    
		    public new RejectTargetExtension Owner
		    {
		        get { return (RejectTargetExtension)base.Owner;}
		    }
		    
		    public RejectParam(RejectTargetExtension owner)
		      :base(owner, RejectTargetOptions.RejectWith)
		    {}

            protected override string GetValueAsString ()
            {
                return AliasUtil.GetDefaultAlias(this.rejectWith);
            }
            
            public override void SetValues (string value)
            { 
                object enumValue = null;
                
                if(!AliasUtil.IsAliasName(typeof(RejectIcmpTypes), value, out enumValue))
                {
                    throw new FormatException("The value for --reject-with option is not"+
                            " a valid enumeration member of RejectIcmpTypes");
                }
                
                this.rejectWith = (RejectIcmpTypes)enumValue; 
            }


		    
		}
	}
}
