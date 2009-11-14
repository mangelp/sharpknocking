
using System;

using SharpKnocking.Common;

namespace SharpKnocking.NetfilterFirewall.Commands
{
	
	
	public static class RuleCommandFactory
	{
	    private static string currentNamespace;
	    
	    static RuleCommandFactory()
	    {
	        Type t = typeof(RuleCommandFactory);
	        currentNamespace = t.Namespace;
	    }
	    
	    public static bool TryGetCommand(GenericParameter param,
	                                          out GenericCommand command,
	                                          out Exception foundException)
	    {
	        foundException = null;
	        command = null;
	        
            try
            {
                command = RuleCommandFactory.GetCommand(param);
                return true;
            }
            catch(Exception ex)
            { 
                foundException = ex;
                return false;
            }
       }
	                                          
	                                          
	    
	    public static GenericCommand GetCommand(GenericParameter param)
	    {
	        GenericCommand res=null;
	        RuleCommands type = GenericCommand.GetCommandType(param.Name);
	        
	        string[] parts = Net20.StringSplit(param.Value, true, ' ');
	        
	        //Create the instance for the concrete object
	        Type objType = Type.GetType(
                                        currentNamespace+"."+type.ToString()+"Command"
                                        , true, false);
	        
	        res = (GenericCommand)Activator.CreateInstance(objType);
	        
	        //Init the values and check all that we can.
	        
	        switch(type)
	        {
	            case RuleCommands.AppendRule:
	            
	                if(parts.Length!=1)
	                {
	                    throw new NetfilterException("Invalid parameters for "+
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
	                    throw new NetfilterException("Invalid parameters for "+
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
	                        ((DeleteRuleCommand)res).RuleNum = ruleNum;
	                }
	                else if(parts.Length==0 || parts.Length>2)
	                {
                        throw new NetfilterException("Invalid parameters for "+
                              type +" command: "+param.Value);
	                }
	                
	                break;
	            case RuleCommands.NewChain:
	                
	                if(parts.Length!=1)
	                {
                        throw new NetfilterException("Invalid parameters for "+
                              type +" command: "+param.Value);   
	                }
	                res.ChainName = parts[0];
	                break;
	            case RuleCommands.RenameChain:
	                
	                if(parts.Length!=2)
	                {
                        throw new NetfilterException("Invalid parameters for "+
                              type +" command: "+param.Value);   
	                }
	                
	                ((RenameChainCommand)res).OldChain = parts[0];
	                ((RenameChainCommand)res).NewChain = parts[1];
	                
	                break;
	            case RuleCommands.ReplaceRule:
	                if(parts.Length!=2)
	                {
                        throw new NetfilterException("Invalid parameters for "+
                              type +" command: "+param.Value);   
	                }
	                
	                ((ReplaceRuleCommand)res).RuleNum = Int32.Parse(parts[1]);
	                res.ChainName = parts[0];
	                break;
	            case RuleCommands.SetChainPolicy:
	                if(parts.Length!=2)
	                {
                        throw new NetfilterException("Invalid parameters for "+
                              type +" command: "+param.Value);   
	                }
	                SetChainPolicyCommand policy = (SetChainPolicyCommand)res;
	                policy.ChainName = parts[0];
	                policy.Target = parts[1];
	                break;
	            default:
	                throw new ArgumentException("Unsupported command type :"+type);
	        }
	        
	        if(res!=null)
	        {
	            //common initialization
	            res.IsLongOption = param.IsLongOption;
	        }
	        
	        return res;
	    }
	}
}
