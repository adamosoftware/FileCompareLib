using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using AdamOneilSoftware.CompareLib;

namespace AdamOneilSoftware
{
	public partial class FileViewer : UserControl
	{
		public event EventHandler FileSelected;

		public FileViewer OppositeViewer { get; set; }

		public string Filename
		{
			get { return tbFilename.Text; }
		}

		public bool HasFile
		{
			get { return File.Exists(tbFilename.Text); }
		}

		public FileViewer()
		{
			InitializeComponent();
		}

		private void btnSelectFile_Click(object sender, EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "Code Files|*.cs;*.cshtml;*.css;*.js|All Files|*.*";
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				tbFilename.Text = dlg.FileName;
				string compareWith = null;
				if ((OppositeViewer?.HasFile) ?? false)
				{
					compareWith = OppositeViewer.Filename;
				}
				//string tempFile = FileComparisonOld.GetSourceAsHtml(dlg.FileName, compareWith);
				//swebBrowser1.Navigate($"file:///{tempFile}");
				FileSelected?.Invoke(this, e);
				//File.Delete(tempFile);
			}
		}

		public void FormatInsertedLines(IEnumerable<SourceLine> lines)
		{
			foreach (var line in lines)
			{

			}
		}
	}
}
