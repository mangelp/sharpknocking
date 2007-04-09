
using System;

namespace SharpKnocking.Common
{
	
	
	public enum DebugTargets: short
	{
	    Warning=0, //Warning messages
	    Error=1, //Application crashes information
	    Information=2, //Keep the user informed about things
	    Debug=3 //Debug things. Prings everything to output target.
	}
	
	public enum OutputTargets: short
	{
	    LogFile=0, //Log to file
	    Console=1 //Log to console
	}
}
