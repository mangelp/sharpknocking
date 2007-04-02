using System;
using System.Collections;
using System.Net;

namespace SharpKnocking.IpTablesManager.RuleHandling
{
    /// <summary>
    /// Models a command. Each line that you find in the iptables configuration
    /// file or when you execute iptables-save or iptables -V is a command.
    /// </summary>
	public class Command
	{
	    private RuleActions action;
	    
	    /// <summary>
	    /// Action for the command
	    /// </summary>
	    public RuleActions Action
	    {
	        get {return this.action;}    
	    }
	    
	    private ArrayList parameters;
	    
	    /// <summary>
	    /// Parameters for the command
	    /// </summary>
	    public RuleParameter Parameters
	    {
	        get
	        {
	            return this.parameters;    
	        }
	    }
	    
	    /// <summary>
	    /// Empty constructor. Initializes the instance to a command with
	    /// default values.
	    /// </summary>
    	public Command()
    	{
    	    this.parameters = new ArrayList();
    	    this.action = RuleActions.None;
    	}
    	
    	/// <summary>
    	/// Gets a string that represents the current command. This string can
    	/// be used directly with iptables.
    	/// </summary>
    	public override string ToString()
    	{
    	    string result = String.Empty;
    	    
    	    if(this.action != RuleActions.None)
    	    {
    	        //Only put the action if there is one
    	        
    	        result += "-"+((char)this.action).ToString()+" ";
    	    
        	    for(int i=0;i<this.parameters.Count;i++)
        	    {
        	        result += " "
        	            +((BaseCommandParameter)this.parameters[i]).ToString();
        	    }
    	    }
    	    else if(this.parameters.Count>0)
    	    {
    	        //If there is no action we can get the command simply joining
    	        //the parameters
    	        result = this.parameters[0].ToString();
    	        
    	        for(int i=1;i<this.parameters.Count;i++)
        	    {
        	        result += " "
        	            +((BaseCommandParameter)this.parameters[i]).ToString();
        	    }
    	    }
    	    
    	    return result;
    	}
    	
    	/// <summary>
    	/// Parses the string and instances a command object.
    	/// </suamary>
    	public static Command ParseString(string iptCommand)
    	{
    	    Command cmd = new Command();
    	    
    	    iptCommand = iptCommand.Trim();
    	    
    	    if(iptCommand.StartsWith("#") || iptCommand.StartsWith(":"))
    	    {
    	        
    	    }
    	    
    	    return cmd;
    	}
		
	}
}
