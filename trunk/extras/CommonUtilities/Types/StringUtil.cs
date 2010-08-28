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
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace CommonUtilities.Types
{
	
	/// <summary>
	/// Utility methods to operate with strings
	/// </summary>
	public static class StringUtil
	{
		/// <summary>
	    /// Splits the input string removing empty strings if specified.
	    /// </summary>
		//[Obsolete("Use String.Split instead")]
	    public static string[] Split(string input, bool removeEmpty, params char[] chars)
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
	    /// Breaks a string into some strings using the spaces as separators
	    /// </summary>
	    public static List<string> BreakCommandLine(ref string input)
	    {
			List<string> gResult = new List<string>();
			bool inString = false;
			bool scapeString = false;
			int stbLength = 48;
			StringBuilder stb = new StringBuilder(stbLength);
	        
	        //Add to the array list only those not null
	        for (int i = 0 ; i < input.Length ; i++) {
	            switch (input[i]) {
					case '\\':
						if (inString && input.Length > (i+1) && input[i+1] == '"') { 
							scapeString = true;
						}
						break;
					case '"' :
						if (!inString) {
							inString = true;
							continue;
						} else if (inString && !scapeString) {
							inString = false;
							continue;
						}
						break;
					case ' ':
						if (!inString) {
							gResult.Add(stb.ToString());
							stb = new StringBuilder(stbLength);
							continue;
						}
						break;
				}
				stb.Append(input[i]);
	        }
			//Add the current string
	        gResult.Add(stb.ToString());
	        return gResult;
	    }
		
		/// <summary>
	    /// Compares two strings. It can be a case sensitive comparation or not.
	    /// </summary>
		[Obsolete("Use String.Equals instead")]
	    public static bool Equals(string a, string b, bool caseInsensitive)
	    {
	        if (a==null && b!=null)
	            return false;
	        else if (a!=null && b==null)
	            return false;
	        else if (a==null && b== null)
	            return true;
	        else if (caseInsensitive)
	            a = a.ToLower();
	            b = b.ToLower();
	        
	        return a==b;
	    }
	}
}
