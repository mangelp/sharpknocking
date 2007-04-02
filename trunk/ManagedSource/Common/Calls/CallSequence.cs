
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Xml.Serialization;

namespace SharpKnocking.Common.Calls
{
	
	/// <summary>
	/// This class defines the information which is needed to know
	/// about the call sequences which are used.
	/// </summary>
	public class CallSequence
	{
		public event EventHandler PackageSent;
	
	    private string address;
		private string description;
		
		private int targetPort;
		
		private int [] ports;
		
		/// <summary>
		/// CallSequence's default constructor.
		/// </summary>
		public CallSequence()
		{
		}
		
		#region Properties
		
		/// <summary>
		/// The sequence's description can be modified and retrieved by using
		/// this property.
		/// </summary>
		public string Description
		{
		    get
		    {
		        return description;
		    }
		    set
		    {		        
		        description=value.Trim();
		    }
		}
		
		/// <summary>
		/// The target's IP address can be altered and retrieved by using
		/// this property.
		/// </summary>
		public string Address
		{
		    get
		    {
		        return address;
		    }
		    set
		    {
		        /* TODO: Obviously, checking that "value" is a 
		         valid IP address would be sooo nice...*/
		        address=value;
		    }
		}
		
		/// <summary>
		/// The sequence's port list can be retrieved and established 
		/// through this property.
		/// </summary>
		public int[] Ports
		{
		    get
		    {
		        return ports;
		    }
		    
		    set
		    {
		        ports=value;
		    }
		}
		
		/// <summary>
		/// Through this property the port we are going to open with this CallSequence can
		/// be retrieved or modified;
		/// </summary>
		public int TargetPort
		{
			get
			{
				return targetPort;
			}
			
			set
			{
				targetPort = value;
			}
		
		}		
		
		#endregion Properties
		
		#region Public methods
		
		/// <summary>
		/// This method allows to retrieve a CallSequence's instance
		/// which had been serialized previously.
		/// </summary>
		/// <param name = "path">
		/// The path of the file which contains the stored instance.
		/// </param>
		/// <returns>
		/// The instance of CallSequence which was readen.
		/// </returns>
		public static CallSequence Load(string path)
		{
			XmlSerializer serializer =  new XmlSerializer(
		                                    typeof(CallSequence),
		                                    new Type[]{typeof(int)});
		    
		    CallSequence res;                           
		    using (FileStream fs = new FileStream(path, FileMode.Open)) 
		    {              
                res = (CallSequence)(serializer.Deserialize(fs));     
            }
            
            return res;
		}
		
		/// <summary>
		/// This method performs the call, which mean that the target IP's
		/// machine's ports specified are sent packages in the given order.
		/// </summary>		
		/// <returns>
		/// True if the call opens the target port, false otherwise.
		/// </returns>
		public bool PerformCall()
		{		
			// We create a socket.
			Socket sock = new Socket(AddressFamily.InterNetwork,
									 SocketType.Dgram,
									 ProtocolType.Udp);
			
			Random dataGenerator = new Random(Environment.TickCount);
			byte [] data = new byte[8];
			
			IPEndPoint destination;
			IPAddress ad =  IPAddress.Parse(address);
			
			byte order = 0;
			
			foreach(int port in ports)
			{
				// We create fake data for the package.
				dataGenerator.NextBytes(data);
				
				data[0] = order;
				
				order++;
				
				// The target's IP address and port are specified.
				destination = new IPEndPoint(ad,port);
				
				// The data is sent, good bye! :D
				sock.SendTo(data,destination);
				
				Debug.VerboseWrite(port +" out");
				
				// We launch an event, so we can monitor proggress
				PackageSentNotify();
				
				
				Thread.Sleep(20);
			}
			
			// TODO: See how to see if the target port opened as intended ->luis
			return true;
			
		}	
		
		/// <summary>
		/// This method's use is to save an instance of an CallSequence object
		/// into a file.
		/// </summary>
		/// <param name = "path">
		/// The path of the file in which the object will be written.
		/// </param>
		public void Store(string path)
		{
			XmlSerializer serializer =  new XmlSerializer(
		                                    typeof(CallSequence),
		                                    new Type[]{typeof(int)});
		                                    
		    using (FileStream fs = new FileStream(path, FileMode.Create)) 
		    {              
                serializer.Serialize(fs, this);     
            }
		}
		
		
		
		#endregion Public methods
		
		#region Private methods
		
		private void PackageSentNotify()
		{
			if(PackageSent != null)
				PackageSent(this, EventArgs.Empty);
		}
		#endregion Private methods
	}
}
