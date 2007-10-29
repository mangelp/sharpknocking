
using System;
using System.Collections;

using IptablesNet.Core.Util;
using IptablesNet.Core.Extensions;
using IptablesNet.Core.Extensions.ExtendedMatch;

using Developer.Common.Net;
using Developer.Common.Types;

namespace IptablesNet.Extensions.Matches
{
    /// <summary>
    /// Implements the icmp match extension.
    /// </summary>
	public class IcmpMatchExtension: MatchExtensionHandler
	{
	    /// <summary>
	    /// cache for decoding names for the enum IcmpTypes
	    /// </summary>
	    private static NameCache icmpTypeNameCache;
	    
	    /// <summary>
	    /// We do a cache with the names for the enum IcmpTypes.
	    /// </summary>
	    static IcmpMatchExtension()
	    {
	        //We are going to keep in memory the list of option names
	        //as the keys of the hashtable and the enum constant value
	        //as the value. This will speed up the search speed.
	        
	        icmpTypeNameCache = new NameCache();
	        icmpTypeNameCache.FillFromEnum(typeof(IcmpTypes));
	    }
	    
		/// <summary>
		/// Default constructor.
		/// </summary>
		public IcmpMatchExtension()
		    :base(typeof(IcmpMatchOptions), MatchExtensions.Icmp)
        {
		}
        
        public override MatchExtensionParameter CreateParameter (string paramType)
        {
            object val=null;
            if(!AliasUtil.IsAliasName (typeof(IcmpMatchOptions), paramType, out val))
                return null;
            
            IcmpMatchOptions option = (IcmpMatchOptions)val;
            
            switch(option)
            {
                case IcmpMatchOptions.IcmpType:
                    return new IcmpTypeParam (this);
                default:
                    throw new ArgumentException ("Not supported option: "+option,"name");
            }
        }
        
		/// <summary>
		/// Create a new parameter. As there is only one parameter type for this extension
		/// this method don't require parameters.
		/// </summary>
        public IcmpTypeParam CreateParam ()
        {
            return new IcmpTypeParam(this);
        }

        public class IcmpTypeParam: MatchExtensionParameter
        {
            public new IcmpMatchExtension Owner
            {
                get { return (IcmpMatchExtension)base.Owner;}
            }
            
            public new IcmpMatchOptions Option
            {
                get { return (IcmpMatchOptions)base.Option;}
            }
            
            private IcmpTypes icmp;
            
            public IcmpTypes Icmp
            {
                get { return this.icmp;}
                set { this.icmp = value;}
            }
            
            public IcmpTypeParam(IcmpMatchExtension owner)
              :base((MatchExtensionHandler)owner, IcmpMatchOptions.IcmpType)
            {
                this.icmp = IcmpTypes.Any;
            }
            
            
            protected override string GetValuesAsString ()
            {
                int code = (int)this.icmp;
                //The format is 
                // - If greater or equal to 100 the last two digits are the subtype
                //   and the first ones the type
                // - If less than 100 is simply a type.
                if(code >= 100){
                    int subCode = code%100;
                    code = code/100;
                    return code+"/"+subCode;
                } else {
                    return code.ToString();    
                }
            }
            
            public override void SetValues (string value)
            {
                this.icmp = this.GetIcmpType (value);
            }
            
            /// <summary>
    		/// Gets the enumeration value for the integer
    		/// </summary>
    		private IcmpTypes GetIcmpType(int type)
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
    		private IcmpTypes GetIcmpType(int type, int code)
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
    		private IcmpTypes GetIcmpType(string typeStr)
    		{
    		    typeStr = typeStr.Trim();
    		    
    		    int pos = typeStr.IndexOf('/');
    		    IcmpTypes otype = IcmpTypes.Any;
    		    
    		    if(pos>=0)
    		    {
    		        int type = Int32.Parse(typeStr.Substring(0, pos));
    		        int code = Int32.Parse(typeStr.Substring(pos+1));
    		        
    		        otype = this.GetIcmpType(type, code);
    		    }
    		    else if(typeStr.Length<=2)
    		    {
    		        int type = Int32.Parse(typeStr);
    		        otype = (IcmpTypes)type;
    		    }
    		    else
    		    {
    		        if(IcmpMatchExtension.icmpTypeNameCache.Exists(typeStr))
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
        }
	}
}
