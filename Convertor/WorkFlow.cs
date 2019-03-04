using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fetcher.Model;
using System.Xml;
using System.Xml.Serialization;

namespace Fetcher
{
	public class WorkFlow
	{
		//private int LogBufferSize = 4096;
		private string exePath = new FileInfo(Assembly.GetExecutingAssembly().GetName().Name).DirectoryName;

		//private static StreamWriter LogStream = new StreamWriter(Path.Combine(exePath, "log.log"),
		//	false, Encoding.UTF8, LogBufferSize);

		public Arguments EssentialParameters { get; set; } = new Arguments();

		public List<Parameter> Parameters { get; set; } = new List<Parameter>();

		public List<DetailFileInfo> StrategyFileList { get; set; } = new List<DetailFileInfo>();

		public List<DetailFileInfo> WaitingFileList { get; set; } = new List<DetailFileInfo>();

		public List<DetailFileInfo> GetWaitingFiles(Parameter input)
		{
			List<DetailFileInfo> files = new List<DetailFileInfo>();

			if (input.Key == Constants.ArgumentConstants.Input && input?.ValidateResult.IsValid == true)
			{
				// input is a file
				if ((input as UriParameter).UriType == UriType.File)
				{
					FileInfo file = new FileInfo(input.Value);
					files.Add(new DetailFileInfo(file));
				}

				// input is directory
				if ((input as UriParameter).UriType == UriType.Directory)
				{
					DirectoryInfo dir = new DirectoryInfo(input.Value);
					FileInfo[] allFiles = dir.GetFiles("*.*", SearchOption.AllDirectories);
					foreach (FileInfo file in allFiles)
					{
						files.Add(new DetailFileInfo(file));
					}
				}
			}

			return files;
		}

		public List<FileInfo> GetStrategyFiles(Parameter strategy)
		{
			List<FileInfo> infos = new List<FileInfo>();

			if (strategy.Key == Constants.ArgumentConstants.Strategy && strategy?.ValidateResult.IsValid == true)
			{
				// is a file
				if ((strategy as UriParameter).UriType == UriType.File)
				{
					FileInfo info = new FileInfo(strategy.Value);
					infos.Add(info);
				}

				// is directory
				if ((strategy as UriParameter).UriType == UriType.Directory)
				{
					DirectoryInfo dir = new DirectoryInfo(strategy.Value);
					FileInfo[] allFiles = dir.GetFiles("*.*", SearchOption.AllDirectories);
					infos = allFiles.ToList();
				}
			}

			return infos;
		}

