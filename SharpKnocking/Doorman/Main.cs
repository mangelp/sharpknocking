
using System;
using System.Threading;

using Gtk;

using SharpKnocking.Common;
using SharpKnocking.Common.Widgets;
using SharpKnocking.Common.Remoting;
using SharpKnocking.Common.Widgets.CommonDialogs;


namespace SharpKnocking.Doorman
{	
	
	public class MainClass
	{
		
		public static void Main(string [] args)
		{
		    // Here we do parameter processing
		    for(int i=0; i<args.Length; i++)
		    {
		        if(args[i]=="--dbg")
		        {
		            // Enable debugging
		            Debug.DebugEnabled = true;
		        }
		        else if(args[i]=="-v")
		        {
		            // Print more messages
		            Debug.MoreVerbose = true;
                    Debug.VerbLevel = VerbosityLevels.Normal;
		        }
                else if(args[i]=="-vv")
                {
                    Debug.MoreVerbose = true;
                    Debug.VerbLevel = VerbosityLevels.High;
                }
                else if(args[i]=="-vvv")
                {
                    Debug.MoreVerbose = true;
                    Debug.VerbLevel = VerbosityLevels.Insane;
                }
                else if(args[i]=="-h" || args[i]=="--help")
                {
                    PrintHelpMessage();
                }
				else
				{
					Console.Out.WriteLine ("Invalid argument or argument number. Use -h option for help");
					return 6;
				}
               	
		    }
		
			// We start the application.
		    Application.Init ();
			
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
		
        private static void PrintHelpMessage()
        {
            Console.Out.WriteLine ("Doorman manager for the SharpKnocking suite.");
            Console.Out.WriteLine ("Released under LGPL terms.");
            Console.Out.WriteLine ("(c)2007 Luís Roman Gutierrez y Miguel Ángel Pérez Valencia"); 
			Console.Out.WriteLine ("Url: http://code.google.com/p/SharpKnocking");
            Console.Out.WriteLine ("Commands: --dbg, -v, -vv, -vvv, -h, --help");
            Console.Out.WriteLine ("     --dbg, -v, -vv, -vvv: The first activates debuggin and the rest the level of");
            Console.Out.WriteLine ("       detail (more 'v' means more verbose).");
        }
	}
}
