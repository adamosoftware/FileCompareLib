using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdamOneilSoftware.CompareLib
{
	public enum MatchSide
	{
		Center,
		Left,
		Right
	}

	public class StringCommonRange
	{
		public string Text { get; set; }
		public CharSpan Edited { get; set; }
		public CharSpan Target { get; set; }
		public MatchSide Side { get; set; }
		public bool IsValid { get { return !string.IsNullOrEmpty(Text); } }		
		public int Order { get; set; }
		public int Length { get { return Text.Length; } }		

		public class CharSpan
		{
			public int Start { get; set; }
			public int End { get; set; }			

			public override string ToString()
			{
				return $"start: {Start}, end: {End}";
			}
		}

		public StringCommonRange()
		{			
		}

		public StringCommonRange(CharRange source, CharRange target)
		{		
			Text = source.Text;
			Edited = new CharSpan() { Start = source.Start, End = source.End };
			Target = new CharSpan() { Start = target.Start, End = target.End };
		}

		public override string ToString()
		{
			return $"\"{Text}\" [{Text.Length}] ({Side})";
		}
	}
}
