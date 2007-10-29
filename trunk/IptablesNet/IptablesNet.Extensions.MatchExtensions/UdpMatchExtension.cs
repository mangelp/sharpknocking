
using System;
using System.Collections;

using IptablesNet.Core.Extensions;
using IptablesNet.Core.Extensions.ExtendedMatch;

using Developer.Common.Types;
using Developer.Common.Net;

namespace IptablesNet.Extensions.Matches
{
	
	public class UdpMatchExtension: MatchExtensionHandler
	{
		
		public UdpMatchExtension()
		  :base(typeof(UdpMatchOptions), MatchExtensions.Udp)
		{
		}
		
		public override MatchExtensionParameter CreateParameter(string par)
		{
			object type;
			
			if(!AliasUtil.IsAliasName ( typeof(UdpMatchOptions), par, out type))
				return null;
			
			UdpMatchOptions option = (UdpMatchOptions)type;
			
			return this.CreateParameter (option);
		}
			                       
        public UdpParam CreateParameter(UdpMatchOptions option)
        {
			switch (option)
			{
				case UdpMatchOptions.SourcePort:
					return new UdpSourceParam (this);
                case UdpMatchOptions.DestinationPort:
                    return new UdpDestinationParam (this);
                default:
                    throw new ArgumentException ("Invalid option type: "+option);
			}
        }
        
        public UdpParam CreateParameter(UdpMatchOptions option, string value)
        {
			UdpParam par = this.CreateParameter(option);
            if(par!=null)
                par.SetValues(value);
            return par;
        }        
        
        public abstract class UdpParam: MatchExtensionParameter
        {
            public new UdpMatchExtension Owner
            {
                get { return (UdpMatchExtension)base.Owner;}
            }
			
			public new UdpMatchOptions Option
			{
				get { return (UdpMatchOptions)base.Option;}
			}
            
            protected UdpParam(UdpMatchExtension owner, UdpMatchOptions option)
                :base((MatchExtensionHandler)owner, option)
            {}
        }
		
		public class UdpSourceParam: UdpParam
		{

            private PortRange range;
            
            public PortRange Range
            {
                get { return  this.range;}
                set
                {
                    this.range = value;
                }
            }
			
			public UdpSourceParam(UdpMatchExtension handler)
				:base(handler, UdpMatchOptions.SourcePort)
			{
				
			}
			
			protected UdpSourceParam(UdpMatchExtension handler, UdpMatchOptions paramType)
				:base(handler, paramType)
			{
				
			}
            
            protected override string GetValuesAsString ()
            {
                if(this.range.IsValid)
                    return this.range.ToString();
                else
                    return String.Empty;
            }
            
            public override void SetValues (string value)
            {
                this.range = PortRange.Parse (value);
            }

		}
		
		public class UdpDestinationParam: UdpSourceParam
		{
			public UdpDestinationParam(UdpMatchExtension owner)
				:base(owner, UdpMatchOptions.DestinationPort)
			{
				
			}
		}

	}
}
