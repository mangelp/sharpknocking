//  
//  Copyright (C) 2010 SharpKnocking project
//  File created by mangelp
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

using NFSharp.Iptables.Core.Commands.Options;
using CommonUtilities.Types;

namespace NFSharp.Iptables.Converter.IptablesSaveFormatString {

	/// <summary>
	/// Converter for GenericOption class and subclasses
	/// </summary>
	public class GenericOptionStringConverter
	{

		public GenericOptionStringConverter () {
		}

		public string convert(JumpOption jumpOption) {
            string result = String.Empty;

            if(this.target == RuleTargets.CustomTarget) {
                // Console.WriteLine("Converting to string: "+this.target+", "+this.customTarget+", "+this.customTargetName+", "+this.extension);
                // Fix: We must return the list of option parameters, that is
				// the name of the target extension and all his parameters.
				// This differs from what the match extension does.
                if(this.customTarget == CustomRuleTargets.CustomExtension) {
                    result = this.extension.ExtensionName + " " + this.extension.ToString();
                } else if(this.customTarget == CustomRuleTargets.UserDefinedChain) {
                    result = this.customTargetName;
                }
            } else {
                result = AliasUtil.GetDefaultAlias(this.target);
            }

            return result;
        }
	}
}
