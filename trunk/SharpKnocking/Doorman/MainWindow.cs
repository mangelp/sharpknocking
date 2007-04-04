using System;
using System.IO;
using System.Threading;
using System.Diagnostics;

using Gtk;
using Glade;

using SharpKnocking.Common;
using SharpKnocking.Common.Calls;
using SharpKnocking.Common.Widgets;
using SharpKnocking.Common.Remoting;
using SharpKnocking.Doorman.Remoting;
using SharpKnocking.Common.Widgets.CommonDialogs;

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
		
		private NodeStoreFilter storeFilter;
		
		
		#endregion Non Glade-related attributes
	
		#region Public
		
		public MainWindow () 
		{
			
			Glade.XML gxml = new Glade.XML (null, "gui.glade", "mainWindow", null);
			gxml.Autoconnect (this);
			
			mainWindow.Icon= ImageResources.SharpKnockingIcon22;
			
			InitializeWidgets();
			
			Init();
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
		    //callsStore.AddNode(newNode);
		    storeFilter.Add(newNode);
		    return newNode;
		}
		
		/// <summary>
		/// This method manages the programs exit.
		/// </summary>
		public void Quit()
		{		
		
			// First we confirm
			
			ResponseType res = 
				ConfirmDialog.Show(mainWindow, "¿Realmente deseas cerrar Doorman?");
			
			if(res == ResponseType.Yes)
			{	
			    
			    SaveData();
				
				res = ConfirmDialog.Show(						
						mainWindow, 
						"¿Desea cerrar también el daemon de apertura de puertos");
				
				if(res == ResponseType.Yes)
				{
					// TODO: Implement the closing of the daemon.
					daemonComm.SendCommand(RemoteCommandActions.Die);
				}
				
				Application.Quit ();
			}		
		}
		
		/// <summary>
		/// Shows the dialog.
		/// </summary>
		public void Show()
		{	
			//mainWindow.SkipTaskbarHint = false;		
			mainWindow.ShowAll();
			//mainWindow.Deiconify();
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
		    
		    txtFilter.Text = "";
		    
		    foreach(TreeViewColumn col in callsView.Columns)
		   		col.QueueResize();	
		   		
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
					ResponseType res = ConfirmDialog.Show(
							chooserDialog.Window,
							"Ya existe un fichero «{0}» en la carpeta seleccionada.\n"+
							"¿Desea sobreescribirlo?",
							Path.GetFileName(filename));
							
					if(res == ResponseType.Yes)
					{
						node.Sequence.Store(filename);
					}		
				}
				else
				{
				    node.Sequence.Store(filename);
				}
				
			}
			
			chooserDialog.Destroy();
			
		}		
		
		private void Init()
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
            
            if(!UnixNative.ExistsLockFile())            
            {
            	OkDialog.Show(
            		mainWindow,
            		MessageType.Info,
            		"El daemon no esta corriendo, se intentará lanzar ahora.");            		
            	
            	string daemonPath = WhichWrapper.Search("knockingdaemon");
            	
            	if(daemonPath == null)            	
            	{
            		OkDialog.Show(
            			mainWindow, 			
            			MessageType.Error,
            			"El daemon de monitorización de paquetes no se encuentra en el sistema.\n"+
            			"Doorman se cerrará inmediatamente.");
            		
            		mainWindow.Destroy();
            			
            	}
            	else
            	{
            		Process.Start(daemonPath);            		
            	}
            }
            
            // We create the communication system with the daemokn.
		    daemonComm = new DaemonCommunication();
		    daemonComm.AccessRequest +=	
		    	new AccessRequestEventHandler(OnAccessRequested);
		    	
		    daemonComm.Init();
            
		}
		
		
		private void InitializeWidgets()
		{
			btnExportImage.FromPixbuf = ImageResources.FileExportIcon16;
			itmExportImage.FromPixbuf = ImageResources.FileExportIcon16;
			itmExport.Sensitive = false;
		
		    // TreeView model inicialization.
			callsStore =  new NodeStore(typeof(CallNode));
			
			
			
			callsView = new NodeView(callsStore);
			
			storeFilter = new NodeStoreFilter(callsStore, "Address");
			
			callsView.RulesHint = true;
			callsView.ExpanderColumn = null;
			callsView.ShowExpanders = false;		
			callsViewScroll.Add(callsView);			
						
		    callsView.HeadersVisible=true;
		    
		    CellRendererToggle crt = new CellRendererToggle();		    
     		crt.Activatable = true;
     		crt.Toggled += OnCallActivationStateChanged;    		
		    
		    callsView.AppendColumn("Activa", crt,"active",0);
		    
		    TreeViewColumn descColumn = 
		    	new TreeViewColumn("Descripción", new CellRendererText (),"text",1);		    
		   	    
		    callsView.AppendColumn (descColumn);
		    
		    TreeViewColumn dirColumn =  
		    	new TreeViewColumn("Dirección IP", new CellRendererText (),"text",2);
		    
		    callsView.AppendColumn(dirColumn);
			
			callsView.AppendColumn ("Puerto", new CellRendererText(),"text",3);
			
			
            
			// We set the selection change event handler.
			callsView.NodeSelection.Changed += OnCallsViewSelectionChanged;	
			callsView.RowActivated += OnCallsViewRowActivated;
			
			callsView.ShowAll();	
			
			mainWindow.Realized += OnWindowRealized;
		}
		
		private void OnWindowRealized(object o, EventArgs a)
		{
			if (!UnixNative.ExecUserIsRoot())
			{
		        OkDialog.Show(
		        	null, 
		        	MessageType.Info, 
		        	"No puedes ejecutar la aplicación si no eres root");
		        			       
		        Application.Quit();
		    }
		}
		
		
		
		
		private void SaveData()
		{
		    DoormanConfig config = new DoormanConfig();
		    
		    foreach(CallNode callNode in storeFilter.Nodes)
		    {
		        config.AddCall(callNode.Sequence, callNode.Active);
		    }
		    
		    config.Save();
		    
		    // Now we have to restart the daemon, so the new rules are applied.
		    daemonComm.SendCommand(RemoteCommandActions.HotRestart);	    
		    
		}
		
		#endregion Private methods
		
		#region Event handlers
		
		private void OnAccessRequested(object sender, AccessRequestEventArgs a)
		{
			// We ask the user what he wants to do.
			ResponseType res = 
				ConfirmDialog.Show( 
					mainWindow,
					"Se detectó la secuencia de llamada «{0}» que abre el " +
					"puerto {1} proveniente de {2}.\n"+
					"¿Desea abrirlo?",
					a.CallSequence.Description,
					a.CallSequence.TargetPort,
					a.SourceIP);
			
			// Then we tell the daemon.
			if(res == ResponseType.Yes)
				daemonComm
				.SendCommand(RemoteCommandActions.Accept);
			else
				daemonComm.SendCommand(RemoteCommandActions.Deny);
				
		}
		
		private void OnBtnAddClicked(object sender, EventArgs a)
		{
			txtFilter.Text = "";
			AddCall();						
		}
		
		private void OnBtnBlockClicked(object sender, EventArgs a)
		{
			string msg;
			if(!trafficBlocked)
			{
				msg = 	"¿Desea bloquear la apertura de puertos?\n" +
						"Esto también cerrará los puertos que estuvieran abiertos.";
			}
			else
			{
				msg = 	"¿Desea activar la apertura de puertos?";
			}
			
			ResponseType res = ConfirmDialog.Show(mainWindow, msg);
			
			if(res == ResponseType.Yes)
			{
				daemonComm.SendCommand(RemoteCommandActions.Stop);
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
			ResponseType res =
				ConfirmDialog.Show(
					mainWindow,                                     
                    "¿Realmente quieres borrar la secuencia de llamada \n«{0}»?",
                    selectedNode.Description);
            
            if(res == ResponseType.Yes)
            {                  
            	txtFilter.Text = "";
				//callsStore.RemoveNode(callsView.NodeSelection.SelectedNode);
				storeFilter.Remove(callsView.NodeSelection.SelectedNode);		
			}
		}
		
		private void OnBtnSaveClicked(object sender, EventArgs a)
		{
			SaveData();
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
			AppInfoDialog.Show(
				mainWindow,
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
				CallSequence imported = CallSequence.LoadFromFile(icfcd.Filename);
				
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
		    btnClearFilter.Sensitive=txtFilter.Text.Length > 0;
		   
		    storeFilter.Filter = txtFilter.Text.Trim();
		}
		
		// Connect the Signals defined in Glade
		private void OnWindowDeleteEvent (object sender, DeleteEventArgs a) 
		{			
			
			//Quit();
			mainWindow.Hide();
			a.RetVal = true;
			
		}
		
		#endregion Event handlers
	}

}
