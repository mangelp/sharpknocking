// project created on 11/01/2007 at 13:21
using System;
using Gtk;

using SharpKnocking.Common;

namespace SharpKnocking.PortKnocker
{
	public class MainClass
	{
	
		public static void Main (string[] args)
		{
		    Debug.VerbLevel = VerbosityLevels.Insane;
		    Debug.DebugEnabled = true;
		    Debug.MoreVerbose = true;
		    
		    // We start the application.
		    Application.Init ();
			new MainWindow ();
			Application.Run();				
		}
	}
}