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
using System.IO;
using System.Collections;
using System.Collections.Specialized;

using NFSharp.Iptables.Core;

namespace NFSharp.Iptables.Parser
{
	/// <summary>
	/// Parses a stream of text assuming it contains iptables commands in the
	/// same format as iptables-save command produces and iptables-restore
	/// supports.
	/// </summary>
	public abstract class TextFileParser : IRuleParser
	{
		private string fileName;

		/// <summary>
		/// Gets/sets the file to be used to load/store the rules. This
		/// must include full path.
		/// </summary>
		public string FileName {
			get { return this.fileName; }
			set { this.fileName = value; }
		}

		public StringDictionary Options {
			get;
			set;
		}

		public TextFileParser (string file) :
			base() {

			this.fileName = file;
		}

		public TextFileParser () :
			base() {

		}

		public NetfilterTableSet parse () {
			if (String.IsNullOrEmpty (this.fileName)) {
				throw new ArgumentException ("The name of the file haven't been set");
			}

			if (!File.Exists(this.fileName)) {
				throw new ArgumentException ("The file doesn't exists: " + this.fileName);
			}

			NetfilterTableSet tableSet = new NetfilterTableSet ();

			StreamReader reader = File.OpenText (this.fileName);
			string line = reader.ReadLine ();
			while (line != null) {
				this.parseLine (line, tableSet);
			}

			return tableSet;
		}

		/// <summary>
		/// Parses the line and changes the set of rules to include the rule.
		/// </summary>
		/// <param name="line">
		/// A <see cref="String"/>
		/// </param>
		/// <param name="tableSet">
		/// A <see cref="NetfilterTableSet"/> to fill in with parsed lines
		/// </param>
		protected abstract void parseLine (String line, NetfilterTableSet tableSet);

		public void store (NetfilterTableSet tableSet) {
			throw new NotImplementedException("Still don't implemented!");
		}
	}
}
