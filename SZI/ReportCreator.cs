using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Printing;
using System.Drawing;
using System.Windows.Forms;

namespace SZI
{
    /// <summary>
    /// Klasa generująca raporty dotyczące stanu bazy danych.
    /// </summary>
    public static class Reports
    {
        private static string FontName = "Times New Roman";
        private static string Date = " " + DateTime.Now.ToShortDateString();

        private static Font HeadFont = new Font(FontName, 20);
        private static Font MediumFont = new Font(FontName, 15);
        private static Font TinyFont = new Font(FontName, 10);

        private static string DoubleFormat = "0.##";

        private static Brush brush = Brushes.Black;
        private static StringFormat format = StringFormat.GenericTypographic;

        /// <summary>
        /// Klasa generująca misje dla inkasentów.
        /// </summary>
        public static class Mission
        {
            private static List<List<string>> Missions;

            private static string ToPrint = String.Empty;
            private static string CollectorsName = String.Empty;

            private static bool FirstPagePrinted = false;
            private static int PlacesToVisit = 0;

            /// <summary>
            /// Tworzy obiekt PrintPreviewDialog, który jest podglądem wydruku wygenerowanej misji dla inkasenta o identyfikatorze id.
            /// </summary>
            /// <param name="id">Identyfikator inkasenta.</param>
            /// <returns>PrintPreviewDialog, który jest podglądem wydruku wygenerowanej misji.</returns>
            public static PrintPreviewDialog CreateMission(string id)
            {
                PrintDocument pd = new PrintDocument();
                PrintPreviewDialog ppd = new PrintPreviewDialog();

                using (var database = new CollectorsManagementSystemEntities())
                {
                    //data 30 dni wcześniejsza od teraźniejszej
                    var date = DateTime.Now.Subtract(new TimeSpan(30, 0, 0, 0));

                    //imię i nazwisko inkasenta
                    CollectorsName = (from collector in database.Collectors
                                      where collector.CollectorId == id
                                      select collector.Name + " " + collector.LastName).FirstOrDefault();

                    //adresy pod którymi są liczniki, których trzeba zebrać odczyty (bo ostatni odczyt był wcześniej niż 30 dni temu, lub nigdy go nie było)
                    //+ właściciel danego licznika
                    //adresy te są tylko dla danego inkasenta (id)
                    var query = (from collector in database.Collectors
                                 where collector.CollectorId == id
                                 join area in database.Areas on collector.CollectorId equals area.CollectorId
                                 join address in database.Addresses on area.AreaId equals address.AreaId
                                 join counter in database.Counters on address.AddressId equals counter.AddressId
                                 join reading in database.Readings on counter.CounterNo equals reading.CounterNo into gj
                                 where (gj.Max(a => (DateTime?)a.Date) == null ? new DateTime() : gj.Max(a => (DateTime?)a.Date)) < date
                                 select new
                                 {
                                     counter.CounterNo,
                                     counter.CircuitNo,
                                     address = area.Street + "  " + address.HouseNo.ToString() + (address.FlatNo == null ? String.Empty : "/" + address.FlatNo.ToString()),
                                     owner = (from customer in database.Customers
                                              join subCounter in database.Counters
                                              on customer.CustomerId equals subCounter.CustomerId
                                              where subCounter.CounterNo == counter.CounterNo
                                              select customer.Name + " " + customer.LastName + " " + customer.PhoneNumber).FirstOrDefault(),
                                     max = gj.Max(a => (DateTime?)a.Date) == null ? new DateTime() : gj.Max(a => (DateTime?)a.Date).Value,
                                 }).ToList();

                    PlacesToVisit = query.Count;
                    Missions = new List<List<string>>();
                    for (int i = 0; i < query.Count; i++)
                    {
                        Missions.Add(new List<string>());
                        Missions[i].Add(query[i].address);
                        Missions[i].Add(query[i].owner);
                        Missions[i].Add(query[i].CounterNo.ToString());
                        Missions[i].Add(query[i].CircuitNo.ToString());
                        Missions[i].Add(query[i].max == new DateTime() ? "nigdy" : query[i].max.ToShortDateString());
                    }
                }

                pd.DefaultPageSettings.Margins.Right = 2 * pd.DefaultPageSettings.Margins.Left;
                pd.OriginAtMargins = true;
                pd.PrintPage += PrintMission;
                pd.DocumentName = LangPL.Missions["MissionDocumentName"];
                ppd.Document = pd;
                ((Form)ppd).WindowState = FormWindowState.Maximized;

                return ppd;
            }

