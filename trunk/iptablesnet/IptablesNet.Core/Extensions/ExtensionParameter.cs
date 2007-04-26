
using System;

using SharpKnocking.Common;

namespace IptablesNet.Core.Extensions
{
	/// <summary>
	/// Models a parameter of a target extension.
	/// </summary>
	/// <remarks>
	/// Each target extension allows a set of options and this class helps
	/// modeling those options as parameters for the target extension.
	/// </remarks>
	public abstract class ExtensionParameter<TOwn>: NegableParameter, IExtensionParameter
		where TOwn:IExtensionHandler
	{
		public override bool IsLongFormat 
		{
			get
			{
				string name = this.GetDefaultAlias ();
					
				if(name!=null && name.Length>1)
					return true;
				else
					return false;
			}
		}
			
		private object enumValue;
			
		/// <summary>
		/// Gets the enumeration constant that defines the parameter type
		/// </summary>
		protected object Option
		{
			get { return this.enumValue;}
		}
		
        private TOwn owner;
	    
	    /// <summary>
	    /// Extension handler owner of this instance.
	    /// </summary>
	    protected TOwn Owner
	    {
	        get { return this.owner;}
	    }
	    
		public ExtensionParameter(TOwn owner, object enumValue)
		  :base()
		{
			if (enumValue==null)
				throw new ArgumentNullException ("enumValue");
				
			this.enumValue = enumValue;
				
			if (owner == null)
				throw new ArgumentNullException ("owner");
			
		    this.owner = owner;
		}
			
		public abstract void SetValues(string value);
		
		/// <summary>
		/// Parses the value string and fills the properties of the parameter.
		/// </summary>
		/// <remarks>
		/// This method must be implemented and throw FormatException when the
		/// string cannot be parsed
		/// </remarks>
		public bool TrySetValues(string value, out string errMsg)
		{
			try {
				this.SetValues(value);
			} catch (Exception ex) {
				errMsg = ex.Message;
				return false;
			} 
			
			errMsg = String.Empty;
			return true;
		}
		
		public override bool IsAlias (string name)
		{
			return TypeUtil.IsAliasName (this.enumValue, name);
		}
			
		public override string GetDefaultAlias ()
		{
			return TypeUtil.GetDefaultAlias(this.enumValue);
		}
	}
}
