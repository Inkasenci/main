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

            /*Button button1 = new Button();
            button1.Text = "Generuj";
            button1.Location = new Point(100, 100);
            button1.Click += button1_Click;
            this.Controls.Add(button1);

            Button button2 = new Button();
            button2.Text = "Wyczyść";
            button2.Location = new Point(100, 130);
            button2.Click += button2_Click;
            this.Controls.Add(button2);*/
        }

        /*void button1_Click(object sender, EventArgs e)
        {
            SampleDataConfig.GenerateDataBase();
        }

        void button2_Click(object sender, EventArgs e)
        {
            SampleDataConfig.ClearDataBase();
        }*/

        private void btCounters_Click(object sender, EventArgs e)
        {
            var countersForm = new CountersForm();
            countersForm.ShowDialog();
        }

        private void btConfig_Click(object sender, EventArgs e)
        {
            var configurationForm = new ConfigurationForm();
            configurationForm.ShowDialog();
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
