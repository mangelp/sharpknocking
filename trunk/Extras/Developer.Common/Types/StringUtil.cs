// /home/mangelp/Projects/sharpknocking/Extras/Developer.Common/Types/StringUtil.cs created with MonoDevelop at 15:39 14/06/2007 by mangelp 
//
//This project is released under the terms of the LGPL V2. See the file lgpl.txt for details.
//(c) 2007 SharpKnocking projects and authors (see AUTHORS).

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
