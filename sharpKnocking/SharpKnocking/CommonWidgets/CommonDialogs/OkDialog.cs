
using System;
using Gtk;

namespace SharpKnocking.Common.Widgets.CommonDialogs
{
	
	/// <summary>
	/// This class inherits from <c>MessageDialog</c>, implementing
	/// a dialog with only an "Ok" button.
	/// </summary>
	public class OkDialog : MessageDialog
	{
		/// <summary>
		/// This is <c>OkDialog</c>'s constructor.
		/// </summary>
		/// <param name = "parent">
		/// The window the dialog is created from.
		/// </param>
		/// <param name = "type">
		/// The type of the message.
		/// </param>
		/// <param name = "message">
		/// The message which will be shown as the dialog's text.
		/// </param>
		private OkDialog(Window parent, MessageType type, string message, params object[] args)
			: base(	parent,
					DialogFlags.DestroyWithParent,
					type,
					ButtonsType.Ok,
					message, args)
		{
			this.Modal = true;
			
			if(parent!=null)
			{
			   this.TransientFor = parent;
			   this.Icon = parent.Icon;
			}
			else
			  this.Icon = ImageResources.SharpKnockingIcon22;
			
			switch(type)
			{
				case MessageType.Error:
					this.Title = "Error";
					break;
				case MessageType.Info:				
					this.Title = "Información";
					break;
				case MessageType.Warning:
					this.Title = "Advertencia";
					break;
				case MessageType.Question:
					throw new ArgumentException(
						"Don't use this class for a question dialog");		
				
			}
		}
		
		
		private new ResponseType Run()
		{
			return (ResponseType)(base.Run());		
		}
		
		
		/// <summary>
		/// This method shows a message dialog to the user.
		/// </summary>
		/// <param name = "parent">
		/// The window the dialog is created from.
		/// </param>
		/// <param name = "type">
		/// The type of the message.
		/// </param>
		/// <param name = "message">
		/// The message which will be shown as the dialog's text.
		/// </param>
		public static ResponseType Show(Window parent, MessageType type, string message, params object[] args)
		{
			OkDialog dialog = new OkDialog(parent,type,message,args);
			
			ResponseType res = dialog.Run();
			
			dialog.Destroy();
			
			return res;
		}
	}
}
