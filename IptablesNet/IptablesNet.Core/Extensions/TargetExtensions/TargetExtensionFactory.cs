
using System;
using System.IO;
using System.Reflection;

using SharpKnocking.Common;
using IptablesNet.Core;

namespace IptablesNet.Core.Extensions.ExtendedTarget
{
	/// <summary>
	/// Factory to build objects derived from TargetExtensionHandler
	/// </summary>
	public class TargetExtensionFactory
	{
	    private static TargetExtensionFactory factory
	                       = new TargetExtensionFactory();

        public static TargetExtensionFactory Instance
        {
           get { return TargetExtensionFactory.factory;}    
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
       	private TargetExtensionFactory()
       	{
       	    Type type = typeof(TargetExtensionFactory);
       	    TargetExtensionFactory.currentNamespace = type.Namespace;
       	    //Set the search path as a subdirectory where this assembly is
       	    //located
       	    Assembly asm = Assembly.GetExecutingAssembly();
       	    
       	    //TODO: ->mangelp. Having this hard-coded is a bad idea.
       	    assemblySearchPath =
       	        Path.GetDirectoryName(asm.CodeBase)+Path.DirectorySeparatorChar+"target_extensions";
       	}
       	
       	/// <summmary>
       	/// Gets the type that implements the extension.
       	/// </summary>
       	/// <returns>
       	/// A reference to the type if it can find the class that implements
       	/// the extension or null otherwise.
       	/// </returns>
       	public static Type GetExtensionType(TargetExtensions extType)
       	{
       	    return TargetExtensionFactory.GetExtensionType(extType.ToString());
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
            if(Net20.StringIsNullOrEmpty(typeName))
                throw new ArgumentException("The name of the type can't be null"+
                                            " or empty", "typeName");
            
       	    //The name of each class must follow this convention.
       	    // IptablesNet.Core.Extensions.{EnumName}TargetExtension
       	    //So here we can build the name automatically
       	    
       	    string fullName = TargetExtensionFactory.currentNamespace+
       	                "."+typeName+"TargetExtension";
       	    
       	    //Search case insensitive but don't throw exceptions
       	    Type theType = Type.GetType(fullName, false, true);
       	    
       	    if(theType==null)
       	    {
       	        string asmName = assemblySearchPath+
                                Path.DirectorySeparatorChar+typeName+
                                "TargetExtension.dll";
       	        
       	        Debug.VerboseWrite("TargetExtensionFactory: Can't find type: "+fullName);
       	        
       	        Debug.VerboseWrite(
       	             "TargetExtensionFactory:Triying to load assembly: "+asmName);
       	        
       	        //Try to load the assembly
       	        Assembly asm = Assembly.LoadFrom(asmName);
       	    
       	        if(asm!=null)
       	        {
       	            Debug.VerboseWrite("TargetExtensionFactory: Assembly loaded");
       	            theType = asm.GetType(typeName, false, true);
       	        }
       	        else
       	        {
       	            Debug.VerboseWrite("TargetExtensionFactory: Assembly not loaded :(");    
       	        }
       	    }
       	    
       	    return theType;
       	}
       	
       	/// <summary>
       	/// Returns an instance of an extension if the name matches any known
       	/// extension.
       	/// </summary>
       	/// <remarks>
       	/// The name is used to search the assembly for a type that implements
       	/// the extension.
       	/// </remarks>
       	public static TargetExtensionHandler GetExtension(string name)
       	{
       	    try
       	    {
       	        Type extensionType = TargetExtensionFactory.GetExtensionType(name);
       	        return (TargetExtensionHandler)Activator.CreateInstance(extensionType);
       	    }
       	    catch(Exception ex)
       	    {
       	        Debug.Write("TargetExtensionFactory.GetExtension: Exception while creating TargetExtensionHandler instance."+
       	                    "Returning null"+
       	                    "\n"+ex);
       	        return null;
       	    }
       	}
       	
       	/// <summary>
       	/// Returns an instance from the type.
       	/// </summary>
       	public static TargetExtensionHandler GetExtension(Type extensionType)
       	{
       	    return (TargetExtensionHandler)Activator.CreateInstance(extensionType);    
       	}
	}
}
