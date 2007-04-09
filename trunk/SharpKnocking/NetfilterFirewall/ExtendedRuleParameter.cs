
using System;

namespace SharpKnocking.IpTablesManager.RuleHandling
{
    /// <summary>
    /// Extended match parameter for a rule. It must be used in conjuntion
    /// with the -m extension module parameter explicitly to work well
    /// <summary>
	public class ExtendedRuleParameter
	{
	    private string name;
	    
	    /// <summary>
	    /// Name of the extended match parameter
	    /// <summary>
	    public string Name
	    {
	        get {return this.name;}
	        set {this.name = value;}
	    }
	    
	    public string text;
	    
	    /// <summary>
	    /// Value of the parameter
	    /// </sumary>
	    public string Text
	    {
	        get {return this.text;}
	        set {this.text = value;}
	    }
	    
	    private ArrayList extendedParameters;
	    
	    /// <summary>
	    /// Array of the extended parameters used
	    /// </summary>
	    public BaseExtension[] ExtendedParameters
	    {
	        get { return this.extendedParameters;}    
	    }
	    
		public ExtendedRuleParameter()
		{
		    
		}
		
		/// <summary>
		/// Adds a new parameter to the current extension.
		/// </summary>
		/// <remarks>
		/// The parameter input is a plain string 
		public void AddParameter(string value)
		{
		
		}
	}
}
