
using System;

using Developer.Common.Types;

namespace IptablesNet.Core.Commands
{
	
	/// <summary>
	/// Factory for GenericCommand derived objects.
	/// </summary>
	public static class IptablesCommandFactory
	{
	    private static string currentNamespace;
	    
	    static IptablesCommandFactory()
	    {
	        Type t = typeof(IptablesCommandFactory);
	        currentNamespace = t.Namespace;
	    }
	    
		/// <summary>
		/// Builds a command from a simple parameter. 
		/// </summary>
		/// <remarks>
		/// There are two output parameters where to get the command and the exception if
		/// any.
		/// This method will never throw out an exception as it is handled internally.
		/// </remarks>
		/// <returns>
		/// True if the command was built or false if not. If it is built the out parameter
		/// <c>command</c> contains the command or null if not. If this method returns false
		/// the out parameter <c>foundException</c> can be the exception that caused the 
		/// fail or null if there was no exception involved.
		/// </returns>
		/// <param name="command">Command built. If built fails can be null</param>
		/// <param name="foundException">Exception got while building the command</param>
	    public static bool TryGetCommand(SimpleParameter param,
	                                          out GenericCommand command,
	                                          out Exception foundException)
	    {
	        foundException = null;
	        command = null;
	        
            try
            {
                command = IptablesCommandFactory.GetCommand(param);
				
				if(command!=null)
					return true;
				else
					return false;
            }
            catch(Exception ex)
            { 
                foundException = ex;
                return false;
            }
       }
	                                          
	                                          
	    /// <summary>
		/// Gets a command from a simple parameter. 
		/// </summary>
		/// <remarks>
		/// This method can throw exceptions if the conversion from the simple parameter
		/// to the command isn't successfull.
		/// </remarks>
		/// <returns>
		/// A GenericCommand instance built with the information in the simple parameter or 
		/// null if it can't be built.
		/// </returns>
		/// <param name="param"> Simple parameter with the information to create the command</param>
	    public static GenericCommand GetCommand(SimpleParameter param)
	    {
	        GenericCommand res=null;
	        RuleCommands type = GenericCommand.GetCommandType(param.Name);
	        
	        string[] parts = StringUtil.Split(param.Value, true, ' ');
	        
	        //Create the instance for the concrete object
	        Type objType = Type.GetType(currentNamespace+"."+type.ToString()+"Command",
                                        true, 
			                            false);
	        
	        res = (GenericCommand)Activator.CreateInstance(objType);
	        
	        //Init the values in the object and check things for sanity
	        
	        switch(type)
	        {
	            case RuleCommands.AppendRule:
	            
	                if(parts.Length!=1)
	                {
	                    throw new IptablesException("Invalid parameters for "+
	                                           type +" command: "+param.Value);
	                }
	                
	                res.ChainName = parts[0];
	            
	                break;
	            case RuleCommands.DeleteChain:
	            case RuleCommands.FlushChain:
                case RuleCommands.ListChain:
                case RuleCommands.ZeroChain:
	            
	                if(parts.Length==1)
	                {
	                    res.ChainName = parts[0];
	                }
	                else if(parts.Length>1)
	                {
	                    throw new IptablesException("Invalid parameters for "+
                              type +" command: "+param.Value);
	                }
	                
	                break;
	            case RuleCommands.DeleteRule:
	            case RuleCommands.InsertRule:

	                if(parts.Length==1)
	                {
	                    res.ChainName = parts[0];
	                }
	                else if(parts.Length==2)
	                {
	                    res.ChainName = parts[0];
	                    int ruleNum = Int32.Parse(parts[1]);
	                    
	                    if(type == RuleCommands.DeleteRule)
	                        ((DeleteRuleCommand)res).RuleNum = ruleNum;
	                    else if(type == RuleCommands.InsertRule)
	                        ((InsertRuleCommand)res).RuleNum = ruleNum;
	                }
	                else if(parts.Length==0 || parts.Length>2)
	                {
                        throw new IptablesException("Invalid parameters for "+
                              type +" command: "+param.Value);
	                }
	                
	                break;
	            case RuleCommands.NewChain:
	                
	                if(parts.Length!=1)
	                {
                        throw new IptablesException("Invalid parameters for "+
                              type +" command: "+param.Value);   
	                }
	                res.ChainName = parts[0];
	                break;
	            case RuleCommands.RenameChain:
	                
	                if(parts.Length!=2)
	                {
                        throw new IptablesException("Invalid parameters for "+
                              type +" command: "+param.Value);   
	                }
	                
	                ((RenameChainCommand)res).ChainName = parts[0];
	                ((RenameChainCommand)res).NewChain = parts[1];
	                
	                break;
	            case RuleCommands.ReplaceRule:
	                if(parts.Length!=2)
	                {
                        throw new IptablesException("Invalid parameters for "+
                              type +" command: "+param.Value);   
	                }
	                
	                ((ReplaceRuleCommand)res).RuleNum = Int32.Parse(parts[1]);
	                res.ChainName = parts[0];
	                break;
	            case RuleCommands.SetChainPolicy:
	                if(parts.Length!=2)
	                {
                        throw new IptablesException("Invalid parameters for "+
                              type +" command: "+param.Value);   
	                }
	                SetChainPolicyCommand policy = (SetChainPolicyCommand)res;
	                policy.ChainName = parts[0];
	                policy.Target = parts[1];
	                break;
	            default:
	                throw new ArgumentException("Unsupported command type :"+type);
	        }
	        
	        return res;
	    }
	}
}
