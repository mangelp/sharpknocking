
using System;
using System.Collections;
using System.Diagnostics;

using SharpKnocking.Common;
using SharpKnocking.Common.Calls;
using SharpKnocking.KnockingDaemon.SequenceDetection;

namespace SharpKnocking.KnockingDaemon.PacketFilter
{
	
	/// <summary>
	/// This class is used to launch a tcpdump proccess so we can listen to the
	/// the packages we are interested in.
	/// </summary>
	public class TcpdumpMonitor: IDaemonProcessUnit
	{
	
		public event PacketCapturedEventHandler PacketCaptured;
		
		private Process monitoringProccess; 
    	
    	private bool running;
		
		private CallSequence [] sequences;
		
		private SequenceDetectorManager seqManager;
		
		private bool stopped;
		
		public TcpdumpMonitor()
		{			
		}
		
		public void Dispose()
		{
		    if(this.monitoringProccess !=null)
		    {
		       if(!this.monitoringProccess.HasExited)
	               monitoringProccess.Kill();
	               
	           this.monitoringProccess = null;
	         }
	         
	         if(this.seqManager !=null)
             {
	             this.seqManager.Dispose();
	             this.seqManager = null;
	         }
	           
	         this.sequences = null;
		}
		
		#region Properties
		
		/// <summary>
		/// Gets the running status of the monitor.
		/// </summary>
		/// <remarks>
		/// If this flag is true means that the monitor is initialized and
		/// working, but it can be in a stopped status so it will not do any
		/// processing task. See <c>Stopped</c> property.
		/// </remarks>
		public bool Running
		{
		    get 
		    {
		        return this.running ;
		    }
		}

		/// <summary>
		/// This property allows its user to retrieve the stopped status of the 
		/// TcpdumpMonitor.
		/// </summary>
		/// <remarks>
		/// If this flag is true means that the monitor is procesing incoming
		/// packets. If it's set to false the monitor kills the process that
		/// capture packets and stops processing. Changing the value from false
		/// to true will cause the creation of a new process and the reload of
		/// the rule set.
		/// To change the flag you must use <c>Stop</c> and <c>Start</c> methods.
		/// </remarks>
		public bool Stopped
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
		
		//Loads the sequences and reinstantiates the sequence manager
		private void LoadSequences()
		{
		     //Load new sequence set
		     this.sequences = CallsLoader.Load();
		     //Clear existing sequence manager
		     if(this.seqManager != null)
		     {
		         this.seqManager.Dispose();
		     }
		     //Get a fresh instance of the manager
             this.seqManager = new SequenceDetectorManager(this.sequences, this);
		}
		
		public void Run()
		{			
		    this.running = true;
			string tcpdumpPath = WhichWrapper.Search("tcpdump");
			
			if(tcpdumpPath == null)
			{
				// There's no tcpdump in the system.
				Console.WriteLine(
					"¡Necesita el programa «tcpdump» para usar SharpKnocking!");
				this.running = false;
				return;
			}
			
			this.LoadSequences();

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
			    if(!this.stopped)
			    {
			        assembler.AddLine(
				        monitoringProccess.StandardOutput.ReadLine());
				}				
			}

		}
		
		public void Kill()
		{
		    //Clear everything
			this.Dispose();
		}
		
		/// <summary>
		/// Stops processing. Pause.
		/// </summary>
		public void Stop()
		{
            if(!this.running)
                return;
            
            this.stopped = true; 
		}
		
		/// <summary>
		/// Resumes processing. Quit pause.
		/// </summary>
		public void Start()
		{
		    if(!this.running)
		        return;
		        
		    this.stopped = false;
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
