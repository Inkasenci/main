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
    public partial class HelpForm : Form
    {
        private TreeView HelpTreeView;
        private RichTextBox HelpTextBox;

        public HelpForm()
        {
            InitializeComponent();
            InitTreeView();
            InitTextBox();
        }

        private void AddEmoticons(string text)
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

        // TextBox ( Print HELP )
        private void InitTextBox()
        {
            HelpTextBox = rtbHelp;
            HelpTextBox.Text = LangPL.FaqAnswers["aboutHelp"];
            AddEmoticons(HelpTextBox.Text);
        }

        // Tree HELP
        private void InitTreeView()
        {
            HelpTreeView = tvFAQ;
            HelpTreeView.AfterSelect += treeViewFAQ_AfterSelect;
            var dataBaseTables = HelpTreeView.Nodes;
            int i = 1;
            // Main Node
            dataBaseTables.Add(TreeNode(i++.ToString() + ". " + LangPL.FaqQuestion["aboutHelp"]));
            dataBaseTables.Add(TreeNode(i++.ToString() + ". " + LangPL.FaqQuestion["dataBaseTables"]));

            // Child Node
            dataBaseTables[1].Nodes.Add(TreeNode(LangPL.FaqQuestion["dataBaseTableAdress"]));
            dataBaseTables[1].Nodes.Add(TreeNode(LangPL.FaqQuestion["dataBaseTableArea"]));
            dataBaseTables[1].Nodes.Add(TreeNode(LangPL.FaqQuestion["dataBaseTableCollector"]));
            dataBaseTables[1].Nodes.Add(TreeNode(LangPL.FaqQuestion["dataBaseTableCounter"]));
            dataBaseTables[1].Nodes.Add(TreeNode(LangPL.FaqQuestion["dataBaseTableCustomer"]));
        }

        private TreeNode TreeNode(string textNode)
        {
            TreeNode node = new TreeNode();
            node.Text = textNode;
            node.ForeColor = Color.Black;
            node.BackColor = Color.White;
            return node;
        }

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
                            default:
                                MessageBox.Show(LangPL.FaqErrors["indexOurOfRange"]);
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
                                        MessageBox.Show(LangPL.FaqErrors["indexOurOfRange"]);
                                        break;
                                }
                                break;
                            default:
                                MessageBox.Show(LangPL.FaqErrors["indexOurOfRange"]);
                                break;
                        }
                        break;

                    default:
                        MessageBox.Show(LangPL.FaqErrors["indexOurOfRange"]);
                        break;
                }
                AddEmoticons(HelpTextBox.Text);
            }
        }
    }
}
