using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fetcher.Model
{
	public class Project
	{
		public string TargetFolderPath { get; set; }

		public List<Strategy> Strategies { get; set; }

		public string OutputFolderOath { get; set; }

		public string LogFilePath { get; set; }
	}
}
