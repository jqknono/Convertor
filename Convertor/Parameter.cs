using Fetcher.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fetcher.Model
{
	public class Arguments
	{
		public UriParameter Input { get; set; }
		public UriParameter Output { get; set; }
		public UriParameter Strategy { get; set; }
		public UriParameter Log { get; set; }

		[XmlIgnore]
		public List<UriParameter> Parameters
		{
			get
			{
				return new List<UriParameter>() { Input, Output, Strategy, Log };
			}
		}

		[XmlIgnore]
		public bool IsAllEssentialParametersValid
		{
			get
			{
				bool isValid = true;
				foreach (Parameter p in this.Parameters)
				{
					if (!p.ValidateResult.IsValid)
					{
						isValid = false;
					}
				}

				return isValid;
			}
		}
	}

	public class Parameter
	{
		[XmlAttribute]
		public string Key { get; set; }

		public string Value { get; set; }

		public bool IsActive { get; set; } = false;

		public string Name { get; set; }

		public ValidationResult ValidateResult { get; set; }
	}
	
	public class UriParameter : Parameter
	{
		public UriType UriType { get; set; } = UriType.Default;
		[XmlIgnore]
		public Uri URI { get; set; }
	}

	class ToggleParammeter : Parameter
	{
		public ToggleState ToggleState { get; set; } = ToggleState.OFF;
	}

}
