
using System;
using System.Collections;
using System.Diagnostics;

using SharpKnocking.Common;
using SharpKnocking.Common.Calls;

namespace SharpKnocking.KnockingDaemon.PacketFilter
{
	
	/// <summary>
	/// This class is used to launch a tcpdump proccess so we can listen to the
	/// the packages we are interested in.
	/// </summary>
	public class TcpdumpMonitor
	{
		public event PacketCapturedEventHandler PacketCaptured; 
	
    	private ArrayList calls;
    	
    	private CallSequence [] sequences;
		
		private Process monitoringProccess;
		
		public TcpdumpMonitor()
		{
			calls = new ArrayList();			
		}
		
		#region Properties

		/// <summary>
		/// This property allows its user to retrieve the running status of the 
		/// TcpdumpMonitor.
		/// </summary>
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
        
        
        /// <summary>
        /// The number of packets captured by the TcpdumpMonitor instance can
        /// be retrieved using this property.
        /// </summary>
        public int NPackets
        {
        	get
        	{
        		// TODO: Return the number of captured packages.
        		return -1;
        	}
        }

		#endregion Properties
		
		#region Public methods
		
		public void Run()
		{			
			string tcpdumpPath = WhichWrapper.Search("tcpdump");
			
			if(tcpdumpPath == null)
			{
				// There's no tcpdump in the system.
				Console.WriteLine(
					"¡Necesita el programa «tcpdump» para usar SharpKnocking!");
			}
			else
			{
				
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
					
				monitoringProccess.Start();
				
				PacketAssembler assembler = new PacketAssembler();
				
				assembler.PacketCaptured += new PacketCapturedEventHandler(OnPacketCaptured);
				
				while(!monitoringProccess.HasExited)
				{
					assembler.AddLine(
						monitoringProccess.StandardOutput.ReadLine());							
				}
			}
		}
		
		public void SetSequences(CallSequence [] sequences)
		{
			this.sequences = sequences;		
		}
		
		public void StopMonitoring()
		{
			monitoringProccess.Kill();
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
				for( i = 0; i < ports.Count - 2; i++)
				{
					expression += String.Format("dst port {0} or ", ports[i]);
				}
				
				expression += String.Format("dst port {0}", ports[i]);
			}
			return expression;
		}
		
		private void OnPacketCaptured(object sender, PacketCapturedEventArgs a)
		{
			// We redirect the event
			OnPacketCapturedHelper(a);
		}
		
		private void OnPacketCapturedHelper(PacketCapturedEventArgs a)
		{
			if(PacketCaptured != null)
				PacketCaptured(this, a);
		}

		#endregion Private methods
	}
}
