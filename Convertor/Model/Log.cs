using Fetcher.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fetcher.Model
{
	public class Logger
	{
		public List<Log> Logs { get; set; } = new List<Log>();

		public static void Log(string message, Exception ex)
		{
			Log log = new Log()
			{
				Message = message,
				AdditionalInformation = ex.Message
			};
		}

		public static void Log(Exception ex)
		{
			Log log = new Log()
			{
				AdditionalInformation = ex.Message
			};
		}
	}

	public class Log
	{
		public string Message { get; set; }
		public DateTime DateTime { get; set; } = DateTime.Now;
		public LogInfoType LogType { get; set; } = LogInfoType.Information;
		public object AdditionalInformation { get; set; }
		public string StackTrace { get; set; }

		public Log() { }

		public Log(string message)
		{
			this.Message = message;
		}

		public Log(string message, Exception ex)
		{
			this.Message = message;
			this.AdditionalInformation = ex.Message;
			this.StackTrace = ex.StackTrace;
		}

		public Log(string message, Exception ex, LogInfoType logType)
		{
			this.Message = message;
			this.AdditionalInformation = ex.Message;
			this.LogType = logType;
			this.StackTrace = ex.StackTrace;
		}

		public Log(KnownException ex)
		{
			this.Message = ex.CustomMessage;
			this.AdditionalInformation = ex;
			this.LogType = LogInfoType.Exception;
			this.StackTrace = ex.Exception.StackTrace;
		}
	}
}
