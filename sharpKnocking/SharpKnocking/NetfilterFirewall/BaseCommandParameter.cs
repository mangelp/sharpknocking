
using System;

namespace SharpKnocking.IpTablesManager.RuleHandling
{
	
	/// <summary>
	/// Base class for command parameters. It models the basic information
	/// and methods for a parameter that can be used in the command-line
	/// call.
	/// </summary>
	public class BaseCommandParameter
	{
		
	    private string text;
	    
	    /// <summary>
	    /// Parameter value as string
	    /// </summary>
	    public string Text
	    {
	        get {return this.text;}
	        set
	        {
	            this.text = value;
	        }
	    }
	    
	    private string name;
	    
	    /// <summary>
	    /// Parameter name without '-' characters
	    /// </summary>
	    public string Name
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
	    public bool IsLongOption
	    {
	        get {return this.isLongOption;}
	        set {this.isLongOption = value;}
	    }
	    
	    public BaseCommandParameter()
	    {}
	    
		public BaseCommandParameter(string name, string text, bool isLongOption)
		{
		    this.isLongOption = isLongOption;
		    this.name = name;
		    this.text = text;
		}
		
		/// <summary>
		/// Init the parameter from the string
		/// </summary>
		public virtual void ParseString(string param)
		{
		    //default
		    this.text = param;
		}
		
		/// <summary>
		/// Convert the parameter to the string that can be used to append
		/// to a command.
		/// </summary>
		public override string ToString()
		{
		    string result = this.name+" "+this.text;
		    
		    if(this.isLongOption)
		        result = "-"+result;
		    else
		        result = "--"+result;
		    
		    return result;
		}
	
	}
}
