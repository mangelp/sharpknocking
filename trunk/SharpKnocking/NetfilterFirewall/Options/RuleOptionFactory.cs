    
using System;

using SharpKnocking.Common;
using SharpKnocking.NetfilterFirewall;

namespace SharpKnocking.NetfilterFirewall.Options
{
	
	
	public static class RuleOptionFactory
	{
	    
	    private static string currentNamespace;
	    
	    static RuleOptionFactory()
	    {
	        Type t = typeof(RuleOptionFactory);
	        currentNamespace = t.Namespace;
	    }
	    
	    public static bool TryGetOption(GenericParameter par,
	                                         out GenericOption opt,
	                                         out Exception ex)
        {
            ex = null;
            opt = null;
            
            try
            {
                opt = RuleOptionFactory.GetOption(par);
                return true;
            }
            catch(Exception tex)
            {
                ex = tex;
                return false;
            }
        }
        
        
		public static GenericOption GetOption(GenericParameter generic)
		{
		    RuleOptions type = GenericOption.GetOptionType(generic.Name);
		    
		    GenericOption opt=null;
		    
		    string typeName = currentNamespace+"."+type+"Option";
		    
		    Debug.VerboseWrite("RuleOptionFactory.GetOption: Loading Type name "+
		                             typeName, VerbosityLevels.Insane);
		    
		    Type objType = Type.GetType(typeName, true, false);
		    
		    Debug.VerboseWrite("RuleOptionFactory.GetOption: Creating object instance "
		                             , VerbosityLevels.Insane);
		    
		    opt = (GenericOption)Activator.CreateInstance(objType);
		    
		    try
		    {
		        Debug.VerboseWrite("RuleOptionFactory.GetOption: Setting value "+
		                                     generic.Value, VerbosityLevels.Insane);
		        
		        opt.Value = generic.Value;    
		    }
		    catch(Exception ex)
		    {
		        throw new InvalidOperationException("Can't build the option object"+
                             " from the generic option. The value set method has"+
                             " thrown an exception", ex);
		    }
		    
		    return opt;
		}
	}
}
