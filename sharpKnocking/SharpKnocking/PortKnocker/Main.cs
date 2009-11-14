// project created on 11/01/2007 at 13:21
using System;
using Gtk;

using SharpKnocking.Common;

namespace SharpKnocking.PortKnocker
{
	public class MainClass
	{
	
		public static int Main (string[] args)
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
			new MainWindow ();
			Application.Run();		
			
			return 0;
		}
		
        private static void PrintHelpMessage()
        {
            Console.Out.WriteLine ("PortKnocker knocking client for the SharpKnocking suite.");
            Console.Out.WriteLine ("Released under LGPL terms.");
            Console.Out.WriteLine ("(c)2007 Luís Roman Gutierrez y Miguel Ángel Pérez Valencia"); 
			Console.Out.WriteLine ("Url: http://code.google.com/p/SharpKnocking");
            Console.Out.WriteLine ("Commands: --dbg, -v, -vv, -vvv, -h, --help");
            Console.Out.WriteLine ("     --dbg, -v, -vv, -vvv: The first activates debuggin and the rest the level of");
            Console.Out.WriteLine ("       detail (more 'v' means more verbose).");
        }
	}
}