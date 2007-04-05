using System;
using System.IO;

using Gtk;
using Glade;

using SharpKnocking.Common;
using SharpKnocking.Common.Calls;
using SharpKnocking.Common.Widgets;
using SharpKnocking.Common.Widgets.CommonDialogs;

namespace SharpKnocking.PortKnocker
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
		private Button btnCall;
		#endregion
		
	    
		
		#region Non Glade-related attributes
		
		private NodeStore callsStore;
		
		private NodeView callsView;
		
		private NodeStoreFilter storeFilter;
		
		private static string configFilePath = 
		    Environment.GetEnvironmentVariable("HOME") + "/.portknocker";
		
		#endregion
	
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
		/// <param name="call">
		/// The call sequence which is going to be added.
		/// </param>		
		public CallNode AddCallSequence(CallSequence call)
		{		    
		    CallNode newNode = new CallNode(call);
		    
		    txtFilter.Text = "";
		    
		    storeFilter.Add(newNode);
		    
		    
		    
		    return newNode;
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
			    CallNode node = AddCallSequence(dg.CallSequence);	    
			    
			    // Then we select it, and scroll the list if needed.
			    callsView.NodeSelection.UnselectAll();
			    callsView.NodeSelection.SelectNode(node);
			    
			    callsView.ScrollToCell(
			        callsView.Selection.GetSelectedRows()[0],
			        null,true,0,1);		    
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
		    
		    // So the columns are resized properly.
		    foreach(TreeViewColumn col in callsView.Columns)
		   		col.QueueResize();
		    
		    ced.Destroy();
		    		    
		}
		
		private void ImportCall()
		{
			ImportCallFileChooserDialog icfcd = new ImportCallFileChooserDialog();
			if(icfcd.Run() == ResponseType.Ok)
			{
				// We have selected a valid? file
				CallSequence imported = CallSequence.LoadFromFile(icfcd.Filename);
				
				AddCallSequence(imported);
			}
			
			icfcd.Destroy();
		}
		
		private void InitializeWidgets()
		{
		    // TreeView model inicialization. Columns are: 'Description' 
		    // and 'IP address'.
			callsStore =  new NodeStore(typeof(CallNode));
			
			callsView = new NodeView(callsStore);
			
			storeFilter = new NodeStoreFilter(callsStore, "Address");
			
			callsView.RulesHint = true;
			callsView.ExpanderColumn = null;
			callsView.ShowExpanders = false;		
			callsViewScroll.Add(callsView);			
						
		    callsView.HeadersVisible=true;
		   
		    
		    callsView.AppendColumn ("Descripción", new CellRendererText (),"text",0);
		    
		    TreeViewColumn dirColumn = 	new TreeViewColumn(
		    								"Dirección IP",
		    								new CellRendererText (),
		    								"text", 1);
		    				
		    callsView.AppendColumn(dirColumn);
			
			callsView.AppendColumn ("Puerto", new CellRendererText(),"text",2);
			
            
			
			// We set the selection change event handler.
			callsView.NodeSelection.Changed += OnCallsViewSelectionChanged;	
			callsView.RowActivated += OnCallsViewRowActivated;
			
			callsView.ShowAll();	
		}
		
		private void LoadData()
		{
		    PortKnockerConfig config = PortKnockerConfig.Load();	    
		    if(config != null)
		    {
		        // There is a valid config file.
		            
		        foreach(CallSequence sequence in config.CallSequences)
		        {
		            AddCallSequence(sequence);
		        }
		    }		    
		}
		
		private void Quit()
		{		
		    SaveData();
			Application.Quit ();			
		}
		
		private void SaveData()
		{
		    PortKnockerConfig config =  new PortKnockerConfig();
		    foreach(CallNode callNode in callsStore)
		    {
		        config.AddCall(callNode.Sequence);
		    }
		    
		    config.Save();
		}
		
		#endregion Private methods
		
		#region Event handlers
		
		private void OnBtnAddClicked(object seder,EventArgs a)
		{
			AddCall();						
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
			// We delegate in another method.
		    EditSelectedCall();
		}
		
		
		private void OnBtnImportClicked(object sender, EventArgs a)
		{
			ImportCall();
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
		
		private void OnCallsViewRowActivated(object sender, RowActivatedArgs a)
		{
		    EditSelectedCall();// We delegate in another method.
		}   
		

		private void OnCallsViewSelectionChanged(object sender, EventArgs a)
		{
			//When a row is selected in the treeview, we enable the buttons
		   	//on the left panel.
			bool somethingSelected = 
		   		callsView.NodeSelection.SelectedNode != null;
		   
		   	btnRemove.Sensitive = somethingSelected;
		   	btnEdit.Sensitive = somethingSelected;
		   	btnCall.Sensitive = somethingSelected;
		   	
		}

		private void OnItmAboutActivated(object sender, EventArgs a)
		{
			AppInfoDialog.Show(
				mainWindow,
				"PortKnocker",
				"Esta es la aplicación que realiza tareas de «llamador» dentro"+
				" del proyecto SharpKnocking.");
		}
		
		private void OnItmAddActivated(object sender, EventArgs a)
		{
			AddCall();
		}
		
		private void OnItmImportActivated(object sender, EventArgs a)
		{
			ImportCall();
			
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
		    
		    storeFilter.Filter =  txtFilter.Text.Trim();
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
