using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Automation4ID
{
    public class Task
    {
        static int argc = 0;
        static string input = string.Empty;
        static string output = string.Empty;
        static string iniPath = string.Empty;
        static string logPath = string.Empty;
        static StringBuilder logStr = new StringBuilder();
        static string exePath = new FileInfo(Assembly.GetExecutingAssembly().GetName().Name).DirectoryName;
        static List<FileInfo> fileList;
        static List<FileInfo> iniFileList;
        static List<Action> ActionsOnName;
        static List<Action> ActionsOnContent;
    }


}
