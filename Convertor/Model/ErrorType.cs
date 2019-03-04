using Fetcher.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fetcher.Model
{
	public class KnownException
	{
		public ExceptionType Type;

		[XmlIgnore]
		public Exception Exception { get; set; }

		public string ExceptionMessage
		{
			get
			{
				return this.Exception.Message;
			}
		}

		public string CustomMessage { get; set; }
	}
}