
using System;
using System.Reflection;
using System.IO;

using SharpKnocking.Common;

namespace SharpKnocking.NetfilterFirewall.ExtendedMatch
{
    /// <summary>
    /// Defines a factory that creates instances of objects that extends the base
    /// class MatchExtensionHandler.
    /// </summary>
    public class MatchExtensionFactory
	{
	    private static MatchExtensionFactory factory
	                       = new MatchExtensionFactory();

        public static MatchExtensionFactory Instance
        {
           get { return MatchExtensionFactory.factory;}    
        }
        
        private static string currentNamespace;
	    
	    private static string assemblySearchPath;
	    
	    public static string AssemblySearchPath
	    {
	        get
	        {
                return assemblySearchPath;
	        }
	    }
	    
	    /// <summary>
	    /// Default private constructor.
	    /// </summary>
       	private MatchExtensionFactory()
       	{
       	    Type type = typeof(MatchExtensionFactory);
       	    MatchExtensionFactory.currentNamespace = type.Namespace;
       	    //Set the search path as a subdirectory where this assembly is
       	    //located
       	    Assembly asm = Assembly.GetExecutingAssembly();
       	    //TODO: ->mangelp. Having this hard-coded is a bad idea.
       	    assemblySearchPath =
       	        Path.GetDirectoryName(asm.CodeBase)+Path.DirectorySeparatorChar+"match_extensions";
       	}
       	
       	/// <summmary>
       	/// Gets the type that implements the extension.
       	/// </summary>
       	/// <returns>
       	/// A reference to the type if it can find the class that implements
       	/// the extension or null otherwise.
       	/// </returns>
       	public static Type GetExtensionType(MatchExtensions extType)
       	{
       	    return MatchExtensionFactory.GetExtensionType(extType.ToString());
       	}
       	
       	/// <summary>
       	/// Gets the type that implements the extension.
       	/// </summary>
       	/// <param name="typeName>Name of the extension. This must be the same
       	/// name that is used to load the extension with the -m option. It is
       	/// case-insensitive for the current implementation of this method.
       	/// </param>
       	/// <returns>
       	/// A reference to the type if it can find the class that implements
       	/// the extension or null otherwise.
       	/// </returns>
       	public static Type GetExtensionType(string typeName)
       	{
            Debug.VerboseWrite("Calling MatchExtension constructor for type '"+
                               typeName+"'");
            
            if(Net20.StringIsNullOrEmpty(typeName))
                throw new ArgumentException("The name for the type can't be null"+
                                            " or empty", "typeName");
            
       	    //The name of each class must follow this convention.
       	    // SharpKnocking.NetfilterFirewall.Extensions.{EnumName}MatchExtension
       	    //So here we can build the name automatically
       	    string fullName = MatchExtensionFactory.currentNamespace+
       	                "."+typeName+"MatchExtension";
       	    
       	    //Search case insensitive but don't throw exceptions
       	    Type theType = Type.GetType(fullName, false, true);
       	    
       	    if(theType==null)
       	    {
       	        string asmName = assemblySearchPath+
                                Path.DirectorySeparatorChar+typeName+
                                "MatchExtension.dll";
       	        
       	        Debug.VerboseWrite("MatchExtensionFactory: Can't find type: "+fullName);
       	        
       	        Debug.VerboseWrite(
       	             "MatchExtensionFactory:Triying to load assembly: "+asmName);
       	        
       	        //Try to load the assembly
       	        Assembly asm = Assembly.LoadFrom(asmName);
       	    

       	        if(asm!=null)
       	        {
       	            Debug.VerboseWrite("MatchExtensionFactory: Assembly loaded");
       	            theType = asm.GetType(typeName, false, true);
       	        }
       	        else
       	        {
       	            Debug.VerboseWrite("MatchExtensionFactory: Assembly not loaded :(");    
       	        }
       	    }
       	    
       	    return theType;
       	}
       	
       	/// <summary>
       	/// Returns an instance from the type
       	/// </summary>
       	public static MatchExtensionHandler GetExtension(Type extensionType)
       	{
       	    return (MatchExtensionHandler)Activator.CreateInstance(extensionType);    
       	}
       	
       	/// <summary>
       	/// Returns an instance of a class that manages a certain extension
       	/// target
       	/// </summary>
       	/// <remarks>
       	/// The type that is expected to be found is:<br/>
  	    /// SharpKnocking.NetfilterFirewall.Extensions.[EnumName]MatchExtension<br/>
  	    /// If it is not found the assembly is expected to load with the name:<br/>
        /// [EnumName]Extension.dll<br/>
        /// In a subdirectory called extensions in the same directory where the
        /// assembly containing this class is.<br/>
        /// </remarks>
        /// <returns>
        /// The instance or null if the type and assembly weren't found.
        /// </returns>
       	public static MatchExtensionHandler GetExtension(MatchExtensions mExtension)
       	{
       	    MatchExtensionHandler result = null;
       	    
       	    Type theType = MatchExtensionFactory.GetExtensionType(mExtension);
       	    
       	    if(theType!=null)
       	    {
       	        result =
       	            (MatchExtensionHandler)Activator.CreateInstance(theType);        
       	    }
       	    else
       	    {
       	        Debug.Write("MatchExtensionFactory: The match extension '"+
       	                    mExtension.ToString().ToLower()+"' can't be loaded");
       	    }
       	    
       	    return result;
       	}
    }
}
