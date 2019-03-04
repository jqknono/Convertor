using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fetcher.Model
{
	public enum TargetType { FileName, FileContent, Other };

	public enum OperationType { Replace, Extract };

	public enum LogInfoType { Information, Exception, Risk, Error };

	public enum StartupParameterCollection { Input, Output, Configuration, Log };

	public enum StartupParameterType { UriParameter, ToggleParameter };

	public enum ToggleState { ON, OFF };

	public enum UriType
	{
		Default,
		File,
		Directory
	}

	public enum ExceptionType
	{
		InputArgumentInvaild,
		OutputArgumentInvalid,
		StrategyArgumentInvalid,
		LogArgumentInvalid,
		Error_Creating_Folder,
	}

}
