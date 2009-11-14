
using System;

using SharpKnocking.Common;

namespace SharpKnocking.NetfilterFirewall
{
	
	/// <summary>
	/// Models a target for rules or parameters
	/// </summary>
	/// <remarks>
	/// The kind of things that can be allowed in a target depends in the
	/// place where the target is used.<br/>
	/// - Built in chain.<br/>
	/// - User defined chain.<br/>
	/// - Target extension <br/>
	/// </remarks>
	public class NetfilterTarget
	{
	    private string targetName;
	    
	    public string TargetName
	    {
	        get { return this.targetName;}
	        set { this.targetName = value;}
	    }
	    
	    
		public NetfilterTarget(string targetName)
		{
		}
		
		public static bool IsBuiltInTarget(string name)
		{
		    object obj;
		    
		    if(TypeUtil.IsAliasName(typeof(RuleTargets), name, out obj))
		        return true;
		    
		    return false;
		}
		
		public static bool IsBuiltInChain(string name)
		{
		    object obj;
		    
		    if(TypeUtil.IsAliasName(typeof(BuiltInChains), name, out obj))
		        return true;
		    
		    return false;
		}
	}
}
