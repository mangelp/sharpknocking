// NetfilterRuleList.cs
//
//  Copyright (C) 2007 iSharpKnocking project
//  Created by Miguel Angel Perez (mangelp{@}gmail{d0t}com)
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
using NFSharp.Iptables;

namespace NFSharp.Iptables.Core {

    /// <summary>
    /// Models a list of rules and operations to handle them
    /// </summary>
    public class NetfilterRuleList: CollectionBase {
        NetfilterChain parentChain;

        /// <summary>
        /// Parent chain that holds this list of rules.
        /// <summary>
        public NetfilterChain ParentChain {
            get {
                return this.parentChain;
            }
        }

        /// <summary>
        /// Constructor. Requires the parent chain reference.
        /// </summary>
        public NetfilterRuleList(NetfilterChain parentChain)
        :base() {
            this.parentChain = parentChain;
        }

        /// <summary>
        /// Gets/Replaces a rule in the list.
        /// </summary>
        public NetfilterRule this[int index] {
            get {
                return (NetfilterRule)this.InnerList[index];
            } set {
                NetfilterRule rule = (NetfilterRule)this.InnerList[index];

                this.List[index] = value;
                rule.ParentChain = null;
            }
        }

        /// <summary>
        /// Adds a rule to the list.
        /// </summary>
        /// <remarks>
        /// The parent chain for the rule is updated with the parent chain
        /// for this list.
        /// </remarks>
        public void Add(NetfilterRule rule) {
            if(rule.ParentChain!=null) {
                rule.ParentChain.Rules.Remove(rule);
            }

            this.List.Add(rule);

            rule.ParentChain = this.parentChain;
        }

        /// <summary>
        /// Creates a new rule from its string representation
        /// </summary>
        /// <param name="rule">
        /// A <see cref="System.String"/>
        /// </param>
        /// <returns>
        /// A <see cref="NetfilterRule"/>
        /// </returns>
        public NetfilterRule Add(string rule) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets if the list contains a rule
        /// </summary>
        public bool Contains(NetfilterRule rule) {
            return this.List.Contains(rule);
        }

        /// <summary>
        /// Gets if a rule exists
        /// </summary>
        /// <param name="rule">
        /// A <see cref="System.String"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Boolean"/>
        /// </returns>
        public bool Contains(string rule) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes all the rules that matches another rule
        /// </summary>
        /// <param name="rule">
        /// A <see cref="NetfilterRule"/>
        /// </param>
        public void Remove(NetfilterRule rule) {
            this.List.Remove(rule);
            rule.ParentChain = null;
        }

        /// <summary>
        /// Removes a rule from its string representation
        /// </summary>
        /// <param name="rule">
        /// A <see cref="System.String"/>
        /// </param>
        public void Remove(string rule) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the position of a rule
        /// </summary>
        /// <param name="rule">
        /// A <see cref="NetfilterRule"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Int32"/>
        /// </returns>
        public int IndexOf(NetfilterRule rule) {
            return this.List.IndexOf(rule);
        }

        public int IndexOf(string rule) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Inserts a rule in the list.
        /// </summary>
        public void Insert(int index, NetfilterRule rule) {
            if(rule.ParentChain!=null) {
                rule.ParentChain.Rules.Remove(rule);
            }

            this.List.Insert(index, rule);

            rule.ParentChain = this.parentChain;
        }

        /// <summary>
        /// Inserts a new rule from its string representation
        /// </summary>
        /// <param name="index">
        /// A <see cref="System.Int32"/>
        /// </param>
        /// <param name="rule">
        /// A <see cref="System.String"/>
        /// </param>
        /// <returns>
        /// A <see cref="NetfilterRule"/>
        /// </returns>
        public NetfilterRule Insert(int index, string rule) {
            throw new NotImplementedException();
        }
    }
}
