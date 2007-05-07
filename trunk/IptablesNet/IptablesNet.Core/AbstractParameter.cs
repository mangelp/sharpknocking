
using System;
using System.Collections.Generic;

namespace IptablesNet.Core
{

	/// <summary>
	/// Models an abstract parameter. This is the base class for everything that
	/// has a command-line option format
	/// </summary>
	public abstract class AbstractParameter
	{
		/// <summary>
		/// Returns if the parameter is in long format or in short format
		/// </summary>
		public virtual bool IsLongFormat
		{
			get {
				string def = this.GetDefaultAlias();
				
				if(def!= null && def.Length>1)
					return true;
				else
					return false;
			}
		}
		
		protected AbstractParameter ()
		{}
		
		/// <summary>
		/// Checks if the name is a valid alias for this parameter.
		/// </summary>
		public abstract bool IsAlias(string name);
		
		/// <summary>
		/// Gets a default alias name for this parameter.
		/// </summary>
		/// <remarks>
		/// This alias is the default name for the command
		/// </remarks>
		public abstract string GetDefaultAlias();
		
		public override string ToString ()
		{
			string val = this.GetValuesAsString();
			
			if(!String.IsNullOrEmpty (val))
				return this.GetNameAsString ()+" "+val;
			else
				return this.GetNameAsString ();
		}
		
		protected virtual string GetNameAsString(bool longFormat)
		{
		    string result = String.Empty;
		    
		    if(longFormat)
		        result = "--"+this.GetDefaultAlias();
		    else
		        result = "-"+this.GetDefaultAlias();
			
			return result;			
		}
		
		protected string GetNameAsString()
		{
			return this.GetNameAsString( this.IsLongFormat);
		}

		protected abstract string GetValuesAsString();
		
	}
}
