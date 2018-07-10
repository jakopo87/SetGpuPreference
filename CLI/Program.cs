using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CLI
{
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
			foreach (var arg in args)
			{
				switch (arg)
				{
					case "-h":
					case "--help":
						PrintDetailedHelp();
						break;
					case "-l":
					case "--list":
						ListPreferences();
						break;
					default:
						PrintUsage();
						break;
				}
			}


		}

		private static void PrintUsage()
		{
			throw new NotImplementedException();
		}

		private static void PrintDetailedHelp()
		{
			throw new NotImplementedException();
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
