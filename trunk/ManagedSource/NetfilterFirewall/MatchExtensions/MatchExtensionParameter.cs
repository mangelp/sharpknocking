
using System;
using System.Collections;

using SharpKnocking.Common;
using SharpKnocking.NetfilterFirewall;

namespace SharpKnocking.NetfilterFirewall.ExtendedMatch
{
	
	public class MatchExtensionParameter: GenericParameter
	{
	    private MatchExtensionHandler owner;
	    
	    public MatchExtensionHandler Owner
	    {
	        get { return this.owner;}
	        set { this.owner = value;}
	    }
	    
	    public override string Name
	    {
	        get
	        {
	           return base.Name;    
	        }
	        set
	        {
	            object type;
	            
	            if(!this.owner.TryConvertToName(value, out type))
	            {
	                throw new ArgumentException("The name doesn't match a valid"+
                                                " option for the owner extension"+
                                                " or it is empty or null");
	            }
	            
	            base.Name = value;
	        }
	    }
	    
	    
	    public override string Value
	    {
	        get
	        {
	           return base.Value;    
	        }
	        set
	        {
	            object objVal;
	            
	            if(!this.owner.TryConvertToValue(this.Name, value, out objVal))
	            {
	               throw new ArgumentException("The name is not a valid option name"+    
	                                           " for this extension or the value is"+
	                                           " not a valid value fot the option");
	            }
	            
	            base.Value = value;
	        }
	    }
	    
		public MatchExtensionParameter(MatchExtensionHandler owner)
		  :base()
		{
		    this.owner = owner;
		}
		
	}
}
