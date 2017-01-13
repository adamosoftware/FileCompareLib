using AdamOneilSoftware;
using AdamOneilSoftware.CompareLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCompareConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			/*var ranges = FileComparison.AsRanges("this is what I like", 4);
			foreach (var r in ranges)
			{
				Console.WriteLine(r.Text);
			}*/

			/*
			var result = FileComparison.LongestCommonSubstring(
				"and this shall become something we think much about.",
				"Be that as it may, and this shall be whatever something we think happily much canopy.");
			Console.WriteLine(result);
			*/

			// "You might think I'm crazy, but what else can be done heresoever to Theseus?"
			// "Theseus might think I'm being obstinate, but what else can be thought of heresoever?"

			//var diffs = sd.Compare();
			//const string first = "poppy red green blue orange yellow";
			//const string second = "red blue airplane yellow pants";

			//const string first = "this be a house we care about today";
			//const string second = "this be a boat we care about tomorrow";

			//var common = StringDiff.FindCommonSubstrings(first, second);

			//"outputClass, TheMethodName, CSharpArgsFromParams(Command, HandleConnection, whencePorvor));",
			//"outputClass, MethodName, CSharpArgsFromParams(Command, !HandleConnection, whenceParvor));");

			
			string first = @"\t\t\tUserName = Encryption.Decrypt(doc.SelectSingleNode(""SavedCredentials/UserName"").InnerText);";
			string second = @"\t\t\tUserName = Encryption.Decrypt(doc.SelectSingleNode(""SavedCredentials/Login"").InnerText);";

			Console.WriteLine(first);
			Console.WriteLine(second);
			Console.WriteLine();

			/*foreach (var c in common)
			{
				Console.WriteLine(c);
			}*/

			var comp = StringDiff.Compare(first, second);
			foreach (var diff in comp)
			{
				Console.WriteLine(diff);
			}

			
			/*
			FileDiff d = new FileDiff(
				@"C:\Users\Adam\Dropbox\Visual Studio 2015\Projects\DbTools\ConnectionUI\dsConnections.cs",
				@"C:\Users\Adam\Dropbox\Visual Studio 2015\Projects\ConnectionManagerUI\dsConnections.cs");
			d.MergeLogFile = @"C:\users\adam\dropbox\diff_merge_log.txt";

			var diffs = d.Compare();			

			Console.WriteLine($"Edited File: {d.EditedFile}");
			Console.WriteLine($"Target File: {d.TargetFile}");
			Console.WriteLine($"{d.Elapsed}");
			Console.WriteLine();

			int index = 0;
			foreach (var diff in diffs)
			{
				index++;
				Console.WriteLine($"diff {index}");
				Console.WriteLine($"\tedited: {diff.EditSite}");
				foreach (var line in diff.EditedLines) Console.WriteLine($"\t\t{line}");
				Console.WriteLine($"\ttarget: {diff.TargetSite}");
				foreach (var line in diff.TargetLines) Console.WriteLine($"\t\t{line}");
				Console.WriteLine();

				//Console.WriteLine(diff.ToXml());
				//Console.WriteLine(diff.ToJson());
			}

			//d.Merge(diffs[0], MergeDirection.Forward);			
			
			/*var results = d.FindCommonRanges();

			int index = 0;
			foreach (var m in results)
			{
				index++;
				Console.WriteLine($"match {index}-{m.Side}:");
				Console.WriteLine($"\tedited lines: {m.EditedSpan}");
				Console.WriteLine($"\ttarget lines: {m.TargetSpan}");
				Console.WriteLine($"\tfirst line: {m.Lines[0].Text}");
				Console.WriteLine($"\tlast line: {m.Lines[m.Lines.Length - 1].Text}");
				Console.WriteLine();
			}*/

			Console.ReadLine();
		}
	}
}
