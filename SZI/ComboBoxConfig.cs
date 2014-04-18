using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SZI
{
    /// <summary>
    /// Obiekt tej klasy pozwala stworzyć i umieścić w formularzu rozwijaną listę, wcześniej ją inicjując i definiując.
    /// Każdy rekord posiada długi i krótki opis. Długi opis jest wykorzystywany, kiedy lista jest rozwinięta.
    /// Krótki opis jest wyświetlany, kiedy lista jest zwinięta i posiada wybrany element.
    /// </summary>
    class ComboBoxConfig
    {
        /// <summary>
        ///  Nazwa tabeli, której rekordy będą wyświetlanie w rozwijanej liście.
        /// </summary>
        string tableName;
        /// <summary>
        /// Klucz elementu, który ma być ustawiony jako wybrany, kiedy lista jest tworzona. 
        /// </summary>
        string foreignKey;
        /// <summary>
        /// Rozwijana lista, która jest tworzona w ramach obiektu.
        /// </summary>
        public ComboBox comboBox;
        /// <summary>
        /// Numery słów rekordu, które mają być użyte jako skrócony opis rekordu (jest on używany, gdy lista jest zwinięta).
        /// </summary>
        int[] shortDescriptionWords;
        /// <summary>
        /// Rekordy, ich krótkie i długie opisy.
        /// </summary>
        List<ComboBoxItem> itemList;
        /// <summary>
        /// Napis przechowując tekst filtrujący rekordy rozwijanej listy.
        /// </summary>
        /// <summary>
        /// Podpowiedź wyświetlając aktualny tekst filtra.
        /// </summary>

        /// <summary>
        /// Konwertuje tablicę napisów na jeden napis.
        /// </summary>
        /// <param name="recordFields">Tablica napisów. Każdy z napisów jest polem rekordu.</param>
        /// <returns>Napis będący połączonymi polami rekordu.</returns>
        private string ConvertRecordToString(string[] recordFields)
        {
            string convertedRecord = String.Empty;

            foreach (string recordField in recordFields)
                convertedRecord += recordField + " ";

            return convertedRecord;
        }

        /// <summary>
        /// Inicjalizuje pola rozwijanej listy.
        /// </summary>
        /// <returns>Lista zainicjowanych pól. Elementy listy zawierają długi i krótki opis danego pola.</returns>
        private List<ComboBoxItem> InitializeItems()
        {
            List<ComboBoxItem> initializedItems = new List<ComboBoxItem>();
            IDataBase dataBase;

            switch (tableName)
            {
                case "Collector":
                    dataBase = new Collectors();
                    shortDescriptionWords = new int[] { 1, 2 };
                    break;
                case "Customer":
                    dataBase = new Customers();
                    shortDescriptionWords = new int[] { 1, 2 };
                    break;
                case "Area":
                    dataBase = new Areas();
                    shortDescriptionWords = new int[] { 1 };
                    break;
                case "Address":
                    dataBase = new Addresses();
                    shortDescriptionWords = new int[] { 1, 2 };
                    break;
                default:
                    dataBase = new Collectors();
                    break;
            }

            List<string[]> itemList = dataBase.itemList;
            string convertedRecord, descriptionWords;

            initializedItems.Add(new ComboBoxItem(" ", "")); //dodanie pustego rekordu, przydatne gdy wybór rekordu jest opcjonalny

            foreach (string[] item in itemList)
            {
                convertedRecord = ConvertRecordToString(item);
                descriptionWords = MineDescriptionWords(convertedRecord);
                ComboBoxItem comboBoxItem = new ComboBoxItem(convertedRecord, descriptionWords);
                initializedItems.Add(comboBoxItem);
            }

            return initializedItems;
        }

        /// <summary>
        /// Wydobywa pierwsze pole rekordu z napisu będącego jego długim opisem.
        /// </summary>
        /// <param name="item">Długi opis rekordu.</param>
        /// <returns>Napis będący kluczem rekordu.</returns>
        private string MineForeignKey(string item)
        {
            return item.Substring(0, item.IndexOf(' '));
        }

        /// <summary>
        /// Wydobywa z długiego opisu rekordu słowa będące jego krótki opisem.
        /// </summary>
        /// <param name="item">Długi opis rekordu.</param>
        /// <returns>Napis będący krótkim opisem rekordu.</returns>
        private string MineDescriptionWords(string item)
        {
            string minedDescription = String.Empty;

            List<int> newWord = new List<int>();
            newWord.Add(0);
            for (int i = 0; i < item.Length; i++)
                if (item[i] == ' ')
                    newWord.Add(i + 1);
            foreach (int descriptionWord in shortDescriptionWords)
                minedDescription += item.Substring(newWord.ElementAt(descriptionWord), newWord.ElementAt(descriptionWord + 1) - newWord.ElementAt(descriptionWord));

            return minedDescription;
        }

        /// <summary>
        /// Dopasowywuje szerokość rozwijanej listy po rozwinięciu do najszerszego jej elementu.
        /// </summary>
        /// <returns>Szerokość rozwijanej listy.</returns>
        private int AdjustComboBoxWidth()
        {
            System.Drawing.Graphics g = comboBox.CreateGraphics();
            System.Drawing.Font f = comboBox.Font;

            int maxWidth = 1;
            int newWidth = 0;
            foreach (ComboBoxItem item in itemList)
            {
                newWidth = Convert.ToInt32(g.MeasureString(item.longItemDescription, f).Width);
                if (newWidth > maxWidth)
                    maxWidth = newWidth;
            }

            return maxWidth;
        }

        /// <summary>
        /// Zwraca klucz aktualnie wybranego rekordu.
        /// </summary>
        /// <returns>Klucz wybranego rekordu</returns>
        public string ReturnForeignKey()
        {
            return MineForeignKey(itemList.ElementAt(comboBox.SelectedIndex).longItemDescription);
        }

        /// <summary>
        /// Zwraca konfigurowaną w ramach obiektu rozwijaną listę.
        /// </summary>
        /// <returns>Rozwijana lista.</returns>
        public ComboBox InitializeComboBox()
        {
            return this.comboBox;
        }

        /// <summary>
        /// Filtruje rekordy według aktualnej zawartości tekstu filtra. Ignoruje polskie znaki i wielkość liter.
        /// </summary>
        private void FilterItems()
        {
            comboBox.Items.Clear();
            foreach (ComboBoxItem item in itemList)
            {
                if (ReplacePolishCharacters(item.longItemDescription.ToLower()).IndexOf(filter) != -1)
                    comboBox.Items.Add(item.longItemDescription);
            }
        }

        /// <summary>
        /// Zamienia polskie znaki na wersje bez "ogonków".
        /// </summary>
        /// <param name="currentString">Napis, w którym ma zostąc dokonana zamiana.</param>
        /// <returns>Napis po zamianie.</returns>
        private string ReplacePolishCharacters(string currentString)
        {
            string result = currentString;

            result = result.Replace("ą", "a");
            result = result.Replace("ć", "c");
            result = result.Replace("ę", "e");
            result = result.Replace("ł", "l");
            result = result.Replace("ń", "n");
            result = result.Replace("ó", "o");
            result = result.Replace("ś", "s");
            result = result.Replace("ź", "z");
            result = result.Replace("ż", "z");

            return result;
        }

        /// <summary>
        /// Dodaje do rozwijanej listy wszystkie elementy przechowywane na liście elementów obiektu.
        /// </summary>
        private void AddAllItems()
        {
            foreach (ComboBoxItem item in itemList)
            {
                comboBox.Items.Add(item.longItemDescription);

            }
        }

        /// <summary>
        /// Znajduje na liście elementów obiektu obiekt będący odpowiednikiem zaznaczonego na rozwijanej liście rekordu.
        /// </summary>
        /// <param name="item">"Aktualnie badany element listy.</param>
        /// <returns>true - badany obiekt jest poszukiwanym obiektem.</returns>
        private bool FindItem(ComboBoxItem item)
        {
            if (item.longItemDescription == comboBox.Items[comboBox.SelectedIndex].ToString())
                return true;
            else
                return false;
        }

        public ComboBoxConfig(string tableName, string comboBoxName, System.Drawing.Point location, string foreignKey = "")
        {
            this.tableName = tableName;
            this.foreignKey = foreignKey;
            comboBox = new ComboBox();

            itemList = InitializeItems();
            AddAllItems();

            comboBox.Name = comboBoxName;
            comboBox.Location = location;
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox.DropDown += comboBox_DropDown;
            comboBox.DropDownClosed += comboBox_DropDownClosed;
            comboBox.SelectedIndex = 0;

            if (foreignKey != String.Empty)
            {
                for (int i = 0; i < itemList.Count; i++)
                {
                    if (MineForeignKey(itemList.ElementAt(i).longItemDescription) == foreignKey)
                    {
                        comboBox.SelectedIndex = i;
                        comboBox.Items[comboBox.SelectedIndex] = itemList.ElementAt(i).shortItemDescription;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Wywoływana, gdy podczas przeglądania rozwijanej listy zostanie naciśnięty klawisz.
        /// </summary>
        /// <param name="sender">Rozwijana lista konfigurowana w ramach obiektu.</param>
        /// <param name="e">Argumenty zdarzenia.</param>
        void comboBox_KeyDown(object sender, KeyEventArgs e)
        {
            int keyValue = e.KeyValue;
            Keys keyCode = e.KeyCode;

            if (keyValue == 32 || (keyValue >= 65 && keyValue <= 90) || (keyValue >= 48 && keyValue <= 57))
                filter += Char.ToLower(Convert.ToChar(keyValue));
            else
                if (keyCode == Keys.Back)
                {
                    if (filter != String.Empty)
                        filter = filter.Remove(filter.Length - 1);
                }

            if (filter != String.Empty)
            {
                filterTip.Show("Filtr: " + filter, comboBox, new System.Drawing.Point(0, 0));
            }
            else
                filterTip.Hide(comboBox);

            FilterItems();
        }

	/// <summary>
        private void comboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (filter != String.Empty)
            {
                int correctSelectedIndex = itemList.FindIndex(FindItem);
                comboBox.Items.Clear();
                AddAllItems();
                comboBox.SelectedIndex = correctSelectedIndex;
            }
            
            if (comboBox.SelectedIndex >= 0)
                comboBox.Items[comboBox.SelectedIndex] = itemList.ElementAt(comboBox.SelectedIndex).shortItemDescription;

            comboBox.KeyDown -= comboBox_KeyDown;
            filter = String.Empty;
            filterTip.Active = false;
        }

        /// <summary>
        /// Wywoływana, gdy rozwijana lista jest rozwijana. Przełącza aktualnie wybrany element z wyświetlania krótkiego opisu na opis długi, a także ustawia prawidłową szerokość listy.
        /// </summary>
        /// <param name="sender">Rozwijana lista, której dotyczy zdarzenie.</param>
        /// <param name="e">Argumenty zdarzenia.</param>
        private void comboBox_DropDown(object sender, EventArgs e)
        {
            comboBox.DropDownWidth = AdjustComboBoxWidth();
            if (comboBox.SelectedIndex >= 0)
                comboBox.Items[comboBox.SelectedIndex] = itemList.ElementAt(comboBox.SelectedIndex).longItemDescription;

            comboBox.KeyDown += comboBox_KeyDown;
            filterTip = new ToolTip();
        }
    }
}
