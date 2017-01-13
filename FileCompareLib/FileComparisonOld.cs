using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdamOneilSoftware
{
	public class FileComparisonOld
	{
		public CompareItem Source { get; set; }
		public CompareItem Target { get; set; }

		public async Task<RangeEdit[]> CompareAsync()
		{
			List<RangeEdit> edits = new List<RangeEdit>();

			await Task.Factory.StartNew(() =>
			{

			});

			return edits.ToArray();
		}
		
		public static IEnumerable<SourceLine> GetInsertedLines(string editedFile, string targetFile)
		{
			var editedLines = GetSourceLines(editedFile);
			var targetLines = GetSourceLines(targetFile);

			return from e in editedLines
				   where (!targetLines.Any(line => line.Text.Equals(e.Text)))
				   select e;
		}

		public static IEnumerable<SourceLine> GetDeletedLines(string editedFile, string targetFile)
		{
			var editedLines = GetSourceLines(editedFile);
			var targetLines = GetSourceLines(targetFile);

			return from t in targetLines
				   where (!editedLines.Any(line => line.Text.Equals(t.Text)))
				   select t;
		}

		public static string GetSourceAsHtml(string fileName, string compareWith = null)
		{
			string result = Path.GetTempFileName() + ".html";

			var lines = GetSourceLines(fileName);
			
			IEnumerable<SourceLine> inserted = null;
			IEnumerable<SourceLine> deleted = null;
			if (!string.IsNullOrEmpty(compareWith))
			{
				inserted = GetInsertedLines(fileName, compareWith);
				deleted = GetDeletedLines(fileName, compareWith);				
			}

			using (StreamWriter writer = File.CreateText(result))
			{
				writer.WriteLine("<html>");
				WriteHtmlHeader(writer);
				writer.WriteLine("<body>");				
				foreach (var line in lines)
				{
					string className = null;
					if (inserted != null)
					{
						if (inserted.Any(item => item.Number == line.Number)) className = "inserted";
					}
					if (deleted != null)
					{
						if (deleted.Any(item => item.Number == line.Number)) className = "deleted";
					}
					
					writer.WriteLine(line.HtmlText(className));
				}				
				writer.WriteLine("</body>");
				writer.WriteLine("</html>");
			}

			return result;
		}

		private static void WriteHtmlHeader(StreamWriter writer)
		{
			writer.WriteLine("<head>");
			writer.WriteLine("<style type=\"text/css\">");

			using (Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream("AdamOneilSoftware.Resources.Code.css"))
			{
				using (StreamReader reader = new StreamReader(s))
				{
					writer.Write(reader.ReadToEnd());
				}
			}

			writer.WriteLine("</style>");
			writer.WriteLine("</head>");
		}

		public static SourceLine[] GetSourceLines(string fileName)
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
					if (!string.IsNullOrWhiteSpace(line))
					{
						results.Add(new SourceLine() { Text = line, Number = lineNum });
					}
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

		public static LineEquality[] CompareSourceLines(string sourceFile, string targetFile)
		{
			var sourceLines = GetSourceLines(sourceFile);
			var targetLines = GetSourceLines(targetFile);

			var results = new List<LineEquality>();
			int lineCount = new int[] { sourceLines.Length, targetLines.Length }.Min();

			int targetIndex = 0;
			for (int sourceIndex = 0; sourceIndex < lineCount; sourceIndex++)
			{
				while (!sourceLines[sourceIndex].Equals(targetLines[targetIndex]))
				{
					// look ahead for where file rejoins
					targetIndex++;
				}
			}

			return results.ToArray();
		}

		private static void CompareSourceLinesInner(SourceLine[] file1Lines, SourceLine[] file2Lines, List<LineEquality> results, int lineCount)
		{
			int file2Index = 0;
			for (int file1Index = 0; file1Index < lineCount; file1Index++)
			{
				while (!file1Lines[file1Index].Equals(file2Lines[file2Index]))
				{
					file2Index++;
				}
				LineEquality lc = new LineEquality() { SourceLine = file1Lines[file1Index].Number, TargetLine = file2Lines[file2Index].Number };
				results.Add(lc);
			}
		}




		private static string ResolveContent(string source)
		{
			if (source.StartsWith("@"))
			{
				string fileName = source.Substring(1);
				if (File.Exists(fileName))
				{
					return File.ReadAllText(fileName);
				}				
			}
			return source;
		}

	}
}
