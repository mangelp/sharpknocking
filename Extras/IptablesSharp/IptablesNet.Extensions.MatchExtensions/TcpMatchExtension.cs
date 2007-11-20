// TcpMatchExtension.cs
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
using System.Text;
using System.Collections;

using IptablesNet.Core.Extensions;
using IptablesNet.Core.Extensions.ExtendedMatch;

using Developer.Common.Net;
using Developer.Common.Types;

namespace IptablesNet.Extensions.Matches
{
	
	/// <summary>
	/// Models the tcp extension options management.
	/// </summary>
	public class TcpMatchExtension: MatchExtensionHandler
	{
        
		public TcpMatchExtension()
		  :base(typeof(TcpMatchOptions), MatchExtensions.Tcp)
		{
		
		}
        
        public override MatchExtensionParameter CreateParameter ( string name )
        {
            object val = null;
            if(!AliasUtil.IsAliasName (typeof(TcpMatchOptions), name, out val))
                return null;
            
            TcpMatchOptions option = (TcpMatchOptions)val;
            
            return this.CreateParameter (option);
        }
        
        public TcpParam CreateParameter (TcpMatchOptions option)
        {
            switch(option)
            {
                case TcpMatchOptions.DestinationPort:
                    return new TcpDestinationParam (this);
                case TcpMatchOptions.SourcePort:
                    return new TcpSourceParam (this);
                case TcpMatchOptions.Syn:
                    return new TcpSynParam (this);
                case TcpMatchOptions.TcpFlags:
                    return new TcpFlagsParam (this);
                case TcpMatchOptions.TcpOption:
                    return new TcpOptionParam (this);
                default:
                    throw new ArgumentException ("Not supported option: "+option,"name");
            }
        }
        
        public TcpParam CreateParameter (TcpMatchOptions option, string value)
        {
            TcpParam par = this.CreateParameter (option);
            if(par!=null)
                par.SetValues(value);
            return par;
        }
        
        public override MatchExtensionParameter CreateParameter (string paramType, string value)
        {
            MatchExtensionParameter meParam = this.CreateParameter (paramType);
            if (meParam != null)
                meParam.SetValues (value);
            return meParam;
        }
		
        /// <summary>
        /// Abstract base class for every parameter
        /// </summary>
		public abstract class TcpParam: MatchExtensionParameter
		{
            public new TcpMatchExtension Owner
            {
                get { return (TcpMatchExtension)base.Owner;}
            }
			
			public new TcpMatchOptions Option
			{
				get { return (TcpMatchOptions)base.Option;}
			}
			
			public TcpParam(TcpMatchExtension handler, object paramType)
				:base((MatchExtensionHandler)handler, paramType)
			{
				
			}
		}
		
        /// <summary>
        /// Source parameter implementation
        /// </summary>
        public class TcpSourceParam: TcpParam
        {            
            private PortRange range;
            
            /// <summary>
            /// Gets/sets the port range.
            /// </summary>
            public PortRange Range
            {
                get { return  this.range;}
                set { this.range = value; }
            }
            
            public TcpSourceParam(TcpMatchExtension owner)
              :base(owner, TcpMatchOptions.SourcePort)
            {
            }
			
			protected TcpSourceParam(TcpMatchExtension handler, object paramType)
				:base(handler, paramType)
			{
				
			}
            
            protected override string GetValueAsString ()
            {
				return range.ToString();
            }
            
            public override void SetValues (string value)
            {
                this.range = PortRange.Parse(value);
            }
        }
		
        /// <summary>
        /// Destination parameter implementation
        /// </summary>
		public class TcpDestinationParam: TcpSourceParam
		{
			public TcpDestinationParam(TcpMatchExtension owner)
				:base(owner, TcpMatchOptions.DestinationPort)
			{
				
			}
		}
		
        /// <summary>
        /// TcpFlags parameter implementation
        /// </summary>
		public class TcpFlagsParam: TcpParam
		{
			private TcpFlags mask;
            
            /// <summary>
            /// Get/Set which flags are being examined.
            /// </summary>
            public TcpFlags Mask{
                get { return this.mask; }
                set { this.mask = value;}
            }
			
			private TcpFlags comp;
            
            /// <summary>
            /// Get/Set which flags are being set when the mask matches
            /// </summary>
            public TcpFlags Comp
            {
                get { return this.comp;}
                set { this.comp = value;}
            }
			
			public TcpFlagsParam (TcpMatchExtension owner)
				:base(owner, TcpMatchOptions.TcpFlags)
			{}
            
            private string GetFlagsString (TcpFlags flags)
            {
                Type t = typeof(TcpFlags);
                
                if(!t.IsEnum)
                    return String.Empty;
                
                Array values = Enum.GetValues (t);
                StringBuilder sb = new StringBuilder ();
                
                foreach (TcpFlags value in values)
                {
                    if ((flags & value) == value)
                        sb.Append(value.ToString()+",");
                }
                
                //Remove last ',' character
                sb.Remove (sb.Length-1, 1);
                
                return sb.ToString();
            }
            
            private TcpFlags GetFlagsEnum (string flags)
            {
		        string[] list = StringUtil.Split (flags, true, ',');
                object obj;
                
		        TcpFlags curFlag = TcpFlags.None; 
		        
		        for (int i=0;i<list.Length;i++) {
		            obj =TypeUtil.GetEnumValue(typeof(TcpFlags), list[i]);
                    if(obj != null)
    		          curFlag |= (TcpFlags)obj;    
		        }
                
                return curFlag;
            }
            
            protected override string GetValueAsString ()
            {
                return GetFlagsString (this.mask)+ " "+
                    GetFlagsString (this.comp);
            }

            public override void SetValues (string value)
            {
                int pos = value.IndexOf(' ');
                
                if(pos<0)
                    throw new FormatException (
                                  "The input string is not well-formed: "+value);
                
                this.mask = this.GetFlagsEnum(value.Substring(0,pos));
                this.comp = this.GetFlagsEnum(value.Substring(pos+1));
            }

		}
		
        /// <summary>
        /// Syn parameter implementation
        /// </summary>
		public class TcpSynParam: TcpParam
		{
			public TcpSynParam(TcpMatchExtension owner)
				:base(owner, TcpMatchOptions.Syn)
			{}
            
            protected override string GetValueAsString ()
            {
                return String.Empty;
            }
            
            public override void SetValues (string value)
            {   
                //Nothing to do!. This parameter has no value
            }
		}
		
        /// <summary>
        /// TcpOption parameter implementation
        /// </summary>
		public class TcpOptionParam: TcpParam
		{
			private int number;
            
            public int Number{
                get { return this.number; }
                set { this.number = value;}
            }
			
			public TcpOptionParam(TcpMatchExtension owner)
				:base(owner, TcpMatchOptions.Syn)
			{}
            
            protected override string GetValueAsString ()
            {
                return this.number.ToString();
            }
            
            public override void SetValues (string value)
            {
                this.number = Int32.Parse(value);
            }
		}
		
//		public class TcpMssParam: TcpParam
//		{
//			private PortRange mss;
//			
//			public TcpMssParam(TcpMatchExtension owner)
//				:base(owner, TcpExtensionOptions.)
//			{}
//		}
	}
}
