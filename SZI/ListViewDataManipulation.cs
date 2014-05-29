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
        /// Wypełnia otwarty w danym momencie ListView i dopisuje go do tablicy IDataBase.
        /// </summary>
        public static void ComplementListView(ConfigManagementForm MainForm)
        {
            switch (ConfigManagementForm.selectedTab)
            {
                case Tables.Collectors:
                    ConfigManagementForm.dataBase[(int)Tables.Collectors] = new Collectors();
                    ListViewConfig.ListViewRefresh(ConfigManagementForm.listView[(int)Tables.Collectors], ConfigManagementForm.dataBase[(int)Tables.Collectors].itemList);
                    break;

                case Tables.Customers:
                    ConfigManagementForm.dataBase[(int)Tables.Customers] = new Customers();
                    ListViewConfig.ListViewRefresh(ConfigManagementForm.listView[(int)Tables.Customers], ConfigManagementForm.dataBase[(int)Tables.Customers].itemList);
                    break;

                case Tables.Areas:
                    ConfigManagementForm.dataBase[(int)Tables.Areas] = new Areas();
                    ListViewConfig.ListViewRefresh(ConfigManagementForm.listView[(int)Tables.Areas], ConfigManagementForm.dataBase[(int)Tables.Areas].itemList);
                    break;

                case Tables.Counters:
                    ConfigManagementForm.dataBase[(int)Tables.Counters] = new Counters();
                    ListViewConfig.ListViewRefresh(ConfigManagementForm.listView[(int)Tables.Counters], ConfigManagementForm.dataBase[(int)Tables.Counters].itemList);
                    break;

                case Tables.Addresses:
                   ConfigManagementForm.dataBase[(int)Tables.Addresses] = new Addresses();
                   ListViewConfig.ListViewRefresh(ConfigManagementForm.listView[(int)Tables.Addresses], ConfigManagementForm.dataBase[(int)Tables.Addresses].itemList);
                   break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Wypełnia otwarty w danym momencie ListView i tworzy tablicę IDataBase.
        /// </summary>
        private static void FillListView(ConfigManagementForm MainForm)
        {
            switch (ConfigManagementForm.selectedTab)
            {
                case Tables.Collectors:
                    ConfigManagementForm.dataBase = new IDataBase[5] { new Collectors(), null, null, null, null };
                    ListViewConfig.ListViewRefresh(ConfigManagementForm.listView[(int)Tables.Collectors], ConfigManagementForm.dataBase[(int)Tables.Collectors].itemList);
                    break;

                case Tables.Customers:
                    ConfigManagementForm.dataBase = new IDataBase[5] { null, new Customers(), null, null, null };
                    ListViewConfig.ListViewRefresh(ConfigManagementForm.listView[(int)Tables.Customers], ConfigManagementForm.dataBase[(int)Tables.Customers].itemList);
                    break;

                case Tables.Areas:
                    ConfigManagementForm.dataBase = new IDataBase[5] { null, null, new Areas(), null, null };
                    ListViewConfig.ListViewRefresh(ConfigManagementForm.listView[(int)Tables.Areas], ConfigManagementForm.dataBase[(int)Tables.Areas].itemList);
                    break;

                case Tables.Counters:
                    ConfigManagementForm.dataBase = new IDataBase[5] { null, null, null, new Counters(), null };
                    ListViewConfig.ListViewRefresh(ConfigManagementForm.listView[(int)Tables.Counters], ConfigManagementForm.dataBase[(int)Tables.Counters].itemList);
                    break;

                case Tables.Addresses:
                    ConfigManagementForm.dataBase = new IDataBase[5] { null, null, null, null, new Addresses() };
                    ListViewConfig.ListViewRefresh(ConfigManagementForm.listView[(int)Tables.Addresses], ConfigManagementForm.dataBase[(int)Tables.Addresses].itemList);
                    break;

                default:
                    break;
            }
            ConfigManagementForm.ListViewFilled[(int)ConfigManagementForm.selectedTab] = true;
        }

        private delegate void UpdateProgressStatusDelegate(ConfigManagementForm MainForm, int i, int max);

        private static void UpdateProgressStatus(ConfigManagementForm MainForm, int i, int max)
        {
            String ProgressStatusName = "progressStatusMain";
            ProgressStatusStrip progressStatus = MainForm.Controls.Find(ProgressStatusName, true)[0] as ProgressStatusStrip;
            if (progressStatus.InvokeRequired)
            {
                UpdateProgressStatusDelegate del = new UpdateProgressStatusDelegate(UpdateProgressStatus);
                progressStatus.Invoke(del, progressStatus, i, max);
                return;
            }
            progressStatus.Value = (i / (float)max) * 100;
            Application.DoEvents();
        }

        /// <summary>
        /// Odświeża wszystkie ListView, które były wcześniej wypełnione.
        /// </summary>
        private static void RefreshFilledListViews(ConfigManagementForm MainForm)
        {
            int ListViewsToUpdate = 0;

            for (int i = 0; i < ConfigManagementForm.ListViewFilled.Count(); i++)
                if (ConfigManagementForm.ListViewFilled[i] == true)
                    ListViewsToUpdate++;

            for (int i = 0; i < ConfigManagementForm.ListViewFilled.Count(); i++)
            {
                MainForm.UpdateStatusLabel(i);
                UpdateProgressStatus(MainForm, i, ListViewsToUpdate);
                if (ConfigManagementForm.dataBase[i] != null)
                {
                    ConfigManagementForm.dataBase[i].RefreshList();
                    ListViewConfig.ListViewRefresh(ConfigManagementForm.listView[i], ConfigManagementForm.dataBase[i].itemList);
                }
            }
            MainForm.UpdateStatusLabel(-1);
            UpdateProgressStatus(MainForm, 0, ListViewsToUpdate);
        }

        /// <summary>
        /// Odświeża ListView w ConfigManagementForm.
        /// </summary>
        /// <param name="sender">Obiekt wywołujący metodę.</param>
        /// <param name="Table">Tabela do odświeżenia.</param>
        public static void RefreshListView(object sender, Tables Table = Tables.Collectors)
        {
            if (sender.GetType() == typeof(Button)) //usunięto rekord
            {
                RefreshNecessaryTables(Table);
                return;
            }

            IForm form = (IForm)sender;
            
            if (form.Modified) //jeśli dokonano modyfikacji lub dodania rekordu, to odśwież listę
            {
                if (form.GetType() == typeof(ConfigManagementForm)) //jeśli naciśnięto przycisk odśwież na formie
                {
                    if (ConfigManagementForm.dataBase != null)
                        RefreshFilledListViews((ConfigManagementForm)form);
                    else
                        FillListView((ConfigManagementForm)form);
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

                    if (ConfigManagementForm.ListViewFilled[(int)Tables.Areas])
                    {
                        ConfigManagementForm.dataBase[(int)Tables.Areas].RefreshList();
                        ListViewConfig.ListViewRefresh(ConfigManagementForm.listView[(int)Tables.Areas], ConfigManagementForm.dataBase[(int)Tables.Areas].itemList);
                    }

                    if (ConfigManagementForm.ListViewFilled[(int)Tables.Addresses])
                    {
                        ConfigManagementForm.dataBase[(int)Tables.Addresses].RefreshList();
                        ListViewConfig.ListViewRefresh(ConfigManagementForm.listView[(int)Tables.Addresses], ConfigManagementForm.dataBase[(int)Tables.Addresses].itemList);
                    }
                    break;

                case Tables.Customers: //jeśli zmieniono coś w klientach, to odświez klientów i liczniki
                    ConfigManagementForm.dataBase[(int)Tables.Customers].RefreshList();
                    ListViewConfig.ListViewRefresh(ConfigManagementForm.listView[(int)Tables.Customers], ConfigManagementForm.dataBase[(int)Tables.Customers].itemList);

                    if (ConfigManagementForm.ListViewFilled[(int)Tables.Counters])
                    {
                        ConfigManagementForm.dataBase[(int)Tables.Counters].RefreshList();
                        ListViewConfig.ListViewRefresh(ConfigManagementForm.listView[(int)Tables.Counters], ConfigManagementForm.dataBase[(int)Tables.Counters].itemList);
                    }
                    break;

                case Tables.Areas: //jeśli zmieniono coś w terenach, to odświez tereny, liczniki i adresy
                    ConfigManagementForm.dataBase[(int)Tables.Areas].RefreshList();
                    ListViewConfig.ListViewRefresh(ConfigManagementForm.listView[(int)Tables.Areas], ConfigManagementForm.dataBase[(int)Tables.Areas].itemList);

                    if (ConfigManagementForm.ListViewFilled[(int)Tables.Counters])
                    {
                        ConfigManagementForm.dataBase[(int)Tables.Counters].RefreshList();
                        ListViewConfig.ListViewRefresh(ConfigManagementForm.listView[(int)Tables.Counters], ConfigManagementForm.dataBase[(int)Tables.Counters].itemList);
                    }

                    if (ConfigManagementForm.ListViewFilled[(int)Tables.Addresses])
                    {
                        ConfigManagementForm.dataBase[(int)Tables.Addresses].RefreshList();
                        ListViewConfig.ListViewRefresh(ConfigManagementForm.listView[(int)Tables.Addresses], ConfigManagementForm.dataBase[(int)Tables.Addresses].itemList);
                    }
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
        /// <param name="listView">ListView w którym dokonano modyfikacji.</param>
        /// <param name="Table">Odpowiadająca modyfikowanemu ListView tabela.</param>
        public static void ModifyRecord(ListView listView, Tables Table)
        {
            List<string> ids = Auxiliary.CreateIdList(listView);
            int selectedIndex = listView.SelectedIndices[0]; //index modyfikowanego itemu
            var modifyForm = new ModifyForm(ids, Table);
            modifyForm.ShowDialog();
            listView.HideSelection = false;

            if (selectedIndex < listView.Items.Count)
                listView.Items[selectedIndex].Selected = true;
        }
    }
}
