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
    public partial class TESTForm : Form
    {
        public TESTForm()
        {
            InitializeComponent();
            ComboBoxConfig cbc1 = new ComboBoxConfig("Collector", "collectorComboBox", new Point(10, 10));
            this.Controls.Add(cbc1.InitializeComboBox());

            ComboBoxConfig cbc2 = new ComboBoxConfig("Customer", "collectorComboBox", new Point(10, 50));
            //MessageBox.Show(cbc2.comboBox.SelectedIndex.ToString());
            this.Controls.Add(cbc2.InitializeComboBox());
        }
    }
}
