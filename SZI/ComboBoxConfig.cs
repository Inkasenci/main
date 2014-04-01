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
        string tableName;
        string foreignKey;
        ComboBox comboBox;
        int[] shortDescriptionWords;
        List<ComboBoxItem> itemList;

        private string ConvertRecordToString(string[] recordFields)
        {
            string convertedRecord = String.Empty;

            foreach (string recordField in recordFields)
                convertedRecord += recordField + " ";

            return convertedRecord;
        }

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
                default:
                    dataBase = new Collectors();
                    break;
            }

            List<string[]> itemList = dataBase.itemList;
            string convertedRecord, descriptionWords;

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
        {
            return item.Substring(0, item.IndexOf(' '));
        }

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

        private int AdjustComboBoxWidth()
        {
            System.Drawing.Graphics g = comboBox.CreateGraphics();
            System.Drawing.Font f = comboBox.Font;

            int maxWidth = 0;
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
        {
            return MineForeignKey(itemList.ElementAt(comboBox.SelectedIndex).longItemDescription);
        }

        public ComboBox InitializeComboBox()
        {
            return this.comboBox;
        }

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
        {
            comboBox.Items[comboBox.SelectedIndex] = itemList.ElementAt(comboBox.SelectedIndex).shortItemDescription;
        }

        private void comboBox_DropDown(object sender, EventArgs e)
        {
            comboBox.DropDownWidth = AdjustComboBoxWidth();

            comboBox.Items[comboBox.SelectedIndex] = itemList.ElementAt(comboBox.SelectedIndex).longItemDescription;
        }
    }
}