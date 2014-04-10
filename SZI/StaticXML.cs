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

namespace SZI
{
    static class StaticXML
    {
        static public void XMLExport( string output, string collectorId, List<string[]> itemList )
        {
            string strRegex = @"([a-zA-Z0-9]*)\.xml";
            XmlTextWriter XmlWriter = new XmlTextWriter(Regex.IsMatch(output, strRegex, RegexOptions.None) ? output : output+".xml", null);
            XmlWriter.WriteStartDocument();
            XmlWriter.WriteComment("XML wygenerowany przez aplikację SZI.");
            XmlWriter.WriteStartElement("Counters");
            XmlWriter.WriteAttributeString("CollectorID", collectorId);

            foreach (var element in itemList)
            {
                XmlWriter.WriteStartElement("Counter");
                XmlWriter.WriteElementString("ReadId", element[0]);
                XmlWriter.WriteElementString("CounterNo", element[1]);
                XmlWriter.WriteElementString("CircuitNo", element[2]);
                XmlWriter.WriteElementString("Customer", element[3]);
                XmlWriter.WriteElementString("Address", element[4]);
                XmlWriter.WriteElementString("LastReadDate", (element[5] != LangPL.CountersWarnings["noRecord"]) ? element[5] : String.Empty);
                XmlWriter.WriteElementString("LastValue", (element[6] != LangPL.CountersWarnings["noRecord"]) ? element[6] : String.Empty);
                XmlWriter.WriteElementString("NewValue", String.Empty);
                XmlWriter.WriteEndElement();
            }

            XmlWriter.WriteEndDocument();
            XmlWriter.Close();  
        }

        static public void XMLImport(string output)
        {
            string collectorId = String.Empty;
            string strRegex = @"([a-zA-Z0-9]*)\.xml";
            string counterNo, newValue, collId;

            if (Regex.IsMatch(output, strRegex, RegexOptions.None))
            {
                try
                {
                    XmlTextReader reader = new XmlTextReader(output);

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
