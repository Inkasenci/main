using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SZI
{
    /// <summary>
    /// Formularz pozwalający zarządzać odczytami.
    /// </summary>
    public partial class ReadingModifyForm : Form
    {
        /// <summary>
        /// Przekazany numer, który identyfikuje odczyt.
        /// </summary>
        private System.Guid editId = System.Guid.Empty;

        /// <summary>
        /// Kopia edytowanego rekordu.
        /// </summary>
        private Reading modifRecord;

        /// <summary>
        /// Teksty etykiet formularza.
        /// </summary>
        private string[] labelsTexts = new string[] { "Id odczytu: ", "Nr licznika: ", "Data odczytu: ", "Wartość odczytu: ", "Wykonawca: " };

        /// <summary>
        /// Nazwy pól tekstowych formularza.
        /// </summary>
        private string[] textBoxesNames = new string[] { "ReadingId", "CounterNo", "ReadingDate", "ReadingValue" };

        /// <summary>
        /// Teksty pól tekstowych formularza, czyli to, co jest wstawione w polach rekordu w momencie otwarcia formularza.
        /// </summary>
        private string[] textBoxesTexts;

        /// <summary>
        /// Dostępność pól tekstowych formularza, czyli określenie możliwości ich edycji.
        /// </summary>
        private bool[] textBoxEnable = new bool[] { false, false, false, true };

        /// <summary>
        /// Nazwy rozwijanych list formularza.
        /// </summary>
        private string comboBoxesNames = "cbCollector";

        /// <summary>
        /// Klucze rekordów wybranych w rozwijanych listach w momencie otwarcia formularza.
        /// </summary>
        private string comboBoxesKeys;

        /// <summary>
        /// Nazwy tabel, z których pochodzą klucze obce modyfikowanego rekordu.
        /// </summary>
        private string TableNames = "Collector";

        /// <summary>
        /// Konfiguracja ComboBoxa zawierającego listę inkasentów.
        /// </summary>
        private ComboBoxConfig CBConfig;

        /// <summary>
        /// Inicjalizuje formę pozwalającą zarządzać odczytami.
        /// </summary>
        /// <param name="id">Parametr identyfikujący odczyt.</param>
        public ReadingModifyForm(string id)
        {
            InitializeComponent();
            if (System.Guid.TryParse(id, out editId))
            {
                InitializeReading();
            }
            else
            {
                MessageBox.Show("Błędy rekord!");
            }
        }

        /// <summary>
        /// Funkcja pozwalająca inicjować formę odczyty.
        /// </summary>
        private void InitializeReading()
        {
            modifRecord = new Reading();
            using (var dataBase = new CollectorsManagementSystemEntities())
            {
                try
                {
                    var item = (from reading in dataBase.Readings
                                where reading.ReadingId == editId
                                select reading).FirstOrDefault();

                    {
                        modifRecord.CollectorId = item.CollectorId;
                        modifRecord.CounterNo = item.CounterNo;
                        modifRecord.Date = item.Date;
                        modifRecord.ReadingId = item.ReadingId;
                        modifRecord.Value = item.Value;
                    }

                    comboBoxesKeys = item.CollectorId;
                    textBoxesTexts = new string[] { item.ReadingId.ToString(), item.CounterNo.ToString(), item.Date.ToString(), item.Value.ToString() };

                    Label[] labels = InitializeLabels();
                    TextBox[] textBoxes = InitializeText();

                    CBConfig = new ComboBoxConfig(TableNames, comboBoxesNames, new Point(150, 30 * (5)), comboBoxesKeys);

                    this.Controls.AddRange(labels);
                    this.Controls.AddRange(textBoxes);
                    this.Controls.Add(CBConfig.comboBox);
                }
                catch
                {
                    MessageBox.Show("Błąd ładowania odczytu!");
                }
            }
        }

        /// <summary>
        /// Inicjalizuje etykiety.
        /// </summary>
        /// <returns>Zainicjalizowane etykiety.</returns>
        private Label[] InitializeLabels()
        {
            Label[] labels = new Label[labelsTexts.Length];
            for (int i = 0; i < labelsTexts.Length; i++)
            {
                labels[i] = new Label();
                labels[i].Text = labelsTexts[i];
                labels[i].Location = new Point(10, 30 * (i + 1));
            }
            return labels;
        }

        /// <summary>
        /// Inicjalizuje pola tekstowe.
        /// </summary>
        /// <returns>Zainicjalizowane pola tekstowe.</returns>
        private TextBox[] InitializeText()
        {
            TextBox[] textBoxes = new TextBox[textBoxesTexts.Length];

            for (int i = 0; i < textBoxesTexts.Length; i++)
            {
                textBoxes[i] = new TextBox();
                textBoxes[i].Name = textBoxesNames[i];
                textBoxes[i].Text = textBoxesTexts[i];
                textBoxes[i].Location = new Point(150, 30 * (i + 1));
                textBoxes[i].Enabled = textBoxEnable[i];
            }

            return textBoxes;
        }

        /// <summary>
        /// Wywoływana po naciśnięciu przycisku "Anuluj". Zamyka formularz bez zapisywania zmian.
        /// </summary>
        /// <param name="sender">Obiekt eventu.</param>
        /// <param name="e">Argument eventu.</param>
        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Wywoływana po naciśnięciu przycisku "Zapisz". Zamyka formularz uprzednio zapisując zmiany.
        /// </summary>
        /// <param name="sender">Obiekt eventu.</param>
        /// <param name="e">Argument eventu.</param>
        private void btSave_Click(object sender, EventArgs e)
        {
            double newValue;
            if (Double.TryParse(this.Controls.Find("ReadingValue", true)[0].Text, out newValue))
            {
                modifRecord.Value = newValue;
                if (CBConfig.ReturnForeignKey() == "")
                    modifRecord.CollectorId = "00000000000";
                else
                    modifRecord.CollectorId = CBConfig.ReturnForeignKey();
            }
            if (!modifRecord.UpdateDB())
            {
                MessageBox.Show("Nie udało się zaktualizować rekordu!");
            }
            this.Close();
        }
    }
}
