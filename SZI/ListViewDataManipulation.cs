using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace SZI
{
    public static class ListViewDataManipulation
    {
        /// <summary>
        /// Metoda usuwająca rekordy w bazie danych odpowiadające zaznaczonym w przekazanym ListView itemom.
        /// </summary>
        /// <param name="listView">ListView którego zaznaczone itemy zostaną usunięte z bazy danych.</param>
        /// <param name="Table">Tabela z której zostaną usunięte rekordy.</param>
        /// <returns>Wartość mówiąca o tym czy użytkownik zdecydował się na usunięcie rekordów.</returns>
        public static bool DeleteItems(ListView listView, Tables Table)
        {
            List<string> ids = Auxiliary.CreateIdList(listView);
            bool idExists = false;
            string tableName = string.Empty;
            DialogResult choiceFromMessageBox = DialogResult.Yes;

            switch (Table)
            {
                case Tables.Collectors:
                    tableName = "Collector";
                    break;
                case Tables.Customers:
                    tableName = "Customer";
                    break;
                case Tables.Areas:
                    tableName = "Area";
                    break;
                case Tables.Addresses:
                    tableName = "Address";
                    break;
                case Tables.Counters:
                    tableName = "Counter";
                    break;
            }

            for (int i = 0; i < ids.Count; i++)
            {
                idExists = DBManipulator.IdExistsInOtherTable(tableName, ids.ElementAt(i));
                if (idExists)
                {
                    tableName = tableName.ToLower();
                    choiceFromMessageBox = MessageBox.Show(LangPL.IntegrityWarnings[tableName + "Removal"], "Ostrzeżenie", MessageBoxButtons.YesNo);
                    break;
                }
            }

            if (choiceFromMessageBox == DialogResult.Yes)
            {
                DBManipulator.DeleteFromDB(ids, Table, idExists);
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// Odświeża ListView w ConfigManagementForm.
        /// </summary>
        /// <param name="sender">Obiekt wywołujący metodę.</param>
        /// <param name="Table">Tabela do odświeżenia.</param>
        public static void RefreshListView(object sender, Tables Table = Tables.Collectors)
        {
            IForm form = (IForm)sender;

            if (form.Modified) //jeśli dokonano modyfikacji lub dodania rekordu, to odśwież listę
            {
                if (form.GetType() == typeof(ConfigManagementForm)) //jeśli naciśnięto przycisk odśwież na formie
                {
                    int i = 0;
                    if (ConfigManagementForm.dataBase != null)
                        foreach (var data in ConfigManagementForm.dataBase)
                        {
                            data.RefreshList();
                            ListViewConfig.ListViewRefresh(ConfigManagementForm.listView[i++], data.itemList);
                        }
                    else
                    {
                        ConfigManagementForm.dataBase = new IDataBase[5] { new Collectors(), new Customers(), new Areas(), new Counters(), new Addresses() };
                        foreach (var data in ConfigManagementForm.dataBase)                        
                            ListViewConfig.ListViewRefresh(ConfigManagementForm.listView[i++], data.itemList);
                        
                    }

                }
                else if (form.GetType() == typeof(InsertForm)) //jeśli wprowadzono rekord
                {
                    InsertForm insertForm = (InsertForm)form;
                    ConfigManagementForm.dataBase[(int)insertForm.InsertedTo].RefreshList();
                    ListViewConfig.ListViewRefresh(ConfigManagementForm.listView[(int)insertForm.InsertedTo], ConfigManagementForm.dataBase[(int)insertForm.InsertedTo].itemList);
                }
                else if (form.GetType() == typeof(ModifyForm))//zmodyfikowano rekord
                {
                    ModifyForm modifyForm = (ModifyForm)form;
                    RefreshNecessaryTables(modifyForm.Table);
                }
                else //usunięto rekord
                {
                    RefreshNecessaryTables(Table);
                }
            }
        }



        /// <summary>
        /// Odświeża ListView odpowiadające zmodyfikowanej tabeli, a także powiązane z nią ListView, które zawierają klucze obce.
        /// </summary>
        /// <param name="ModifiedTable">Zmodyfikowana tabela.</param>
        public static void RefreshNecessaryTables(Tables ModifiedTable)
        {
            switch (ModifiedTable)
            {
                case Tables.Collectors: //jeśli zmieniono coś w inkasentach, to odświez inkasentów, tereny i adresy
                    ConfigManagementForm.dataBase[(int)Tables.Collectors].RefreshList();
                    ListViewConfig.ListViewRefresh(ConfigManagementForm.listView[(int)Tables.Collectors], ConfigManagementForm.dataBase[(int)Tables.Collectors].itemList);

                    ConfigManagementForm.dataBase[(int)Tables.Areas].RefreshList();
                    ListViewConfig.ListViewRefresh(ConfigManagementForm.listView[(int)Tables.Areas], ConfigManagementForm.dataBase[(int)Tables.Areas].itemList);

                    ConfigManagementForm.dataBase[(int)Tables.Addresses].RefreshList();
                    ListViewConfig.ListViewRefresh(ConfigManagementForm.listView[(int)Tables.Addresses], ConfigManagementForm.dataBase[(int)Tables.Addresses].itemList);
                    break;

                case Tables.Customers: //jeśli zmieniono coś w klientach, to odświez klientów i liczniki
                    ConfigManagementForm.dataBase[(int)Tables.Customers].RefreshList();
                    ListViewConfig.ListViewRefresh(ConfigManagementForm.listView[(int)Tables.Customers], ConfigManagementForm.dataBase[(int)Tables.Customers].itemList);

                    ConfigManagementForm.dataBase[(int)Tables.Counters].RefreshList();
                    ListViewConfig.ListViewRefresh(ConfigManagementForm.listView[(int)Tables.Counters], ConfigManagementForm.dataBase[(int)Tables.Counters].itemList);
                    break;

                case Tables.Areas: //jeśli zmieniono coś w terenach, to odświez tereny, liczniki i adresy
                    ConfigManagementForm.dataBase[(int)Tables.Areas].RefreshList();
                    ListViewConfig.ListViewRefresh(ConfigManagementForm.listView[(int)Tables.Areas], ConfigManagementForm.dataBase[(int)Tables.Areas].itemList);

                    ConfigManagementForm.dataBase[(int)Tables.Counters].RefreshList();
                    ListViewConfig.ListViewRefresh(ConfigManagementForm.listView[(int)Tables.Counters], ConfigManagementForm.dataBase[(int)Tables.Counters].itemList);

                    ConfigManagementForm.dataBase[(int)Tables.Addresses].RefreshList();
                    ListViewConfig.ListViewRefresh(ConfigManagementForm.listView[(int)Tables.Addresses], ConfigManagementForm.dataBase[(int)Tables.Addresses].itemList);
                    break;

                case Tables.Counters:
                    ConfigManagementForm.dataBase[(int)Tables.Counters].RefreshList();
                    ListViewConfig.ListViewRefresh(ConfigManagementForm.listView[(int)Tables.Counters], ConfigManagementForm.dataBase[(int)Tables.Counters].itemList);
                    break;

                case Tables.Addresses:
                    ConfigManagementForm.dataBase[(int)Tables.Addresses].RefreshList();
                    ListViewConfig.ListViewRefresh(ConfigManagementForm.listView[(int)Tables.Addresses], ConfigManagementForm.dataBase[(int)Tables.Addresses].itemList);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Wywoływana po naciśnięciu przycisku "Modyfikuj". Otwiera formularz umożliwiający modyfikację zaznaczonego rekordu.
        /// </summary>
        /// <param name="sender">Przycisk "Modyfikuj".</param>
        /// <param name="e">Argumenty zdarzenia.</param>
        public static void ModifyRecord(ListView listView, Tables Table)
        {
            List<string> ids = Auxiliary.CreateIdList(listView);
            int selectedIndex = listView.SelectedIndices[0]; //index modyfikowanego itemu
            var modifyForm = new ModifyForm(ids, Table);
            modifyForm.FormClosing += modifyForm_FormClosing;
            modifyForm.ShowDialog();
            listView.HideSelection = false;

            if (selectedIndex < listView.Items.Count)
                listView.Items[selectedIndex].Selected = true;
        }

        /// <summary>
        /// Metoda wywoływana po zamknięciu ModifyForm.
        /// </summary>
        /// <param name="sender">ModifyForm wywołujące metodę.</param>
        /// <param name="e">Parametry zdarzenia.</param>
        static private void modifyForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            RefreshListView(sender);
        }

    }
}
