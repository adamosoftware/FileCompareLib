using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdamOneilSoftware.CompareLib
{
	public static class StringDiff
	{
		public static StringEdit[] Compare(string edited, string target, out TimeSpan elapsed)
		{
			Stopwatch sw = Stopwatch.StartNew();

			var commonRanges = FindCommonSubstrings(edited, target);

			string[] skipSubstrings = commonRanges.Select(r => r.Text).ToArray();
			var inserts = FindInsertions(edited, skipSubstrings);
			var deletes = FindInsertions(target, skipSubstrings);

			var results = 
				inserts.Select(cr => new StringEdit() { Range = cr, Action = EditAction.Insert })
				.Concat(deletes.Select(cr => new StringEdit() { Range = cr, Action = EditAction.Delete }))
				.ToArray();

			sw.Stop();
			elapsed = sw.Elapsed;

			return results.ToArray();
		}		

		public static StringEdit[] Compare(string edited, string target)
		{
			TimeSpan elapsed;
			return Compare(edited, target, out elapsed);
		}

		private static CharRange[] FindInsertions(string input, string[] skipSubstrings)
		{
			List<CharRange> results = new List<CharRange>();
			int start = 0;
			int end = 0;
			foreach (var substring in skipSubstrings)
			{
				//if (start > substring.Length) break;

				end = input.IndexOf(substring, start);
				if (end == -1) end = input.Length;
				string insert = input.Substring(start, (end - start));
				if (insert.Length > 0)
				{
					results.Add(new CharRange() { Text = insert, End = end, Start = start });
				}
				start = end + substring.Length;
			}

			if (start < input.Length)
			{
				results.Add(new CharRange() { Text = input.Substring(start), Start = start, End = input.Length });
			}

			return results.ToArray();
		}

		private static StringCommonRange[] FindCommonSubstrings(string edited, string target)
		{
			List<StringCommonRange> results = new List<StringCommonRange>();

			var match = LongestCommonSubstring(edited, target);
			if (match == null) return null;

			match.Side = MatchSide.Center;
			results.Add(match);

			FindCommonSubstringsR(results, match, edited, target);

			return results.OrderBy(item => item.Order).ToArray();
		}

		private static void FindCommonSubstringsR(List<StringCommonRange> results, StringCommonRange match, string edited, string target)
		{
			string leftSource = edited.Substring(0, match.Edited.Start);
			string leftTarget = target.Substring(0, match.Target.Start);
			var leftMatch = LongestCommonSubstring(leftSource, leftTarget);
			if (leftMatch?.IsValid ?? false)
			{
				leftMatch.Side = MatchSide.Left;
				leftMatch.Order = match.Order - 1;
				results.Add(leftMatch);				
			}

			string rightSource = edited.Substring(match.Edited.End);
			string rightTarget = target.Substring(match.Target.End);
			var rightMatch = LongestCommonSubstring(rightSource, rightTarget);
			if (rightMatch?.IsValid ?? false)
			{
				rightMatch.Side = MatchSide.Right;
				rightMatch.Order = match.Order + 1;
				results.Add(rightMatch);
			}

			if (leftMatch.IsValid)
			{
				FindCommonSubstringsR(results, leftMatch, leftSource, leftTarget);
			}
			
			if (rightMatch.IsValid)
			{
				FindCommonSubstringsR(results, rightMatch, rightSource, rightTarget);
			}			
		}

		private static StringCommonRange LongestCommonSubstring(string source, string target)
		{
			if (target == null || source == null) return null;

			int length = 0;
			int shorterString = new int[] { source.Length, target.Length }.Min();

			while (true)
			{
				length++;				

				int matchCount = (from r1 in AsRanges(source, length)
								  join r2 in AsRanges(target, length) on r1.Text equals r2.Text
								  select r1.Text).Count();

				if (matchCount == 0 && length > 0 || (length > shorterString))
				{
					// go back one, that was your match
					length--;
					try
					{
						return (from r1 in AsRanges(source, length)
								join r2 in AsRanges(target, length) on r1.Text equals r2.Text
								select new StringCommonRange(r1, r2)).First();
					}
					catch
					{
						return null;
					}
				}
			}			
		}

		private static CharRange[] AsRanges(string text, int length)
		{
			List<CharRange> results = new List<CharRange>();

			int position = 0;
			while (true)
			{
				if (position + length > text.Length) break;
				string substring = text.Substring(position, length);

				CharRange r = new CharRange() { Text = substring, Start = position, End = position + length };
				results.Add(r);
				position++;
			}

			return results.ToArray();
		}
	}
}
