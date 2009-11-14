// AssemblyUtil.cs
//
//  Copyright (C) 2007 iSharpKnocking project
//  Created by Miguel Angel Pérez, mangelp@gmail.com
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
using System.Collections.Generic;

namespace Developer.Common.Types
{
	/// <summary>
	/// Utility attributes when working with assemblies
	/// </summary>
	public static class AssemblyUtil
	{
		private static Dictionary<string, Assembly> cache;
		
		/// <summary>
		/// Static constructor
		/// </summary>
		static AssemblyUtil()
		{
			cache = new Dictionary<string, Assembly>();
		}
		
		/// <summary>
		/// Tries to load an assembly
		/// </summary>
		/// <param name="asmName">
		/// A <see cref="System.String"/> assembly path and name
		/// </param>
		/// <returns>
		/// A <see cref="Assembly"/> if the assembly file was found or null if not
		/// </returns>
		public static Assembly TryLoadAssembly(string asmName)
		{
			if(cache.ContainsKey(asmName))
			{
				//Console.WriteLine("Assembly found in cache: "+asmName);
				return cache[asmName];
			} 
//			else if (!File.Exists(asmName)) 
//			{
//				Console.WriteLine ("Assembly file not found: "+asmName);
//				Console.WriteLine ("Current Directory: "+Directory.GetCurrentDirectory());
//				if(!Directory.Exists(Path.GetDirectoryName(asmName)))
//					Console.WriteLine ("The directory does not exists");
//				else
//					Console.WriteLine ("The file doesn't exists");
//				return null;
//			}
			
			Assembly asm=null;
			
			if(!File.Exists(asmName))
			{
				//Console.WriteLine ("Assembly file not found: "+asmName);
				return asm;
			}
			
			//Try to load an assembly with the name
			try {
				//Console.WriteLine("Trying to load assembly: "+asmName);
				asm = Assembly.LoadFrom(asmName);
				cache.Add(asmName, asm);
				//Console.WriteLine("Assembly loaded");
			} catch(Exception ex) {
				Console.WriteLine("Can't load assembly. Reason: "+ex.Message);
				asm = null;
			}
			
			return asm;
		}
		
		/// <summary>
		/// Tries to load a type from a list of assemblies
		/// </summary>
		/// <param name="fullTypeName">
		/// A <see cref="System.String"/> full name of the type
		/// </param>
		/// <param name="assemblies">
		/// A <see cref="System.String"/> list of assemblies to search the type
		/// </param>
		/// <returns>
		/// A <see cref="Type"/> if it was found in an assembly or null if not
		/// </returns>
		public static Type TryLoadWithType(string fullTypeName, params string[] assemblies)
		{
			//Console.WriteLine("Loading type: "+fullTypeName+", from "+(assemblies.Length+1));
			Type result = Type.GetType(fullTypeName, false, true);
			Assembly asm = null;
			int pos = 0;
			//While the type is not found go throught the array
			while(result==null && pos<assemblies.Length)
			{
				asm = TryLoadAssembly(assemblies[pos]);
				if( asm != null )
					result = asm.GetType(fullTypeName, false, true);
				pos = pos + 1;
			}
			
			return result;
		}
	}
}
