// SystemProcess.cs
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
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;

using IptablesNet.Core.Util;

using Developer.Common.Unix.Native;

namespace IptablesNet.Core.Util
{
	/// <summary>
	/// Models a process wrapper
	/// </summary>
	public class SystemProcess: IDisposable
	{
		public static SystemProcess GetNewProcess(string program, string args, int errCapacity, int outputCapacity)
		{
			ProcessStartInfo psi = new ProcessStartInfo();
			psi.FileName = program;
			psi.Arguments = args;
			psi.CreateNoWindow = true;
			psi.ErrorDialog = false;
			psi.RedirectStandardOutput = true;
			psi.RedirectStandardError = true;
			
			SystemProcess sProc = new SystemProcess(psi,errCapacity, outputCapacity);
			return sProc;
		}
		
		private Process innerProc;
		private StreamWriter comSw;
		private string tmpComFile; 
		
		/// <value>
		/// Process object behing the wrapper
		/// </value>
		public Process InnerProcess
		{
			get {return this.innerProc;}
		}
		
		//Error list
		private CircularList<string> errList;
		
		public bool HasErrors
		{
			get {return this.errList.Count>0;}
		}
		
		//Output messages list
		private CircularList<string> outList;
		
		public bool HasOutput
		{
			get {return this.outList.Count>0;}
		}
		
		///<summary>
		/// Creates a new process redirecting the output and input. The input
		/// is taken from a temporal file.
		///</summary>
		private SystemProcess(ProcessStartInfo psi, int errCapacity, int outCapacity)
		{
			this.innerProc = new Process();
			this.innerProc.StartInfo = psi;
			
			this.innerProc.ErrorDataReceived += new DataReceivedEventHandler(this.OnErrorDataReceived);
			this.innerProc.OutputDataReceived += new DataReceivedEventHandler(this.OnOutputDataReceived);
			this.innerProc.EnableRaisingEvents = true;
			
			tmpComFile = UnixNative.CreateTempFileName();
			comSw = File.CreateText(tmpComFile);
			this.comSw.AutoFlush = true;
			this.errList = new CircularList<string>(errCapacity);
			this.outList = new CircularList<string>(outCapacity);
		}
		
		public void WriteToProcess(string data)
		{
			this.comSw.Write(data);
		}
		
		private void OnErrorDataReceived(object sender, DataReceivedEventArgs args)
		{
			this.errList.Add (args.Data);
		}
					
		private void OnOutputDataReceived(object sender, DataReceivedEventArgs args)
		{
			this.outList.Add (args.Data);
		}

		public virtual Void Dispose()
		{
			this.comSw.Flush();
			this.comSw.Close();
			//Send first a friendly sigterm
			this.innerProc.Close();
			//Then wait patiently for 250 ms
			this.innerProc.WaitForExit(250);
			//And finally kill it if it doesn't ended by itself
			if(!this.innerProc.HasExited)
				this.innerProc.Kill();
			
			this.innerProc.Dispose();
			File.Delete (this.tmpComFile);
			this.comSw.Dispose();
		}
		
		public void FlushOutputs()
		{
			this.errList.Clear();
			this.outList.Clear();
		}

		public string GetErrors()
		{
			string result = String.Empty; 
			foreach(string str in this.errList.ToArray())
				result+=str+"\n";
			return result;
		}
	}
}