            /// <summary>
            /// Event handler drukujący raport - misję.
            /// </summary>
            /// <param name="sender">Obiekt PrintDocument wywołujący metodę.</param>
            /// <param name="e">Argumenty zdarzenia.</param>
            private static void PrintMission(object sender, PrintPageEventArgs e)
            {
                if (!FirstPagePrinted)
                {
                    float TotalHeight;
                    string Head = LangPL.Missions["MissionHead"] + CollectorsName + " (" + Date + ")\n\n\n";
                    string NumberOfPlacesToVisitString = LangPL.Missions["NumberOfPlacesToVisit"] + PlacesToVisit.ToString() + "\n\n\n";
                    string PlacesToVisitString = LangPL.Missions["PlacesToVisit"];
                    string MissionAttributesListString = LangPL.Missions["MissionAttributesList"];


                    SizeF HeadSize = e.Graphics.MeasureString(Head, HeadFont);
                    SizeF NumberOfPlacesToVisit = e.Graphics.MeasureString(NumberOfPlacesToVisitString, MediumFont);
                    SizeF PlacesToVisitSize = e.Graphics.MeasureString(PlacesToVisitString, MediumFont);
                    SizeF MissionAttributesList = e.Graphics.MeasureString(MissionAttributesListString, MediumFont);

                    Reports.Printing.PrintIntheMiddle(Head, 0, e, HeadFont);

                    Printing.DrawString(NumberOfPlacesToVisitString, MediumFont,
                        new Point(e.PageBounds.Location.X, e.PageBounds.Location.Y + (int)HeadSize.Height), e);

                    //wysokość dotychczas narysowanych elementów
                    TotalHeight = HeadSize.Height + NumberOfPlacesToVisit.Height + PlacesToVisitSize.Height + MissionAttributesList.Height;

                    Printing.PrintIntheMiddle(PlacesToVisitString, TotalHeight - MissionAttributesList.Height - PlacesToVisitSize.Height, e, MediumFont);
                    Printing.PrintIntheMiddle(MissionAttributesListString, TotalHeight - MissionAttributesList.Height, e, MediumFont);

                    Printing.DrawLine(e, TotalHeight);

                    int charactersOnPage = 0;
                    int linesPerPage = 0;

                    ToPrint = ListToString();
                    e.Graphics.MeasureString(ToPrint, TinyFont,
                        new Size(e.PageBounds.Width, e.PageBounds.Height - (int)TotalHeight - e.PageSettings.Margins.Top - e.PageSettings.Margins.Bottom),
                        StringFormat.GenericTypographic, out charactersOnPage, out linesPerPage);

                    SizeF MissionStringSize = e.Graphics.MeasureString(ToPrint, TinyFont);

                    string temp = ToPrint.Substring(0, charactersOnPage);
                    Printing.DrawString(temp, TinyFont, new Point(e.PageBounds.Location.X, e.PageBounds.Location.Y + (int)TotalHeight), e);

                    ToPrint = ToPrint.Substring(charactersOnPage);
                    if (ToPrint.Length > 0)
                    {
                        e.HasMorePages = true;
                        FirstPagePrinted = true;
                    }
                    else
                    {
                        TotalHeight += MissionStringSize.Height;
                        Printing.DrawLine(e, TotalHeight);
                    }
                }
                else
                {
                    int charactersOnPage = 0;
                    int linesPerPage = 0;

                    e.Graphics.MeasureString(ToPrint, TinyFont,
                        new Size(e.PageBounds.Width, e.MarginBounds.Height),
                        StringFormat.GenericTypographic, out charactersOnPage, out linesPerPage);

                    SizeF CollectorsStringSize = e.Graphics.MeasureString(ToPrint, TinyFont);

                    string temp = ToPrint.Substring(0, charactersOnPage);
                    Printing.DrawString(temp, TinyFont, e.PageBounds, e);

                    ToPrint = ToPrint.Substring(charactersOnPage);
                    if (ToPrint.Length > 0)
                    {
                        e.HasMorePages = true;
                    }
                    else
                    {
                        float TotalHeight = CollectorsStringSize.Height;
                        Printing.DrawLine(e, TotalHeight);
                        FirstPagePrinted = false;
                    }
                }
            }

