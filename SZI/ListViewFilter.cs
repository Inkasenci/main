using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace SZI
{
    /// <summary>
    /// Filtr dla kontrolek typu listView. Każda listView ma swój indywidualny obiekt filtra.
    /// Dla każdej kolumny listView istnieje pole tekstowe, które pozwala filtrować zawartość kolumny.
    /// </summary>
    public class ListViewFilter
    {
        /// <summary>
        /// Indeks karty, z której pochodzi filtrowana listView.
        /// </summary>
        int tabPageIndex;
        /// <summary>
        /// Tablica przechowująca nagłówki kolumn listView, które wyświetlanie są potem w etykietach pól filtra.
        /// </summary>
        string[] fieldsNames;
        /// <summary>
        /// Tablica pól tekstowych, które pozwalają na ustawianie filtra. 
        /// </summary>
        TextBox[] filterTextBoxes;

        /// <summary>
        /// Inicjalizuje etykiety filtra.
        /// </summary>
        /// <returns>Zainicjalizowane etykiety.</returns>
        Label[] InitializeLabels()
        {
            Label[] labels = new Label[fieldsNames.Length];

            for (int i = 0; i < labels.Length; i++)
            {
                labels[i] = new Label();
                labels[i].Text = fieldsNames[i] + ": ";
                labels[i].Location = new Point(10, 30 * (i + 1));
                labels[i].AutoSize = true;
            }

            return labels;
        }

        /// <summary>
        /// Inicjalzuje pola tekstowe, dzięki którym można filtrować listView.
        /// </summary>
        /// <returns>Zainicjalizowane pola tekstowe.</returns>
        TextBox[] InitializeTextBoxes()
        {
            TextBox[] textBoxes = new TextBox[fieldsNames.Length];

            for (int i = 0; i < textBoxes.Length; i++)
            {
                textBoxes[i] = new TextBox();
                textBoxes[i].Location = new Point(125, 30 * (i + 1));
                textBoxes[i].TextChanged += FilterTextBox_TextChanged;
            }

            return textBoxes;
        }

        /// <summary>
        /// Inicjalizuje cały panel filtra, jego etykiety, pola tekstowe oraz przycisk je czyszczący.
        /// </summary>
        /// <returns>Panel filtra.</returns>
        public GroupBox InitializeFilter()
        {
            Button clearTextBoxesButton = new Button();
            clearTextBoxesButton.Text = "Wyczyść pola filtra";
            clearTextBoxesButton.AutoSize = true;
            clearTextBoxesButton.Location = new Point(75, (fieldsNames.Length + 1) * 30);
            clearTextBoxesButton.Click += clearTextBoxesButton_Click;
            GroupBox groupBox = new GroupBox();
            groupBox.Text = "Filtr";
            groupBox.Location = new Point(625, 10);
            groupBox.Size = new Size(250, (fieldsNames.Length + 2) * 30);

            foreach (Label label in InitializeLabels())
                groupBox.Controls.Add(label);

            foreach (TextBox textBox in filterTextBoxes)
                groupBox.Controls.Add(textBox);

            groupBox.Controls.Add(clearTextBoxesButton);

            return groupBox;
        }

        /// <summary>
        /// Jeśli przynajmniej jedno z pól tekstowych filtra nie jest puste, dokonuje filtrowania listView.
        /// W przeciwnym razie dodaje wszystkie elementy odpowiedniej tabeli do listView. Dokonuje dopasowania szerokości kolumn listView do ich zawartości.
        /// </summary>
        public void FilterRecords()
        {
            bool filteringNecessary = false;
            ListView filteredListView = ConfigManagementForm.listView[tabPageIndex];
            List<string[]> records = ConfigManagementForm.dataBase[tabPageIndex].itemList;

            filteredListView = ListViewConfig.ClearListView(filteredListView);

            foreach (TextBox filterTextBox in filterTextBoxes)
                if (filterTextBox.Text != String.Empty)
                {
                    filteringNecessary = true;
                    break;
                }

            if (filteringNecessary)
            {
                foreach (string[] record in records)
                {
                    bool recordPassesThroughFilter = true;

                    for (int i = 0; i < filterTextBoxes.Length; i++)
                        if (record[i].ToLower().IndexOf(filterTextBoxes[i].Text.ToLower()) == -1)
                        {
                            recordPassesThroughFilter = false;
                            break;
                        }

                    if (recordPassesThroughFilter)
                        ListViewConfig.AddItem(filteredListView, record);
                }
            }
            else
                foreach (string[] record in records)
                    ListViewConfig.AddItem(filteredListView, record);

            ListViewConfig.AdjustColumnWidth(filteredListView);
        }

        /// <summary>
        /// Konstruktor obiektu. Inicjalizuje jego pola.
        /// </summary>
        /// <param name="tabPageIndex">Numer karty, której listView ma być filtrowane.</param>
        /// <param name="fieldsNames">Nagłówki kolumn filtrowanego listView.</param>
        public ListViewFilter(int tabPageIndex, string[] fieldsNames)
        {
            this.tabPageIndex = tabPageIndex;
            this.fieldsNames = fieldsNames;
            filterTextBoxes = InitializeTextBoxes();
        }

        /// <summary>
        /// Wywoływana, gdy zmieni się zawartość pola tekstowego pełniącego rolę filtra. Dokonuje filtrowania.
        /// </summary>
        /// <param name="sender">Pole tekstowe należące do filtra.</param>
        /// <param name="e">Argumenty zdarzenia.</param>
        void FilterTextBox_TextChanged(object sender, EventArgs e)
        {
            FilterRecords();
        }

        /// <summary>
        /// Wywoływana po naciśnięciu przycisku "Wyczyść pola filtra". Czyści pola filtra.
        /// </summary>
        /// <param name="sender">Przycisk "Wyczyść pola filtra".</param>
        /// <param name="e">Argumenty zdarzenia.</param>
        void clearTextBoxesButton_Click(object sender, EventArgs e)
        {
            foreach (TextBox filterTextBox in filterTextBoxes)
                filterTextBox.Text = String.Empty;
        }
    }
}
