using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SZI
{
    public delegate bool ValidatingMethod(string S);

    public enum Tables { Collectors, Customers, Areas, Counters, Addresses};

    

    public static class Auxiliary
    {
        /// <summary>
        /// Strona główna twórców programu.
        /// </summary>
        public static string MainPageURL = "https://github.com/Inkasenci/main/wiki";


        /// <summary>
        /// Zwraca listę identyfikatorów związanych z zaznaczonymi w przekazanej ListView itemami.
        /// </summary>
        /// <param name="listView">ListView którego identyfikatory zaznaczonych itemów zostaną stworzone.</param>
        /// <returns>Lista identyfikatorów zaznaczonych itemów.</returns>
        public static List<string> CreateIdList(ListView listView)
        {
            List<string> ids = new List<string>();

            ListView.SelectedListViewItemCollection selectedItems = listView.SelectedItems;
            foreach (ListViewItem item in selectedItems)
            {
                ids.Add(item.SubItems[0].Text);
            }

            return ids;
        }

        /// <summary>
        /// Kopiuje zaznaczone elementy listy do schowka.
        /// </summary>
        /// <param name="sender">ListView, której elementy zostaną skopiowane.</param>
        /// <param name="e">Nieistotny parametr, niezbędny do przypisania metody do EventHandlera ToolStripItemu.</param>
        public static void CopyItemstoClipboard(object sender, EventArgs e)
        {
            string clipboard = string.Empty;
            ListView lv = (ListView)sender;

            if (lv.SelectedItems.Count > 0)
            {
                for (int i = 0; i < lv.SelectedItems.Count; i++)
                {
                    for (int j = 0; j < lv.SelectedItems[i].SubItems.Count; j++)
                    {
                        clipboard += lv.SelectedItems[i].SubItems[j].Text;
                        if (j != lv.SelectedItems[i].SubItems.Count - 1)
                            clipboard += '\t';                            
                    }

                    clipboard += "\n";
                }
                Clipboard.SetText(clipboard);
            }
        }

        public static bool IsCurrentValueOK(Dictionary<Control, bool> Dict)
        {
            Dictionary<Control, bool>.ValueCollection valueColl = Dict.Values;
            foreach (bool b in valueColl)
                if (!b) return false;

            return true;
        }
        
        public static Guid ToGuid(int value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }

        public static void SetErrorProvider(ErrorProvider ep, Control tb)
        {
            ep.SetIconAlignment(tb, ErrorIconAlignment.MiddleRight);
            ep.SetIconPadding(tb, 2);
        }

        public static ErrorProvider InitializeErrorProvider(Control tb)
        {
            ErrorProvider ep = new ErrorProvider();
            ep.SetIconAlignment(tb, ErrorIconAlignment.MiddleRight);
            ep.SetIconPadding(tb, 2);
            ep.BlinkRate = 750;
            ep.BlinkStyle = ErrorBlinkStyle.AlwaysBlink;
            ep.Icon = new System.Drawing.Icon("icons/ladybug.ico");

            return ep;
        }

        public static Dictionary<string, ValidatingMethod> Modify_CreateNameToMethodDict()
        {
            return new Dictionary<string, ValidatingMethod>()
            {
                {"Name", new ValidatingMethod(MainValidation.CityNameValidation)},
                {"City", new ValidatingMethod(MainValidation.CityNameValidation)},
                {"LastName", new ValidatingMethod(MainValidation.CityNameValidation)},
                {"PostalCode", new ValidatingMethod(MainValidation.PostalCodeValidation)},
                {"PhoneNumber", new ValidatingMethod(MainValidation.PhoneValidation)},
                {"Address", new ValidatingMethod(MainValidation.EmptyString)},
                {"CounterNo", new ValidatingMethod(MainValidation.CircuitAndCounterAndHouseAndFlatNumberValidation)},
                {"CircuitNo", new ValidatingMethod(MainValidation.CircuitAndCounterAndHouseAndFlatNumberValidation)},
                {"AddressId", new ValidatingMethod(MainValidation.EmptyString)},
                {"CustomerId", new ValidatingMethod(MainValidation.CustomerExists)},
                {"HouseNo", new ValidatingMethod(MainValidation.CircuitAndCounterAndHouseAndFlatNumberValidation)},
                {"Street", new ValidatingMethod(MainValidation.StreetValidation)},
                {"CollectorId", new ValidatingMethod(MainValidation.OptionalCollector)},
                {"FlatNo", new ValidatingMethod(MainValidation.CircuitAndCounterAndHouseAndFlatNumberValidation)},
                
                {"cbArea", new ValidatingMethod(MainValidation.MandatoryChoice_ComboBox)},
                {"cbCustomer", new ValidatingMethod(MainValidation.MandatoryChoice_ComboBox)},
                {"cbAddress", new ValidatingMethod(MainValidation.MandatoryChoice_ComboBox)},
                {"cbCollector", new ValidatingMethod(MainValidation.OptionalChoice_ComboBox)}
            };
        }

        public static Dictionary<string, ValidatingMethod> Insert_CreateNameToMethodDict()
        {
            return new Dictionary<string, ValidatingMethod>()
            {
                {"tbCollectorID", new ValidatingMethod(MainValidation.IDValidation)},
                {"tbCollectorFirstName", new ValidatingMethod(MainValidation.CityNameValidation)},
                {"tbCollectorLastName", new ValidatingMethod(MainValidation.CityNameValidation)},
                {"tbCollectorPostalCode", new ValidatingMethod(MainValidation.PostalCodeValidation)},
                {"tbCollectorCity", new ValidatingMethod(MainValidation.CityNameValidation)},
                {"tbCollectorAddress", new ValidatingMethod(MainValidation.EmptyString)},
                {"tbCollectorPhoneNumber", new ValidatingMethod(MainValidation.PhoneValidation)},

                {"tbCustomerID", new ValidatingMethod(MainValidation.IDValidation)},
                {"tbCustomerFirstName", new ValidatingMethod(MainValidation.CityNameValidation)},
                {"tbCustomerLastName", new ValidatingMethod(MainValidation.CityNameValidation)},
                {"tbCustomerPostalCode", new ValidatingMethod(MainValidation.PostalCodeValidation)},
                {"tbCustomerCity", new ValidatingMethod(MainValidation.CityNameValidation)},
                {"tbCustomerAddress", new ValidatingMethod(MainValidation.EmptyString)},
                {"tbCustomerPhoneNumber", new ValidatingMethod(MainValidation.PhoneValidation)},

                {"tbStreet", new ValidatingMethod(MainValidation.StreetValidation)},
                {"tbAreaCollectorID", new ValidatingMethod(MainValidation.OptionalCollector)},

                {"tbCounterNo", new ValidatingMethod(MainValidation.CircuitAndCounterAndHouseAndFlatNumberValidation)},
                {"tbCircuitNo", new ValidatingMethod(MainValidation.CircuitAndCounterAndHouseAndFlatNumberValidation)},
                {"tbCounterAddressID", new ValidatingMethod(MainValidation.EmptyString)},
                {"tbCounterCustomerID", new ValidatingMethod(MainValidation.CustomerExists)},

                {"cbCustomer", new ValidatingMethod(MainValidation.MandatoryChoice_ComboBox)},
                {"cbAddress", new ValidatingMethod(MainValidation.MandatoryChoice_ComboBox)},
                {"cbArea", new ValidatingMethod(MainValidation.MandatoryChoice_ComboBox)},
                {"cbAddress_Collector", new ValidatingMethod(MainValidation.MandatoryChoice_ComboBox)},
                {"cbAddress_Customer", new ValidatingMethod(MainValidation.MandatoryChoice_ComboBox)},
                {"cbCollector", new ValidatingMethod(MainValidation.OptionalChoice_ComboBox)},

                {"tbHouseNo", new ValidatingMethod(MainValidation.CircuitAndCounterAndHouseAndFlatNumberValidation)},
                {"tbFlatNo", new ValidatingMethod(MainValidation.CircuitAndCounterAndHouseAndFlatNumberValidation)}
            };
        }
    }
}