            /// <summary>
            /// Zamienia listy misji na stringa przystosowanego do dodania do raportu.
            /// </summary>
            /// <returns>String przystosowany do wydruku jako część raportu.</returns>
            private static string ListToString()
            {
                String s = String.Empty;
                s += "\n";

                for (int i = 0; i < Missions.Count; i++)
                {
                    s += (i + 1).ToString() + ".";
                    for (int j = 0; j < Missions[0].Count; j++)
                    {
                        s += " " + Missions[i][j].ToString();
                    }
                    s += "\n";
                }

                return s;
            }
        }


        /// <summary>
        /// Klasa generująca raport dotyczący inkasentów.
        /// </summary>
        public static class Collectors
        {
            private static List<List<string>> CollectorsWithArea;
            private static List<List<string>> CollectorsWithoutArea;

            private static string ToPrint = string.Empty;

            private static bool FirstPagePrinted = false;

            private static double averageAreaPerCollector = 0;

            /// <summary>
            /// Tworzy obiekt PrintPreviewDialog, który jest podglądem wydruku wygenerowanego raportu dotyczącego inkasentów.
            /// </summary>
            /// <returns>Raport dotyczący inkasentów.</returns>
            public static PrintPreviewDialog CreateReport()
            {
                PrintDocument pd = new PrintDocument();
                PrintPreviewDialog ppd = new PrintPreviewDialog();
                ((Form)ppd).WindowState = FormWindowState.Maximized;

                using (var database = new CollectorsManagementSystemEntities())
                {
                    //inkasenci z przydzielonym terenem
                    var query = (from collector in database.Collectors
                                 join area in database.Areas
                                 on collector.CollectorId equals area.CollectorId into gj
                                 where gj.Any()
                                 select new
                                 {
                                     id = collector.CollectorId,
                                     name = collector.Name,
                                     lastname = collector.LastName,
                                     areacount = (from subArea in database.Areas
                                                  where subArea.CollectorId == collector.CollectorId
                                                  group subArea by subArea.AreaId into grp
                                                  select grp.Count()).Count(),
                                     addresscount = (from subArea in database.Areas
                                                     where subArea.CollectorId == collector.CollectorId
                                                     join address in database.Addresses
                                                     on subArea.AreaId equals address.AreaId into gw
                                                     select gw.Count()).Count()
                                 }).ToList();



                    //inkasenci bez terenu
                    var query1 = (from collector in database.Collectors
                                  join area in database.Areas
                                  on collector.CollectorId equals area.CollectorId into gj
                                  where !gj.Any()
                                  select new
                                  {
                                      id = collector.CollectorId,
                                      name = collector.Name,
                                      lastname = collector.LastName
                                  }).ToList();

                    averageAreaPerCollector = (double)(from area in database.Areas
                                                       join collector in database.Collectors
                                                       on area.CollectorId equals collector.CollectorId into gj
                                                       where gj.Any()
                                                       select area).Count() /
                                             (query.Count + query1.Count);

                    CollectorsWithArea = new List<List<string>>();
                    CollectorsWithoutArea = new List<List<string>>();

                    for (int i = 0; i < query.Count; i++)
                    {
                        CollectorsWithArea.Add(new List<string>());
                        CollectorsWithArea[i].Add(query[i].id);
                        CollectorsWithArea[i].Add(query[i].name);
                        CollectorsWithArea[i].Add(query[i].lastname);
                        CollectorsWithArea[i].Add(query[i].areacount.ToString());
                        CollectorsWithArea[i].Add(query[i].addresscount.ToString());
                    }

                    for (int i = 0; i < query1.Count(); i++)
                    {
                        CollectorsWithoutArea.Add(new List<string>());
                        CollectorsWithoutArea[i].Add(query1[i].id);
                        CollectorsWithoutArea[i].Add(query1[i].name);
                        CollectorsWithoutArea[i].Add(query1[i].lastname);
                        //CollectorsWithoutArea[i].Add(query1[i].areacount.ToString());
                        //CollectorsWithoutArea[i].Add(query1[i].addresscount.ToString());
                    }

                }

                pd.DefaultPageSettings.Margins.Right = 2 * pd.DefaultPageSettings.Margins.Left;
                pd.OriginAtMargins = true;
                pd.PrintPage += PrintCollectors;
                pd.DocumentName = LangPL.Reports["CollectorsReportDocumentName"];
                ppd.Document = pd;

                return ppd;
            }

