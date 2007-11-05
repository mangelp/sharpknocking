// UlogTargetExtension.cs
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

using IptablesNet.Core.Extensions;
using IptablesNet.Core.Extensions.ExtendedTarget;

namespace IptablesNet.Extensions.Targets
{
	
	/// <summary>
	/// This  target provides userspace logging of matching packets.  When this
    /// target is set for a rule, the Linux kernel will multicast  this  packet
    /// through a netlink socket. One or more userspace processes may then sub-
    /// scribe to various multicast groups and receive the packets.  Like  LOG,
    /// this  is  a  "non-terminating target", i.e. rule traversal continues at
    /// the next rule.
	/// </summary>
	public class UlogTargetExtension: TargetExtensionHandler
	{
		
		public UlogTargetExtension()
		  :base(typeof(UlogTargetOptions), TargetExtensions.Ulog)
		{
		}
        
        public override TargetExtensionParameter CreateParameter (string paramType)
        {
            object obj=null;
            if(!AliasUtil.IsAliasName ( typeof(UlogTargetOptions), paramType, out obj))
                return null;
            
            UlogTargetOptions option = (UlogTargetOptions)obj;
            
            switch(option)
            {
                case UlogTargetOptions.UlogCprange:
                    return new UlogCprangeParam (this);
                case UlogTargetOptions.UlogNlgroup:
                    return new UlogNlgroupParam (this);
                case UlogTargetOptions.UlogPrefix:
                    return new UlogPrefixParam (this);
                case UlogTargetOptions.UlogQthreshold:
                    return new UlogQthresholdParam (this);
                default:
                    throw new ArgumentException ("Not supported option: "+option,"name");
            }
        }

		public abstract class UlogParam: TargetExtensionParameter
		{
		    public new UlogTargetExtension Owner
		    {
		        get { return (UlogTargetExtension)base.Owner; }
		    }
		    
		    private UlogTargetOptions option;
		    
		    public new UlogTargetOptions Option
		    {
		        get { return this.option;}
		    }
		    
		    protected UlogParam(UlogTargetExtension owner, UlogTargetOptions option)
		    :base(owner, option)
		    {}
		}
        
        public class UlogNlgroupParam: UlogParam
        {
            private int nlgroup;
            
            /// <summary>
            /// Gets/Sets the netlink group where a packet is sent.
            /// </summary>
            /// <remarks>
            /// If the number set is greater than 32 or lower than 1 a value of 
            /// 32 or 1 (respectively) is assigned.
            /// </remarks>
            public int Nlgroup
            {
                get { return this.nlgroup;}
                set {
                    if(value>=32)
                        this.nlgroup = 32;
                    else if(value<1)
                        this.nlgroup = 1;
                    else
                        this.nlgroup = value;
                }
            }
            
            public UlogNlgroupParam (UlogTargetExtension owner)
                :base (owner, UlogTargetOptions.UlogNlgroup)
            {}
            
            protected override string GetValuesAsString ()
            {
                return this.nlgroup.ToString();
            }

            public override void SetValues (string value)
            {
                this.Nlgroup = Int32.Parse(value);
            }

        }
        
        public class UlogCprangeParam: UlogParam
        {
            private int cprange;
            
            /// <summary>
            /// Gets/Sets the netlink group where a packet is sent.
            /// </summary>
            /// <remarks>
            /// If the number set is less than 0 the value assigned is 0
            /// </remarks>
            public int Cprange
            {
                get { return this.cprange;}
                set {
                    if(value<0)
                        this.cprange = 0;
                    else
                        this.cprange = value;
                }
            }
            
            public UlogCprangeParam (UlogTargetExtension owner)
                :base (owner, UlogTargetOptions.UlogCprange)
            {}
            
            protected override string GetValuesAsString ()
            {
                return this.cprange.ToString();
            }

            public override void SetValues (string value)
            {
                this.Cprange = Int32.Parse(value);
            }
        }
        
        public class UlogPrefixParam: UlogParam
        {
            private string prefix;
            
            /// <summary>
            /// Gets/Sets the netlink group where a packet is sent.
            /// </summary>
            /// <remarks>
            /// If the string assigned has more than 32 characters its truncated.
            /// </remarks>
            public string Prefix
            {
                get { return this.prefix;}
                set {
                    if(String.IsNullOrEmpty(value))
                        throw new ArgumentException("Can't be null or empty","value");
                    
                    if(value.Length>32)
                        this.prefix = value.Substring(0,32);
                    else
                        this.prefix = value;
                }
            }
            
            public UlogPrefixParam (UlogTargetExtension owner)
                :base (owner, UlogTargetOptions.UlogPrefix)
            {}
            
            protected override string GetValuesAsString ()
            {
                return this.prefix;
            }

            public override void SetValues (string value)
            {
                this.prefix = value;
            }
        }
        
        public class UlogQthresholdParam: UlogParam
        {
            private int qthreshold;
            
            /// <summary>
            /// Gets/Sets the netlink group where a packet is sent.
            /// </summary>
            /// <remarks>
            /// If the number to set is less than 1 the value of 1 is set.
            /// </remarks>
            public int Qthreshold
            {
                get { return this.qthreshold;}
                set {
                    if(value<1)
                        this.qthreshold = 1;
                    else
                        this.qthreshold = value;
                }
            }
            
            public UlogQthresholdParam (UlogTargetExtension owner)
                :base (owner, UlogTargetOptions.UlogQthreshold)
            {}
            
            protected override string GetValuesAsString ()
            {
                return this.qthreshold.ToString();
            }

            public override void SetValues (string value)
            {
                this.Qthreshold = Int32.Parse(value);
            }
        }
	}
}
