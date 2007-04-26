
using System;

namespace IptablesNet.Core
{
	
	
	public class IptablesException: Exception
	{
		
		public IptablesException()
		    :base()
		{
		}
		
		public IptablesException(string msg)
		    :base(msg)
		{
		}
		
		public IptablesException(string msg, Exception innerEx)
		    :base(msg, innerEx)
		{
		}		
	}
}
