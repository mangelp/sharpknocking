
using System;
using System.Collections;

using IptablesNet.Core;
using IptablesNet.Core.Options;
using IptablesNet.Core.Commands;
using IptablesNet.Core.Extensions;
using IptablesNet.Core.Extensions.ExtendedMatch;
using IptablesNet.Core.Extensions.ExtendedTarget;

using Developer.Common.Types;

namespace IptablesNet.Core
{
	
    /// <summary>
    /// Models all the logic needed to build an iptables rule as a instance
    /// </summary>
	public static class RuleParser
	{
	    
	    /// <summary>
	    /// Builds a command from the string. This also parses every parameter
		/// like the rule.
	    /// </summary>
	    public static GenericCommand GetCommand(string line, NetfilterTable table)
	    {
	        
	        if(!GenericCommand.CanBeACommand (line))
	            return null;
	        
	        NetfilterRule rule= new NetfilterRule();
	        GenericOption option=null;
			GenericCommand gCmd = null;
	        SimpleParameter currParam;
	        
	        try
	        {
	            SimpleParameter[] parameters = RuleParser.GetParameterList(line);
	            
	            int pos=0;
	            
	            while(pos <parameters.Length)
	            {
	                currParam = parameters[pos];
	                if(GenericCommand.IsCommand(currParam.Name))
	                {
                        Exception ex = null;
                        gCmd=null;
                        
                        IptablesCommandFactory.TryGetCommand(currParam,
                           out gCmd, out ex);
                        
                        if(gCmd==null)
                            return null;
						
						//FIXME: What we should do now with the command?!
	                }
	                else if(GenericOption.IsOption(currParam.Name))
	                {
	                    Exception ex=null;
	                    GenericOption opt=null;
	                    
	                    IptablesOptionFactory.TryGetOption(currParam,
                                                           out opt, out ex);
                        option = opt;
                                                           
	                    if(option==null)
	                        return null;
	                    
	                    rule.Options.Add(option);
	                }
	                else
	                {
	                    //The parameter can be a option for an extension
	                    MatchExtensionHandler matchHandler =
	                       rule.FindMatchExtensionHandlerFor(currParam.Name);
	                    
	                    //If is null can't be a extension's parameter
	                    if(matchHandler!=null)
	                    {
    	                    //If its not null is correct and we must add it
    	                    matchHandler.AddParameter(currParam.Name, currParam.Value);
	                    }
	                    else
	                    {   
	                        TargetExtensionHandler targetHandler =
	                           rule.FindTargetExtensionHandler(currParam.Name);
	                        
	                        //If doesn't is a match extension and if it isn't
	                        //a target extension we don't know what the hell is.
	                        if(targetHandler==null)
	                            return null;
	                        
	                        targetHandler.AddParameter(currParam.Name, currParam.Value);
	                        
	                    }
	                }
	                
	                pos++;
	            }
	        }
	        catch(Exception)
	        {
	            return null;
	        }
	        
			gCmd.Rule = rule;
	        return gCmd;
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
	    public static SimpleParameter[] GetParameterList(string line)
	    {
	        string[] parts = StringUtil.Split(line, true, ' ');
	        
	        ArrayList temp = new ArrayList();
	        int pos=0;
	        bool negateNext=false;
	        
	        //Convert parameters and values into objects
	        SimpleParameter par = null;
	        
	        while(pos<parts.Length)
	        {
	            //If the part starts with - is a parameter. If not it is something
	            //unknowm
	            if(parts[pos][0]=='-')
	            {
	                par = new SimpleParameter();
	                
	                if(negateNext)
	                {
	                    par.Not = true;
	                    negateNext = false;
	                }
	                
	                bool longFormat = SimpleParameter.CheckLongFormat(parts[pos]);
                    
                    if(longFormat)
                        par.Name = parts[pos].Substring(2);
                    else
                        par.Name = parts[pos].Substring(1);
	                
	                pos++;
	                par.Value = String.Empty;
	                
	                //Check for negation at the start of values for the current
	                //parameter
	                
	                if(parts[pos].Length==1 && parts[pos][0]=='!')
	                {
	                    par.Not = true;
	                    pos++;
	                }
	                
	                //if the next thing is not a parameter is the value of this
	                //parameter.
	                while(pos<parts.Length && parts[pos][0]!='-')
	                {
	                    if(!String.IsNullOrEmpty(par.Value))
	                        par.Value += " "+parts[pos];
	                    else
	                        par.Value = parts[pos];
	                    
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
	                             throw new IptablesException("Invalid '!' found");    
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
	                // We found negation prior to parameter. Is the negation of
	                // the next parameter.
	                negateNext = true;
	            }
	            
	            pos++;   
	        }
	        
	        //Convert the arrayList to an array of the correct type
	        SimpleParameter[] result = new SimpleParameter[temp.Count];
	        
	        for(int j=0;j<temp.Count;j++)
	        {
	            //Debug.Write("GetParameterList: Adding to result: "+temp[j]);
	            result[j] = (SimpleParameter)temp[j];
	        }
	        
	        return result;
	    }
	    
	   
	}   
}
