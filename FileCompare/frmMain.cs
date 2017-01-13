using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdamOneilSoftware
{
    public partial class frmMain: Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

		private void fvTarget_FileSelected(object sender, EventArgs e)
		{
			DoComparison();
		}

		private void DoComparison()
		{
			if (!fvEdited.HasFile) return;
			if (!fvTarget.HasFile) return;

			//var insertedLines = FileComparisonOld.GetInsertedLines(fvEdited.Filename, fvTarget.Filename);
			//fvEdited.FormatInsertedLines(insertedLines);

			//var deletedLines = FileComparisonOld.GetDeletedLines(fvEdited.Filename, fvTarget.Filename);
		}

		private void fvEdited_FileSelected(object sender, EventArgs e)
		{
			DoComparison();
		}
	}
}
