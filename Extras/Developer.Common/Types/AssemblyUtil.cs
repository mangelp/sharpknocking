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
using System.Reflection;
using System.Collections.Generic;

namespace Developer.Common.Types
{
	public static class AssemblyUtil
	{
		private static Dictionary<string, Assembly> cache;
		
		static AssemblyUtil()
		{
			cache = new Dictionary<string, Assembly>();
		}
		
		public static Assembly TryLoadAssembly(string asmName)
		{
			if(cache.ContainsKey(asmName))
			{
				//Console.WriteLine("Assembly found in cache: "+asmName);
				return cache[asmName];
			}
			Assembly asm=null;
			
			//Try to load an assembly with the name
			try {
				//Console.WriteLine("Trying to load assembly: "+asmName);
				asm = Assembly.LoadFrom(asmName);
				cache.Add(asmName, asm);
				//Console.WriteLine("Assembly loaded");
			} catch(Exception ex) {
				//Console.WriteLine("Can't load assembly. Reason: "+ex.Message);
				asm = null;
			}
			
			return asm;
		}
		
		public static Type TryLoadWithType(string fullTypeName, params string[] assemblies)
		{
			//Console.WriteLine("Loading type: "+fullTypeName+", from "+(assemblies.Length+1));
			Type result = Type.GetType(fullTypeName, false, true);
			Assembly asm = null;
			int pos = 0;
			
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
