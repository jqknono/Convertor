using Fetcher.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fetcher
{
	public static class ExceptionHandler
	{
		public static Log HandleExceptions(KnownException ex)
		{
			return new Log(ex);
		}
	}
}
