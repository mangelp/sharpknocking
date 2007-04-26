
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using SharpKnocking.Common;

using IptablesNet.Core;
using IptablesNet.Core.Util;

namespace IptablesNet.Core.Extensions
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
	public abstract class ExtensionHandler<TParam>: IExtensionHandler
		where TParam:IExtensionParameter
	{
	    // ------------------------------------------------------------- //
	    // Instance properties and methods                               //
	    // ------------------------------------------------------------- //
			
	    private List<TParam> parameters;
	    
	    /// <summary>
	    /// Internal reference to the parameter list.
	    /// </summary>
	    /// <remarks>
	    /// Child classes can use this to manage parameters
	    /// </remakrs>
	    protected List<TParam> InternalList
	    {
	        get { return this.parameters;}    
	    }
	    
	    /// <summary>
	    /// Array with all the parameters used for this extension.
	    /// </summary>
	    public TParam[] Parameters
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
			
		private object handlerType;
			
		protected object HandlerType
		{
			get { return this.handlerType;}
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
	    protected ExtensionHandler(object handlerType, Type enumType)
	    {
			if(handlerType == null)
				throw new ArgumentNullException ("handlerType");
			
			if(enumType == null)
				throw new ArgumentNullException("enumType");
			
	        this.parameters = new List<TParam>();
			this.handlerType = handlerType;
	        this.extensionName = TypeUtil.GetDefaultAlias (this.handlerType);
			this.enumType = enumType;
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
		    TParam param = this.CreateParameter(name, value);
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
	    protected void AddParameterWithoutChecks(TParam param)
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
		
		private int IndexOf(string paramName)
		{
			for(int i=0;i<this.parameters.Count;i++)
			{
				if(this.parameters[i].IsAlias(paramName))
					return i;
			}
			
			return -1;
		}
		
		public bool IsSupportedParam (string paramName)
		{
			object val;
			return TypeUtil.IsAliasName (this.enumType, paramName, out val);
		}
            
		public virtual TParam CreateParameter(string paramType, string value)
        {
            TParam par = this.CreateParameter (paramType);
            if (par != null)
                par.SetValues (value);
            return par;
        }
	    
	    // ------------------------------------------------------------- //
	    // Abstract methods and properties                               //
	    // ------------------------------------------------------------- //
	    
		public abstract TParam CreateParameter(string paramType);
	}
}
