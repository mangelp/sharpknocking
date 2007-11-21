// StringUtil.cs
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
using System.Collections.Generic;

namespace Developer.Common.Types
{
	
	
	public static class StringUtil
	{
		/// <summary>
	    /// Splits the input string removing empty strings if specified.
	    /// </summary>
	    public static string[] Split(string input,
                                       bool removeEmpty,
                                       params char[] chars)
	    {
	        //Normal split
	        string[] result = input.Split(chars);
	        
	        if(!removeEmpty)
	            return result;
	        
	        List<string> gResult = new List<string>(); 
	        
	        //Add to the array list only those not null
	        for(int i=0;i<result.Length;i++)
	        {
	            if(!String.IsNullOrEmpty(result[i]))
	                gResult.Add(result[i]);
	        }
	        
	        //Create the result split array and set the items
	        result = new string[gResult.Count];
	        
	        for(int i=0;i<gResult.Count;i++)
	        {
	            result[i]=(string)gResult[i];    
	        }
	        
	        return result;
	    }
		
		/// <summary>
	    /// Compares two strings. It can be a case sensitive comparation or
	    /// not.
	    /// </summary>
	    public static bool Equals(string a, string b, bool caseInsensitive)
	    {
	        if(a==null && b!=null)
	        {
	            return false;
	        }
	        else if(a!=null && b==null)
	        {
	            return false;
	        }
	        else if(a==null && b== null)
	        {
	            return true;
	        }
	        else if(caseInsensitive)
	        {
	            a = a.ToLower();
	            b = b.ToLower();
	        }
	        
	        return a==b;
	    }
	}
}
