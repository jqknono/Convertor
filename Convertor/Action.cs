using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Fetcher
{
    public class Action
    {
        public enum ObjectType { FileName, FileContent };

        public ObjectType TargetType = ObjectType.FileContent;

        private string _target = string.Empty;
        public string Target
        {
            get { return _target; }
            set
            {
                if (value.StartsWith("\""))
                {
                    _target = value.Substring(1, value.Length - 2);
                }
                else
                {
                    _target = ToUnEscapedCharacter(value);
                }
            }
        }
        private string _desired = string.Empty;
        public string Desired
        {
            get
            {
                return _desired;
            }
            set
            {
                if (value.StartsWith("\""))
                {
                    _desired = value.Substring(1, value.Length - 2);
                }
                else
                {
                    _desired = ToUnEscapedCharacter(value);
                }
            }
        }

        public string ActionName { get; set; }
        private StringBuilder _log = new StringBuilder();
        public string Log { get { return _log.ToString(); } }

        public Action()
        {

        }

        public Action(string name)
        {
            this.ActionName = name;
        }

        public string BeginExtracting(string content)
        {
            this._log.Clear();
            StringBuilder afterContent = new StringBuilder();
            if (_target != "")
            {
                MatchCollection matches = Regex.Matches(@content, @_target);
                string beforeSentence = string.Empty;
                string afterSentence = string.Empty;
                foreach (Match match in matches)
                {
                    beforeSentence = match.Value;
                    afterSentence = Regex.Replace(match.Value, _target, _desired);
                    afterContent.AppendLine(afterSentence);
                    SaveToLog("\"" + beforeSentence + "\"", "\"" + afterSentence + "\"");
                }
            }

            return afterContent.ToString();
        }

        public string BeginReplacing(string content)
        {
            this._log.Clear();
            string afterContent = content;
            if (_target != "")
            {
                while (Regex.IsMatch(@afterContent, @_target))
                {
                    MatchCollection matches = Regex.Matches(@afterContent, _target);
                    string beforeSentence;
                    string afterSentence;
                    afterContent = Regex.Replace(@afterContent, _target, _desired);
                    foreach (Match match in matches)
                    {
                        beforeSentence = match.Value;
                        afterSentence = Regex.Replace(match.Value, _target, _desired);
                        SaveToLog("\"" + beforeSentence + "\"", "\"" + afterSentence + "\"");
                    }
                }
            }

            return afterContent;
        }

        public void SaveToLog(string before, string after)
        {
            string singleLog = string.Empty;
            if (before != after)
            {
                switch (TargetType)
                {
                    case ObjectType.FileName:
                        singleLog = "\r\nFile NAME CHANGE: " + "\r\n\t" + before + " => " + after + "\r\n";
                        break;
                    case ObjectType.FileContent:
                        singleLog = "\r\nFile CONTENT CHANGE: " + "\r\n\t" + before + " => " + after + "\r\n";
                        break;
                    default:
                        break;
                }
            }
            this._log.Append(singleLog);
        }

        public string ToUnEscapedCharacter(string plainString)
        {
            string unEscapeString = string.Empty;
            switch (plainString)
            {
                case "\\f":
                    unEscapeString = "\f";
                    break;
                case "\\n":
                    unEscapeString = "\n";
                    break;
                case "\\r":
                    unEscapeString = "\r";
                    break;
                case "\\t":
                    unEscapeString = "\t";
                    break;
                case "\\v":
                    unEscapeString = "\v";
                    break;
                case "\\r\\n":
                    unEscapeString = "\r\n";
                    break;
            }
            return unEscapeString;
        }
    }
}
