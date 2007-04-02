using System;
using System.IO;

using Gtk;
using Glade;

using SharpKnocking.Common;
using SharpKnocking.Common.Calls;
using SharpKnocking.Common.Widgets;
using SharpKnocking.Common.Remoting;
using SharpKnocking.Doorman.Remoting;

namespace SharpKnocking.Doorman
{
	public class MainWindow
	{
		#region GUI-related attributes
		
		[WidgetAttribute]
		private Window mainWindow;
		
		[WidgetAttribute]
		private ScrolledWindow callsViewScroll;
		
		[WidgetAttribute]
		private Entry txtFilter;
		
		[WidgetAttribute]
		private Button btnClearFilter;
		
		[WidgetAttribute]
		private Button btnRemove;
		
		[WidgetAttribute]
		private Button btnEdit;
		
		[WidgetAttribute]
		private Button btnExport;
		
		[WidgetAttribute]
		private Image btnExportImage;
		
		[WidgetAttribute]
		private Image itmExportImage;
		
		[WidgetAttribute]
		private MenuItem itmExport;
		
		#endregion
				
		#region Non Glade-related attributes
		
		private NodeStore callsStore;		
		private NodeView callsView;
		private DaemonCommunication daemonComm;		
		
		private bool trafficBlocked;
		
		
		#endregion Non Glade-related attributes
	
		#region Public
		
		public MainWindow () 
		{
			
			Glade.XML gxml = new Glade.XML (null, "gui.glade", "mainWindow", null);
			gxml.Autoconnect (this);
			
			mainWindow.Icon= ImageResources.SharpKnockingIcon22;
			
			InitializeWidgets();
			
			LoadData();
		}
		
		/// <summary>
		/// Call sequences can be added to the list invoking this method. 
		/// </summary>		
		/// <param name = "call">
		/// The call sequence which is going to be added.
		/// </param>		
		/// <param name = "status">
		/// Whether if the call is active or not.
		/// </param>
		public CallNode AddCallSequence(CallSequence call, bool status)
		{		    
		    CallNode newNode = new CallNode(call);
		    newNode.Active = status;
		    callsStore.AddNode(newNode);
		    return newNode;
		}
		
		/// <summary>
		/// This method manages 
		public void Quit()
		{		
		
			// First we confirm
			
			ConfirmDialog cd = new ConfirmDialog(mainWindow, "¿Realmente deseas cerrar Doorman?");
			
			if(cd.Run() == ResponseType.Yes)
			{	
			    try
			    {
			       SaveData();
			    }
			    catch(System.UnauthorizedAccessException)
			    {
			        OkDialog dlg = new OkDialog(mainWindow, MessageType.Info, 
			                              "No se tienen los permisos necesarios para"+
			                              "\nguardar la configuración de llamadas."+
			                              "\n\nSi quiere guardar cambios inicie la"+
			                              "\n aplicación como root.");
			        dlg.Run();
			    }
			    
				Application.Quit ();	
				
				ConfirmDialog cd1 = 
					new ConfirmDialog(
						mainWindow, 
						"¿Desea cerrar también el daemon de apertura de puertos");
				
				if(cd1.Run() == ResponseType.Yes)
				{
					// TODO: Implement the closing of the daemon.
				}
				
			}		
			
			cd.Destroy();
		}
		
		#endregion Public
		
		
		
		#region Private methods
		
		private void AddCall()
		{
			CallEditDialog dg=new CallEditDialog(mainWindow);
			
			ResponseType res;
			while((res = dg.Run()) == ResponseType.None)
			{
				;// Because it always give a response, even when it shouldn't.
			}			
			
			if(res == ResponseType.Ok)
			{
			    // We add the call sequence to the list.
			    CallNode node = AddCallSequence(dg.CallSequence, true);	    
			    
			    // Then we select it, and scroll the list if needed.
			    callsView.NodeSelection.UnselectAll();
			    callsView.NodeSelection.SelectNode(node);
			    
			    callsView.ScrollToCell(
			        callsView.Selection.GetSelectedRows()[0],
			        null,true,0,1);		    
			}
			else
			{
				Console.WriteLine(res);
			}
			dg.Destroy();
		}
		
