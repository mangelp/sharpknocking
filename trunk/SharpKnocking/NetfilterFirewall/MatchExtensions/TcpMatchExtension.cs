
using System;
using System.Collections;

using SharpKnocking.Common;
using SharpKnocking.Common.Net;
using SharpKnocking.NetfilterFirewall;

namespace SharpKnocking.NetfilterFirewall.ExtendedMatch
{
	
	/// <summary>
	/// Models the tcp extension options management.
	/// </summary>
	public class TcpMatchExtension: MatchExtensionHandler
	{
		
		public TcpMatchExtension()
		  :base(typeof(TcpExtensionOptions), "tcp")
		{
		      
		}
		
		/// <summary>
		/// Returns true if the name can be converted to an enumeration constant
		/// of the type TcpExceptionOptions. It also sets the value for the type
		/// in the out parameter "type".
		/// Otherwise it returns false and the out parameter has a value that
		/// must not be used for nothing.
		/// </summary>
		public static bool TryGetOptionNameType(string name, out TcpExtensionOptions type)
		{
		    type = TcpExtensionOptions.None;
		    object oType;
            if(TypeUtil.IsAliasName(typeof(TcpExtensionOptions), name, out oType))
            {
                type = (TcpExtensionOptions)oType;
                return true;    
            }
            
            return false;
		}
		
		/// <summary>
		/// Returns true if the string can be converted to a single TcpFlags enumeration
		/// constant.
		/// </summary>
		public static bool TryGetSingleTcpFlag(string value, out TcpFlags flag)
		{
		    flag = TcpFlags.None;
		    
		    try
		    {
		        object obj =TypeUtil.GetEnumValue(typeof(TcpFlags), value);
		        
		        if(obj!=null)
		        {
		            flag = (TcpFlags)obj;
		            return true;
		        }
		    }
		    catch(Exception)
		    { 
		    }
		    
		    return false;
		}
		
		/// <summary>
		/// Returns true if the string is a comma separated list of valid tcp
		/// flags (almost with one element) or false if the string has invalid
		/// format.<br/>
		/// The flags value is returned in the out parameter
		/// </summary>
		public static bool TryGetTcpFlags(string value, out TcpFlags flags)
		{
		    flags = TcpFlags.None;
		    
		    try
		    {
		        string[] list = Net20.StringSplit(value, true, ',');

		        flags = TcpFlags.None;
		        TcpFlags curFlag = TcpFlags.None; 
		        
		        for(int i=0;i<list.Length;i++)
		        {
		            if(!TcpMatchExtension.TryGetSingleTcpFlag(value, out curFlag))
		                return false;
		            else
		                flags = flags | curFlag;    
		        }
		        
		        return true;
		    }
		    catch(Exception)
		    {
		    }
		    
		    return false;
		}
		
		public override Type GetInternalParameterType ()
		{
			return typeof(TcpParameter);
		}
		
		public override MatchExtensionParameter CreateParameter ()
		{
			return new TcpParameter(this);
		}
		
		public override MatchExtensionParameter CreateParameter(string name, string value)
		{
		    TcpParameter param = new TcpParameter(this);
		    param.Name = name;
		    param.Value = value;
		    
		    return param;
		}


		
		public override bool TryConvertToName (string paramName, out object objName)
		{
			if(!TypeUtil.IsAliasName(typeof(TcpExtensionOptions), paramName, out objName))
            {
                return false;    
            }
            
            return true;
		}
		
		public override bool TryConvertToValue (
              string paramName,
              string value,
              out object objValue)
        {
            TcpExtensionOptions option = TcpExtensionOptions.None;
            object oOption;
            objValue = null;
            
            if(!TypeUtil.IsAliasName(typeof(TcpExtensionOptions), paramName, out oOption))
            {
                return false;    
            }
            
            option = (TcpExtensionOptions)oOption;
            
            switch(option)
            {
                case TcpExtensionOptions.DestinationPort:
                case TcpExtensionOptions.SourcePort:
                    int port;
                    if(!Net20.Int32TryParse(value, out port))
                        return false;
                    objValue = port;
                    break;
                case TcpExtensionOptions.TcpFlags:
                    TcpFlags flags;
                    if(!TcpMatchExtension.TryGetTcpFlags(value, out flags))
                        return false;
                    objValue = flags;
                    break;
                case TcpExtensionOptions.TcpOption:
                    int number;
                    if(!Net20.Int32TryParse(value, out number))
                        return false;
                    objValue = number;
                    break;
                case TcpExtensionOptions.Syn:
                    if(!Net20.StringIsNullOrEmpty(value))
                        return false;
                    objValue=null;
                    break;
                default:
                    throw new InvalidOperationException("Unsupported option "+option);
            }
            
            return true;
        }
        
        public override bool IsValidName (object option)
        {
        	return (option is TcpExtensionOptions);
        }
        
        public override bool IsValidValue (object option, object value)
        {
            if(!this.IsValidName(option))
            {
                return false;    
            }
            
            switch((TcpExtensionOptions)option)
            {
                case TcpExtensionOptions.DestinationPort:
                case TcpExtensionOptions.SourcePort:
                    if(!(value is int))
                        return false;
                    break;
                case TcpExtensionOptions.TcpFlags:
                    if(value==null || !(value is TcpFlags))
                        return false;
                    break;
                case TcpExtensionOptions.TcpOption:
                    if(!(value is int))
                        return false;
                    break;
                case TcpExtensionOptions.Syn:
                    if(value!=null || !(value is string) || ((string)value).Length!=0)
                        return false;
                    break;
                default:
                    throw new InvalidOperationException("Unsupported option "+option);
            }
            
            return true;
        }



        public class TcpParameter: MatchExtensionParameter
        {
            public new TcpMatchExtension Owner
            {
                get { return (TcpMatchExtension)base.Owner;}
                set { base.Owner = (TcpMatchExtension)value;}
            }
            
            private int port;
            
            public int Port
            {
                get { return  this.port;}
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
            
            private TcpExtensionOptions type;
            
            public new TcpExtensionOptions Type
            {
                get { return this.type;}
                set { this.type = value;}
            }
            
            public TcpParameter(TcpMatchExtension owner)
              :base((MatchExtensionHandler)owner)
            {
            }

        }
	}
}
