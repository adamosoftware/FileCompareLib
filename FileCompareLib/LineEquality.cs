using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdamOneilSoftware
{
	public class LineEquality
	{
		public string Text { get; set; }
		public int SourceLine { get; set; }
		public int TargetLine { get; set; }

		public override string ToString()
		{
			if (SourceLine == TargetLine)
			{
				return $"{SourceLine} : {Text}";
			}
			else
			{
				return $"{SourceLine} - {TargetLine} : {Text}";
			}			
		}
	}
}
