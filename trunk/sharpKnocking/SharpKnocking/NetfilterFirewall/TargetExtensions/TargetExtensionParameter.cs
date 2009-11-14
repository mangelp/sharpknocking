
using System;

using SharpKnocking.Common;
using SharpKnocking.NetfilterFirewall;

namespace SharpKnocking.NetfilterFirewall.ExtendedTarget
{
	
	
	public abstract class TargetExtensionParameter: GenericParameter
	{
		
        private TargetExtensionHandler owner;
	    
	    /// <summary>
	    /// Target extension handler owner of this instance.
	    /// </summary>
	    public TargetExtensionHandler Owner
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
	            if(!this.DisableInternalParsing)
    	            this.ParseName(value);
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
	            if(!this.DisableInternalParsing)
	                this.ParseValue(value);
	            
	            base.Value = value;
	        }
	    }
	    
	    // This controls if the parsing of the name and the value when they are
	    // set is activated o no.
	    protected bool DisableInternalParsing=false;
	    
		public TargetExtensionParameter(TargetExtensionHandler owner)
		  :base()
		{
		    this.owner = owner;
		}
		
		
		/// <summary>
		/// Parses the value string and fills the properties of the parameter.
		/// </summary>
		/// <remarks>
		/// This method must be overriden and throw FormatException when the
		/// string cannot be parsed
		/// </remarks>
		protected abstract void ParseValue(string value);
		
		
		/// <summary>
		/// Parses the name string and initializes things
		/// </summary>
		/// <remarks>
		/// This must be overriden to chech that this name is a correct
		/// option name for this parameter.
		/// </remarks>
		protected abstract void ParseName(string name);
		
	}
}
