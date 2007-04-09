
using System;
using System.Collections;
using System.Text;

using SharpKnocking.Common;
using SharpKnocking.NetfilterFirewall;

namespace SharpKnocking.NetfilterFirewall.ExtendedMatch
{
	/// <summary>
	/// Base class for all the implementations of a match extension.
	/// </summary>
	/// <remarks>
	/// When extending this class the names of each class must
	/// follow this fully qualified scheme:<br/>
    /// SharpKnocking.NetfilterFirewall.ExtendedMatch.[EnumName]MatchExtension
    /// Where [EnumName] must be replaced by the name of the enum that
    /// represents the match extension type used if the extension is one included
    /// with iptables.<br/>
    /// Custom extensions are not supported by now so you can only implement
    /// those defined in the enumeration
    /// <see cref="T:SharpKnocking.IpTablesManager.RuleHandler.MatchExtensions"/>
    /// <br/><br/>
	/// </remarks>
	public abstract class MatchExtensionHandler
	{
	    // ------------------------------------------------------------- //
	    // Instance properties and methods                               //
	    // ------------------------------------------------------------- //
	    
	    private GenericParameterList parameters;
	    
	    /// <summary>
	    /// Internal reference to the parameter list.
	    /// </summary>
	    /// <remarks>
	    /// Child classes can use this to manage parameters
	    /// </remakrs>
	    protected GenericParameterList InternalList
	    {
	        get { return this.parameters;}    
	    }
	    
	    /// <summary>
	    /// Array with all the parameters used for this extension.
	    /// </summary>
	    public GenericParameter[] Parameters
	    {
	        get
	        {
	             return this.parameters.ToArray();    
	        }
	    }
	    
	    private Type enumType;
	    private string extensionName;
	    
	    public string ExtensionName
	    {
	       get { return this.extensionName;}    
	    }
	    
	    // ------------------------------------------------------------- //
	    // Constructors                                                  //
	    // ------------------------------------------------------------- //
	    
	    /// <summary>
	    /// Inits the instance with the values specified.
	    /// </summary>
	    /// <param name="enumType"> Type of the enumeration used for the options
	    /// that this extension supports</param>
	    /// <param name="name"> Name for the extension. This name has internal
	    /// use only. For client code use the MatchExtensions enumeration.
	    /// </param>
	    /// <remarks>
	    /// This constructor is for the child classes. Every one must define
	    /// the parameters.
	    /// </remarks>
	    protected MatchExtensionHandler(Type enumType, string name)
	    {
	        this.parameters = new GenericParameterList();
	        this.enumType = enumType;
	        this.extensionName = name;
	    }
	    
	    // ------------------------------------------------------------- //
	    // Instance methods                                              //
	    // ------------------------------------------------------------- //
	    
	    /// <summary>
	    /// Checks if exists a parameter
	    /// </summary>
	    public virtual bool Contains(string name)
	    {
	        if(this.parameters.ContainsName(name))
	        {
	            return true;
	        }
	        
	        return false;
	    }
	    

	    /// <summary>
	    /// Adds a parameter. The parameter is converted to other type if needed
	    /// so the original parameter can be added or not (a copy is added) in
	    /// the collection.
	    /// </summary>
	    /// <remarks>
	    /// Before adding first checks that is a valid parameter and then does
	    /// a check over the parameter to know if it matches the internal type
	    /// used by the extension for the parameters or not.<br/>
	    /// If the type doesn't match a new parameter instance is created and
	    /// then assigned to the internal collection.<br/>
	    /// Due to this it's a bad idea to keep a instance of the parameter added
	    /// and it's best to look for the parameter before modifying it.
	    /// </remarks>
	    public virtual void AddParameter(string name, string value)
	    {
		    if(!this.IsValidName(name))
		    {
		        throw new ArgumentException(
                     "AddParameter: Invalid parameter '"+name+"' for extension "+
                     this.extensionName, "param");
		    }
		    else if(!this.IsValidValue(name, value))
		    {
		        throw new ArgumentException(
                     "AddParameter: Invalid parameter value '"+value
                     +"' for extension "+this.extensionName, "param");
		    }
		    
		    MatchExtensionParameter param = this.CreateParameter();
	        param.Name = name;
	        param.Value = value;
	        param.IsLongOption = name.Length>1;
	        
	        Debug.VerboseWrite("MatchExtensionHandler:"+this.extensionName
	                     +": Adding option "+param, VerbosityLevels.Insane);
	        
	        this.AddParameterWithoutChecks(param);
	    }
	    
	    /// <summary>
	    /// Adds a parameter without checking if it is correct (correct name
        /// and correct value string format)
        /// </summary>
        /// <remarks>
        /// Child classes will need this to add internally created parameters
        /// </remarks>
	    protected void AddParameterWithoutChecks(GenericParameter param)
	    {
	        int pos = this.parameters.IndexOf(param.Name);
	        
	        if(pos>=0)
	        {
	            throw new InvalidOperationException(
                             "Can't add the parameter: "+
                             param+
                             " because it is already in the collection");
	        }

	        this.parameters.Add(param);
	    }
	    