		public List<Parameter> GetParameters(string[] args)
		{
			List<Parameter> parameters = new List<Parameter>();
			StringBuilder sb = new StringBuilder();
			foreach (string str in args)
			{
				sb.AppendFormat($" {str}");
			}
			string tempStr = string.Empty;
			string[] clauses = sb.ToString().Split('/', '\\');
			foreach (string clause in clauses)
			{
				try
				{
					string[] strs = clause.Split(" ".ToArray(), 2);
					switch (strs[0])
					{
						case Constants.ArgumentConstants.Input:
							UriParameter input = new UriParameter()
							{
								Key = Constants.ArgumentConstants.Input,
								Value = strs[1],
								IsActive = true,
								Name = Constants.Input
							};
							parameters.Add(input);
							this.EssentialParameters.Input = input;
							break;

						case Constants.ArgumentConstants.Output:
							UriParameter output = new UriParameter()
							{
								Key = Constants.ArgumentConstants.Output,
								Value = strs[1],
								IsActive = true,
								Name = Constants.Output
							};
							parameters.Add(output);
							this.EssentialParameters.Output = output;
							break;

						case Constants.ArgumentConstants.Strategy:
							UriParameter strategy = new UriParameter()
							{
								Key = Constants.ArgumentConstants.Strategy,
								Value = strs[1],
								IsActive = true,
								Name = Constants.Strategy
							};
							parameters.Add(strategy);
							this.EssentialParameters.Strategy = strategy;
							break;

						case Constants.ArgumentConstants.Log:
							UriParameter log = new UriParameter()
							{
								Key = Constants.ArgumentConstants.Log,
								Value = strs[1],
								IsActive = true,
								Name = Constants.Log
							};
							parameters.Add(log);
							this.EssentialParameters.Log = log;
							break;

						// function parameter
						case Constants.ArgumentConstants.Version:
							parameters.Add(new Parameter()
							{
								Key = Constants.ArgumentConstants.Version,
								Value = strs[1]
							});
							break;

						case Constants.ArgumentConstants._Version:
							parameters.Add(new Parameter()
							{
								Key = Constants.ArgumentConstants._Version,
								Value = strs[1]
							});
							break;

						default:
							break;
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
					Environment.Exit(1);
				}
			}

			return parameters;
		}

		public void Start(string[] args)
		{
			////AppDomain.CurrentDomain.ProcessExit += (s, e) => LogStream.Close();

			//List<Parameter> parameters = GetParameters(args);

			//// file list.
			//WaitingFileList = new List<FileInfo>();
			//if (File.Exists(this.EssentialParameters.Input.Value))
			//{
			//	WaitingFileList.Add(new FileInfo(this.EssentialParameters.Input.Value));
			//}
			//else if (Directory.Exists(this.EssentialParameters.Input.Value))
			//{
			//	// recurse
			//	WaitingFileList = new DirectoryInfo(this.EssentialParameters.Input.Value).GetFiles("*.*", SearchOption.AllDirectories).ToList();
			//	Console.WriteLine("input path exist.");
			//}

			//// strategies file list.
			//StrategyFileList = new List<FileInfo>();
			//if (File.Exists(this.EssentialParameters.Strategy.Value))
			//{
			//	StrategyFileList.Add(new FileInfo(this.EssentialParameters.Strategy.Value));
			//	Console.WriteLine("ini file exist.");
			//}
			//else if (Directory.Exists(this.EssentialParameters.Strategy.Value))
			//{
			//	StrategyFileList = new DirectoryInfo(this.EssentialParameters.Strategy.Value).GetFiles("*.ini*", SearchOption.TopDirectoryOnly).ToList();
			//	Console.WriteLine("ini path exist.");
			//}

			//foreach (FileInfo fi in StrategyFileList)
			//{
			//	IniReader reader = new IniReader(fi.FullName);
			//	if (reader.GetValue("OperationType") == "Extract")
			//	{
			//		//_operationType = OperationType.Extract;
			//		//Console.WriteLine(_operationType.ToString());
			//	}
			//}
			//SetActions();
			//StartProcessing();

			//File.WriteAllText(logPath, logStr.ToString(), Encoding.UTF8);
		}

		public ValidationResults ValidateEssentialParameters()
		{
			ValidationResults results = new ValidationResults();

			foreach (UriParameter p in this.EssentialParameters.Parameters)
			{
				ValidationResult r = GetValidationResult(p);
				results.Results.Add(r);
				p.ValidateResult = r;
			}

			return results;
		}

		private ValidationResult GetValidationResult(UriParameter p)
		{
			ValidationResult result = new ValidationResult();

			try
			{
				if (!File.Exists(p.Value) && !Directory.Exists(p.Value))
				{
					throw new Exception("File or directory not exist.");
				}
				p.UriType = File.GetAttributes(p.Value).HasFlag(FileAttributes.Directory) ? UriType.Directory : UriType.File;
				p.URI = new Uri(p.Value);
				result.IsValid = true;
			}
			catch (Exception ex)
			{
				result.IsValid = false;
				result.Message = ex.Message;
			}

			return result;
		}

		public bool CreateEssenticalFolder()
		{
			bool isSuccess = false;
			try
			{
				List<UriParameter> paras = new List<UriParameter>()
				{
					this.EssentialParameters.Output,
					this.EssentialParameters.Log
				};

				foreach (UriParameter p in paras)
				{
					string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
					string inputRelativePath = Path.GetFileName(this.EssentialParameters.Input.URI.LocalPath);
					if (!p.ValidateResult.IsValid)
					{
						DirectoryInfo dirInfo = Directory.CreateDirectory(p.Value);
						// try to create folder at expected location
						if (dirInfo.Exists)
						{
							p.ValidateResult.IsValid = true;
							p.URI = new Uri(dirInfo.FullName);
						}

						// create folder at default location
						else
						{
							string path = Path.Combine(docFolder, inputRelativePath, p.Name);
							p.URI = new Uri(path);
							if (Directory.CreateDirectory(path).Exists)
							{
								p.ValidateResult.IsRevised = true;
								p.ValidateResult.IsValid = true;
							}
							else
							{
								throw new Exception("Can not create folders.");
							}
						}
					}
				}

				isSuccess = true;
			}
			catch (Exception ex)
			{
				KnownException exception = new KnownException()
				{
					Exception = ex,
					Type = ExceptionType.Error_Creating_Folder
				};
				ExceptionHandler.HandleExceptions(exception);
			}

			return isSuccess;
		}

		public List<Strategy> GetStrategies()
		{
			List<Strategy> strategies = new List<Strategy>();
			foreach (DetailFileInfo file in this.StrategyFileList)
			{
				string readContents;
				using (StreamReader streamReader = new StreamReader(file.FileInfo.FullName, file.Encoding))
				{
					readContents = streamReader.ReadToEnd();
				}
			}

			return strategies;
		}

		//static Action SentenceParser(string sentence)
		//{
		//	Action action = new Action();
		//	string lowerSentence = sentence.ToLower();
		//	if (lowerSentence.Contains("file name") || lowerSentence.Contains("name")
		//		|| lowerSentence.Contains("file_name"))
		//		action.TargetType = Action.ObjectType.FileName;
		//	if (lowerSentence.Contains("file content") || lowerSentence.Contains("content")
		//		|| lowerSentence.Contains("file_content"))
		//		action.TargetType = Action.ObjectType.FileContent;

		//	string pattern = "\"[^\\s]*\"";
		//	MatchCollection matches = Regex.Matches(sentence, pattern);
		//	if (matches.Count == 2)
		//	{
		//		action.Target = matches[0].Value;
		//		action.Desired = matches[1].Value;
		//		return action;
		//	}
		//	else if (!string.IsNullOrWhiteSpace(sentence))
		//	{
		//		Console.WriteLine("Can't parse \" " + sentence + " \"");
		//	}
		//	return null;
		//}

		//static void SimpleSentenceProcessing(IniReader reader)
		//{
		//	foreach (FileInfo fi in iniFileList)
		//	{
		//		reader.Path = fi.FullName;
		//		if (reader.GetValue("MakeItSimple") == "True")
		//		{
		//			List<string> sentences = File.ReadLines(fi.FullName).ToList();
		//			foreach (string sentence in sentences)
		//			{
		//				if (!(sentence.StartsWith(";") || string.IsNullOrWhiteSpace(sentence)))
		//				{
		//					Action action = SentenceParser(sentence);
		//					if (action != null)
		//					{
		//						if (action.TargetType == Action.ObjectType.FileName)
		//							ActionsOnName.Add(action);
		//						else if (action.TargetType == Action.ObjectType.FileContent)
		//							ActionsOnContent.Add(action);
		//					}
		//				}
		//			}
		//		}
		//	}
		//}

		//private static void StartProcessing()
		//{
		//	foreach (FileInfo fi in fileList)
		//	{
		//		string nameAfter = fi.Name;
		//		string newFileOperationLog = string.Empty;
		//		newFileOperationLog += "\r\n******************";
		//		newFileOperationLog += "CHANGE TO: " + fi.Name;
		//		newFileOperationLog += "******************\r\n";
		//		WriteToLog(newFileOperationLog);
		//		foreach (Action action in ActionsOnName)
		//		{
		//			nameAfter = action.BeginReplacing(nameAfter);
		//			WriteToLog(action.Log);
		//		}

		//		// relative to the input path.
		//		string relativePath = fi.FullName.Substring(Arguments.Input.Value.Length, fi.FullName.Length - Arguments.Input.Value.Length);
		//		if (File.Exists(Arguments.Input.Value))
		//		{
		//			relativePath = "/" + fi.Name;
		//		}

		//		string fullOutPutDirectoryPath = Arguments.Output.Value + relativePath;

		//		string fullOutPutFileNameChangedPath = fullOutPutDirectoryPath.Substring(0, fullOutPutDirectoryPath.Length - fi.Name.Length) + nameAfter;

		//		string dir = Path.GetDirectoryName(fullOutPutFileNameChangedPath);
		//		if (!Directory.Exists(dir))
		//		{
		//			Directory.CreateDirectory(dir);
		//		}

		//		if (TextFileType.Contains(fi.Extension))
		//		{
		//			string contentAfter = string.Empty;
		//			if (_operationType == OperationType.Replace)
		//			{
		//				contentAfter = @File.ReadAllText(fi.FullName);
		//				foreach (Action action in ActionsOnContent)
		//				{
		//					contentAfter = action.BeginReplacing(contentAfter);
		//					WriteToLog(action.Log);
		//				}
		//			}
		//			else if (_operationType == OperationType.Extract)
		//			{
		//				string contentBefore = @File.ReadAllText(fi.FullName);
		//				foreach (Action action in ActionsOnContent)
		//				{
		//					contentAfter += action.BeginExtracting(contentBefore);
		//					WriteToLog(action.Log);
		//				}
		//			}
		//			//WriteToLog(action.Log);

		//			File.WriteAllText(fullOutPutFileNameChangedPath, contentAfter, Encoding.UTF8);
		//		}
		//		else if (BinaryFileType.Contains(fi.Extension))
		//		{
		//			File.Copy(fi.FullName, fullOutPutFileNameChangedPath, true);
		//		}
		//	}
		//}

		//private static void SetActions()
		//{
		//	IniReader reader = new IniReader();
		//	foreach (FileInfo fi in iniFileList)
		//	{
		//		reader.Path = fi.FullName;
		//		if (reader.GetValue("MakeItSimple") == "True")
		//		{
		//			SimpleSentenceProcessing(reader);
		//		}
		//		else
		//		{
		//			string[] sections = reader.GetSections();
		//			foreach (string section in sections)
		//			{
		//				Action action = new Action(section);
		//				if (reader.GetValue("Type", section) != null)
		//				{
		//					action.TargetType = reader.GetValue("Type", section).ToLower().Contains("name") ? Action.ObjectType.FileName : Action.ObjectType.FileContent;
		//				}
		//				action.Target = reader.GetValue("Target", section);
		//				action.Desired = reader.GetValue("Desired", section);
		//				if (action.TargetType == Action.ObjectType.FileName)
		//				{
		//					ActionsOnName.Add(action);
		//				}
		//				else if (action.TargetType == Action.ObjectType.FileContent)
		//				{
		//					ActionsOnContent.Add(action);
		//				}
		//			}
		//		}

		//	}
		//}

		//static void WriteToLog(string log)
		//{
		//	Console.Write(log);
		//}
	}
}