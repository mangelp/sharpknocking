
using System;
using System.Threading;

using Gtk;

using SharpKnocking.Common;
using SharpKnocking.Common.Widgets;
using SharpKnocking.Common.Widgets.CommonDialogs;

namespace SharpKnocking.Doorman
{	
	
	public class MainClass
	{
		
		public static void Main(string [] args)
		{
			// We start the application.
		    Application.Init ();
		    
		    // Activate debugging
		    Debug.DebugEnabled = true;
		    Debug.MoreVerbose = true;
			
			if(!UnixNative.ExecUserIsRoot())
			{
				OkDialog.Show(
					null,
					MessageType.Warning,
					"Debe tener privilegios para ejecutar Doorman.\n"+
					"El programa terminará ahora.");
			}
			else
			{
				new TrayIcon();	
				Application.Run();		
			}
		}
	}
}
