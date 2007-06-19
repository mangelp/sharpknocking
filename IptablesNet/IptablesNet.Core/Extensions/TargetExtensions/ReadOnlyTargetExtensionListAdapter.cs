
using System;
using System.Collections;
using System.Collections.Generic;

using IptablesNet.Core;

namespace IptablesNet.Core.Extensions.ExtendedTarget
{
	
	public class ReadOnlyTargetExtensionListAdapter: ReadOnlyListAdapter<TargetExtensionHandler>
	{
		public ReadOnlyTargetExtensionListAdapter(List<TargetExtensionHandler> adapted)
			:base(adapted)
		{}
	}
}
