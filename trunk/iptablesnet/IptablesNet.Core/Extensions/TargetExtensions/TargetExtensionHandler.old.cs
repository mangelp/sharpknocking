
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using SharpKnocking.Common;

using IptablesNet.Core;
using IptablesNet.Core.Util;
using IptablesNet.Core.Extensions;

namespace IptablesNet.Core.Extensions.ExtendedTarget
{
    /// <summary>
	/// Base class for all the implementations of a target extension.
	/// </summary>
	/// <remarks>
	/// When extending this class the names of each class must
	/// follow this fully qualified scheme:<br/>
    /// IptablesNet.Core.ExtendedTarget.[EnumName]TargetExtension
    /// Where [EnumName] must be replaced by the name of the enum that
    /// represents the target extension type used if the extension is one included
    /// with iptables.<br/>
    /// Custom extensions are not supported by now so you can only implement
    /// those defined in the enumeration
    /// <see cref="T:SharpKnocking.IpTablesManager.RuleHandler.TargetExtensions"/>
    /// <br/><br/>
	/// </remarks>
	public abstract class TargetExtensionHandler:ExtensionHandler<TargetExtensionParameter>, IComparable<TargetExtensionHandler>
	{
	    // ------------------------------------------------------------- //
	    // Instance properties and methods                               //
	    // ------------------------------------------------------------- //
	    
	    private List<TargetExtensionParameter> parameters;
	    
	    /// <summary>
	    /// Internal reference to the parameter list.
	    /// </summary>
	    /// <remarks>
	    /// Child classes can use this to manage parameters
	    /// </remakrs>
	    protected List<TargetExtensionParameter> InternalList
	    {
	        get { return this.parameters;}    
	    }
	    
	    /// <summary>
	    /// Array with all the parameters used for this extension.
	    /// </summary>
	    public TargetExtensionParameter[] Parameters
	    {
	        get
	        {
	             return this.parameters.ToArray();    
	        }
	    }
	    
		//Type of the enumeration that defines the options available for
		//an extension
	    private Type enumType;

	    private string extensionName;
	    
		/// <summary>
		/// Gets the name for the extension
		/// </summary>
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
	    /// <param name="name"> Name for the extension.</param>
	    /// <remarks>
	    /// This constructor is protected as it is intended for use in the child 
		/// classes only, so each one must define their own public constructor.
	    /// </remarks>
	    protected TargetExtensionHandler(Type enumType, string name)
			:base(name, TargetExtensionHandler.optNameCache, enumType)
	    {
			if(String.IsNullOrEmpty(name))
				throw new ArgumentException ("The extension name can't be null or empty!","name");
			
			if(enumType == null)
				throw new ArgumentNullException("enumType");
			
			if(!TargetExtensionHandler.IsTargetExtension (name))
				throw new ArgumentException ("The extension name is not valid!","name");
			
	        this.parameters = new List<TargetExtensionParameter>();
	        this.enumType = enumType;
	        this.extensionName = name;
	    }
	    
	    // ------------------------------------------------------------- //
	    // Instance methods                                              //
	    // ------------------------------------------------------------- //
	    
	    /// <summary>
	    /// Checks if exists a parameter with the given name. When checking the names
		/// is also checked the aliases for each parameter.
	    /// </summary>
	    public virtual bool Contains(string name)
	    {
			if(this.IndexOf(name)>=0)
				return true;
			
			return false;
	    }

	    /// <summary>
	    /// Adds a parameter. The parameter is converted to other type if needed
	    /// so the original parameter is not added (a copy is added) in
	    /// the collection.
	    /// </summary>
	    /// <remarks>
	    /// Before adding first checks that is a valid parameter and then does
	    /// a check over the parameter to know if it matches the internal type
	    /// used by the extension for the parameters or not.<br/>
	    /// If the type doesn't match a new parameter instance is created and
	    /// then assigned to the internal collection.<br/>
	    /// Due to this it's a bad idea to keep a instance of the parameter added
	    /// and it's best to look for the parameter before touching it.
	    /// </remarks>
	    public virtual void AddParameter(string name, string value)
	    {
		    if(!this.IsValidName(name))
		    {
		        throw new ArgumentException(
                     "Invalid parameter '"+name+"' for extension "+
                     this.extensionName, "param");
		    }
		    
		    TargetExtensionParameter param = this.CreateParameter();
	        
			string errMsg=String.Empty;
			
			if(!param.TrySetType(name, out errMsg) ||
			   !param.TrySetValues(name, out errMsg))
			{
				throw new ArgumentException(
				                            "Invalid parameter value '"+
				                            value+"' for extension "+
				                            this.extensionName+". Reason: "+errMsg, 
				                            "param");
			}
//	        
//	        Debug.VerboseWrite("MatchExtensionHandler:"+this.extensionName
//	                     +": Adding option "+param, VerbosityLevels.Insane);
	        
	        this.AddParameterWithoutChecks(param);
	    }
	    
