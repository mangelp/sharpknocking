// project created on 20/04/2007 at 21:50
using System;
using Gtk;

namespace IptablesNet.UI.Desktop
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			MainWindow win = new MainWindow ();
			win.Show ();
			Application.Run ();
		}
	}
}