// IptablesOptionFactory.cs
//
//  Copyright (C) 2006 SharpKnocking project
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
using System;

using NFSharp.Iptables;

using NFSharp.Iptables.Parser.IptablesSaveFormat;

namespace NFSharp.Iptables.Core.Commands.Options
{
	
	/// <summary>
	/// Models a factory to build GenericOption derived objects.
	/// </summary>
	public static class IptablesOptionFactory
	{
	    
	    private static string currentNamespace;
	    
	    static IptablesOptionFactory()
	    {
	        Type t = typeof(IptablesOptionFactory);
	        currentNamespace = t.Namespace;
	    }
	    
	    public static bool TryGetOption(SimpleParameter par,
	                                         out GenericOption opt,
	                                         out Exception ex)
        {
            ex = null;
            opt = null;
            
            try {
                opt = IptablesOptionFactory.GetOption(par);
                return true;
            } catch(Exception tex) {
                ex = tex;
                return false;
            }
        }
        
        
		public static GenericOption GetOption(SimpleParameter generic)
		{
		    RuleOptions type = GenericOption.GetOptionType(generic.Name);
		    
		    GenericOption opt=null;
		    
		    string typeName = currentNamespace+"."+type+"Option";
		    
//		    Console.WriteLine("IptablesOptionFactory.GetOption: Loading Type name "+
//		                             typeName);
		    
		    Type objType = Type.GetType(typeName, true, false);
		    
//		    Console.WriteLine("IptablesOptionFactory.GetOption: Creating object instance ");
		    
		    opt = (GenericOption)Activator.CreateInstance(objType);
		    
//		    Console.WriteLine("IptablesOptionFactory.GetOption: Setting value "+
//		                                     generic.Value);
		    string errStr;
			                   
		    if (!opt.TryReadValues(generic.Value, out errStr))   
			{
				throw new InvalidOperationException("Can't build the option object"+
                        " from the generic option. Reason: "+errStr);
			}
		    
		    return opt;
		}
	}
}
