// OptionCaller.cs
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
using System.Reflection;

namespace Developer.Common.Options
{
	/// <summary>
	/// Stores the required information to call a method (static or not) from another object or type or
	/// to call a delegate upon client request.
	/// </summary>
	public class OptionCaller
	{
		/// <summary>
		/// Delegate to execute every time this option is found
		/// </summary>
		private OptionActionDelegate procesingDelegate;
		
		/// <summary>
		/// Delegate that will be executed to perform custom processing of the
		/// option.
		/// </summary>
		public OptionActionDelegate ProcesingDelegate
		{
			get { return this.procesingDelegate;}
			internal set { this.procesingDelegate = value;}
		}
		
		private string methodName;
		
		/// <summary>
		/// Name of a method that can process the option
		/// </summary>
		public string MethodName
		{
			get { return this.methodName; }
			internal set { this.methodName = value; }
		}
		
		private object owner;
		
		/// <summary>
		/// Owner type or instance to use when invoking
		/// </summary>
		public object Owner
		{
			get {
				return this.owner;
			}
			internal set {
				this.owner = value;
			}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="owner">
		/// A <see cref="System.Object"/>
		/// </param>
		public OptionCaller(object owner)
		{
			this.owner = owner;
		}
		
		/// <summary>
		/// Executes the method if there is any specified
		/// </summary>
		/// <remarks>
		/// If there have been specified a procesing delegate it will be used, but if not the information
		/// is used to search a method to call by reflection.
		/// </remarks>
		/// <param name="data">
		/// A <see cref="OptionCallerData"/>
		/// </param>
		public void CallMethod(OptionCallerData data)
		{
			if (this.procesingDelegate != null) {
				this.procesingDelegate(data);
				return;
			} else if (this.owner == null) {
				throw new InvalidOperationException (
				    "Can't call method "+data.SourceOption.Caller.methodName
                    +". The source type or instance are not specified");
			}

			BindingFlags flags = BindingFlags.InvokeMethod | BindingFlags.Public 
					| BindingFlags.Instance;
			Type t = null;
			object instance = null;
			
			if (this.owner is Type) {
				t = (Type)this.owner;
				flags |= BindingFlags.Static;
			} else if (this.owner != null) {
				//If there is a methodOwnerInstance specified use it to execute the
				//method that is required to be public
				t = this.owner.GetType();
				instance = this.owner;
			} else {
				throw new InvalidOperationException("There is no target to execute the method");
			}

			t.InvokeMember(data.SourceOption.Caller.MethodName, flags, null, 
			               instance, new object[]{data});
		}
	}
}
