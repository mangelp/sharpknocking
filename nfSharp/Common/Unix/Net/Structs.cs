// Structs.cs
//
//  Copyright (C)  2007 SharpKnocking project
//  Created by Miguel Angel Perez mangelp@gmail.com
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

namespace NFSharp.Common.Unix.Net {
    /// <summary>
    /// Represents a network service like those in /etc/services
    /// </summary>
    public struct NetworkService {
        /// <summary>
        /// Empty instance
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/>
        /// </returns>
        public static readonly NetworkService Empty = new NetworkService();

        /// <summary>
        /// Service name
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/>
        /// </returns>
        public string Name;

        /// <summary>
        /// Service protocol
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/>
        /// </returns>
        public Protocols Protocol;

        /// <summary>
        /// Service port number
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/>
        /// </returns>
        public ushort Port;

        /// <summary>
        /// Gets an string that represents this instace
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/>
        /// </returns>
        public override string ToString () {
            return Name+"@"+((ushort)Protocol)+":"+Port.ToString();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">
        /// A <see cref="System.String"/>
        /// </param>
        /// <param name="proto">
        /// A <see cref="Protocols"/>
        /// </param>
        /// <param name="port">
        /// A <see cref="System.UInt16"/>
        /// </param>
        public NetworkService(string name, Protocols proto, ushort port) {
            this.Name = name;
            this.Protocol = proto;
            this.Port = port;
        }

        /// <summary>
        /// Parses a string and gets an instance of NetworkService initiallized
        /// with the information found
        /// </summary>
        /// <param name="netServ">
        /// A <see cref="System.String"/>
        /// </param>
        /// <returns>
        /// A <see cref="NetworkService"/>
        /// </returns>
        public static NetworkService Parse(string netServ) {
            int pos = netServ.IndexOf('@');
            if(pos>0 && pos<(netServ.Length-1)) {
                int pos2 = netServ.IndexOf(':', pos+1);
                if(pos2>(pos+1) && pos2<(netServ.Length-1)) {
                    NetworkService nsResult = new NetworkService();
                    nsResult.Protocol = (Protocols) ushort.Parse(netServ.Substring(pos+1, pos2 -pos));
                    nsResult.Port = ushort.Parse(netServ.Substring(pos2+1));
                    nsResult.Name = netServ.Substring(0, pos);
                    return nsResult;
                }
            }

            throw new FormatException("Invalid format for input string");
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>
        /// A <see cref="System.Int32"/>
        /// </returns>
        public override int GetHashCode () {
            return this.ToString().GetHashCode();
        }
    }
}
