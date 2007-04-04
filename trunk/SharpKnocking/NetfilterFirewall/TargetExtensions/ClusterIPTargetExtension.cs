
using System;
using SharpKnocking.Common;

namespace SharpKnocking.NetfilterFirewall.ExtendedTarget
{
	
	/// <summary>
    /// This  module  allows  you  to  configure a simple cluster of nodes that
    /// share a certain IP and MAC address without an explicit load balancer in
    /// front  of  them.   Connections  are  statically distributed between the
    /// nodes in this cluster.
	/// </summary>
	public class ClusterIPTargetExtension: TargetExtensionHandler
	{
		
		public ClusterIPTargetExtension()
		  :base(typeof(ClusterIPTargetOptions), "clusterip")
		{
            
		}
		
		public override TargetExtensionParameter CreateParameter ()
		{
			return new ClusterIPParameter(this);
		}
		
		public override TargetExtensionParameter CreateParameter (string name, string value)
		{
		    ClusterIPParameter param = new ClusterIPParameter(this);
		    param.Name = name;
		    param.Value = value;
		    
		    return param;
		}
		
		public override Type GetInternalParameterType ()
		{
            return typeof(ClusterIPParameter);
		}
		
		
		public class ClusterIPParameter: TargetExtensionParameter
		{
		    public new ClusterIPTargetExtension Owner
		    {
		        get { return (ClusterIPTargetExtension)base.Owner;}
		        set { base.Owner = (TargetExtensionHandler)value;}
		    }
		    
		    private ClusterIPTargetOptions option;
		    
		    /// <summary>
		    /// Current option parameter
		    /// </summary>
		    public ClusterIPTargetOptions Option
		    {
		        get { return this.option;}
		        set
		        {
		            this.option = value;
		            base.Name = TypeUtil.GetDefaultAlias(this.option);
		        }
		    }

		    
		    public ClusterIPParameter(ClusterIPTargetExtension owner)
		      :base(owner)
		    {
		          
		    }
		    
		    protected override void ParseValue (string value)
		    {
		        switch(this.option)
		        {
		            case ClusterIPTargetOptions.New:
		                throw new InvalidOperationException(
		                  "This parameter doesn't have a value!");
		            case ClusterIPTargetOptions.TotalNodes:
		            case ClusterIPTargetOptions.LocalNode:
		                Int32.Parse(this.Value);
		                break;
		            case ClusterIPTargetOptions.Hashmode:
		                if(!TypeUtil.IsEnumValue(typeof(ClusterIPHashingMode), value))
		                    throw new FormatException("Invalid hashing mode");
		                break;
		            case ClusterIPTargetOptions.Clustermac:
		            case ClusterIPTargetOptions.HashInit:
		                if(Net20.StringIsNullOrEmpty(value))
		                    throw new FormatException("The value can't be null or empty");
		                break;
		        }
		    }
		    
		    protected override void ParseName (string name)
		    {
		        object objType = this.Owner.ValidateAndGetParameter(name);
		        this.option = (ClusterIPTargetOptions)objType;
		    }
		    
		    
		    /// <summary>
		    /// Returns the value of the parameter as an int if the type of the
		    /// value is an int.
		    /// </summary>
		    /// <remarks>
		    /// If the type is not an int this will throw a FormatException
		    /// </remarks>
		    public int GetValueAsInt()
		    {
		        if(this.option != ClusterIPTargetOptions.LocalNode &&
		                     this.option != ClusterIPTargetOptions.TotalNodes)
                {
                    throw new FormatException("The value type for this parameter is not an int!");
                }
                
                return Int32.Parse(this.Value);
		    }
		    
		    
		    /// <summary>
		    /// Returns the value of the parameter as an int if the type of the
		    /// value is an int.
		    /// </summary>
		    /// <remarks>
		    /// If the type is not an int this will throw a FormatException
		    /// </remarks>
		    public ClusterIPHashingMode GetValueAsClusterIPHashingMode()
		    {
		        if(this.option != ClusterIPTargetOptions.Hashmode)
		        {
		            throw new FormatException("The value type for this parameter is not a hashing mode!");    
		        }
		        
		        object objType;
		        
		        if(TypeUtil.IsAliasName(typeof(ClusterIPHashingMode), this.Value, out objType))
		            return (ClusterIPHashingMode)objType;
		        else
		            throw new FormatException("Can't format value. Unknown error.");
		    }
            
		}

	}
}
