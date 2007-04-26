
using System;
using System.Threading;
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
	    private bool running;
	    
		public event PacketCapturedEventHandler PacketCaptured;
		
		private Process monitoringProccess; 
		
		private CallSequence [] sequences;
		
		public bool Die
		{
		    get { return this.die;}
		    set 
		    { 
		        //Net20.PrintThreadInformation("TcpdumpMonitor_Die_Set");
		        this.die = value;
		    }
		}
		
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
            if(this.monitoringProccess !=null)
            {
                this.monitoringProccess.Dispose();
                this.monitoringProccess = null;
            }
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
                return this.running ;
            }
        }

		#endregion Properties
		
		#region Public methods
		
		public static void Run(object obj)
		{			
		    TcpdumpMonitor monitor = (TcpdumpMonitor) obj;
		    monitor.running = true;
		    //Net20.PrintThreadInformation ("TCPDUMPMONITOR_RUN");
		    SharpKnocking.Common.Debug.VerboseWrite("TcpdumpMonitor::Run() Starting");
            try
            {
    			string tcpdumpPath = WhichWrapper.Search("tcpdump");
    			
    			if(tcpdumpPath == null)
    			{
    				// There's no tcpdump in the system.
    				Console.WriteLine(
    					"TcpDumpMonitor::Run():tcpdump is required to use SharpKnocking!");
    				return;
    			}
    			
    			if(monitor.sequences==null)
    			    return;

    			string expression = monitor.CreateExpression(monitor.sequences);				
    		    
    			monitor.monitoringProccess = new Process();
    		    
    			monitor.monitoringProccess.StartInfo.FileName = tcpdumpPath;
    			
    			// Arguments given to tcpdump are
    			// -i any, so we monitor every network interface.
    			// -x, so the package's content is showed as hexadecimal numbers,
    			// -l, so the output is buffered,
    			// -q, so the output contains less info,
    			// -f, so tcpdump doesn't try to resolve ip names.
    			monitor.monitoringProccess.StartInfo.Arguments =
    				 "-i any -n -x -l -q -f " + expression ;
    				 
    			monitor.monitoringProccess.StartInfo.UseShellExecute = false;
    			monitor.monitoringProccess.StartInfo.RedirectStandardOutput = true;	
    		    SharpKnocking.Common.Debug.VerboseWrite("TcpdumpMonitor::Run() Starting monitor process");
    			monitor.monitoringProccess.Start();
    			
    			PacketAssembler assembler = new PacketAssembler();
    			
    			assembler.PacketCaptured += new PacketCapturedEventHandler(monitor.OnPacketCaptured);
    			
    			SharpKnocking.Common.Debug.VerboseWrite("TcpdumpMonitor::Run() Reading incoming packets.");
    			
    			while(monitor!=null && !monitor.die  
    			         && monitor.monitoringProccess!=null 
    			         && !monitor.monitoringProccess.HasExited 
    			         && monitor.monitoringProccess.StandardOutput!=null)
    			{
    			    try
    			    {
    		            assembler.AddLine(
    			            monitor.monitoringProccess.StandardOutput.ReadLine());
    			    }
    			    catch(Exception ex)
    			    {
    			        monitor.die = true;
    		            SharpKnocking.Common.Debug.VerboseWrite("TcpdumpMonitor::Run(): "+
    		                          " Exception assembling packets: "+ex.Message);
    		            SharpKnocking.Common.Debug.VerboseWrite("TcpdumpMonitor::Run(): "+
    		                          " Details: \n"+ex);
    			    }
    			}
    			
    		    SharpKnocking.Common.Debug.VerboseWrite("TcpdumpMonitor::Run(): "+
    		                          " Loop end");
			}
			catch(Exception ex)
			{
                SharpKnocking.Common.Debug.VerboseWrite("TcpdumpMonitor::Run(): "+
                                      "Error while processing packets: "+ex);
                monitor.die = true;
			}
			
			monitor.die = true;
			//Stop monitor
			monitor.KillActions();
            monitor.running  = false;
            
            SharpKnocking.Common.Debug.Write("TcpdumpMonitor::Run(): Exiting run method");
		}
		
		/// <summary>
		/// Stops processing. Kills this process.
		/// </summary>
		public void Stop()
		{            
		    
		    //Net20.PrintThreadInformation("TCPDUMPMONITOR_STOP");
		    SharpKnocking.Common.Debug.VerboseWrite (
		        "TcpDumpMonitor::Stop(): Stopping packet capture in "+
		        Thread.CurrentThread.GetHashCode());
		    this.die = true;
		    //This stops the monitor exiting the main loop
		    this.KillActions();
		}
		
		#endregion Public methods
		
		#region Private methods
		
		private void KillActions()
		{
		    //Net20.PrintThreadInformation("TCPDUMPMONITOR_KILLACTIONS");
		    SharpKnocking.Common.Debug.VerboseWrite (
		        "TcpDumpMonitor::KillActions: Ending tcpdump process");
		    try
		    {
    		    if(this.monitoringProccess !=null)
    		    {
    		        this.monitoringProccess.Kill ();
    		    }
    		    else
    		    {
        		    SharpKnocking.Common.Debug.VerboseWrite (
        		        "TcpDumpMonitor::KillActions: tcpdump process already finished!");    		      
    		    }
		    }
		    catch(Exception ex)
		    {
		        SharpKnocking.Common.Debug.VerboseWrite (
		            "TcpDumpMonitor::KillActions: Exception catched. "+
		            "\nDetails:\n"+ex);
		    }
		    
		    this.monitoringProccess = null;
		}
		
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
        
		#endregion Private methods
	}
}
