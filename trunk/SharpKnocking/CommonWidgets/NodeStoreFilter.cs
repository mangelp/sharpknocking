
using System;
using System.Reflection;
using System.Collections;

using Gtk;

using SharpKnocking.Common;

namespace SharpKnocking.Common.Widgets
{	
	
	public class NodeStoreFilter
	{
		#region Attributes
		private ArrayList store;
		
		private string filterProperty;
		
		private string filter;
		
		private NodeStore nodeStore;
		
		#endregion
		
		public NodeStoreFilter(NodeStore nodeStore, string filterProperty)
		{
			this.store = new ArrayList();
			this.nodeStore = nodeStore;
			this.filterProperty = filterProperty;
		}
		
		#region Properties
		
		/// <summary>
		/// This property alows the user to set and retrieve a filter.
		/// This filter is always lowercase.
		/// </summary>		
		public string Filter
		{
			get
			{
				return filter;
			}
			
			set			
			{
				
				if(Net20.StringIsNullOrEmpty(value))
				{
					ClearFilter();
				}
				else
				{
					filter = value.ToLower();
					DoFilter();
				}
			}
		}
		
		public string FilterProperty
		{
			get
			{
				return filterProperty;
			}
			
			set
			{		
				if(!Net20.StringIsNullOrEmpty(filter))
				{		
					throw new ArgumentException("Filter column can not be empty.");	
				}
				
				filterProperty = value;
				Filter = "";
			}
		}
		
		#endregion Properties
		
		#region Public methods
		
		/// <summary>
		/// This method allows the user to add a new node to the filtered NodeStore.
		/// </summary>
		/// <param name = "node">
		/// The node that will be added.
		/// </param>
		public void Add(ITreeNode node)
		{
			Filter = "";
			store.Add(node);
			nodeStore.AddNode(node);
		}
		
		public void Remove(ITreeNode node)
		{
			Filter = "";
			
			store.Remove(node);
			nodeStore.RemoveNode(node);
		}
		
		public TreeNode[] Nodes
		{
			get
			{
				return (TreeNode[])(store.ToArray(typeof(TreeNode)));
			}
			
		}
		
				
		#endregion Public Methods
		
		#region Private methods
		
		private void ClearFilter()
		{
			filter = "";
			nodeStore.Clear();
			foreach(TreeNode node in store)
				nodeStore.AddNode(node);
		}
		
		private void DoFilter()
		{
			ArrayList aux = new ArrayList();
			
			
			foreach(TreeNode node in store)
			{
				PropertyInfo pinfo = node.GetType().GetProperty(filterProperty);
				string val = pinfo.GetValue(node, null) as string;
				if(val.ToLower().IndexOf(filter) != -1)
					aux.Add(node);
			}
			
		
			nodeStore.Clear();
			foreach(TreeNode node in aux)
			{
				nodeStore.AddNode(node);
			}
			
		}
		
		#endregion Private methods
	}
}
