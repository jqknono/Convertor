using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fetcher
{
	public class Constants
	{
		public static string TestFileForSave { get; set; } =
			@"D:\Cecil's Projects\Automation4ID\Automation4ID\Convertor\testTxt.txt";

		public static class ArgumentConstants
		{
			public const string Input = "i";
			public const string Output = "o";
			public const string Strategy = "s";
			public const string Log = "l";

			public const string Version = "version";
			public const string _Version = "v";
		}

		public const string Input = "Input";
		public const string Output = "Output";
		public const string Log = "Log";
		public const string Strategy = "Strategy";
	}
}
