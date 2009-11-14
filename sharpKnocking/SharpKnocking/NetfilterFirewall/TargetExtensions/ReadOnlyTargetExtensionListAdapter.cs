
using System;
using SharpKnocking.Common;

using SharpKnocking.NetfilterFirewall;

namespace SharpKnocking.NetfilterFirewall.ExtendedTarget
{
	
	public class ReadOnlyTargetExtensionListAdapter
	{
		
        private TargetExtensionList adapted;
	    
	    /// <summary>
	    /// Returns the extension handler if it exists.
	    /// </summary>
	    /// <param name="extension">Enumeration constant that represents the
	    /// extension to return</param>
	    /// <returns>
	    /// The extension object if it exists or null if not
	    /// </returns>
	    public TargetExtensionHandler this[TargetExtensions extension]
	    {
	        get
	        {
	            return this.adapted[extension.ToString()];
	        }
	    }
	    
	    public TargetExtensionHandler this[string name]
	    {
	        get
	        {
	            return this.adapted[name];
	        }
	    }
	    
	    public TargetExtensionHandler this[int index]
	    {
	        get
	        {
	            return this.adapted[index];
	        }
	    }
	    
	    public int Count
	    {
	        get { return this.adapted.Count;}    
	    }
	    
		public ReadOnlyTargetExtensionListAdapter(TargetExtensionList adapted)
		{
		    this.adapted = adapted;
		}
	}
}
