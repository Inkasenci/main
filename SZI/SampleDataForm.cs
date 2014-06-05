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
    /// Formularz umożliwiający konfigurację liczby generowanych losowo danych.
    /// </summary>
    public partial class SampleDataForm : Form
    {
        /// <summary>
        /// Kontruktor formularza. Przeprowadza podstawowe inicjalizacje generowanie automatycznie. Dodaje do pól tekstowych metody obsługujące zdarzenie.
        /// </summary>
        public SampleDataForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            collectorsCount.KeyPress += textBox_KeyPress;
            customersCount.KeyPress += textBox_KeyPress;
            areasCount.KeyPress += textBox_KeyPress;
            countersCount.KeyPress += textBox_KeyPress;
            addressesCount.KeyPress += textBox_KeyPress;
            readingsCount.KeyPress += textBox_KeyPress;
        }

        /// <summary>
        /// Wywoływana, gdy pola tekstowe są aktywne i zostanie naciśnięty klawisz. Sprawia, że w polach tekstowych pojawiają się tylko liczby całkowite.
        /// </summary>
        /// <param name="sender">Pola tekstowe.</param>
        /// <param name="e">Argumenty zdarzenia związanego z naciśnięciem klawisza.</param>
        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        /// <summary>
        /// Wywoływana, gdy zostanie naciśnięty przycisk "Generuj". Rozpoczyna generowanie przykładowej bazy danych. 
        /// </summary>
        /// <param name="sender">Przycisk "Generuj".</param>
        /// <param name="e">Argumenty zdarzenia.</param>
        private void generationButton_Click(object sender, EventArgs e)
        {
            if (collectorsCount.Text == String.Empty || Convert.ToInt32(collectorsCount.Text) < 1)
                collectorsCount.Text = "1";

            if (customersCount.Text == String.Empty || Convert.ToInt32(customersCount.Text) < 1)
                customersCount.Text = "1";

            if (areasCount.Text == String.Empty || Convert.ToInt32(areasCount.Text) < 1)
                areasCount.Text = "1";

            if (areasCount.Text == String.Empty || Convert.ToInt32(areasCount.Text) > 10)
                areasCount.Text = "10";

            if (countersCount.Text == String.Empty || Convert.ToInt32(countersCount.Text) < 1)
                countersCount.Text = "1";

            if (addressesCount.Text == String.Empty || Convert.ToInt32(addressesCount.Text) < 1)
                addressesCount.Text = "1";

            if (readingsCount.Text == String.Empty || Convert.ToInt32(readingsCount.Text) < 1)
                readingsCount.Text = "1";

            SampleDataConfig.numberOfCollectors = Convert.ToInt32(collectorsCount.Text);
            SampleDataConfig.numberOfCustomers = Convert.ToInt32(customersCount.Text);
            SampleDataConfig.numberOfAreas = Convert.ToInt32(areasCount.Text);
            SampleDataConfig.numberOfCounters = Convert.ToInt32(countersCount.Text);
            SampleDataConfig.numberOfAddresses = Convert.ToInt32(addressesCount.Text);
            SampleDataConfig.numberOfReadings = Convert.ToInt32(readingsCount.Text);

            SampleDataConfig.GenerateDataBase();
            MessageBox.Show("Przykładowa baza danych wygenerowana. ");
        }
    }
}
