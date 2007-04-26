
using System;

namespace IptablesNet.Core
{
	public abstract class NegableParameter: AbstractParameter
	{
	    private bool not;
	    
	    /// <summary>
	    /// Gets/sets if the meaning of the match is interpreted as the opposite.
	    /// This will not apply to every parameter.
	    /// </summary>
	    public bool Not
	    {
	        get { return this.not;}
	        set { this.not = value;}
	    }
		
		public NegableParameter()
			:base()
		{
		}
		
		/// <summary>
		/// Returns an string that represents the parameter that models this class.
		/// </summary>
		/// <remarks>
		/// This method uses only name and value (along with IsLongOption and Not
        /// properties values) to build the string.
        /// every class that extends this class should keep these fields updated
        /// to keep this working.
		/// </remarks>
		public override string ToString()
		{
		    string result = base.GetNameAsString();
			
			string value = this.GetValuesAsString ();
			
		    if(this.not)
		    {
		        if(String.IsNullOrEmpty(value))
		            result = "! "+result;
		        else
		            result += " ! ";
		    }
		    else if(!String.IsNullOrEmpty(value))
		    {
		        result+=" "+this.GetValuesAsString();
		    }
		    
		    return result;
		}
	}
}
