using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdamOneilSoftware.CompareLib
{
	public class FileCommonRange
	{
		public FileCommonRange(LineRange left, LineRange right)
		{
			Lines = left.Lines;
			EditedSpan = new LineSpan() { Start = left.Start, End = left.End };
			TargetSpan = new LineSpan() { Start = right.Start, End = right.End };
		}

		public MatchSide Side { get; set; }
		public SourceLine[] Lines { get; private set; }
		public LineSpan EditedSpan { get; private set; }
		public LineSpan TargetSpan { get; private set; }
		public bool IsValid { get { return Lines.Length > 0; } }

		public class LineSpan
		{
			public int Start { get; set; }
			public int End { get; set; }
			public int Length { get { return End - Start; } }

			public override string ToString()
			{
				return $"{Start}-{End} ({Length} lines)";
			}

			public override int GetHashCode()
			{
				return new int[] { Start, End }.GetHashCode();
			}

			public override bool Equals(object obj)
			{
				LineSpan ls = obj as LineSpan;
				if (ls != null)
				{
					return (this.Start == ls.Start && this.End == ls.End);
				}
				return false;
			}
		}

		public override string ToString()
		{
			return $"{Side}: source lines {EditedSpan}, target lines {TargetSpan}, first line: {Lines[0].Text}, last line: {Lines[Lines.Length - 1].Text}";
		}

		public override int GetHashCode()
		{
			return Lines.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			FileCommonRange m = obj as FileCommonRange;
			if (m != null)
			{
				return (m.EditedSpan.Equals(this.EditedSpan) && m.TargetSpan.Equals(this.TargetSpan));
			}
			return false;
		}
	}
}
