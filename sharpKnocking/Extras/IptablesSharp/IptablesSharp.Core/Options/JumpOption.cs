// JumpOption.cs
//
//  Copyright (C) 2006 SharpKnocking project
//  Created by Miguel Angel Pérez, mangelp@gmail.com
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA 
//
using System;

using IptablesSharp.Core;
using IptablesSharp.Core.Extensions.ExtendedTarget;

using Developer.Common.Types;

namespace IptablesSharp.Core.Options
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
		    if(String.IsNullOrEmpty(strVal)) {
				errStr = "The string is null or empty";
		        return false;
			}
		    
		    errStr=String.Empty;
		    
		    object obj;
		    
		    if( AliasUtil.IsAliasName(typeof(RuleTargets), strVal, out obj)) {
		        this.Target = (RuleTargets)obj;
		        
		        if(this.Target == RuleTargets.CustomTarget)
		        {
		            errStr="CustomTarget is an invalid target!";
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
                    throw new NetfilterException("Can't create object for "+strVal);
            }
		    else
		    {
        	    this.Target = RuleTargets.CustomTarget;
	            //The only custom target supported is user-defined chain
	            this.customTarget = CustomRuleTargets.UserDefinedChain;
	            this.customTargetName = strVal;
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
		    if(this.extension==null)
		        return false;
		    
		    return this.extension.IsSupportedParam (paramName);
		}
		
		protected override string GetValueAsString()
		{
            string str = String.Empty;
            
			if(this.target == RuleTargets.CustomTarget)
			{
				//Console.WriteLine("Converting to string: "+this.target+", "+this.customTarget+", "+this.customTargetName+", "+this.extension);
				//Fix: We must return the list of option parameters, that is the name of the target extension and
				//all his parameters. This differs from what the match extension does
				if(this.customTarget == CustomRuleTargets.CustomExtension)
					str = this.extension.ExtensionName+" "+this.extension.ToString();
				else if(this.customTarget == CustomRuleTargets.UserDefinedChain)
					str = this.customTargetName;
			}
			else
			{
				str=AliasUtil.GetDefaultAlias(this.target);
            }
            
            return str;
		}
	}
}
