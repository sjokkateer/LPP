using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace LogicAndSetTheoryApplication
{
    public partial class LogicForm : Form
    {
        private LogicAppBase selectedApp;

        private LogicApp logicApp;
        private SemanticTableauxApp semanticTableauxApp;

        private TruthTable simplifiedTruthTable;
        private List<string> hashList;

        public LogicForm()
        {
            InitializeComponent();
            
            logicApp = new LogicApp(new Parser());
            semanticTableauxApp = new SemanticTableauxApp(new Parser());

            // Hashlist and missing variable can all go to logic app.
            hashList = new List<string>();
        }

        private void AddHashCodeInfo(Proposition proposition, string typeOfProposition, int hashBase, TruthTable tt = null)
        {
            if (tt == null)
            {
                tt = new TruthTable(proposition);
            }

            HashCodeCalculator hashCodeCalculator = new HashCodeCalculator(tt.GetConvertedResultColumn(), hashBase);
            hashList.Add(hashCodeCalculator.HashCode);
            hashesListBox.Items.Add($"{typeOfProposition}: ");
            hashCodesListbox.Items.Add($"{hashCodeCalculator.HashCode}");
        }

        private void parseBtn_Click(object sender, EventArgs e)
        {
            selectedApp = logicApp;

            ResetHashRelatedItems();

            Parse();

            DisplayProposition();

            DisplayUniqueVariables();

            DisplayTruthTableInformation();

            //DisplayDisjunctiveNormalFormInformation();

            DisplaySimplifiedTruthTable();

            // DisplaySimplifiedDisjunctiveNormalFormInformation();

            // DisplayNandifiedSimplifiedDisjunctiveNormalFormInformation();

            // DisplayNandifiedInformation();

            // Test if all hashcodes are equal.
            if (hashCodesMatched(hashList, hashList.Count - 1))
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

        private void Parse()
        {
            string proposition = propositionTbx.Text;
            selectedApp.Parse(proposition);
        }

        private void DisplayProposition()
        {   
            infixTbx.Text = logicApp.Root.ToString();
        }

        private void DisplayNandifiedSimplifiedDisjunctiveNormalFormInformation()
        {
            throw new NotImplementedException();
        }

        //private void DisplaySimplifiedDisjunctiveNormalFormInformation()
        //{
        //    simplifiedDisjunctiveFormTbx.Text = logicApp.SimplifiedDisjunctiveNormalForm.ToString();

        //    if (missingVariableList.Count > 0)
        //    {
        //        AddHashCodeInfo(missingVariableList[0], "Simplified Disjunctive Normal", 16);
        //    }
        //    else
        //    {
        //        // Keeping the original the original.
        //        AddHashCodeInfo(simplifiedDisjunctiveProposition, "Simplified Disjunctive Normal", 16); // 5. ORIGINAL + SIMPLIFY + NORMALIZE + EVALUATE
        //    }

        //    if (missingVariableList.Count > 0)
        //    {
        //        missingVariableList[missingVariableList.Count - 1].RightSuccessor = nandifiedFive;
        //        AddHashCodeInfo(missingVariableList[0], "Simplified Disjunctive Normal NAND", 16);
        //    }
        //    else
        //    {
        //        // Keeping the original the original.
        //        AddHashCodeInfo(nandifiedFive, "Simplified Disjunctive Normal NAND", 16); // 6. ORIGINAL + SIMPLIFY + NORMALIZE + EVALUATE
        //    }
        //}

        private void DisplayNandifiedInformation()
        {
            // Reset the content of their textboxes.
            nandifiedTbx.Text = string.Empty;
        }

        private void DisplaySimplifiedTruthTable()
        {
            simplifiedTruthTable = logicApp.SimplifiedTruthTable;
            AddTruthTable(simplifiedTruthTableLbx, simplifiedTruthTable);
        }

        private void DisplayDisjunctiveNormalFormInformation()
        {
            disjunctiveFormTbx.Text = logicApp.DisjunctiveNormalForm.ToString();
            AddHashCodeInfo(logicApp.DisjunctiveNormalForm, "Disjunctive normal", 16); // 4. ORIGINAL + EVALUATE + NORMALIZE + EVALUATE
        }

        private void DisplayTruthTableInformation()
        {
            TruthTable truthTable = logicApp.TruthTable;
            AddTruthTable(truthTableLbx, truthTable);
            AddHashCodeInfo(logicApp.Root, "Proposition", 16, truthTable); // 1. ORIGINAL EVALUATE.

            // Need to keep this as exclusive for the original proposition.
            hashCodeTbx.Text = hashList[0]; // <---- THIS IS NOT GOOD CODING!
            hashCodeTbx.BackColor = Color.LightGreen;
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

        private bool hashCodesMatched(List<string> hashList, int index)
        {
            if (index == 0)
            {
                return true;
            }
            else
            {
                // Compare the two adjacent hashcodes and make a call downwards.
                return hashList[index] == hashList[index - 1] && hashCodesMatched(hashList, index - 1);
            }
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

        private void ResetHashRelatedItems()
        {
            // The list of hashes used for comparing the hash codes to ensure the program is working correctly.
            hashList.Clear();

            hashesListBox.Items.Clear();
            hashesListBox.Items.Add("Hashes:");
            hashesListBox.Items.Add("");

            hashCodesListbox.Items.Clear();
            hashCodesListbox.Items.Add("Codes:");
            hashCodesListbox.Items.Add("");
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
    }
}