		private void EditSelectedCall()
		{
		    CallNode selectedNode = 
		    	callsView.NodeSelection.SelectedNode as CallNode;
		    	
		    CallEditDialog ced = 
		    	new CallEditDialog(mainWindow, selectedNode.Sequence);		
		    	
		    ced.Run();  
		    ced.Destroy();  
		}
		
		private void ExportCall()
		{
			ExportCallFileChooserDialog chooserDialog =
				new ExportCallFileChooserDialog();
		
			if(chooserDialog.Run() == ResponseType.Ok)
			{		
				// We retrieve the selected call.
				CallNode node = callsView.NodeSelection.SelectedNode as CallNode;
				
				string filename = chooserDialog.Filename;
				if(!Path.HasExtension(filename))
				{
					filename += ".call";					
				}
				
				if(File.Exists(filename))
				{
					ConfirmDialog overwriteDialog = 
						new ConfirmDialog(
							chooserDialog.Window,
							"Ya existe un fichero «{0}» en la carpeta seleccionada.\n"+
							"¿Desea sobreescribirlo?",
							Path.GetFileName(filename));
					if(overwriteDialog.Run() == ResponseType.Yes)
					{
						node.Sequence.Store(filename);
					}
					
					overwriteDialog.Destroy();			
				}
				else
				{
				    node.Sequence.Store(filename);
				}
				
			}
			
			chooserDialog.Destroy();
			
		}		
		
		private void InitializeWidgets()
		{
			btnExportImage.FromPixbuf = ImageResources.FileExportIcon16;
			itmExportImage.FromPixbuf = ImageResources.FileExportIcon16;
			itmExport.Sensitive = false;
		
		    // TreeView model inicialization. Columns are: 'Description' 
		    // and 'IP address'.
			callsStore =  new NodeStore(typeof(CallNode));
			
			callsView = new NodeView(callsStore);
			
			callsView.RulesHint = true;
			callsView.ExpanderColumn = null;
			callsView.ShowExpanders = false;		
			callsViewScroll.Add(callsView);			
						
		    callsView.HeadersVisible=true;
		    
		    CellRendererToggle crt = new CellRendererToggle();		    
     		crt.Activatable = true;
     		crt.Toggled += OnCallActivationStateChanged;    		
		    
		    callsView.AppendColumn("Activada", crt,"active",0);
		    
		    TreeViewColumn dirColumn =  
		    	new TreeViewColumn("Dirección IP", new CellRendererText (),"text",1);
		    
		    callsView.AppendColumn(dirColumn);
			
			callsView.AppendColumn ("Puerto", new CellRendererText(),"text",2);
			callsView.AppendColumn ("Descripción", new CellRendererText (),"text",3);
            
			callsView.SearchEntry = txtFilter;			
			callsView.SearchColumn = 0;
			
			// We set the selection change event handler.
			callsView.NodeSelection.Changed += OnCallsViewSelectionChanged;	
			callsView.RowActivated += OnCallsViewRowActivated;
			
			callsView.ShowAll();	
		}
		
		private void LoadData()
		{
		    // We have to check that we had stored a config file.
		    
		    DoormanConfig config = DoormanConfig.Load();
		    	    
		    if(config != null)
		    {
		    	//There is a config file
		        foreach(CallSequence sequence in config.CallSequences)
		        {
		            AddCallSequence(sequence,
		            				config.GetActivationStatus(sequence));
		        }
		    }		    
            
            this.daemonComm = new DaemonCommunication();
            this.daemonComm.Init();
		}
		
		
		
		private void SaveData()
		{
		    DoormanConfig config = new DoormanConfig();
		    
		    foreach(CallNode callNode in callsStore)
		    {
		        config.AddCall(callNode.Sequence, callNode.Active);
		    }
		    
		    config.Save();
		}
		
		#endregion Private methods
		
		#region Event handlers
		
		private void OnBtnAddClicked(object seder,EventArgs a)
		{
			AddCall();						
		}
		
		private void OnBtnBlockClicked(object sender, EventArgs a)
		{
			string msg;
			if(trafficBlocked)
			{
				msg = 	"¿Desea bloquear la apertura de puertos?" +
						"Esto también cerrará los puertos que estuvieran abiertos.";
			}
			else
			{
				msg = 	"¿Desea activar la apertura de puertos?";
			}
			
			ConfirmDialog cd = new ConfirmDialog(mainWindow, msg);
			if(cd.Run() == ResponseType.Yes)
			{
				
			}
		}
		
