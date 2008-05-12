using System;
using System.IO;

namespace Developer.Common.System
{
	/// <summary>
	/// Detects details about the current unix system
	/// </summary>
	public static class UnixOsDetect
	{
		private static string releaseFedora = "/etc/fedora-release";
		
		private static UnixPlatform platform;
		
		/// <summary>
		/// Unix platform id
		/// </summary>
		public static UnixPlatform Platform
		{
			get { return platform; }
		}
		
		private static string version;
		
		/// <summary>
		/// Platform version string
		/// </summary>
		public static string Version
		{
			get { return version; }
		}
		
		private static string release;
		
		/// <summary>
		/// Platform release string
		/// </summary>
		public static string Release
		{
			get { return release; }
		}
		
		private static string codename;
		
		/// <summary>
		/// Release code name
		/// </summary>
		public static string Codename
		{
			get { return codename; }
		}
		
		private static void GetData()
		{
			platform = UnixPlatform.Other;
			if (File.Exists(releaseFedora))
				platform = UnixPlatform.Fedora;
			
			switch(platform)
			{
				case UnixPlatform.Fedora:
					string[] data = File.ReadAllLines(releaseFedora);
					data = data[0].Split(' ');
					version = data[2];
					release = data[1];
					codename = data[3].Trim(' ', '(', ')');
					break;
				default:
					throw new NotImplementedException("Missing support for "+platform);
			}
		}
		
		static UnixOsDetect()
		{
			GetData();
		}
	}
}