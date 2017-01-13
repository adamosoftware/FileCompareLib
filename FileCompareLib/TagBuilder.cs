using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdamOneilSoftware.CompareLib
{
	public enum TagCloseType
	{
		Normal,
		SelfClosing
	}

	public class TagBuilder
	{
		private readonly string _name = null;
		private AttributeBuilder _attributes = null;		

		public TagBuilder(string name)
		{
			_name = name;
			_attributes = new AttributeBuilder();
		}

		public TagBuilder(string name, object attributes)
		{
			_name = name;
			_attributes = new AttributeBuilder(attributes);
		}

		public TagBuilder(string name, string innerContent, object attributes = null)
		{
			_name = name;
			InnerContent = innerContent;
			_attributes = new AttributeBuilder(attributes);
		}

		public string InnerContent { get; set; }

		public Dictionary<string, string> Attributes { get { return _attributes; } }

		public override string ToString()
		{
			string result = string.Empty;

			if (!
				string.IsNullOrEmpty(InnerContent))
			{
				result = $"<{_name}{_attributes}>{InnerContent}</{_name}>";
			}
			else
			{
				result = $"<{_name}{_attributes}/>";
			}
			
			return result;
		}

		private string TagContent()
		{
			throw new NotImplementedException();
		}
	}

	internal class AttributeBuilder : Dictionary<string, string>
	{
		public AttributeBuilder()
		{
		}

		public AttributeBuilder(object attributes)
		{
			if (attributes == null) return;

			PropertyInfo[] props = attributes.GetType().GetProperties();
			foreach (var pi in props)
			{
				object value = pi.GetValue(attributes);
				if (value != null)
				{
					Add(FormatAttrName(pi.Name), value.ToString());
				}				
			}
		}

		public override string ToString()
		{
			if (this.Count() > 0)
			{
				return " " + string.Join(" ", this.Select(keyPair => $"{FormatAttrName(keyPair.Key)}=\"{keyPair.Value}\""));
			}
			return string.Empty;
		}

		private string FormatAttrName(string name)
		{
			string result = name.ToLower();

			result = result.Replace('_', '-');

			return result;
		}
	}
}
