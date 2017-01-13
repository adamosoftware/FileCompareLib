using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdamOneilSoftware.CompareLib
{
	public enum MergeDirection
	{
		ToTarget,
		ToEdited,
		Forward,
		Backward
	}

	public class FileDiff
	{
		public FileDiff()
		{
		}

		public FileDiff(string editedFile, string targetFile)
		{
			EditedFile = editedFile;
			TargetFile = targetFile;
		}

		public string EditedFile { get; set; }
		public string TargetFile { get; set; }
		public TimeSpan Elapsed { get; private set; }

		public string MergeLogFile { get; set; }

		public FileDifferenceRange[] Compare(string editedFile, string targetFile)
		{
			EditedFile = editedFile;
			TargetFile = targetFile;
			return Compare();
		}

		public FileDifferenceRange[] Compare()
		{
			Stopwatch sw = Stopwatch.StartNew();

			var editedLines = GetSourceLines(EditedFile);
			var targetLines = GetSourceLines(TargetFile);

			var commonRanges = FindCommonRanges(editedLines, targetLines).ToArray();
			var results = new List<FileDifferenceRange>();

			for (int i = 0; i < commonRanges.Length - 1; i++)
			{
				var diff = new FileDifferenceRange(commonRanges[i], commonRanges[i + 1], editedLines, targetLines);
				if (!diff.IsWhitespace) results.Add(diff);
			}

			sw.Stop();
			Elapsed = sw.Elapsed;

			return results.ToArray();
		}		

		public FileDifferenceRange[] Merge(FileDifferenceRange range, MergeDirection direction)
		{
			string sourceFile = (direction == MergeDirection.Forward || direction == MergeDirection.ToEdited) ? EditedFile : TargetFile;
			string destFile = (direction == MergeDirection.Forward || direction == MergeDirection.ToEdited) ? TargetFile : EditedFile;
			SourceLine[] startLines = (direction == MergeDirection.Forward || direction == MergeDirection.ToEdited) ? range.EditedLines : range.TargetLines;
			SourceLine[] destLines = (direction == MergeDirection.Forward || direction == MergeDirection.ToEdited) ? range.TargetLines : range.EditedLines;

			LineFileEditor editor = new LineFileEditor(destFile);

			// same index updates
			var edits = from start in startLines
						join dest in destLines on start.Index equals dest.Index
						select new { SourceLineText = start.Text, TargetLineNum = dest.Number };
			foreach (var edit in edits)
			{
				editor[edit.TargetLineNum] = edit.SourceLineText;
			}

			// delete line indexes from target not in edited
			var deleted = from dest in destLines
						  where !startLines.Any(sl => sl.Index == dest.Index)
						  select dest.Number;
			if (deleted.Any())
			{
				editor.Delete(deleted.First(), deleted.Count());
			}
			
			// insert line indexes from edited not in target
			var inserted = (from start in startLines
						   where !destLines.Any(sl => sl.Index == start.Index)
						   select start).ToArray();
			if (inserted.Any())
			{				
				editor.Insert(range.TargetSite.Start, inserted);
			}
			
			editor.Save();

			if (!string.IsNullOrEmpty(MergeLogFile))
			{
				LogMerge(sourceFile, destFile, range, direction);
			}

			OnMerge(range, direction);

			return Compare();
		}

		private void LogMerge(string sourceFile, string destFile, FileDifferenceRange range, MergeDirection direction)
		{
			string folder = Path.GetDirectoryName(MergeLogFile);
			if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

			using (StreamWriter writer = File.AppendText(MergeLogFile))
			{
				writer.WriteLine($"{DateTime.Now}");
				writer.WriteLine($"{sourceFile} -> {destFile} ({direction})");
				writer.Write(range.ToJson());
				writer.WriteLine();
			}
		}

		public FileDifferenceRange[] Undo(FileDifferenceRange range)
		{
			return Merge(range, MergeDirection.Backward);
		}

		public virtual void OnMerge(FileDifferenceRange range, MergeDirection direction)
		{
		}

		private static SourceLine[] GetSourceLines(string fileName)
		{
			// omits blank and whitespace-only lines
			List<SourceLine> results = new List<SourceLine>();
			using (StreamReader reader = File.OpenText(fileName))
			{
				int lineNum = 0;				
				while (!reader.EndOfStream)
				{
					lineNum++;
					string line = reader.ReadLine();
					line = TrimEndingWhitespace(line);					
					results.Add(new SourceLine() { Text = line, Number = lineNum });					
				}
			}
			return results.ToArray();
		}

		private static string TrimEndingWhitespace(string input)
		{
			if (string.IsNullOrEmpty(input)) return input;
			if (string.IsNullOrWhiteSpace(input)) return null;

			int position = input.Length - 1;
			char c = input[position];
			while (char.IsWhiteSpace(c))
			{
				position--;
				c = input[position];
			}
			return input.Substring(0, position + 1);
		}		

		private IEnumerable<FileCommonRange> FindCommonRanges(SourceLine[] editedLines, SourceLine[] targetLines)
		{
			List<FileCommonRange> results = new List<FileCommonRange>();
			var match = FindLongestCommonRange(editedLines, targetLines);
			match.Side = MatchSide.Center;
			results.Add(match);

			FindCommonRangesR(results, match, editedLines, targetLines);

			return results.OrderBy(m => m.EditedSpan.Start).ToArray();
		}

		private IEnumerable<FileCommonRange> FindCommonRanges()
		{
			var editedLines = GetSourceLines(EditedFile);
			var targetLines = GetSourceLines(TargetFile);
			return FindCommonRanges(editedLines, targetLines);
		}

		private void FindCommonRangesR(List<FileCommonRange> results, FileCommonRange referenceMatch, SourceLine[] editedLines, SourceLine[] targetLines)
		{
			var leftEdited = editedLines.Where(sl => sl.Number < referenceMatch.EditedSpan.Start).ToArray();
			var leftTarget = targetLines.Where(sl => sl.Number < referenceMatch.TargetSpan.Start).ToArray();

			var rightEdited = editedLines.Where(sl => sl.Number > referenceMatch.EditedSpan.End).ToArray();
			var rightTarget = targetLines.Where(sl => sl.Number > referenceMatch.TargetSpan.End).ToArray();

			var leftMatch = FindLongestCommonRange(leftEdited, leftTarget);
			AddMatch(results, leftMatch, MatchSide.Left);

			var rightMatch = FindLongestCommonRange(rightEdited, rightTarget);
			AddMatch(results, rightMatch, MatchSide.Right);

			if (leftMatch != null)
			{
				FindCommonRangesR(results, leftMatch, leftEdited, leftTarget);
			}
			
			if (rightMatch != null)
			{
				FindCommonRangesR(results, rightMatch, rightEdited, rightTarget);
			}					
		}

		private void AddMatch(List<FileCommonRange> results, FileCommonRange comparisonMatch, MatchSide side)
		{
			if (comparisonMatch == null) return;
			if (!results.Any(m => m.Equals(comparisonMatch)))
			{
				comparisonMatch.Side = side;
				results.Add(comparisonMatch);
			}
		}

		private int MaxLineNum(SourceLine[] lines)
		{
			return lines.Max(l => l.Number);
		}

		private FileCommonRange FindLongestCommonRange()
		{
			var editedLines = GetSourceLines(EditedFile);
			var targetLines = GetSourceLines(TargetFile);
			return FindLongestCommonRange(editedLines, targetLines);
		}

		private static FileCommonRange FindLongestCommonRange(SourceLine[] editedLines, SourceLine[] targetLines)
		{
			if (editedLines == null || targetLines == null || editedLines.Length == 0 || targetLines.Length == 0)
			{
				return null;
			}

			if (SourcesAreEqual(editedLines, targetLines))
			{
				return new FileCommonRange(new LineRange(editedLines), new LineRange(targetLines));
			}			

			int length = 0;
			int shorterFile = new int[] { editedLines.Length, targetLines.Length }.Min();

			while (true)
			{
				length++;				

				int matchCount = (from left in AsRanges(editedLines, length)
								  join right in AsRanges(targetLines, length) on left equals right
								  select left).Count();

				if ((matchCount == 0 && length > 1) || (length > shorterFile))
				{
					// go back one, that's your match
					length--;

					try
					{
						return (from left in AsRanges(editedLines, length)
								join right in AsRanges(targetLines, length) on left equals right
								select new FileCommonRange(left, right)).First();
					}
					catch
					{
						return null;
					}
				}
			}
		}

		private static bool SourcesAreEqual(SourceLine[] array1, SourceLine[] array2)
		{
			if (array1.Length == array2.Length)
			{
				for (int i = 0; i < array1.Length; i++)
				{
					if (!array1[i].Equals(array2[i])) return false;
				}
				return true;
			}
			return false;
		}

		private static FileCommonRange FindLongestCommonRange(string editedFile, string targetFile)
		{			
			SourceLine[] editedLines = GetSourceLines(editedFile);
			SourceLine[] targetLines = GetSourceLines(targetFile);

			return FindLongestCommonRange(editedLines, targetLines);
		}

		private static IEnumerable<LineRange> AsRanges(SourceLine[] lines, int length)
		{
			List<LineRange> results = new List<LineRange>();

			int index = 0;
			while (true)
			{
				if (index + length > lines.Length) break;
				var selection = lines.Skip(index).Take(length).ToArray();

				LineRange s = new LineRange(selection);
				results.Add(s);
				index++;
			}

			return results.ToArray();
		}
	}
}
