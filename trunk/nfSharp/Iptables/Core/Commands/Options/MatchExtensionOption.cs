// MatchExtensionOption.cs
//
//  Copyright (C) 2006 SharpKnocking project
//  Created by Miguel Angel Pérez, mangelp@gmail.com
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
using System;

using NFSharp.Iptables;
using NFSharp.Iptables.Core.Extensions;

using CommonUtilities.Types;

namespace NFSharp.Iptables.Core.Commands.Options {

    /// <summary>
    /// Implements the match extension option.
    /// </summary>
    /// <remarks>
    /// Only built in extensions are allowed. The rest will result in a
    /// conversion failure and the logic will throw an exception.
    /// </remarks>
    public class MatchExtensionOption: GenericOption {
        private MatchExtension extension;

        /// <summary>
        /// Match extension added to a rule
        /// </summary>
        public MatchExtension Extension {
            get {
                return this.extension;
            } set {
                this.extension = value;
            }
        }

        private string customExtension;

        /// <summary>
        /// Custom extension name
        /// </summary>
        /// <remarks>
        /// This property can be set but the support to use custom extensions
        /// is not done and will throw an exception if you try to add the rule.
        /// </remarks>
        public string CustomExtension {
            get {
                return this.customExtension;
            } set {
                this.customExtension = value;
            }
        }

        public MatchExtensionOption()
        :base(RuleOptions.MatchExtension) {

        }

        public override bool TryReadValues (string strVal, out string errStr) {
            object obj;

            //This conversion only supports builtIn extensions
            if(AliasUtil.IsAliasName(typeof(MatchExtension), strVal, out obj)) {
                this.extension = (MatchExtension)obj;
                //Console.Out.WriteLine("Setting the implicit extension "+this.extension);
                this.SetImplicitExtension(this.extension);
                errStr = String.Empty;
                return true;
            }

            errStr = "The value can't be converted to any known extension";
            return false;
        }

        protected override string GetValueAsString() {
            if(this.extension == MatchExtension.CustomExtension) {
                return this.customExtension;
            }

            return AliasUtil.GetDefaultAlias (this.extension);
        }
    }
}
