
using System;
using Gtk;
using Glade;

namespace SharpKnocking.Common.Widgets
{
	
	
	/// <summary>
	/// This class implements the "About" dialog.
	/// </summary>
	public class AppInfoDialog
	{
		[WidgetAttribute]
		private Gtk.AboutDialog aboutDialog;
	
		/// <summary>
		/// This class implements a dialog to show info about a SharpKnoking
		/// suite's application.
		/// </summary>
		/// <param name = "title">
		/// The title of the application which info is going to be shown.
		/// </param>
		/// <param name = "msg">
		/// This parameter represents the description message it's shown
		/// in the dialog.
		/// </param>
		public AppInfoDialog(string title, string msg)
		{
			Glade.XML gxml = new Glade.XML (null, "gui.glade", "aboutDialog", null);
			gxml.Autoconnect (this);
			
			aboutDialog.Icon = ImageResources.SharpKnockingIcon22;
			aboutDialog.Logo = ImageResources.SharpKnockingIcon96;			
		    
		    aboutDialog.Name = title;
		    aboutDialog.Comments = msg + 
		    	"\n\nDesarrollado por:\n\n"+
		    	"Luis Román Gutiérrez\n"+
		    	"Miguel Ángel Pérez Valencia";
			
			aboutDialog.Artists=null;
			
			aboutDialog.Run();
			aboutDialog.Hide();		
		}
		
		
		
		
	}
}