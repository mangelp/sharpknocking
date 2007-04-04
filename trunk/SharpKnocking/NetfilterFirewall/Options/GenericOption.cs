
using System;
using System.Collections;
using SharpKnocking.Common;
using SharpKnocking.NetfilterFirewall.ExtendedMatch;

namespace SharpKnocking.NetfilterFirewall.Options
{
	
	/// <summary>
	/// Option in the rule. Generic option modeling.
	/// </summary>
	public abstract class GenericOption: GenericParameter
	{
        protected bool ignoreNextValueUpdate;
        
	    /// <summary>
	    /// Sets the name of the concrete option.
	    /// </summary>
	    public override string Name
	    {
	        get
	        {
	           return base.Name;    
	        }
	        set
	        {
	            if(!GenericOption.optNameCache.ContainsKey(value))
	            {
	                throw new ArgumentException("The name '"+
	                                            value+"' is not a valid option"+
	                                            " name");                                
	            }
	            
	            //This property takes care of keeping base.Name and base.TypeTag
	            //up to date
	            this.Option = (RuleOptions)GenericOption.optNameCache[value];
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
	            string errMsg;
	            
	            if(!this.ignoreNextValueUpdate &&
                   !this.TryReadValues(value, out errMsg))
	            {
	                throw new ArgumentException("The value '"+value
	                                           +" is not valid "+
	                                           "for the option "+this.Name+
	                                           ". Reason: "+errMsg);
	            }
                else if(this.ignoreNextValueUpdate)
                {
                    this.ignoreNextValueUpdate = false;
                }
	            
	            //Use base properties. The local ones are overriden and this
	            //prevent loops
	            base.Value = value;
	        }
	    }

	    
	    private NetfilterRule parentRule;
	    
	    /// <summary>
	    /// Gets/Sets the parent rule owner of this option
	    /// </summary>
	    public NetfilterRule ParentRule
	    {
	        get { return this.parentRule;}
	        set { this.parentRule = value;}
	    }
	    
	    private RuleOptions option;
	    
	    /// <summary>
	    /// Gets/Sets the option
	    /// </summary>
	    protected RuleOptions Option
	    {
	        get {return this.option;}
	        set
	        {
	            this.option = value;
	            //Use base implementations to avoid loops between properties
	            base.Name = TypeUtil.GetDefaultAlias(value);
	            base.Value = null;
	        }
	    }
	    
	    private bool hasImplicitExtension;
	    
	    /// <summary>
	    /// Gets if the option implicitly loads a extension
	    /// </summary>
	    public bool HasImplicitExtension
	    {
	        get
	        {
	           return this.hasImplicitExtension;    
	        }
	    }
	    
	    private Type extensionType;
	    
	    /// <summary>
	    /// Returns the type for the implicit extension if this option has
	    /// a implicit extension.
	    /// </summary>
	    public virtual Type ExtensionType
	    {
	       get {  return this.extensionType; }
	    }
	    
	    /// <summary>
	    /// Default constructor.
	    /// </summary>
		public GenericOption(RuleOptions option, MatchExtensions extension)
		{
		    this.Option = option;
		    
		    this.SetImplicitExtension(extension);
		}
		
		/// <summary>
		/// Default constructor
		/// </summary>
		public GenericOption(RuleOptions option)
		{
		    //This sets everything to defaults
		    this.Option = option;
		}
		
		/// <summary>
		/// Sets the extension that will be loaded implicitly if the current
		/// option is used.
		/// </summary>
		protected void SetImplicitExtension(MatchExtensions extension)
		{
		    this.extensionType = MatchExtensionFactory.GetExtensionType(extension);
		    
		    if(this.extensionType==null)
		    {
		        throw new InvalidOperationException("Can't load the implementation "+
		                                            "for the extension "+ 
                                                    extension.ToString().ToLower()+
                                                    ".");
		    }
		}

	    //cache for decoding names as options
	    private static Hashtable optNameCache;
	    
	    static GenericOption()
	    {
	        //We are going to keep in memory the list of option names
	        //as the keys of the hashtable and the enum constant value
	        //as the value. This will speed up the search speed.
	        
	        optNameCache = new Hashtable();
	        
	        Array arr = Enum.GetValues(typeof(RuleOptions));
	        string[] aliases;
	        
	        foreach(object obj in arr)
	        {
	            aliases = TypeUtil.GetAliases(obj);
	            
	            for(int i=0;i<aliases.Length;i++)
	            {
	                optNameCache.Add(aliases[i], obj);
	            }
	        }
	    }
		
		/// <summary>
		/// Gets if the parameter name is a option
		/// </summary>
		public static bool IsOption(string option)
		{
		    if(optNameCache.ContainsKey(option))
		    {
		        return true;    
		    }
		    
		    return false;
		}
		
	    /// <summary>
		/// Returns the Options enumeration constant that matches the
		/// option name. The name can be any valid alias for the option but
		/// it can't be the name of a constant of the enumeration.
		/// </summary>
		/// <returns>
		/// Options.None if the name is not a valid alias or it is the name of
		/// a constant in the enumeration.
		/// </returns>
		public static RuleOptions GetOptionType(string optName)
		{
		    if(optNameCache.ContainsKey(optName))
		    {
		        return (RuleOptions)optNameCache[optName];    
		    }
		    
		    return RuleOptions.None;
		}
		
		/// <summary>
		/// Returns if the name is a valid option.
		/// </summary>
		public static bool IsValidName(string name)
		{
		    if(GenericOption.optNameCache.ContainsKey(name))
		        return true;
		    
		    return false;
		}
		
//		/// <summary>
//		/// Returns true if the value is a valid value for the current option
//		/// type.
//		/// </summary>
//		public abstract bool IsValidValue(string value);
//	    
//		/// <summary>
//		/// Returns true if the value is a valid value for the current option
//		/// type.
//		/// </summary>
//		public abstract bool IsValidValue(object value);
		
		/// <summary>
		/// Tries to load values for the current option from a string. If fails
		/// an error string is returned by an output parameter
		/// </summary>
		/// <returns>
		/// True if the string can be converted to a set of values for the option
		/// or false if not.
		/// </returns>
		public abstract bool TryReadValues(string strVal, out string errMsg);
		
//		/// <summary>
//		/// This method is executed after the Value property has been completely
//		/// set (no errors and ValueTag have been set too)
//		/// </summary>
//		protected virtual void OnValueSet()
//		{
//		
//		}

        /// <summary>
        /// Returns a string with all the values for this option.
        /// </summary>
        protected virtual string GetValuesAsString()
        {
            return base.Value;
        }
		
		/// <summary>
		/// Returns an string with all the information in the option in the
		/// format used in iptables-save.
		/// </summary>
        public override string ToString ()
        {
            string result = TypeUtil.GetDefaultAlias(this.option);
            
            if(this.IsLongOption)
                result = "--"+result;
            else
                result = "-"+result;
            
            string strVal = this.GetValuesAsString();
            
            if(!Net20.StringIsNullOrEmpty(strVal))
                result+=" "+strVal;
            
            return result;
        }


	}
}
