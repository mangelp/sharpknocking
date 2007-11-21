// AttributeUtil.cs
//
//  Copyright (C)  2007 iSharpKnocking project
//  Created by Miguel Angel Perez Valencia, mangelp@gmail.com
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
