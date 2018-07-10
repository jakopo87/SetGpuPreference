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
					case "add":
						if (args.Count() == 3)
						{
							AddPreference(args[1], args[2]);
						}
						else
						{
							PrintAddUsage();
						}
						break;
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

		private static void PrintAddUsage()
		{
			StringBuilder builder = new StringBuilder();

			builder.AppendLine();
			builder.AppendLine("Set the GPU preference for an app");
			builder.AppendLine();
			builder.AppendLine("Usage:");
			builder.AppendLine(AppDomain.CurrentDomain.FriendlyName + " add <full-path> <preference>");
			builder.AppendLine();
			builder.AppendLine("Params:");
			builder.AppendLine("full-path: \t\t Absolute path to the executable app");
			builder.AppendLine("preference: \t\t 0 = Default, 1 = Power saving, 2 = Max performance");

			Console.WriteLine(builder.ToString());
		}

		private static void AddPreference(string v1, string v2)
		{

			throw new NotImplementedException();
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

		/// <summary>
		/// Print the help guide
		/// </summary>
		private static void PrintDetailedHelp()
		{
			StringBuilder builder = new StringBuilder();

			builder.AppendLine("Usage: " + AppDomain.CurrentDomain.FriendlyName + " <command> [<params>]");
			builder.AppendLine();
			builder.AppendLine("Available commands:");
			builder.AppendLine("add: \t\t Add an app to the list");
			builder.AppendLine("help: \t\t Print this list");
			builder.AppendLine("list: \t\t List the current GPU preferences");
			builder.AppendLine("remove: \t Add an app to the list");

			Console.WriteLine(builder.ToString());
		}

		/// <summary>
		/// List the current GPU preferences
		/// </summary>
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
