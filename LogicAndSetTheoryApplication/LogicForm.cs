using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace LogicAndSetTheoryApplication
{
    [ExcludeFromCodeCoverage]
    public partial class LogicForm : Form
    {
        private LogicAppBase selectedApp;

        private LogicApp logicApp;
        private SemanticTableauxApp semanticTableauxApp;

        private PropositionGenerator propositionGenerator;
        private Proposition generatedProposition;

        public LogicForm()
        {
            InitializeComponent();
            
            logicApp = new LogicApp(new Parser());
            semanticTableauxApp = new SemanticTableauxApp(new Parser());
            propositionGenerator = new PropositionGenerator();
        }

        private void parseBtn_Click(object sender, EventArgs e)
        {
            selectedApp = logicApp;

            ResetHashRelatedItems();

            Parse();

            DisplayProposition();
            DisplayUniqueVariables();
            DisplayTruthTableInformation();
            DisplaySimplifiedTruthTable();

            if (logicApp.NonConstantExpressionWasParsed())
            {
                DisplaySimplifiedDnfExpression();
                DisplayDisjunctiveNormalFormInformation();
                DisplayNandifiedInformation();
            }

            DisplayHashCodes();
            DisplayIfAllCodesMatched();
        }

        private void ResetHashRelatedItems()
        {
            hashesListBox.Items.Clear();
            hashesListBox.Items.Add("Hashes:");
            hashesListBox.Items.Add("");

            hashCodesListbox.Items.Clear();
            hashCodesListbox.Items.Add("Codes:");
            hashCodesListbox.Items.Add("");
        }

        private void Parse()
        {
            string proposition = propositionTbx.Text;

            if (proposition != string.Empty)
            {
                generatedProposition = null;
                selectedApp.Parse(proposition);
            }

            if (generatedProposition != null)
            {
                propositionTbx.Text = "";
                selectedApp.Parse(generatedProposition);
            }
        }

        private void DisplayProposition()
        {   
            infixTbx.Text = logicApp.Root.ToString();
        }

        private void DisplayUniqueVariables()
        {
            uniqueVariablesLb.Text = "Unique Variables:";
            List<Proposition> uniqueVariablesSet = logicApp.Variables;
            uniqueVariablesSet.Sort();
            uniqueVariablesTbx.Text = "";

            foreach (Proposition s in uniqueVariablesSet)
            {
                uniqueVariablesTbx.Text += $" {s}";
            }
        }

        private void DisplayTruthTableInformation()
        {
            TruthTable truthTable = logicApp.TruthTable;
            AddTruthTable(truthTableLbx, truthTable);
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

        private void DisplaySimplifiedTruthTable()
        {
            AddTruthTable(simplifiedTruthTableLbx, logicApp.SimplifiedTruthTable);
        }

        private void DisplaySimplifiedDnfExpression()
        {
            simplifiedDisjunctiveFormTbx.Text = logicApp.SimplifiedTruthTable.GetSimplifiedExpression().ToString();
        }

        private void DisplayDisjunctiveNormalFormInformation()
        {
            disjunctiveFormTbx.Text = logicApp.DisjunctiveNormalForm.ToString();
        }

        private void DisplayNandifiedInformation()
        {
            nandifiedTbx.Text = logicApp.Nandified.ToString();
        }

        private void DisplayHashCodes()
        {
            for (int i = 0; i < logicApp.HashCodes.Count; i++)
            {
                if (i == 0)
                {
                    hashCodeTbx.Text = logicApp.HashCodes[i];
                    hashCodeTbx.BackColor = Color.LightGreen;
                }

                hashesListBox.Items.Add(LogicApp.hashCodeLabels[i]);
                hashCodesListbox.Items.Add(logicApp.HashCodes[i]);
            }
        }

        private void DisplayIfAllCodesMatched()
        {
            if (logicApp.HashCodesMatched())
            {
                hashCodeValidationTbx.Text = "Succes! All hashes match.";
                hashCodeValidationTbx.BackColor = Color.LightGreen;
            }
            else
            {
                hashCodeValidationTbx.Text = "Failure! Not all hashes match.";
                hashCodeValidationTbx.BackColor = Color.LightPink;
            }
        }

        private void viewTreeBtn_Click(object sender, EventArgs e)
        {
            selectedApp.CreateGraphImage();
        }

        private void semanticTableauxBtn_Click(object sender, EventArgs e)
        {
            selectedApp = semanticTableauxApp;
            
            Parse();
            
            bool isTautology = semanticTableauxApp.IsTautology();
            infixTbx.Text = semanticTableauxApp.Root.ToString();

            if (isTautology)
            {
                semanticTableauxBtn.BackColor = Color.LightGreen;
                semanticTableauxBtn.ForeColor = Color.Black;
            }
            else
            {
                semanticTableauxBtn.BackColor = Color.Red;
                semanticTableauxBtn.ForeColor = Color.White;
            }
        }

        private void randomPropositionBtn_Click(object sender, EventArgs e)
        {
            generatedProposition = propositionGenerator.GenerateExpression();
            infixTbx.Text = generatedProposition.ToString();
        }
    }
}
