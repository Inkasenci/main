using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

namespace SZI
{
    /// <summary>
    /// Statyczna klasa odpowiedzialna za zapis i odczyt z pliku XML.
    /// </summary>
    static class StaticXML
    {
        /// <summary>
        /// Wczytywanie rekord�w z pliku XML.
        /// </summary>
        /// <param name="path">Adres pliku do odczytu.</param>
        /// <param name="cCollection">Argument wej�ciowo/wyj�ciowy zawieraj�cy kolekcj� odczyt�w.</param>
        /// <param name="toDataBase">Argument informuj�cy o konieczno�ci (b�d� nie) zapisu danych do bazy.</param>
        static public void ReadFromXml(string path, bool toDataBase, out CountersCollection cCollection)
        {
            cCollection = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(CountersCollection));

                StreamReader reader = new StreamReader(path);
                cCollection = (CountersCollection)serializer.Deserialize(reader);
                reader.Close();
                if (cCollection != null && toDataBase)
                    cCollection.AddNewElementsToDataBase();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Zapis danych do pliku XML.
        /// </summary>
        /// <param name="path">Adres pliku do odczytu.</param>
        /// <param name="cCollection">Argument zawieraj�cy kolekcj� odczyt�w.</param>
        /// <param name="SaveFile">Zwraca informacj� o zapisie danych do pliku (true w przypadku nieudanej pr�by, true w przypadku udanej ).</param>
        static public void WriteToXml(string path, CountersCollection cCollection, out bool SaveFile)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer writer =
                        new System.Xml.Serialization.XmlSerializer(typeof(CountersCollection));

                System.IO.StreamWriter file = new System.IO.StreamWriter(path);
                writer.Serialize(file, cCollection);
                file.Close();
                SaveFile = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                SaveFile = true;
            }
        }
    }
}
