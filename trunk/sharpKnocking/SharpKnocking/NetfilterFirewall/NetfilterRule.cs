
using System;
using System.Text;
using System.Collections;

using SharpKnocking.Common;
using SharpKnocking.NetfilterFirewall;
using SharpKnocking.NetfilterFirewall.Options;
using SharpKnocking.NetfilterFirewall.Commands;
using SharpKnocking.NetfilterFirewall.ExtendedMatch;
using SharpKnocking.NetfilterFirewall.ExtendedTarget;


namespace SharpKnocking.NetfilterFirewall
{
	
	/// <summary>
	/// Base class for rules. It store an array of generic parameters with
	/// all the parameters that the rule contains. The rule also has the
	/// command parameter included.
	/// </summary>
	/// <remarks>
	/// To use match extension options in the rule you have to add an option
	/// to load explicitly the extension and then you can access the extension
	/// handler from the list and add the options to the extension handler object.
	/// </remarks>
	public class NetfilterRule
	{
	    private NetfilterChain parentChain;
	    
	    /// <summary>
	    /// Table where this rule is defined.
	    /// </summary>
	    /// <remarks>
	    /// Adding an option with the -t parameter to change the target table
	    /// doesn't change this value but it will cause a runtime exception
	    /// when trying to convert this rule as text.
	    /// </remarks>
	    public NetfilterChain ParentChain
	    {
	        get {return this.parentChain;}
	        set {this.parentChain = value;}
	    }
	    
	    private GenericCommand command;
	    
	    /// <summary>
	    /// Command for the rule.
	    /// </summary>
	    /// <remarks>
	    /// Each rule must have only one command.<br/>
	    /// Depending on the type of command the rule can have a set of
	    /// options (extended or not) that is the real specification for the rule.
	    /// </remarks>
	    public GenericCommand Command
	    {
	        get { return this.command;}
	        set { this.command = value;}
	    }
	    
	    private MatchExtensionList loadedExtensions;
	    
	    /// <summary>
	    /// Gets the list of loaded extensions in the rule.
	    /// </summary>
	    /// <remarks>
	    /// Adding a extension is the same as adding the -m option to the
	    /// command line. But there is one main difference, here the extended
	    /// options must be added to the extension directly, not to the list
	    /// of normal options (see Options property).
	    /// </remarks>
	    public ReadOnlyMatchExtensionListAdapter LoadedExtensions
	    {
	        get
	        {
	            //We return a reference to the adapter that will do read-only
	            //access to the list
	           return this.loadedExtensions.Adapter;    
	        }
	    }
	    
	    private RuleOptionList options;
	    
	    /// <summary>
	    /// Gets the array of options for this rule.
	    /// </summary>
	    public RuleOptionList Options
	    {
	        get
	        {
	            return this.options;
	        }
	    }
	    
	    private JumpOption jumpOption;
	    
	    /// <summary>
	    /// Default constructor.
	    /// </summary>
		public NetfilterRule()
		{
		    this.options = new RuleOptionList(this);
		    this.loadedExtensions = new MatchExtensionList(this);
		    this.options.ItemAdded += new GenericOptionListEventHandler(this.ItemAddedHandler);
		    this.options.ItemRemoved += new GenericOptionListEventHandler(this.ItemRemovedHandler);
		    this.options.ItemsCleared += new EventHandler(this.ItemsClearedHandler);
		}
		
		private void ItemAddedHandler(object obj, GenericOptionListEventArgs args)
		{
		    if(args.Item.HasImplicitExtension)
		    {
		        Debug.VerboseWrite("NetfilterRule:: Option added with implicit extension: "+args.Item);
    		    //Exit if the implicit extension type have been loaded
    		    if(this.IsExtensionHandlerLoaded(args.Item.ExtensionType))
    		        return;
    		    
                this.LoadExtensionHandler(args.Item.ExtensionType);
		    }
		    else if(args.Item is MatchExtensionOption)
		    {
		        Debug.VerboseWrite("NetfilterRule:: Option added with explicit extension: "+args.Item);
		        //If it's this option we load the extension
		        Type t = MatchExtensionFactory.GetExtensionType(args.Item.Value);
		        this.LoadExtensionHandler(t);
		    }
		    else if(args.Item is JumpOption)
		    {
		        //we keep a reference to this option to check names
		        this.jumpOption = (JumpOption)args.Item;
		    }
		}
		
