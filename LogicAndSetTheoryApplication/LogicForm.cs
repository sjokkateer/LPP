using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogicAndSetTheoryApplication
{
    public partial class LogicForm : Form
    {
        private Proposition propositionRoot;
        private Parser propositionParser;
        private TruthTable truthTable;
        private TruthTable simplifiedTruthTable;
        private HashCodeCalculator hashCalc;

        public LogicForm()
        {
            InitializeComponent();
        }

        private void parseBtn_Click(object sender, EventArgs e)
        {
            string proposition = propositionTbx.Text;
            propositionParser = new Parser(proposition);
            propositionRoot = propositionParser.Parse();
            infixTbx.Text = propositionRoot.ToString();

            uniqueVariablesLb.Text = "Unique Variables:";
            List<Proposition> uniqueVariablesSet = propositionRoot.GetVariables();
            uniqueVariablesSet.Sort();
            uniqueVariablesTbx.Text = "";
            foreach (Proposition s in uniqueVariablesSet)
            {
                uniqueVariablesTbx.Text += $" {s}";
            }

            truthTable = new TruthTable(propositionRoot);
            AddTruthTable(truthTableLbx, truthTable);
            disjunctiveFormTbx.Text = truthTable.CreateDisjunctiveNormalForm().ToString();

            simplifiedTruthTable = truthTable.Simplify();
            AddTruthTable(simplifiedTruthTableLbx, simplifiedTruthTable);
            // simplifiedDisjunctiveFormTbx.Text = simplifiedTruthTable.CreateDisjunctiveNormalForm().ToString();

            hashCalc = new HashCodeCalculator(truthTable.GetConvertedResultColumn(), 16);
            Console.WriteLine(hashCalc);
            Console.WriteLine();

            hashCodeTbx.Text = hashCalc.HashCode;
            hashCodeTbx.BackColor = Color.LightGreen;
        }

        private void AddTruthTable(ListBox truthTableLbx, TruthTable truthTable)
        {
            truthTableLbx.Items.Clear();
            if (truthTable != null)
            {
                truthTableLbx.Items.Add(truthTable.TableHeader());
                foreach (TruthTableRow row in truthTable.Rows)
                {
                    truthTableLbx.Items.Add(row);
                }
            }
        }

        private void CreateGraphOfExpression(Proposition propositionRoot, string dotFileName)
        {
            Grapher.CreateGraphOfFunction(propositionRoot, dotFileName);
            Process.Start($"{dotFileName}.png");
        }

        private void viewTreeBtn_Click(object sender, EventArgs e)
        {
            if (propositionRoot != null)
            {
                CreateGraphOfExpression(propositionRoot.Copy(), "Proposition");
            }
        }
    }
}
