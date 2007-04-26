
using System;

using SharpKnocking.Common;
using IptablesNet.Core;
using IptablesNet.Core.Extensions.ExtendedTarget;

namespace IptablesNet.Core.Options
{ 
	/// <summary>
	/// Jump option.
	/// </summary>
	/// <remarks>
	/// This option is where the target extensions are specified. When
	/// a target extension is used a extension handler instance is
	/// obtained to handle the parameters.<br/><br/>
	/// Something to do is to implement the table restrictions for some
	/// extensions.
	/// </remarks>
	public class JumpOption: GenericOption
	{
	    private RuleTargets target;
	    
	    /// <summary>
	    /// Target defined.
	    /// </summary>
	    /// <remarks>
	    /// The enum value has defined a constant to specify that other property
	    /// has to be checket to know the type of target to look for.
	    /// </remarks>
	    public RuleTargets Target
	    {
	        get { return this.target;}
	        set
	        {
	            if(value != RuleTargets.CustomTarget)
	            {
	               this.customTarget = CustomRuleTargets.None;    
	            }
	            
	            this.target = value;
	        }
	    }
	    
	    private CustomRuleTargets customTarget;
	    
	    /// <summary>
	    /// Custom target defined.
	    /// </summary>
	    /// <remarks>
	    /// The enum value has defined a constant to specify that there is no
	    /// custom target.<br/>
	    /// This enumeration determines the type of custom target. The name of
	    /// the custom target is in the CustomTargetName property.
	    /// </remarks>
	    public CustomRuleTargets CustomTarget
	    {
	        get { return this.customTarget;}
	        set
	        {
	            if(this.target != RuleTargets.CustomTarget)
	            {
	                throw new InvalidOperationException(
                                  "Can't set CustomTarget if Target is"+
                                  " not set first to CustomTarget!");
	            }
	            else if(value == CustomRuleTargets.CustomExtension)
	            {
	                throw new NotSupportedException("Using of extensions for targets"+
	                                                " is not supported");
	            }
	            
	            this.customTarget = value;
	        }
	    }
	    
	    private string customTargetName;
	    
	    /// <summary>
	    /// Custom target name if defined. This can be a user-defined chain
	    /// or an extension for targets.
	    /// </summary>
	    /// <remarks>
	    /// Extension targets are partially implemented. Currently there is only
	    /// Ulog and reject implementations.<br/><br/>
        /// If this is set to a user-defined chain name or to a target extension
        /// you will have to set the Target and CustomTarget properties to
        /// reflect the situation. These enumerations are used to know what type
        /// of custom value we have.
	    /// </remarks>
	    public string CustomTargetName
	    {
	        get { return this.customTargetName;}
	        set
	        {
	            this.customTargetName = value;
                
                if(TargetExtensionHandler.IsTargetExtension(value))
	               this.extension = TargetExtensionFactory.GetExtension(value);
                else
                    this.extension = null;
	        }
	    }
	    
	    private TargetExtensionHandler extension;
	    
	    /// <summary>
	    /// Handler for the extension.
	    /// </summary>
	    public TargetExtensionHandler Extension
	    {
	        get
	        {
	            return this.extension;
	        }
	        set
	        {
	            this.extension = value;
	            this.customTargetName = this.extension.ExtensionName;
	        }
	    }
		
		public JumpOption()
		  :base(RuleOptions.Jump)
		{ 
		    this.customTarget = CustomRuleTargets.None;   
		}
		
		public override bool TryReadValues (string strVal, out string errStr)
		{
		    Debug.VerboseWrite("JumpOption.TryReadValues: Reading from '"
		                       +strVal+"'");
		    
		    if(Net20.StringIsNullOrEmpty(strVal))
		    {
		        errStr="Value can't be null or empty";
                Debug.VerboseWrite("JumpOption.TryReadValues: Empt or null value.");
		        return false;
		    }
		    
		    errStr=String.Empty;
		    
		    object obj;
		    
		    if( TypeUtil.IsAliasName(typeof(RuleTargets), strVal, out obj))
		    {
                Debug.VerboseWrite("JumpOption.TryReadValues: Found RuleTargets enum");
		        this.Target = (RuleTargets)obj;
		        
		        if(this.Target == RuleTargets.CustomTarget)
		        {
		            errStr="CustomTarget is an invalid target!";
                    Debug.VerboseWrite("JumpOption.TryReadValues: Invalid target.");
		            return false;
		        }
                
                this.customTarget = CustomRuleTargets.None;
                this.customTargetName = String.Empty;
		        
		    }
            else if(TargetExtensionHandler.IsTargetExtension(strVal))
            {
                this.target = RuleTargets.CustomTarget;
                this.customTarget = CustomRuleTargets.CustomExtension;
                this.extension = TargetExtensionFactory.GetExtension(strVal);
                
                if(this.extension==null)
                    throw new IptablesException("Can't create object for "+strVal);
                
                Debug.VerboseWrite("JumpOption.TryReadValues: Found target extension "+this.extension);
            }
		    else
		    {
        	    this.Target = RuleTargets.CustomTarget;
	            //The only custom target supported is user-defined chain
	            this.customTarget = CustomRuleTargets.UserDefinedChain;
	            this.customTargetName = strVal;
                Debug.VerboseWrite("JumpOption.TryReadValues: Found custom target");
		    }
		    
		    return true;
		}
		
		/// <summary>
		/// Checks if the current loaded target extension has a option with
		/// the given name.
		/// </summary>
		/// <returns>
		/// True if there is an extension loaded and if that extension has an
		/// option with the given name.
		/// False if there is no extension.
		/// </returns>
		public bool HasOptionNamed(string paramName)
		{
            Debug.VerboseWrite("HasOptionNamed("+paramName+")");
            
		    if(this.extension==null)
		        return false;
		    
		    return this.extension.IsSupportedParam (paramName);
		}
		
		protected override string GetValuesAsString()
		{
            string str = String.Empty;
                
            Debug.VerboseWrite("JumpOption: Converting to string ("+
                               this.target+","+this.customTarget+","+
                               this.customTargetName+","+this.extension+")",
                               VerbosityLevels.Insane);
            
            if(this.target == RuleTargets.CustomTarget)
            {
               if(this.customTarget == CustomRuleTargets.CustomExtension)
                    str = this.extension.ToString();
               else if(this.customTarget == CustomRuleTargets.UserDefinedChain)
                    str = this.customTargetName;
            }
            else
            {
                str=TypeUtil.GetDefaultAlias(this.target);
            }
            
            return str;
		}
	}
}
