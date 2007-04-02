
using System;
using System.Net;
using System.Collections;

using Gtk;
using Glade;

using SharpKnocking.Common;
using SharpKnocking.Common.Calls;
using SharpKnocking.Common.Widgets.CommonDialogs;


namespace SharpKnocking.Common.Widgets
{
	
	
	/// <summary>
	/// This class implements a dialog which is used for the creation and
	/// edition of "calls".
	/// </summary>
	public class CallEditDialog
	{
	    #region GUI-related attributes
	    
		[WidgetAttribute]
		private Dialog callEditDialog;
		
		[WidgetAttribute]
		private TreeView tvPorts;
		
		[WidgetAttribute]
		private Button btnAdd;		
		
		[WidgetAttribute]
		private Button btnRemove;		
		
		[WidgetAttribute]
		private Button btnUp;
				
		[WidgetAttribute]
		private Button btnDown;
		
		[WidgetAttribute]
		private Entry txtDescription;
		
		[WidgetAttribute]
		private SpinButton spIP1;
		
		[WidgetAttribute]
		private SpinButton spIP2;
		
		[WidgetAttribute]
		private SpinButton spIP3;
		
		[WidgetAttribute]
		private SpinButton spIP4;
		
		[WidgetAttribute]
		private SpinButton spTargetPort;
		
		#endregion GUI-related attributes
		
		#region Non GUI-related attributes
		
		private ListStore portsStore;	
		
		private bool modified;
		private bool editing;	
		
		private CallSequence callSequence;
		
		
	
		#endregion
	    
	    /// <summary>
	    /// CallEditDialog's default constructor creates a dialog which allow
	    /// to create a new call sequence.
	    /// </summary>
	    /// <param name = "parent">
		/// The dialog's parent <c>Window</c>.
		/// </param>
	    public CallEditDialog(Window parent)
	        : this(parent,null)
	    {
	    
	    }
		
		/// <summary>
		/// This constructor creates a dialog in which a call sequence can be
		/// modified.
		/// </summary>
		/// <param name = "parent">
		/// The dialog's parent <c>Window</c>.
		/// </param>
		/// <param name = "call">
		/// The sequence to be modified.
		/// </param>
		public CallEditDialog(Window parent, CallSequence call)
		{
			Glade.XML gxml = 
				new Glade.XML(null, "gui.glade", "callEditDialog", null);
				
			gxml.Autoconnect (this);
			
			
			callEditDialog.Icon = parent.Icon;
			callEditDialog.TransientFor = parent;
			callEditDialog.Modal = true;
						
			InitializeWidgets();
			
			// Now it's time to deal with the parameter.
			if(call == null)
			{
			    editing = false;
			}
			else
			{
			    editing = true;
			    LoadData(call);
			}		
			
			modified = false;
		}
		
		private void LoadData(CallSequence call)
		{
		    // We set the description.
		    txtDescription.Text=call.Description;
		    
		    // We split up the ip address...
		    string [] ipParts = call.Address.Split('.');
		    
		    spIP1.Value = Int32.Parse(ipParts[0]);
		    spIP2.Value = Int32.Parse(ipParts[1]);
		    spIP3.Value = Int32.Parse(ipParts[2]);
		    spIP4.Value = Int32.Parse(ipParts[3]);
		    
		    spTargetPort.Value = call.TargetPort;
		    
		    // Finally, we add the sequece's ports to the list.
		    foreach(int port in call.Ports)
		    {
		        portsStore.AppendValues(port);
		    }	
		    
		    callSequence = call;
		}
		
		/// <summary>
		/// This property allows to retrieve and set the call sequence we
		/// have been editing in the dialog.
		/// </summary>
		public CallSequence CallSequence
		{
		    get
		    {
		    	return callSequence;
		    }
		    
		    set
		    {
		    	LoadData(value);		    
		    }		
		}
		
		/// <summary>
		/// This method allows to wait until the dialog has been closed or
		/// it has emited an answer.
		/// </summary>
		public ResponseType Run()
		{
		    return (ResponseType)(callEditDialog.Run());		
		}
		
		/// <summary>
		/// This methods destroys the dialog.
		/// </summary>
		public void Destroy()
		{
			callEditDialog.Destroy();		
		}
		
		private void InitializeWidgets()
		{
		    //We are going to setup the properties of the TreeView.
		    portsStore=new ListStore(typeof(int));		    
		    
		    tvPorts.Model = portsStore;		    
		    tvPorts.AppendColumn ("Puerto", new CellRendererText (), "text", 0);	        
            tvPorts.Selection.Changed += OnTvPortsSelectionChanged;           
		}
		
