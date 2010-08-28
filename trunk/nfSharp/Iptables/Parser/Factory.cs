//  
//  Copyright (C) 2009 SharpKnocking project
//  File created by Miguel Angel Perez
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace NFSharp.Iptables.Parser
{
	/// <summary>
	/// Factory that returns a rule parser for a concrete format
	/// </summary>
	public class Factory
	{
		private static Factory instance;

		/// <summary>
		/// Singleton that returns the factory instance
		/// </summary>
		public static Factory Instance {
			get {
				if (instance == null) {
					instance = new Factory();
				}
					
				return instance;
			}
		}
		
		private Factory()
		{
		}

		/// <summary>
		/// Gets a parser for a file
		/// </summary>
		/// <param name="parserName">
		/// A <see cref="System.String"/> with the name of the parser to instantiante.
		/// </param>
		/// <example>
		/// GetParser("IptablesSaveFormat");
		/// </example>
		public IRuleParser GetParser (string parserName, StringDictionary options) {
			parserName += "Parser";

			// TODO: Support to load parsers from an assembly in a given extensions folder

			IRuleParser result = null;

			switch (parserName) {
				case "IptablesSaveFormatParser":
					result = new IptablesSaveFormatParser ();
					break;
				default:
					throw new ArgumentException("Invalid parser name " + parserName);
			}

			result.Options = options;

			return result;
		}
	}
}
