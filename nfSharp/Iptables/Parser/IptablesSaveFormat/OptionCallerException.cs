// OptionCallerException.cs
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

namespace NFSharp.Iptables.Parser.IptablesSaveFormat {
    /// <summary>
    /// Exception thrown when an error happens in the option caller.
    /// </summary>
    public class OptionCallerException: Exception {
        /// <summary>
        /// Constructor
        /// </summary>
        public OptionCallerException()
        :base() {
        }

        /// <summary>
        /// Constructor with message
        /// </summary>
        /// <param name="msg">
        /// A <see cref="System.String"/>
        /// </param>
        public OptionCallerException(string msg)
        :base(msg)
        {}

        /// <summary>
        /// Constructor with message and inner exception
        /// </summary>
        /// <param name="msg">
        /// A <see cref="System.String"/>
        /// </param>
        /// <param name="inner">
        /// A <see cref="Exception"/>
        /// </param>
        public OptionCallerException(string msg, Exception inner)
        :base(msg, inner)
        {}
    }
}
