using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdamOneilSoftware.CompareLib
{
	public class StringDifferenceRange
	{
		public StringDifferenceRange(StringCommonRange before, StringCommonRange after, string edited, string target)
		{
			if (after.Edited.Start > before.Edited.End)
			{
				Inserted = new Edit() { Text = edited.Substring(before.Length, after.Edited.Start - before.Edited.End) };
			}			
		}

		public Edit Inserted { get; set; }
		public Edit Deleted { get; set; }

		public override string ToString()
		{
			return $"inserted: {Inserted}, deleted {Deleted}";
		}

		public class Edit
		{
			public string Text { get; set; }
			public int Start { get; set; }
			public int End { get; set; }

			public override string ToString()
			{
				return $"\"{Text}\" [{Start}-{End}]";
			}
		}
	}
}
