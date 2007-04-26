
using System;
using System.Collections;
using System.Collections.Generic;

using SharpKnocking.Common;

using IptablesNet.Core;
using IptablesNet.Core.Extensions;

namespace IptablesNet.Core.Extensions.ExtendedMatch
{
	
	
	public class ReadOnlyMatchExtensionListAdapter: ReadOnlyListAdapter<MatchExtensionHandler>
	{
	    
		public ReadOnlyMatchExtensionListAdapter(List<MatchExtensionHandler> adapted)
			:base(adapted)
		{
		    
		}
	}
}
