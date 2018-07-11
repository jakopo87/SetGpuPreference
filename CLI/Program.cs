﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CLI
{
	class Program
	{
		private const string DEFAULT_GPU = "0";
		private const string POWER_SAVE_GPU = "1";
		private const string MAX_PERF_GPU = "2";
		private const string registryValue = "GpuPreference={0};";


		private Dictionary<string, string> GpuRegistyValues = new Dictionary<string, string>()
		{
			{DEFAULT_GPU,String.Format(registryValue,DEFAULT_GPU) },
			{POWER_SAVE_GPU,String.Format(registryValue,POWER_SAVE_GPU) },
			{MAX_PERF_GPU,String.Format(registryValue,MAX_PERF_GPU) }
		};

		private static string registryPath = @"Software\Microsoft\DirectX\UserGpuPreferences";

		static void Main(string[] args)
		{
			var length = args.Count();
			if (length == 0)
			{
				PrintDetailedHelp();
			}
			else
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
						return;
					case "delete":
						if (args.Count() == 2)
						{
							RemovePreference(args[1]);
						}
						else
						{
							PrintDeleteUsage();
						}
						return;
					case "help":
						PrintDetailedHelp();
						return;
					case "list":
						ListPreferences();
						return;
					default:
						PrintUnknownCommand(arg);
						return;
				}
			}

		}

		private static void PrintDeleteUsage()
		{
			StringBuilder builder = new StringBuilder();

			builder.AppendLine();
			builder.AppendLine("Delete the GPU preference for an app");
			builder.AppendLine();
			builder.AppendLine("Usage:");
			builder.AppendLine(AppDomain.CurrentDomain.FriendlyName + " delete <path>");
			builder.AppendLine();
			builder.AppendLine("Params:");
			builder.AppendLine("path: \t\t Path to the executable app");

			Console.WriteLine(builder.ToString());
		}

		private static void PrintAddUsage()
		{
			StringBuilder builder = new StringBuilder();

			builder.AppendLine();
			builder.AppendLine("Set the GPU preference for an app");
			builder.AppendLine();
			builder.AppendLine("Usage:");
			builder.AppendLine(AppDomain.CurrentDomain.FriendlyName + " add <path> <preference>");
			builder.AppendLine();
			builder.AppendLine("Params:");
			builder.AppendLine("path: \t\t Path to the executable app");
			builder.AppendLine("preference: \t\t 0 = Default, 1 = Power saving, 2 = Max performance");

			Console.WriteLine(builder.ToString());
		}

		private static void AddPreference(string path, string pref)
		{
			if (File.Exists(path))
			{
				var fullPath = Path.GetFullPath(path);

				var key = Registry.CurrentUser.OpenSubKey(registryPath, true);
				key.SetValue(fullPath, String.Format(registryValue, pref), RegistryValueKind.String);

				Console.WriteLine("Set GPU preference for {0} to {1}", fullPath, GpuLabel(pref));
			}
			else
			{
				Console.WriteLine("{0} does not exists", path);
			}
		}

		private static void RemovePreference(string path)
		{
			if (File.Exists(path))
			{
				var fullPath = Path.GetFullPath(path);

				var key = Registry.CurrentUser.OpenSubKey(registryPath, true);

				key.DeleteValue(fullPath);

				Console.WriteLine("Deleted GPU preference for {0}", fullPath);
			}
			else
			{
				Console.WriteLine("{0} does not exists", path);
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

		/// <summary>
		/// Print the help guide
		/// </summary>
		private static void PrintDetailedHelp()
		{
			StringBuilder builder = new StringBuilder();

			builder.AppendLine();
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
			var key = Registry.CurrentUser.OpenSubKey(registryPath, true);
			foreach (var item in key.GetValueNames())
			{
				var s = key.GetValue(item) as string;
				Console.WriteLine(item + " = >" + GpuLabel(s.Substring(14, 1)));
			}
		}

		private static string GpuLabel(string s)
		{
			switch (s)
			{
				case DEFAULT_GPU:
					return "Default GPU";
				case POWER_SAVE_GPU:
					return "Power saving GPU";
				case MAX_PERF_GPU:
					return "Max performance GPU";
				default:
					return "Unknown value";
			}
		}

	}
}
