// SimpleParameter.cs
//
//  Copyright (C) 2008 iSharpKnocking project
//  Created by Miguel Angel Perez (mangelp@gmail.com)
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA 
//
//

using System;

namespace Developer.Common.Options
{
	
    /// <summary>
    /// Models a generic parameter that can store a value for the parameter, the name
    /// for the parameter and has a modifier that changes the meaning of the value.
    /// </summary>
	public class SimpleParameter: NegableParameter
	{		
	    private string value;
	    
	    /// <summary>
	    /// Value of the parameter
	    /// </summary>
		/// <remarks>
		/// This value set will be checked by the concrete parameter to ensure that its
		/// correct.
		/// </remarks>
	    public virtual string Value
	    {
	        get {return this.value;}
	        set
	        {
				this.OnValueSet(value);
	            this.value = value;
	        }
	    }
		
		private string name;
		
		/// <summary>
		/// Name of the parameter.
		/// </summary>
		/// <remarks>
		/// Parameters with different names can be the same if they are aliases.
		/// Before the name is set the <c>OnNameSet</c> method is called to perform
		/// aditional checks and set the default value of the <c>isLongOption</c> property.
		/// </remarks>
		/// <see>IptablesNet.Core.IsAlias(string)</see>
		/// <see>IptablesNet.Core.SimpleParameter.Name</see>
		public string Name
		{
			get { return this.name;}
			set { 
				this.OnNameSet (value);
				this.name = value;
			}
		}
	    
	    /// <summary>
	    /// Constructor.
	    /// </summary>
		/// <param name="name">Name of the parameter</param>
		/// <param name="value">Value of the parameter</param>
		/// <remarks>
		/// This constructor initializes the value of <b>IsLongOption</b> based in the
		/// length of the name.
		/// This constructor can fail with an exception if the call of <b>OnValueSet</b>
		/// finds that the value is not correct.
		/// </remarks>
		public SimpleParameter(string name, string value)
		{
		    //This also sets the isLongOption field
		    this.Name = name;
		    //If this property is overriden this will ensure correct initialization
		    this.Value = value;
		}
		
	    /// <summary>
	    /// Default constructor.
	    /// </summary>		
		public SimpleParameter()
		{
		    
		}
		
		/// <summary>
		/// Performs further checks over a value that is going to be set for this option.
		/// </summary>
		/// <param name="value">
		/// A <see cref="System.String"/> with the value to set to the option.
		/// </param>
		/// <remarks>
		/// The default implementation does nothing.
		/// If the value is not valid for this option inheritors should handle it here or
		/// throw an exception.
		/// </remarks>
		protected virtual void OnValueSet (string value)
		{
			
		}
		
		/// <summary>
		/// Performs further checks over a value that is going to be set for this option.
		/// </summary>
		/// <param name="name">
		/// A <see cref="System.String"/> with the name to set to the option
		/// </param>
		/// <remarks>
		/// The default implementation does nothing.
		/// If the name is not a valid alias for this option inheritors should handle it
		/// here or throw an exception. 
		/// </remarks>
		protected virtual void OnNameSet (string name)
		{
			
		}
		
		/// <summary>
		/// Obtains a string with the value of the option
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> with the value of the option.
		/// </returns>
		protected override string GetValueAsString()
		{
			//Simply return the value
			return this.value;
		}
		
		/// <summary>
		/// Gets the default alias name for this option
		/// </summary>
		/// <remarks>
		/// This method is used to convert the option back to an string
		/// </remarks>
		/// <returns>
		/// A <see cref="System.String"/> with the default alias name for this option.
		/// </returns>
		public override string GetDefaultAlias()
		{
			return this.name;
		}
		
		/// <summary>
		/// Gets if the name is an alias name for this option.
		/// </summary>
		/// <param name="name">
		/// A <see cref="System.String"/> with the name to check
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/> that indicates if the name is an alias of the
		/// name of this object.
		/// </returns>
		/// <remarks>
		/// This method returs false ever. This object has no information about the posible
		/// aliases of a name.
		/// </remarks>
		public override bool IsAlias(string name)
		{
			return false;
		}
		
		/// <summary>
	    /// Gets if the string starts with two '-' characters.
	    /// </summary>
	    /// <returns>
	    /// True if the string have at least two characters and it starts with two
	    /// characters '-'. If not returns false.
	    /// </returns>
	    public static bool CheckLongFormat(string option)
	    {
			return !String.IsNullOrEmpty(option) && option.Length>=2 && 
	                    option[0]=='-' && option[1]=='-';
	    }
	}
}
