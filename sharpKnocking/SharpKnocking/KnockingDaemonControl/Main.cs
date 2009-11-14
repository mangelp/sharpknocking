// project created on 17/03/2007 at 20:45
using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

using SharpKnocking.Common;
using SharpKnocking.Common.Remoting;

namespace KnockingDaemonControl
{
	class MainClass
	{
	    [Alias("n", "a")]
	    private Int32 testMe;
	
	    private int num;
	    
	    
	    /// <summary>
	    /// Get/Set the value for TestMe1 property
	    /// </summary>
	    /// <remarks>
	    /// Generated automatically by a command.
	    /// </remarks>
	    public Int32 TestMe1
	    {
	    	get
	    	{
	    		return this.testMe;
	    	}
	    	set
	    	{
	    		this.testMe = value;
	    	}
	    }
	    
	    
	    /// <summary>
	    /// Get/Set the value for TestMe2 property
	    /// </summary>
	    /// <remarks>
	    /// Generated automatically by a command.
	    /// </remarks>
	    public Int32 TestMe2
	    {
	    	get
	    	{
	    		return this.testMe;
	    	}
	    	set
	    	{
	    		this.testMe = value;
	    	}
	    }
	    
	    
	    /// <summary>
	    /// Get/Set the value for TestMe property
	    /// </summary>
	    /// <remarks>
	    /// Generated automatically by a command.
	    /// </remarks>
	    public Int32 TestMe
	    {
	    	get
	    	{
	    		return this.testMe;
	    	}
	    	set
	    	{
	    		this.testMe = value;
	    	}
	    }
	    
	    
	    /// <summary>
	    /// Get/Set the value for Num property
	    /// </summary>
	    /// <remarks>
	    /// Generated automatically by a command.
	    /// </remarks>
	    public Int32 Num
	    {
	    	get
	    	{
	    		return this.num;
	    	}
	    	set
	    	{
	    		this.num = value;
	    	}
	    }
	    
	    
	    /// <summary>
	    /// Get/Set the value for TestMe3 property
	    /// </summary>
	    /// <remarks>
	    /// Generated automatically by a command.
	    /// </remarks>
	    public Int32 TestMe3
	    {
	    	get
	    	{
	    		return this.testMe;
	    	}
	    	set
	    	{
	    		this.testMe = value;
	    	}
	    }
	   
		public static void Main(string[] args)
		{
            
//            TcpChannel channel = new TcpChannel();
//            ChannelServices.RegisterChannel(channel);
//            
//            IRemoteCommand cmd = 
//                (IRemoteCommand)Activator.GetObject(typeof(IRemoteCommand), 
//                                "tcp://localhost:29566/KnockingDaemonControl");
//            
//            Console.WriteLine("KnockingDaemonControl: Asking for daemon status ...");
//            
//            string status = cmd.GetStatusDetail();
//            
//            Console.WriteLine("KnockingDaemonControl: Daemon status response follow:\n"+status);
            UnixNative.ExecUserIsRoot ();
		}
	}
}