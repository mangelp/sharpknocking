
using System;
using System.Collections;

using IptablesNet.Core.Extensions;
using IptablesNet.Core.Extensions.ExtendedMatch;

using Developer.Common.Types;

namespace IptablesNet.Core.Options
{
	
	/// <summary>
	/// Option in the rule. Generic option modeling.
	/// </summary>
	public abstract class GenericOption: NegableParameter
	{
		/// <summary>
		/// Returns true if the option is in long format (-- prefix) or false if it
		/// is in short format (- prefix).
		/// </summary>
		/// <value>True of false</value>
		public override bool IsLongFormat
		{
			get 
			{
				string def = AliasUtil.GetDefaultAlias (this.optionType);
				if(def!=null && def.Length > 1)
					return true;
				
				return false;
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
	    
	    private RuleOptions optionType;
	    
	    /// <summary>
	    /// Gets/Sets the option
	    /// </summary>
	    protected RuleOptions OptionType
	    {
	        get {return this.optionType;}
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
		/// <value>Type of the implicit extension this option has.</value>
	    public virtual Type ExtensionType
	    {
	       get {  return this.extensionType; }
	    }
	    
	    /// <summary>
	    /// Default constructor.
	    /// </summary>
		public GenericOption(RuleOptions option, MatchExtensions extension)
		{
		    this.optionType = option;
		    
		    this.SetImplicitExtension(extension);
		}
		
		/// <summary>
		/// Default constructor
		/// </summary>
		public GenericOption(RuleOptions option)
		{
		    //This sets everything to defaults
		    this.optionType = option;
		}
		
		public override string GetDefaultAlias()
		{
			return AliasUtil.GetDefaultAlias (this.optionType);
		}
		
		public override bool IsAlias(string name)
		{
			return AliasUtil.IsAliasName (this.optionType, name);
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
		    
		    this.hasImplicitExtension = true;
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
	            aliases = AliasUtil.GetAliases(obj);
	            
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
		
		/// <summary>
		/// Tries to load values for the current option from a string. If fails
		/// an error string is returned by an output parameter
		/// </summary>
		/// <returns>
		/// True if the string can be converted to a set of values for the option
		/// or false if not.
		/// </returns>
		public abstract bool TryReadValues(string strVal, out string errMsg);
	}
}
