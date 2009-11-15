// ArrayComparer.cs
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

namespace NFSharp.Common.Types
{
	
	/// <summary>
	/// Helpers to compare two arrays
	/// </summary>
	public class ArrayComparer
	{
		/// <summary>
		/// Compares two arrays looking that all the elements in the same positions
		/// are equal.
		/// </summary>
		/// <param name="a">
		/// A <see cref="Array"/>
		/// </param>
		/// <param name="b">
		/// A <see cref="Array"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public static bool Same(Array a, Array b)
		{
			if (a == null || b == null || a.Length != b.Length || a.Rank != b.Rank)
				return false;
			
			if (a.Length == 0 && b.Length == 0)
				return true;
			
			if (a.Rank > 1)
				throw new NotSupportedException("Multidimensional arrays not supported");
			
			if (a.Rank == 1 && a.GetValue(0).GetType() != b.GetValue(0).GetType())
				return false;
			
			for(int i=0; i<a.Length; i++) {
				if (a.GetValue(i) != b.GetValue(i))
					return false;
			}
			
			return true;
		}
	}
}
