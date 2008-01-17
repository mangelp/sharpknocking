// CmdLineOptionsParser.cs
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
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using Developer.Common.Types;

namespace Developer.Common.Options
{
	/// <summary>
	/// Option parser for command lines
	/// </summary>
	public class CmdLineOptionsParser
	{
		private Type methodsOwner;
		private object methodsOwnerInstance;
		private List<CmdLineOption> options;
		
		/// <summary>
		/// Gets an option with the name if it exists
		/// </summary>
		public CmdLineOption this[string name]
		{
			get {
				//The array is kept ordered by insertion so we can use binary
				//search but we need an option object to do so
				CmdLineOption opt = new CmdLineOption(name);
				int pos = this.options.BinarySearch(opt);
				if(pos>=0)
					return this.options[pos];
				return null;
			}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		public CmdLineOptionsParser()
		{
			this.options = new List<CmdLineOption>();
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="methodsOwner">
		/// A <see cref="Type"/> of the object that holds the static methods to call
		/// </param>
		public CmdLineOptionsParser(Type methodsOwner)
		{
			this.options = new List<CmdLineOption>();
			this.methodsOwner = methodsOwner;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="methodsOwnerInstance">
		/// A <see cref="System.Object"/> reference to the instance of the object
		/// with the methods to execute.
		/// </param>
		public CmdLineOptionsParser(object methodsOwnerInstance)
		{
			this.options = new List<CmdLineOption>();
			this.methodsOwnerInstance = methodsOwnerInstance;
			//This and the delegate must be null to see that we want to use
			//the instance
			this.methodsOwner = null;
		}
		
	    /// <summary>
	    /// Extracts the parameter list from a string. 
	    /// </summary>
	    /// <remarks>
	    /// Simply returns an array of objects where two properties are set,
	    /// the parameter name and the parameter value. This is helpful when
	    /// trying to analize the input.
	    /// </remarks>
	    public SimpleParameter[] ProcessParameters(string line)
	    {
			Console.WriteLine("Get parameter list'1");
			//Convert the line to a list of strings and pass it to the overload
			//of this method that does the work
			return ProcessParameters(StringUtil.BreakCommandLine(ref line).ToArray());
	    }

		/// <summary>
		/// Gets an array of parameters from an array of strings
		/// </summary>
		/// <param name="args">
		/// A <see cref="System.String"/>
		/// </param>
		/// <remarks>
		/// Simply returns an array of objects where two properties are set,
	    /// the parameter name and the parameter value. This is helpful when
	    /// trying to analize the input.
		/// </remarks>
		/// <returns>
		/// A <see cref="SimpleParameter"/> array with all the parameters found
		/// in the command line even if there are no options set to match them
		/// </returns>
		public SimpleParameter[] ProcessParameters(string[] args)
		{
	        //Temporal list to add parameters as they are found
	        List<SimpleParameter> temp = new List<SimpleParameter>();
	        int pos = 0;
	        bool negateNext = false;
	        SimpleParameter par = null;
			
			//Console.WriteLine("Procesing "+args.Length+" parameters");
			
	        while (pos < args.Length) {
				
				//Console.WriteLine("Args["+pos+"]= "+args[pos]);
				//Console.WriteLine("Pos: "+pos);
				
	            //If the part starts with - is a parameter. If not it is something
	            //unknowm like the values of previous parameters
				
				if (String.IsNullOrEmpty(args[pos])) {
					throw new OptionParserException("Found an empty string");
				} else if (args[pos].StartsWith("-")) {
	                par = new SimpleParameter();
					par.Value = "";
	                bool longFormat = SimpleParameter.CheckLongFormat(args[pos]);
                    //Check the format of the options and get the name of the option
                    if (longFormat)
                        par.Name = args[pos].Substring(2);
                    else
                        par.Name = args[pos].Substring(1);
	                
					if (String.IsNullOrEmpty(par.Name))
						throw new OptionParserException("The parameter "+pos+" has an empty name");
	                //Check if the next parameter is a negated one
	                if (negateNext) {
						//Console.WriteLine("Negating "+par.Name);
	                    par.Not = true;
	                    negateNext = false;
	                }
					//Console.WriteLine("Adding parameter "+par);
					temp.Add(par);
				} else if (args[pos].StartsWith("!") && args[pos].Length == 1) {
					//Console.WriteLine("Next have to be negated");
					negateNext = true;
				} else if (negateNext) {
					throw new OptionParserException("Invalid '!' found before a value and after "+par.Name+" parameter.");
				} else if (par == null) {
					throw new OptionParserException("Bad format. Found value without option");
				} else {
					//Console.WriteLine("Adding value "+args[pos]+" to "+par.Name);
					par.Value += String.IsNullOrEmpty(par.Value) ? args[pos] : " " + args[pos];
				}
				//Increment the counter
				pos++;
	        }
			
			for (int i=0; i<temp.Count; i++) {
				if (this.ProcessParameter(temp[i], i))
					break;
			}
			
	        //The result is an array of parameters
	        return temp.ToArray();
		}
		
		/// <summary>
		/// Adds a new option to the parser to perform user-defined actions when
		/// a parameter that matches the option name is found.
		/// </summary>
		/// <param name="name">
		/// A <see cref="System.String"/>
		/// </param>
		public void AddOption(string name)
		{
			if (String.IsNullOrEmpty(name))
				throw new ArgumentException("The option name can't be null or empty");
			
			this.AddOption(name, name.Length > 1, null, null);
		}
		
		public void AddOption(string name, params string[] aliases)
		{
			if (String.IsNullOrEmpty(name))
				throw new ArgumentException("The option name can't be null or empty");
			
			this.AddOption(name, name.Length > 1, null, null, aliases);
		}

		/// <summary>
		/// Adds a new option to the parser to perform user-defined actions when
		/// a parameter that matches the option name is found.
		/// </summary>
		/// <param name="name">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="pdel">
		/// A <see cref="OptionProcesingDelegate"/>
		/// </param>
		public void AddOptionWithDelegate(string name, OptionProcessingDelegate pdel)
		{
			if (String.IsNullOrEmpty(name))
				throw new ArgumentException("The option name can't be null or empty");
			
			this.AddOption(name, name.Length > 1, String.Empty, pdel);
		}
		
		public void AddOptionWithDelegate(string name, OptionProcessingDelegate pdel, params string[] aliases)
		{
			if (String.IsNullOrEmpty(name))
				throw new ArgumentException("The option name can't be null or empty");
			
			this.AddOption(name, name.Length > 1, String.Empty, pdel, aliases);
		}
		
		/// <summary>
		/// Adds a new option to the parser to perform user-defined actions when
		/// a parameter that matches the option name is found.
		/// </summary>
		/// <param name="name">
		/// A <see cref="System.String"/> with the name of the option
		/// </param>
		/// <param name="isLong">
		/// A <see cref="System.Boolean"/> with the type of the option
		/// </param>
		/// <param name="pdel">
		/// A <see cref="CmdLineOption.OptionProcesing"/> with the delegate to
		/// perform custom procesing on the option if it is found
		/// </param>
		public void AddOptionWithDelegate(string name, OptionProcessingDelegate pdel, bool isLong)
		{
			if (String.IsNullOrEmpty(name))
				throw new ArgumentException("The option name can't be null or empty");
			
			this.AddOption(name, isLong, String.Empty, pdel);
		}
		
		public void AddOptionWithDelegate(string name, OptionProcessingDelegate pdel, bool isLong, params string[] aliases)
		{
			if (String.IsNullOrEmpty(name))
				throw new ArgumentException("The option name can't be null or empty");
			
			this.AddOption(name, isLong, String.Empty, pdel, aliases);
		}
		
		public void AddOptionWithMethod(string name, string method)
		{
			if (String.IsNullOrEmpty(name))
				throw new ArgumentException("The option name can't be null or empty");
			
			this.AddOption(name, name.Length > 1, method, null);
		}
		
		public void AddOptionWithMethod (string name, string method, params string[] aliases)
		{
			this.AddOption(name, name.Length > 1, method, null, aliases);
		}
		
		private void AddOption(string name, bool isLong, string methodName, 
		                       OptionProcessingDelegate pdel, params string[] aliases)
		{
			//Console.WriteLine(
            //      String.Format("Adding option Name:{0}, IsLong:{1}, MethodName:{2}, HasDelegate:{3}, Args:{4}",
            //                    name, isLong, methodName, pdel!=null, 
            //                    aliases.Length));
			
			CmdLineOption opt = new CmdLineOption(name, pdel, isLong);
			opt.MethodName = methodName;
			this.InternalAddOption(opt);
			//Console.WriteLine("+ Procesing aliases");
			foreach(string alias in aliases) {
				//Console.WriteLine("     * Procesing "+alias);
				opt = new CmdLineOption(alias, pdel);
				opt.MethodName = methodName;
				this.InternalAddOption(opt);
			}
		}
		
		//** Private stuff **//
		
		/// <summary>
		/// Calls a method using reflection
		/// </summary>
		/// <param name="data">
		/// A <see cref="OptionCallData"/>
		/// </param>
		private void CallMethod(OptionCallData data)
		{
			if (this.methodsOwner != null) {
				//If there is a methodOwner type specified use it to execute the method
				//that is required to be public and static
				//Console.WriteLine("Calling method: "+data.SourceOption.MethodName+" from "+this.methodsOwner.FullName+" type");
				BindingFlags flags = BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static;
				this.methodsOwner.InvokeMember(data.SourceOption.MethodName, flags, 
				                               null, null, new object[]{data});
			} else if (this.methodsOwnerInstance != null) {
				//If there is a methodOwnerInstance specified use it to execute the
				//method that is required to be public
				//Console.WriteLine("Calling method: "+data.SourceOption.MethodName+" from "+this.methodsOwnerInstance.GetType().FullName+" instance");
				BindingFlags flags = BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance;
				Type t = this.methodsOwnerInstance.GetType();
				t.InvokeMember(data.SourceOption.MethodName, flags, null, this.methodsOwnerInstance, new object[]{data});
			} else {
				throw new InvalidOperationException ("Can't call method "+data.SourceOption.MethodName
				                                     +". The source type or instance are not specified");
			}
				
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="opt">
		/// A <see cref="CmdLineOption"/>
		/// </param>
		private void InternalAddOption(CmdLineOption opt)
		{
			//Console.WriteLine("<Internal>: Adding option: "+opt.Name); 
			int pos = this.options.BinarySearch(opt);
			if(pos >= 0)
				throw new ArgumentException("The option "+opt.Name+" is already added");
			pos = ~pos;
			if(pos<this.options.Count)
				this.options.Insert(pos, opt);
			else
				this.options.Add(opt);		
		}
	    
		/// <summary>
		/// Processes only one parameter
		/// </summary>
		/// <param name="par">
		/// A <see cref="SimpleParameter"/> with the parameter to process
		/// </param>
		/// <param name="order">
		/// A <see cref="System.Int32"/> with the order of the parameter in the
		/// list.
		/// </param>
		private bool ProcessParameter(SimpleParameter par, int order)
		{
			//Console.WriteLine("ProcessingParameter: "+par);
			CmdLineOption opt = new CmdLineOption(par.Name);
			int pos = this.options.BinarySearch(opt);
			
			if (pos < 0) {
				//Console.WriteLine("Not an option");
				return false;
			}
			
			if (this.options[pos].IsLong != par.IsLongFormat)
				throw new OptionParserException("The option format doesn't match"+
				                                " the parameter format");
			
			//Console.WriteLine("Procesing option "+this.options[pos].Name);
			OptionCallData data = new OptionCallData();
			data.Parameter = par;
			data.SourceOption = this.options[pos];
			data.Order = order;
			//Console.WriteLine("Procesing option");
			if (this.options[pos].ProcesingDelegate != null)
				this.options[pos].ProcesingDelegate(data);
			else if (!String.IsNullOrEmpty(data.SourceOption.MethodName))
				this.CallMethod(data);
			
			if(data.AbortParsing)
				Console.WriteLine(data.ErrorMessage);
			
			return data.AbortParsing;
		}
	   
	}
}
