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
            ResetHashListBoxes();

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
            // < Disjunctive normal form >
            Proposition disjunctiveProposition = truthTable.CreateDisjunctiveNormalForm();
            disjunctiveFormTbx.Text = disjunctiveProposition.ToString();

            // Simplified truth table.
            simplifiedTruthTable = truthTable.Simplify();
            AddTruthTable(simplifiedTruthTableLbx, simplifiedTruthTable);
            // < Disjunctive normal form >
            Proposition simplifiedDisjunctiveProposition = simplifiedTruthTable.CreateDisjunctiveNormalForm();
            simplifiedDisjunctiveFormTbx.Text = simplifiedDisjunctiveProposition.ToString();

            hashCalc = new HashCodeCalculator(truthTable.GetConvertedResultColumn(), 16);
            hashCodeTbx.Text = hashCalc.HashCode;
            hashesListBox.Items.Add("Proposition:");
            hashCodesListbox.Items.Add($"{hashCalc.HashCode}");
            hashCodeTbx.BackColor = Color.LightGreen;
            
            TruthTable disjunctiveTruthTable = new TruthTable(disjunctiveProposition);
            hashCalc = new HashCodeCalculator(disjunctiveTruthTable.GetConvertedResultColumn(), 16);
            hashesListBox.Items.Add($"Disjunctive normal:");
            hashCodesListbox.Items.Add($"{hashCalc.HashCode}");

            // Create truth table of the simplified disjunctive
            // Create a new hash calculator with the result column, base 16.
            // Add hash and bcde to the list boxes.
            TruthTable simplifiedDisjunctiveTruthTable = new TruthTable(simplifiedDisjunctiveProposition);
            hashCalc = new HashCodeCalculator(simplifiedDisjunctiveTruthTable.GetConvertedResultColumn(), 16);
            hashesListBox.Items.Add($"Simplified Disjunctive normal:");
            hashCodesListbox.Items.Add($"{hashCalc.HashCode}");
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

        private void ResetHashListBoxes()
        {
            hashesListBox.Items.Clear();
            hashesListBox.Items.Add("Hashes:");
            hashesListBox.Items.Add("");

            hashCodesListbox.Items.Clear();
            hashCodesListbox.Items.Add("Codes:");
            hashCodesListbox.Items.Add("");
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