		/// <summary>
		/// Here we set the sensitivity of the buttons which control the list's 
		/// rows movement.
		/// </summary>
		private void UpDownButtonsControl()
		{
		    if(tvPorts.Selection.CountSelectedRows() > 0)
		    {
    		    // We get the selected index (a so much complex expression is needed).
    		    int selectedIndex=tvPorts.Selection.GetSelectedRows()[0].Indices[0];
    		
    		    btnUp.Sensitive=selectedIndex>0;
    		    
    		    // IterNChildren() is such a very bad name for a method which 
    		    // retrieves the list's row count.
    		    btnDown.Sensitive=selectedIndex < portsStore.IterNChildren()-1;
		    }
		}
		
		private ResponseType ShowSaveQuestion()
		{
		    // A message dialog is used to ask the user
		    // whether to save the changes or not.		   
            ResponseType res = 
            	ConfirmDialog.Show(    
            		callEditDialog,                                    
                    "¿Estas seguro de guardar esta secuencia de llamada?");                    
     
            
            return res;	
		}
		
		/// <summary>
		/// Here we check all the values are correct.
		/// </summary>
		/// <param name = "ipAddress">
		/// An string containg an IP address
		/// </param>
		/// <returns>
		/// True if there were no errors, false otherwise.
		/// </returns>		
		private bool CheckValues(string ipAddress)
		{
		    string errors="";
		    
		    if(txtDescription.Text.Trim().Length == 0)
		    {
		        errors += " .- Debe introducir una descripción para la llamada.\n";
		    }
		    
		    if(portsStore.IterNChildren()==0)
		    {
		        errors += " .- No hay puertos en la lista de puertos.";
		    }		
		    
		    if(!Net20.TryParseIP(ipAddress))
		    {
		    	errors += " .- La dirección IP usada como destino no es válida.\n";
		    }
		    
		    if(errors.Length == 0)
		    {
		        // There were no errors, return true;
		        return true;
		    }
		    else
		    {
		        // The errors are shown.
		        OkDialog.Show(callEditDialog, MessageType.Error,
		        	          "No se pudo guardar la llamada porque:\n\n{0}",
		        	          errors);		        	          
		             
		        return false;
		    }
		    	
		}
		
		#region EventHandlers
		
		private void OnBtnAddClicked(object sender, EventArgs a)
		{
		    // We open a dialog to select the new port.
		    PortDialog pd = new PortDialog(callEditDialog);
		    
		    if(pd.Run() == ResponseType.Ok)
		    {
		        // The new port is inserted at the list's end.
		        TreeIter newRow = portsStore.AppendValues(pd.Value);
		        
		        // We deselect whetever port was selected.
		        tvPorts.Selection.UnselectAll();
		        
		        // Then the row that was added is selected.
		        tvPorts.Selection.SelectIter(newRow);
		        
		        // And finally, the list is scrolled if necessary.
		       	        
		        tvPorts.ScrollToCell(portsStore.GetPath(newRow),null,true,1f,0);
		        
		        modified = true;
            }		        
		}
		
		private void OnBtnDownClicked(object sender, EventArgs a)
		{
		    TreePath current,next;
		    
		    // We store the current selected row.
		    current=tvPorts.Selection.GetSelectedRows()[0];
		    next=current.Copy();
		    next.Next(); // Now we have the next row too.
		    
		    // Finally, we swap the associated TreeIters.
		    TreeIter currentIter, nextIter;
		    
		    portsStore.GetIter(out currentIter, current);		    
		    portsStore.GetIter(out nextIter, next);
		    
		    portsStore.Swap(currentIter,nextIter);
		    tvPorts.ScrollToCell(next,null,true,1f,0);
		    
		    modified = true;
		    
		    UpDownButtonsControl();
		}
		
		private void OnBtnRandomClicked(object sender, EventArgs a)
		{
			RandomPortGeneratorDialog dialog =
				new RandomPortGeneratorDialog(
					callEditDialog,
					portsStore.IterNChildren() > 0);
					
			if(dialog.Run() == ResponseType.Ok)
			{
				if(dialog.Overwrite)
				{
					// We want to overwrite 
					TreeIter iter;
					while(portsStore.IterNChildren() > 0)
					{
						portsStore.GetIterFirst(out iter);
						portsStore.Remove(ref iter);
					}
				}
				
				Random r = new Random(Environment.TickCount);
				
				for(int i = 0; i < dialog.PortNumber; i++)
				{
					portsStore.AppendValues(r.Next(1,10000));
				}
			}
					
			dialog.Destroy();
		}
		
