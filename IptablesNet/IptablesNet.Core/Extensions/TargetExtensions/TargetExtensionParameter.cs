
using System;
using System.Collections;
using System.Collections.Generic;

using IptablesNet.Core;

namespace IptablesNet.Core.Extensions.ExtendedTarget
{
	/// <summary>
	/// Models a parameter of a target extension.
	/// </summary>
	/// <remarks>
	/// Each target extension allows a set of options and this class helps
	/// modeling those options as parameters for the target extension.
	/// </remarks>
	public abstract class TargetExtensionParameter: ExtensionParameter<TargetExtensionHandler>
	{
	    
		public TargetExtensionParameter(TargetExtensionHandler owner, object enumValue)
		  :base(owner, enumValue)
		{
		}
	}
}
