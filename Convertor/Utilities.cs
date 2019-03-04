using Fetcher.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Fetcher
{
	public static class Utilities
	{
		/// <summary>
		/// Determines a text file's encoding by analyzing its byte order mark (BOM).
		/// Defaults to ANSI when detection of the text file's endianness fails.
		/// </summary>
		/// <param name="filename">The text file to analyze.</param>
		/// <returns>The detected encoding.</returns>
		public static Encoding GetEncoding(string filename)
		{
			// Read the BOM
			var bom = new byte[4];
			using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
			{
				file.Read(bom, 0, 4);
			}

			// Analyze the BOM
			if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
			if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
			if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
			if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
			if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return Encoding.UTF32;

			return Encoding.Default;
		}

		public static string TestModelIsSerializable(object obj)
		{
			string str = string.Empty;
			try
			{
				XmlSerializer serializer = new XmlSerializer(obj.GetType());
				using (StringWriter textWriter = new StringWriter())
				{
					serializer.Serialize(textWriter, obj);
					Console.Write(textWriter.ToString());
					str = textWriter.GetStringBuilder().ToString();
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex);
			}

			return str;
		}

		public static object TestModelIsDeSerializable(string xmlStr, Type type)
		{
			Object obj = null;
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(xmlStr);
			try
			{
				XmlSerializer serializer = new XmlSerializer(type);
				using (XmlReader reader = new XmlNodeReader(doc))
				{
					obj = serializer.Deserialize(reader);
				}
			}
			catch (Exception ex)
			{
				Logger.Log(ex);
			}

			return obj;
		}

		public static List<string> IsClassSerializable()
		{
			List<string> strs = new List<string>
			{
				typeof(Parameter).ToString() + ": " + typeof(Parameter).IsSerializable.ToString(),
				typeof(UriParameter).ToString() + ": " + typeof(UriParameter).IsSerializable.ToString(),
				typeof(ToggleParammeter).ToString() + ": " + typeof(ToggleParammeter).IsSerializable.ToString(),
				typeof(Arguments).ToString() + ": " + typeof(Arguments).IsSerializable.ToString(),
				typeof(ValidationResults).ToString() + ": " + typeof(ValidationResults).IsSerializable.ToString()
			};
			return strs;
		}
	}
}