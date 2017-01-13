using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdamOneilSoftware
{	
	public class RangeEdit
	{
		public CharRange Keep { get; set; }
		public CharRange Delete { get; set; }
		public CharRange Insert { get; set; }		

		public int Start
		{
			get { return new int[] { Keep.Start, Delete.Start, Insert.Start }.Min(); }
		}

		public int End
		{
			get { return new int[] { Keep.End, Delete.End, Insert.End }.Max(); }
		}
	}
}