	    /// <summary>
	    /// Removes a parameter from the extension.
	    /// </summary>
	    public virtual void RemoveParameter(string name)
	    {
	        int pos = this.parameters.IndexOf(name);
	        
	        if(pos>=0)
	        {
	            this.parameters.RemoveAt(pos);
	        }
	        else
	        {
	            throw new InvalidOperationException(
                    "There is no parameter named: "+name+" in the extension.");
	        }
	    }
	    
	    /// <summary>
	    /// Checks if the name is a valid parameter name for this extension
	    /// </summary>
	    public bool IsValidName(string name)
	    {
	        if(Net20.StringIsNullOrEmpty(name))
	        {
	            Debug.VerboseWrite("MatchExtensionHandler.IsValidName: False. Name null or empty");
	            return false;
	        }
	        
	        object obj;
	        
	        if(TypeUtil.IsAliasName(this.enumType, name, out obj))
	            return true;
	        
	        Debug.VerboseWrite("MatchExtensionHandler.IsValidName: False. Not an alias");
	        return false;
	    }
	    
	    /// <summary>
	    /// Checks if the name is a valid parameter name and then it checks
	    /// the value.
	    /// </summary>
	    public bool IsValidValue(string paramName, string value)
	    {
	        if(!this.IsValidName(paramName))
	            return false;
	        
	        object objValue;
	        
	        if(!this.TryConvertToValue(paramName, value, out objValue))
	            return false;
	        
	        return true;
	    }
	    
	    
	    // ------------------------------------------------------------- //
	    // Abstract methods and properties                               //
	    // ------------------------------------------------------------- //
	    
	    /// <summary>
	    /// Returns true if the option has the correct type and is one of the
	    /// options for the extension.
	    /// </summary>
	    public abstract bool IsValidName(object option);
	    
	    /// <summary>
	    /// Returns true if the option and the value are valid (the value must
        /// be a valid value for the option).
        /// </return>
	    public abstract bool IsValidValue(object option, object value);
	    
	    
	    /// <summary>
	    /// Returns true if the value can be converted to the type that the
	    /// parameter uses internally for his values.
	    /// If not returns false.
	    /// </summary>
	    /// <remarks>
	    /// If the method returns true the parameter objValue is set to the
	    /// value converted to the type used by the parameter.
	    /// If not this value is set to some random value and must not be used
	    /// </remarks>
	    public abstract bool TryConvertToValue(
            string name,
            string value,
            out object objValue);
	    
	    /// <summary>
	    /// Returns true if the name can be converted to an option for the extension.
	    /// If not returns false.
	    /// </summary>
	    /// <param name="paramName"/> Name of the parameter </param>
	    /// <param name="objName"/> If the method returns true this is the name
	    /// converted to a enumeration constant.
	    /// </param>
	    public abstract bool TryConvertToName(
            string paramName,
            out object objName);
	    
   	    /// <summary>
	    /// Returns an instance of a type that inherits from GenericParameter
	    /// but its extended to allow strict checking in the options.
	    /// </summary>
	    public abstract MatchExtensionParameter CreateParameter();
	    
	    /// <summary>
	    /// Returns an instance of a type that extends the class GenericParameter.
	    /// The instance is initialized using an existing parameter
	    /// </summary>
	    public abstract MatchExtensionParameter CreateParameter(string name, string value);
	    
	    /// <summary>
	    /// Returns the type that extends GenericParameter for this extension
	    /// </summary>
	    public abstract Type GetInternalParameterType();
	    
	    // ------------------------------------------------------------- //
	    // Static method and properties                                  //
	    // ------------------------------------------------------------- //
	    
	    /// <summary>
	    /// Returns if the parameter name matches any extension name alias
	    /// </summary>
	    public static bool IsMatchExtension(string paramName)
	    {
	        if(optNameCache.ContainsKey(paramName))
	            return true;
	        
	        return false;
	    }
	    
	    public override string ToString ()
	    {
	        StringBuilder sb = new StringBuilder();
	        
	        //The name of the extension is not printed because if there
	        //is implicit loading or the -m option have been specified
	        //we will get duplicated options.
	        
	        //This won't hurt anyway. Uncomment if needed
	        //sb.Append("-m "+this.extensionName);
	        bool first = true;
	        
	        for(int i=0;i<this.parameters.Count;i++)
	        {
	            if(!first)
	                sb.Append(" ");
	            else
	                first=false;
	            
	            sb.Append(this.parameters[i]);
	        }
	        
	        return sb.ToString();
	    }

	    
	    //cache for decoding names as MatchExtensions enum constants.
	    private static Hashtable optNameCache;
	    
	    static MatchExtensionHandler()
	    {
	        //We are going to keep in memory the list of option names
	        //as the keys of the hashtable and the enum constant value
	        //as the value. This will speed up the search speed.
	        
	        optNameCache = new Hashtable();
	        
	        Array arr = Enum.GetValues(typeof(MatchExtensions));
	        string[] aliases;
	        
	        foreach(object obj in arr)
	        {
	            aliases = TypeUtil.GetAliases(obj);
	            
	            for(int i=0;i<aliases.Length;i++)
	            {
	                optNameCache.Add(aliases[i], obj);
	            }
	        }
	    }
	  
	}
}
