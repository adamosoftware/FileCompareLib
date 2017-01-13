using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdamOneilSoftware.CompareLib
{
	internal class LineFileEditor
	{
		private string _fileName = null;
		private Dictionary<int, string> _lines = null;

		public LineFileEditor(string fileName)
		{
			_fileName = fileName;
			Dictionary<int, string> lines = new Dictionary<int, string>();

			using (StreamReader reader = File.OpenText(fileName))
			{
				int lineNum = 0;
				while (!reader.EndOfStream)
				{
					lineNum++;
					lines.Add(lineNum, reader.ReadLine());
				}
			}

			_lines = lines;
		}

		public string this[int line]
		{
			get { return _lines[line]; }
			set { _lines[line] = value; }
		}
		
		public void Save()
		{
			using (StreamWriter writer = File.CreateText(_fileName))
			{
				foreach (var keyPair in _lines)
				{
					writer.WriteLine(keyPair.Value);
				}
			}
		}

		public void Insert(int lineNum, SourceLine[] lines)
		{
			List<string> targetLines = _lines.Select(keyPair => keyPair.Value).ToList();
			for (int i = 0; i < lines.Length; i++)
			{
				targetLines.Insert(lineNum, lines[i].Text);
				lineNum++;
			}
			_lines = targetLines.Select((s, i) => new { Key = i, Value = s }).ToDictionary(item => item.Key, item => item.Value);
		}

		internal void Delete(int lineNum, int length)
		{			
			List<string> targetLines = _lines.Select(keyPair => keyPair.Value).ToList();
			targetLines.RemoveRange(lineNum - 1, length);
			_lines = targetLines.Select((s, i) => new { Key = i, Value = s }).ToDictionary(item => item.Key, item => item.Value);
		}
	}
}
