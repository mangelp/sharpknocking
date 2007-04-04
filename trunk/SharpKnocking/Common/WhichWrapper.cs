
using System;
using System.Diagnostics;

namespace SharpKnocking.Common
{
	
	/// <summary>
	/// This class is used to retrieve a program's system path.
	/// </summary>
	public class WhichWrapper
	{
		/// <summary>
		/// Asks for a program's path.
		/// </summary>
		/// <param name = "programName">
		/// The name of the program which path is to be retrieved.
		/// </param>
		/// <returns>
		/// The path of the program, or null if it wasn't found.
		/// </returns>
		public static string Search(string programName)
		{
			Process p = new Process();
			p.StartInfo.FileName = "which";
			p.StartInfo.Arguments = programName;
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.RedirectStandardOutput = true;
			p.Start();			
			
			string path = "";
			path = p.StandardOutput.ReadLine();
			
			
			if(path == "")
			{				
				return null;
			}
			
			return path;		
		}		
	}
}
