
using System;
using System.Collections;

using SharpKnocking.Common;
using SharpKnocking.Common.Net;
using SharpKnocking.NetfilterFirewall;

namespace SharpKnocking.NetfilterFirewall.ExtendedMatch
{
	
	public class UdpMatchExtension: MatchExtensionHandler
	{
		
		public UdpMatchExtension()
		  :base(typeof(UdpExtensionOptions), "udp")
		{
		}
		
		public override MatchExtensionParameter CreateParameter ()
		{
			return new UdpParameter(this);
		}
		
		public override MatchExtensionParameter CreateParameter (string name, string value)
		{
		    UdpParameter par = new UdpParameter(this);
		    par.Name = name;
		    par.Value = value;
		    
		    return par;
		}
		
		public override Type GetInternalParameterType ()
		{
			return typeof(UdpParameter);
		}

		public override bool IsValidName (object option)
		{
			return (option is UdpExtensionOptions);
		}
		
		public override bool IsValidValue (object option, object value)
		{
		    if(!this.IsValidName(option))
		        return false;
		    
		    return (value!=null && value is int);
		}
		
		public override bool TryConvertToName (string paramName,out object objName)
		{
		    objName = null;
		    
		    try
		    {
		        if(TypeUtil.IsAliasName(typeof(UdpExtensionOptions), paramName, out objName))
		            return true;
		        else
		            return false;
		    }
		    catch(Exception)
		    {
		       return false;    
		    }
		}
		
		public override bool TryConvertToValue (string name, string value, out object objValue)
		{
		    objValue = null;
		    object objName = null;
		    
		    if(!this.TryConvertToName(name, out objName))
		        return false;
		    
		    
		    switch((UdpExtensionOptions)objName)
		    {
		        case UdpExtensionOptions.SourcePort:
		        case UdpExtensionOptions.DestinationPort:
		            //The port is an integer
		            int port;
		            if(Net20.Int32TryParse(value, out port))
		            {
		                objValue = port;
		            }
		            break;
		        default:
                    throw new InvalidOperationException("Unsupported option "+objName);
		    }
		    
		    return true;
		}

		
		
		public class UdpParameter: MatchExtensionParameter
		{
		    public new UdpMatchExtension Owner
		    {
		        get { return (UdpMatchExtension)base.Owner;}
		        set { base.Owner = (MatchExtensionHandler)value;}
		    }
		    
		    private UdpExtensionOptions option;
		    
		    public UdpExtensionOptions Option
		    {
		        get { return this.option;}
		        set { this.option = value;}
		    }
		    
		    private int port;
		    
		    public int Port
		    {
		        get
		        {
		            return this.port;
		        }
		        set
		        {
		            this.port = value;
		            this.Value = port+"";
		        }
		    }
		    
		    public override string Value
		    {
		    	get { return base.Value; }
		    	set
		    	{
		    	    this.port = Int32.Parse(value);
		    	    base.Value = value;
		    	}
		    }

		    
		    public UdpParameter(UdpMatchExtension owner)
		      :base((MatchExtensionHandler) owner)
		    {
		        
		    }
		}

	}
}
