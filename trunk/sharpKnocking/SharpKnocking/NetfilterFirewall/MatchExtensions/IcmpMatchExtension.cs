
using System;
using System.Collections;

using SharpKnocking.Common;

namespace SharpKnocking.NetfilterFirewall.ExtendedMatch
{
    
    /// <summary>
    /// Implements the icmp extension.
    /// </summary>
	public class IcmpMatchExtension: MatchExtensionHandler
	{
	    /// <summary>
	    /// cache for decoding names for the enum IcmpTypes
	    /// </summary>
	    private static Hashtable icmpTypeNameCache;
	    
	    /// <summary>
	    /// We do a cache with the names for the enum IcmpTypes.
	    /// </summary>
	    static IcmpMatchExtension()
	    {
	        //We are going to keep in memory the list of option names
	        //as the keys of the hashtable and the enum constant value
	        //as the value. This will speed up the search speed.
	        
	        icmpTypeNameCache = new Hashtable();
	        
	        Array arr = Enum.GetValues(typeof(IcmpTypes));
	        string[] aliases;
	        
	        foreach(object obj in arr)
	        {
	            aliases = TypeUtil.GetAliases(obj);
	            
	            for(int i=0;i<aliases.Length;i++)
	            {
	                icmpTypeNameCache.Add(aliases[i], obj);
	            }
	        }
	    }
	    
		public IcmpMatchExtension()
		    :base(typeof(IcmpExtensionOptions), "icmp")
        {

		}
		
		
		/// <summary>
		/// Gets the enumeration value for the integer
		/// </summary>
		public static IcmpTypes GetIcmpType(int type)
		{
		    if(type<0)
		        throw new ArgumentException("A icmp type can't be less tan 0",
		                                              "type");
		    
		    return (IcmpTypes)type;
		}
		
		/// <summary>
		/// Gets the enum constant from the type of icmp and the code of the
		/// icmp.
		/// </summary>
		public static IcmpTypes GetIcmpType(int type, int code)
		{
		    if(type<0)
		        throw new ArgumentException("A icmp type can't be less tan 0",
		                                              "type");
		    
		    if(code<0)
		        throw new ArgumentException("A icmp code can't be less tan 0",
		                                              "code");
		    
		    if(code==0)
		        return (IcmpTypes)type;
		    else
		        return (IcmpTypes)(type*100+code);        
		}
		
		/// <summary>
		/// Gets the enum constant from a string that must be a integer or
		/// two integers with the character '/' between them.
		/// </summary>
		public static IcmpTypes GetIcmpType(string typeStr)
		{
		    typeStr = typeStr.Trim();
		    
		    int pos = typeStr.IndexOf('/');
		    IcmpTypes otype = IcmpTypes.Any;
		    
		    if(pos>=0)
		    {
		        int type = Int32.Parse(typeStr.Substring(0, pos));
		        int code = Int32.Parse(typeStr.Substring(pos+1));
		        
		        otype = IcmpMatchExtension.GetIcmpType(type, code);
		    }
		    else if(typeStr.Length<=2)
		    {
		        int type = Int32.Parse(typeStr);
		        otype = (IcmpTypes)type;
		    }
		    else
		    {
		        if(IcmpMatchExtension.icmpTypeNameCache.ContainsKey(typeStr))
		        {
		            return (IcmpTypes)icmpTypeNameCache[typeStr];        
		        }
		        else
		        {
		            throw new InvalidCastException("Can't convert from "+
		                           typeStr+" to IcmpTypes enumeration");
		        }
		    }
		    
		    return otype;
		}
		
		/// <summary>
		/// Returns true if the string can be converted to IcmpTypes enumeration
		/// and false if not.
		/// </summary>
		public static bool TryGetIcmpType(string type, out IcmpTypes otype)
		{
		    try
		    {
		        otype = IcmpMatchExtension.GetIcmpType(type);
		    }
		    catch(Exception)
		    {
		        //We have to set a value in the out parameter
		        otype = IcmpTypes.Any;
		        return false;    
		    }
		    
		    return true;
		}
		
		public override MatchExtensionParameter CreateParameter ()
		{
			return new IcmpParameter(this);
		}
		
		public override MatchExtensionParameter CreateParameter (string name, string value)
		{
		    IcmpParameter newPar = new IcmpParameter(this);
		    newPar.Value = value;
		    newPar.Name = name;
		    
		    return newPar;
		}
		
		public override Type GetInternalParameterType ()
		{
			return typeof(IcmpParameter);
		}
		
		public override bool IsValidName (object option)
		{
			return (option is IcmpExtensionOptions);
		}
		
		public override bool IsValidValue (object option, object value)
		{
		    if(!this.IsValidName(option))
		        return false;
		    
		    switch((IcmpExtensionOptions)option)
		    {
		        case IcmpExtensionOptions.IcmpType:
		            return (value is IcmpTypes);
		        default:
		            throw new InvalidOperationException("Unsupported option "+option);
		    }
		}
		
		public override bool TryConvertToName (string paramName, out object objName)
		{
			objName = null;
		    
		    try
		    {
		        if(TypeUtil.IsAliasName(typeof(IcmpExtensionOptions), paramName, out objName))
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
		    
		    
		    switch((IcmpExtensionOptions)objName)
		    {
		        case IcmpExtensionOptions.IcmpType:
		            IcmpTypes oType = IcmpTypes.Any;
		            if(!IcmpMatchExtension.TryGetIcmpType(value, out oType))
		                return false;
		            objValue = oType;
		            break;
		        default:
                    throw new InvalidOperationException("Unsupported option "+objName);
		    }
		    
		    return true;
		}


        public class IcmpParameter: MatchExtensionParameter
        {
            public new IcmpMatchExtension Owner
            {
                get { return (IcmpMatchExtension)base.Owner;}
                set { base.Owner = (IcmpMatchExtension)value;}
            }
            
            private IcmpExtensionOptions option;
            
            public IcmpExtensionOptions Option
            {
                get { return this.option;}
                set { this.option = value;}
            }
            
            private IcmpTypes icmp;
            
            public IcmpTypes Icmp
            {
                get { return this.icmp;}
                set { this.icmp = value;}
            }
            
            public IcmpParameter(IcmpMatchExtension owner)
              :base((MatchExtensionHandler)owner)
            {
            }

        }
	}
}
