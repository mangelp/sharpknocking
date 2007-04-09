
using System;
using System.Net;
using System.Collections;

namespace SharpKnocking.Common
{
	
	/// <summary>
	/// Methods that implement functionality that is already in the versión
	/// 2.0 of the .net and mono 1.2 frameworks but we use the 1.1 profile under
	/// mono so we can't use that functionality.
	/// </summary>
	public static class Net20
	{
	    /// <summary>
	    /// Splits the input string removing empty strings if specified.
	    /// </summary>
	    public static string[] StringSplit(string input,
	                                       bool removeEmpty,
	                                       params char[] chars)
	    {
	        //Normal split
	        string[] result = input.Split(chars);
	        
	        if(!removeEmpty)
	            return result;
	        
	        ArrayList gResult = new ArrayList(); 
	        
	        //Add to the array list only those not null
	        for(int i=0;i<result.Length;i++)
	        {
	            if(!StringIsNullOrEmpty(result[i]))
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
	    public static bool StringEquals(string a, string b, bool caseInsensitive)
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
	    
	    /// <summary>
	    /// Returns if the string is null or has a length of 0
	    /// </summary>
	    public static bool StringIsNullOrEmpty(string str)
	    {
	        if(str == null || str.Length == 0)
	            return true;
	        
	        return false;
	    }
	    
	    /// <summary>
	    /// This method checks if a string contains a valid IP.
	    /// </summary>
	    /// <param name = "ipAddress">
	    /// The string we want to check.
	    /// </param>
	    /// <returns>
	    /// True, if the string given as parameter was a valid IP,
	    /// false otherwise.
	    /// </returns>
	    public static bool TryParseIP(string ipAddress)
	    {
	    	if(Net20.StringIsNullOrEmpty(ipAddress))
	    		return false;
	    		
	    	bool res = true;
	    	try
	    	{
	    		IPAddress.Parse(ipAddress);
	    	}
	    	catch(FormatException)
	    	{
	    		res = false;
	    	}
	    	
	    	return res;
	    }
	    
	    /// <summary>
	    /// Returns true if the string can be converted to an int or false
	    /// if not. If the result is true the value resulting from the conversion
	    /// is set in the ouput parameter value
	    /// </summary>
	    /// <param name="strValue">
	    /// String to try to convert to an int32
	    /// </param>
	    /// <param name="value">
	    /// Output parameter. Value resulting from the conversion if the method
	    /// return true. If the method returns false this can be any value between
	    /// Int32.MaxValue and Int32.MinValue
	    /// </param>
	    public static bool Int32TryParse(string strValue, out int value)
	    {
	        value = -1;
	        
	        try
	        {
	            value = Int32.Parse(strValue);
	            return true;
	        }
	        catch(Exception)
	        {
	            return false;
	        }
	    }
	}
}
