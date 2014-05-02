using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace SZI
{
    public partial class ConfigurationForm : Form
    {
        private string ToPrint;
        private string documentContents;
        private PrintPreviewDialog printPreviewDialog1 = new PrintPreviewDialog();
        private List<List<string>> CollectorsList;
        public ConfigurationForm()
        {
            InitializeComponent();
        }

        private void btSampleData_Click(object sender, EventArgs e)
        {
            SampleDataConfig.GenerateDataBase();
        }

        private void btClearDataBase_Click(object sender, EventArgs e)
        {
            SampleDataConfig.ClearDataBase();
        }

        private void btBasicRaport_Click(object sender, EventArgs e)
        {
            printPreviewDialog1 = Reports.Collectors.CreateReport();
            printPreviewDialog1.Show();
        }

        void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            int charactersOnPage = 0;
            int linesPerPage = 0;

            // Sets the value of charactersOnPage to the number of characters 
            // of stringToPrint that will fit within the bounds of the page.
            e.Graphics.MeasureString(ToPrint, new Font("Arial", 10),
                e.MarginBounds.Size, StringFormat.GenericTypographic,
                out charactersOnPage, out linesPerPage);
            
            // Draws the string within the bounds of the page.
            e.Graphics.DrawString(ToPrint, new Font("Arial", 10), Brushes.Black,
            e.MarginBounds, StringFormat.GenericTypographic);

            // Remove the portion of the string that has been printed.
            ToPrint = ToPrint.Substring(charactersOnPage);

            // Check to see if more pages are to be printed.
            e.HasMorePages = (ToPrint.Length > 0);

            // If there are no more pages, reset the string to be printed.
            if (!e.HasMorePages)
                ToPrint = documentContents;
        }
    }
}
