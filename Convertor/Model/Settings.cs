using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fetcher.Model
{
	public class Settings
	{
		private static Settings _instance = null;
		private Settings() { }
		public Settings GetInstance()
		{
			return _instance ?? (_instance = new Settings());
		}

		[XmlAttribute("Encoding")]
		public string DefaultSaveEncodingName { get; set; } = Encoding.UTF8.EncodingName;
	}
}