            /// <summary>
            /// Event handler drukujący raport dotyczący inkasentów.
            /// </summary>
            /// <param name="sender">Obiekt PrintDocument wywołujący metodę.</param>
            /// <param name="e">Argumenty zdarzenia.</param>
            private static void PrintCollectors(object sender, PrintPageEventArgs e)
            {
                if (!FirstPagePrinted)
                {
                    float TotalHeight;

                    int TotalCollectorsNumber = CollectorsWithArea.Count + CollectorsWithoutArea.Count();

                    string Head = LangPL.Reports["CollectorsHead"] + Date;
                    SizeF HeadSize = e.Graphics.MeasureString(Head, HeadFont);
                    SizeF TotalCollectorsNumberSize = e.Graphics.MeasureString(LangPL.Reports["TotalCollectorsNumber"] + TotalCollectorsNumber, MediumFont);
                    SizeF CollectorsWithAreaSize = e.Graphics.MeasureString(LangPL.Reports["CollectorsWithArea"] + CollectorsWithArea.Count, MediumFont);
                    SizeF CollectorsWithoutAreaSize = e.Graphics.MeasureString(LangPL.Reports["CollectorsWithoutArea"] + CollectorsWithoutArea.Count(), MediumFont);
                    SizeF AverageAreaPerCollector = e.Graphics.MeasureString(LangPL.Reports["AverageAreaPerCollector"] + CollectorsWithoutArea.Count(), MediumFont);
                    SizeF CollectorsWithAreaListSize = e.Graphics.MeasureString(LangPL.Reports["CollectorsList"], MediumFont);
                    SizeF AttributesList = e.Graphics.MeasureString(LangPL.Reports["CollectorsAttributesList"], MediumFont);

                    Reports.Printing.PrintIntheMiddle(Head, 0, e, HeadFont);

                    Printing.DrawString(LangPL.Reports["TotalCollectorsNumber"] + TotalCollectorsNumber, MediumFont,
                        new Point(e.PageBounds.Location.X, e.PageBounds.Location.Y + (int)HeadSize.Height), e);

                    Printing.DrawString(LangPL.Reports["CollectorsWithArea"] + CollectorsWithArea.Count, MediumFont,
                        new Point(e.PageBounds.Location.X, e.PageBounds.Location.Y + (int)(HeadSize.Height + TotalCollectorsNumberSize.Height)), e);

                    Printing.DrawString(LangPL.Reports["CollectorsWithoutArea"] + CollectorsWithoutArea.Count(), MediumFont,
                         new Point(e.PageBounds.Location.X, e.PageBounds.Location.Y + (int)(HeadSize.Height + TotalCollectorsNumberSize.Height + CollectorsWithAreaSize.Height)), e);

                    Printing.DrawString(LangPL.Reports["AverageAreaPerCollector"] + averageAreaPerCollector.ToString(DoubleFormat), MediumFont,
                         new Point(e.PageBounds.Location.X, e.PageBounds.Location.Y + (int)(HeadSize.Height + TotalCollectorsNumberSize.Height + CollectorsWithAreaSize.Height + CollectorsWithoutAreaSize.Height)), e);

                    //wysokość dotychczas narysowanych elementów
                    TotalHeight = HeadSize.Height + TotalCollectorsNumberSize.Height + CollectorsWithAreaSize.Height + CollectorsWithoutAreaSize.Height + CollectorsWithAreaListSize.Height + CollectorsWithoutAreaSize.Height + AttributesList.Height;

                    Printing.PrintIntheMiddle(LangPL.Reports["CollectorsList"], TotalHeight - CollectorsWithAreaListSize.Height - AttributesList.Height, e, MediumFont);
                    Printing.PrintIntheMiddle(LangPL.Reports["CollectorsAttributesList"], TotalHeight - AttributesList.Height, e, MediumFont);

                    Printing.DrawLine(e, TotalHeight);

                    int charactersOnPage = 0;
                    int linesPerPage = 0;

                    ToPrint = ListToString();
                    e.Graphics.MeasureString(ToPrint, TinyFont,
                        new Size(e.PageBounds.Width, e.PageBounds.Height - (int)TotalHeight - e.PageSettings.Margins.Top - e.PageSettings.Margins.Bottom),
                        StringFormat.GenericTypographic, out charactersOnPage, out linesPerPage);

                    SizeF CollectorsStringSize = e.Graphics.MeasureString(ToPrint, TinyFont);

                    string temp = ToPrint.Substring(0, charactersOnPage);
                    Printing.DrawString(temp, TinyFont, new Point(e.PageBounds.Location.X, e.PageBounds.Location.Y + (int)TotalHeight), e);

                    ToPrint = ToPrint.Substring(charactersOnPage);
                    if (ToPrint.Length > 0)
                    {
                        e.HasMorePages = true;
                        FirstPagePrinted = true;
                    }
                    else
                    {
                        TotalHeight += CollectorsStringSize.Height;
                        Printing.DrawLine(e, TotalHeight);
                    }
                }
                else
                {
                    int charactersOnPage = 0;
                    int linesPerPage = 0;

                    e.Graphics.MeasureString(ToPrint, TinyFont,
                        new Size(e.PageBounds.Width, e.MarginBounds.Height),
                        StringFormat.GenericTypographic, out charactersOnPage, out linesPerPage);

                    SizeF CollectorsStringSize = e.Graphics.MeasureString(ToPrint, TinyFont);

                    string temp = ToPrint.Substring(0, charactersOnPage);
                    Printing.DrawString(temp, TinyFont, e.PageBounds, e);

                    ToPrint = ToPrint.Substring(charactersOnPage);
                    if (ToPrint.Length > 0)
                    {
                        e.HasMorePages = true;
                    }
                    else
                    {
                        float TotalHeight = CollectorsStringSize.Height;
                        Printing.DrawLine(e, TotalHeight);
                        FirstPagePrinted = false;
                    }
                }
            }

