// MatchExtensionFactory.cs
//
//  Copyright (C) 2007 iSharpKnocking project
//  Created by mangelp<@>gmail[*]com
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA 
//
//

using System;
using System.IO;
using System.Reflection;

using NFSharp.Common.Types;

using NFSharp.Iptables.Core.Extensions;

namespace NFSharp.Iptables.Core.Extensions.Match
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
       	    //Set the search path as a subdirectory where this assembly is
       	    //located
       	    Assembly asm = Assembly.GetExecutingAssembly();
			//The search path is where the asembly is
       	    assemblySearchPath =
				Path.GetDirectoryName(asm.Location);
       	}
       	
       	/// <summmary>
       	/// Gets the type that implements the extension.
       	/// </summary>
       	/// <returns>
       	/// A reference to the type if it can find the class that implements
       	/// the extension or null otherwise.
       	/// </returns>
       	public static Type GetExtensionType(MatchExtension extType)
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
       	    // IptablesSharp.Extensions.Match.{EnumName}MatchExtension
       	    //So here we can build the name automatically
       	    string fullName = "NFSharp.Iptables.MatchExtensions."+typeName+"MatchExtension";
			string customAsmName = assemblySearchPath+Path.DirectorySeparatorChar
					+"IptablesSharp.Extensions."+typeName+"Matches.dll";
			string commonAsmName = assemblySearchPath+Path.DirectorySeparatorChar+
					"NFSharp.Iptables.MatchExtensions.dll";
			//Console.WriteLine ("Trying to load "+fullName+" from: \n- "+customAsmName+"\n- "+commonAsmName);
			return AssemblyUtil.TryLoadWithType(fullName, customAsmName, commonAsmName); 
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
  	    /// NFSharp.Iptables.Core.Extensions.[EnumName]MatchExtension<br/>
  	    /// If it is not found the assembly is expected to load with the name:<br/>
        /// [EnumName]Extension.dll<br/>
        /// In a subdirectory called extensions in the same directory where the
        /// assembly containing this class is.<br/>
        /// </remarks>
        /// <returns>
        /// The instance or null if the type and assembly weren't found.
        /// </returns>
       	public static MatchExtensionHandler GetExtension(MatchExtension mExtension)
       	{
       	    MatchExtensionHandler result = null;
       	    Type theType = MatchExtensionFactory.GetExtensionType(mExtension);
       	    
       	    if(theType!=null)
       	        result = (MatchExtensionHandler)Activator.CreateInstance(theType);        
       	    
       	    return result;
       	}
    }
}
