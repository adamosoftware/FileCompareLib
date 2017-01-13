using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdamOneilSoftware.CompareLib
{
	public class LineRange
	{
		public SourceLine[] Lines { get; private set; }
		public string HashExpression { get; private set; }
		public int Start { get; private set; }
		public int End { get; private set; }

		public LineRange(SourceLine[] lines)
		{
			Lines = lines;
			HashExpression = string.Join(":", lines.Select(line => line.GetHashCode().ToString()));
			Start = lines.Min(l => l.Number);
			End = lines.Max(l => l.Number);
		}

		public override bool Equals(object obj)
		{
			LineRange test = obj as LineRange;
			if (test != null) return test.HashExpression.Equals(this.HashExpression);
			return false;
		}

		public override int GetHashCode()
		{
			return HashExpression.GetHashCode();
		}
	}
}