		private void OnBtnRemoveClicked(object sender, EventArgs a)
		{
		    // First, we have to retrieve the selected item.
		    TreeIter selIter;
		    tvPorts.Selection.GetSelected(out selIter);    
		    
		    // Then we remove it from the list.
		    portsStore.Remove(ref selIter);
		    
		    modified = true;
		}
		
		private void OnBtnUpClicked(object sender, EventArgs a)
		{
		    TreePath current,previous;
		    
		    // We store the current selected row.
		    current=tvPorts.Selection.GetSelectedRows()[0];
		    previous=current.Copy();
		    previous.Prev(); // Now we have the next row too.
		    
		    // Finally, we swap the associated TreeIters.
		    TreeIter currentIter, previousIter;
		    
		    portsStore.GetIter(out currentIter, current);
		    portsStore.GetIter(out previousIter, previous);
		    
		    portsStore.Swap(currentIter,previousIter);	
		    
		    tvPorts.ScrollToCell(previous,null,true,0f,0);
		    
		    modified = true;
		    
		    UpDownButtonsControl();	
		}
		
		private void OnCancelButtonClicked(object sender, EventArgs a)
		{
		    callEditDialog.Hide();		
		}
		
		private void OnIPChanged(object sender, EventArgs a)
		{
			
		    modified = true;
		}
		
		private void OnOkButtonClicked(object sender, EventArgs a)
		{
		    if(modified)
		    {
		    	string ipAddress = String.Format(
					            "{0}.{1}.{2}.{3}",
					            spIP1.ValueAsInt,
					            spIP2.ValueAsInt,
					            spIP3.ValueAsInt,
					            spIP4.ValueAsInt);
					            
    			if(CheckValues(ipAddress))
    		    {
                    // If we have modified the call, we must ask the user.
    		        ResponseType result = ShowSaveQuestion();                    
                    
    		        if (result == ResponseType.Yes)
    		        {   
    		        	// Changes are made
    		        	if(callSequence == null)
    		        		callSequence = new CallSequence();
		        
					    callSequence.Description =  txtDescription.Text;
					        
					    callSequence.Address = ipAddress;
					            
					    callSequence.TargetPort = spTargetPort.ValueAsInt;
					         
					    ArrayList ports =  new ArrayList();   
					    foreach(object [] row in portsStore)
					    {
					    	ports.Add(row[0]);
					    } 
					        
				        callSequence.Ports = (int[])(ports.ToArray(typeof(int)));   
		    		        	
    		            // We report we have accepted.
    		            callEditDialog.Respond(ResponseType.Ok); 
    		            return;
	    		    }
    		            
    		    }    
		    }    
		    else if(!editing)
		    {
		        OkDialog.Show(
		        	callEditDialog,
		        	MessageType.Info,		        
		            "Debe introducir los datos de la sequencia de llamada\n"+
		            "antes de aceptar los cambios.");
		    }
		    
		    callEditDialog.Respond(ResponseType.None);
		    
		    
		}
		
		private void OnSpTargetPortChanged(object sender, EventArgs a)
		{
			modified = true;
		}
		
		private void OnTxtDescriptionChanged(object sender, EventArgs a)
		{
		    modified = true;
		}
		
		
		
		private void OnTvPortsRowActivated(object sender, RowActivatedArgs a)
		{
		    // After we had double-clicked a row, we show the editing dialog.
		    
		    TreeIter iter;
		    tvPorts.Selection.GetSelected(out iter);
		    
		    
		    // We retrieve the port's value.
		    int value = (int)(portsStore.GetValue(iter,0));	   
		    
		    PortDialog pd = new PortDialog(callEditDialog, value);
		    
		    if(pd.Run() == ResponseType.Ok)
		    {
		        // We have accepted the changes.
		        portsStore.SetValue(iter,0,pd.Value);		  
		        
		        modified = true;  
		    }		    
		}		
		
		private void OnTvPortsSelectionChanged(object sender, EventArgs a)
		{
		    // We enable the disabled buttons on the left panel.		    
		    btnRemove.Sensitive=true;
		    UpDownButtonsControl();		    
		}	
		
		#endregion EventHandlers
	}
}
