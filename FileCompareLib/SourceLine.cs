using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdamOneilSoftware.CompareLib
{
	public class SourceLine
	{
		public string Text { get; set; }
		public int Number { get; set; }
		internal int Index { get; set; } // used for joining like lines during merges

		public string HtmlText(string className = null)
		{
			if (!string.IsNullOrEmpty(className)) className = " " + className;

			int indentCount = Text.Count(c => c.Equals('\t'));
			string text = Text;
			text = text.Replace("<", "&lt;");
			text = text.Replace(">", "&gt;");

			string result = $"<div>";
				result += $"<span class=\"line linenum\">{Number}:</span>";
				result += $"<span class=\"line indent{indentCount}{className}\">{text}</span>";
			result += "</div>";
			return result;		
		}

		public bool IsWhitespace
		{
			get
			{
				if (string.IsNullOrEmpty(Text)) return true;
				return (Text.ToCharArray().All(c => char.IsWhiteSpace(c)));
			}
		}

		public override string ToString()
		{
			return $"{Number}: {Text}";
		}

		public override int GetHashCode()
		{
			return (Text != null) ? Text.GetHashCode() : 0;
		}

		public override bool Equals(object obj)
		{
			SourceLine test = obj as SourceLine;
			if (test != null && test.Text != null && this.Text != null)
			{
				return test.Text.Equals(this.Text);
			}
			return false;
		}
	}
}
