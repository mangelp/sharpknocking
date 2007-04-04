
using System;

namespace SharpKnocking.NetfilterFirewall
{
	
	
	public class RuleException: Exception
	{
		
		public RuleException(string text)
		    :base(text)
		{
		}
		
		public RuleException(string text, Exception innerException)
		    :base(text, innerException)
		{
		    
		}
	}
}
