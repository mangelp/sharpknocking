
using System;

namespace IptablesNet.Core
{
	public class NetfilterException: Exception
	{
		public NetfilterException()
		    :base()
		{
		}
		
		public NetfilterException(string msg)
		    :base(msg)
		{
		}
		
		public NetfilterException(string msg, Exception innerEx)
		    :base(msg, innerEx)
		{
		}		
	}
}
