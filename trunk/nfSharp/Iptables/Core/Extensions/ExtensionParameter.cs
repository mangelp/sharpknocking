// ExtensionParameter.cs
//
//  Copyright (C) 2007 iSharpKnocking project
//  Created by mangelp<@>gmail[*]com
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

using CommonUtilities.Types;
using NFSharp.Iptables.Parser.IptablesSaveFormat;

namespace NFSharp.Iptables.Core.Extensions {
    /// <summary>
    /// Models a parameter of a target extension.
    /// </summary>
    /// <remarks>
    /// Each target extension allows a set of options and this class helps
    /// modeling those options as parameters for the target extension.
    /// </remarks>
    public abstract class ExtensionParameter<TOwn>: NegableParameter, IExtensionParameter
        where TOwn:IExtensionHandler {
        public override bool IsLongFormat {
            get {
                string name = this.GetDefaultAlias ();

                if(name!=null && name.Length>1) {
                    return true;
                } else {
                    return false;
                }
            }
        }

        private object enumValue;

        /// <summary>
        /// Gets the enumeration constant that defines the parameter type
        /// </summary>
        protected object Option {
            get {
                return this.enumValue;
            }
        }

        private TOwn owner;

        /// <summary>
        /// Extension handler owner of this instance.
        /// </summary>
        protected TOwn Owner {
            get {
                return this.owner;
            }
        }

        public ExtensionParameter(TOwn owner, object enumValue)
        :base() {
            if (enumValue==null) {
                throw new ArgumentNullException ("enumValue");
            }

            this.enumValue = enumValue;

            if (owner == null) {
                throw new ArgumentNullException ("owner");
            }

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
        public bool TrySetValues(string value, out string errMsg) {
            try {
                this.SetValues(value);
            } catch (Exception ex) {
                errMsg = ex.Message;
                return false;
            }

            errMsg = String.Empty;
            return true;
        }

        public override bool IsAlias (string name) {
            return AliasUtil.IsAliasName (this.enumValue, name);
        }

        public override string GetDefaultAlias () {
            return AliasUtil.GetDefaultAlias(this.enumValue);
        }
    }
}
