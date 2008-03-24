// OptionCallData.cs
//
//  Copyright (C)  2008 iSharpKnocking project
//  Created by Miguel Angel Perez (mangelp_AT_gmail_DOT_com)
//
//  This library is free software; you can redistribute it and/or
//  modify it under the terms of the GNU Lesser General Public
//  License as published by the Free Software Foundation; either
//  version 2.1 of the License, or (at your option) any later version.
//
//  This library is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//  Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public
//  License along with this library; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

using System;

namespace Developer.Common.Options
{
	/// <summary>
	/// Models the data about the procesing of a parameter
	/// </summary>
	public class OptionCallData
	{
		private CmdLineOption sourceOption;
		private string errorMessage;
		private bool hasErrors;
		private bool abortParsing;

		/// <summary>
		/// Gets/Sets the option that matched the parameter
		/// </summary>
		public CmdLineOption SourceOption {
			get {
				return sourceOption;
			}
			set {
				sourceOption = value;
			}
		}
		
		/// <summary>
		/// Return the position of the parameter in the input array of arguments
		/// </summary>
		public int Order
		{
			get {
				if (this.sourceOption != null)
					return this.sourceOption.HitPosition;
				else
					return -1;
			}
		}
		
		/// <summary>
		/// Return the value of the parameter in the input array of arguments
		/// </summary>
		public string Value
		{
			get {
				if (this.sourceOption != null)
					return this.sourceOption.Parameter.Value;
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
		public bool HasErrors {
			get {
				return hasErrors;
			}
			set {
				hasErrors = value;
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
		public OptionCallData()
		{
		}
	}
}
