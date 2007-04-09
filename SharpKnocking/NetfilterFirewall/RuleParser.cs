
using System;
using SharpKnocking.Common;
using System.Collections;
using SharpKnocking.NetfilterFirewall;
using SharpKnocking.NetfilterFirewall.Options;
using SharpKnocking.NetfilterFirewall.ExtendedMatch;
using SharpKnocking.NetfilterFirewall.ExtendedTarget;
using SharpKnocking.NetfilterFirewall.Commands;

namespace SharpKnocking.NetfilterFirewall
{
	
    /// <summary>
    /// Models all the logic needed to build an iptables rule as a instance
    /// </summary>
	public static class RuleParser
	{
	    
	    /// <summary>
	    /// Builds a rule from the string.
	    /// </summary>
	    public static NetfilterRule GetRule(string line, NetfilterTable table)
	    {
	        
	        if(!NetfilterRule.IsRule(line))
	            return null;
	        
	        NetfilterRule rule= new NetfilterRule();
	        GenericOption option=null;
	        GenericParameter currParam;
	        
	        try
	        {
	            Debug.VerboseWrite("Splitting line in set of GenericParameter objects");
	            
	            GenericParameter[] parameters = RuleParser.GetParameterList(line);
	            
	            Debug.VerboseWrite("Found "+parameters.Length+" parameters", parameters);
	            
	            int pos=0;
	            
	            while(pos <parameters.Length)
	            {
	                currParam = parameters[pos];
	                if(GenericCommand.IsCommand(currParam.Name))
	                {
	                    Debug.VerboseWrite("GetRule: IsCommand: "+currParam);
	                    
	                    if(rule.Command !=null)
	                    {
	                        throw new DuplicateElementException("Can't have two"
                                             +" commands in the same line");
	                    }
	                    else
	                    {
	                        Exception ex = null;
	                        GenericCommand cmd;
	                        
	                        if(!RuleCommandFactory.TryGetCommand(currParam,
                                out cmd, out ex))
                            {
                                Debug.Write("Error while trying to create command."+
                                            " "+ex.Message);
                                Debug.VerboseWrite("RuleParser: "+ex);
                            }
                            
                            rule.Command = cmd;
	                        
	                        if(rule.Command==null)
	                        {
	                            Debug.Write("Null command built but it was"+
                                            " previously detected. "
                                            +"\nRule parsing broken");
	                            return null;
	                        }
	                        
	                        Debug.VerboseWrite("GetRule: GotCommand: "+rule.Command);
	                    }
	                }
	                else if(GenericOption.IsOption(currParam.Name))
	                {
	                    Debug.VerboseWrite("GetRule: IsOption: "+currParam);
	                    Exception ex=null;
	                    GenericOption opt=null;
	                    
	                    if(!RuleOptionFactory.TryGetOption(currParam,
                                                           out opt, out ex))
                        {
                            Debug.Write("Error while trying to create option."+
                                        " "+ex.Message);
                            Debug.VerboseWrite("RuleParser: "+ex);
                        }
                        
                        option = opt;
                                                           
	                    if(option==null)
	                    {
	                        Debug.Write("Null option built but it was"+
                                         " previously detected."+
                                         "\nRule parsing broken");
	                        
	                        return null;
	                    }
	                    
	                    Debug.VerboseWrite("Adding new option");
	                    
	                    rule.Options.Add(option);
	                    
	                    Debug.VerboseWrite("GetRule: GotOption: "+option);
	                }
	                else
	                {
	                    Debug.Write("Checking for match extension's parameters");
	                    
	                    //The parameter can be a option for an extension
	                    MatchExtensionHandler matchHandler =
	                       rule.FindMatchExtensionHandlerFor(currParam.Name);
	                    
	                    //If is null can't be a extension's parameter
	                    if(matchHandler!=null)
	                    {
	                        Debug.Write("Found parameter for match extension handler "+
	                                    matchHandler.ExtensionName);
    	                    
    	                    //If its not null is correct and we must add it
    	                    matchHandler.AddParameter(currParam.Name, currParam.Value);
	                    }
	                    else
	                    {
	                        Debug.Write("Checking for target extension's parameters");
	                        
	                        TargetExtensionHandler targetHandler =
	                           rule.FindTargetExtensionHandler(currParam.Name);
	                        
	                        //If doesn't is a match extension and if it isn't
	                        //a target extension we don't know what the hell is.
	                        if(targetHandler==null)
	                        {
	                            Debug.Write("Don't know what is this: "+currParam);
	                            Debug.Write("Parsing broken");
	                            return null;
	                        }
	                        
	                        Debug.Write("Found parameter for target extension handler: "+
	                                    targetHandler.ExtensionName);
	                        
	                        targetHandler.AddParameter(currParam.Name, currParam.Value);
	                        
	                    }
	                }
	                
	                pos++;
	            }
	        }
	        catch(Exception ex)
	        {
	            Debug.Write("Error parsing rule line: -"+line+"\nError Message: "+ex.Message);
	            Debug.VerboseWrite("Error Details: "+ex);
	            return null;
	        }
	        
	        if(rule==null)
	            Debug.VerboseWrite("Can't create a rule from line: "+line);
	        
	        return rule;
	    }
	    
	    
	    /// <summary>
	    /// Extracts the parameter list from the line. 
	    /// </summary>
	    /// <remarks>
	    /// The parameters are not procesed so this method doesn't know nothing
	    /// about match extensions, targets or actions.<br/>
	    /// Simply returns an array of objects where two properties are set,
	    /// the parameter name and the parameter value. This is helpful when
	    /// trying to analize the rule.
	    /// </remarks>
	    public static GenericParameter[] GetParameterList(string line)
	    {
	        string[] parts = Net20.StringSplit(line, true, ' ');
	        
	        ArrayList temp = new ArrayList();
	        int pos=0;
	        bool negateNext=false;
	        
	        //Convert parameters and values into objects
	        GenericParameter par = null;
	        
	        Debug.VerboseWrite("GetParameterList: Loop", VerbosityLevels.Insane);
	        
	        while(pos<parts.Length)
	        {
	            //If the part starts with - is a parameter. If not it is something
	            //unknowm
	            if(parts[pos][0]=='-')
	            {
	                Debug.VerboseWrite("GetParameterList: Found parameter at "
	                                   +pos+":"+parts[pos], VerbosityLevels.Insane);
	                
	                par = new GenericParameter();
	                
	                if(negateNext)
	                {
	                    Debug.VerboseWrite("GetParameterList: Negated",
	                                       VerbosityLevels.Insane);
	                    par.Not = true;
	                    negateNext = false;
	                }
	                
	                par.IsLongOption = GenericParameter.CheckLongOption(parts[pos]);
                    
                    if(par.IsLongOption)
                        par.Name = parts[pos].Substring(2);
                    else
                        par.Name = parts[pos].Substring(1);
	                
	                pos++;
	                par.Value = String.Empty;
	                
	                //Check for negation at the start of values for the current
	                //parameter
	                
	                if(parts[pos].Length==1 && parts[pos][0]=='!')
	                {
	                    Debug.VerboseWrite("GetParameterList: Negation for "+par
	                                       , VerbosityLevels.Insane);
	                    par.Not = true;
	                    pos++;
	                }
	                
                    Debug.VerboseWrite("GetParameterList: Inner Loop"
                                       , VerbosityLevels.Insane);
	                
	                //if the next thing is not a parameter is the value of this
	                //parameter.
	                while(pos<parts.Length && parts[pos][0]!='-')
	                {
	                    if(!Net20.StringIsNullOrEmpty(par.Value))
	                        par.Value += " "+parts[pos];
	                    else
	                        par.Value = parts[pos];
	                    
	                    Debug.VerboseWrite("GetParameterList: Checking at "
	                                       +pos+":'"+parts[pos]+"'"
                                           , VerbosityLevels.Insane);
	                    
	                    //If there is a ! the next must be a parameter. If not
	                    //there is a bad format.
	                    if(parts[pos][0]=='!')
	                    {
	                        if((pos+1)<parts.Length &&
                                parts[pos+1][0]=='-')
                             {
                                 negateNext = true;
                             }
	                         else
	                         {
	                             throw new NetfilterException("Invalid '!' found");    
	                         }
	                    }
	                    
	                    //Go to the next position
	                    pos++;
	                }
	                
	                //Debug.Write("GetParameterList: New parameter: "+par);
	                
	                //Don't forget to add the object to the temporal list
	                temp.Add(par);
	                
	                //skip the next increment to process the current parameter
	                continue;
	            }
	            else if(parts[pos][0]=='!')
	            {
	                Debug.VerboseWrite("GetParameterList: Found ! at "+pos+":"+parts[pos]);
	                // We found negation prior to parameter. Is the negation of
	                // the next parameter.
	                negateNext = true;
	            }
	            else
	            {
	            	Debug.Write("GetParameterList: Unknown option: "+parts[pos]);    
	            }
	            
	            pos++;   
	        }
	        
	        //Convert the arrayList to an array of the correct type
	        GenericParameter[] result = new GenericParameter[temp.Count];
	        
	        for(int j=0;j<temp.Count;j++)
	        {
	            //Debug.Write("GetParameterList: Adding to result: "+temp[j]);
	            result[j] = (GenericParameter)temp[j];
	        }
	        
	        return result;
	    }
	    
	   
	}   
}
