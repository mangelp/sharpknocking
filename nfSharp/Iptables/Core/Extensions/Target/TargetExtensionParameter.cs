// TargetExtensionParameter.cs
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
using System.Collections;
using System.Collections.Generic;

using NFSharp.Iptables;

namespace NFSharp.Iptables.Core.Extensions.Target {
    /// <summary>
    /// Models a parameter of a target extension.
    /// </summary>
    /// <remarks>
    /// Each target extension allows a set of options and this class helps
    /// modeling those options as parameters for the target extension.
    /// </remarks>
    public abstract class TargetExtensionParameter: ExtensionParameter<TargetExtensionHandler> {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="owner">
        /// A <see cref="TargetExtensionHandler"/>
        /// </param>
        /// <param name="enumValue">
        /// A <see cref="System.Object"/>
        /// </param>
        public TargetExtensionParameter(TargetExtensionHandler owner, object enumValue)
        :base(owner, enumValue) {
        }
    }
}
