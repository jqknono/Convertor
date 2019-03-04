using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fetcher.Model
{
	public class Action
	{
		public string SourcePattern { get; set; } = string.Empty;

		public string ResultPattern { get; set; } = string.Empty;

		public string SourceType { get; set; } = string.Empty;
	}
}
