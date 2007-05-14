
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using SharpKnocking.Common;
using IptablesNet.Core;
using IptablesNet.Core.Options;
using IptablesNet.Core.Commands;
using IptablesNet.Core.Extensions.ExtendedMatch;
using IptablesNet.Core.Extensions.ExtendedTarget;


namespace IptablesNet.Core
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
	public class NetfilterRule: IComparable <NetfilterRule>
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
	    
	    private List<MatchExtensionHandler> loadedExtensions;
		private ReadOnlyMatchExtensionListAdapter adapter;
	    
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
	           return adapter;
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
		    this.loadedExtensions = new List<MatchExtensionHandler>();
			this.adapter = new ReadOnlyMatchExtensionListAdapter(this.loadedExtensions);
		    this.options.ItemAdded += new GenericOptionListEventHandler(this.ItemAddedHandler);
		    this.options.ItemRemoved += new GenericOptionListEventHandler(this.ItemRemovedHandler);
		    this.options.ItemsCleared += new EventHandler(this.ItemsClearedHandler);
		}
		
		private void ItemAddedHandler(object obj, ListChangedEventArgs<GenericOption> args)
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
				//FIXME: GOT BROKEN!
//		        Type t = MatchExtensionFactory.GetExtensionType(args.Item.Value);
//		        this.LoadExtensionHandler(t);
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
		
		private void UnloadExtensionHandler(Type extType)
		{
			int pos=0;
			
			while(pos<this.loadedExtensions.Count)
			{
				if(this.loadedExtensions[pos].GetType()==extType)
				{
					this.loadedExtensions.RemoveAt(pos);
					return;
				}
			}
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
		
		private void ItemRemovedHandler(object obj, ListChangedEventArgs<GenericOption> args)
		{
		    //If an extension option is removed we must remove that extension with
		    //all the options.
		    
		    if(!args.Item.HasImplicitExtension)
		        return;
		    else if(args.Item is JumpOption)
		    {
		        this.jumpOption = null;    
		    }
		    
		    this.UnloadExtensionHandler(args.Item.ExtensionType);
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

		        if(handler.IsSupportedParam(paramName))
		        {
		            Debug.VerboseWrite("FindMatchExtensionHandlerFor: Supporting extension found! "
                                     +handler.ExtensionName,
                                     VerbosityLevels.Insane);
		            
                    return handler;
		        }
		    }
		    
		    return null;
		}
		
		//---------------------------------------------------
		// IComparable<T> implementation and overrides of ToString, Equals and
		// GetHashCode.
		
		public override string ToString ()
		{
		    StringBuilder stb = new StringBuilder();
			// FIXME: The rule doesn't have now the reference to the command. We
			// should fix the generation of the string as this brokes it, and the
			// generated string isn't in the correct format now. 
			//-------
			//stb.Append(this.command+" ");
		    
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
		
		public override bool Equals (object o)
		{
			if(!(o is NetfilterRule) || o == null)
				return false;
			
			string strThis = this.ToString ();
			return strThis.Equals(o.ToString(), StringComparison.InvariantCultureIgnoreCase);
		}

		public override int GetHashCode ()
		{
			return this.ToString ().GetHashCode ();
		}
		
		public int CompareTo (NetfilterRule rule)
		{
			if(rule==null)
				throw new ArgumentNullException ("rule");
				
			string strThis = this.ToString ();
			return strThis.CompareTo (rule.ToString());
		}

	}
}
