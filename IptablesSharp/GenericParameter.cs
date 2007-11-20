
using System;

using SharpKnocking.Common;

namespace IptablesNet.Core
{
    
    /// <summary>
    /// Models a generic parameter with the common properties needed.
    /// </summary>
    /// <remarks>
    /// The GetHashCode method is overriden to return the hash code of the name.
    /// So when using hashtables two parameters with the same name will be the
    /// same parameter.<br/>
    /// The Equals method is overriden to return true when the names and the values
    /// are equal and false otherwise.<br/>
    /// </remarks>
	public class GenericParameter: AbstractParameter
	{
	    private TValue value;
	    
	    /// <summary>
	    /// Parameter value as string.
	    /// Currently the number of long format parameters are limited.
	    /// </summary>
	    public virtual TValue Value
	    {
	        get {return this.value;}
	        set
	        {
				this.OnValueSet(value);
	            this.value = value;
	        }
	    }
		
		private TId name;
		
		/// <summary>
		/// Name of the parameter.
		/// </summary>
		/// <remarks>
		/// Parameters with different names can be the same if they are aliases
		/// see <c>IsAlias(string)</c>
		/// Before the name is set further procesing can be done in the <c>OnNameSet</c>
		/// method like aditional checks.
		/// </remarks>
		public TName Name
		{
			get { return this.name;}
			set { 
				this.OnNameSet (value);
				this.name = value;
			}
		}
	    
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
	    
	    /// <summary>
	    /// Parametrized constructor.
	    /// </summary>
		public GenericParameter(TName name, TValue value)
		{
		    //This also sets the isLongOption field
		    this.Name = name;
		    //If this property is overriden this will ensure correct initialization
		    this.Value = value;
		}
		
	    /// <summary>
	    /// Default constructor.
	    /// </summary>		
		public GenericParameter()
		{
		    
		}
		
		protected virtual void OnValueSet (TValue value)
		{
			
		}
		
		protected virtual void OnNameSet (TName name)
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
		    string result = base.ToString();
			
		    if(this.not)
		    {
		        if(Net20.StringIsNullOrEmpty(this.value))
		            result = "! "+result;
		        else
		            result += " ! ";
		    }
		    else if(!Net20.StringIsNullOrEmpty(this.value))
		    {
		        result+=" "+this.GetValueString();
		    }
		    
		    return result;
		}
				
		public override bool IsAlias (string name)
		{
			return true;
		}

		public override string GetDefaultAlias ()
		{
			return this.name.ToString();
		}
		
		public virtual string GetValueString()
		{
			return this.value+"";
		}
		
		/// <summary>
	    /// Gets if the string starts with two '-' characters.
	    /// </summary>
	    /// <returns>
	    /// True if the string have at least two characters and those
	    /// characters are '-'. In any other way it returns false.
	    /// </returns>
	    public static bool CheckLongFormat(string option)
	    {
	        bool result=false;
	        
	        if(option!=null && option.Length>=2 &&
	                    option[0]=='-' && option[1]=='-')
            {
                result = true;
            }
            
            return result;
	    }

	}
}
