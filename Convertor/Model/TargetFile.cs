using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fetcher.Model
{
	public class DetailFileInfo
	{
		public FileType FileType { get; set; }
		public string Name { get; set; }

		[XmlAttribute("Encoding")]
		public string EncodingName { get; set; }

		[XmlIgnore]
		public FileInfo FileInfo { get; set; }

		[XmlIgnore]
		public Encoding Encoding
		{
			get
			{
				Encoding e;
				try
				{
					e = Encoding.GetEncoding(this.EncodingName);
				}
				catch
				{
					e = Encoding.Default;
				}

				return e;
			}
		}

		public DetailFileInfo() { }

		public DetailFileInfo(FileInfo file)
		{
			this.FileInfo = file;
		}
	}
}
