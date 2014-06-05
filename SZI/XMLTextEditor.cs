using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace SZI
{
    /// <summary>
    /// Klasa obsługująca Model XML Odczytu - SZI.
    /// </summary>
    public partial class XMLTextEditor : Form
    {
        /// <summary>
        /// Lista textBoxów zawierających dane poszczególnych pól pliku XMA.
        /// </summary>
        private List<TextBox> textBox;
        /// <summary>
        /// Informuje o nie zapisanych danych - zapobiega ich utracie.
        /// </summary>
        private bool SaveData = false;
        /// <summary>
        /// Rekord, w którym aktualnie przebywamy.
        /// </summary>
        private int nrRecord = 0;
        /// <summary>
        /// Wczytane z pliku XML rekordy.
        /// </summary>
        private CountersCollection xmlRecords = new CountersCollection();
        /// <summary>
        /// Adres pliku, z którego pobieramy dane.
        /// </summary>
        private string path;

        /// <summary>
        /// Pozycja kolejnych textBoxów.
        /// </summary>
        private int[][] initPosition = new int[][]
        {
            new int[]{50,50},
            new int[]{450,50},
            new int[]{50,100},
            new int[]{450,100},
            new int[]{50,150},
            new int[]{450,150},
            new int[]{50,200},
            new int[]{450,200}
        };

        /// <summary>
        /// Określanie dostępu do kolejnych pól.
        /// </summary>
        private bool[] readOnly = new bool[]
        {
            true,
            true,
            true,
            true,
            true,
            true,
            true,
            false
        };

        /// <summary>
        /// Opis poszczególnych textBoxów.
        /// </summary>
        private string[] labels = new string[]
        {
                "Odczyt nr: ",
                "Nr licznika: ",
                "Nr układu: ",
                "Klient: ",
                "Adres: ",
                "Ostatni odczyt wykonano: ",
                "Ostatni odczyt: ",
                "Aktualny odczyt: "
        };

        /// <summary>
        /// Funkcja inicjowana przy starcie okna.
        /// </summary>
        public XMLTextEditor()
        {
            InitializeComponent();
            InitForm();
        }

        /// <summary>
        /// Ustawienie textBoxów i opisów w oknie edytora.
        /// </summary>
        private void InitForm()
        {
            int i = 0;
            textBox = new List<TextBox>();
            TextBox tmp;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            foreach (var element in initPosition)
            {
                tmp = new TextBox();
                tmp.Location = new Point(element[0], element[1]);
                tmp.Size = new System.Drawing.Size(200, 50);
                tmp.ReadOnly = readOnly[i];
                textBox.Add(tmp);
                this.Controls.Add(tmp);
                Label tmpL = new Label();
                tmpL.Text = labels[i++];
                tmpL.Location = new Point(element[0], element[1]-15);
                tmpL.Size = new System.Drawing.Size(200, 50);
                this.Controls.Add(tmpL);
            }
            this.FormClosed += Event_FormClosing;
        }

        /// <summary>
        /// Odczyt elementu z listy.
        /// </summary>
        /// <param name="i">Indeks odczytywanego elementu.</param>
        private void ReadElement(int i)
        {
            if (xmlRecords.RecordsCount > 0)
            {
                int j = 0;
                textBox.ElementAt(j++).Text = xmlRecords.counter[i].ReadId;
                textBox.ElementAt(j++).Text = xmlRecords.counter[i].CircuitNo;
                textBox.ElementAt(j++).Text = xmlRecords.counter[i].CounterNo;
                textBox.ElementAt(j++).Text = xmlRecords.counter[i].Customer;
                textBox.ElementAt(j++).Text = xmlRecords.counter[i].Address;
                textBox.ElementAt(j++).Text = xmlRecords.counter[i].LastReadDate;
                textBox.ElementAt(j++).Text = xmlRecords.counter[i].LastValue;
                textBox.ElementAt(j++).Text = xmlRecords.counter[i].NewValue;
            }
            else
            {
                MessageBox.Show("Brak rekordów w bazie!");
            }
        }

        /// <summary>
        /// Kończenie działania aplikacji - sprawdza, czy zapisać dane.
        /// </summary>
        /// <param name="sender">Obiekt eventu.</param>
        /// <param name="e">Argument eventu.</param>
        private void Event_FormClosing(object sender, EventArgs e)
        {
            DialogResult choiceFromMessageBox = DialogResult.No;
            if (SaveData)
                choiceFromMessageBox = MessageBox.Show("Czy zapisać dane przed wyjściem z edytora?", "Ostrzeżenie", MessageBoxButtons.YesNo);
            if (choiceFromMessageBox == DialogResult.Yes)
            {
                zapiszToolStripMenuItem_Click(sender, e);
            }
        }

        /// <summary>
        /// Kończenie działania aplikacji - Click.
        /// </summary>
        /// <param name="sender">Obiekt eventu.</param>
        /// <param name="e">Argument eventu.</param>
        private void zakończProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
                this.Close();
        }

        /// <summary>
        /// Otwarcie i wczytanie danych z pliku XML - Click.
        /// </summary>
        /// <param name="sender">Obiekt eventu.</param>
        /// <param name="e">Argument eventu.</param>
        private void otwórzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @"C:\";
            openFileDialog.DefaultExt = "xml";
            openFileDialog.Filter = "XML file|*.xml";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Title = "Wczytaj dane";
            DialogResult check = openFileDialog.ShowDialog();

            if (openFileDialog.FileName == String.Empty && check == DialogResult.OK && File.Exists(openFileDialog.FileName))
                MessageBox.Show("Błąd! Brak pliku!");
            else if (check == DialogResult.OK)
            {
                this.path = openFileDialog.FileName;
                StaticXML.ReadFromXml(this.path, false, out xmlRecords);
                nrRecord = 0;
                ReadElement(nrRecord);
            }
        }

        /// <summary>
        /// Zapis danych do tego samego pliku - Click.
        /// </summary>
        /// <param name="sender">Obiekt eventu.</param>
        /// <param name="e">Argument eventu.</param>
        private void zapiszToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (xmlRecords.RecordsCount > 0)
            {
                StaticXML.WriteToXml(this.path, xmlRecords, out SaveData);
            }
            else
            {
                MessageBox.Show("Błąd! Brak danych!");
            }
        }

        /// <summary>
        /// Uruchomienie pomocy - Click.
        /// </summary>
        /// <param name="sender">Obiekt eventu.</param>
        /// <param name="e">Argument eventu.</param>
        private void pomocToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var helpForm = new HelpForm();
            helpForm.ShowDialog();
        }

        /// <summary>
        /// Przejście do następnego rekordu - Click.
        /// </summary>
        /// <param name="sender">Obiekt eventu.</param>
        /// <param name="e">Argument eventu.</param>
        private void btNext_Click(object sender, EventArgs e)
        {
            if (++nrRecord >= xmlRecords.RecordsCount)
                nrRecord = 0;
            ReadElement(nrRecord);
        }

        /// <summary>
        /// Przejście do poprzedniego rekordu - Click.
        /// </summary>
        /// <param name="sender">Obiekt eventu.</param>
        /// <param name="e">Argument eventu.</param>
        private void btPrevious_Click(object sender, EventArgs e)
        {
            if (--nrRecord < 0)
                nrRecord = xmlRecords.RecordsCount - 1;
            ReadElement(nrRecord);
        }

        /// <summary>
        /// Zapis zmian wprowadzonych w textBoxie - Click.
        /// </summary>
        /// <param name="sender">Obiekt eventu.</param>
        /// <param name="e">Argument eventu.</param>
        private void btSaveChanges_Click(object sender, EventArgs e)
        {
            double lastVal, newVal;
            Double.TryParse(textBox.ElementAt(textBox.Count - 2).Text, out lastVal);
            if (xmlRecords.RecordsCount > 0)
                if (Double.TryParse(textBox.ElementAt(textBox.Count - 1).Text, out newVal))
                {
                    if (lastVal <= newVal)
                    {
                        this.xmlRecords.counter[nrRecord].NewValue = textBox.ElementAt(textBox.Count - 1).Text;
                        SaveData = true;
                    }
                    else
                    {
                         DialogResult choiceFromMessageBox = MessageBox.Show("Nowy odczyt jest mniejszy niż poprzedni, czy chcesz zapisać dane?", "Ostrzeżenie", MessageBoxButtons.YesNo);
                         if (choiceFromMessageBox == DialogResult.Yes)
                         {
                             this.xmlRecords.counter[nrRecord].NewValue = textBox.ElementAt(textBox.Count - 1).Text;
                             SaveData = true;
                         }
                    }
                }
        }

        /// <summary>
        /// Przejście do następnego rekordu - Click (Menu).
        /// </summary>
        /// <param name="sender">Obiekt eventu.</param>
        /// <param name="e">Argument eventu.</param>
        private void rekordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btNext_Click(sender, e);
        }

        /// <summary>
        /// Przejście do poprzedniego rekordu - Click (Menu).
        /// </summary>
        /// <param name="sender">Obiekt eventu.</param>
        /// <param name="e">Argument eventu.</param>
        private void poprzedniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btPrevious_Click(sender, e);
        }

        /// <summary>
        /// Zapisz jako ... wybranie nowego pliku XML jako docelowy zapisu (aktualizacja metody this.path) - Click.
        /// </summary>
        /// <param name="sender">Obiekt eventu.</param>
        /// <param name="e">Argument eventu.</param>
        private void zapiszJakoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (xmlRecords.RecordsCount > 0)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = @"C:\";
                saveFileDialog.DefaultExt = "xml";
                saveFileDialog.Filter = "XML file|*.xml";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.Title = "Zapisz dane";
                DialogResult check = saveFileDialog.ShowDialog();

                if (saveFileDialog.FileName == String.Empty && check == DialogResult.OK)
                    MessageBox.Show("Błąd! Brak pliku!");
                else if (check == DialogResult.OK)
                {
                    this.path = saveFileDialog.FileName;
                    StaticXML.WriteToXml(this.path, xmlRecords, out SaveData);
                }
            }
            else
            {
                MessageBox.Show("Błąd! Brak danych!");
            }
        }
    }
}
