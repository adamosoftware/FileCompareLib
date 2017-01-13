using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace AdamOneilSoftware.CompareLib
{
	public enum Orientation
	{
		Horizontal,
		Vertical
	}

	public enum DifferenceType
	{
		Inserted,
		Deleted,
		Modified
	}

	public class FileDifferenceRange
	{
		public SourceLine[] EditedLines { get; set; }
		public SourceLine[] TargetLines { get; set; }

		public DifferenceType DifferenceType
		{
			get
			{
				if ((EditedLines?.Length ?? 0) > (TargetLines?.Length ?? 0)) return DifferenceType.Deleted;
				if ((EditedLines?.Length ?? 0) < (TargetLines?.Length ?? 0)) return DifferenceType.Inserted;
				return DifferenceType.Modified;
			}
		}

		public SourceLine[] EditedContextBefore { get; set; }
		public SourceLine[] EditedContextAfter { get; set; }
		public SourceLine[] TargetContextBefore { get; set; }
		public SourceLine[] TargetContextAfter { get; set; }

		public Span EditSite { get; set; }
		public Span TargetSite { get; set; }

		public bool IsWhitespace
		{
			get { return IsAllWhitespace(EditedLines) && IsAllWhitespace(TargetLines); }
		}

		private bool IsAllWhitespace(SourceLine[] lines)
		{
			return lines.All(l => l.IsWhitespace);
		}

		public FileDifferenceRange()
		{
		}

		public override string ToString()
		{
			string edited = (EditedLines.Any()) ? $"edited: {EditedLines.Min(sl => sl.Number)} - {EditedLines.Max(sl => sl.Number)}" : string.Empty;
			string target = (TargetLines.Any()) ? $"target: {TargetLines.Min(sl => sl.Number)} - {TargetLines.Max(sl => sl.Number)}" : string.Empty;
			return string.Join(", ", new string[] { edited, target }.Where(s => !string.IsNullOrEmpty(s)));
		}

		public static FileDifferenceRange FromJson(string json)
		{
			return (FileDifferenceRange)JsonConvert.DeserializeObject(json);
		}

		public static FileDifferenceRange FromXml(string xml)
		{
			XmlSerializer xs = new XmlSerializer(typeof(FileDifferenceRange));
			using (StringReader reader = new StringReader(xml))
			{				
				return (FileDifferenceRange)xs.Deserialize(reader); 
			}
		}

		public FileDifferenceRange(
			FileCommonRange before, FileCommonRange after,
			SourceLine[] editedLines, SourceLine[] targetLines, int contextLines = 6)
		{
			EditedContextBefore = editedLines.Where(sl => sl.Number >= before.EditedSpan.End - contextLines && sl.Number < before.EditedSpan.End).ToArray();
			EditedContextAfter = editedLines.Where(sl => sl.Number <= after.EditedSpan.Start + contextLines && sl.Number > after.EditedSpan.Start).ToArray();

			TargetContextBefore = targetLines.Where(sl => sl.Number >= before.TargetSpan.End - contextLines && sl.Number < before.TargetSpan.End).ToArray();
			TargetContextAfter = targetLines.Where(sl => sl.Number <= after.TargetSpan.Start + contextLines && sl.Number > after.TargetSpan.Start).ToArray();

			EditedLines = editedLines.Where(sl => sl.Number > before.EditedSpan.End && sl.Number < after.EditedSpan.Start)
				.Select((sl, i) => new SourceLine() { Number = sl.Number, Index = i, Text = sl.Text }).ToArray();
			TargetLines = targetLines.Where(sl => sl.Number > before.TargetSpan.End && sl.Number < after.TargetSpan.Start)
				.Select((sl, i) => new SourceLine() { Number = sl.Number, Index = i, Text = sl.Text }).ToArray();

			int editOffset = (AreContinguous(before.EditedSpan, after.EditedSpan)) ? 0 : 1;
			EditSite = new Span() { Start = before.EditedSpan.End + editOffset, End = after.EditedSpan.Start - editOffset };

			int targetOffset = (AreContinguous(before.TargetSpan, after.TargetSpan)) ? 0 : 1;
			TargetSite = new Span() { Start = before.TargetSpan.End + targetOffset, End = after.TargetSpan.Start - targetOffset };
		}

		private bool AreContinguous(FileCommonRange.LineSpan before, FileCommonRange.LineSpan after)
		{
			return ((before.End + 1) == after.Start);
		}

		public string ToXml()
		{
			XmlSerializer xs = new XmlSerializer(typeof(FileDifferenceRange));
			using (StringWriter writer = new StringWriter())
			{
				xs.Serialize(writer, this);
				return writer.ToString();
			}
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this);
		}

		public string ToHtml(Orientation orientation, int contextLines = 6)
		{
			TagBuilder divOutput = new TagBuilder("div");
			switch (orientation)
			{
				case Orientation.Vertical:
					TagBuilder divEdited = new TagBuilder("div", new { @class = "edited" });
					divOutput.InnerContent += divEdited;
					break;

				case Orientation.Horizontal:
					break;
			}
			return divOutput.ToString();
		}
	}
}
