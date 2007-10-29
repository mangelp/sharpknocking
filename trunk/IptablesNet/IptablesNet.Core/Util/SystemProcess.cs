// /home/mangelp/Projects/sharpknocking/IptablesNet/IptablesNet.Core/Util/SystemProcess.cs created with MonoDevelop at 0:54 12/06/2007 by mangelp 
//
//This project is released under the terms of the LGPL V2. See the file lgpl.txt for details.
//(c) 2007 SharpKnocking projects and authors (see AUTHORS).

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
