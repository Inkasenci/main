using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SZI
{
    class ComboBoxConfig
    {
        string tableName; //nazwa tabeli, ktorej rekordy beda wyswietlanie w comboBox
        string foreignKey; //klucz elementu, ktory ma byc wybrany po otwarciu formularza z comboBox (potrzebne przy modyfikacji)
        public ComboBox comboBox; //zwracany comboBox
        int[] shortDescriptionWords; //numery slow rekordu, ktore maja byc uzyte jako skrocony opis rekordu (liczone od zera)
        List<ComboBoxItem> itemList; //rekordy, ich krotkie i dlugie opisy
        string filter = string.Empty;
        ToolTip filterTip;

        private string ConvertRecordToString(string[] recordFields)
        /*przyjmuje tablice stringow (pol rekordu)
         * zwraca pola polaczone w jeden string*/
        {
            string convertedRecord = String.Empty;

            foreach (string recordField in recordFields)
                convertedRecord += recordField + " ";

            return convertedRecord;
        }

        private List<ComboBoxItem> InitializeItems()
        //inicjalizuje rekordy comboBoxa, ich krotki i dlugi opis
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

        private string MineForeignKey(string item)
        /*przyjmuje dlugi opis rekordu
         * zwraca klucz rekordu*/
        {
            return item.Substring(0, item.IndexOf(' '));
        }

        private string MineDescriptionWords(string item)
        /*przyjmuje dlugi opis rekordu
         * zwraca krotki opis*/
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

        private int AdjustComboBoxWidth()
        //dopasowywuje szerokosc comboBox po rozwinieciu do najszerszego elementu
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

        public string ReturnForeignKey()
        //zwraca klucz rekordu, mozna wykorzystac ta metode, aby zapisac klucz w tabeli na koniec modyfikacji lub dodawania
        {
            return MineForeignKey(itemList.ElementAt(comboBox.SelectedIndex).longItemDescription);
        }

        public ComboBox InitializeComboBox()
        //zwraca zainicjalizowany comboBox
        {
            return this.comboBox;
        }

        private void FilterItems()
        {
            comboBox.Items.Clear();
            foreach (ComboBoxItem item in itemList)
            {
                if (ReplacePolishCharacters(item.longItemDescription.ToLower()).IndexOf(filter) != -1)
                    comboBox.Items.Add(item.longItemDescription);
            }
        }

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

        private void AddAllItems()
        {
            foreach (ComboBoxItem item in itemList)
            {
                comboBox.Items.Add(item.longItemDescription);

            }
        }

        private bool FindItem(ComboBoxItem item)
        {
            if (item.longItemDescription == comboBox.Items[comboBox.SelectedIndex].ToString())
                return true;
            else
                return false;
        }

        public ComboBoxConfig(string tableName, string comboBoxName, System.Drawing.Point location, string foreignKey = "")
        /*przyjmuje nazwe tabeli, ktorej rekordy beda w comboBox, nazwe comboBoxa, jego polozenie
         * ewentualnie przyjmuje tez klucz glowny rekordu, jesli comboBox powinien miec od razu cos ustawione*
         * inicjalizuje odpowiednie pola klasy i wykonuje podstawowe czynnosci*/
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

        private void comboBox_DropDownClosed(object sender, EventArgs e)
        //kiedy comboBox jest zwijany
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

        private void comboBox_DropDown(object sender, EventArgs e)
        //kiedy comboBox jest rozwijany
        {
            comboBox.DropDownWidth = AdjustComboBoxWidth();
            if (comboBox.SelectedIndex >= 0)
                comboBox.Items[comboBox.SelectedIndex] = itemList.ElementAt(comboBox.SelectedIndex).longItemDescription;

            comboBox.KeyDown += comboBox_KeyDown;
            filterTip = new ToolTip();
        }
    }
}