		private void OnBtnCallClicked(object sender, EventArgs a)
		{
			// The selected node is retrieved.
			CallNode selected = callsView.NodeSelection.SelectedNode as CallNode;
			
			selected.Sequence.PerformCall();
		}
		
		private void OnBtnClearFilterClicked(object sender, EventArgs a)
		{
		    //When the clear button is clicked, we clear the entry box		   
		    txtFilter.Text="";
		}	
		
		private void OnBtnCloseClicked(object sender, EventArgs a)
		{			
			Quit();
		}
		
		private void OnBtnEditClicked(object sender, EventArgs a)
		{
		    EditSelectedCall();// We delegate in another method.
		}

		private void OnBtnRemoveClicked(object sender, EventArgs a)
		{
		
			CallNode selectedNode = (CallNode)(callsView.NodeSelection.SelectedNode);
			
			// First, we have to confirm.				
			MessageDialog confirmDialog = new MessageDialog (mainWindow, 
                                      DialogFlags.DestroyWithParent,
                                   	  MessageType.Question, 
                                      ButtonsType.YesNo,
                                      "¿Realmente quieres borrar la secuencia de llamada \n«"+
                                      selectedNode.Description+"»?");
            
            confirmDialog.Title = "Pregunta";
            
            if(confirmDialog.Run()  == (int)ResponseType.Yes)
            {                  
				callsStore.RemoveNode(callsView.NodeSelection.SelectedNode);			
			}	
			
			confirmDialog.Destroy();
		}
		
		private void OnCallActivationStateChanged(object sender, ToggledArgs a)
		{
			// Argh
		    CallNode node =  callsStore.GetNode(new TreePath(a.Path)) as CallNode;
		    node.Active = !node.Active;
		}
		
		private void OnCallsViewRowActivated(object sender, RowActivatedArgs a)
		{
		    EditSelectedCall();// We delegate in another method.
		}   
		

		private void OnCallsViewSelectionChanged(object sender, EventArgs a)
		{
		   	// When a row is selected in the treeview, we enable the buttons
		  	// on the left panel.
		   	bool somethingSelected = 
		   		callsView.NodeSelection.SelectedNode != null;
		   
		   	btnRemove.Sensitive = somethingSelected;
		   	btnEdit.Sensitive = somethingSelected;
		   	btnExport.Sensitive = somethingSelected;
		   	itmExport.Sensitive = somethingSelected;
		   
		}
		
		private void OnExportClicked(object sender, EventArgs a)
		{
			ExportCall();
		}

		private void OnItmAboutActivated(object sender, EventArgs a)
		{
			AppInfoDialog ad = new AppInfoDialog(
				"Doorman",
				"Esta aplicación del proyecto SharpKnocking permite establecer"+
				" que «llamadas» abrirán los puertos asociados a las mismas.");
		}
		
		private void OnItmAddActivated(object sender, EventArgs a)
		{
			AddCall();
		}
		
		private void OnItmImportActivated(object sender, EventArgs a)
		{
			ImportCallFileChooserDialog icfcd = new ImportCallFileChooserDialog();
			if(icfcd.Run() == ResponseType.Ok)
			{
				// We have selected a valid? file
				CallSequence imported = CallSequence.Load(icfcd.Filename);
				
				AddCallSequence(imported,true);
			}
			
			icfcd.Destroy();
		}		
		
		private void OnItmQuitActivated(object sender, EventArgs a)
		{
			Quit();
		}
		
		private void OnItmSaveActivated(object sender, EventArgs a)
		{
			SaveData();
		}

		
		
		private void OnTxtFilterChanged(object sender, EventArgs a)
		{
		    //The clear button is activated only when the searched text exists.
		    btnClearFilter.Sensitive=txtFilter.Text.Length>0;
		}
		
		// Connect the Signals defined in Glade
		private void OnWindowDeleteEvent (object sender, DeleteEventArgs a) 
		{
			Quit();
			a.RetVal = true;
			
		}
		
		#endregion Event handlers
	}

}