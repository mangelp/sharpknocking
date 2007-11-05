
using System;
using System.Text;
using System.Collections;

using IptablesNet.Core.Extensions;
using IptablesNet.Core.Extensions.ExtendedMatch;

using Developer.Common.Net;
using Developer.Common.Types;

namespace IptablesNet.Extensions.Matches
{
	public class StateMatchExtension: MatchExtensionHandler
	{
	    public StateMatchExtension()
	      :base(typeof(StateMatchOptions), MatchExtensions.State)
		{
		}
		
		/// <summary>
		/// Converts a comma separated string of valid tcp connection states
		/// to an enumeration constact.
		/// </summary>
		/// <returns>
		/// The result of applying the logical or opertaion between all the
		/// values of the list or ConnectionStates.None if no value was found or
		/// if the string was not in a valid format.
		/// </returns>
		public static ConnectionStates GetStateType(string type)
        {
			//Console.Out.WriteLine("** Getting state from: "+type);
            ConnectionStates result;
            
            if(StateMatchExtension.TryGetStateType(type, out result))
                return result;
            
			throw new FormatException("The string is not a valid status");
        }
        
        /// <summary>
        /// Returns true if the string can be converted correctly or false if not
        /// </summary>
        /// <remarks>
        /// see GetStateType
        /// </remarks>
        public static bool TryGetStateType(string type,out ConnectionStates result)
        {
			//Console.Out.WriteLine("** Try getting state from: "+type);
            result = ConnectionStates.None;
            
            string[] list = StringUtil.Split(type, true, ',');
            
            if(list.Length==0)
            {
                // FIXED: This method must not throw any exception 
                // throw new FormatException("Bad formed value. It must be a list"+
                //               " of valid states separated by commas");
                return false;
            }
            
            ConnectionStates state;
            
            for(int i=0;i<list.Length;i++)
            {
                if(StateMatchExtension.TryGetSingleStateType(list[i], out state))
                {
                    result = state | result; 
                }
                else
                    return false;
            }
            
            return true;
        }
        
        /// <summary>
        /// Returns true if the string can be converted to a constant of the
        /// ConnectionStates enumeration (only one of them) or false if not.
        /// </summary>
        public static bool TryGetSingleStateType(string type, out ConnectionStates state)
        {
            state = ConnectionStates.None;
            object oObj=null;
            
            if(!AliasUtil.IsAliasName(typeof(ConnectionStates), type, out oObj))
                return false;
            
            state = (ConnectionStates)oObj;
            
            return true;
        }
        
        public override MatchExtensionParameter CreateParameter (string paramType)
        {
            object val=null;
            if(!AliasUtil.IsAliasName (typeof(StateMatchOptions), paramType, out val))
                return null;
            
            StateMatchOptions option = (StateMatchOptions)val;
            
            switch(option)
            {
                case StateMatchOptions.State:
                    return new StateParam (this);
                default:
                    throw new ArgumentException ("Not supported option: "+option,"name");
            }
        }
        
        public StateParam CreateParameter()
        {
            return new StateParam(this);
        }

        public class StateParam: MatchExtensionParameter
        {
            public new StateMatchExtension Owner
            {
                get { return (StateMatchExtension)base.Owner;}
            }
            
            public new StateMatchOptions Option
            {
                get { return (StateMatchOptions)base.Option;}
            }
            
            private ConnectionStates state;
            
            public ConnectionStates State
            {
                get { return this.state;}
                set { this.state = value;}
            }
            
            public StateParam(StateMatchExtension owner)
              :base((MatchExtensionHandler)owner, StateMatchOptions.State)
            {
                  
            }
            
            protected override string GetValuesAsString ()
            {
                return this.GetStateFlagsString(this.state, true);
            }
            
            public override void SetValues (string value)
            {
				//Console.Out.WriteLine("Getting status values from: "+value);
                object obj=null;
                string[] list = StringUtil.Split (value, true, ',');
                
				this.state = ConnectionStates.None;
                
                for(int i=0;i<list.Length;i++){
					//Console.Out.WriteLine("Is "+list[i]+" a enumeration member?");
                    if(AliasUtil.IsAliasName(typeof(ConnectionStates), list[i], out obj))
                    {
						//Console.Out.WriteLine("Yes: "+obj);
						this.state = this.state | (ConnectionStates)obj;
						//Console.Out.WriteLine("After asigning we have: "+this.state);
                    }
                    else
                    {
                        throw new InvalidCastException ("Can't convert from "+
                                                        list[i]+
                                                        " to ConnectionStates enum");
                    }
                }
            }
            
            private string GetStateFlagsString (ConnectionStates states, bool iptablesFormat)
            {
                Array values = Enum.GetValues (typeof(ConnectionStates));
                StringBuilder sb = new StringBuilder();
                
                foreach (ConnectionStates value in values) {
					//Fix: Exclude the None value as it is 0 matches everytime
					if( (value & states) == value && value != ConnectionStates.None )
                        sb.Append(value + ",");
                }
				
                //Remove the ',' character at the end
                sb.Remove(sb.Length-1, 1);
				
				if(!iptablesFormat)
					return sb.ToString();
				else
					return sb.ToString().ToUpper();
            }
        }

	}
}
