using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdamOneilSoftware.CompareLib
{
	public class Span
	{
		public int Start { get; set; }
		public int End { get; set; }

		public override string ToString()
		{
			return $"[{Start}-{End}]";
		}
	}
}
