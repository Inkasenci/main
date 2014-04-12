using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SZI
{
    /**
     * Obiekt tej klasy pozwala stworzyć i umieścić w formularzu rozwijaną listę, wcześniej ją inicjując i definiując.
     * Każdy rekord posiada długi i krótki opis. Długi opis jest wykorzystywany, kiedy lista jest rozwinięta.
     * Krótki opis jest wyświetlany, kiedy lista jest zwinięta i posiada wybrany element.
     */
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

        //! Konwertuje tablicę napisów na jeden napis.
        //! \param recordFields Tablica napisów. Każdy z napisów jest polem rekordu.
        //! \return Napis będący połączonymi polami rekordu.
        private string ConvertRecordToString(string[] recordFields)
        {
            string convertedRecord = String.Empty;

            foreach (string recordField in recordFields)
                convertedRecord += recordField + " ";

            return convertedRecord;
        }

        
        //! Inicjalizuje pola rozwijanej listy.
        //! \return Lista zainicjowanych pól. Elementy listy zawierają długi i krótki opis danego pola.
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

        //! Wydobywa pierwsze pole rekordu z napisu będącego jego długim opisem.
        //! \param item Długi opis rekordu.
        //! \return Napis będący kluczem rekordu.
        private string MineForeignKey(string item)
        {
            return item.Substring(0, item.IndexOf(' '));
        }

        //! Wydobywa z długiego opisu rekordu słowa będące jego krótki opisem.
        //! \param item Długi opis rekordu.
        //! \return Napis będący krótkim opisem rekordu.
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

        //! Dopasowywuje szerokość rozwijanej listy po rozwinięciu do najszerszego jej elementu.
        //! \return Szerokość rozwijanej listy.
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

        //! Zwraca klucz aktualnie wybranego rekordu.
        //! \return Klucz wybranego rekordu
        public string ReturnForeignKey()
        {
            return MineForeignKey(itemList.ElementAt(comboBox.SelectedIndex).longItemDescription);
        }

        //! Zwraca konfigurowaną w ramach obiektu rozwijaną listę.
        //! \return Rozwijana lista.
        public ComboBox InitializeComboBox()
        {
            return this.comboBox;
        }

        //! Konstruktor obiektu. Inicjalizuje pola klasy i właściwości rozwijanej listy konfigurowanej w ramach obiektu.
        //! \param tableName Nazwa tabeli, której rekordy mają zasilić rozwijaną listę.
        //! \param comboBoxName Nazwa, która zostanie nadana tworzonej rozwijanej liście.
        //! \param location Położenie tworzonej rozwijanej listy na formularzu.
        //! \param foreignKey Jeśli lista ma mieć od razu wybrany element, będzie to właśnie ten, który posiada taki klucz.
        public ComboBoxConfig(string tableName, string comboBoxName, System.Drawing.Point location, string foreignKey = "")
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

        //! Wywoływana, gdy rozwijana lista jest zwijana. Przełącza aktualnie wybrany element z wyświetlania długiego opisu na opis krótki.
        //! \param sender Rozwijana lista, której dotyczy zdarzenie.
        //! \param e Argumenty zdarzenia.
        private void comboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (comboBox.SelectedIndex >= 0)
                comboBox.Items[comboBox.SelectedIndex] = itemList.ElementAt(comboBox.SelectedIndex).shortItemDescription;
        }

        //! Wywoływana, gdy rozwijana lista jest rozwijana. Przełącza aktualnie wybrany element z wyświetlania krótkiego opisu na opis długi, a także ustawia prawidłową szerokość listy.
        //! \param sender Rozwijana lista, której dotyczy zdarzenie.
        //! \param e Argumenty zdarzenia.
        private void comboBox_DropDown(object sender, EventArgs e)
        {
            comboBox.DropDownWidth = AdjustComboBoxWidth();
            if (comboBox.SelectedIndex >= 0)
                comboBox.Items[comboBox.SelectedIndex] = itemList.ElementAt(comboBox.SelectedIndex).longItemDescription;
        }
    }
}