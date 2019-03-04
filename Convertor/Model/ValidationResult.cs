using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fetcher.Model
{
	public class ValidationResult
	{
		public bool IsValid { get; set; } = false;

		public string Message { get; set; }

		/// <summary>
		/// if the value is invalid, we will try to fix it.
		/// Then IsRevised should be to true.
		/// </summary>
		public bool IsRevised { get; set; } = false;
	}

	public class ValidationResults
	{
		public List<ValidationResult> Results { get; set; } = new List<ValidationResult>();
	}
}
