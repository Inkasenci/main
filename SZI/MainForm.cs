using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SZI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btCounters_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Nie teraz!");
        }

        private void btConfig_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Nie teraz!");
        }

        private void btHelp_Click(object sender, EventArgs e)
        {
            var helpForm = new HelpForm();
            helpForm.ShowDialog();
        }

        private void btDataManagement_Click(object sender, EventArgs e)
        {
            var configManagementForm = new ConfigManagementForm();
            configManagementForm.ShowDialog();
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
