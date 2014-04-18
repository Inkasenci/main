using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace SZI
{
    /// <summary>
    /// Klasa obsługująca pomoc do aplikacji.
    /// </summary>
    public partial class HelpForm : Form
    {
        /// <summary>
        /// TreeView - lista pytań.
        /// </summary>
        private TreeView HelpTreeView;
        /// <summary>
        /// Box wyświetlający odpowiedzi na pytania.
        /// </summary>
        private RichTextBox HelpTextBox;

        /// <summary>
        /// Konstruktor klasy.
        /// </summary>
        public HelpForm()
        {
            InitializeComponent();
            InitTreeView();
            InitTextBox();
        }

        /// <summary>
        /// Funkcja zmieniająca adresy obrazów na obrazy.
        /// </summary>
        /// <param name="text">Walidowany text.</param>
        private void AddImages(string text)
        {
            HelpTextBox.ReadOnly = false;
            string strRegex = @"HelpImg/([a-zA-Z0-9]*)\.png";
            Regex myRegex = new Regex(strRegex, RegexOptions.None);
            DataFormats.Format myFormat = DataFormats.GetFormat(DataFormats.Bitmap);
            Bitmap myBitmap;
            string textToAdd;
            int i = 1;
            int w = 0;
            foreach (Match match in myRegex.Matches(text))
            {
                try
                {
                    myBitmap = new Bitmap(Regex.Replace(match.Value, "/", "\\"));
                    Clipboard.Clear();
                    Clipboard.SetDataObject(myBitmap);
                    HelpTextBox.Select(match.Index - w, match.Length);
                    HelpTextBox.Paste();
                    Clipboard.Clear();
                    textToAdd = "\r\n Rys. " + i++.ToString() + "\r\n\r\n";
                    Clipboard.SetText(textToAdd);
                    HelpTextBox.Select(match.Index - w + 1, 0);
                    HelpTextBox.Paste();
                    w += match.Length - 1 - textToAdd.Length + 4;
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(LangPL.FaqErrors["loadHelp"] + ": " + Ex.ToString());
                }
            }
            HelpTextBox.ReadOnly = true;
        }

        /// <summary>
        /// Inicjalizacja Boxu - wyświetlającego odpowiedzi.
        /// </summary>
        private void InitTextBox()
        {
            HelpTextBox = rtbHelp;
            HelpTextBox.Text = LangPL.FaqAnswers["aboutHelp"];
            AddImages(HelpTextBox.Text);
        }

        /// <summary>
        /// Inicjalizacja TreeView - wyświtlający pytania.
        /// </summary>
        private void InitTreeView()
        {
            HelpTreeView = tvFAQ;
            HelpTreeView.AfterSelect += treeViewFAQ_AfterSelect;
            var dataBaseTables = HelpTreeView.Nodes;
            int i = 1;
            // Main Node
            dataBaseTables.Add(TreeNode(i++.ToString() + ". " + LangPL.FaqQuestion["aboutHelp"]));
            dataBaseTables.Add(TreeNode(i++.ToString() + ". " + LangPL.FaqQuestion["dataBaseTables"]));
            dataBaseTables.Add(TreeNode(i++.ToString() + ". " + LangPL.FaqQuestion["dataBaseReading"]));
            dataBaseTables.Add(TreeNode(i++.ToString() + ". " + LangPL.FaqQuestion["dataBaseXML"]));
            dataBaseTables.Add(TreeNode(i++.ToString() + ". " + LangPL.FaqQuestion["XMLTextEditor"]));

            // Child Node 1
            dataBaseTables[1].Nodes.Add(TreeNode(LangPL.FaqQuestion["dataBaseTableAdress"]));
            dataBaseTables[1].Nodes.Add(TreeNode(LangPL.FaqQuestion["dataBaseTableArea"]));
            dataBaseTables[1].Nodes.Add(TreeNode(LangPL.FaqQuestion["dataBaseTableCollector"]));
            dataBaseTables[1].Nodes.Add(TreeNode(LangPL.FaqQuestion["dataBaseTableCounter"]));
            dataBaseTables[1].Nodes.Add(TreeNode(LangPL.FaqQuestion["dataBaseTableCustomer"]));

            // Child Node 2
            dataBaseTables[2].Nodes.Add(TreeNode(LangPL.FaqQuestion["dataBaseReadingEmptyDataBase"]));
            dataBaseTables[2].Nodes.Add(TreeNode(LangPL.FaqQuestion["dataBaseReadingZeroReading"]));
            dataBaseTables[2].Nodes.Add(TreeNode(LangPL.FaqQuestion["dataBaseReadingExport"]));
            dataBaseTables[2].Nodes.Add(TreeNode(LangPL.FaqQuestion["dataBaseReadingImport"]));
            dataBaseTables[2].Nodes.Add(TreeNode(LangPL.FaqQuestion["dataBaseReadingImportError"]));

            // Child Node 3
            dataBaseTables[3].Nodes.Add(TreeNode(LangPL.FaqQuestion["dataBaseXMLFormat"]));
            dataBaseTables[3].Nodes.Add(TreeNode(LangPL.FaqQuestion["dataBaseXMLEmptyElement"]));
            dataBaseTables[3].Nodes.Add(TreeNode(LangPL.FaqQuestion["dataBaseXMLPriority"]));

            // Child Node 4
            dataBaseTables[4].Nodes.Add(TreeNode(LangPL.FaqQuestion["XMLTextEditorReadOnly"]));
            dataBaseTables[4].Nodes.Add(TreeNode(LangPL.FaqQuestion["XMLTextEditorNextPrevError"]));
            dataBaseTables[4].Nodes.Add(TreeNode(LangPL.FaqQuestion["XMLTextEditorImportError"]));
            dataBaseTables[4].Nodes.Add(TreeNode(LangPL.FaqQuestion["XMLTextEditorSaveError"]));
        }

        /// <summary>
        /// Tworzene elementu drzewa
        /// </summary>
        /// <param name="textNode">Text wyświetlany na liście.</param>
        private TreeNode TreeNode(string textNode)
        {
            TreeNode node = new TreeNode();
            node.Text = textNode;
            node.ForeColor = Color.Black;
            node.BackColor = Color.White;
            return node;
        }

        /// <summary>
        /// Generowanie obsługi drzewa - kolejny elementy odpowiadają wybranym odpowiedzią ( wyswietlanych w RichTextBox ).
        /// </summary>
        /// <param name="sender">Obiekt eventu.</param>
        /// <param name="e">Argument eventu.</param>
        private void treeViewFAQ_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse)
            {
                switch (e.Node.Level)
                {
                    case 0:
                        switch (e.Node.Index)
                        {
                            case 0:
                                rtbHelp.Text = LangPL.FaqAnswers["aboutHelp"];
                                break;
                            case 1:
                                rtbHelp.Text = LangPL.FaqAnswers["dataBaseTables"];
                                break;
                            case 2:
                                rtbHelp.Text = LangPL.FaqAnswers["dataBaseReading"];
                                break;
                            case 3:
                                rtbHelp.Text = LangPL.FaqAnswers["dataBaseXML"];
                                break;
                            case 4:
                                rtbHelp.Text = LangPL.FaqAnswers["XMLTextEditor"];
                                break;
                            default:
                                MessageBox.Show(LangPL.FaqErrors["indexOutOfRange"]);
                                break;
                        }
                        break;

                    case 1:
                        switch (e.Node.Parent.Index)
                        {
                            case 0:
                                rtbHelp.Text = LangPL.FaqAnswers["dataBaseTables"];
                                break;
                            case 1:
                                switch (e.Node.Index)
                                {
                                    case 0:
                                        rtbHelp.Text = LangPL.FaqAnswers["dataBaseTableAdress"];
                                        break;
                                    case 1:
                                        rtbHelp.Text = LangPL.FaqAnswers["dataBaseTableArea"];
                                        break;
                                    case 2:
                                        rtbHelp.Text = LangPL.FaqAnswers["dataBaseTableCollector"];
                                        break;
                                    case 3:
                                        rtbHelp.Text = LangPL.FaqAnswers["dataBaseTableCounter"];
                                        break;
                                    case 4:
                                        rtbHelp.Text = LangPL.FaqAnswers["dataBaseTableCustomer"];
                                        break;
                                    default:
                                        MessageBox.Show(LangPL.FaqErrors["indexOutOfRange"]);
                                        break;
                                }
                                break;
                            case 2:
                                switch (e.Node.Index)
                                {
                                    case 0:
                                        rtbHelp.Text = LangPL.FaqAnswers["dataBaseReadingEmptyDataBase"];
                                        break;
                                    case 1:
                                        rtbHelp.Text = LangPL.FaqAnswers["dataBaseReadingZeroReading"];
                                        break;
                                    case 2:
                                        rtbHelp.Text = LangPL.FaqAnswers["dataBaseReadingExport"];
                                        break;
                                    case 3:
                                        rtbHelp.Text = LangPL.FaqAnswers["dataBaseReadingImport"];
                                        break;
                                    case 4:
                                        rtbHelp.Text = LangPL.FaqAnswers["dataBaseReadingImportError"];
                                        break;
                                    default:
                                        MessageBox.Show(LangPL.FaqErrors["indexOutOfRange"]);
                                        break;
                                }
                                break;

                            case 3:
                                switch (e.Node.Index)
                                {
                                    case 0:
                                        rtbHelp.Text = LangPL.FaqAnswers["dataBaseXMLFormat"];
                                        break;
                                    case 1:
                                        rtbHelp.Text = LangPL.FaqAnswers["dataBaseXMLEmptyElement"];
                                        break;
                                    case 2:
                                        rtbHelp.Text = LangPL.FaqAnswers["dataBaseXMLPriority"];
                                        break;
                                    default:
                                        MessageBox.Show(LangPL.FaqErrors["indexOutOfRange"]);
                                        break;
                                }
                                break;

                            case 4:
                                switch (e.Node.Index)
                                {
                                    case 0:
                                        rtbHelp.Text = LangPL.FaqAnswers["XMLTextEditorReadOnly"];
                                        break;
                                    case 1:
                                        rtbHelp.Text = LangPL.FaqAnswers["XMLTextEditorNextPrevError"];
                                        break;
                                    case 2:
                                        rtbHelp.Text = LangPL.FaqAnswers["XMLTextEditorImportError"];
                                        break;
                                    case 3:
                                        rtbHelp.Text = LangPL.FaqAnswers["XMLTextEditorSaveError"];
                                        break;
                                    default:
                                        MessageBox.Show(LangPL.FaqErrors["indexOutOfRange"]);
                                        break;
                                }
                                break;
                        }
                        break;
                    default:
                        MessageBox.Show(LangPL.FaqErrors["indexOutOfRange"]);
                        break;
                }
                AddImages(HelpTextBox.Text);
            }
        }
    }
}
