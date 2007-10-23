
using System;
using System.Diagnostics;

using Developer.Common.SystemCommands;

namespace Developer.Common.Unix.SystemCommands
{	
	/// <summary>
	/// This class is used to retrieve a program's system path.
	/// </summary>
	public static class SearchWrapper
	{
		/// <summary>
		/// Asks for a program's path.
		/// </summary>
		/// <param name = "programName">
		/// The name of the program which path is to be retrieved.
		/// </param>
		/// <returns>
		/// The path of the program if was found. If not returns null or empty.
		/// </returns>
		public static string WhichSearch(string programName)
		{
			Process p = new Process();
			p.StartInfo.FileName = "which";
			p.StartInfo.Arguments = programName;
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.RedirectStandardOutput = true;
			p.Start();
			
			string path = String.Empty;
			path = p.StandardOutput.ReadLine();
			
			return path;		
		}		
	}
}
