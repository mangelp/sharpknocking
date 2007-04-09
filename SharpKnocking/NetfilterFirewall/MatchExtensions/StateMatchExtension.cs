
using System;
using System.Collections;
using SharpKnocking.Common;
using SharpKnocking.NetfilterFirewall;

namespace SharpKnocking.NetfilterFirewall.ExtendedMatch
{
	public class StateMatchExtension: MatchExtensionHandler
	{
	    public StateMatchExtension()
	      :base(typeof(StateExtensionOptions), "state")
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
            ConnectionStates result;
            
            if(StateMatchExtension.TryGetStateType(type, out result))
                return result;
            
            return ConnectionStates.None;
        }
        
        /// <summary>
        /// Returns true if the string can be converted correctly or false if not
        /// </summary>
        /// <remarks>
        /// see GetStateType
        /// </remarks>
        public static bool TryGetStateType(string type,out ConnectionStates result)
        {
            result = ConnectionStates.None;
            
            string[] list = Net20.StringSplit(type, true, ',');
            
            if(list.Length==0)
            {
                // FIXED: This method must not throw any exception 
                // throw new FormatException("Bad formed value. It must be a list"+
                //               " of valid states separated by commas");
                return false;
            }
            
            result = ConnectionStates.None;
            ConnectionStates state;
            
            for(int i=0;i<list.Length;i++)
            {
                if(StateMatchExtension.TryGetSingleStateType(list[i], out state))
                {
                    result = state | result; 
                }
                else
                {
                    Debug.VerboseWrite("Not a valid connection state: "+list[i]);
                    return false;
                }
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
            
            if(!TypeUtil.IsAliasName(typeof(ConnectionStates), type, out oObj))
                return false;
            
            state = (ConnectionStates)oObj;
            
            return true;
        }
        
        public override MatchExtensionParameter CreateParameter ()
        {
        	return new StateParameter(this);
        }
        
        public override MatchExtensionParameter CreateParameter (string name, string value)
        {
            StateParameter result = new StateParameter(this);
            result.Name = name;
            result.Value = value;
            
            return result;
        }
        
        public override Type GetInternalParameterType ()
        {
        	return typeof(StateParameter);
        }
        
        public override bool IsValidName (object option)
        {
        	return (option is StateExtensionOptions);
        }
        
        public override bool IsValidValue (object option, object value)
        {
            if(!this.IsValidName(option))
                return false;
            
            switch((StateExtensionOptions)option)
		    {
		        case StateExtensionOptions.State:
		            return (value is ConnectionStates);
		        default:
		            throw new InvalidOperationException("Unsupported option "+option);
		    }
        }
        
        public override bool TryConvertToName (string paramName, out object objName)
		{
			objName = null;
		    
		    try
		    {
		        if(TypeUtil.IsAliasName(typeof(StateExtensionOptions), paramName, out objName))
		            return true;
		        else
		            return false;
		    }
		    catch(Exception)
		    {
		       return false;
		    }
		}
		
		public override bool TryConvertToValue (string name, string value, out object objValue)
		{
			objValue = null;
		    object objName = null;
		    
		    if(!this.TryConvertToName(name, out objName))
		        return false;
		    
		    switch((StateExtensionOptions)objName)
		    {
		        case StateExtensionOptions.State:
		            ConnectionStates oType = ConnectionStates.None;
		            if(!StateMatchExtension.TryGetStateType(value, out oType))
		                return false;
		            objValue = oType;
		            break;
		        default:
                    throw new InvalidOperationException("Unsupported option "+objName);
		    }
		    
		    return true;
		}



        public class StateParameter: MatchExtensionParameter
        {
            public new StateMatchExtension Owner
            {
                get { return (StateMatchExtension)base.Owner;}
                set { base.Owner = value;}
            }
            
            private StateExtensionOptions option;
            
            public StateExtensionOptions Option
            {
                get { return this.option;}
                set { this.option = value;}
            }
            
            private ConnectionStates state;
            
            public ConnectionStates State
            {
                get { return this.state;}
                set { this.state = value;}
            }
            
            public StateParameter(StateMatchExtension owner)
              :base((MatchExtensionHandler)owner)
            {
                  
            }

        }

	}
}
