using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdamOneilSoftware.CompareLib
{
	public enum EditAction
	{
		Insert,
		Delete
	}

	public class StringEdit
	{
		public EditAction Action { get; set; }
		public CharRange Range { get; set; }

		public override string ToString()
		{
			return $"{Action}: {Range}";
		}
	}
}
