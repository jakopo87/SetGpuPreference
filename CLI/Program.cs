using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CLI
{
	/// <summary>
	/// Computer\HKEY_CURRENT_USER\Software\Microsoft\DirectX\UserGpuPreferences
	/// </summary>
	class Constants
	{
		public const string Default = "GpuPreference=0;";
		public const string PowerSaving = "GpuPreference=1;";
		public const string MaxPerformance = "GpuPreference=2;";
	}

	class Program
	{
		static void Main(string[] args)
		{
			var length = args.Count();
			for (int i = 0; i < length; ++i)
			{
				var arg = args[0];
				switch (arg)
				{
					case "help":
						PrintDetailedHelp();
						break;
					case "list":
						ListPreferences();
						break;
					default:
						PrintUnknownCommand(arg);
						break;
				}
			}
		}

		/// <summary>
		/// Unknown command
		/// </summary>
		/// <param name="c">Unknown used</param>
		private static void PrintUnknownCommand(string c)
		{
			Console.WriteLine("Unknown command '{0}'", c);
			Console.WriteLine("Use '" + AppDomain.CurrentDomain.FriendlyName + " help' for a detailed list of all the commands ");
		}

		private static void PrintDetailedHelp()
		{
			StringBuilder builder = new StringBuilder();

			builder.AppendLine("Usage: " + AppDomain.CurrentDomain.FriendlyName + " <command> [<params>]");
			builder.AppendLine("");
			builder.AppendLine("Available commands:");
			builder.AppendLine("add: \t\t Add an app to the list");
			builder.AppendLine("help: \t\t Print this list");
			builder.AppendLine("list: \t\t List the preferences");
			builder.AppendLine("remove: \t Add an app to the list");

			Console.WriteLine(builder.ToString());
		}

		private static void ListPreferences()
		{
			var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\DirectX\UserGpuPreferences", true);
			foreach (var item in key.GetValueNames())
			{
				Console.WriteLine(item + " = >" + GpuLabel(key.GetValue(item) as string));
			}
		}

		private static string GpuLabel(string s)
		{
			switch (s)
			{
				case Constants.Default:
					return "Default";
				case Constants.PowerSaving:
					return "Power saving";
				case Constants.MaxPerformance:
					return "Max performance";
				default:
					return "Unknown value";
			}
		}

	}
}
