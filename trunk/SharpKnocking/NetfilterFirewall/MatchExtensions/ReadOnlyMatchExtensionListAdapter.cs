
using System;
using System.Collections;

using SharpKnocking.NetfilterFirewall;

namespace SharpKnocking.NetfilterFirewall.ExtendedMatch
{
	
	
	public class ReadOnlyMatchExtensionListAdapter
	{
	    private MatchExtensionList adapted;
	    
	    /// <summary>
	    /// Returns the extension handler if it exists.
	    /// </summary>
	    /// <param name="extension">Enumeration constant that represents the
	    /// extension to return</param>
	    /// <returns>
	    /// The extension object if it exists or null if not
	    /// </returns>
	    public MatchExtensionHandler this[MatchExtensions extension]
	    {
	        get
	        {
	            return this.adapted[extension.ToString()];
	        }
	    }
	    
	    public MatchExtensionHandler this[string name]
	    {
	        get
	        {
	            return this.adapted[name];
	        }
	    }
	    
	    public MatchExtensionHandler this[int index]
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
	    
		public ReadOnlyMatchExtensionListAdapter(MatchExtensionList adapted)
		{
		    this.adapted = adapted;
		}
	}
}
