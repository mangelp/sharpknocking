
using System;
using System.Collections;
using SharpKnocking.IpTablesManager.RuleHandling.Extensions;

namespace SharpKnocking.IpTablesManager.RuleHandling
{
	
	/// <summary>
	/// Parameter in the rule.
	/// </summary>
	public class RuleParameter
	{
	    
	    private string text;
	    
	    /// <summary>
	    /// Parameter value as string.
	    /// Currently the number of long format parameters are limited.
	    /// </summary>
	    public string Text
	    {
	        get {return this.text;}
	        set
	        {
	            this.text = value;
	        }
	    }
	    
	    private bool isLongOption;
	    
	    /// <summary>
	    /// Gets if the parameter is a long format paramerter or not.
	    /// </summary>
	    public bool IsLongOption
	    {
	        get {return this.isLongOption;}
	        set {this.isLongOption = value;}
	    }
	    
	    private RuleParameters type;
	    
	    /// <summary>
	    /// Gets/Sets the type of the parameter.
	    /// </summary>
	    public RuleParameters Type
	    {
	        get {return this.type;}
	        set {this.type = value;}
	    }
	    
	    /// <summary>
	    /// Gets/Sets if the parameter is a match extension parameter.
	    /// </summary>
	    public bool IsMatchExtension
	    {
	        get
	        {
	            return (this.type == RuleParameters.MatchExtension);
	        }
            set
            {
                this.type = RuleParameters.MatchExtension;
            }
	    }
	    
	    private BaseExtension extension;
	    
	    /// <summary>
	    /// Gets/Sets the match extension object instance that handles the
	    /// extension.
	    /// </summary>
	    public BaseExtension Extension
	    {
	        get {return this.extension;}
	        set {this.extension = value;}
	    }
	    
	    /// <summary>
	    /// Default constructor.
	    /// </summary>
		public RuleParameter()
		{
		}
	}
}
