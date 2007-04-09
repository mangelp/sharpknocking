
using System;

using Gtk;

using SharpKnocking.Common.Calls;

namespace SharpKnocking.PortKnocker
{
	/// <summary>
	/// This class implements the nodes used to store and represent
	/// call sequences.
	/// </summary>    
    public class CallNode : TreeNode
    {
        private CallSequence callSequence;

        /// <summary>
    	/// CallNode's constructor.
    	/// </summary>
    	/// <param name = "callSequence">
    	/// The CallSequence's instance which will be stored in the node.
    	/// </param>
        public CallNode (CallSequence callSequence)
        {
            this.callSequence = callSequence;
        }

        
        [TreeNodeValue (Column=1)]
        public string Address 
        {
            get 
            { 
                return callSequence.Address;
            }
        }

		[TreeNodeValue (Column=2)]
		public int TargetPort
		{
			get
			{
				return callSequence.TargetPort;
			}
		}
			
        [TreeNodeValue (Column=0)]
        public string Description
        {
            get 
            { 
                return callSequence.Description; 
            }
        }
        
        /// <summary>
        /// This property is used to retrieve or modify the instance of CallSequence
        /// which is stored in the node.
        /// </summary>
        public CallSequence Sequence
        {
            get
            {
                return callSequence;
            }
            set
            {
                callSequence = value;
            }
        }
     }
}