            /// <summary>
            /// Zamienia listy inkasentów na stringa przystosowanego do dodania do raportu.
            /// </summary>
            /// <returns>String przystosowany do wydruku jako część raportu.</returns>
            private static string ListToString()
            {
                String s = String.Empty;
                s += "\n";

                for (int i = 0; i < CollectorsWithArea.Count; i++)
                {
                    s += (i + 1).ToString() + ".";
                    for (int j = 0; j < CollectorsWithArea[0].Count - 2; j++)
                    {
                        s += " " + CollectorsWithArea[i][j].ToString();
                    }
                    s += " " + CollectorsWithArea[i][3] + "(" + CollectorsWithArea[i][4] + ")" + "\n";
                }

                s += "\n";

                for (int i = 0; i < CollectorsWithoutArea.Count; i++)
                {
                    s += (i + 1).ToString() + ".";
                    for (int j = 0; j < CollectorsWithoutArea[0].Count; j++)
                    {
                        s += " " + CollectorsWithoutArea[i][j].ToString();
                    }
                    s += " " + 0.ToString() + "(" + 0.ToString() + ")" + "\n";
                }
                return s;
            }
        }

        /// <summary>
        /// Klasa generująca raport dotyczący klientów.
        /// </summary>
        public static class Customers
        {
            private static List<List<string>> CustomersWithCounters;
            private static List<List<string>> CustomersWithoutCounters;

            private static string ToPrint = String.Empty;

            private static bool FirstPagePrinted = false;


            private static double averageCounterPerCustomerNumber = 0;

