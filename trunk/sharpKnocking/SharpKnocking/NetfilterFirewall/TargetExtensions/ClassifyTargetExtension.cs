
using System;

using SharpKnocking.Common;

namespace SharpKnocking.NetfilterFirewall.ExtendedTarget
{
	
	
	public class ClassifyTargetExtension: TargetExtensionHandler
	{
	    /// <summary>
	    /// This module allows you to set the skb->priority value (and  thus  clas-
        /// sify the packet into a specific CBQ class).
        /// </summary>
		public ClassifyTargetExtension()
		  :base(typeof(ClassifyTargetOptions), "classify")
	    {
		          
	    }
	    
	    public override TargetExtensionParameter CreateParameter ()
	    {
	    	return new ClassifyParameter(this);
	    }
	    
	    public override TargetExtensionParameter CreateParameter (string name, string value)
	    {
	        ClassifyParameter param = new ClassifyParameter(this);
	        param.Name = name;
	        param.Value = value;
	        
	        return param;
	    }

        public override Type GetInternalParameterType ()
        {
        	return typeof(ClassifyParameter);
        }
	    
	    
	    public class ClassifyParameter: TargetExtensionParameter
	    {

	        
	        public new ClassifyTargetExtension Owner
	        {
	            get { return (ClassifyTargetExtension)base.Owner;}
	            set { base.Owner = (TargetExtensionHandler)value; }
	        }
	        
	        private string major;
	        
	        /// <summary>
	        /// Major class value
	        /// </summary>
	        public string Major
	        {
	            get { return this.major;}
	            set
	            {
	                this.UpdateValue();
	                this.major = value;
	            }
	        }
	        
	        private string minor;
	        
	        /// <summary>
	        /// Minor class value
	        /// </summary>
	        public string Minor
	        {
	            get { return this.minor; }
	            set
	            {
	                this.UpdateValue();
	                this.minor = value;
	            }
	        }
	        
	        private ClassifyTargetOptions option;
	        
	        public ClassifyTargetOptions Option
	        {
	            get { return this.option;}
	            set
	            {
	                this.option = value;
	                this.DisableInternalParsing = true;
	                base.Name = TypeUtil.GetDefaultAlias(value);
	                this.DisableInternalParsing = false;
	            }
	        }
	        
	        public ClassifyParameter(TargetExtensionHandler owner)
	          :base(owner)
	        {
	            
	        }
	        
	        private void UpdateValue()
	        {
	            this.DisableInternalParsing = true;
	            base.Value = this.major+":"+this.minor;
	            this.DisableInternalParsing = false;
	        }
	        
	        protected override void ParseValue(string value)
	        {
	            int pos = value.IndexOf(':');
	            
	            if(pos<0)
	                throw new FormatException("The value has incorrect format");
	            
	            this.minor = value.Substring(0, pos);
	            this.major = value.Substring(pos+1);
	        }
	        
	        protected override void ParseName (string name)
	        {
	            object objType = this.Owner.ValidateAndGetParameter(name);
	            this.option = (ClassifyTargetOptions)objType;
	        }

	    }

	}
}
