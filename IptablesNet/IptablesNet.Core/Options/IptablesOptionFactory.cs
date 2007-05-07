    
using System;

using SharpKnocking.Common;
using IptablesNet.Core;

namespace IptablesNet.Core.Options
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
            
            try
            {
                opt = IptablesOptionFactory.GetOption(par);
                return true;
            }
            catch(Exception tex)
            {
                ex = tex;
                return false;
            }
        }
        
        
		public static GenericOption GetOption(SimpleParameter generic)
		{
		    RuleOptions type = GenericOption.GetOptionType(generic.Name);
		    
		    GenericOption opt=null;
		    
		    string typeName = currentNamespace+"."+type+"Option";
		    
//		    Debug.VerboseWrite("IptablesOptionFactory.GetOption: Loading Type name "+
//		                             typeName, VerbosityLevels.Insane);
		    
		    Type objType = Type.GetType(typeName, true, false);
		    
//		    Debug.VerboseWrite("IptablesOptionFactory.GetOption: Creating object instance "
//		                             , VerbosityLevels.Insane);
		    
		    opt = (GenericOption)Activator.CreateInstance(objType);
		    
//		    Debug.VerboseWrite("IptablesOptionFactory.GetOption: Setting value "+
//		                                     generic.Value, VerbosityLevels.Insane);
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
