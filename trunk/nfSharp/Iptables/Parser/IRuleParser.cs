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
using System.Collections;
using System.Collections.Specialized;
using NFSharp.Iptables.Core;

namespace NFSharp.Iptables.Parser {
    public interface IRuleParser {
        /// <summary>
        /// Parses the rule source and returns the table set
        /// </summary>
        /// <returns>
        /// A <see cref="NetfilterTableSet"/>
        /// </returns>
        NetfilterTableSet parse();
        /// <summary>
        /// Stores a table set in the rule source replacing the previous one.
        /// </summary>
        /// <param name="tableSet">
        /// A <see cref="NetfilterTableSet"/>
        /// </param>
        void store(NetfilterTableSet tableSet);

        /// <summary>
        /// Options for parsing
        /// </summary>
        StringDictionary Options {
            get;
            set;
        }
    }
}
