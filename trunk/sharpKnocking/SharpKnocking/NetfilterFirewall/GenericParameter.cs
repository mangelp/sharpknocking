
using System;

using SharpKnocking.Common;

namespace SharpKnocking.NetfilterFirewall
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
	public class GenericParameter: IComparable
	{
	    private string name;
	    
	    /// <summary>
	    /// Parameter value as string.
	    /// Currently the number of long format parameters are limited.
	    /// </summary>
	    public virtual string Name
	    {
	        get {return this.name;}
	        set
	        {            
	            if(Net20.StringIsNullOrEmpty(value))
	            {
	                throw new ArgumentException("The name can't be null or emtpy");  
	            }
	            
	            this.name = value;
	        }
	    }

	    
//	    /// <summary>
//	    /// Gets/sets an object that identifies the concrete type of the parameter.
//	    /// </summary>
//	    /// <remarks>
//	    /// This property is by default set to null.<br/>
//	    /// This property is mainly useful for estoring the type of the parameter
//	    /// on it, so child classes can set this to make code more consistent and
//	    /// not being evaluating strings continuously.<br/><br/>
//	    /// This property must set the Name to keep the base class consistent.
//	    /// </remarks>
//	    public virtual object TypeTag
//	    {
//	        get { return typeTag;}
//	        set { typeTag = value;}
//	    }
//	    
//	    private object valueTag=null;
//	    
//	    /// <summary>
//	    /// Gets/Sets aditional value information.
//	    /// </summary>
//	    /// <remarks>
//	    /// When developing extensions is better to handle typed objects than
//	    /// building custom strings for the value, so this property allows
//	    /// child classes to work with typed values and let other work aganist
//	    /// them throught this base implementation.<br/><br/>
//	    /// This property must set the Vaue to keep the base class consistent.
//	    /// </remarks>
//	    public virtual object ValueTag
//	    {
//	        get { return valueTag; }
//	        set { valueTag = value;}
//	    }
	    
	    private string value;
	    
	    /// <summary>
	    /// Parameter value as string.
	    /// Currently the number of long format parameters are limited.
	    /// </summary>
	    public virtual string Value
	    {
	        get {return this.value;}
	        set
	        {
	            this.value = value;
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
	    
	    private bool isLongOption;
	    
	    /// <summary>
	    /// Gets if the parameter is a long format paramerter or not.
	    /// </summary>
	    /// <remarks>
	    /// The long options are prefixed with two '-' characters and the short
	    /// ones with only one '-' character.
	    /// </remarks>
	    public bool IsLongOption
	    {
	        get {return this.isLongOption;}
	        set {this.isLongOption = value;}
	    }
	    
	    /// <summary>
	    /// Parametrized constructor.
	    /// </summary>
		public GenericParameter(string name, string value)
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
		
		/// <summary>
		/// Returns an string that represents the parameter that models this class.
		/// </summary>
		/// <remarks>
		/// This method uses only name and value (along with IsLongOption and Not
        /// properties values) to build the string.
        /// every class that extends this class should keep these fields updated
        /// to keep this working.
		public override string ToString()
		{
		    string result = String.Empty;
		    
		    if(this.isLongOption)
		        result = "--"+this.name;
		    else
		        result = "-"+this.name;
		    
		    if(this.not)
		    {
		        if(Net20.StringIsNullOrEmpty(this.value))
		            result = "! "+result;
		        else
		            result += " ! ";
		    }
		    else if(!Net20.StringIsNullOrEmpty(this.value))
		    {
		        result+=" "+this.value;
		    }
		    
		    return result;
		}
		
		public int CompareTo(object obj)
		{
		    if(!(obj is GenericParameter))
		    {
		        throw new InvalidOperationException("Can't only compare with"+
		                                            " objects of the same type");
		    }
		    
		    GenericParameter gp = (GenericParameter)obj;
		    
		    //The result depends only in the name of the parameters
		    return this.name.CompareTo(gp.name);
		}
		
		public override bool Equals (object o)
		{
		    if(!(o is GenericParameter))
		        return false;
		    
		    GenericParameter gp = (GenericParameter)o;
		    
		    if(this.name != gp.Name || this.value != gp.Value)
		    {
		        return false;
		    }
		    
		    return true;
		}
		
		/// <summary>
		/// Returns the hash code for this parameter based in the name of it.
		/// </summary>
		public override int GetHashCode ()
		{
			return this.name.GetHashCode();
		}
		
		/// <summary>
	    /// Gets if the string starts with two '-' characters.
	    /// </summary>
	    /// <returns>
	    /// True if the string have at least two characters and those
	    /// characters are '-'. In any other way it returns false.
	    /// </returns>
	    public static bool CheckLongOption(string option)
	    {
	        bool result=false;
	        
	        if(option!=null && option.Length>=2 &&
	                    option[0]=='-' && option[1]=='-')
            {
                result = true;
            }
            
            Debug.VerboseWrite("Is long option: '"+option+"' ? "+result);
            
            return result;
	    }

	}
}
