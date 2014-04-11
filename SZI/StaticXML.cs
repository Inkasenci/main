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
    /// Statyczna klasa odpowiedzialna za zapis i odczyt z pliku XML
    /// </summary>
    static class StaticXML
    {
        /// <summary>
        /// Wczytywanie rekordów z pliku XML
        /// </summary>
        /// <param name="path">Adres plku do odczytu</param>
        /// <param name="cCollection">Argument wyjściowy zawierający kolekcję odczytów</param>
        static public void ReadFromXml(string path, out CounersCollection cCollection)
        {
            cCollection = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(CounersCollection));

                StreamReader reader = new StreamReader(path);
                cCollection = (CounersCollection)serializer.Deserialize(reader);
                reader.Close();
                if (cCollection != null)
                    cCollection.AddNewElementsToDataBase();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Zapis danych do pliku XML
        /// </summary>
        /// <param name="path">Adres plku do odczytu</param>
        /// <param name="cCollection">Argument zawierający kolekcję odczytów</param>
        /// <param name="SaveFile">Zwracaj informację o zapisie danych do pliku ( true w przypadku nieudanej próby, true w przypadku udanej )</param>
        static public void WriteToXml(string path, CounersCollection cCollection, out bool SaveFile)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer writer =
                        new System.Xml.Serialization.XmlSerializer(typeof(CounersCollection));
                System.IO.StreamWriter file = new System.IO.StreamWriter(path);
                writer.Serialize(file, cCollection);
                file.Close();
                SaveFile = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                SaveFile = true;
                    counterNo = String.Empty;
                    collId = String.Empty;
                    newValue = String.Empty;

                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "Counters":
                                    collId = reader.GetAttribute("CollectorID");
                                    break;
                                case "ReadId":
                                    break;
                                case "CounterNo":
                                    counterNo = reader.ReadString();
                                    break;
                                case "CircuitNo":
                                    break;
                                case "Address":
                                    break;
                                case "LastReadDate":
                                    break;
                                case "LastValue":
                                    break;
                                case "NewValue":
                                    newValue = reader.ReadString();
                                    if (newValue != String.Empty)
                                    {
                                        Reading newRecord = new Reading();
                                        newRecord.CollectorId = collId;
                                        newRecord.Date = DateTime.Now;
                                        newRecord.CounterNo = Convert.ToInt32(counterNo);
                                        newRecord.Value = Convert.ToInt32(newValue);
                                        newRecord.ReadingId = Guid.NewGuid();
                                        newRecord.InsertIntoDB();
                                    }
                                    counterNo = String.Empty;
                                    newValue = String.Empty;
                                    break;
                            }
                        }
                    }
                } 
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
    }
}
