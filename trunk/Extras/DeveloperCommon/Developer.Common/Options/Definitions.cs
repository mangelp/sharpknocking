// Definitions.cs
//
//  Copyright (C)  2008 iSharpKnocking project
//  Created by Miguel Angel Perez (mangelp_AT_gmail_DOT_com)
//
//  This library is free software; you can redistribute it and/or
//  modify it under the terms of the GNU Lesser General Public
//  License as published by the Free Software Foundation; either
//  version 2.1 of the License, or (at your option) any later version.
//
//  This library is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//  Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public
//  License along with this library; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

using System;

namespace Developer.Common.Options
{

//	/// Delegate to call aditional procesing over an option found in the command
//	/// line
//	/// </summary>
//	public delegate void OptionProcessingDelegate(OptionCallData data);
	
	/// <summary>
	/// Delegate to call aditional procesing over an option found in the command
	/// line
	/// </summary>
	public delegate void OptionActionDelegate(OptionCallerData data);
	
	/// <summary>
	/// Flags for options. This makes the option parser to perform some validations.
	/// </summary>
	[Flags]
	public enum OptionFlags
	{
		/// <summary>
		/// Default flag value. This means: optional, single, not negable, no path check, value optional
		/// </summary>
		Defaults=0,
		/// <summary>
		/// The option can be more than one time in the command line. 
		/// The default is to allow only one.
		/// </summary>
		Multiple=1,
		/// <summary>
		/// This marks the option as default. You must provide a default value for it
		/// </summary>
		DefaultOption=2,
		/// <summary>
		/// The value is optional
		/// </summary>
		ValueRequired=4,
		/// <summary>
		/// The option is required. Not having it will cause an exception to be thrown
		/// </summary>
		Required=8,
		/// <summary>
		/// The option is required but it will not cause an exception if there
		/// is somewhere another option with this flag set.
		/// </summary>
		/// <remarks>
		/// This flag also makes necesary to specify a value for the RequireGroup property
		/// (an integer) that identifies the group of options that can satisfy the requirement.
		/// </remarks>
		RequiredAny=16,
		/// <summary>
		/// The option can be negated using the '!' character. This inverts the sense
		/// of the parameter
		/// </summary>
		Negable=32,
		/// <summary>
		/// The option value is a path that must exists. Otherwise an exception will be 
		/// thrown.
		/// </summary>
		ExistingPath=64
	}
	
	/// <summary>
	/// Type flags to assert the type of a value to an option
	/// </summary>
	/// <remarks>
	/// You can mix these values to specify various types as valid types for an option or to allow 
	/// lists of values, for examble the flag ValueList ored with Int will assert that the value is a list
	/// of integers or one integer.
	/// You can also specify various types like this: ValueList | Int | String. This way the parser will
	/// assert that the value is an string, an integer, a list of integers or a list of strings.
	/// As the default type is string if you specify only the flag ValueList that implies String. The String
	/// type is the native type of values (as they are input to the parser) and no assertion is done to
	/// them.
	/// </remarks>
	[Flags]
	public enum TypeAssertionFlags
	{
		/// <summary>
		/// All options defaults to an string type. When used in a list the strings must be delimited with
		/// the ' character
		/// </summary>
		String=0,
		/// <summary>
		/// Type byte
		/// </summary>
		Byte=1,
		/// <summary>
		/// Type short (16 bits)
		/// </summary>
		Short=2,
		/// <summary>
		/// Type int (32 bits)
		/// </summary>
		Int=4,
		/// <summary>
		/// Type long (64 bits)
		/// </summary>
		Long=8,
		/// <summary>
		/// Type single (single precision)
		/// </summary>
		Single=16,
		/// <summary>
		/// Type float (floating point)
		/// </summary>
		Float=32,
		/// <summary>
		/// Value list of values. Values are separated by commas.
		/// </summary>
		/// <remarks>
		/// If you want to use commas in an option and that option is marked with this flag
		/// you must scape them using a backslash \
		/// </remarks>
		ValueList=64,
		/// <summary>
		/// Range of values. Two values separated by the - character.
		/// </summary>
		/// <remarks>
		/// If you want to use the - character in the option value you can scape it using
		/// a backslash \
		/// </remarks>
		ValueRange=128,
	}
}
