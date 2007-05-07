
using System;

namespace IptablesNet.Core
{
	
	
	public class DuplicateElementException: Exception
	{
		
		public DuplicateElementException()
		    :base()
		{
		}
		
		public DuplicateElementException(string msg)
		    :base(msg)
		{
		}
		
		public DuplicateElementException(string msg, Exception innerException)
		    :base(msg, innerException)
		{
		}
	}
}
