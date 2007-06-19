
using System;

using IptablesNet.Core;
using IptablesNet.Core.Extensions;

using Developer.Common.Types;

namespace IptablesNet.Core.Options
{
	
	/// <summary>
	/// Implements the match extension option.
	/// </summary>
	/// <remarks>
	/// Only built in extensions are allowed. The rest will result in a
	/// conversion failure and the logic will throw an exception.
	/// </remarks>
	public class MatchExtensionOption: GenericOption
	{
	    private MatchExtensions extension;
	    
	    /// <summary>
	    /// Match extension added to a rule
	    /// </summary>
	    public MatchExtensions Extension
	    {
	        get { return this.extension;}
	        set 
            { 
                this.extension = value;
            }
	    }
	    
	    private string customExtension;
	    
	    /// <summary>
	    /// Custom extension name
	    /// </summary>
	    /// <remarks>
	    /// This property can be set but the support to use custom extensions
	    /// is not done and will throw an exception if you try to add the rule.
	    /// </remarks>
	    public string CustomExtension
	    {
	        get { return this.customExtension;}
	        set { this.customExtension = value;}
	    }
		
		public MatchExtensionOption()
		  :base(RuleOptions.MatchExtension)
		{
		      
		}
		
		public override bool TryReadValues (string strVal, out string errStr)
		{
		    object obj;
		    
		    //This conversion only supports builtIn extensions
		    if(AliasUtil.IsAliasName(typeof(MatchExtensions), strVal, out obj))
		    {
		        this.extension = (MatchExtensions)obj;
		        errStr = String.Empty;
		        return true;
		    }
		    
		    errStr = "The value can't be converted to any known extension";
		    return false;
		}
		
		protected override string GetValuesAsString()
		{
			if(this.extension == MatchExtensions.CustomExtension)
				return this.customExtension;

			return AliasUtil.GetDefaultAlias (this.extension);
		}
	}
}
