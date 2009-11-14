
using System;

using Gtk; 

namespace SharpKnocking.Common.Widgets
{
	
	/// <summary>
	/// This class implements a dialgo to select the file from which a CallSequence
	/// will be deserialized.
	/// </summary>
	public class ImportCallFileChooserDialog
	{
		#region Glade widgets
		[Glade.WidgetAttribute]
		private FileChooserDialog importCallFileChooserDialog;
		
		[Glade.WidgetAttribute]
		private Button btnOpen;
		
		[Glade.WidgetAttribute]
		private Button btnCancel;

		#endregion Glade widgets
		
		public ImportCallFileChooserDialog()
		{
			Glade.XML gxml = new Glade.XML(null,"gui.glade","importCallFileChooserDialog",null);
			gxml.Autoconnect(this);
			
			importCallFileChooserDialog.AddActionWidget(btnOpen, ResponseType.Ok);
			importCallFileChooserDialog.AddActionWidget(btnCancel, ResponseType.Cancel);
			
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
		
			importCallFileChooserDialog.AddFilter(callFilter);	
			importCallFileChooserDialog.AddFilter(xmlFilter);
			importCallFileChooserDialog.AddFilter(allFilter);
		}
		
		#region Properties
		
		/// <summary>
		/// This property allows to retrieve the selected filename.
		/// </summary>
		public string Filename
		{
			get
			{
				return importCallFileChooserDialog.Filename;
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
			return (ResponseType)(importCallFileChooserDialog.Run());
		}
		
		/// <summary>
		/// This method allows to destroy de dialog.
		/// </summary>
		public void Destroy()
		{
			importCallFileChooserDialog.Destroy();
		}
		
		#endregion Public methods
	}
}