            /// <summary>
            /// Metoda tworząca obiekt klasy PrintPreviewDialog, który jest raportem dotyczącym klientów.
            /// </summary>
            /// <returns>Obiekt klasy PrintPreviewDialog, który jest raportem dotyczącym klientów.</returns>
            public static PrintPreviewDialog CreateReport()
            {
                PrintDocument pd = new PrintDocument();
                PrintPreviewDialog ppd = new PrintPreviewDialog();
                ((Form)ppd).WindowState = FormWindowState.Maximized;

                using (var database = new CollectorsManagementSystemEntities())
                {
                    //klienci (z >0 licznikami) i liczba liczników przypisanych do każdego z nich
                    var query = (from customer in database.Customers
                                 join counter in database.Counters
                                 on customer.CustomerId equals counter.CustomerId into gj
                                 where gj.Any()
                                 select new
                                 {
                                     id = customer.CustomerId,
                                     name = customer.Name,
                                     lastname = customer.LastName,
                                     countercount = gj.Count()
                                 }).ToList();


                    //klienci bez przypisanych liczników
                    var query1 = (from customer in database.Customers
                                  join counter in database.Counters
                                  on customer.CustomerId equals counter.CustomerId into gj
                                  where !gj.Any()
                                  select new
                                  {
                                      id = customer.CustomerId,
                                      name = customer.Name,
                                      lastname = customer.LastName,
                                      countercount = 0
                                  }).ToList();

                    averageCounterPerCustomerNumber = (from counter in database.Counters
                                                       join customer in database.Customers
                                                       on counter.CustomerId equals customer.CustomerId into gj
                                                       where gj.Any()
                                                       select counter).Count() / (double)(query.Count + query1.Count);

                    CustomersWithCounters = new List<List<string>>();
                    for (int i = 0; i < query.Count; i++)
                    {
                        CustomersWithCounters.Add(new List<string>());
                        CustomersWithCounters[i].Add(query[i].id);
                        CustomersWithCounters[i].Add(query[i].name);
                        CustomersWithCounters[i].Add(query[i].lastname);
                        CustomersWithCounters[i].Add(query[i].countercount.ToString());
                    }

                    CustomersWithoutCounters = new List<List<string>>();
                    for (int i = 0; i < query1.Count; i++)
                    {
                        CustomersWithoutCounters.Add(new List<string>());
                        CustomersWithoutCounters[i].Add(query1[i].id);
                        CustomersWithoutCounters[i].Add(query1[i].name);
                        CustomersWithoutCounters[i].Add(query1[i].lastname);
                        CustomersWithoutCounters[i].Add(query1[i].countercount.ToString());
                    }

                }

                pd.DefaultPageSettings.Margins.Right = 2 * pd.DefaultPageSettings.Margins.Left;
                pd.OriginAtMargins = true;
                pd.PrintPage += PrintCustomers;
                pd.DocumentName = LangPL.Reports["CustomersReportDocumentName"];
                ppd.Document = pd;

                return ppd;
            }

