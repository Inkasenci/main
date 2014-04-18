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
    /// <summary>
    /// Klasa obsługująca podstawowe ukazujące się okno.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Konstruktor formy.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Okno pozwalajace na obsługę odczytów.
        /// </summary>
        /// <param name="sender">Obiekt eventu.</param>
        /// <param name="e">Argument eventu.</param>
        private void btCounters_Click(object sender, EventArgs e)
        {
            var countersForm = new CountersForm();
            countersForm.ShowDialog();
        }

        /// <summary>
        /// Okno obsługi ustawień aplikacji.
        /// </summary>
        /// <param name="sender">Obiekt eventu.</param>
        /// <param name="e">Argument eventu.</param>
        private void btConfig_Click(object sender, EventArgs e)
        {
            var configurationForm = new ConfigurationForm();
            configurationForm.ShowDialog();
        }

        /// <summary>
        /// Okno pomocy.
        /// </summary>
        /// <param name="sender">Obiekt eventu.</param>
        /// <param name="e">Argument eventu.</param>
        private void btHelp_Click(object sender, EventArgs e)
        {
            var helpForm = new HelpForm();
            helpForm.ShowDialog();
        }

        /// <summary>
        /// Okno zarządzania danymi ( Inkasenci, Tereny, Adresy, Klienci, Liczniki ).
        /// </summary>
        /// <param name="sender">Obiekt eventu.</param>
        /// <param name="e">Argument eventu.</param>
        private void btDataManagement_Click(object sender, EventArgs e)
        {
            var configManagementForm = new ConfigManagementForm();
            configManagementForm.ShowDialog();
        }

        /// <summary>
        /// Zamykanie aplikacji.
        /// </summary>
        /// <param name="sender">Obiekt eventu.</param>
        /// <param name="e">Argument eventu.</param>
        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Edytor plików XML wygenerowanych przez aplikację.
        /// </summary>
        /// <param name="sender">Obiekt eventu.</param>
        /// <param name="e">Argument eventu.</param>
        private void btXMLTextEditor_Click(object sender, EventArgs e)
        {
            var XMLTextEditorForm = new XMLTextEditor();
            XMLTextEditorForm.ShowDialog();
        }
    }
}
