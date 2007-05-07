// project created on 05/05/2007 at 13:46
using System;
using Gtk;

namespace IptablesNet.UI.Gtk
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