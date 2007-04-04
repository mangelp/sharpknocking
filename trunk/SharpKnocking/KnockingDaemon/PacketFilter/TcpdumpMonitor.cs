
using System;
using System.Collections;
using System.Diagnostics;

using Mono.Unix.Native;

using SharpKnocking.Common;
using SharpKnocking.Common.Calls;
using SharpKnocking.KnockingDaemon.SequenceDetection;

namespace SharpKnocking.KnockingDaemon.PacketFilter
{
	
	/// <summary>
	/// This class is used to launch a tcpdump proccess so we can listen to the
	/// the packages we are interested in.
	/// </summary>
	public class TcpdumpMonitor: IDisposable
	{
	    private bool die=false;
	    
		public event PacketCapturedEventHandler PacketCaptured;
		
		private Process monitoringProccess; 
		
		private CallSequence [] sequences;
		
		/// <summary>
		/// Constructor with the sequences to detect. It creates an expresion
		/// for tcpdump to detect these sequences
		/// </summary>
		public TcpdumpMonitor(CallSequence[] sequences)
		{
		    this.sequences = sequences;			
		}
		
		/// <summary>
		/// Clears all references, events and threads
		/// </summary>
		public void Dispose()
		{
		    this.die = true;
		    if(this.monitoringProccess !=null)
		    {
		       if(!this.monitoringProccess.HasExited)
		       {
		          try
		          {
	                  monitoringProccess.Kill();
	              }
	              catch(InvalidOperationException ex)
	              {
	                  SharpKnocking.Common.Debug.VerboseWrite(
	                           "Exception killing monitor: "+ex);
	              }
	              
	              this.monitoringProccess.Dispose();
	           }
	           else
	           {
	               this.monitoringProccess.Dispose();
	           }
	           
	           this.monitoringProccess = null;
	         }
	         
	         this.sequences = null;
	         
//	         this.OnDisposedEvent();
		}
		
		#region Properties

		/// <summary>
		/// This property allows its user to retrieve the running status of the 
		/// TcpdumpMonitor.
		/// </summary>
		/// <remarks>
		/// If this flag is true means that the monitor is procesing incoming
		/// packets. If it's set to false the monitor kills the process that
		/// capture packets and stops processing. Changing the value from false
		/// to true will cause the creation of a new process.
		/// To change the flag you must use <c>Stop</c> and <c>Start</c> methods.
		/// </remarks>
		public bool Running
        {
            get
            {
                if(this.monitoringProccess!=null 
                	&& !this.monitoringProccess.HasExited)
                {
                    return true;
                }
                
                return false;
            }
        }

		#endregion Properties
		
		#region Public methods
		
		public void Run()
		{			
            try
            {
    			string tcpdumpPath = WhichWrapper.Search("tcpdump");
    			
    			if(tcpdumpPath == null)
    			{
    				// There's no tcpdump in the system.
    				Console.WriteLine(
    					"¡Necesita el programa «tcpdump» para usar SharpKnocking!");
    				return;
    			}
    			
    			if(sequences==null)
    			    return;

    			string expression = CreateExpression(sequences);				
    		    
    			monitoringProccess = new Process();
    		    
    			monitoringProccess.StartInfo.FileName = tcpdumpPath;
    			
    			// Arguments given to tcpdump are
    			// -i any, so we monitor every network interface.
    			// -x, so the package's content is showed as hexadecimal numbers,
    			// -l, so the output is buffered,
    			// -q, so the output contains less info,
    			// -f, so tcpdump doesn't try to resolve ip names.
    			monitoringProccess.StartInfo.Arguments =
    				 "-i any -x -l -q -f " + expression ;
    				 
    			monitoringProccess.StartInfo.UseShellExecute = false;
    			monitoringProccess.StartInfo.RedirectStandardOutput = true;	
    		    SharpKnocking.Common.Debug.VerboseWrite("TcpdumpMonitor::Run() Starting monitor process");
    			monitoringProccess.Start();
    			
    			PacketAssembler assembler = new PacketAssembler();
    			
    			assembler.PacketCaptured += new PacketCapturedEventHandler(OnPacketCaptured);
    			
    			SharpKnocking.Common.Debug.VerboseWrite("TcpdumpMonitor::Run() Reading incoming packets.");
    			
    			while(!die && monitoringProccess!=null && !monitoringProccess.HasExited)
    			{
    		        assembler.AddLine(
    			        monitoringProccess.StandardOutput.ReadLine());			
    			}
			}
			catch(Exception ex)
			{
                SharpKnocking.Common.Debug.Write("TcpdumpMonitor::Run(): Error while processing packets: "+ex);
			}

		}
		
		/// <summary>
		/// Stops processing. Kills this process.
		/// </summary>
		public void Stop()
		{           
		    this.die = true;
            this.sequences = null;
            this.Dispose (); 
		}
		
		#endregion Public methods
		
		#region Private methods
		
		private string CreateExpression(CallSequence [] sequences)
		{
			ArrayList ports = new ArrayList();
			// Now we have a list with all ports appearing only once.
			// This list is going to be converted to a expression for tcpdump.
			foreach(CallSequence seq in sequences)
			{			
				// The call sequence is enabled.
				foreach(int port in seq.Ports)
				{
					if(!ports.Contains(port))
						ports.Add(port);
				}
				
			}
			
			string expression = "";
			if(ports.Count > 0)
			{	
				int i;
				for( i = 0; i < ports.Count - 1; i++)
				{
					expression += String.Format("dst port {0} or ", ports[i]);
				}
				
				expression += String.Format("dst port {0}", ports[i]);
			}
			return expression;
		}
		
		//Handles the event that occurs when a packet is captured 
		private void OnPacketCaptured(object sender, PacketCapturedEventArgs a)
		{
			// We redirect the event
			OnPacketCapturedHelper(a);
		}
		
		//Sends an event that notifies when a packet is captured
		private void OnPacketCapturedHelper(PacketCapturedEventArgs a)
		{
			if(PacketCaptured != null)
				PacketCaptured(this, a);
		}
		
//		private void OnDisposedEvent()
//		{
//		    if(this.Disposed!=null)
//		      this.Disposed(this, EventArgs.Empty);
//		}

		#endregion Private methods
	}
}
