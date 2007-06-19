// /home/mangelp/Projects/sharpknocking/Extras/Developer.Common/Type/AttributeUtil.cs created with MonoDevelop at 14:34 14/06/2007 by mangelp 
//
//This project is released under the terms of the LGPL V2. See the file lgpl.txt for details.
//(c) 2007 SharpKnocking projects and authors (see AUTHORS).

using System;

namespace Developer.Common.Types
{
	
	
	public static class AttributeUtil
	{
		/// <summary>
        /// Returns the first custom attribute of a type
        /// </summary>
        /// <remarks>
        /// If the attribute isn't found returns null or the attribute if exists.
        /// </remarks>
        public static Attribute GetAttribute(Type attrType, object target)
        {
		    object[] attributes = target.GetType().GetCustomAttributes(true);
		    
		    foreach (Attribute attr in attributes)
		    {
		        if (attr.GetType() == attrType)
		        {
		            return attr;
		        }
		    }
		    
		    return null;
        }
	}
}