		private void LoadExtensionHandler(Type extHandlerType)
		{
		    Debug.VerboseWrite("NetfilterRule: Loading extension handler: "+extHandlerType.Name);
            //Load the implicit extension
            MatchExtensionHandler handler = MatchExtensionFactory.GetExtension(
                                                    extHandlerType);
		    Debug.VerboseWrite("NetfilterRule: Adding the current list ");
            //Add to the list
            this.loadedExtensions.Add(handler);
		}
		
		private bool IsExtensionHandlerLoaded(Type extHandlerType)
		{
            for(int i=0;i<this.loadedExtensions.Count;i++)
            {
                if(this.loadedExtensions[i].GetType() == extHandlerType)
                    return true;
            }
            
            return false;
		}
		
		private void ItemRemovedHandler(object obj, GenericOptionListEventArgs args)
		{
		    //If an extension option is removed we must remove that extension with
		    //all the options.
		    
		    if(!args.Item.HasImplicitExtension)
		        return;
		    else if(args.Item is JumpOption)
		    {
		        this.jumpOption = null;    
		    }
		    
		    this.loadedExtensions.Remove(args.Item.ExtensionType);
		}
		
		private void ItemsClearedHandler(object obj, EventArgs args)
		{
		    //When all the items all cleared we must clear the loaded extensions
		    //list too.
		    this.loadedExtensions.Clear();
		}
		
		/// <summary>
		/// Gets if the current extension target can accept a parameter with a 
		/// given name.
		/// </summary>
		/// <remarks>
		/// The extension targets are specified throught the jump option
		/// <c>Options.JumpOption</c>
		/// </remarks>
		public bool IsTargetExtensionParameter(string paramName)
		{
		    if(this.jumpOption!=null && this.jumpOption.HasOptionNamed(paramName))
		      return true;
		    
		    return false;
		}
		
		/// <summary>
		/// Returns an instance of the handler for the parameter.
		/// </summary>
		/// <remarks>
		/// If it is a parameter supported by the current loaded target extension
		/// the extension is returned but if the extension doesn't support the
		/// parameter or there is no extension target loaded it returns null.
		/// </remarks>
		public TargetExtensionHandler FindTargetExtensionHandler(string paramName)
		{
            Debug.VerboseWrite("Searching target extension handler for: "+paramName);
            
		    if(this.jumpOption!=null &&
		             this.jumpOption.HasOptionNamed(paramName))
		    {
		        return this.jumpOption.Extension;
		    }
		    
		    return null;
		}
		
		/// <summary>
		/// Iterates the list of loaded handlers searching for one that supports
		/// the parameter.
		/// </summary>
		/// <returns>
		/// The instance of the loaded handler that supports the parameter or
		/// null if it doesn't exist
		/// </returns>
		public MatchExtensionHandler FindMatchExtensionHandlerFor(string paramName)
		{
		    Debug.VerboseWrite("FindMatchExtensionHandlerFor: Searching for a extension with a "
	                             +paramName+" option over "
	                             +this.loadedExtensions.Count
	                             +" extensions",
  		                         VerbosityLevels.Insane);
		    
		    MatchExtensionHandler handler;
		    
		    for(int i=0;i<this.loadedExtensions.Count;i++)
		    {
		        
		        handler = this.loadedExtensions[i];
		        
		        Debug.VerboseWrite("FindMatchExtensionHandlerFor: Testing at "+i
		                               +" on "+handler.ExtensionName
                                     , VerbosityLevels.Insane);

		        if(handler.IsValidName(paramName))
		        {
		            Debug.VerboseWrite("FindMatchExtensionHandlerFor: Supporting extension found! "
                                     +handler.ExtensionName,
                                     VerbosityLevels.Insane);
		            
                    return handler;
		        }
		    }
		    
		    return null;
		}
		
		/// <summary>
		/// Gets if the line can be a rule candidate. 
		/// </summary>
		/// <remarks>
		/// This only checks if the line starts with a -. So this check is poor
		/// and only useful when reading lines in iptables config format (the
        /// same format as iptables-save output)
        /// </summary>
		public static bool IsRule(string line)
		{
		    line = line.Trim();
		    
		    if(line.StartsWith("-"))
		    {
		        return true;
		    }
		    
		    return false;
		}
		
		public override string ToString ()
		{
		    StringBuilder stb = new StringBuilder();
		    stb.Append(this.command+" ");
		    
		    for(int i=0;i<this.options.Count;i++)
		    {
		        stb.Append(this.options[i]+" ");    
		    }
		    
		    for(int i=0;i<this.loadedExtensions.Count;i++)
		    {
		        stb.Append(this.LoadedExtensions[i]+" ");    
		    }
		    
		    return stb.ToString();
		}

	}
}
