
using System;

using Gtk; 

using SharpKnocking.Common;
using SharpKnocking.Common.Widgets;

namespace SharpKnocking.Doorman
{
	
	/// <summary>
	/// This class implements a dialgo to select the file from which a CallSequence
	/// will be deserialized.
	/// </summary>
	public class ExportCallFileChooserDialog
	{
		#region Glade widgets
		[Glade.WidgetAttribute]
		private FileChooserDialog exportCallFileChooserDialog;		
		
		[Glade.WidgetAttribute]
		private Button cancelButton;		
		
		[Glade.WidgetAttribute]
		private Button exportButton;	
	
		#endregion Glade widgets
		
		/// <summary>
		/// <c>ExportCallFileChooser</c>'s constructor method.
		/// </summary>
		/// <param name = "parent">
		/// The window this dialog is created from.
		/// </param>
		public ExportCallFileChooserDialog(Window parent)
		{
			Glade.XML gxml =
				new Glade.XML(null,"gui.glade","exportCallFileChooserDialog",null);
				
			gxml.Autoconnect(this);
			
			exportCallFileChooserDialog.TransientFor = parent;
			exportCallFileChooserDialog.Modal = true;
			
			exportCallFileChooserDialog.AddActionWidget(exportButton, ResponseType.Ok);
			exportCallFileChooserDialog.AddActionWidget(cancelButton, ResponseType.Cancel);
			
			FileFilter callFilter = new FileFilter();
			callFilter.Name = "Secuencia de llamada";
			callFilter.AddPattern("*.call");
			callFilter.AddPattern("*.CALL");
			
			FileFilter xmlFilter = new FileFilter();
			xmlFilter.Name = "Documentos XML";
			xmlFilter.AddPattern("*.xml");
			xmlFilter.AddPattern("*.XML");
			
			FileFilter allFilter = new FileFilter();
			allFilter.Name = "Todos los archivos";
			allFilter.AddPattern("*.*");	
			
			exportCallFileChooserDialog.AddFilter(callFilter);
			exportCallFileChooserDialog.AddFilter(xmlFilter);
			exportCallFileChooserDialog.AddFilter(allFilter);
		}
		
		#region Properties
		
		/// <summary>
		/// This property allows to retrieve the selected filename.
		/// </summary>
		public string Filename
		{
			get
			{
				return exportCallFileChooserDialog.Filename;
			}
		}
		
		/// <summary>
		/// This property allows the retrieving of the object which
		/// implements the window.
		/// </summary>
		public Window Window
		{
			get
			{
				return exportCallFileChooserDialog;
			}
		}
		
		#endregion Properties
		
		#region Public methods
		
		/// <summary>
		/// This method allows to wait until the dialog is closed or has sent
		/// a response.
		/// </summary>
		public ResponseType Run()
		{
			return (ResponseType)(exportCallFileChooserDialog.Run());
		}
		
		/// <summary>
		/// This method allows to destroy de dialog.
		/// </summary>
		public void Destroy()
		{
			exportCallFileChooserDialog.Destroy();
		}
		
		#endregion Public methods
	}
}
