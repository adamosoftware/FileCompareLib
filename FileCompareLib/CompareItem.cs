using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdamOneilSoftware
{
	public enum SourceType
	{
		NotSet,
		String,
		File
	}

	public class CompareItem
	{
		public SourceType Type { get; set; }
		public string Source { get; set; }

		public override string ToString()
		{
			switch (Type)
			{
				case SourceType.NotSet:
				case SourceType.String:
					if (Source.StartsWith("@"))
					{
						return File.ReadAllText(Source);
					}
					else
					{
						return Source;
					}
				case SourceType.File:
					return File.ReadAllText(Source);
				default:
					throw new ArgumentException("SourceType");
			}
		}
	}
}
