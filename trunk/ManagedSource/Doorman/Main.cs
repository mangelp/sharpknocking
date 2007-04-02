
using System;
using System.Threading;

using Gtk;

using SharpKnocking.Common;
using SharpKnocking.Common.Widgets;

namespace SharpKnocking.Doorman
{	
	
	public class MainClass
	{
		
		public static void Main(string [] args)
		{
			// We start the application.
		    Application.Init ();
		    
		    //Activate debuggin
		    Debug.DebugEnabled = true;
		    Debug.MoreVerbose = true;

//		    if (!UnixNative.ExecUserIsRoot())
//		    {
//		        OkDialog dlg = new OkDialog(null, MessageType.Info, "No puedes ejecutar la aplicación si no eres root");
//		        return;
//		    }
//		    else
//		    {
			    //new MainWindow ();
			    new TrayIcon();
//			}
			
            Application.Run();
		}
	}
}