            /// <summary>
            /// Event handler drukujący raport dotyczący klientów.
            /// </summary>
            /// <param name="sender">Obiekt PrintDocument wywołujący metodę.</param>
            /// <param name="e">Argumenty zdarzenia.</param>
            private static void PrintCustomers(object sender, PrintPageEventArgs e)
            {
                if (!FirstPagePrinted)
                {
                    float TotalHeight;

                    int TotalCustomersNumber = CustomersWithCounters.Count + CustomersWithoutCounters.Count;

                    string Head = LangPL.Reports["CustomersHead"] + Date;
                    SizeF HeadSize = e.Graphics.MeasureString(Head, HeadFont);
                    SizeF TotalCustomersNumberSize = e.Graphics.MeasureString(LangPL.Reports["TotalCustomersNumber"] + TotalCustomersNumber, MediumFont);
                    SizeF CustomersWithCountersSize = e.Graphics.MeasureString(LangPL.Reports["CustomersWithCounters"] + CustomersWithCounters.Count, MediumFont);
                    SizeF CustomersWithoutCountersSize = e.Graphics.MeasureString(LangPL.Reports["CustomersWithoutCounters"] + CustomersWithoutCounters.Count, MediumFont);
                    SizeF AverageCounterpCustomer = e.Graphics.MeasureString(LangPL.Reports["AverageCounterPerCustomer"], MediumFont);
                    SizeF CustomersWithCountersListSize = e.Graphics.MeasureString(LangPL.Reports["CustomersList"], MediumFont);
                    SizeF AttributesList = e.Graphics.MeasureString(LangPL.Reports["CustomersAttributesList"], MediumFont);

                    Reports.Printing.PrintIntheMiddle(Head, 0, e, HeadFont);

                    Printing.DrawString(LangPL.Reports["TotalCustomersNumber"] + TotalCustomersNumber, MediumFont, new Point(e.PageBounds.Location.X, e.PageBounds.Location.Y + (int)HeadSize.Height), e);

                    Printing.DrawString(LangPL.Reports["CustomersWithCounters"] + CustomersWithCounters.Count, MediumFont, new Point(e.PageBounds.Location.X, e.PageBounds.Location.Y + (int)(HeadSize.Height + TotalCustomersNumberSize.Height)), e);

                    Printing.DrawString(LangPL.Reports["CustomersWithoutCounters"] + CustomersWithoutCounters.Count, MediumFont, new Point(e.PageBounds.Location.X, e.PageBounds.Location.Y + (int)(HeadSize.Height + TotalCustomersNumberSize.Height + CustomersWithCountersSize.Height)), e);

                    Printing.DrawString(LangPL.Reports["AverageCounterPerCustomer"] + averageCounterPerCustomerNumber.ToString(DoubleFormat), MediumFont, new Point(e.PageBounds.Location.X, e.PageBounds.Location.Y + (int)(HeadSize.Height + TotalCustomersNumberSize.Height + CustomersWithCountersSize.Height + CustomersWithoutCountersSize.Height)), e);

                    //wysokość dotychczas narysowanych elementów
                    TotalHeight = HeadSize.Height + TotalCustomersNumberSize.Height + CustomersWithCountersSize.Height +
                        CustomersWithoutCountersSize.Height + AverageCounterpCustomer.Height + CustomersWithCountersListSize.Height + AttributesList.Height;

                    Printing.PrintIntheMiddle(LangPL.Reports["CustomersList"], TotalHeight - CustomersWithCountersListSize.Height - AttributesList.Height, e, MediumFont);
                    Printing.PrintIntheMiddle(LangPL.Reports["CustomersAttributesList"], TotalHeight - AttributesList.Height, e, MediumFont);

                    Printing.DrawLine(e, TotalHeight);

                    int charactersOnPage = 0;
                    int linesPerPage = 0;

                    ToPrint = ListToString();
                    e.Graphics.MeasureString(ToPrint, TinyFont,
                        new Size(e.PageBounds.Width, e.PageBounds.Height - (int)TotalHeight - e.PageSettings.Margins.Top - e.PageSettings.Margins.Bottom),
                        StringFormat.GenericTypographic, out charactersOnPage, out linesPerPage);

                    SizeF CustomersStringSize = e.Graphics.MeasureString(ToPrint, TinyFont);

                    string temp = ToPrint.Substring(0, charactersOnPage);

                    Printing.DrawString(temp, TinyFont, new Point(e.PageBounds.Location.X, e.PageBounds.Location.Y + (int)TotalHeight), e);

                    ToPrint = ToPrint.Substring(charactersOnPage);
                    if (ToPrint.Length > 0)
                    {
                        e.HasMorePages = true;
                    }
                    else
                    {
                        TotalHeight += CustomersStringSize.Height;

                        Printing.DrawLine(e, TotalHeight);
                    }
                    FirstPagePrinted = true;
                }
                else
                {
                    int charactersOnPage = 0;
                    int linesPerPage = 0;

                    e.Graphics.MeasureString(ToPrint, TinyFont,
                        new Size(e.PageBounds.Width, e.PageBounds.Height - e.PageSettings.Margins.Top - e.PageSettings.Margins.Bottom),
                        StringFormat.GenericTypographic, out charactersOnPage, out linesPerPage);

                    SizeF CustomersStringSize = e.Graphics.MeasureString(ToPrint, TinyFont);
                    float TotalHeight = CustomersStringSize.Height;

                    string temp = ToPrint.Substring(0, charactersOnPage);

                    Printing.DrawString(temp, TinyFont, new Point(e.PageBounds.Location.X, e.PageBounds.Location.Y), e);

                    ToPrint = ToPrint.Substring(charactersOnPage);
                    if (ToPrint.Length > 0)
                    {
                        e.HasMorePages = true;
                    }
                    else
                    {
                        TotalHeight += CustomersStringSize.Height;
                        Printing.DrawLine(e, TotalHeight);
                    }
                }
            }

