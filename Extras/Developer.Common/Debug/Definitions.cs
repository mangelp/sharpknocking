
using System;

namespace Developer.Common.Debug
{
	public enum LogLevel
    {
		Low=1,
        Normal=2,
        High=3,
        Insane=4
    }
	
	public enum LogCategory: short
	{
	    Warning=0, //Warning messages
	    Error=1, //Application crashes information
	    Information=2, //Keep the user informed about things
	    Debug=3 //Debug things. Prints everything to output target. 
	}
	
	public enum OutputTarget: short
	{
	    File=0, //Log to file
	    Console=1 //Log to console
	}
}
