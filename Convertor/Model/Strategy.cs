using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fetcher.Model
{
	public class Strategy
	{
		public string Name { get; set; }
		public List<FileType> TargetFileTypes { get; set; } = new List<FileType>();
		public List<Action> Actions { get; set; } = new List<Action>();
		public OperationType OperationType { get; set; }
	}
}
