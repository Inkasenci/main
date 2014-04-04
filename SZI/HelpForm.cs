using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SZI
{
    public partial class HelpForm : Form
    {
        private TreeView HelpTreeView;

        public HelpForm()
        {
            InitializeComponent();
            InitTreeView();
        }

        private void InitTreeView()
        {
            HelpTreeView = tvFAQ;
            HelpTreeView.AfterSelect += treeViewFAQ_AfterSelect;
            var dataBaseTables = HelpTreeView.Nodes;
            dataBaseTables.Add(TreeNode(LangPL.FaqQuestion["dataBaseTables"]));
            dataBaseTables[0].Nodes.Add(TreeNode(LangPL.FaqQuestion["dataBaseTableAdress"]));
            dataBaseTables[0].Nodes.Add(TreeNode(LangPL.FaqQuestion["dataBaseTableArea"]));
            dataBaseTables[0].Nodes.Add(TreeNode(LangPL.FaqQuestion["dataBaseTableCollector"]));
            dataBaseTables[0].Nodes.Add(TreeNode(LangPL.FaqQuestion["dataBaseTableCounter"]));
            dataBaseTables[0].Nodes.Add(TreeNode(LangPL.FaqQuestion["dataBaseTableCustomer"]));
        }

        private TreeNode TreeNode( string textNode )
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
                if (e.Node.Level == 0)
                {
                    switch (e.Node.Index)
                    {
                        case 0:
                            lHelp.Text = LangPL.FaqAnswers["dataBaseTables"];
                            break;
                        default:
                            MessageBox.Show(LangPL.FaqErrors["indexOurOfRange"]);
                            break;
                    }
                }
                else if( e.Node.Level >= 1 )
                    if (e.Node.Parent.Index == 0)
                    {
                        switch (e.Node.Index)
                        {
                            case 0:
                                lHelp.Text = LangPL.FaqAnswers["dataBaseTableAdress"];
                                break;
                            case 1:
                                lHelp.Text = LangPL.FaqAnswers["dataBaseTableArea"];
                                break;
                            case 2:
                                lHelp.Text = LangPL.FaqAnswers["dataBaseTableCollector"];
                                break;
                            case 3:
                                lHelp.Text = LangPL.FaqAnswers["dataBaseTableCounter"];
                                break;
                            case 4:
                                lHelp.Text = LangPL.FaqAnswers["dataBaseTableCustomer"];
                                break;
                            default:
                                MessageBox.Show(LangPL.FaqErrors["indexOurOfRange"]);
                                break;
                        }
                    }
            }
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
