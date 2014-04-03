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
        ComboBox comboBox; //zwracany comboBox
        int[] shortDescriptionWords; //numery slow rekordu, ktore maja byc uzyte jako skrocony opis rekordu (liczone od zera)
        List<ComboBoxItem> itemList; //rekordy, ich krotkie i dlugie opisy

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
                default:
                    dataBase = new Collectors();
                    break;
            }

            List<string[]> itemList = dataBase.itemList;
            string convertedRecord, descriptionWords;

            initializedItems.Add(new ComboBoxItem(" ", ""));

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

        public ComboBoxConfig(string tableName, string comboBoxName, System.Drawing.Point location, string foreignKey = "")
        /*przyjmuje nazwe tabeli, ktorej rekordy beda w comboBox, nazwe comboBoxa, jego polozenie
         * ewentualnie przyjmuje tez klucz glowny rekordu, jesli comboBox powinien miec od razu cos ustawione*
         * inicjalizuje odpowiednie pola klasy i wykonuje podstawowe czynnosci*/
        {
            this.tableName = tableName;
            this.foreignKey = foreignKey;
            comboBox = new ComboBox();

            itemList = InitializeItems();
            foreach (ComboBoxItem item in itemList)
                comboBox.Items.Add(item.longItemDescription);

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

        private void comboBox_DropDownClosed(object sender, EventArgs e)
        //kiedy comboBox jest zwijany
        {
            if (comboBox.SelectedIndex >= 0)
                comboBox.Items[comboBox.SelectedIndex] = itemList.ElementAt(comboBox.SelectedIndex).shortItemDescription;
        }

        private void comboBox_DropDown(object sender, EventArgs e)
        //kiedy comboBox jest rozwijany
        {
            comboBox.DropDownWidth = AdjustComboBoxWidth();
            if (comboBox.SelectedIndex >= 0)
                comboBox.Items[comboBox.SelectedIndex] = itemList.ElementAt(comboBox.SelectedIndex).longItemDescription;
        }
    }
}