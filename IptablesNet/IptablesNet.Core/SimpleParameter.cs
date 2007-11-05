// SimpleParameter.cs
//
//  Copyright (C) 2007 iSharpKnocking project
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

namespace IptablesNet.Core
{
	
    /// <summary>
    /// Models a generic parameter with the common properties needed.
    /// </summary>
    /// <remarks>
    /// The GetHashCode method is overriden to return the hash code of the name.
    /// So when using hashtables two parameters with the same name will be the
    /// same parameter.<br/>
    /// The Equals method is overriden to return true when the names and the values
    /// are equal and false otherwise.<br/>
    /// </remarks>
	public class SimpleParameter: NegableParameter
	{
		public override bool IsLongFormat
		{
			get 
			{
				if(!String.IsNullOrEmpty(this.name) &&
				   name.Length>1)
				{
					return true;
				}
				
				return false;
			}
		}
		
		
	    private string value;
	    
	    /// <summary>
	    /// Parameter value as string.
	    /// Currently the number of long format parameters are limited.
	    /// </summary>
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
		/// Parameters with different names can be the same if they are aliases
		/// see <c>IsAlias(string)</c>
		/// Before the name is set further procesing can be done in the <c>OnNameSet</c>
		/// method like aditional checks.
		/// </remarks>
		public string Name
		{
			get { return this.name;}
			set { 
				this.OnNameSet (value);
				this.name = value;
			}
		}
	    
	    /// <summary>
	    /// Parametrized constructor.
	    /// </summary>
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
		
		protected virtual void OnValueSet (string value)
		{
			
		}
		
		protected virtual void OnNameSet (string name)
		{
			
		}
		
		protected override string GetValuesAsString()
		{
			return this.value;
		}
		
		public override string GetDefaultAlias()
		{
			return this.name;
		}
		
		public override bool IsAlias(string name)
		{
			return false;
		}
		
		/// <summary>
	    /// Gets if the string starts with two '-' characters.
	    /// </summary>
	    /// <returns>
	    /// True if the string have at least two characters and those
	    /// characters are '-'. In any other way it returns false.
	    /// </returns>
	    public static bool CheckLongFormat(string option)
	    {
	        bool result=false;
	        
	        if(option!=null && option.Length>=2 &&
	                    option[0]=='-' && option[1]=='-')
            {
                result = true;
            }
            
            return result;
	    }
	}
}
