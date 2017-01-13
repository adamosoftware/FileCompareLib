using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdamOneilSoftware.CompareLib
{
	public class CharRange
	{
		public string Text { get; set; }
		public int Start { get; set; }
		public int End { get; set; }
		public int Length { get { return Text.Length; } }

		public override string ToString()
		{
			return $"\"{Text}\", start: {Start}, end: {End}";
		}
	}
}