            /// <summary>
            /// Zamienia listy klientów na stringa przystosowanego do dodania do raportu.
            /// </summary>
            /// <returns>String przystosowany do wydruku jako część raportu.</returns>
            private static string ListToString()
            {
                String s = String.Empty;
                s += "\n";

                for (int i = 0; i < CustomersWithCounters.Count; i++)
                {
                    s += (i + 1).ToString() + ".";
                    for (int j = 0; j < CustomersWithCounters[0].Count - 1; j++)
                    {
                        s += " " + CustomersWithCounters[i][j].ToString();
                    }
                    s += "   " + CustomersWithCounters[i][3].ToString();
                    s += "\n";
                }
                s += "\n";
                for (int i = 0; i < CustomersWithoutCounters.Count; i++)
                {
                    s += (i + 1).ToString() + ".";
                    for (int j = 0; j < CustomersWithoutCounters[0].Count - 1; j++)
                    {
                        s += " " + CustomersWithoutCounters[i][j].ToString();
                    }
                    s += "   " + CustomersWithoutCounters[i][3].ToString();
                    s += "\n";
                }

                return s;
            }
        }

        /// <summary>
        /// Klasa zawierająca metody ułatwiające rysowanie obiektów na stronie.
        /// </summary>
        private static class Printing
        {
            /// <summary>
            /// Metoda rysująca stringa s czcionką font w punkcie point na stronie, z którą związany jest parametr e.
            /// </summary>
            /// <param name="s">Napis do narysowania.</param>
            /// <param name="font">Czcionka, którą napis zostanie narysowany.</param>
            /// <param name="point">Punkt, w którym napis zostanie narysowany.</param>
            /// <param name="e">Parametr związany ze stroną, na której napis zostanie narysowany.</param>
            public static void DrawString(String s, Font font, Point point, PrintPageEventArgs e)
            {
                e.Graphics.DrawString(s, font, brush, point, format);
            }

            /// <summary>
            /// Metoda rysująca stringa s czcionką font w prostokącie rectangle na stronie, z którą związany jest parametr e.
            /// </summary>
            /// <param name="s">Napis do narysowania.</param>
            /// <param name="font">Czcionka, którą napis zostanie narysowany.</param>
            /// <param name="rectangle">Prostokąt, w którym napis zostanie narysowany.</param>
            /// <param name="e">Parametr związany ze stroną, na której napis zostanie narysowany.</param>
            public static void DrawString(String s, Font font, Rectangle rectangle, PrintPageEventArgs e)
            {
                e.Graphics.DrawString(s, font, brush, rectangle, format);
            }

            /// <summary>
            /// Rysuje napis s na wysokości CurrentHeight na środku strony czcionką font.
            /// </summary>
            /// <param name="s">Napis do narysowania.</param>
            /// <param name="CurrentHeight">Wysokość, na jakiej napis zostanie narysowany.</param>
            /// <param name="e">Parametr zawierający informację o marginesach na drukowanej stronie.</param>
            /// <param name="font">Czcionka, którą napis zostanie narysowany.</param>
            /// <returns></returns>
            public static SizeF PrintIntheMiddle(String s, float CurrentHeight, PrintPageEventArgs e, Font font)
            {
                SizeF Size = e.Graphics.MeasureString(s, font);

                e.Graphics.DrawString(s, font, Brushes.Black,
                    new Point(e.MarginBounds.Location.X + (int)(e.MarginBounds.Width / 2 - Size.Width / 2), e.PageBounds.Y + (int)CurrentHeight),
                    StringFormat.GenericTypographic);

                return Size;
            }

            /// <summary>
            /// Rysuje linię od lewej do prawej krawędzi według koordynatów zawartych w parametrze e, na wysokości TotalHeight.
            /// </summary>
            /// <param name="e">Parametr zawierający informację o marginesach na drukowanej stronie.</param>
            /// <param name="TotalHeight">Wysokość, na jakiej linia zostanie narysowana.</param>
            public static void DrawLine(PrintPageEventArgs e, float TotalHeight)
            {
                e.Graphics.DrawLine
                    (
                    new Pen(Brushes.Black),
                    new Point(e.PageBounds.Left, e.PageBounds.Y + (int)TotalHeight),
                    new Point(e.MarginBounds.Right, e.PageBounds.Y + (int)TotalHeight)
                    );
            }

        }


    }
}