	    /// <summary>
	    /// Adds a parameter without checking if it is correct (correct name
        /// and correct value string format)
        /// </summary>
        /// <remarks>
        /// Child classes will need this to add internally created parameters
        /// </remarks>
	    protected void AddParameterWithoutChecks(TargetExtensionParameter param)
	    {	        
			int pos = this.IndexOf(param.GetDefaultAlias ());
			
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
	        int pos = this.IndexOf(name);
	        
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
	            //Debug.VerboseWrite("MatchExtensionHandler.IsValidName: False. Name null or empty");
	            return false;
	        }
	        
	        object obj;
	        
	        if(TypeUtil.IsAliasName(this.enumType, name, out obj))
	            return true;
	        
//	        Debug.VerboseWrite("MatchExtensionHandler.IsValidName('"+name+
//	              "'): False. Not an alias in "+this.enumType.Name);
	        return false;
	    }
	    
	    
	    /// <summary>
	    /// This validates the parameter and throws an exception if it isn't correct.
		/// It also returns the type for the parameter as a enumeration constant.
	    /// </summary>
	    public object ValidateAndGetType(string name)
	    {
        	object objType = null;
	        
	        if(!TypeUtil.IsAliasName(this.enumType, name, out objType))
	        {
	            throw new ArgumentException("The parameter name "+name+" is "+
                   " an invalid parameter for the "+this.extensionName+
                   " target extension");
	        }
	        
	        return objType;
	    }
	    
	    
	    /// <summary>
	    /// Returns true if the name can be converted to an option for the extension.
	    /// If not returns false.
	    /// </summary>
	    /// <param name="paramName"/> Name of the parameter </param>
	    /// <param name="objName"/> If the method returns true this is the name
	    /// converted to a enumeration constant.
	    /// </param>
	    public virtual bool TryConvertToName(string paramName, out object objName)
        {
            objName = null;
           
            if(this.IsValidName(paramName))
            {
                return TypeUtil.IsAliasName(this.enumType, paramName, out objName);    
            }
            
            return false;
        }
		
		
	    public override string ToString ()
	    {
            //Debug.VerboseWrite("TargetExtensionHandler: Converting to string "+this.parameters.Count+" options");
	        StringBuilder sb = new StringBuilder();
	        
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
	    
		public int CompareTo(TargetExtensionHandler handler)
		{
			return this.extensionName.CompareTo (handler.extensionName);
		}
		
		private int IndexOf(string paramName)
		{
			for(int i=0;i<this.parameters.Count;i++)
			{
				if(this.parameters[i].IsAlias(paramName))
					return i;
			}
			
			return -1;
		}
	    
	    // ------------------------------------------------------------- //
	    // Abstract methods and properties                               //
	    // ------------------------------------------------------------- //
	    
   	    /// <summary>
	    /// Returns an instance of a type that inherits from GenericParameter
	    /// but its extended to allow strict checking in the options.
	    /// </summary>
	    public abstract TargetExtensionParameter CreateParameter();
	    
	    /// <summary>
	    /// Returns an instance of a type that extends the class GenericParameter.
	    /// The instance is initialized using an existing parameter
	    /// </summary>
	    public abstract TargetExtensionParameter CreateParameter(string name, string value);
	    
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
	    public static bool IsTargetExtension(string paramName)
	    {
//            Debug.VerboseWrite("TargetExtensionHandler: Is target extension '"+
//                    paramName+"'?");
            
	        if(optNameCache.ContainsKey(paramName))
            {
                //Debug.VerboseWrite("TargetExtensionHandler: Yes");
	            return true;
            }
	        
            //Debug.VerboseWrite("TargetExtensionHandler: No");
	        return false;
	    }
	    
	    //cache for decoding names as MatchExtensions enum constants.
	    private static NameCache optNameCache;
	    
	    static TargetExtensionHandler()
	    {
	        //We are going to keep in memory the list of option names
	        //as the keys of the hashtable and the enum constant value
	        //as the value. This will speed up the search speed.
	        
	        optNameCache = new NameCache();
	        
	        optNameCache.FillFromEnum (typeof(TargetExtensions));
	    }
	}
}
