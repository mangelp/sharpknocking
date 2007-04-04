
using System;
using SharpKnocking.Common;
using SharpKnocking.IpTablesManager.RuleHandling.Extensions;

namespace SharpKnocking.IpTablesManager.RuleHandling
{
	
	/// <summary>
	/// Defines an extended option of fixed type MatchExtension.
	/// </summary>
	public class RuleOptionExtended: RuleOption
	{
        //Disable the set of the method
	    public override Options Option
	    {
	    	set { ; }
	    }
	    
	    private MatchExtensions matchExtension;
	    
	    /// <summary>
	    /// Type of extension.
	    /// </summary>
	    public MatchExtensions MatchExtension
	    {
	        get
	        {
	            return this.matchExtension;
	        }
	        set
	        {
	            this.matchExtension = value;
	            this.UpdateValue();
	        }
	    }
	    
	    private string customExtension;
	    
	    /// <summary>
	    /// Gets/sets the name of the custom extension if the type of extension
	    /// is set to MatchExtensions.CustomExtension
	    /// </summary>
	    public string CustomExtension
	    {
	        get { return this.customExtension;}
	        set
	        {
	            this.customExtension = value;
	            this.UpdateValue();
	        }
	    }
	    
	    private MatchExtensionHandler extension;
	    
	    public MatchExtensionHandler Extension
	    {
	       get { return this.extension;}    
	    }
	    
		public RuleOptionExtended()
		  :base()
		{
		    base.Option = Options.MatchExtension;
		}
		
		/// <summary>
		/// Updates the value field
		/// </summary>
		private void UpdateValue()
		{
		    if(this.matchExtension == MatchExtensions.CustomExtension)
		    {
		        base.SetValue(this.customExtension);
		        //The custom extensions are not supported. This is notified
		        //using an exception.
		        throw new InvalidOperationException(
		                       "The custom match extensions are not supported");
		    }
		    else
		    {
		        base.SetValue(TypeUtil.GetDefaultAlias(this.matchExtension));
		        
		        //Load the extension
		        extension = MatchExtensionFactory.GetExtension(this.matchExtension);
		        
		        if(extension==null)
		        {
		            throw new InvalidOperationException(
                        "Can't load the extension: "+this.matchExtension);
		        }
		    }
		}
		
	}
}
