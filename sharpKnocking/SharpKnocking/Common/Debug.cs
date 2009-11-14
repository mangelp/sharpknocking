
using System;

namespace SharpKnocking.Common
{
    public enum VerbosityLevels
    {
        Normal=1,
        High=2,
        Insane=3
    }
	
	public static class Debug
	{
	    public static bool MoreVerbose = false;
	    
	    public static VerbosityLevels VerbLevel = VerbosityLevels.Insane;
	    
	    //Disabled by default.
	    public static bool DebugEnabled = false;
	    
	    public static void VerboseWrite(string line, VerbosityLevels level)
	    {
	        if(!DebugEnabled || !MoreVerbose || ((int)Debug.VerbLevel)<((int)level))
	            return;
	        
	        Console.Out.WriteLine(line);    
	    }
	    
	    public static void VerboseWrite(string line)
	    {
	        if(!DebugEnabled || !MoreVerbose)
	            return;
	        
	        Console.Out.WriteLine(line);    
	    }
	    
	    public static void Write(string line)
	    {
	        if(!DebugEnabled)
	            return;
	        
	        Console.Out.WriteLine(line);    
	    }
	    
	    public static void Write(string header, object[] parts)
	    {
	        if(!DebugEnabled)
	            return;
	        
	        if(Net20.StringIsNullOrEmpty(header))
	        {
	            Console.Out.WriteLine("Array["+parts.Length+"]");
	        }
	        else
	        {
	            Console.Out.WriteLine(header);    
	        }
	        
	        for(int i=0;i<parts.Length;i++)
	        {
	            Console.Out.WriteLine("["+i+"]: "+parts[i]);
	        }
	    }
	    
	    public static void VerboseWrite(string header, object[] parts)
	    {
	        if(!DebugEnabled || !MoreVerbose)
	            return;
	        
	        if(Net20.StringIsNullOrEmpty(header))
	        {
	            Console.Out.WriteLine("Array["+parts.Length+"]");
	        }
	        else
	        {
	            Console.Out.WriteLine(header);    
	        }
	        
	        for(int i=0;i<parts.Length;i++)
	        {
	            Console.Out.WriteLine("["+i+"]: "+parts[i]);
	        }
	    }
	    
	    public static void VerboseWrite(string header, object[] parts, VerbosityLevels level)
	    {
	        if(!DebugEnabled || !MoreVerbose || ((int)Debug.VerbLevel)<((int)level))
	            return;
	        
	        if(Net20.StringIsNullOrEmpty(header))
	        {
	            Console.Out.WriteLine("Array["+parts.Length+"]");
	        }
	        else
	        {
	            Console.Out.WriteLine(header);    
	        }
	        
	        for(int i=0;i<parts.Length;i++)
	        {
	            Console.Out.WriteLine("["+i+"]: "+parts[i]);
	        }
	    }
	}
}
