
using System;
using Gdk;

namespace SharpKnocking.Common
{
	
	/// <summary>
	/// This class serves as a repository for the icons and images used
	/// in all the interfaces.
	/// </summary>
	public class ImageResources
	{
		
		public static Pixbuf FileExportIcon16
		{
			get
			{
				return Pixbuf.LoadFromResource("fileexport.png");
			}
		}
	
		public static Pixbuf SharpKnockingIcon22
		{
			get
			{
				return Pixbuf.LoadFromResource("sharpknocking22.png");
			}
		}
		
		public static Pixbuf SharpKnockingIcon96
		{
			get
			{
				return Pixbuf.LoadFromResource("sharpknocking96.png");
			}
		}
		
		
	}
}
