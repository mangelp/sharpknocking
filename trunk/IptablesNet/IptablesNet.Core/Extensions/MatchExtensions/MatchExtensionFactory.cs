
using System;
using System.IO;
using System.Reflection;

using Developer.Common.Types;

using IptablesNet.Core.Extensions;

namespace IptablesNet.Core.Extensions.ExtendedMatch
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
       	        Path.GetDirectoryName(asm.CodeBase);
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
            if(String.IsNullOrEmpty(typeName))
                throw new ArgumentException("The name for the type can't be null"+
                                            " or empty", "typeName");

      	    //The full name of each class must follow this convention:
       	    // IptablesNet.Extensions.Match.{EnumName}MatchExtension
       	    //So here we can build the name automatically
       	    string fullName = "IptablesNet.Extensions.Matches."+typeName+"MatchExtension";
			string customAsmName = assemblySearchPath+Path.DirectorySeparatorChar
					+"IptablesNet.Extensions."+typeName+"Matches.dll";
			string commonAsmName = assemblySearchPath+Path.DirectorySeparatorChar+
					"IptablesNet.Extensions.Matches.dll";
			return AssemblyUtil.TryLoadWithType(fullName, customAsmName, commonAsmName); 
       	    
//       	    //Search case insensitive but don't throw exceptions
//       	    Type theType = Type.GetType(fullName, false, true);
//       	    
//			//If it is not in this namespace try in his own namespace and
//			//assembly
//       	    if(theType==null) {
//				fullName = "IptablesNet.Extensions.Match."+typeName+
//					"MatchExtension";
//       	        string asmName = assemblySearchPath+Path.DirectorySeparatorChar+typeName+
//					"MatchExtension.dll";
//       	        
//       	        //Try to load an assembly with the name of the extension
//       	        Assembly asm = AssemblyUtil.TryLoadAssembly(asmName);
//				//If the assembly loads we try the fullname
//       	        if(asm!=null)
//       	            theType = asm.GetType(typeName, false, true);
//       	    }
//			
//			//Now try to find a MatchExtensions.dll in the same directory if the type
//			//is still missing. This library is supossed to contain the common set
//			//of match extensions
//			if( theType == null ) {
//				fullName = "IptablesNet.Extensions.Match."+typeName+
//					"MatchExtension";
//				string asmName = assemblySearchPath+Path.DirectorySeparatorChar+
//					"MatchExtensions.dll";
//				Assembly asm = AssemblyUtil.TryLoadAssembly(asmName);
//				
//				if(asm!=null)
//					theType = asm.GetType(typeName, false, true);
//			}
//       	    
//       	    return theType;
       	}
       	
       	/// <summary>
       	/// Returns an instance from the type
       	/// </summary>
       	public static MatchExtensionHandler GetExtension(Type extensionType)
       	{
			if(extensionType == null)
				throw new ArgumentNullException("extensionType", "The type can't be null");
       	    return (MatchExtensionHandler)Activator.CreateInstance(extensionType);    
       	}
       	
       	/// <summary>
       	/// Returns an instance of a class that manages a certain extension
       	/// target
       	/// </summary>
       	/// <remarks>
       	/// The type that is expected to be found is:<br/>
  	    /// IptablesNet.Core.Extensions.[EnumName]MatchExtension<br/>
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
       	        result = (MatchExtensionHandler)Activator.CreateInstance(theType);        
       	    
       	    return result;
       	}
    }
}
