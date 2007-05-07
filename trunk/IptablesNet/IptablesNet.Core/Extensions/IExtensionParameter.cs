
using System;

namespace IptablesNet.Core
{	
	public interface IExtensionParameter
	{
		void SetValues (string value);
		
		/// <summary>
		/// Parses the value string and fills the properties of the parameter.
		/// </summary>
		/// <remarks>
		/// This method must be implemented and throw FormatException when the
		/// string cannot be parsed
		/// </remarks>
		bool TrySetValues (string value, out string errMsg);
		
		/// <summary>
		/// Returns the default name for the parameter
		/// </summary>
		string GetDefaultAlias ();

		/// <summary>
		/// Returns if a name is a valid alias for the parameter
		/// </summary>
		bool IsAlias (string name);
	}
}
