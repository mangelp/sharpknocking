// RuleParser.cs
//
//  Copyright (C) 2007 iSharpKnocking project
//  Created by Miguel Angel Perez (mangelp{@}gmail{d0t}com)
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA 
//
//

using System;
using System.Collections;

using IptablesSharp.Core;
using IptablesSharp.Core.Options;
using IptablesSharp.Core.Commands;
using IptablesSharp.Core.Extensions;
using IptablesSharp.Core.Extensions.ExtendedMatch;
using IptablesSharp.Core.Extensions.ExtendedTarget;

using Developer.Common.Types;
using Developer.Common.Options;

namespace IptablesSharp.Core
{
    /// <summary>
    /// Parser for rule lines in the format of iptables-save command.
    /// </summary>
	public static class RuleParser
	{
	    /// <summary>
	    /// Builds a command from the string. This also parses every parameter
		/// like the rule.
	    /// </summary>
		/// <param name="curTable">Table where is defined the chain that the
		/// command affects</param>
		/// <remarks>
		/// The line must be in the same format that iptables-save uses for output. So
		/// any line must start with a command followed by the parameters.
		/// In this context the real rules are parameters of the command so at the
		/// end the command will have in her properties everything. 
		/// </remarks>
	    public static GenericCommand GetCommand(string line, NetfilterTable curTable)
	    {
			//Console.WriteLine("Processing line: "+line);
			//If the line doesn't look like a valid command we can't do nothing
			if(!GenericCommand.CanBeACommand (line))
				return null;
			
	        GenericOption option=null;
			GenericCommand gCmd = null;
			Exception ex = null;
			SimpleParameter currParam;
			int pos=1;
			MatchExtensionHandler meh = null;
			TargetExtensionHandler teh = null;
			
            SimpleParameter[] parameters = RuleParser.GetParameterList(line);
			
			if(parameters.Length==0)
				throw new ArgumentException("There are nothing to parse here: "+line);
			//The first thing must be the command
			else if(!GenericCommand.IsCommand(parameters[0].Name))
				throw new FormatException("The line doesn't start with a command");
			//Try to get the command or throw an exception
			else if(!IptablesCommandFactory.TryGetCommand(parameters[0], out gCmd, out ex))
				throw new FormatException("Error while parsing line: "+line, ex);
			//With only 2 parameters there is no room for a rule specifcation. Cry if it is required
			else if(gCmd.MustSpecifyRule && parameters.Length<2)
				throw new FormatException("Unexpected parameters in line after command: "+line);
            
			if(gCmd.MustSpecifyRule)
			   gCmd.Rule = new NetfilterRule();
			   
			//Now we must parse the rest of the parameters and add them to the command
			while(pos <parameters.Length) {
				currParam = parameters[pos];
				//Console.WriteLine("Processing["+pos+"-"+(parameters.Length-1)+"]: "+currParam);
				//Give to the parameter the correct procesing based in the guess
				//of his type
				if(GenericOption.IsOption(currParam.Name)) {
					//If this command doesn't have a rule there can't be more than the 
					//parameters for the command that where already grouped into the
					//first SimpleParameter object. So if there is a parameter something
					//got broken and we must give back an exception
					if(!gCmd.MustSpecifyRule)
						throw new FormatException("Found rule where no rule was expected");
					//We use a factory to build an option object
					else if(!IptablesOptionFactory.TryGetOption(currParam, out option, out ex)) {
						throw ex;
					}
					//Console.WriteLine("Adding option parameter: "+currParam);
					//Add the option to the rule
					gCmd.Rule.Options.Add(option);
				} else if(gCmd.Rule.TryGetMatchExtensionHandler(currParam.Name, out meh)) {
					//Console.WriteLine("Adding a match extension parameter: "+currParam);
					//The parameter is an option for a match extension. We add to it
					//TODO: If the name is not a valid parameter we get a null reference exception
					//check that.
					meh.AddParameter(currParam.Name, currParam.Value);
				} else if(gCmd.Rule.TryGetTargetExtensionHandler(currParam.Name, out teh)) {
					//Console.WriteLine("Adding a target extension parameter: "+currParam.Name+", "+currParam.Value);
					//The parameter is an option or the target extension of the rule
					//We add to it
					teh.AddParameter(currParam.Name, currParam.Value);
				}

                pos++;
            }
	        
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
