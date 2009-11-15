// OptionCallerData.cs
//
//  Copyright (C) 2008 [name of author]
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

namespace NFSharp.Iptables.Parser.IptablesSaveFormat
{
	
	/// <summary>
	/// Models the data about the procesing of a parameter
	/// </summary>
	public class OptionCallerData: EventArgs
	{
		private Option sourceOption;
		private string errorMessage;
		private bool abortParsing;
		private SimpleParameter parameter;
		private int position;

		/// <summary>
		/// Gets/Sets the option that matched the parameter
		/// </summary>
		public Option SourceOption {
			get {
				return sourceOption;
			}
			set {
				sourceOption = value;
			}
		}
		
		/// <value>
		/// Gets/sets the parameter that matched the option
		/// </value>
		public SimpleParameter Parameter
		{
			get {
				return this.parameter;
			}
			set {
				this.parameter = value;
			}
		}
		
		/// <summary>
		/// Return the position of the parameter in the input array of arguments
		/// </summary>
		public int Position
		{
			get {
				return this.position;
			}
			
			set {
				this.position = value;
			}
		}
		
		/// <summary>
		/// Return the value of the parameter in the input array of arguments
		/// </summary>
		public string Value
		{
			get {
				if (this.parameter != null)
					return this.parameter.Value;
				else
					return String.Empty;
			}
		}

		/// <summary>
		/// Gets/Sets the error string
		/// </summary>
		public string ErrorMessage {
			get {
				return errorMessage;
			}
			set {
				errorMessage = value;
			}
		}

		/// <summary>
		/// Gets/Sets if the parameter has errors
		/// </summary>
		public bool HasError {
			get {
				return !String.IsNullOrEmpty(this.ErrorMessage);
			}
		}

		/// <summary>
		/// Gets/Sets if the parameter parsing must be aborted after this
		/// parameter.
		/// </summary>
		public bool AbortParsing {
			get {
				return abortParsing;
			}
			set {
				abortParsing = value;
			}
		}
		
		/// <summary>
		/// TODO:
		/// </summary>
		public OptionCallerData()
		{
		}
	}
}
