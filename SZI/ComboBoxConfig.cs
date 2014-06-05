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
        ///  Nazwa tabeli, której rekordy będą wyświetlane w rozwijanej liście.
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
        /// Lista obiektów reprezentujących elementy rozwijanej listy.
        /// </summary>
        List<ComboBoxItem> itemList;
        /// <summary>
        /// Napis przechowujący tekst filtrujący rekordy rozwijanej listy.
        /// </summary>
        string filter = string.Empty;
        /// <summary>
        /// Podpowiedź wyświetlająca aktualny tekst filtra.
        /// </summary>
        ToolTip filterTip;
        /// <summary>
        /// Szerokości kolumn rozwijanej listy.
        /// </summary>
        int[] widthsOfColumns;

        /// <summary>
        /// Inicjalizuje pola rozwijanej listy. Ustawia odpowiednie szerokości kolumn.
        /// </summary>
        /// <returns>Lista zainicjowanych pól. Elementy listy zawierają długi i krótki opis danego pola.</returns>
        private List<ComboBoxItem> InitializeItems()
        {
            List<ComboBoxItem> initializedItems = new List<ComboBoxItem>();
            IDataBase dataBase = null;
            int[] shortDescriptionWords = null;

            switch (tableName)
            {
                case "Collector":
                    dataBase = new Collectors();
                    shortDescriptionWords = new int[] { 1, 2 };
                    widthsOfColumns = new int[7];
                    break;
                case "Customer":
                    dataBase = new Customers();
                    shortDescriptionWords = new int[] { 1, 2 };
                    widthsOfColumns = new int[7];
                    break;
                case "Area":
                    dataBase = new Areas();
                    shortDescriptionWords = new int[] { 1 };
                    widthsOfColumns = new int[3];
                    break;
                case "Address":
                    dataBase = new Addresses();
                    shortDescriptionWords = new int[] { 1, 2, 3 };
                    widthsOfColumns = new int[4];
                    break;
            }

            List<string[]> itemList = dataBase.itemList;
            System.Drawing.Graphics g = comboBox.CreateGraphics();
            System.Drawing.Font f = comboBox.Font;

            string[] nullList = new string[widthsOfColumns.Length];
            for (int i = 0; i < nullList.Length; i++)
            {
                nullList[i] = String.Empty;
                widthsOfColumns[i] = 0;
            }

            initializedItems.Add(new ComboBoxItem(nullList, new int[] { 0 }));
            initializedItems.ElementAt(0).formattedLongItemDescription = String.Empty;

            foreach (string[] item in itemList)
            {

                ComboBoxItem comboBoxItem = new ComboBoxItem(item, shortDescriptionWords);

                for (int j = 0; j < widthsOfColumns.Length; j++)
                {
                    int newWidth = Convert.ToInt32(g.MeasureString(comboBoxItem.fields.ElementAt(j), f).Width);
                    if (newWidth > widthsOfColumns[j])
                        widthsOfColumns[j] = newWidth;
                }

                initializedItems.Add(comboBoxItem);
            }

            return initializedItems;
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
                newWidth = Convert.ToInt32(g.MeasureString(item.formattedLongItemDescription, f).Width);
                if (newWidth > maxWidth)
                    maxWidth = newWidth;
            }

            return maxWidth;
        }

        /// <summary>
        /// Zwraca klucz aktualnie wybranego rekordu.
        /// </summary>
        /// <returns>Klucz wybranego rekordu.</returns>
        public string ReturnForeignKey()
        {
            return itemList.ElementAt(comboBox.SelectedIndex).fields.ElementAt(0);
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
                    comboBox.Items.Add(item.formattedLongItemDescription);
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
        /// Znajduje na liście elementów rozwijanej listy obiekt będący odpowiednikiem zaznaczonego na rozwijanej liście rekordu.
        /// </summary>
        /// <param name="item">"Aktualnie badany element listy.</param>
        /// <returns>true - badany obiekt jest poszukiwanym obiektem.</returns>
        private bool FindItem(ComboBoxItem item)
        {
            if (comboBox.SelectedIndex >= 0 && comboBox.Items.Count > 0)
                if (item.formattedLongItemDescription == comboBox.Items[comboBox.SelectedIndex].ToString())
                    return true;
            return false;
        }

        /// <summary>
        /// Za pomocą tabulatorów formatuje długi opis rekordu rozwijanej listy tak, aby były wyświetlanie w kolumnach.
        /// </summary>
        /// <param name="fields">Tablica pól rekordu.</param>
        /// <returns>Sformatowany długi opis rekordu.</returns>
        private string FormatDescription(string[] fields)
        {
            string result = String.Empty;

            System.Drawing.Graphics g = comboBox.CreateGraphics();
            System.Drawing.Font f = comboBox.Font;

            int tabWidth = Convert.ToInt32(g.MeasureString("\t", f).Width);

            for (int i = 0; i < widthsOfColumns.Length; i++)
            {
                string field = fields[i];
                int fieldWidth = Convert.ToInt32(g.MeasureString(field, f).Width);
                result += field;

                while (fieldWidth <= widthsOfColumns[i])
                {
                    result += "\t";
                    field += "\t";
                    fieldWidth = Convert.ToInt32(g.MeasureString(field, f).Width);
                }
            }

            return result;
        }

        /// <summary>
        /// Konstruktor obiektu. Inicjalizuje pola klasy i właściwości rozwijanej listy konfigurowanej w ramach obiektu.
        /// </summary>
        /// <param name="tableName">Nazwa tabeli, której rekordy mają zasilić rozwijaną listę.</param>
        /// <param name="comboBoxName">Nazwa, która zostanie nadana tworzonej rozwijanej liście.</param>
        /// <param name="location">Położenie tworzonej rozwijanej listy na formularzu.</param>
        /// <param name="foreignKey">Jeśli lista ma mieć od razu wybrany element, będzie to właśnie ten, który posiada taki klucz.</param>
        public ComboBoxConfig(string tableName, string comboBoxName, System.Drawing.Point location, string foreignKey = "")
        {
            this.tableName = tableName;
            this.foreignKey = foreignKey;
            comboBox = new ComboBox();
            itemList = InitializeItems();

            foreach (ComboBoxItem item in itemList)
            {
                item.formattedLongItemDescription = FormatDescription(item.fields);
                comboBox.Items.Add(item.formattedLongItemDescription);
            }

            comboBox.Name = comboBoxName;
            comboBox.Location = location;
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox.DrawMode = DrawMode.OwnerDrawFixed;
            comboBox.DrawItem += comboBox_DrawItem;
            comboBox.DropDown += comboBox_DropDown;
            comboBox.DropDownClosed += comboBox_DropDownClosed;
            comboBox.LostFocus += comboBox_DropDownClosed;
            comboBox.SelectedIndex = 0;

            if (foreignKey != String.Empty)
            {
                for (int i = 0; i < itemList.Count; i++)
                {
                    if (itemList.ElementAt(i).fields.ElementAt(0) == foreignKey)
                    {
                        comboBox.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Wywoływana, aby narysować element rozwijanej listy.
        /// </summary>
        /// <param name="sender">Rozwijana lista konfigurowana w ramach obiektu.</param>
        /// <param name="e">Argumenty zdarzenia.</param>
        void comboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;

            e.DrawBackground();

            if (e.Index >= 0)
            {
                System.Drawing.Brush brush = new System.Drawing.SolidBrush(comboBox.ForeColor);
                string s;

                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                    brush = System.Drawing.SystemBrushes.HighlightText;

                if (comboBox.DroppedDown)
                {
                    if (filter == String.Empty)
                        s = itemList.ElementAt(e.Index).formattedLongItemDescription;
                    else
                        s = comboBox.Items[e.Index].ToString();
                }
                else
                    s = itemList.ElementAt(e.Index).shortItemDescription;

                e.Graphics.DrawString(s, comboBox.Font, brush, e.Bounds);
            }
        }

        /// <summary>
        /// Wywoływana, gdy podczas przeglądania rozwijanej listy zostanie naciśnięty klawisz, co uruchamia filtrowanie.
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
        /// Wywoływana, gdy rozwijana lista jest zwijana.
        /// </summary>
        /// <param name="sender">Rozwijana lista konfigurowana w ramach obiektu.</param>
        /// <param name="e">Argumenty zdarzenia.</param>
        private void comboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (filter != String.Empty)
            {
                int correctSelectedIndex = itemList.FindIndex(FindItem);
                comboBox.Items.Clear();

                foreach (ComboBoxItem item in itemList)
                    comboBox.Items.Add(item.formattedLongItemDescription);

                comboBox.SelectedIndex = correctSelectedIndex;
            }

            comboBox.KeyDown -= comboBox_KeyDown;
            filter = String.Empty;
            filterTip.Active = false;
        }

        /// <summary>
        /// Wywoływana, gdy rozwijana lista jest rozwijana. 
        /// </summary>
        /// <param name="sender">Rozwijana lista, której dotyczy zdarzenie.</param>
        /// <param name="e">Argumenty zdarzenia.</param>
        private void comboBox_DropDown(object sender, EventArgs e)
        {
            comboBox.DropDownWidth = AdjustComboBoxWidth();
            comboBox.KeyDown += comboBox_KeyDown;
            filterTip = new ToolTip();
            filterTip.Popup += filterTip_Popup;
        }

        /// <summary>
        /// Wywoływana, gdy pojawia się podpowiedź zawierająca tekst filtra. Dostosowywuje rozmiar podpowiedzi.
        /// </summary>
        /// <param name="sender">Podpowiedź wyświetlająca tekst filtra.</param>
        /// <param name="e">Argumenty zdarzenia.</param>
        void filterTip_Popup(object sender, PopupEventArgs e)
        {
            e.ToolTipSize = comboBox.Size;
        }
    }
}
